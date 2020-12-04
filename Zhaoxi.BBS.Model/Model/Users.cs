using System.Linq;
using Dapper.Contrib.Extensions;

namespace Zhaoxi.BBS.Model
{
	[TableAttribute(tableName: "Users")]
	public class Users : BaseEntity
	{
		public string UserName { get; set; }
		public int UserLevel { get; set; }
		public string UserNo { get; set; }
		public string Password { get; set; }

	}
}