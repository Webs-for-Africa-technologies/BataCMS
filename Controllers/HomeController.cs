using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using BataCMS.Infrastructure;
using BataCMS.ViewModels;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BataCMS.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IRentalAssetRepository _rentalAssetRepository;



        public HomeController( ICategoryRepository categoryRepository, IRentalAssetRepository rentalAssetRepository)
        {;
            _categoryRepository = categoryRepository;
            _rentalAssetRepository = rentalAssetRepository; 
        }
        public ViewResult Index(string category)
        {
            string _category = category;
            IEnumerable<RentalAsset> rentalAssets;

            if (string.IsNullOrEmpty(category))
            {
                rentalAssets = _rentalAssetRepository.rentalAssets.Where(p => p.Category.CategoryName == "Mbare Musika");
            }
            else
            {
                rentalAssets = _rentalAssetRepository.rentalAssets.Where(p => p.Category.CategoryName.Equals(_category));
            }

            var homeViewModel = new HomeViewModel
            {
                HomeItems = rentalAssets
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