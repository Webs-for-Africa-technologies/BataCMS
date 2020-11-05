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

        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IRentalAssetRepository _rentalAssetRepository;



        public HomeController( IServiceTypeRepository serviceTypeRepository, IRentalAssetRepository rentalAssetRepository)
        {;
            _serviceTypeRepository = serviceTypeRepository;
            _rentalAssetRepository = rentalAssetRepository; 
        }
        public ViewResult Index()
        {
            return View();
        }


        public ViewResult OnDemandIndex()
        {
            var homeViewModel = new HomeViewModel
            {
                ServiceTypes = _serviceTypeRepository.ServiceTypes
            };

            return View(homeViewModel);
        }

    }
}