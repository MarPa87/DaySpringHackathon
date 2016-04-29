
using System.Collections.Generic;
using DaySpringApp.DataLayer;

namespace DaySpringApp.Abstracts
{
  public interface IDonationRepository
  {
    IEnumerable<Donation> GetDonations(int year, int month = 0);
    void AddDonation(Donation donationData);
    void BulkAddDonation(IEnumerable<Donation> donations);
    void ClearAll();
  }
}