using Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class EcomDbContext : IdentityDbContext<IdentityUser>
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        { 

        
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
        public DbSet<Address> Address { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcomDbContext).Assembly);

            //disable delete cascade
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull; 
            
            base.OnModelCreating(modelBuilder);
        }

    }
}
