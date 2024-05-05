using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderViewModel Order { get; set; }
        public IEnumerable<OrderItemViewModel> Items { get; set; }
        public decimal TotalPrice { get; set; }
        [Range(0, 80)]
        public int TotalQuantity { get; set; }
    }
}
