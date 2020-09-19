using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BataCMS.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IUnitItemRepository _unitItemRepository;
        private readonly ICheckoutRepository _checkoutRepository;

        public CheckoutController(IUnitItemRepository unitItemRepository, ICheckoutRepository checkoutRepository)
        {
            _unitItemRepository = unitItemRepository;
            _checkoutRepository = checkoutRepository;
        }
        public async Task<ViewResult> IndexAsync()
        {
            var items = await _checkoutRepository.GetCheckoutItemsAsync();

            Checkout checkout = new Checkout { CheckoutItems = items };

            var cVM = new CheckoutViewModel
            {
                Checkout = checkout,
                CheckoutTotal = _checkoutRepository.GetCheckoutTotal()
          
            };
            return View(cVM);  
        }

        public async Task<RedirectToActionResult> AddToCheckoutAsync(int itemId, string selectedOptions)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.RentalAssetId == itemId);

            if (selectedItem != null)
            {
                await _checkoutRepository.AddItemAsync(selectedItem, 1, selectedOptions);
            }
            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> RemoveFromCheckoutAsync(int unitItemId)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.RentalAssetId == unitItemId);

            if (selectedItem != null)
            {
                await _checkoutRepository.RemoveItemAsync(selectedItem);
            }

            return RedirectToAction("Index");
        }
    }
}