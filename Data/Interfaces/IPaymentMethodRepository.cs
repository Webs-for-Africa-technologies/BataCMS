﻿using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IPaymentMethodRepository
    {
        void CreatePaymentMethod(PaymentMethod paymentMethod, Transaction purchase);

        PaymentMethod GetPaymentMethodById(int paymentMethodId);

        PaymentMethod GetMethodByPurchaseId(int purchaseId);

        Task UpdatePaymentMethod(PaymentMethod paymentMethod);
    }
}
