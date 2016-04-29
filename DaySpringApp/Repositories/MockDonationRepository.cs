using System;
using System.Collections.Generic;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;

namespace DaySpringApp.Repositories
{
  public class MockDonationRepository : IDonationRepository
  {
    public IEnumerable<Donation> GetDonations(int year, int month = 0)
    {
      var result = new List<Donation>();
      var rng = new Random((int)DateTime.Now.Ticks);
      for (var i = 1; i < 50; ++i)
      {
        result.Add(new Donation
        {
          IdType = IdType.Nric,
          IdNumber = "S1234567K",
          FirstName = "John",
          LastName = "Doe",
          AddressLine1 = "Sims Ave. 123",
          AddressLine2 = "#12-34",
          AddressLine3 = "2nd unit",
          DonationAmount = rng.Next(5, 9000),
          DonationDate = new DateTime(year, month > 0 ? month : rng.Next(1,13), rng.Next(1,25)),
          PostalCode = rng.Next(100000,999999).ToString(),
          ReceiptNumber = $"REC{rng.Next(1000,9999)}"
        });
      }
      return result;
    }

    public void AddDonation(Donation donationData)
    {
      
    }

    public void BulkAddDonation(IEnumerable<Donation> donations)
    {
    }
  }
}