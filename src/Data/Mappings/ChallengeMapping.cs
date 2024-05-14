using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class ChallengeMapping : IEntityTypeConfiguration<Challenge>
    {
        public void Configure(EntityTypeBuilder<Challenge> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.RightAnswer)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Image)
                .HasColumnType("varchar(500)");

            builder.ToTable("Challenges");

        }
    }
    
}
