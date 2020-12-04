using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Model;
using Zhaoxi.BBS.Model.DTO;

namespace Zhaoxi.BBS.Interface
{
	public interface IUserService
	{
		bool Insert(Users users);
		Users GetUser(string n, string p);
		Users GetByID(int id);
	}
}
