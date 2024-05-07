using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ForumViewModel
    {

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Título")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Descrição")]
        [StringLength(5000, ErrorMessage = "Mínimo 50 e Máximo 5000 caracteres", MinimumLength = 50)]
        public string Description { get; set; }

        [DisplayName("Imagem")]
        public string Image { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
