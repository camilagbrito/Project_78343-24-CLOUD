namespace App.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<OrderItemViewModel> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
