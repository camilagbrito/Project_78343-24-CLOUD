using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderViewModel Order { get; set; }
        public IEnumerable<OrderItemViewModel> Items { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
