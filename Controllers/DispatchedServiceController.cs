using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class DispatchedServiceController : Controller
    {
        private readonly IDispatchedServiceRepository _dispatchedServiceRepository;

        public DispatchedServiceController(IDispatchedServiceRepository dispatchedServiceRepository)
        {
            _dispatchedServiceRepository = dispatchedServiceRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult List(string filter)
        {
            IEnumerable<DispatchedService> dispatchedServices = null;

            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                dispatchedServices = _dispatchedServiceRepository.DispatchedServices;
            }
            else
            {
                if (filter == "hour")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddHours(-1))));
                }
                if (filter == "day")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddDays(-1))));
                }
                if (filter == "week")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddDays(-7))));
                }
            }


            return View(dispatchedServices);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(int Id)
        {

            DispatchedService dispatchedService = await _dispatchedServiceRepository.GetServiceByIdAsync(Id);


            return View(dispatchedService);

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
