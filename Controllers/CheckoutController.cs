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
        private readonly Checkout _checkout;

        public CheckoutController(IUnitItemRepository unitItemRepository, Checkout checkout)
        {
            _unitItemRepository = unitItemRepository;
            _checkout = checkout;
            
        }
        public ViewResult Index()
        {
            var items = _checkout.GetCheckoutItems();
            _checkout.CheckoutItems = items;

            var cVM = new CheckoutViewModel
            {
                Checkout = _checkout,
                CheckoutTotal = _checkout.GetCheckoutTotal()
          
            };
            return View(cVM);  
        }

        public RedirectToActionResult AddToCheckout(int itemId)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.unitItemId == itemId);

            if(selectedItem != null)
            {
                _checkout.AddItem(selectedItem, 1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromCheckout(int unitItemId)
        {
            var selectedItem = _unitItemRepository.unitItems.FirstOrDefault(p => p.unitItemId == unitItemId);

            if (selectedItem != null)
            {
                _checkout.RemoveItem(selectedItem);
            }

            return RedirectToAction("Index");
        }
    }
}