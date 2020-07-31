using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Migrations;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BataCMS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly IPurchasePayementMethodRepository _purchasePaymentMethodRepository;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly ICurrencyRepository _currencyRepository;



        public PurchaseController(IPurchaseRepository purchaseRepository, ICheckoutRepository checkoutRepository, IPaymentMethodRepository paymentMethodRepository,IUnitItemRepository unitItemRepository,IPurchasePayementMethodRepository purchasePayementMethod, IPurchasedItemRepository purchasedItemRepository, ICurrencyRepository currencyRepository)
        {
            _purchaseRepository = purchaseRepository;
            _checkoutRepository = checkoutRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _unitItemRepository = unitItemRepository;
            _purchasePaymentMethodRepository = purchasePayementMethod;
            _purchasedItemRepository = purchasedItemRepository;
            _currencyRepository = currencyRepository; 
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
            var items = _checkoutRepository.GetCheckoutItems();

            Checkout checkout = new Checkout { CheckoutItems = items };

            if (checkout.CheckoutItems.Count == 0)
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

                _purchasePaymentMethodRepository.AddPurchasePaymentMethod(purchasePaymentMethod);;
                _checkoutRepository.ClearCheckout();
                return RedirectToAction("CheckoutComplete"); 
            }

            return View(purchase);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListPurchases()
        {
            IEnumerable<Purchase> purchases = _purchaseRepository.Purchases;
            var vm = new ListPurchaseViewModel
            { 
                Purchases = purchases
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PurchaseDetail(int purchaseId)
        {
            var purchaseObjects = new List<PurchaseObject>();

            Purchase purchase = _purchaseRepository.GetPurchaseById(purchaseId);

            IEnumerable<PurchasedItem> purchasedItems = _purchasedItemRepository.GetPurchasedItemsById(purchaseId);

            foreach (var item in purchasedItems)
            {
                unitItem unit = _unitItemRepository.GetItemById(item.unitItemId);

                var purchaseObject = new PurchaseObject { 
                    PurchaseAmount = item.Amount,
                    Price = item.Price,
                    ItemName = unit.Name
                };

                purchaseObjects.Add(purchaseObject);
            }
     

            PurchasePaymentMethod purchasePaymentMethod = _purchasePaymentMethodRepository.GetPurchasePaymentMethodByPurchaseId(purchaseId);
            PaymentMethod paymentMethod = _paymentMethodRepository.GetPaymentMethodById(purchasePaymentMethod.PaymentMethodId);
            Currency currency = _currencyRepository.GetCurrencyByName(paymentMethod.PaymentMethodName);


            var vm = new PurchaseDetailViewModel
            {
                PurchaseId = purchase.PurchaseId,
                PurchasedItems = purchaseObjects,
                PaymentMethod = paymentMethod,
                Purchase = purchase,
                Currency = currency
                
            }; 

            return View(vm);
        }
    }
}