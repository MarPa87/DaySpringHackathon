using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaySpringApp.Abstracts;

namespace DaySpringApp.Controllers
{
  public class DetailController : Controller
  {
    private readonly IDonationRepository _donationRepository;
    public DetailController(IDonationRepository donationRepository)
    {
      if (donationRepository == null) throw new ArgumentNullException(nameof(donationRepository));
      _donationRepository = donationRepository;
    }

    // GET: Detail
    public ActionResult Index(int year = 0, int month = 0)
    {
      if (year == 0 || month == 0)
      {
        year = DateTime.Now.Year;
        month = DateTime.Now.Month;
      }

      if (year == DateTime.Now.Year && month > DateTime.Now.Month) month = DateTime.Now.Month;
      ViewBag.Year = year;
      ViewBag.Month = month;
      var donations = _donationRepository.GetDonations(year, month);
      return View(donations);
    }

    // GET: Detail/Details/5
    public ActionResult Details(string id)
    {
      return View();
    }

    // GET: Detail/Edit/5
    public ActionResult Edit(string id)
    {
      return View();
    }

    // POST: Detail/Edit/5
    [HttpPost]
    public ActionResult Edit(string id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
