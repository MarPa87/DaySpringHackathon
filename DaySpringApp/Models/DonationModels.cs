using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaySpringApp.Models
{
    public class DonationModels
    {
        public byte DonationType { get; set; }

        public string FullName { get; set; }

        public string NricNo { get; set; }

        public string CompanyRegNo { get; set; }

        public string Address { get; set; }

        public string EmailAddress { get; set; }

        public string ContactNo { get; set; }

        public byte PaymentType { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public string Remarks { get; set; }
    }
}