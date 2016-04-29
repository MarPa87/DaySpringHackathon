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
      for (var i = 0; i < 10000; ++i)
      {
        donations.Add(new Donation
        {
          IdType = IdType.Nric,
          IdNumber = $"S{rng.Next(1000000,9999999)}D",
          FirstName = "John",
          LastName = "Doe",
          AddressLine1 = "Sims Ave. 1",
          AddressLine2 = "#12-34",
          AddressLine3 = "2nd floor",
          DonationDate = new DateTime(rng.Next(2013,2015), rng.Next(1,12), rng.Next(1,28)),
          DonationAmount = rng.Next(5, 1000),
          Email = "lorem@ipsum.com",
          ReceiptNumber = $"REC{i}",
          Phone = "6123 4567",
          PostalCode = rng.Next(100000,999999).ToString()
        });
      }
      _donationRepository.BulkAddDonation(donations);
      return Request.CreateResponse(HttpStatusCode.OK);
    }
    
  }
}