using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Nome")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Descrição")]
        [StringLength(200, ErrorMessage = "Mínimo 5 e Máximo 200 caracteres", MinimumLength = 5)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.3, 5000.0)]
        public decimal Price { get; set; }

        public string Image { get; set; }

        [DisplayName("Imagem do Produto")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public IFormFile ImageUpload { get; set; }
   
        [DisplayName("Disponível?")]
        public bool IsAvailable { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
