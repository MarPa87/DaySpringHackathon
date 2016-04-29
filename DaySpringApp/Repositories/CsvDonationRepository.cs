using System;
using System.Collections.Generic;
using System.Linq;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;
using Newtonsoft.Json;

namespace DaySpringApp.Repositories
{
  public class CsvDonationRepository : IDonationRepository
  {
    private readonly List<Donation> _donationData;
    private readonly string _donationFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/donations.csv";

    public CsvDonationRepository()
    {
      if (!System.IO.File.Exists(_donationFileName))
      {
        _donationData = new List<Donation>();
        return;
      }
      var content = System.IO.File.ReadAllText(_donationFileName);
      _donationData = JsonConvert.DeserializeObject<List<Donation>>(content);
    }
    
    public IEnumerable<Donation> GetDonations(int year, int month = 0)
    {
      return _donationData.Where(c => c.DonationDate.Year == year && (month == 0 || c.DonationDate.Month == month));
    }

    public void AddDonation(Donation donationData)
    {
      _donationData.Add(donationData);
      SaveToFile();
    }

    public void BulkAddDonation(IEnumerable<Donation> donations)
    {
      _donationData.AddRange(donations);
      SaveToFile();
    }

    public void ClearAll()
    {
      _donationData.Clear();
      SaveToFile();
    }

    private void SaveToFile()
    {
      System.IO.File.WriteAllText(_donationFileName, JsonConvert.SerializeObject(_donationData));
    }
  }
}