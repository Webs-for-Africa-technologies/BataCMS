using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data;
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
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly AppDbContext _appDbContext;

        public PurchaseController(IPurchaseRepository purchaseRepository, Checkout checkout, IPaymentMethodRepository paymentMethodRepository, AppDbContext appDbContext)
        {
            _purchaseRepository = purchaseRepository;
            _checkout = checkout;
            _paymentMethodRepository = paymentMethodRepository;
            _appDbContext = appDbContext; 
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize] 
        public IActionResult Checkout(Purchase purchase, PaymentMethod paymentMethod)
        {
            var items = _checkout.GetCheckoutItems();
            _checkout.CheckoutItems = items;

            if (_checkout.CheckoutItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty");
            }
            if (ModelState.IsValid)
            {
                _purchaseRepository.CreatePurchase(purchase);
                _paymentMethodRepository.CreatePaymentMethod(paymentMethod);

                var purchasePaymentMethod = new PurchasePaymentMethod
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    PurchaseId =  purchase.PurchaseId
                };

                _appDbContext.Add(purchasePaymentMethod);
                _appDbContext.SaveChanges();
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