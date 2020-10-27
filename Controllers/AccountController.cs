using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using COHApp.Data;
using COHApp.Data.Models.Configuration;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio.Rest.Verify.V2.Service;

namespace BataCMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TwilioVerifySettings _settings;


        public AccountController(IOptions<TwilioVerifySettings> settings, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settings = settings.Value;

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


        public ActionResult ConfirmPhone( string phoneNumber)
        {
            ConfirmPhoneViewModel vm = new ConfirmPhoneViewModel
            {
                PhoneNumber = phoneNumber
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPhone(ConfirmPhoneViewModel confirmPhoneViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var verification = await VerificationCheckResource.CreateAsync(
                        to: confirmPhoneViewModel.PhoneNumber,
                        code: confirmPhoneViewModel.VerificationCode,
                        pathServiceSid: _settings.VerificationServiceSID
                    );

                    if (verification.Status == "approved")
                    {
                        var identityUser = await _userManager.GetUserAsync(User);
                        identityUser.PhoneNumberConfirmed = true;
                        var updateResult = await _userManager.UpdateAsync(identityUser);

                        if (updateResult.Succeeded)
                        {
                            return RedirectToAction("ConfirmPhoneSuccess");
                        }
                        else
                        {
                            ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("",
                        "There was an error confirming the code, please check the verification code is correct and try again");
                }

            }
            return View(confirmPhoneViewModel);
        }

        public ActionResult ConfirmPhoneSuccess()
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

                var existingNumber = _userManager.FindByPhoneNumber(registerViewModel.Number);

                if (existingNumber.Result == null)
                {
                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        //add the user to User role by default. 
                        await _userManager.AddToRoleAsync(user, "User");

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        {
                            return RedirectToAction("ListUsers", "Admin");
                        }

                        try
                        {
                            var verification = await VerificationResource.CreateAsync(
                                to: registerViewModel.Number,
                                channel: "sms",
                                pathServiceSid: _settings.VerificationServiceSID
                            );

                            if (verification.Status == "pending")
                            {
                                return RedirectToAction("ConfirmPhone", new { phoneNumber = registerViewModel.Number });
                            }

                            ModelState.AddModelError("", $"There was an error sending the verification code: {verification.Status}");
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("",
                                "There was an error sending the verification code, please check the phone number is correct and try again");
                        }
                    }

                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError("", "A user has already registered with the number " + registerViewModel.Number);
                }
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccountAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id);


            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }

            var currentRoles = await _userManager.GetRolesAsync(user.GetAwaiter().GetResult());

            var model = new EditUserViewModel
            {
                Id = user.Result.Id,
                UserName = user.Result.UserName,
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                IdNumber = user.Result.IDNumber,
                Number = user.Result.PhoneNumber,
                Roles = currentRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageAccountAsync(EditUserViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.Id).GetAwaiter().GetResult();

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }

            else
            {

                user.UserName = model.UserName;
                user.IDNumber = model.IdNumber;

                if (user.PhoneNumber != model.Number)
                {
                    user.PhoneNumber = model.Number;
                    user.PhoneNumberConfirmed = false;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
       
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }



    }
}