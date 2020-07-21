using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class EditUnitItemViewModel : CreateUnitItemViewModel
    {
        public int unitItemId { get; set; }
        public string ExistingImagePath { get; set; }
    }
}
