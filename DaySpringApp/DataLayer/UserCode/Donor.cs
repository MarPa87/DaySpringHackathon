using DaySpringApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaySpringApp.DataLayer
{
    public partial class Donor
    {
        public string IdentificationTypeFormatted
        {
            get
            {
                return string.Format("{0}-{1}", IdentificationTypeId, IdentificationType.Name);
            }
        }

        public string FullNameFormatted
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName).Trim();
            }
        }

        public void FillName(string fullName)
        {
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                // Assumption: no single name more than 30 characters.

                fullName = fullName.Trim().ToUpper();

                int firstStop = fullName.IndexOf(' ');
                if (firstStop > -1)
                {
                    FirstName = fullName.Substring(0, firstStop);
                    LastName = fullName.Substring(firstStop + 1);
                    return;
                }

                FirstName = fullName;
            }
        }

        public void FillIdNo(string nricNo, string companyRegNo)
        {
            if (!string.IsNullOrWhiteSpace(companyRegNo))
            {
                IdNo = companyRegNo.Trim().ToUpper();
                if (IdNo.Length == 10)
                {
                    IdentificationTypeId = "10"; // ITR
                    return;
                }

                char prefix = IdNo[0];
                if (prefix == 'A')
                {
                    IdentificationTypeId = "08"; // ASGD
                    return;
                }

                if (prefix == 'T' || prefix == 'R' || prefix == 'S')
                {
                    IdentificationTypeId = "35"; // UEN-Others
                    return;
                }

                int year;
                if (int.TryParse(IdNo.Substring(0, 4), out year))
                {
                    if (year >= 1800 && year <= DateTime.Today.Year)
                    {
                        IdentificationTypeId = "06"; // UEN-Local Company
                        return;
                    }

                    IdentificationTypeId = "05"; // UEN-Business
                }

                // TODO: unhandled type?
                // IrasIdType = ??
                return;
            }

            if (!string.IsNullOrWhiteSpace(nricNo))
            {
                IdNo = nricNo.Trim().ToUpper();
                char prefix = IdNo[0];
                if (prefix == 'S' || prefix == 'T')
                {
                    IdentificationTypeId = "01"; // NRIC
                    return;
                }

                if (prefix == 'G' || prefix == 'F')
                {
                    IdentificationTypeId = "02"; // FIN
                    return;
                }

                // TODO: unhandled type?
                // IrasIdType = ??
                return;
            }
        }
        
        public void FillAddress(string address)
        {
            GoogleGeocoder geocoder = new GoogleGeocoder();
            var addresses = geocoder.Geocode(address);
            AddressLine1 = (addresses[0] ?? string.Empty).ToUpper();
            AddressLine2 = (addresses[1] ?? string.Empty).ToUpper();
            AddressLine3 = (addresses[2] ?? string.Empty).ToUpper();
            PostalCode = (addresses[3] ?? string.Empty).ToUpper();
        }
    }
}