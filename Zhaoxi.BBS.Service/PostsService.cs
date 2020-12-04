using Dapper.Contrib.Extensions;
using Zhaoxi.BBS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.AglieFramework.Core;

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
			PostInfoDto postInfoDto = new PostInfoDto();
			postInfoDto.post = GetPostsInclude(dbService.Query<Posts>().Where(m => m.Id == pid)).FirstOrDefault();
			postInfoDto.PostReplys = GetPostReplysByPostId(pid);//new  
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
					conn.Insert(new PostsContent()
					{
						Id = post.Id,
						PostContent = post.PostContent
					}, tran);

					post.PostContent = null;
					post.CreateUser = null;
					post.EditUser = null;
					post.LastReplyUser = null;

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
			var users = posts.Select(m => m.CreateUserId.ToString()).Union(posts.Select(m => m.EditUserId.ToString())).Union(posts.Select(m => m.LastReplyUserId.ToString())).Distinct().ToList();

			var userinfos = dbService.Query<Users>().Where(m => users.Contains(m.Id.ToString()));

			foreach (var item in posts)
			{
				item.CreateUser = userinfos.Where(m => m.Id == item.CreateUserId).FirstOrDefault();
				item.LastReplyUser = userinfos.Where(m => m.Id == item.LastReplyUserId).FirstOrDefault();
				item.EditUser = userinfos.Where(m => m.Id == item.EditUserId).FirstOrDefault();
				item.PostContent = dbService.Query<PostsContent>().Where(m => m.Id == item.Id).FirstOrDefault()?.PostContent;
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

			// 存redis
			cacheClientDB.AddItemToSortedSet(PostEnum.Content.ToString() + ":" + postId, userId.ToString(), isUp == true ? ((double)UpEnum.Up) : ((double)UpEnum.Down)) ;

			//using (var conn = this.cacheClientDB.AcquireLock("uplock", TimeSpan.FromSeconds(100)))
			//{

			//}
			//bool isLocked = client.Add<string>("DataLock:" + key, key, timeout);

			//var lua = @"local count = redis.call('get',KEYS[1])
			//                        if(tonumber(count)>=0)
			//                        then
			//                            redis.call('INCR',ARGV[1])
			//                            return redis.call('DECR',KEYS[1])
			//                        else
			//                            return -1
			//                        end";
			//Console.WriteLine(client.ExecLuaAsString(lua, keys: new[] { "number" }, args: new[] { "ordercount" }));
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
