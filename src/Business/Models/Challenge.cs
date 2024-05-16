namespace Business.Models
{
    public class Challenge: Entity
    {
        public string Image { get; set; }
        public string RightAnswer { get; set; } 
        public string Tip {  get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}
