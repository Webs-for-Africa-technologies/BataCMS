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
        private readonly Checkout _checkout;
        public CheckoutSummary(Checkout checkout)
        {
            _checkout = checkout;
        }

        public IViewComponentResult Invoke(string viewName = null)
        {
            var items = _checkout.GetCheckoutItems();
            _checkout.CheckoutItems = items;

            var checkoutViewModel = new CheckoutViewModel
            {
                Checkout = _checkout,
                CheckoutTotal = _checkout.GetCheckoutTotal()
            };

            if (!string.IsNullOrEmpty(viewName))
            {
                return View(viewName, checkoutViewModel);
            }

            return View(checkoutViewModel);
        }
    }
}
