namespace Business.Models
{
    public class Comment : Entity
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}