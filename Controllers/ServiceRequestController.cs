using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class ServiceRequestController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVendorApplicaitonRepository _vendorApplicationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IServiceRequestRepository _oDServiceRequestRepository;
        private readonly IDispatchedServiceRepository _dispatchedServiceRepository;

        public ServiceRequestController(IDispatchedServiceRepository dispatchedServiceRepository, IServiceRequestRepository oDServiceRequestRepository, IServiceTypeRepository serviceTypeRepository, IWebHostEnvironment webHostEnvironment, IVendorApplicaitonRepository vendorApplicaitonRepository, UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _vendorApplicationRepository = vendorApplicaitonRepository;
            _userManager = userManager;
            _serviceTypeRepository = serviceTypeRepository;
            _oDServiceRequestRepository = oDServiceRequestRepository;
            _dispatchedServiceRepository = dispatchedServiceRepository;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateAsync(int serviceType)
        {
            ViewBag.serviceType = serviceType;

            ServiceType service = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceType);

            ViewBag.serviceName = service.ServiceName;
            ViewBag.servicePrice = service.Pricing;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateODServiceRequestViewModel model)
        {
            if (ModelState.IsValid)
            {

                string ImageUrl = ProcessUploadedImage(model.Image);

                ServiceType serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(model.ServiceTypeId);

                ServiceRequest newODServiceRequest = new ServiceRequest
                {
                    ApplicantName = model.FullName,
                    ServiceTypeId = model.ServiceTypeId,
                    Location = model.Location,
                    ImageUrl = ImageUrl,
                    ApplicantId = model.ApplicantId,
                    ApplicationDate = DateTime.Now,
                    Status = "Pending"
                };
                await _oDServiceRequestRepository.AddAsync(newODServiceRequest);
                return RedirectToAction("RequestComplete");

            }
            return View();
        }

        public IActionResult MyRequests(string applicantId)
        {
            IEnumerable<ServiceRequest> serviceRequests =  _oDServiceRequestRepository.ODServiceRequests.Where(p => p.ApplicantId == applicantId);
            return View(serviceRequests);
        }

        public async Task<IActionResult> ViewRequest(int requestId)
        {

            ServiceRequest serviceRequest = await _oDServiceRequestRepository.GetServiceRequestIdAsync(requestId);

            ApplicationUser user = await _userManager.FindByIdAsync(serviceRequest.ApplicantId);

            ServiceType serviceType = await _serviceTypeRepository.GetServiceTypeByIdAsync(serviceRequest.ServiceTypeId);

            var vm = new ViewRequestViewModel
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                User = user,
                ImageUrl = serviceRequest.ImageUrl,
                Location = serviceRequest.Location,
                ApplicationDate = serviceRequest.ApplicationDate,
                RejectMessage = serviceRequest.RejectMessage,
                Status = serviceRequest.Status,
                ServiceName = serviceType.ServiceName
               
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int requestId)
        {
            ServiceRequest serviceRequest = await _oDServiceRequestRepository.GetServiceRequestIdAsync(requestId);

            EditRequestViewModel vm = new EditRequestViewModel
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                ExistingImageURL = serviceRequest.ImageUrl,
                FullName = serviceRequest.ApplicantName,
                Status = serviceRequest.Status,
                RejectMessage = serviceRequest.RejectMessage,
                ApplicantId = serviceRequest.ApplicantId
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceRequest serviceRequest = await _oDServiceRequestRepository.GetServiceRequestIdAsync(model.ServiceRequestId);

                serviceRequest.ApplicantName = model.FullName;
                serviceRequest.ApplicationDate = DateTime.Now;

                //make all application pending after editing
                serviceRequest.Status = "Pending";
                serviceRequest.RejectMessage = null;
                serviceRequest.Location = model.Location;

                if (model.Image != null)
                {
                    if (model.ExistingImageURL != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath + model.ExistingImageURL);
                        System.IO.File.Delete(filePath);
                    }

                    serviceRequest.ImageUrl = ProcessUploadedImage(model.Image);

                }

                await _oDServiceRequestRepository.UpdateRequestAsync(serviceRequest);
                return RedirectToAction("MyRequests", new { applicantId = serviceRequest.ApplicantId });
            }
            return View();
        }

        public IActionResult CancelRequest(int requestId)
        {
            ServiceRequest serviceRequest =  _oDServiceRequestRepository.GetServiceRequestIdAsync(requestId).Result;

            if (serviceRequest != null)
            {
                _oDServiceRequestRepository.RemoveRequest(serviceRequest);
                return RedirectToAction("MyRequests", new { applicantId = serviceRequest.ApplicantId });
            }
            else
            {
                ViewBag.ErrorMessage = $"Error Canceling Request, Request Not Found";
                return View("NotFound");
            }
        }

        [Authorize(Roles = "Employee")]
        public IActionResult ListRequests(string filter)
        {
            IEnumerable<ServiceRequest> serviceRequests = null;

            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                serviceRequests = _oDServiceRequestRepository.ODServiceRequests.Where(p => p.Status == "Pending");
            }
            else
            {
                if (filter == "hour")
                {
                    serviceRequests = _oDServiceRequestRepository.ODServiceRequests.Where((p => p.ApplicationDate >= (DateTime.Now.AddHours(-1)) && p.Status == "Pending"));
                }
                if (filter == "day")
                {
                    serviceRequests = _oDServiceRequestRepository.ODServiceRequests.Where((p => p.ApplicationDate >= (DateTime.Now.AddDays(-1)) && p.Status == "Pending"));
                }
                if (filter == "week")
                {
                    serviceRequests = _oDServiceRequestRepository.ODServiceRequests.Where((p => p.ApplicationDate >= (DateTime.Now.AddDays(-7)) && p.Status == "Pending"));
                }
            }

            return View(serviceRequests);
        }

        [HttpPost]
        public async Task<IActionResult> DeclineRequest(ViewRequestViewModel model)
        {
            ServiceRequest serviceRequest = await _oDServiceRequestRepository.GetServiceRequestIdAsync(model.ServiceRequestId);

            serviceRequest.Status = "Declined";
            serviceRequest.RejectMessage = model.RejectMessage;

            await _oDServiceRequestRepository.UpdateRequestAsync(serviceRequest);
            return RedirectToAction("ListRequests");
        }

        [HttpGet]
        public async Task<IActionResult> DispatchServiceAsync(int requestId, string userId)
        {
            ServiceRequest serviceRequest = await _oDServiceRequestRepository.GetServiceRequestIdAsync(requestId);
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            DispatchedService dispatchedService = new DispatchedService
            {
                ApplicationUserId = user.Id,
                ServiceRequestId = serviceRequest.ServiceRequestId,
                DispatchTime = DateTime.Now
            };

            var Result = await _dispatchedServiceRepository.AddDispatchedServiceAsync(dispatchedService);

            if (Result != null)
            {
                serviceRequest.Status = "Dispatched";
                await _oDServiceRequestRepository.UpdateRequestAsync(serviceRequest); 
                return View("RequestComplete");
            }
            else
            {
                ViewBag.ErrorMessage = $"Error Dispatching the Request";
                return View("NotFound");
            }
        }


        public IActionResult RequestComplete()
        {
            ViewBag.RequestComplete = "Your application is done";
            return View();
        }

        private string ProcessUploadedImage(IFormFile Image)
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ODServiceRequests");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                Image.CopyTo(fileStream);

            }

            string photoPath = "/images/ODServiceRequests/" + uniqueFileName;
            return photoPath;
        }
    }
}
