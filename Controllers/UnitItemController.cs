using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.ViewModels;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BataCMS.Controllers
{
    public class UnitItemController : Controller 
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        


        public UnitItemController(IUnitItemRepository unitItemRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _unitItemRepository = unitItemRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment; 
        }

        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var unitItem = _unitItemRepository.GetItemById(id);

            if (unitItem == null)
            {
                ViewBag.ErrorMessage = $"Item with  id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _unitItemRepository.DeleteItem(id)   ;

                //success
                if (result > 0)
                {
                    return RedirectToAction("List");
                }
                return View("List");

            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["categories"] = _categoryRepository.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryName,
                Text = x.CategoryName
            }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUnitItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Image != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath ,"images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                }

                var category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == model.Category);

                string photoPath = "/images/" + uniqueFileName;

                unitItem newUnitItem = new unitItem
                {
                    Name = model.Name,
                    Price = model.Price,
                    InStock = model.InStock,
                    DateModified = DateTime.Today,
                    CategoryId = category.CategoryId,
                    ImageUrl = photoPath
                };

                _unitItemRepository.Add(newUnitItem);
                return RedirectToAction("List", new { id = newUnitItem.unitItemId });

            }
            return View();
        }

        /*        [HttpGet]
                public async Task<IActionResult> EditUserAsync(int id)
                {
                    var unitItem = _unitItemRepository.GetItemById(id);

                    if (unitItem == null)
                    {
                        ViewBag.ErrorMessage = $"Item with id ={id} cannot be found";
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
                }*/

        public ViewResult List(string category)
        {
            string _category = category;
            IEnumerable<unitItem> unitItems;

            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
            {
                unitItems = _unitItemRepository.unitItems.OrderBy(p => p.unitItemId);
                currentCategory = "All Items";
            }
            else
            {
                if (string.Equals("Alcoholic",_category,StringComparison.OrdinalIgnoreCase))
                {
                    unitItems = _unitItemRepository.unitItems.Where(p => p.Category.CategoryName.Equals("Alcoholic"));
                }
                else
                {
                    unitItems = _unitItemRepository.unitItems.Where(p => p.Category.CategoryName.Equals("Food"));
                }

                currentCategory = _category;
            }

            var vm = new UnitItemListViewModel
            {
                UnitItems = unitItems,
                CurrentCategory = currentCategory
            };

            return View(vm);
        }

    }
}
