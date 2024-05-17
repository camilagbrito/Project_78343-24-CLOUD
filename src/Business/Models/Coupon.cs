namespace Business.Models
{
    public class Coupon:Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Discount { get; set; }
        public bool Expired { get; set; } = false;
        public bool Used { get; set; } = false;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
        public Guid AssociatedOrderId { get; set; }
        public Order AssociatedOrder { get; set; }

    }
}