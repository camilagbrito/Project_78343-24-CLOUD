using Business.Interfaces;
using Business.Models;
using Data.Context;

namespace Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EcomDbContext context) : base(context)
        {
        }
    }
}
