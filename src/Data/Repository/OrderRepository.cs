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

        public async Task<IEnumerable<Order>> GetOrdersOrderItemsUser()
        {
            return await _context.Orders.AsNoTracking().Include(o => o.User).Include(o => o.Items).ToListAsync();
        }

        public async Task<Order> GetOrderandItems(Guid id)
        {
            return await _context.Orders.AsNoTracking().Include(o => o.Items).FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
