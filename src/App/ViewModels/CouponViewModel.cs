using Business.Models;

namespace App.ViewModels
{
    public class CouponViewModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Discount { get; set; }
        public string UserId { get; set; }
        public Guid ChallengeId { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public ChallengeViewModel Challenge { get; set; }
    }
}
