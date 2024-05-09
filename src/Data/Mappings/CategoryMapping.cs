using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.ToTable("Categories");
        }
    }
}
