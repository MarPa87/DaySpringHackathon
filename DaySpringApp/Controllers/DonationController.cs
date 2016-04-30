using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DaySpringApp.Abstracts;
using DaySpringApp.DataLayer;
using Newtonsoft.Json;

namespace DaySpringApp.Controllers
{
  public class DonationController : ApiController
  {
    private readonly IDonationRepository _donationRepository;

    public DonationController(IDonationRepository donationRepository)
    {
      if (donationRepository == null) throw new ArgumentNullException(nameof(donationRepository));
      _donationRepository = donationRepository;
    }

    public HttpResponseMessage Get(int year = 0, int month = 0)
    {
      var donations = _donationRepository.GetDonations(year, month);
      return Request.CreateResponse(HttpStatusCode.OK, donations);
    }

    public HttpResponseMessage Post(Donation donation)
    {
      donation.DonationDate = DateTime.Now.Date;
      donation.ReceiptNumber = GetReceiptNumber();
      _donationRepository.AddDonation(donation);
      var validationError = ValidateDonation(donation);
      return validationError.Any()
        ? Request.CreateResponse(HttpStatusCode.BadRequest, validationError)
        : Request.CreateResponse(HttpStatusCode.Created);
    }

    /// <summary>
    /// TODO: Check for field max length
    /// </summary>
    /// <param name="donation"></param>
    /// <returns></returns>
    private List<string> ValidateDonation(Donation donation)
    {
      var errorMessages = new List<string>();
      if (donation == null)
      {
        errorMessages.Add("Donation data is missing");
        return errorMessages;
      }

      if (!IsValidIdNumber(donation.IdType, donation.IdNumber))
      {
        errorMessages.Add("ID number is invalid");
      }

      if (string.IsNullOrWhiteSpace(donation.FirstName) || string.IsNullOrWhiteSpace(donation.LastName))
      {
        errorMessages.Add("First and last name must be specified");
      }

      if (string.IsNullOrWhiteSpace(donation.Email))
      {
        errorMessages.Add("Email address must be specified");
      }

      if(string.IsNullOrWhiteSpace(donation.Phone))
      {
        errorMessages.Add("Phone number must be specified");
      }

      return errorMessages;
    }

    private bool IsValidIdNumber(IdType type, string number)
    {
      if (number == null || number.Length > 12) return false;
      //Todo: Implement algorithm to check validity
      return true;
    }

    private string GetReceiptNumber()
    {
      return $"REC124235{new Random().Next(100, 999)}";
    }
  }
}
