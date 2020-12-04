using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.BBS.Model
{
	[TableAttribute(tableName: "PostReplys")]
	public class PostReplys
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public string ReplyContent { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public DateTime? EditTime { get; set; }
		public int? EditUserId { get; set; }
		public string Up { get; set; }
		public string Down { get; set; }

	}
}
