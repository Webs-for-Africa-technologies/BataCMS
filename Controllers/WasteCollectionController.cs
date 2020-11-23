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
    public class WasteCollectionController : Controller
    {
        public readonly IWasteCollectionsRepository _wasteCollectionsRepository;

        public WasteCollectionController(IWasteCollectionsRepository wasteCollectionsRepository)
        {
            _wasteCollectionsRepository = wasteCollectionsRepository;
        }

        public IActionResult List()
        {
            IEnumerable<WasteCollection> wasteCollections = _wasteCollectionsRepository.WasteCollections;
            ViewBag.SearchString = "All Locations";

            return View(wasteCollections);
        }

        [HttpPost]
        public IActionResult List(string SearchString)
        {
            IEnumerable<WasteCollection> wasteCollections;
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                wasteCollections = _wasteCollectionsRepository.WasteCollections.Where(p => p.ServiceArea.Contains(SearchString));
                ViewBag.SearchString = SearchString;
            }
            else
            {
                wasteCollections = _wasteCollectionsRepository.WasteCollections;
                ViewBag.SearchString = "All Locations";
            }

            return View(wasteCollections);
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
                WasteCollection wasteCollection = new WasteCollection
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

                await _wasteCollectionsRepository.AddWasteCollectionAsync(wasteCollection);

                return RedirectToAction("Complete", new { message = "Waste collection information has been added successfully" });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var wasteCollection = await _wasteCollectionsRepository.GetWasteCollectionAsync(id);

            if (wasteCollection == null)
            {
                ViewBag.ErrorMessage = $"Waste collection information with={id} cannot be found";
                return View("NotFound");
            }

            var model = new EditWasteCollectionViewModel
            {
                Id = wasteCollection.WasteCollectionId,
                ServiceArea = wasteCollection.ServiceArea,
                Mon = wasteCollection.Mon,
                Tue = wasteCollection.Tue,
                Wed = wasteCollection.Wed,
                Thur = wasteCollection.Thur,
                Fri = wasteCollection.Fri,
                Sat = wasteCollection.Sat,
                Sun = wasteCollection.Sun,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditWasteCollectionViewModel model)
        {
            var wasteCollection = await _wasteCollectionsRepository.GetWasteCollectionAsync(model.Id);

            if (wasteCollection == null)
            {
                ViewBag.ErrorMessage = $"Waste collection with Id={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                wasteCollection.ServiceArea = model.ServiceArea;
                wasteCollection.Mon = model.Mon;
                wasteCollection.Tue = model.Tue;
                wasteCollection.Wed = model.Wed;
                wasteCollection.Thur = model.Thur;
                wasteCollection.Fri = model.Fri;
                wasteCollection.Sat = model.Sat;
                wasteCollection.Sun = model.Sun;
                wasteCollection.DateModified = DateTime.Now;

                await _wasteCollectionsRepository.UpdateWasteCollection(wasteCollection);
                return RedirectToAction("Complete", new { message = "The waste collection was updated successfully" });

            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var wasteCollection = await _wasteCollectionsRepository.GetWasteCollectionAsync(id);

            if (wasteCollection == null)
            {
                ViewBag.ErrorMessage = $"The waste collection with  Id  {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _wasteCollectionsRepository.DeleteWasteCollection(wasteCollection);

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
