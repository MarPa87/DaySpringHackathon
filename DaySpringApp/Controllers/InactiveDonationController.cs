using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;

namespace DaySpringApp.Controllers
{
  public class InactiveDonationController : ApiController
  {
    private readonly IDonationRepository _donationRepository;
    public InactiveDonationController(IDonationRepository donationRepository)
    {
      if (donationRepository == null) throw new ArgumentNullException(nameof(donationRepository));
      _donationRepository = donationRepository;
    }

    // GET api/<controller>
    public IEnumerable<Donation> Get(int days)
    {
      var donations = _donationRepository.GetDonations().ToArray();
      return
        donations.Where(
          c => !donations.Any(d => c.IdNumber == d.IdNumber && d.DonationDate > DateTime.Now.AddDays(-1*days)));
    }
  }
}