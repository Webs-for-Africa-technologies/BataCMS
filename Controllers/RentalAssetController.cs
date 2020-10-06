using COHApp.Data.Interfaces;
using BataCMS.Data.Models;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace COHApp.Controllers
{
    public class RentalAssetController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRentalAssetRepository _rentalAssetRepository;
        private readonly IActiveLeaseRepository _activeLeaseRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<ApplicationUser> _userManager;





        public RentalAssetController(IInvoiceRepository invoiceRepository, UserManager<ApplicationUser> userManager, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, IRentalAssetRepository rentalAssetRepository, IImageRepository imageRepository, IActiveLeaseRepository activeLeaseRepository, ILeaseRepository leaseRepository)
        {
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _rentalAssetRepository = rentalAssetRepository;
            _imageRepository = imageRepository;
            _activeLeaseRepository = activeLeaseRepository;
            _leaseRepository = leaseRepository;
            _userManager = userManager;
            _invoiceRepository = invoiceRepository;

        }


        public ViewResult List(string category, string searchString)
        {
            string _category = category;
            IEnumerable<RentalAsset> rentalasset;

            string currentCategory = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                rentalasset = _rentalAssetRepository.rentalAssets.Where(p => p.Name.Contains(searchString));
            }
            else if (!string.IsNullOrEmpty(category))
            {
                rentalasset = _rentalAssetRepository.rentalAssets.Where(p => p.Category.CategoryName.Equals(_category));
                currentCategory = _category;
            }
            else
            {
                rentalasset = _rentalAssetRepository.rentalAssets.OrderBy(p => p.RentalAssetId);
                currentCategory = "All Stalls";
            }

            var vm = new RentalAssetListViewModel
            {
                RentalAssets = rentalasset,
                CurrentCategory = currentCategory
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ViewResult BookedList(string category, string searchString)
        {
            string _category = category;
            IEnumerable<RentalAsset> rentalasset;

            string currentCategory = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                rentalasset = _rentalAssetRepository.rentalAssets.Where(p => p.Name.Contains(searchString)).Where(p => p.IsAvailable == false);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                rentalasset = _rentalAssetRepository.rentalAssets.Where(p => p.Category.CategoryName.Equals(_category)).Where(p => p.IsAvailable == false);
                currentCategory = _category;
            }
            else
            {
                rentalasset = _rentalAssetRepository.rentalAssets.Where(p => p.IsAvailable == false).OrderBy(p => p.RentalAssetId);
                currentCategory = "All Stalls";
            }

            var vm = new RentalAssetListViewModel
            {
                RentalAssets = rentalasset,
                CurrentCategory = currentCategory
            };

            return View(vm);
        }


        public async Task<IActionResult> EndBooking(int id)
        {

            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(id);

            ActiveLease activeLease =  _activeLeaseRepository.GetActiveLeaseByAssetId(rentalAsset.RentalAssetId);

            Lease lease = await _leaseRepository.GetLeaseById(activeLease.LeaseId);

            ApplicationUser user = await _userManager.FindByIdAsync(lease.UserId);

            if (rentalAsset == null)
            {
                ViewBag.ErrorMessage = $"The rental asset id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    if (rentalAsset.BookTillDate < DateTime.Now)
                    {
                        // Calculate amount paid

                        var totalDays = (lease.leaseTo - lease.leaseFrom).TotalDays;
                        decimal AmountPaid = rentalAsset.Price * (decimal)totalDays;

                        //Add invoice 
                        var invoice = new Invoice 
                        {
                            RentalAssetId = rentalAsset.RentalAssetId,
                            LeaseFrom = lease.leaseFrom,
                            LeaseTo = lease.leaseTo,
                            ApplicationId = user.Id,
                            AmountPaid = AmountPaid
                        };

                       await _invoiceRepository.AddInvoice(invoice);
                        
                    }
                    await _rentalAssetRepository.EndBooking(id);
                    await _activeLeaseRepository.RemoveLease(activeLease);
                    return RedirectToAction("BookedList");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }   
            }
        }



        [HttpGet]
        public async Task<IActionResult> ViewAsync(int itemId)
        {
            RentalAsset unitItem = await _rentalAssetRepository.GetItemByIdAsync(itemId);
            return View(unitItem);
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["categories"] = _categoryRepository.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryName,
                Text = x.CategoryName
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRentalAssetViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Image> assetImages = new List<Image>();
                String mainPhotoPath = null;


                if (model.Images.Count > 1)
                {
                    for (int i = 0; i < model.Images.Count; i++)
                    {
                        if (i == 0)
                        {
                            mainPhotoPath = ProcessUploadedImage(model.Images[i]);
                            continue;
                        }

                        string photoPath = ProcessUploadedImage(model.Images[i]);
                        Image image = new Image { ImageName = model.Images[i].FileName, ImageUrl = photoPath };
                        assetImages.Add(image);

                    }

                }

                var category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == model.Category);

                RentalAsset newRentalAsset = new RentalAsset
                {
                    Name = model.Name,
                    Price = model.Price,
                    IsAvailable = model.IsAvailable,
                    DateModified = DateTime.Today,
                    CategoryId = category.CategoryId,
                    ImageUrl = mainPhotoPath,
                    Description = model.Description,
                    Images = assetImages
                };

                await _rentalAssetRepository.AddAsync(newRentalAsset);
                return RedirectToAction("List", new { id = newRentalAsset.RentalAssetId });

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            this.ViewData["categories"] = _categoryRepository.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryName,
                Text = x.CategoryName
            }).ToList();

            RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(id);
            Category category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryId == rentalAsset.CategoryId);

            EditRentalAssetViewModel editRentalAssetViewModel = new EditRentalAssetViewModel
            {
                RentalAssetId = rentalAsset.RentalAssetId,
                Name = rentalAsset.Name,
                Price = rentalAsset.Price,
                Location = rentalAsset.Location,
                IsAvailable = rentalAsset.IsAvailable,
                Category = category.CategoryName,
                ExistingImagePath = rentalAsset.ImageUrl,
                Description = rentalAsset.Description,
                ExistingImages = rentalAsset.Images
            };
            return View(editRentalAssetViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditRentalAssetViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Image> assetImages = new List<Image>();
                List<Image> deleteImages = new List<Image>();
                RentalAsset rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(model.RentalAssetId);
                Category category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == model.Category);


                rentalAsset.Name = model.Name;
                rentalAsset.Price = model.Price;
                rentalAsset.Location = model.Location;
                rentalAsset.CategoryId = category.CategoryId;
                rentalAsset.IsAvailable = model.IsAvailable;
                rentalAsset.DateModified = DateTime.Today;
                rentalAsset.Description = model.Description;

                if (model.Images != null)
                {

                    if (model.Images.Count > 1)
                    {
                        if (model.ExistingImagePath != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath + model.ExistingImagePath);
                            System.IO.File.Delete(filePath);
                        }
                        foreach (var item in rentalAsset.Images)
                        {
                            deleteImages.Add(item);
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath + item.ImageUrl);
                            System.IO.File.Delete(filePath);
                        }

                        for (int i = 0; i < model.Images.Count; i++)
                        {
                            if (i == 0)
                            {
                                rentalAsset.ImageUrl = ProcessUploadedImage(model.Images[i]);
                                continue;
                            }

                            string photoPath = ProcessUploadedImage(model.Images[i]);
                            Image image = new Image { ImageName = model.Images[i].FileName, ImageUrl = photoPath };
                            assetImages.Add(image);

                        }
                    }

                    rentalAsset.Images = assetImages;
                }

                await _rentalAssetRepository.EditItemAsync(rentalAsset);

                foreach (var item in deleteImages)
                {
                    _imageRepository.DeleteImage(item);
                }

                return RedirectToAction("List");

            }
            return View();
        }

        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var unitItem = _rentalAssetRepository.GetItemByIdAsync(id);

            if (unitItem == null)
            {
                ViewBag.ErrorMessage = $"Item with  id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _rentalAssetRepository.DeleteItem(id);

                //success
                if (result > 0)
                {
                    return RedirectToAction("List");
                }
                return View("List");

            }
        }


        private string ProcessUploadedImage(IFormFile Image)
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/rentalAssets");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                Image.CopyTo(fileStream);

            }

            string photoPath = "/images/rentalAssets/" + uniqueFileName;
            return photoPath;
        }

    }
}
