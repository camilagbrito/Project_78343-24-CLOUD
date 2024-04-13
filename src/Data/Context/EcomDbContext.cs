using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class EcomDbContext : DbContext
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        { 

        
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcomDbContext).Assembly);

            //disable delete cascade
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull; 
            
            base.OnModelCreating(modelBuilder);
        }

    }
}
