using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class CommentViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Comentário")]
        [StringLength(5000, ErrorMessage = "Mínimo 5 e Máximo 5000 caracteres", MinimumLength = 5)]
        public string Message { get; set; }

        [DisplayName("Data de Criação")]
        public DateTime CreatedDate { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        [DisplayName("Utilizador")]
        public string UserId { get; set; }
        [DisplayName("Utilizador")]
        public ApplicationUserViewModel User { get; set; }
    }
}
