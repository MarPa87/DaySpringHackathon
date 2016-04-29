using DaySpringApp.DataLayer;
using DaySpringApp.Helpers;
using DaySpringApp.Models;
using DaySpringApp.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace DaySpringApp.Controllers
{
    //public class IrasController : Controller
    //{
    //    DaySpringDbEntities db = new DaySpringDbEntities();

    //    // GET: Iras
    //    [HttpGet]
    //    public ActionResult Export()
    //    {
    //        ViewBag.YearDropDownList = new[] { DateTime.Today.AddYears(-1).ToString("yyyy"), DateTime.Today.ToString("yyyy") }
    //            .Select(e => new SelectListItem() { Value = e, Text = e })
    //            .ToList();
    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult Export(ExportModels model)
    //    {
    //        var donations = db.Donations.Where(e => e.DonationType == 2 && e.DonationDate.Year == model.Year)
    //                                         .Include("Donor")
    //                                         .ToArray();

    //        var ipcIdNo = ConfigurationManager.AppSettings["IPC_ID_NO"];
    //        var ipcAbbrName = ConfigurationManager.AppSettings["IPC_ABBR_NAME"];
    //        var fileNo = 0;
    //        var i = 0;

    //        var result = new ZipResult()
    //        {
    //            FileName = "DONS-" + model.Year + "-" + ipcAbbrName + ".zip"
    //        };
            
    //        while (i < donations.Length)
    //        {
    //            fileNo++;
    //            using (var exporter = new CsvExporter())
    //            {
    //                exporter.Create();

    //                string[] headers = new string[6];
    //                headers[0] = "0";
    //                headers[1] = "7";
    //                headers[2] = model.Year.ToString();
    //                headers[3] = "7";
    //                headers[4] = "0";
    //                headers[5] = ipcIdNo;
    //                exporter.WriteLine(headers);

    //                string[] fields = new string[14];
    //                int count = 0;
    //                decimal total = 0;

    //                for (; i < donations.Length && count < 40000; i++)
    //                {
    //                    var donation = donations[i];
    //                    count++;
    //                    total += donation.Amount;
    //                    fields[0] = "1"; // Details
    //                    fields[1] = donation.Donor.IdentificationTypeId;
    //                    fields[2] = donation.Donor.IdNo;
    //                    fields[3] = donation.Donor.FirstName;
    //                    fields[4] = donation.Donor.LastName;
    //                    fields[5] = donation.Donor.AddressLine1;
    //                    fields[6] = donation.Donor.AddressLine2;
    //                    fields[7] = donation.Donor.AddressLine3;
    //                    fields[8] = donation.Donor.PostalCode;
    //                    fields[9] = donation.Amount.ToString();
    //                    fields[10] = donation.DonationDate.ToString("yyyyMMdd");
    //                    fields[11] = string.Empty; // Receipt No.
    //                    fields[12] = "O"; // Outright Cash
    //                    fields[13] = "Z"; // Default is Z
    //                    exporter.WriteLine(fields);
    //                }

    //                string[] footers = new string[3];
    //                footers[0] = "2";
    //                footers[1] = count.ToString();
    //                footers[2] = total.ToString();
    //                exporter.WriteLine(footers);

    //                var memoryStream = new MemoryStream();
    //                exporter.Save(memoryStream);
    //                result.AddEntry("DONS-" + model.Year + "-" + ipcAbbrName + "-" + fileNo.ToString("99") + ".csv", memoryStream.ToArray());
    //            }
    //        }

    //        return result;
    //    }
   // }
}