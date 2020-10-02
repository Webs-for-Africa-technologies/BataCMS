using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;

namespace COHApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly IRentalAssetRepository _rentalAssetRepository;
        private readonly ITransactionRepository _transactionRepository;

        private readonly UserManager<ApplicationUser> _userManager;


        public TransactionController(ILeaseRepository leaseRepository, IRentalAssetRepository rentalAssetRepository, UserManager<ApplicationUser> userManager, ITransactionRepository transactionRepository)
        {
            _leaseRepository = leaseRepository;
            _rentalAssetRepository = rentalAssetRepository;
            _userManager = userManager;
            _transactionRepository = transactionRepository;
        }

        [Authorize]
        public async Task<IActionResult> CheckoutAsync(int leaseId)
        {

            List<SelectListItem> paymentOptions = new List<SelectListItem>();
            paymentOptions.Add(new SelectListItem() { Text = "Debit Card", Value = "swipe" });
            paymentOptions.Add(new SelectListItem() { Text = "Cash", Value = "cash" });
            paymentOptions.Add(new SelectListItem() { Text = "Eco Cash", Value = "ecocash" });

            this.ViewData["paymentOptions"] = paymentOptions;

            Lease lease = await _leaseRepository.GetLeaseById(leaseId);
            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);

            var totalDays = (lease.leaseTo - lease.leaseFrom).TotalDays;
            decimal transactionTotal = rentalAsset.Price * (decimal)totalDays;

            TransactionCheckoutViewModel vm = new TransactionCheckoutViewModel()
            {
                AssetPricing = rentalAsset.Price,
                RentalDuration = totalDays,
                TransactionTotal = transactionTotal
            };

            ViewBag.leaseId = leaseId;
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(TransactionCheckoutViewModel model)
        {
            //get the lease
            Lease lease = await _leaseRepository.GetLeaseById(model.LeaseId);
            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);

            //get the vendorUserId 
             ApplicationUser user = await _userManager.FindByIdAsync(lease.UserId);

            var totalDays = (lease.leaseTo - lease.leaseFrom).TotalDays;
            decimal transactionTotal = rentalAsset.Price * (decimal)totalDays;

            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                TransactionTotal = transactionTotal,
                ServerName = user.FirstName + user.LastName,
                TransactionDate = DateTime.Today,
                TransactionNotes = model.TransactionNotes,
                TransactionType =  model.TransactionType,
                };

               var result =  await _transactionRepository.CreateTransactionAsync(transaction);

                //success 
                if (result > 0)
                {
                    await _rentalAssetRepository.BookAsset(lease.leaseTo, rentalAsset.RentalAssetId);
                }

                return RedirectToAction("CheckoutComplete");
            }
            return View(model);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
        }
    }
}
