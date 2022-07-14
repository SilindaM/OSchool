using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Data;
using School.Models;
using System.Threading.Tasks;

namespace School.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserAuthController(ApplicationDbContext context,UserManager<IdentityUser> userManager,SignInManager <IdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(LoginModel loginModel)
        {
            loginModel.LoginInValid = "true";

            //if all the values given are correct
            if (ModelState.IsValid)
            {
                //
                var result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

                //if the login is successful
                if (result.Succeeded)
                {
                    loginModel.LoginInValid = "";
                }
                //if user not authenticated
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            //return if there is error in the input to login
            return PartialView("_UserLoginPartial", loginModel);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            if(returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
