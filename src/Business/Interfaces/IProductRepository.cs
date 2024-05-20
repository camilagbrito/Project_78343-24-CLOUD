using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(Guid CategoryId);

        Task<IEnumerable<Product>> GetProductsandCategory();

        Task<Product> GetProductandCategoryById(Guid Id);

        Task<IEnumerable<Product>> GetProductsByName(string ProductName);
       
    }
}
