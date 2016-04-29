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
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InactiveDonor()
        {
            return View();
        }

        public ActionResult TopDonor()
        {
            return View();
        }
    }
}