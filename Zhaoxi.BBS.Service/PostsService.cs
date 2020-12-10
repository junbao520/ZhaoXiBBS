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
			return UpDown(upDownInput.IsUp, upDownInput.UserId, upDownInput.PostOrReplyId);
		}

	}

	public partial class PostsService
	{
		private IEnumerable<Posts> GetPostsInclude(IEnumerable<Posts> posts)
		{
			// 贪婪加载  一次获取,不是每一行去读一次数据库,然后关联
			// 获取多行数据的userid 去重,一次获取 /
			var users = posts.Select(m => m.CreateUserId.ToString()).Union(posts.Select(m => m.EditUserId.ToString())).Union(posts.Select(m => m.LastReplyUserId.ToString())).Distinct().ToList();
			//一次获取
			var userinfos = dbService.Query<Users>().Where(m => users.Contains(m.Id.ToString()));

			// 绑定用户
			//var ids = posts.Select(m => ConfigManger.Clicks + m.Id).ToList();
			// 泛型处理 redis本身返回的是空,组件解决

			// 没有办法,可以一次从zset 集合中获取的多个key的分数
			//  简单,一次请求一次响应
			//  事务 打包发送  --不要监听,但是能打包发送,相当一次请求  一次把结果搞过来
			// 管道  ,批量发送,性能比简单好一些,打包发送,多个请求,多个结果


			// 没有并发,各自请求,各自方法资源
			Dictionary<string, double> socores = new Dictionary<string, double>();
			using (var client = cacheClientDB.GetClient().CreatePipeline())
			{
				foreach (var item in posts.Select(m => m.Id))
				{
					// 一个一个找的,根据value  使用hash 存储的
					client.QueueCommand(p => p.GetItemScoreInSortedSet(ConfigManger.Clicks, item.ToString()), score =>
					{
						socores.Add(item.ToString(), score);
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
			foreach (var item in dbService.Query<PostReplys>().Where(m => m.PostId == postId))
			{
				postReplysDto.Add(new PostReplysOutDto()
				{
					CreateTime = item.CreateTime,
					CreateUserId = item.CreateUserId,
					Down = item.Down,
					EditTime = item.EditTime,
					EditUserId = item.EditUserId,
					Id = item.Id,
					PostId = item.PostId,
					ReplyContent = item.ReplyContent,
					Up = item.Up

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
