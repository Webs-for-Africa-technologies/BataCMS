using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class ListPurchaseViewModel
    {
        public IEnumerable<Purchase> Purchases { get; set; }

    }
}
