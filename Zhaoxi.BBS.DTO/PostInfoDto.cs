using System;
using System.Collections.Generic;
using System.Linq; 
using Zhaoxi.BBS.DataBase;

namespace Zhaoxi.BBS.DTO
{
	public class PostInfoDto
	{
		public Posts post { get; set; }
		public List<PostReplys> PostReplys { get; set; }
		

	}
}