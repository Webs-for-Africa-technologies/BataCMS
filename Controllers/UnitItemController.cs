using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Controllers
{
    public class UnitItemController : Controller 
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitItemRepository _unitItemRepository;

        public UnitItemController(IUnitItemRepository unitItemRepository, ICategoryRepository categoryRepository)
        {
            _unitItemRepository = unitItemRepository;
            _categoryRepository = categoryRepository;
        }

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
