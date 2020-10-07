using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class LeaseController : Controller
    {
        private readonly ILeaseRepository _leaseRepository;

        
        public LeaseController(ILeaseRepository leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }


        [HttpGet]
        public IActionResult MakeBooking(int rentalAssetId)
        {
            ViewBag.RentalAssetId = rentalAssetId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeBooking(Lease lease)
        {
            // add the to the leave 
            if (ModelState.IsValid)
            {
               var success =  await _leaseRepository.AddLeaseAsync(lease);
                return RedirectToAction("Checkout", "Transaction", new {leaseId = success.LeaseId });
            }

            return View(lease);
        }

    }
}
