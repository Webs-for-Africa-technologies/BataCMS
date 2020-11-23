using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using COHApp.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class LeaseController : Controller
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly UserManager<VendorUser> _vendorUserManager;


        public LeaseController(ILeaseRepository leaseRepository, UserManager<VendorUser> userManager)
        {
            _leaseRepository = leaseRepository;
            _vendorUserManager = userManager;
        }


        [HttpGet]
        public IActionResult MakeBooking(int rentalAssetId)
        {
            ViewBag.RentalAssetId = rentalAssetId;
            return View();
        }


        [HttpGet]
        public IActionResult CashBooking(int rentalAssetId)
        {
            ViewBag.RentalAssetId = rentalAssetId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CashBooking(AddLeaseViewModel model)
        {

            VendorUser vendorUser = await _vendorUserManager.FindByPhoneNumber(ProcessPhoneNumber(model.VendorPhone));

            // add the to the lease 
            if (ModelState.IsValid)
            {
                if (vendorUser == null)
                {
                    ModelState.AddModelError("", "The phone number "+ model.VendorPhone + " is not a registered Vendor");
                    return View();
                }

                Lease lease = new Lease
                {
                    UserId = vendorUser.Id,
                    RentalAssetId = model.RentalAssetId,
                    leaseFrom = model.leaseFrom,
                    leaseTo = model.leaseTo
                };
                var success =  await _leaseRepository.AddLeaseAsync(lease);
                return RedirectToAction("Checkout", "Transaction", new {leaseId = success.LeaseId });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeBooking(Lease lease)
        {
            // add the to the lease 
            if (ModelState.IsValid)
            {
                var success = await _leaseRepository.AddLeaseAsync(lease);

                return RedirectToAction("Checkout", "Transaction", new { leaseId = success.LeaseId });
            }

            return View(lease);
        }

        private string ProcessPhoneNumber(string phoneNumber)
        {
            string processedNumber = null;
            string extension = "+263";

            if (phoneNumber != null)
            {
                string startingNum = phoneNumber.Substring(0, 1);

                //not in E.164 format
                if (startingNum != "+")
                {
                    if (startingNum == "0")
                    {
                        processedNumber = extension + phoneNumber.Substring(1);
                    }
                }
            }
            return processedNumber;
        }
    }
}
