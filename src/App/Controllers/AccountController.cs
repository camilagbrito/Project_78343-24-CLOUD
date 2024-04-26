using App.ViewModels;
using Business.Models;
using Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Azure.Messaging;


namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
      

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginvm);
            }

            var user = await _userManager.FindByNameAsync(loginvm.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginvm.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginvm.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginvm.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Falha no login!");
            return View(loginvm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel regViewModel)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = regViewModel.UserName, 
                    FirstName = regViewModel.FirstName, LastName = regViewModel.LastName,
                BirthDate = regViewModel.BirthDate};

                var result = await _userManager.CreateAsync(user, regViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("Registo", "Erro ao registar, tente novamente mais tarde.");
                }
            }
            return View(regViewModel);
        }

    }
}
