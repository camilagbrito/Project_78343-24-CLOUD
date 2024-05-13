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
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.3, 5000.0)]
        public decimal Price { get; set; }

        [DisplayName("Imagem do Produto")]
        public string Image { get; set; }

        [DisplayName("Imagem do Produto")]
        public IFormFile ImageUpload { get; set; }
   
        [DisplayName("Disponível?")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Categoria")]
        public Guid CategoryId { get; set; }
      
        [DisplayName("Categoria")]
        public CategoryViewModel Category { get; set; }

        //list to select in the form
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
