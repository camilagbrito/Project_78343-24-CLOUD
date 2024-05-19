using Business.Models;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderViewModel Order { get; set; }
        public IEnumerable<OrderItemViewModel> Items { get; set; }
        public CouponViewModel Coupon { get; set; }
        public decimal DiscountPercent {  get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
