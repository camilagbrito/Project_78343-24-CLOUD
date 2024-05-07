namespace Business.Models
{
    public class Post:Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Image {  get; set; }
        public Forum Forum { get; set; }
        public Guid ForumId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}