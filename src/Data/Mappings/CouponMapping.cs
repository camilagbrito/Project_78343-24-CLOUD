using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class CouponMapping : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(c => c.AssociatedOrder)
               .WithOne(o => o.Coupon)
               .HasForeignKey<Coupon>(c => c.AssociatedOrderId);
               

            builder.ToTable("Coupons");

        }
    }
    
}
