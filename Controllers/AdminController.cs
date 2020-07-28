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


        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ICategoryRepository categoryRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }   

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(AddCategoryViewModel model)
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
                    _categoryRepository.AddCategory(category);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "The Category already exists");
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole{Name = model.RoleName};

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
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }


        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserAsync(EditUserViewModel model)
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

                var result = await _userManager.UpdateAsync(user.Result);

                if (result.Succeeded)
                {
                    RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserAsync(string id)
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
        }

}
