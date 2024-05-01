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
        public string UserId { get; set; }
        public ApplicationUserViewModel ApplicationUserViewModel { get; set; }
        public ICollection<OrderItemViewModel> Items { get; set; }
    }
}
