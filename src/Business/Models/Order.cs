using Business.Models.Enum;

namespace Business.Models
{
    public class Order : Entity
    {
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public int DiscountPercent { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public ApplicationUser User { get; set; }
        public Guid? CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
