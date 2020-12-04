using System;

namespace MyBBSDemo.Models
{
    public class UpDownInputDto
    {
        public bool IsUp { get; set; }
        public int UserId { get; set; }
        public int PostOrReplyId { get; set; }
    }
}