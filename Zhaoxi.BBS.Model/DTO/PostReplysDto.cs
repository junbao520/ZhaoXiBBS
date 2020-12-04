using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Zhaoxi.BBS.Model.DTO;

namespace Zhaoxi.BBS.Model
{
	
	 
	public class PostReplysOutDto
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public string ReplyContent { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public Users CreateUser { get; set; }
		public DateTime? EditTime { get; set; }
		public int? EditUserId { get; set; }
		public Users EditUser { get; set; }
		public string Up { get; set; }
		public string Down { get; set; }

	}
	 

}