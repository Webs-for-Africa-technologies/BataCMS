using BataCMS.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class CreateVendorApplicationViewModel
    {
        [Required]
        public string FullName { get; set; }

        public IFormFile IdProof { get; set; }

        public IFormFile ResidencyProof { get; set; }

        public string ApplicantId { get; set; }

    }
}
