using DaySpringApp.Helpers;
using DaySpringApp.Results;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using DaySpringApp.Abstracts;

namespace DaySpringApp.Controllers
{
  public class IrasController : Controller
  {
    private readonly IDonationRepository _donationRepository;

    public IrasController(IDonationRepository donationRepository)
    {
      if (donationRepository == null) throw new ArgumentNullException(nameof(donationRepository));
      _donationRepository = donationRepository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Export(int year)
    {
      var donations = _donationRepository
                        .GetDonations(year)
                        .ToArray();

      var ipcIdNo = ConfigurationManager.AppSettings["IPC_ID_NO"];
      var ipcAbbrName = ConfigurationManager.AppSettings["IPC_ABBR_NAME"];
      var fileNo = 0;
      var i = 0;

      var result = new ZipResult
      {
        FileName = "DONS-" + year + "-" + ipcAbbrName + ".zip"
      };

      while (i < donations.Length)
      {
        fileNo++;
        using (var exporter = new CsvExporter())
        {
          exporter.Create();

          string[] headers = new string[6];
          headers[0] = "0";
          headers[1] = "7";
          headers[2] = year.ToString();
          headers[3] = "7";
          headers[4] = "0";
          headers[5] = ipcIdNo;
          exporter.WriteLine(headers);

          string[] fields = new string[14];
          int count = 0;
          decimal total = 0;

          for (; i < donations.Length && count < 40000; i++)
          {
            var donation = donations[i];
            count++;
            total += donation.DonationAmount;
            fields[0] = "1"; // Details
            fields[1] = ((int)donation.IdType).ToString();
            fields[2] = donation.IdNumber;
            fields[3] = donation.FirstName;
            fields[4] = donation.LastName;
            fields[5] = donation.AddressLine1;
            fields[6] = donation.AddressLine2;
            fields[7] = donation.AddressLine3;
            fields[8] = donation.PostalCode;
            fields[9] = donation.DonationAmount.ToString();
            fields[10] = donation.DonationDate.ToString("yyyyMMdd");
            fields[11] = donation.ReceiptNumber;
            fields[12] = "O"; // Outright Cash
            fields[13] = "Z"; // Default is Z
            exporter.WriteLine(fields);
          }

          var footers = new string[3];
          footers[0] = "2";
          footers[1] = count.ToString();
          footers[2] = total.ToString(CultureInfo.InvariantCulture);
          exporter.WriteLine(footers);

          var memoryStream = new MemoryStream();
          exporter.Save(memoryStream);
          result.AddEntry("DONS-" + year + "-" + ipcAbbrName + "-" + fileNo.ToString("99") + ".csv", memoryStream.ToArray());
        }
      }

      return result;
    }
  }
}