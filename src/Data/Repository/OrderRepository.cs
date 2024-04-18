using Business.Interfaces;
using Business.Models;
using Data.Context;


namespace Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(EcomDbContext context) : base(context)
        {
        }
    }
}
