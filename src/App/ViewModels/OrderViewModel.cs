using Business.Models;
using Business.Models.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class OrderViewModel:Entity
    {

        [DisplayName("Data do Pedido")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Total do Pedido")]
        public decimal Total { get; set; }
        [DisplayName("Estado do Pedido")]
        public OrderStatus Status { get; set; }
        [DisplayName("Cliente")]
        public string UserId { get; set; }
        [DisplayName("Cliente")]
        public ApplicationUserViewModel ApplicationUserViewModel { get; set; }
        public IEnumerable<OrderItemViewModel> Items { get; set; }
        public IEnumerable<OrderStatus> ListStatus { get; set; }
    }
}
