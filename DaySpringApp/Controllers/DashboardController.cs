using DaySpringApp.DataLayer;
using DaySpringApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaySpringApp.Abstracts;
using DaySpringApp.Repositories;

namespace DaySpringApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDonationRepository _donationRepository = new MockDonationRepository();

        // GET: Dashboard
        [ChildActionOnly]
        public ActionResult Donation(int year)
        {
            //var donations = _donationRepository.GetDonations(year);
            //return PartialView("_DonationByYear", donations);
            ViewBag.Year = year;
            return PartialView("_Donation");
        }
    }
}