using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace COHApp.Controllers
{
    public class DispatchedServiceController : Controller
    {
        private readonly IDispatchedServiceRepository _dispatchedServiceRepository;

        public DispatchedServiceController(IDispatchedServiceRepository dispatchedServiceRepository)
        {
            _dispatchedServiceRepository = dispatchedServiceRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult List(string filter)
        {
            IEnumerable<DispatchedService> dispatchedServices = null;

            if (String.IsNullOrEmpty(filter) || filter == "all")
            {
                dispatchedServices = _dispatchedServiceRepository.DispatchedServices;
            }
            else
            {
                if (filter == "hour")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddHours(-1))));
                }
                if (filter == "day")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddDays(-1))));
                }
                if (filter == "week")
                {
                    dispatchedServices = _dispatchedServiceRepository.DispatchedServices.Where((p => p.DispatchTime >= (DateTime.Now.AddDays(-7))));
                }
            }


            return View(dispatchedServices);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(int Id)
        {

            DispatchedService dispatchedService = await _dispatchedServiceRepository.GetServiceByIdAsync(Id);


            return View(dispatchedService);

        }

        [HttpPost]
        public IActionResult PostExportExcel()
        {
            var dispatchedServices = _dispatchedServiceRepository.DispatchedServices.ToList();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[1, 1].Value = "DispatchId";
                workSheet.Cells[1, 2].Value = "Service";
                workSheet.Cells[1, 3].Value = "Location";
                workSheet.Cells[1, 4].Value = "Requested By";
                workSheet.Cells[1, 5].Value = "Requested Date";
                workSheet.Cells[1, 6].Value = "Dispatch Time";
                workSheet.Cells[1, 7].Value = "Price";

                workSheet.Row(1).Height = 20;
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 40;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 16;

                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells["E2:E" + (dispatchedServices.Count + 1)].Style.Numberformat.Format = "yyyy-mm-dd";
                workSheet.Cells["F2:F" + (dispatchedServices.Count + 1)].Style.Numberformat.Format = "yyyy-mm-dd";


                for (int index = 1; index <= dispatchedServices.Count; index++)
                {
                    workSheet.Cells[index + 1, 1].Value = dispatchedServices[index - 1].DispatchedServiceId;
                    workSheet.Cells[index + 1, 2].Value = dispatchedServices[index - 1].ServiceRequest.ServiceType.ServiceName;
                    workSheet.Cells[index + 1, 3].Value = dispatchedServices[index - 1].ServiceRequest.Location;
                    workSheet.Cells[index + 1, 4].Value = dispatchedServices[index - 1].ServiceRequest.ApplicantName;
                    workSheet.Cells[index + 1, 5].Value = dispatchedServices[index - 1].ServiceRequest.ApplicationDate;
                    workSheet.Cells[index + 1, 6].Value = dispatchedServices[index - 1].DispatchTime;
                    workSheet.Cells[index + 1, 7].Value = dispatchedServices[index - 1].ServiceRequest.ServiceType.Pricing;
                }
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"DipatchedServices-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
            // above I define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
