using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class CreateODServiceRequestViewModel
    {
        [Required]
        public string FullName { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public string Location { get; set; }

        public int ServiceTypeId { get; set; }

        public string ApplicantId { get; set; }
    }
}
