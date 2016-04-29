using DaySpringApp.DataLayer;
using DaySpringApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DaySpringApp.Controllers
{
    public class DonationController : ApiController
    {
        DaySpringDbEntities db = new DaySpringDbEntities();

        public HttpResponseMessage Post([FromBody] DonationModels model)
        {
            var currency = db.Currencies.FirstOrDefault(e => e.Code == model.CurrencyCode);
            if (currency == null)
            {
                throw new ApplicationException("Invalid currency code");
            }

            var donor = db.Donors.FirstOrDefault(e => e.IdNo == model.NricNo);
            if (donor == null)
            {
                donor = db.Donors.FirstOrDefault(e => e.IdNo == model.CompanyRegNo);

                if (donor == null)
                {
                    donor = new Donor();
                    db.Donors.Add(donor);
                }
            }

            donor.FillName(model.FullName);
            donor.FillIdNo(model.NricNo, model.CompanyRegNo);
            donor.FillAddress(model.Address);
            donor.EmailAddress = model.EmailAddress;
            donor.ContactNo = model.ContactNo;

            db.SaveChanges();

            var donation = new Donation()
            {
                DonationDate = DateTime.Today,
                DonationType = model.DonationType,
                PaymentType = model.PaymentType,
                CurrencyId = currency.CurrencyId,
                Amount = model.Amount,
                Remarks = model.Remarks,
                DonorId = donor.DonorId
            };

            db.Donations.Add(donation);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
