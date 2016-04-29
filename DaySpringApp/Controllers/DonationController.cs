using System.Net;
using System.Net.Http;
using System.Web.Http;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;
using DaySpringApp.Repositories;

namespace DaySpringApp.Controllers
{
  public class DonationController : ApiController
  {
    private readonly IDonationRepository _donationRepository = new MockDonationRepository();

    public HttpResponseMessage Get(int year, int month = 0)
    {
      var donations = _donationRepository.GetDonations(year, month);
      return Request.CreateResponse(HttpStatusCode.OK, donations);
    }

    public HttpResponseMessage Post(Donation donation)
    {
      _donationRepository.AddDonation(donation);
      return Request.CreateResponse(donation);
    }
  }
}
