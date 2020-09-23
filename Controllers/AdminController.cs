using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitItemRepository _unitItemRepository;



        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ICategoryRepository categoryRepository, ICurrencyRepository currencyRepository, IUnitItemRepository unitItemRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _unitItemRepository = unitItemRepository; 
        }   

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description
                };

                var existingCategory = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == category.CategoryName);

                if (existingCategory == null)
                {
                    await _categoryRepository.AddCategoryAsync(category);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "The Category already exists");
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListCategories()
        {
            var categories = _categoryRepository.Categories;
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            int _categoryId = id;
            if (category == null)
            {
                ViewBag.ErrorMessage = $"User with category id ={id} cannot be found";
                return View("NotFound");
            }

            IEnumerable<RentalAsset> unitItems = _unitItemRepository.unitItems.Where(p => p.CategoryId == _categoryId);

            var model = new EditCategoryViewModel
            {
                Id = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                ItemList = unitItems
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategoryAsync(EditCategoryViewModel model)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(model.Id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                category.CategoryName = model.CategoryName;
                category.Description = model.Description;

                await _categoryRepository.UpdateCategoryAsync(category);
                return RedirectToAction("ListCategories", "Admin");

            }
        }

        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _categoryRepository.DeleteCategoryAsync(category);
                
                return RedirectToAction("ListCategories");
            }
        }


        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpPost]
        public IActionResult ListUsers(string SearchString)
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                var searchedUsers = _userManager.Users.Where(p => p.UserName.Contains(SearchString));
                return View(searchedUsers);
            }
            else
            {
                var users = _userManager.Users;
                return View(users);
            }
        }


        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserAsync(EditUserViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.Id);

            if (user == null )
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Result.UserName = model.UserName;
                user.Result.PhoneNumber = model.Number;

                var result = await _userManager.UpdateAsync(user.Result);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers",  "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user.Result);

            var model = new EditUserViewModel
            {
                Id = user.Result.Id,
                UserName = user.Result.UserName,
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                IdNumber = user.Result.IDNumber,
                Number = user.Result.PhoneNumber,
                Roles = userRoles
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with roleId = {roleId} cannot be found";
                return View("Not found");
            }

            var model = new List<UserRoleViewModel>();

            var AllUser = _userManager.Users;
            var UserInThisRole = await _userManager.GetUsersInRoleAsync(role.Result.Name);

            foreach (var user in AllUser)
            {
                var userRoleViewModel = new UserRoleViewModel
                { 
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = false

                };
                if (UserInThisRole.Any(p=>p.Id == userRoleViewModel.UserId))
                {
                    userRoleViewModel.IsSelected = true;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserRoleViewModel> model, string roleId) 
        {
            var role = _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with roleId = {roleId} cannot be found";
                return View("Not found");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Result.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Result.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Result.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Result.Name);

                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("ListRoles");   
                    }
                }

            }

            return RedirectToAction("ListRoles");

            }

        
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user.Result);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }            
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                return View("ListUsers");

            }
        }

        [HttpGet]
        public IActionResult AddCurrency()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrencyAsync(AddCurrencyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Currency currency = new Currency
                {
                    CurrencyName = model.CurrencyName,
                    Rate = model.Rate
                };

                var existingCurrency = await _currencyRepository.GetCurrencyByNameAsync(model.CurrencyName);

                if (existingCurrency == null)
                {
                    await _currencyRepository.AddCurrencyAsync(currency);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "The Currency already exists");
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListCurrencies()
        {
            var currencies = _currencyRepository.Currencies;
            return View(currencies);
        }

        [HttpGet]
        public async Task<IActionResult> EditCurrencyAsync(int id)
        {
            var currency = await _currencyRepository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }


            var model = new EditCurrencyViewModel
            {
                Id = currency.CurrencyId,
                CurrencyName = currency.CurrencyName,
                Rate = currency.Rate
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCurrencyAsync(EditCurrencyViewModel model)
        {
            var currency = await _currencyRepository.GetCurrencyByIdAsync(model.Id);

            if (currency == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                currency.CurrencyName = model.CurrencyName;
                currency.Rate = model.Rate;

                if (model.IsActive == true)
                {
                    await _currencyRepository.SetCurrentCurrencyAsync(currency);
                }

                await _currencyRepository.UpdateCurrencyAsync(currency);

                return RedirectToAction("ListCurrencies", "Admin");
            }
        }

        public async Task<IActionResult> DeleteCurrencyAsync(int id)
        {
            var currency = await _currencyRepository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                if (currency.isCurrent == true)
                {
                    ViewBag.ErrorMessage = $"You cannot delete the currency {currency.CurrencyName}, the currency is currently active";
                    return View("NotFound");

                }
                else
                {
                    await _currencyRepository.DeleteCurrencyAsync(currency);
                    return RedirectToAction("ListCurrencies");

                }

            }
        }



    }

}
