using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.BBS.DataBase.DTO
{
	public class UsersDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserLevel { get; set; }
        public string UserNo { get; set; }

        public string Token { get; set; }
    }
}
