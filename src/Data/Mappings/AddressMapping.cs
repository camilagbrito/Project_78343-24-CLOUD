using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Business.Models;

namespace Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasColumnType("varchar(200)");
            builder.Property(a => a.Number)
                .IsRequired()
                .HasColumnType("varchar(20)");
            builder.Property(a => a.City)
                .IsRequired()
                .HasColumnType("varchar(50)");
            builder.Property(a => a.Region)
                .IsRequired()
                .HasColumnType("varchar(50)");
            builder.Property(a => a.PostalCode)
                .IsRequired()
                .HasColumnType("varchar(10)");
            builder.Property(a => a.Country)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");
        }
    }
}