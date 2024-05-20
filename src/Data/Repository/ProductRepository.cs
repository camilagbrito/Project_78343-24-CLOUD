using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(EcomDbContext context) : base(context) {}

        public async Task<IEnumerable<Product>> GetProductsandCategory()
        {
            return await _context.Products.AsNoTracking().Include(p => p.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(Guid CategoryId)
        {
            return await Search(p => p.CategoryId == CategoryId);
        }

        public async Task<Product> GetProductandCategoryById(Guid Id)
        {
            return await _context.Products.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string ProductName)
        {
            return await _context.Products.AsNoTracking().Include(p => p.Category).Where(p => p.Name.Contains(ProductName)).ToListAsync();
        }
    }
}
