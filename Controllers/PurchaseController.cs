using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BataCMS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly Checkout _checkout;

        public PurchaseController(IPurchaseRepository purchaseRepository, Checkout checkout)
        {
            _purchaseRepository = purchaseRepository;
            _checkout = checkout;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize] 
        public IActionResult Checkout(Purchase purchase)
        {
            var items = _checkout.CheckoutItems;
            _checkout.CheckoutItems = items;

            if (_checkout.CheckoutItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty");
            }
            if (ModelState.IsValid)
            {
                _purchaseRepository.CreatePurchase(purchase);
                _checkout.ClearCheckout();
                return RedirectToAction("CheckoutComplete"); 
            }

            return View(purchase);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
        }
    }
}