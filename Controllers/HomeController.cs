using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using BataCMS.Infrastructure;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BataCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly ICategoryRepository _categoryRepository;



        public HomeController(IUnitItemRepository unitItem, ICategoryRepository categoryRepository)
        {
            _unitItemRepository = unitItem;
            _categoryRepository = categoryRepository;
        }
        public ViewResult Index(string category)
        {
            string _category = category;
            IEnumerable<unitItem> unitItems;

            if (string.IsNullOrEmpty(category))
            {
                unitItems = _unitItemRepository.unitItems.Where(p => p.Category.CategoryName == "Food");
            }
            else
            {
                unitItems = _unitItemRepository.unitItems.Where(p => p.Category.CategoryName.Equals(_category));
            }

            var homeViewModel = new HomeViewModel
            {
                HomeItems = unitItems
            };

            return View(homeViewModel);
        }

        public RedirectToActionResult SetCurrency(int currencyId) 
        {
            HttpContext.Session.SetInt32("CurrencyId", currencyId);
            return RedirectToAction("Index");
        }
    }
}