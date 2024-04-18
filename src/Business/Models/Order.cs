namespace Business.Models
{
    public class Order: Entity
    {
       public DateTime Date { get; set; }
       public decimal Total { get; set; }
       public Guid UserId {  get; set; }
       public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
