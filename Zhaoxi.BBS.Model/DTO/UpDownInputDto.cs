using System;

namespace Zhaoxi.BBS.Model
{
    public class UpDownInputDto
    {
        public bool IsUp { get; set; }
        public int UserId { get; set; }
        public int PostOrReplyId { get; set; }
    }
}