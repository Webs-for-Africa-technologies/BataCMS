using BataCMS.Data.Interfaces;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Components
{
    public class PaymentMethod : ViewComponent
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;


        public PaymentMethod(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View();

        }

    }
}
