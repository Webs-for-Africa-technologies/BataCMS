using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Components
{
    public class CheckoutSummary : ViewComponent
    {
        private readonly ICheckoutRepository _checkoutRepository;
        public CheckoutSummary(Checkout checkout, ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public IViewComponentResult Invoke(string viewName = null)
        {
            var items = _checkoutRepository.GetCheckoutItems();

            Checkout checkout = new Checkout { CheckoutItems = items };

            var checkoutViewModel = new CheckoutViewModel
            {
                Checkout = checkout,
                CheckoutTotal = _checkoutRepository.GetCheckoutTotal()
            };

            if (!string.IsNullOrEmpty(viewName))
            {
                return View(viewName, checkoutViewModel);
            }

            return View(checkoutViewModel);
        }
    }
}
