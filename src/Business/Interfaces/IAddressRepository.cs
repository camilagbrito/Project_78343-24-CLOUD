using Business.Models;

namespace Business.Interfaces
{
    public interface IAddressRepository:IRepository<Address>
    {
        Task<IEnumerable<Address>> GetAddressesByUserId(string id);
    }
}
