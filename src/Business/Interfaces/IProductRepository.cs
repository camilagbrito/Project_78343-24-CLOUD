﻿using Business.Models;

namespace Business.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategory(Guid CategoryId);

    }
}
