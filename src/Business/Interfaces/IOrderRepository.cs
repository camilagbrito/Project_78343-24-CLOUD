using Business.Models;

namespace Business.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAndUsers();
        Task<Order> GetOrderandItems(Guid id);
        Task<Order> GetOrderUser(Guid id);
        Task<IEnumerable<Order>> GetOrdersByUserId(string id);
        Task<Order> GetOrderByCouponId(Guid id);
    }
}
