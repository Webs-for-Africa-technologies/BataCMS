using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class WaterAvailabilityController : Controller
    {
        public readonly IWaterAvailabilityRepository _waterAvailabilityRepository;

        public WaterAvailabilityController(IWaterAvailabilityRepository waterAvailabilityRepository)
        {
            _waterAvailabilityRepository = waterAvailabilityRepository;
        }

        public IActionResult List()
        {
            IEnumerable<WaterAvailability> waterAvailabilities = _waterAvailabilityRepository.WaterAvailabilities;
            ViewBag.SearchString = "All Locations";

            return View(waterAvailabilities);
        }

        [HttpPost]
        public IActionResult List(string SearchString)
        {
            IEnumerable<WaterAvailability> waterAvailabilities;

            if (!string.IsNullOrEmpty(SearchString))
            {
                waterAvailabilities = _waterAvailabilityRepository.WaterAvailabilities.Where(p => p.ServiceArea.Contains(SearchString));
                ViewBag.SearchString = SearchString;
            }
            else
            {
                waterAvailabilities = _waterAvailabilityRepository.WaterAvailabilities;
                ViewBag.SearchString = "All Locations";
            }

            return View(waterAvailabilities);
        }

       [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddWasteCollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                WaterAvailability waterAvailability = new WaterAvailability
                {
                    ServiceArea = model.ServiceArea,
                    Mon = model.Mon,
                    Tue = model.Tue,
                    Wed = model.Wed,
                    Thur = model.Thur,
                    Fri = model.Fri,
                    Sat = model.Sat,
                    Sun = model.Sun,
                    DateModified = DateTime.Now
                };

                await _waterAvailabilityRepository.AddWaterAvailabilityAsync(waterAvailability);

                return RedirectToAction("Complete", new { message = "Water availability information has been added successfully" });
            }
            return View(model);
        }
     
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var waterAvailability = await _waterAvailabilityRepository.GetWaterAvailabilityAsync(id);

            if (waterAvailability == null)
            {
                ViewBag.ErrorMessage = $"Water availabilty information with Id {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditWasteCollectionViewModel
            {
                Id = waterAvailability.WaterAvailabilityId,
                ServiceArea = waterAvailability.ServiceArea,
                Mon = waterAvailability.Mon,
                Tue = waterAvailability.Tue,
                Wed = waterAvailability.Wed,
                Thur = waterAvailability.Thur,
                Fri = waterAvailability.Fri,
                Sat = waterAvailability.Sat,
                Sun = waterAvailability.Sun,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditWasteCollectionViewModel model)
        {
            var waterAvailability = await _waterAvailabilityRepository.GetWaterAvailabilityAsync(model.Id);

            if (waterAvailability == null)
            {
                ViewBag.ErrorMessage = $"Water availability information with Id {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                waterAvailability.ServiceArea = model.ServiceArea;
                waterAvailability.Mon = model.Mon;
                waterAvailability.Tue = model.Tue;
                waterAvailability.Wed = model.Wed;
                waterAvailability.Thur = model.Thur;
                waterAvailability.Fri = model.Fri;
                waterAvailability.Sat = model.Sat;
                waterAvailability.Sun = model.Sun;
                waterAvailability.DateModified = DateTime.Now;

                await _waterAvailabilityRepository.UpdateWaterAvailability(waterAvailability);
                return RedirectToAction("Complete", new { message = "The water availabilty information was updated successfully" });

            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var waterAvailability = await _waterAvailabilityRepository.GetWaterAvailabilityAsync(id);

            if (waterAvailability == null)
            {
                ViewBag.ErrorMessage = $"The water availabilty information with  Id  {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _waterAvailabilityRepository.DeleteWaterAvailabitly(waterAvailability);

                return RedirectToAction("Complete", new { message = "The waste collection information was deleted successfully" });
            }
        }


        public IActionResult Complete(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
