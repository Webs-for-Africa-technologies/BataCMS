using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.ViewModels
{
    public class UnitItemListViewModel
    {
        public IEnumerable<RentalAsset> UnitItems { get; set; }

        public string CurrentCategory { get; set; }
    }
}
