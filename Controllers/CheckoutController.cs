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
        public ViewResult Index()
        {
            var items = _checkoutRepository.GetCheckoutItems();

            Checkout checkout = new Checkout { CheckoutItems = items };

            var cVM = new CheckoutViewModel
            {
                Checkout = checkout,
                CheckoutTotal = _checkoutRepository.GetCheckoutTotal()
          
            };
            return View(cVM);  
        }

        public RedirectToActionResult AddToCheckout(int itemId)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.unitItemId == itemId);

            if(selectedItem != null)
            {
                _checkoutRepository.AddItem(selectedItem, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCheckout(int unitItemId)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.unitItemId == unitItemId);

            if (selectedItem != null)
            {
                _checkoutRepository.RemoveItem(selectedItem);
            }

            return RedirectToAction("Index");
        }
    }
}