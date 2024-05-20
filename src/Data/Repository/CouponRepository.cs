using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Coupon>> GetCouponsByUserId(string id)
        {
            return await _context.Coupons.AsNoTracking().Where(c => c.UserId == id).OrderByDescending(c => c.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetCouponsAndUsers()
        {
            return await _context.Coupons.AsNoTracking().Include(c => c.User).OrderByDescending(c => c.CreatedDate).ToListAsync();
        }
    }
}
