using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(EcomDbContext context) : base(context)
        {
          
        }

        public async Task<IEnumerable<Order>> GetOrdersAndUsers()
        {
            return await _context.Orders.AsNoTracking().Include(o => o.User).ThenInclude(u => u.Addresses).OrderByDescending(o => o.Date).ToListAsync();
        }

        public async Task<Order> GetOrderandItems(Guid id)
        {
            return await _context.Orders.AsNoTracking().Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetOrderUser(Guid id)
        {
            return await _context.Orders.AsNoTracking().Include(o => o.User).ThenInclude(u => u.Addresses).FirstOrDefaultAsync(o => o.Id == id);
        }

      
        public async Task<IEnumerable<Order>> GetOrdersByUserId(string id)
        {
            return await _context.Orders.AsNoTracking().Where(o => o.UserId == id).OrderByDescending(o => o.Date).ToListAsync();
        }

        public async Task<Order> GetOrderByCouponId(Guid id)
        {
            return await _context.Orders.AsNoTracking().Where(o => o.CouponId == id).FirstOrDefaultAsync();
        }


    }
}
