using BataCMS.Data.Models;
using System;


namespace COHApp.ViewModels
{
    public class ViewRequestViewModel
    {
        public int ServiceRequestId { get; set; }
        public ApplicationUser User { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string RejectMessage { get; set; }
    }
}
