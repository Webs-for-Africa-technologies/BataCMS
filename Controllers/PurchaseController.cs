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
using System.Text.Json;
using RentalAsset = BataCMS.Data.Models.RentalAsset;
using Transaction = BataCMS.Data.Models.Transaction;

namespace BataCMS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;



        public PurchaseController(IPurchaseRepository purchaseRepository, ICheckoutRepository checkoutRepository, IPaymentMethodRepository paymentMethodRepository,IUnitItemRepository unitItemRepository, IPurchasedItemRepository purchasedItemRepository, ICurrencyRepository currencyRepository)
        {
            _purchaseRepository = purchaseRepository;
            _checkoutRepository = checkoutRepository;
            _unitItemRepository = unitItemRepository;
            _purchasedItemRepository = purchasedItemRepository;
            _currencyRepository = currencyRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CheckoutAsync(Transaction purchase, PaymentMethod paymentMethod)
        {
            var items = await _checkoutRepository.GetCheckoutItemsAsync();
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
                await _purchaseRepository.CreatePurchaseAsync(purchase);
                await _checkoutRepository.ClearCheckoutAsync();
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
        public IActionResult ListOrders(string filter)
        {
            IEnumerable<Transaction> purchases = null; 
            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                purchases = _purchaseRepository.Purchases.Where(p => p.isDelivered == false);
            }
            else
            {
                if (filter == "hour")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddHours(-1)) && p.isDelivered == false));  
                }
                if (filter == "day")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-1)) && p.isDelivered == false));
                }
                if (filter == "week")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-7)) && p.isDelivered == false));
                }
            }
            var vm = new ListPurchaseViewModel
            { 
                Purchases = purchases
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListPurchases(string filter)
        {
            IEnumerable<Transaction> purchases = null;
            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                purchases = _purchaseRepository.Purchases.Where(p => p.isDelivered == true);
            }
            else
            {
                if (filter == "hour")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddHours(-1)) && p.isDelivered == true));
                }
                if (filter == "day")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-1)) && p.isDelivered == true));
                }
                if (filter == "week")
                {
                    purchases = _purchaseRepository.Purchases.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-7)) && p.isDelivered == true));
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
                    workSheet.Cells[index + 1, 1].Value = myPurchases[index - 1].TransactionId;
                    workSheet.Cells[index + 1, 2].Value = myPurchases[index - 1].TransactionDate;
                    workSheet.Cells[index + 1, 3].Value = myPurchases[index - 1].ServerName;
                    workSheet.Cells[index + 1, 4].Value = myPurchases[index - 1].TransactionTotal;
                }
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"Purchases-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            // above I define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeliveryAsync(int purchaseId)
        {
            Transaction purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            purchase.isDelivered = true;

            //update the paymentMethod
            await _purchaseRepository.UpdatePurchaseAsync(purchase);
            return RedirectToAction("ListOrders", "Purchase");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PurchaseDetailAsync(int purchaseId)
        {
            var purchaseObjects = new List<PurchaseObject>();

            Transaction purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);

            IEnumerable<PurchasedItem> purchasedItems = _purchasedItemRepository.GetPurchasedItemsById(purchaseId);

            foreach (var item in purchasedItems)
            {
                RentalAsset unit = await _unitItemRepository.GetItemByIdAsync(item.unitItemId);

                List<userOptionObject> userOptionValues = new List<userOptionObject>();

                if (item.selectedOptionData != "dummyData")
                {
                    using var jsonDoc = JsonDocument.Parse(item.selectedOptionData);
                    var root = jsonDoc.RootElement;

                    for (int i = 0; i < root.GetArrayLength(); i++)
                    {
                        var selectedValue = string.Empty;
                        var userDataOption = string.Empty; 

                        var mainLabel = root[i].GetProperty("label").ToString();
                        var values = root[i].GetProperty("values");

                        bool isUserData = root[i].TryGetProperty("userData", out var jsonElement);

                        if (!isUserData)
                        {
                            selectedValue = "No";

                        }
                        else
                        {
                            userDataOption = root[i].GetProperty("userData")[0].ToString();
                            if (values.GetArrayLength() == 1 /*checkbox */)
                            {
                                selectedValue = "Yes";
                            } else
                            {
                                for (int j = 0; j < values.GetArrayLength(); j++)
                                {
                                    var optionVal = values[j].GetProperty("value").ToString();
                                    if (optionVal == userDataOption)
                                    {
                                        selectedValue = values[j].GetProperty("label").ToString();
                                        break;
                                    }
                                }

                            }
                        }
                        


                        var userOptionValue = new userOptionObject
                        {
                            LabelName = mainLabel,
                            SelectedValueName = selectedValue,
                        };
                        userOptionValues.Add(userOptionValue); 
                    }
                    
                }

                

                var purchaseObject = new PurchaseObject { 
                    PurchaseAmount = item.Amount,
                    Price = item.Price,
                    ItemName = unit.Name,
                    purchaseSelectedOptions = userOptionValues,
                };

                purchaseObjects.Add(purchaseObject);
            }

            //Currency currency = await _currencyRepository.GetCurrencyByNameAsync(purchase.PaymentMethods.First().PaymentMethodName);
            //PaymentMethod paymentMethod = purchase.PaymentMethods.First();


            var vm = new PurchaseDetailViewModel
            {
                PurchaseId = purchase.TransactionId,
                PurchasedItems = purchaseObjects,
                Purchase = purchase,
                
            }; 

            return View(vm);
        }

        public async Task<IActionResult> DeletePurchasesAsync()
        {
            await _purchaseRepository.DeletePurchasesAsync();

            return RedirectToAction("ListPurchases");

        }
    }



}