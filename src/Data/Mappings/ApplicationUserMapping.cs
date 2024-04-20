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

            builder.Property(u => u.Image)
                .HasColumnType("varchar(100)");

            builder.HasOne(u => u.Address)
            .WithOne(a => a.User);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);
        }
    }
}