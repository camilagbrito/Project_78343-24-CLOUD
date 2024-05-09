using Business.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Título")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Conteúdo")]
        [StringLength(5000, ErrorMessage = "Mínimo 5 e Máximo 5000 caracteres", MinimumLength = 5)]
        public string Content { get; set; }

        [DisplayName("Data de Criação")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public string Image { get; set; }

        public IFormFile ImageUpload { get; set; }
    
        [DisplayName("Utilizador")]
        public ApplicationUserViewModel User { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
