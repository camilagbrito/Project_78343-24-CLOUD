using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class Comment : Entity
    {
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Conteúdo")]
        [StringLength(5000, ErrorMessage = "Mínimo 5 e Máximo 5000 caracteres", MinimumLength = 50)]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}