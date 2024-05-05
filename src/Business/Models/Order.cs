using Business.Models.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class Order: Entity
    {
       public DateTime Date { get; set; }
       public decimal Total { get; set; }
       public string UserId { get; set; }
       public OrderStatus Status {  get; set; }
       public ApplicationUser User { get; set; }
       public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
