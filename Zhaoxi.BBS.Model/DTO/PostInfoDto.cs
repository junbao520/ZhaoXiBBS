using System;
using System.Collections.Generic;
using System.Linq;
using Zhaoxi.BBS.Model; 

namespace Zhaoxi.BBS.Model
{
	public class PostInfoDto
	{
		public Posts post { get; set; }
		public List<PostReplysOutDto> PostReplys { get; set; }
		

	}
}