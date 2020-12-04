using Dapper.Contrib.Extensions;
using System;

namespace Zhaoxi.BBS.Model
{
    [TableAttribute(tableName: "PostTypes")]
    public class PostTypes
    {
        public int Id { get; set; }
        public string PostType { get; set; }    
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
    }
}