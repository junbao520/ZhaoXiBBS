using Dapper.Contrib.Extensions;
using Zhaoxi.BBS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.AglieFramework.Core;
using Dapper;
using ServiceStack.Redis;

namespace Zhaoxi.BBS.Service
{
	public partial class PostsService : IPostsService
	{

		IDbService dbService;
		CacheClientDB cacheClientDB;
		public PostsService(IDbService dbService, CacheClientDB cacheClientDB)
		{
			this.dbService = dbService;
			this.cacheClientDB = cacheClientDB;
		}
		public List<Posts> GetAll()
		{
			var posts = GetPostsInclude(dbService.Query<Posts>().ToList()).ToList();

			return posts;

		}

		public PostInfoDto GetByID(int pid)
		{
			var ss = dbService.GetDbConnection().GetAll<Posts>().Where(m => m.Id == 5);
			PostInfoDto postInfoDto = new PostInfoDto();
			postInfoDto.post = GetPostsInclude(dbService.Query<Posts>().Where(m => m.Id == pid).ToList()).FirstOrDefault();
			postInfoDto.PostReplys = GetPostReplysByPostId(pid);//new  


			// 把当前的点击率缓存下来pid

			// 

			#region 推翻了
			//// 每次做一个操作的,时候,吧当前的key 写到集合里面,.方便我后续刷盘的时候使用
			//// 会记录每次操作的点击量修改的主题,如果知道同步时间的话
			//cacheClientDB.AddItemToSet(ConfigManger.Clicks_AllKeys, ConfigManger.Clicks + pid);

			////这是详情页的 
			//postInfoDto.post.Clicks = (int)cacheClientDB.Increment(ConfigManger.Clicks + pid, 1);
			//// todo  后面考虑要不要把这,,)字段持久化到数据库,当前表中的字段要不要拆出来 
			#endregion
			cacheClientDB.IncrementItemInSortedSet(ConfigManger.Clicks, pid.ToString(), 1);
			postInfoDto.post.Clicks = (int)cacheClientDB.GetItemScoreInSortedSet(ConfigManger.Clicks, pid.ToString());

			return postInfoDto;


		}

		public bool InsertPost(Posts post)
		{
			IDbTransaction tran = null;
			IDbConnection conn = null;
			try
			{
				conn = dbService.GetDbConnection();
				{
					conn.Open();
					tran = conn.BeginTransaction();
					// 插入主表
					conn.Insert(new PostsContent()
					{
						Id = post.Id,
						PostContent = post.PostContent
					}, tran);
					// 既然这个值都是null ,新增和修改的时候,可以把这个字段忽略掉吧

					// 多些几个实体  分布式,加全链路,sql  ---
					post.PostContent = null;
					post.CreateUser = null;
					post.EditUser = null;
					post.LastReplyUser = null;
					// 插入分表 
					conn.Insert(post, tran);
					tran.Commit();
				}
			}
			catch (Exception ex)
			{
				tran.Rollback();
				conn.Close();
				throw;
			}

			return true;
		}
		public bool SetUpDwon(UpDownInputDto upDownInput)
		{
			// 需要缓存  去重 互斥
			//如果是赞,则删除踩
			// 如果是踩,则删除赞
			//比如踩,则判断有么有赞,如果有,则删出,没有啥都不敢,然后直接写入踩



			if (upDownInput.IsUp)
			{
				//事务 集群的话事务百搭
				// 锁 
				/// 阻塞锁,能不用则不用
				/// //实现原子性, redis ,一个lua 脚本对于redis服务而已就是调了一个方法,想当于打包然后保证原子性执行
				using (var datalock = cacheClientDB.AcquireLock("DataLock:", TimeSpan.FromSeconds(2)))
				{
					// lua  
					// 大道至简
					// 持久化-- 定时作业 一个key是一个帖子,到时候定时刷盘,不能全盘扫描redis的key
					// 每次不管是点赞还是点踩,都维护一个list,专门存在我们点赞和点踩的key,list有分页

					// 能做,,clay 
					// 想想,换------ 

					string lua = @"";
					cacheClientDB.RemoveItemFromSet(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Down, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());

					//cacheClientDB.GetClient().Custom("SMOVE", GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Down, upDownInput.PostOrReplyId), GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Up, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());

					cacheClientDB.AddItemToSet(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Up, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
				}



				return true;
			}

			var isok = cacheClientDB.Add<string>("lockdata2", "lockdata2", TimeSpan.FromSeconds(2));
			// 非阻塞锁
			if (isok)
			{
				try
				{
					cacheClientDB.RemoveItemFromSet(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Up, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
					// 首先往redis里面写入这个key
					cacheClientDB.AddItemToSet(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Down, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
				}
				catch (Exception)
				{

					throw;
				}
				finally
				{
					cacheClientDB.Remove("lockdata2");
				}


			}


			//cacheClientDB.GetClient().Custom("SMOVE", GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Up, upDownInput.PostOrReplyId), GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Down, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
			return true;

			//return UpDown(upDownInput.IsUp, upDownInput.UserId, upDownInput.PostOrReplyId);
		}

	}

	public partial class PostsService
	{
		private string GetSetIDByUpDown(PostRepleyEnum postRepleyEnum, UpDownEnum upDownEnum, int postid)
		{
			return ConfigManger.Project + postRepleyEnum.ToString() + ":" + postid + ":" + upDownEnum;
		}

		private IEnumerable<Posts> GetPostsInclude(IEnumerable<Posts> posts)
		{
			// 贪婪加载  一次获取,不是每一行去读一次数据库,然后关联
			// 获取多行数据的userid 去重,一次获取 /
			var users = posts.Select(m => m.CreateUserId.ToString()).Union(posts.Select(m => m.EditUserId.ToString())).Union(posts.Select(m => m.LastReplyUserId.ToString())).Distinct().ToList();
			//一次获取
			var userinfos = dbService.Query<Users>().Where(m => users.Contains(m.Id.ToString()));

			// 一次性获取所有的帖子id,然后统一从redis 获取是赞还是踩


			// 绑定用户
			//var ids = posts.Select(m => ConfigManger.Clicks + m.Id).ToList();
			// 泛型处理 redis本身返回的是空,组件解决

			// 没有办法,可以一次从zset 集合中获取的多个key的分数
			//  简单,一次请求一次响应
			//  事务 打包发送  --不要监听,但是能打包发送,相当一次请求  一次把结果搞过来
			// 管道  ,批量发送,性能比简单好一些,打包发送,多个请求,多个结果


			// 没有并发,各自请求,各自方法资源
			Dictionary<string, double> socores = new Dictionary<string, double>();
			Dictionary<string, long> ups = new Dictionary<string, long>();
			Dictionary<string, long> downs = new Dictionary<string, long>();
			using (var client = cacheClientDB.GetClient().CreatePipeline())
			{
				foreach (var item in posts.Select(m => m.Id))
				{
					// 一个一个找的,根据value  使用hash 存储的
					client.QueueCommand(p => p.GetItemScoreInSortedSet(ConfigManger.Clicks, item.ToString()), score =>
					{
						socores.Add(item.ToString(), score);
					});

					client.QueueCommand(p => p.GetSetCount(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Up, item)), result =>
						{
							ups.Add(item.ToString(), result);

						});

					client.QueueCommand(p => p.GetSetCount(GetSetIDByUpDown(PostRepleyEnum.Content, UpDownEnum.Down, item)), result =>
					{
						downs.Add(item.ToString(), result);

					});
				}
				// 批量发送
				client.Flush();

				// 这边不管怎么样,解决了

				//刷盘的时候,分页---跳跃表  ----  

				//没有一个方法,一次性读取zset,然后获取我们想要的多个zset 中的value的分数// 批量操作
			}
			//完全可以利用排名来分页
			// 0,100
			// 100 200
			// 200 300 
			// 排名会变,,可能拿到重复数据  
			cacheClientDB.GetRangeFromSortedSet("setid", 0, 2);
			// scan 扫描  
			foreach (var item in posts)
			{
				// 刚好利用他的循环
				item.CreateUser = userinfos.Where(m => m.Id == item.CreateUserId).FirstOrDefault();
				item.LastReplyUser = userinfos.Where(m => m.Id == item.LastReplyUserId).FirstOrDefault();
				item.EditUser = userinfos.Where(m => m.Id == item.EditUserId).FirstOrDefault();
				item.PostContent = dbService.Query<PostsContent>().Where(m => m.Id == item.Id).FirstOrDefault()?.PostContent;

				item.Clicks = 0;
				if (socores.ContainsKey(item.Id.ToString()))
				{
					item.Clicks = (int)socores[item.Id.ToString()];
				}


				// 每一个请求一次 免为奇难  通过多个key获取多个key的分数

				// 所有的点和踩需要读缓存

				item.Up = ups[item.Id.ToString()].ToString();
				item.Down = downs[item.Id.ToString()].ToString();
			}
			return posts.ToList();
		}

		private bool UpDown(bool isUp, int userId, int postId)
		{

			var post = dbService.Query<Posts>().Where(m => m.Id == postId).FirstOrDefault();
			string res = "";
			if (isUp) res = post.Up;
			else res = post.Down;
			if (string.IsNullOrEmpty(res))
			{
				res = userId + "";
			}
			else if (res.Split(",").Any(m => m == userId + ""))
			{
				var listRes = res.Split(",").ToList();
				listRes.Remove(userId + "");
				res = string.Join(",", listRes.ToArray());
			}
			else
			{
				res = res + "," + userId;
			}

			if (isUp) post.Up = res;
			else post.Down = res;



			//dapper 修改可以忽略掉为空的字段;
			post.PostContent = null;
			post.CreateUser = null;
			post.EditUser = null;
			post.LastReplyUser = null;
			dbService.Update(post);


			return true;
		}

		private List<PostReplysOutDto> GetPostReplysByPostId(int postId)
		{

			List<PostReplysOutDto> postReplysDto = new List<PostReplysOutDto>();
			var replyslist = dbService.Query<PostReplys>().Where(m => m.PostId == postId).ToList();
			Dictionary<string, long> ups = new Dictionary<string, long>();
			Dictionary<string, long> downs = new Dictionary<string, long>();
			using (var client = cacheClientDB.GetClient().CreatePipeline())
			{
				foreach (var item in replyslist.Select(m => m.Id))
				{

					client.QueueCommand(p => p.GetSetCount(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Up, item)), result =>
					{
						ups.Add(item.ToString(), result);

					});

					client.QueueCommand(p => p.GetSetCount(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Down, item)), result =>
					{
						downs.Add(item.ToString(), result);

					});
				}
				// 批量发送
				client.Flush();

				// 这边不管怎么样,解决了

				//刷盘的时候,分页---跳跃表  ----  

				//没有一个方法,一次性读取zset,然后获取我们想要的多个zset 中的value的分数// 批量操作
			}

			foreach (var item in replyslist)
			{
				postReplysDto.Add(new PostReplysOutDto()
				{
					CreateTime = item.CreateTime,
					CreateUserId = item.CreateUserId,
					Down = downs[item.Id.ToString()].ToString(),
					EditTime = item.EditTime,
					EditUserId = item.EditUserId,
					Id = item.Id,
					PostId = item.PostId,
					ReplyContent = item.ReplyContent,
					Up = ups[item.Id.ToString()].ToString()

				});
			}
			var replys = GetPostReplysInclude(postReplysDto).ToList();

			replys.ForEach(m => m.ReplyContent = m.ReplyContent?.Replace("[quote]", "<span style='border:1px solid #e5e5e5;background-color: #f5f5f5; font-size: 13px;  font-style: italic ;padding:5px;display: block;'>")?.Replace("[/quote]", "</span>"));
			return replys;
		}

		private IEnumerable<PostReplysOutDto> GetPostReplysInclude(IEnumerable<PostReplysOutDto> postReplys)
		{
			var users = postReplys.Select(m => m.CreateUserId.ToString()).Union(postReplys.Select(m => m.EditUserId.ToString())).Distinct().ToList();
			var userinfos = dbService.Query<Users>().Where(m => users.Contains(m.Id.ToString()));
			foreach (var item in postReplys)
			{
				item.CreateUser = userinfos.Where(m => m.Id == item.CreateUserId).FirstOrDefault();
				item.EditUser = userinfos.Where(m => m.Id == item.EditUserId).FirstOrDefault();
			}
			return postReplys.ToList();
		}
	}
}
