using BataCMS.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace COHApp.Data.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        public string ApplicantName { get; set; }

        public int ServiceTypeId { get; set; }

        [Required]
        public string ApplicantId { get; set; }

        [Display(Name = "Photo")]
        public string ImageUrl { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Location { get; set; }

        public string RejectMessage { get; set; }

        public string Status { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

