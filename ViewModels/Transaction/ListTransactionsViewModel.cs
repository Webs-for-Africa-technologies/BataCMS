using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class ListTransactionsViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }

        public decimal TotalSales { get; set; }
    }
}
