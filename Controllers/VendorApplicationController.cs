using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.ViewModels;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using BataCMS.Data;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using COHApp.Data.Models;
using COHApp.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using COHApp.ViewModels;

namespace COHApp.Controllers
{
    public class VendorApplicationController :  Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVendorApplicaitonRepository _vendorApplicationRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public VendorApplicationController(IWebHostEnvironment webHostEnvironment, IVendorApplicaitonRepository vendorApplicaitonRepository, UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _vendorApplicationRepository = vendorApplicaitonRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateVendorApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {

                string IdProofPath = ProcessUploadedImage(model.IdProof);
                string ResidencyProofPath = ProcessUploadedImage(model.ResidencyProof);


                VendorApplication newVendorApplication = new VendorApplication
                {
                    ApplicantName = model.FullName,
                    IdProofUrl = IdProofPath,
                    ResidencyProofUrl = ResidencyProofPath,
                    ApplicantId  = model.ApplicantId,
                    ApplicationDate = DateTime.Today,
                    Status = "Pending"                   
                };

                await _vendorApplicationRepository.AddAsync(newVendorApplication);
                return RedirectToAction("ApplicationComplete");

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int applicationId)
        {
            VendorApplication application = await _vendorApplicationRepository.GetApplicationByIdAsync(applicationId);


            EditApplicationViewModel editApplicationViewModel = new EditApplicationViewModel
            {
                VendorApplicationId = application.VendorApplicationId,
                ExitingIdProofURL = application.IdProofUrl,
                ExistingResidencyProofURL = application.ResidencyProofUrl,
                FullName = application.ApplicantName,
                Status = application.Status,
                RejectMessage = application.RejectMessage,
                ApplicantId = application.ApplicantId
            };
            return View(editApplicationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                VendorApplication application = await _vendorApplicationRepository.GetApplicationByIdAsync(model.VendorApplicationId);

                application.ApplicantName = model.FullName;
                application.ApplicationDate = DateTime.Now;

                //make all application pending after editing
                application.Status = "Pending";
                application.RejectMessage = null;

                if (model.IdProof != null)
                {
                    if (model.ExitingIdProofURL != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath + model.ExitingIdProofURL);
                        System.IO.File.Delete(filePath);
                    }

                    application.IdProofUrl = ProcessUploadedImage(model.IdProof);

                }

                if (model.ResidencyProof != null)
                {
                    if (model.ExistingResidencyProofURL != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath + model.ExistingResidencyProofURL);
                        System.IO.File.Delete(filePath);
                    }

                    application.ResidencyProofUrl = ProcessUploadedImage(model.ResidencyProof);

                }

                await _vendorApplicationRepository.UpdateApplicationAsync(application);
                return RedirectToAction("MyApplications", new { applicantId = application.ApplicantId });
            }
            return View();
        }

        public IActionResult MyApplications(string applicantId)
        {
            IEnumerable<VendorApplication> applications = _vendorApplicationRepository.vendorApplications.Where(p => p.ApplicantId == applicantId);

            var vm = new ListApplicationsViewModel
            {
                Applications = applications
            };
            return View(vm);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult ListApplications(string filter)
        {
            IEnumerable<VendorApplication> applications = null;

            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                applications = _vendorApplicationRepository.vendorApplications.Where(p => p.Status == "Pending");
            }
            else
            {
                if (filter == "hour")
                {
                    applications = _vendorApplicationRepository.vendorApplications.Where((p => p.ApplicationDate >= (DateTime.Now.AddHours(-1)) && p.Status == "Pending"));
                }
                if (filter == "day")
                {
                    applications = _vendorApplicationRepository.vendorApplications.Where((p => p.ApplicationDate >= (DateTime.Now.AddDays(-1)) && p.Status == "Pending"));
                }
                if (filter == "week")
                {
                    applications = _vendorApplicationRepository.vendorApplications.Where((p => p.ApplicationDate >= (DateTime.Now.AddDays(-7)) && p.Status == "Pending"));
                }
            }
            var vm = new ListApplicationsViewModel
            {
                Applications = applications
            };
            return View(vm);
        }

        public async Task<IActionResult> ViewApplicationAsync(int applicationId)
        {

            VendorApplication application = await _vendorApplicationRepository.GetApplicationByIdAsync(applicationId);

            //Currency currency = await _currencyRepository.GetCurrencyByNameAsync(purchase.PaymentMethods.First().PaymentMethodName);
            //PaymentMethod paymentMethod = purchase.PaymentMethods.First();
            ApplicationUser user = await _userManager.FindByIdAsync(application.ApplicantId);


            var vm = new ViewApplicationViewModel
            {
                VendorApplicationId = application.VendorApplicationId,
                Applicant = user,
                IdProofUrl = application.IdProofUrl,
                ResidenceProof = application.ResidencyProofUrl,
                ApplicationDate = application.ApplicationDate,
                RejectMessage = application.RejectMessage
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveApplication(int applicationId)
        {
            VendorApplication application = await _vendorApplicationRepository.GetApplicationByIdAsync(applicationId);
            application.Status = "Approved";

            //Add the user to the VendorRole
            ApplicationUser user = await _userManager.FindByIdAsync(application.ApplicantId);
            await _userManager.RemoveFromRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Vendor");


            await _vendorApplicationRepository.UpdateApplicationAsync(application);
            return RedirectToAction("ListApplications", "VendorApplication");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineApplication(ViewApplicationViewModel model)
        {
            VendorApplication application = await _vendorApplicationRepository.GetApplicationByIdAsync(model.VendorApplicationId);
            application.Status = "Declined";
            application.RejectMessage = model.RejectMessage;

            await _vendorApplicationRepository.UpdateApplicationAsync(application);
            return RedirectToAction("ListApplications", "VendorApplication");
        }

        public IActionResult ApplicationComplete()
        {
            ViewBag.ApplicationComplete = "Your application is done";
            return View();
        }

        private string ProcessUploadedImage(IFormFile Image)
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/applications");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

            }

            string photoPath = "/images/applications/" + uniqueFileName;
            return photoPath;
        }
    }
}
