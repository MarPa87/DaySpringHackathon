using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;

namespace DaySpringApp.Controllers
{
  public class MockDataController : ApiController
  {
    private readonly IDonationRepository _donationRepository;
    public MockDataController(IDonationRepository donationRepository)
    {
      if (donationRepository == null) throw new ArgumentNullException(nameof(donationRepository));
      _donationRepository = donationRepository;
    }
    
    public HttpResponseMessage Get()
    {
      var rng = new Random();
      var donations = new List<Donation>();
      for (var i = 0; i < 20000; ++i)
      {
        var year = rng.Next(2014, 2016);
        var maxMonth = year < DateTime.Now.Year ? 13 : DateTime.Now.Month;

        donations.Add(new Donation
        {
          IdType = IdType.Nric,
          IdNumber = $"S{rng.Next(4000000,4003999)}D",
          FirstName = "John",
          LastName = "Doe",
          AddressLine1 = "Sims Ave. 1",
          AddressLine2 = "#12-34",
          AddressLine3 = "2nd floor",
          DonationDate = new DateTime(year, rng.Next(1, maxMonth), rng.Next(1,28)),
          DonationAmount = rng.Next(5, 1001),
          Email = "lorem@ipsum.com",
          ReceiptNumber = $"REC000{i}",
          Phone = $"{rng.Next(1235,9583)} {rng.Next(1234,9598)}",
          PostalCode = rng.Next(356472,999999).ToString()
        });
      }
      _donationRepository.ClearAll();
      _donationRepository.BulkAddDonation(donations);
      return Request.CreateResponse(HttpStatusCode.OK);
    }
    
  }
}