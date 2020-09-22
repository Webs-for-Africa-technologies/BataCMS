using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Phone Number")]
        public string Number { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }


    }
}
