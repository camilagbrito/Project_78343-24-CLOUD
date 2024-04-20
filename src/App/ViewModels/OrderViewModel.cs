using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class OrderViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Data do Pedido")]
        public DateTime Date { get; set; }

        [DisplayName("Total do Pedido")]
        public decimal Total { get; set; }
        [DisplayName("Cliente")]
        public Guid UserId { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
