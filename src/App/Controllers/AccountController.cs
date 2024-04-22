using App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
      

    }
}
