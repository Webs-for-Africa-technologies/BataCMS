using System;
using System.Collections.Generic;
using System.IO;
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
using OfficeOpenXml;

namespace BataCMS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly ICurrencyRepository _currencyRepository;



        public PurchaseController(IPurchaseRepository purchaseRepository, ICheckoutRepository checkoutRepository, IPaymentMethodRepository paymentMethodRepository,IUnitItemRepository unitItemRepository, IPurchasedItemRepository purchasedItemRepository, ICurrencyRepository currencyRepository)
        {
            _purchaseRepository = purchaseRepository;
            _checkoutRepository = checkoutRepository;
            _unitItemRepository = unitItemRepository;
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
            decimal checkoutTotal = _checkoutRepository.GetCheckoutTotal();

            Checkout checkout = new Checkout { CheckoutItems = items };

            if (checkout.CheckoutItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty");
            }

            if (paymentMethod.AmountPaid < checkoutTotal)
            {
                ModelState.AddModelError("", "Please enter the amount equal or greater than your Purchase Total");
                return View();
            }
            if (ModelState.IsValid)
            {
                _purchaseRepository.CreatePurchase(purchase);
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
        public IActionResult ListPurchases(string filter)
        {
            IEnumerable<Purchase> purchases = null; 
            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                purchases = _purchaseRepository.Purchases;
            }
            else
            {
                if (filter == "hour")
                {
                    purchases = _purchaseRepository.Purchases.Where(p => p.PurchaseDate >= (DateTime.Now.AddHours(-1)));  
                }
                if (filter == "day")
                {
                    purchases = _purchaseRepository.Purchases.Where(p => p.PurchaseDate >= (DateTime.Now.AddDays(-1)));
                }
                if (filter == "week")
                {
                    purchases = _purchaseRepository.Purchases.Where(p => p.PurchaseDate >= (DateTime.Now.AddDays(-7)));
                }
            }
            var vm = new ListPurchaseViewModel
            { 
                Purchases = purchases
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult PostExportExcel()
        {
            var myPurchases =  _purchaseRepository.Purchases.ToList();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[1, 1].Value = "PurchaseId";
                workSheet.Cells[1, 2].Value = "PurchaseDate";
                workSheet.Cells[1, 3].Value = "ServerName";
                workSheet.Cells[1, 4].Value = "PurchasesTotal";
                workSheet.Cells[1, 5].Value = "PaymentMethods";

                workSheet.Row(1).Height = 20;
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 15;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 16;

                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells["B2:B" + (myPurchases.Count+1)].Style.Numberformat.Format = "yyyy-mm-dd";

                for (int index = 1; index <= myPurchases.Count; index++)
                {                      
                    workSheet.Cells[index + 1, 1].Value = myPurchases[index - 1].PurchaseId;
                    workSheet.Cells[index + 1, 2].Value = myPurchases[index - 1].PurchaseDate;
                    workSheet.Cells[index + 1, 3].Value = myPurchases[index - 1].ServerName;
                    workSheet.Cells[index + 1, 4].Value = myPurchases[index - 1].PurchasesTotal;
                    workSheet.Cells[index + 1, 5].Value = myPurchases[index - 1].PaymentMethods.First().PaymentMethodName;
                }
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"Purchases-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            // above I define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
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

            Currency currency = _currencyRepository.GetCurrencyByName(purchase.PaymentMethods.First().PaymentMethodName);
            PaymentMethod paymentMethod = purchase.PaymentMethods.First();


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