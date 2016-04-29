using DaySpringApp.DataLayer;
using DaySpringApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaySpringApp.Controllers
{
    public class DashboardController : Controller
    {
        private DaySpringDbEntities db = new DaySpringDbEntities();

        // GET: Dashboard
        [ChildActionOnly]
        public ActionResult DonationByYear(int year)
        {
            var donations = db.Donations.Where(e => e.DonationDate.Year == year)
                                             .ToList()
                                             .GroupBy(e => e.DonationDate.Month)
                                             .OrderBy(e => e.Key)
                                             .Select(e => new DonationByYearModels()
                                             {
                                                 Month = e.First().DonationDate.Month,
                                                 MonthName = e.First().DonationDate.ToString("MMM"),
                                                 Total = e.Sum(f => f.Amount)
                                             })
                                             .ToArray();

            return PartialView("_DonationByYear", donations);
        }
    }
}