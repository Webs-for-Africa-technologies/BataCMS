using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
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

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null )
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded )
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
                        return RedirectToAction( action, controller);
                    }
                }
            }
            ModelState.AddModelError("", "Username/Password was not found");
            return View(loginViewModel);
        }

        //implement the AccessDenied that redirect to the ReturnUrl and displays Access denied error. Comment#0001 
        public ActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            IDictionary<string, object> value = new Dictionary<string, object>();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = loginViewModel.UserName };

                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

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
            return View(loginViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}