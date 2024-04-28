using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.ViewModels
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Rua")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Número")]
        [StringLength(5, ErrorMessage = "Máximo 5 caracteres")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [DisplayName("Concelho")]
        public string City { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [DisplayName("Distrito")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(8, ErrorMessage = "Máximo 8 caracteres")]
        [DisplayName("Código Postal")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [DisplayName("País")]
        public string Country { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public string UserId { get; set; }
    }
}
