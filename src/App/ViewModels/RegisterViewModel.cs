using Business.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Informe o email")]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password, ErrorMessage ="Senha deve conter letras, número e caracteres especiais")]
        [MinLength(6, ErrorMessage ="Mínimo 6 caracteres")]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

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
    }
}
