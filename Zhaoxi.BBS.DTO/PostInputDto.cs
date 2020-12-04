namespace MyBBSDemo.Models
{
    public class PostInputDto
    {
        public int UserId { get; set; }
        public int PostTypeId { get; set; }
        public string PostType { get; set; }
        public string PostTitle { get; set; }
        public string Content { get; set; }
    }
}