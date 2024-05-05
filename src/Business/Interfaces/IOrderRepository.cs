using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAndUsers();

        Task<Order> GetOrderandItems(Guid id);
        Task<Order> GetOrderUser(Guid id);
    }
}
