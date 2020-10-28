using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class RentalAssetListViewModel
    {
        public IEnumerable<RentalAsset> RentalAssets { get; set; }

        public string CurrentCategory { get; set; }
    }
}
