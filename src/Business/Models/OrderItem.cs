namespace Business.Models
{
    public class OrderItem:Entity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId {  get; set; }
        public Guid OrderId {  get; set; }
        public Order Order { get; set; }
        
    }
}
