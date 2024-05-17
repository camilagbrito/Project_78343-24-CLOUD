using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class CouponViewModel
    {
        [Key]
        [DisplayName("Código do Cupão")]
        public Guid Id { get; set; }
        [DisplayName("Data de Criação")]
        [DataType(DataType.Date)]

        public DateTime CreatedDate { get; set; }
        [DisplayName("Data de Expiração")]
        [DataType(DataType.Date)]

        public DateTime ExpirationDate { get; set; }
        [DisplayName("Percentagem Desconto")]
        public int Discount { get; set; }
        [DisplayName("Expirado")]
        public bool Expired { get; set; } = false;
        [DisplayName("Usado")]
        public bool Used { get; set; } = false;
        public string UserId { get; set; }
        public Guid ChallengeId { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public ChallengeViewModel Challenge { get; set; }
        [DisplayName("Pedido Associado")]
        public Guid AssociatedOrderId { get; set; }
        public OrderViewModel AssociatedOrder { get; set; }
    }
}
