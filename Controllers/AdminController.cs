using BataCMS.Data.Models;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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


        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
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
    } 
}