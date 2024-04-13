using Business.Interfaces;
using Business.Models;
using Data.Context;

namespace Data.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(EcomDbContext context) : base(context) {}
        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid CategoryId)
        {
            return await Search(p => p.CategoryId == CategoryId);
        }
    }
}
