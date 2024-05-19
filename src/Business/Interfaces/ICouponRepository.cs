using Business.Models;

namespace Business.Interfaces
{
    public interface ICouponRepository:IRepository<Coupon>
    {
        Task<IEnumerable<Coupon>> GetCouponsByUserId(string id);
        Task<IEnumerable<Coupon>> GetCouponsAndUsers();
    }
}
