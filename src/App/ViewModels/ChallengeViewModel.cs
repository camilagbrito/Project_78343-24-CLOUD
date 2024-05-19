using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ChallengeViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Imagem Atual")]
        public string Image { get; set; }

        [DisplayName("Imagem")]
        [Required(ErrorMessage = "Campo Requerido!")]
        public IFormFile ImageUpload { get; set; }

        [DisplayName("Resposta")] 
        public string UserAnswer { get; set; }

        [DisplayName("Percentagem Desconto")]
        [Range(0, 30, ErrorMessage = "Deve ser entre 0 e 30")]
        public int DiscountPercent { get; set; }
 

        [DisplayName("Data para publicar")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Campo Requerido!")]
        public DateTime Date { get; set; }

        [DisplayName("Dica")]
        [Required(ErrorMessage = "Campo Requerido!")]
        public string Tip { get; set; }

        [DisplayName("Resposta Certa")]
        [Required(ErrorMessage = "Campo Requerido!")]
        public string RightAnswer { get; set; }
        public ICollection<CouponViewModel> Coupons { get; set; }
    }
}
