using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.BBS.DataBase.DTO
{

	[TableAttribute(tableName: "Posts")]
	public class PostsDto : BaseEntity
	{
		public string PostTitle { get; set; }
		public string PostIcon { get; set; }
		public int PostTypeId { get; set; }
		public string PostType { get; set; }
		public string PostContent { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public DateTime EditTime { get; set; }
		public int EditUserId { get; set; }
		public DateTime LastReplyTime { get; set; }
		public int LastReplyUserId { get; set; }
		public int Clicks { get; set; }
		public int Replys { get; set; }
		public string Up { get; set; }
		public string Down { get; set; }
	}
}
