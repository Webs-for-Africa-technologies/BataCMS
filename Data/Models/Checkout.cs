using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Checkout
    {
        public string CheckoutId { get; set; }

        public List<CheckoutItem> CheckoutItems { get; set; }

    }
}
