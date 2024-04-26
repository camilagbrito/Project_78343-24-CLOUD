using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Business.Models;


namespace Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Image)
                .HasColumnType("varchar(500)");

            builder.HasMany(p => p.OrderItems)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId);

            builder.ToTable("Products");
        }
    }
}