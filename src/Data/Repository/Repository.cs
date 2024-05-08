using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repository
{
    //cannot be instantiated
    // where TEntity is Entity's child
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {

        //context
        protected readonly EcomDbContext _context;

        //shortcut to dbset Db.Set<entity>...
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(EcomDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            //AsNoTracking - just read - performance
            //return a list with the result of the predicate
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetbyId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task Add(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            //created a new instance using ID to not search directly
            //from the database
            var entityDelete = await _dbSet.FindAsync(id);

            _dbSet.Remove(entityDelete);
            await SaveChanges();

        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
           _context?.Dispose();
        }
    }
}
