﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Nome")]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "Campo requerido")]
        [DisplayName("Data de Nascimento")]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Endereços")]
        public IEnumerable<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
        public IEnumerable<OrderViewModel> Orders{ get; set; } = new List<OrderViewModel>();

    }
}
