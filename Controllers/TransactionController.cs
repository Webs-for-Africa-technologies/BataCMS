using System;
using System.Collections.Generic;
using System.IO;
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
using OfficeOpenXml;

namespace COHApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly IRentalAssetRepository _rentalAssetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IActiveLeaseRepository _activeLeaseRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserManager<VendorUser> _vendorUserManager;




        public TransactionController(UserManager<VendorUser> vendorUserManager, ILeaseRepository leaseRepository, UserManager<ApplicationUser> userManager, IRentalAssetRepository rentalAssetRepository, ITransactionRepository transactionRepository, IActiveLeaseRepository activeLeaseRepository)
        {
            _leaseRepository = leaseRepository;
            _rentalAssetRepository = rentalAssetRepository;
            _userManager = userManager;
            _transactionRepository = transactionRepository;
            _activeLeaseRepository = activeLeaseRepository;
            _vendorUserManager = vendorUserManager;
        }

        [Authorize]
        public async Task<IActionResult> CheckoutAsync(int leaseId)
        {

            List<SelectListItem> paymentOptions = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Debit Card", Value = "swipe" },
                new SelectListItem() { Text = "Eco Cash", Value = "ecocash" }
            };

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

        [Authorize(Roles = "Admin")]
        public IActionResult ListTransactions(string filter, string type)
        {
            IEnumerable<Transaction> transactions = null;
            decimal totalSales = 0M;

            if (!string.IsNullOrEmpty(type))
            {
                transactions = _transactionRepository.Transactions.Where((p => p.TransactionType == type));

            }
            else
            {
                if (String.IsNullOrEmpty(filter) || filter == "all")
                {
                    transactions = _transactionRepository.Transactions;
                }
                else
                {
                    if (filter == "hour")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddHours(-1))));
                    }
                    if (filter == "day")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-1))));
                    }
                    if (filter == "week")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-7))));
                    }
                }

            }



            foreach (var item in transactions)
            {
                totalSales += item.TransactionTotal;
            }
            var vm = new ListTransactionsViewModel
            {
                Transactions = transactions,
                TotalSales = totalSales
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(TransactionCheckoutViewModel model)
        {
            //get the lease
            Lease lease = await _leaseRepository.GetLeaseById(model.LeaseId);
            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);

            string transactionType;

            if (lease.UserId == model.ServerId && model.TransactionType != null)
            {
                transactionType = model.TransactionType;
            }
            else
            {
                transactionType = "Cash";
            }

            var totalDays = (lease.leaseTo - lease.leaseFrom).TotalDays;
            decimal transactionTotal = rentalAsset.Price * (decimal)totalDays;

            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    TransactionTotal = transactionTotal,
                    ServerId = model.ServerId,
                    TransactionDate = DateTime.Now,
                    TransactionNotes = model.TransactionNotes,
                    TransactionType = transactionType,
                    LeaseId = lease.LeaseId,
                    VendorUserId = lease.UserId,
                };

                var ActiveLease = new ActiveLease
                { 
                    RentalAssetId = rentalAsset.RentalAssetId,
                    LeaseId = lease.LeaseId
                };


                var result = await _transactionRepository.CreateTransactionAsync(transaction);

                //success 
                if (result > 0)
                {
                    await _rentalAssetRepository.BookAsset(lease.leaseTo, rentalAsset.RentalAssetId);
                    await _activeLeaseRepository.AddActiveLeaseAsync(ActiveLease);
                    await _leaseRepository.RemoveUnPaidLeases();
                }

                return RedirectToAction("CheckoutComplete");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult PostExportExcel()
        {
            var myTranscations = _transactionRepository.Transactions.ToList();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[1, 1].Value = "TransactionId";
                workSheet.Cells[1, 2].Value = "TransactionDate";
                workSheet.Cells[1, 3].Value = "Vendor Name";
                workSheet.Cells[1, 4].Value = "Transaction Total";
                workSheet.Cells[1, 5].Value = "Transaction Type";

                workSheet.Row(1).Height = 20;
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 15;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 16;

                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells["B2:B" + (myTranscations.Count + 1)].Style.Numberformat.Format = "yyyy-mm-dd";

                for (int index = 1; index <= myTranscations.Count; index++)
                {
                    workSheet.Cells[index + 1, 1].Value = myTranscations[index - 1].TransactionId;
                    workSheet.Cells[index + 1, 2].Value = myTranscations[index - 1].TransactionDate;
                    workSheet.Cells[index + 1, 3].Value = myTranscations[index - 1].ServerId;
                    workSheet.Cells[index + 1, 4].Value = myTranscations[index - 1].TransactionTotal;
                    workSheet.Cells[index + 1, 5].Value = myTranscations[index - 1].TransactionType;
                }
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"Transactions-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
            // above I define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransactionDetailAsync(int Id)
        {

            Transaction transaction = await _transactionRepository.GetPurchaseByIdAsync(Id);

            Lease lease = transaction.Lease;

            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);

            VendorUser vendor = await _vendorUserManager.FindByIdAsync(lease.UserId);

            ApplicationUser server = await _userManager.FindByIdAsync(transaction.ServerId);


            var vm = new TransactionDetailViewModel
            {
                RentalAsset = rentalAsset,
                Transaction = transaction,
                VendorUser = vendor,
                Server = server,
                Lease = lease 
            };

            return View(vm);

        }


        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
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









