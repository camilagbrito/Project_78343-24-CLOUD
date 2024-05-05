using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(EcomDbContext context) : base(context)
        {

        }

       
        public async Task<IEnumerable<Address>> GetAddressesByUserId(string id)
        {
            return await _context.Address.AsNoTracking().Where(a => a.UserId == id).ToListAsync();
        }
    }
}
