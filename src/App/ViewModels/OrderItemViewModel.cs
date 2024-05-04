using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.ViewModels
{
    public class OrderItemViewModel
    {

        [Key]
        public Guid Id { get; set; }

        [DisplayName("Produto")]
        public ProductViewModel Product { get; set; }

        [DisplayName("Quantidade")]
        public int Quantity { get; set; }

        [DisplayName("Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.3, 5000.0)]
        public decimal Price { get; set; }

        [DisplayName("Produto")]
        public Guid ProductId { get; set; }

        [DisplayName("Pedido")]
        public Guid OrderId { get; set; }

        public OrderViewModel Order { get; set; }

    }
}
