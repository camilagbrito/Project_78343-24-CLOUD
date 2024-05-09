using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Content)
              .IsRequired()
              .HasColumnType("varchar(5000)");

            builder.Property(p => p.Image)
                .HasColumnType("varchar(500)");

            builder.HasMany(c => c.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);

            builder.ToTable("Posts");

        }
    }
    
}
