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
        public IFormFile ImageUpload { get; set; }
        [DisplayName("Resposta Certa")]
        public string RightAnswer { get; set; }
        public ICollection<CouponViewModel> Coupons { get; set; }
    }
}
