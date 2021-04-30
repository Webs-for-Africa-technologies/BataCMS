using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRentalAssetRepository _rentalAssetRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;



        public AdminController(IServiceTypeRepository serviceTypeRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ICategoryRepository categoryRepository, IRentalAssetRepository rentalAssetRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _rentalAssetRepository = rentalAssetRepository;
            _serviceTypeRepository = serviceTypeRepository;
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

            IEnumerable<RentalAsset> unitItems = _rentalAssetRepository.rentalAssets.Where(p => p.CategoryId == _categoryId);

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
        public IActionResult ListUsers(string role)
        {

            string _role = role;
            IEnumerable<ApplicationUser> users;
            
            if (!string.IsNullOrEmpty(role))
            {
                users = _userManager.GetUsersInRoleAsync(role).Result;
                ViewBag.Role = role;
            }
            else
            {
                users = _userManager.Users;
                ViewBag.Role = "All Users";
            }


            var roles = _roleManager.Roles;

            var vm = new ListUsersViewModel 
            {
                Users = users,
                Roles = roles,

            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult ListUsers(string role, string SearchString)
        {
            string _role = role;
            IEnumerable<ApplicationUser> users;

            if (!string.IsNullOrEmpty(role))
            {
                users = _userManager.GetUsersInRoleAsync(role).Result;
                ViewBag.Role = role;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    users = users.Where(p => p.UserName.Contains(SearchString));
                }
            }
            else if (!string.IsNullOrEmpty(SearchString))
            {
                users = _userManager.Users.Where(p => p.UserName.Contains(SearchString));

            }
            else
            {
                users = _userManager.Users;
                ViewBag.Role = "All Users";
            }
            var roles = _roleManager.Roles;
            var vm = new ListUsersViewModel
            {
                Users = users,
                Roles = roles
            };
            return View(vm);
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
            var user = _userManager.FindByIdAsync(model.Id).GetAwaiter().GetResult();

            if (user == null )
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }
            
            else
            {

                //add a user to a role
                IdentityResult roleResult = null;
                if (model.AddedRole != null)
                {
                    var addedRole = _roleManager.FindByIdAsync(model.AddedRole).GetAwaiter().GetResult();
                    roleResult = await _userManager.AddToRoleAsync(user, addedRole.Name);
                }

                //remove user from role
                if (model.RemovedRole != null)
                {
                    var removedRole = _roleManager.FindByNameAsync(model.RemovedRole).GetAwaiter().GetResult();
                    roleResult = await _userManager.RemoveFromRoleAsync(user, removedRole.Name);
                }

                user.UserName = model.UserName;
                user.PhoneNumber = ProcessPhoneNumber(model.Number);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (roleResult != null)
                    {
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("ListUsers", "Admin");
                        }
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }

                    return RedirectToAction("ListUsers", "Admin");

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

            this.ViewData["availableRoles"] = _roleManager.Roles.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name
            }).ToList();

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={id} cannot be found";
                return View("NotFound");
            }

            var currentRoles = await _userManager.GetRolesAsync(user.GetAwaiter().GetResult());

            this.ViewData["currentRoles"] = currentRoles.Select(x => new SelectListItem
            {
                Value = x,
                Text = x
            }).ToList();

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

        private string ProcessPhoneNumber(string phoneNumber)
        {
            string processedNumber = phoneNumber;
            string extension = "+263";

            if (phoneNumber != null)
            {
                string startingNum = phoneNumber.Substring(0, 1);

                //not in E.164 format
                if (startingNum != "+")
                {
                    if (startingNum == "0")
                    {
                        processedNumber = extension + phoneNumber.Substring(1);
                    }
                }
            }
            return processedNumber;
        }

        [HttpGet]
        public IActionResult ListServiceTypes()
        {
            var serviceTypes = _serviceTypeRepository.ServiceTypes;
            return View(serviceTypes);
        }

        [HttpGet]
        public IActionResult AddServiceType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceTypeAsync(AddServiceTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceType serviceType = new ServiceType
                {
                    ServiceName = model.ServiceName,
                    ServiceDescription = model.Description,
                    Pricing = model.Pricing
                };

                var existingServiceType = _serviceTypeRepository.ServiceTypes.FirstOrDefault(p => p.ServiceName == serviceType.ServiceName);

                if (existingServiceType == null)
                {
                    await _serviceTypeRepository.AddServiceTypeAsync(serviceType);
                    return RedirectToAction("ListServiceTypes", "Admin");

                }
                else
                {
                    ModelState.AddModelError("", "The Category already exists");
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditServiceTypeAsync(int id)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(id);
            int _serviceTypeId = id;
            if (serviceType == null)
            {
                ViewBag.ErrorMessage = $"User with category id ={id} cannot be found";
                return View("NotFound");
            }


            var model = new EditServiceTypeViewModel
            {
                Id = serviceType.ServiceTypeId,
                ServiceTypeName = serviceType.ServiceName,
                Description = serviceType.ServiceDescription,
                Pricing = serviceType.Pricing
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditServiceTypeAsync(EditServiceTypeViewModel model)
        {
            var serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(model.Id);

            if (serviceType == null)
            {
                ViewBag.ErrorMessage = $"User with user id ={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                serviceType.ServiceName = model.ServiceTypeName;
                serviceType.ServiceDescription = model.Description;
                serviceType.Pricing = model.Pricing;

                await _serviceTypeRepository.UpdateServiceTypeAsync(serviceType);
                return RedirectToAction("ListServiceTypes", "Admin");

            }
        }

    }

}
