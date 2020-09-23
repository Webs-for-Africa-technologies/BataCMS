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

namespace COHApp.Controllers
{
    public class VendorApplicationController :  Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVendorApplicaitonRepository _vendorApplicationRepository;


        public VendorApplicationController(IWebHostEnvironment webHostEnvironment, IVendorApplicaitonRepository vendorApplicaitonRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _vendorApplicationRepository = vendorApplicaitonRepository; 
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

                string IdProofPath = ProcessUploadedImage(model.IdProof, model.FullName);
                string ResidencyProofPath = ProcessUploadedImage(model.ResidencyProof, model.FullName);


                VendorApplication newVendorApplication = new VendorApplication
                {
                    ApplicantName = model.FullName,
                    IdProofUrl = IdProofPath,
                    ResidencyProofUrl = ResidencyProofPath,
                    ApplicationDate = DateTime.Today,
                    Status = "Pending"                   
                };

                await _vendorApplicationRepository.AddAsync(newVendorApplication);
                return RedirectToAction("ApplicationComplete");

            }
            return View();
        }

        public IActionResult ApplicationComplete()
        {
            ViewBag.ApplicationComplete = "Your application is done";
            return View();
        }

        private string ProcessUploadedImage(IFormFile Image, string Owner)
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

            string photoPath = "/images/applications" + uniqueFileName;
            return photoPath;
        }
    }
}
