using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model;
using Zhaoxi.BBS.Model.DTO;

namespace Zhaoxi.BBS.Service
{
	public class UserService : IUserService
	{
		IDbService dbService;
		public UserService(IDbService dbService)
		{
			this.dbService = dbService;
		}
		public bool Insert(Users users)
		{

			dbService.Insert(users);
			return true;

		}
		public Users GetUser(string n, string p)
		{
			var userInfo = dbService.Query<Users>().Where(m => m.UserName == n && m.Password == p).FirstOrDefault();
			return userInfo;
		}
		public Users GetByID(int id)
		{
			var userInfo = dbService.Query<Users>().Where(m => m.Id == id).FirstOrDefault();
			return userInfo;
		}
	}
}
