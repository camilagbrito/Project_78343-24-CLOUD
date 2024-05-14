using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Business.Models;


namespace Data.Mappings
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);

            builder.HasMany(u => u.Posts)
                 .WithOne(p => p.User)
                 .HasForeignKey(p => p.UserId);

            builder.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.Coupons)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId);

        }
    }
}