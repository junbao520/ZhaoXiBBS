using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.BBS.Model
{
	[TableAttribute(tableName: "PostsContent")]
	public class PostsContent : BaseEntity
	{
		public string PostContent { get; set; }

	}
}
