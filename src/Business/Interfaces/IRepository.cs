using Business.Models;
using System.Linq.Expressions;


namespace Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
        Task<TEntity> GetbyId(Guid id);
        Task<List<TEntity>> GetAll();

        //Search with and expression
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);

        //Abstract save changes, return number lines affected
        Task<int> SaveChanges();
    }
}
