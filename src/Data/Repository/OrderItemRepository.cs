using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<OrderItem> GetOrderItemProduct(Guid id)
        {
            return await _context.OrderItems.AsNoTracking().Include(p => p.Product).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
