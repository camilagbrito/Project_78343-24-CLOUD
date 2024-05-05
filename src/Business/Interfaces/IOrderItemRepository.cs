
using Business.Models;

namespace Business.Interfaces
{
    public interface IOrderItemRepository: IRepository<OrderItem>
    {
        Task<OrderItem> GetOrderItemProduct(Guid id);

        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderId(Guid id);
    }
}
