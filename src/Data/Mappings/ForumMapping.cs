using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class ForumMapping : IEntityTypeConfiguration<Forum>
    {
        public void Configure(EntityTypeBuilder<Forum> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Title)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(f => f.Description)
              .IsRequired()
              .HasColumnType("varchar(5000)");

            builder.Property(f => f.Image)
                .HasColumnType("varchar(500)");

            builder.HasMany(p => p.Posts)
                .WithOne(f => f.Forum)
                .HasForeignKey(f => f.ForumId);

            builder.ToTable("Forums");

        }
    }
    
}
