using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using COHApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BataCMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel {
                ReturnUrl = returnUrl
            });
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByPhoneNumber(loginViewModel.Number);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var parsedString = loginViewModel.ReturnUrl.Split('/');
                        string controller = parsedString[1];
                        string action = parsedString[2];
                        return RedirectToAction(action, controller);
                    }
                }
            }
            ModelState.AddModelError("", "Username/Password was not found");
            return View(loginViewModel);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            IDictionary<string, object> value = new Dictionary<string, object>();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = registerViewModel.LastName+registerViewModel.IdNumber, FirstName = registerViewModel.FirstName, LastName = registerViewModel.LastName, PhoneNumber = registerViewModel.Number, IDNumber = registerViewModel.IdNumber };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers","Admin"); 
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Account", "Login");
        }

    }
}