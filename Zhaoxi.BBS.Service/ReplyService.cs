using Zhaoxi.BBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.AglieFramework.Core;

namespace Zhaoxi.BBS.Service
{
	public partial class ReplyService : IReplyService
	{
		CacheClientDB cacheClientDB;
		IDbService dbService;
		public ReplyService(IDbService dbService, CacheClientDB cacheClientDB)
		{
			this.dbService = dbService;
			this.cacheClientDB = cacheClientDB;
		}
		public bool Insert(PostReplys replysInputDto)
		{

			dbService.Insert(replysInputDto);
			return true;


		}
		public bool SetUpDwon(UpDownInputDto upDownInput)
		{

			if (upDownInput.IsUp)
			{
				 
				using (var datalock = cacheClientDB.AcquireLock("DataLock:", TimeSpan.FromSeconds(2)))
				{
					 

					string lua = @"";
					cacheClientDB.RemoveItemFromSet(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Down, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());

					cacheClientDB.AddItemToSet(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Up, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
				}



				return true;
			}

			var isok = cacheClientDB.Add<string>("lockdata3", "lockdata3", TimeSpan.FromSeconds(2));
			if (isok)
			{
				try
				{
					cacheClientDB.RemoveItemFromSet(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Up, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
					cacheClientDB.AddItemToSet(GetSetIDByUpDown(PostRepleyEnum.Repley, UpDownEnum.Down, upDownInput.PostOrReplyId), upDownInput.UserId.ToString());
				}
				catch (Exception)
				{

					throw;
				}
				finally
				{
					cacheClientDB.Remove("lockdata3");
				}


			}

			//cacheClientDB.AddItemToSet(ConfigManger.Project + PostRepleyEnum.Repley.ToString()+":"+upDownInput.PostOrReplyId, upDownInput.UserId.ToString());
			return true;
			//return UpDown(upDownInput.IsUp, upDownInput.UserId, upDownInput.PostOrReplyId);
		}

	}

	public partial class ReplyService
	{
		private string GetSetIDByUpDown(PostRepleyEnum postRepleyEnum, UpDownEnum upDownEnum, int postid)
		{
			return ConfigManger.Project + postRepleyEnum.ToString() + ":" + postid + ":" + upDownEnum;
		}

		public bool UpDown(bool isUp, int userId, int replyId)
		{
			var replys = dbService.Query<PostReplys>().Where(m => m.Id == replyId).FirstOrDefault();
			string res = "";
			if (isUp) res = replys.Up;
			else res = replys.Down;
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

			if (isUp) replys.Up = res;
			else replys.Down = res;
			dbService.Update(replys);
			return true;
		}

	}
}
