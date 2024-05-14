namespace Business.Models
{
    public class Coupon:Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Discount { get; set; }
        public string UserId { get; set; }
        public Guid ChallengeId { get; set; }
        public ApplicationUser User { get; set; }
        public Challenge Challenge { get; set; }
    }
}