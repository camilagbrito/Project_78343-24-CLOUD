using Business.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Date)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.HasMany(o=> o.Items)
                   .WithOne(i => i.Order);

            builder.HasOne(o => o.Coupon)
              .WithOne(c => c.AssociatedOrder)
              .HasForeignKey<Order>(o => o.CouponId)
              .IsRequired(false);

            builder.ToTable("Orders");
        }
    }
}
