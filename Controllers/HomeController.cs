using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                HomeItems = _unitItemRepository.unitItems.Where(p => p.Category.CategoryName == "Food")
            };

            return View(homeViewModel);
        }
    }
}