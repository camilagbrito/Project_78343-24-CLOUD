using Business.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.ViewModels
{
    public class ApplicationUserViewModel:IdentityUser
    {
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Nome")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Apelido")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Data de Nascimento")]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Endereços")]
        public ICollection<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
        public ICollection<OrderViewModel> Orders{ get; set; } = new List<OrderViewModel>();

    }
}
