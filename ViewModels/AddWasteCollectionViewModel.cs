using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class AddWasteCollectionViewModel
    {
        public string ServiceArea { get; set; }

        [Display(Name = "Monday")]
        public bool Mon { get; set; }

        [Display(Name = "Tuesday")]
        public bool Tue { get; set; }

        [Display(Name = "Wednesday")]
        public bool Wed { get; set; }

        [Display(Name = "Thursday")]
        public bool Thur { get; set; }

        [Display(Name = "Friday")]
        public bool Fri { get; set; }

        [Display(Name = "Saturday")]
        public bool Sat { get; set; }

        [Display(Name = "Sunday")]
        public bool Sun { get; set; }
    }
}
