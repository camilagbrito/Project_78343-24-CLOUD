#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using AjaxControlToolkit;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IAddressRepository addressRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

  
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

   
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
        
            [Required (ErrorMessage="Campo requerido")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo requerido")]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{6,}$", ErrorMessage = "A password deve conter letras maiusculas e minúsculas, números e caracteres especiais.")]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a password")]
            [Compare("Password", ErrorMessage = "A password e a confirmação devem ser iguais.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Campo requerido")]
            [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 2)]
            [Display(Name = "Nome")]
            public string FirstName {  get; set; }

            [Required(ErrorMessage = "Campo requerido")]
            [StringLength(50, ErrorMessage = "O campo {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 2)]
            [Display(Name = "Apelido")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Campo requerido")]
            [DataType(DataType.Date)]
            [Display(Name = "Data de Nascimento")]
            public DateTime BirthDate { get; set; }

            [Required(ErrorMessage = "Campo requerido")]
            [DisplayName("Endereço")]
            public AddressViewModel Address { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid)
            {

            
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    BirthDate = Input.BirthDate 
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                 var address = new Address
                {
                    Street = Input.Address.Street,
                    Number = Input.Address.Number,
                    City = Input.Address.City,
                    Region = Input.Address.Region,
                    PostalCode = Input.Address.PostalCode,
                    Country = Input.Address.Country,
                    UserId = user.Id
                };

                await _addressRepository.Add(address);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Por favor confirme seu email <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            return Page();
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
