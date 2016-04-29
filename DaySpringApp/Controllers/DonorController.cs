using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DaySpringApp.DataLayer;
using DaySpringApp.Models;

namespace DaySpringApp.Controllers
{
    //public class DonorController : Controller
    //{
    //    private DaySpringDbEntities db = new DaySpringDbEntities();

    //    // GET: Donor
    //    public ActionResult Index()
    //    {
    //        return View(db.Donors.Include("IdentificationType").OrderBy(e => e.IdNo).AsQueryable());
    //    }

    //    // GET: Donor/Details/5
    //    public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Donor donor = db.Donors.Find(id);
    //        if (donor == null)
    //        {
    //            return HttpNotFound();
    //        }

    //        var model = new DonorViewModel()
    //        {
    //            DonorId = donor.DonorId,
    //            IdentificationTypeId = donor.IdentificationTypeId,
    //            IdNo = donor.IdNo,
    //            FirstName = donor.FirstName,
    //            LastName = donor.LastName,
    //            AddressLine1 = donor.AddressLine1,
    //            AddressLine2 = donor.AddressLine2,
    //            AddressLine3 = donor.AddressLine3,
    //            PostalCode = donor.PostalCode,
    //            EmailAddress = donor.EmailAddress,
    //            ContactNo = donor.ContactNo
    //        };
    //        return View(model);
    //    }

    //    // GET: Donor/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Donor/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create(DonorViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var donor = new Donor()
    //            {
    //                IdentificationTypeId = model.IdentificationTypeId,
    //                IdNo = model.IdNo,
    //                FirstName = model.FirstName,
    //                LastName = model.LastName,
    //                AddressLine1 = model.AddressLine1,
    //                AddressLine2 = model.AddressLine2,
    //                AddressLine3 = model.AddressLine3,
    //                PostalCode = model.PostalCode,
    //                EmailAddress = model.EmailAddress,
    //                ContactNo = model.ContactNo
    //            };
    //            db.Donors.Add(donor);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(model);
    //    }

    //    // GET: Donor/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Donor donor = db.Donors.Find(id);
    //        if (donor == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        var model = new DonorViewModel()
    //        {
    //            DonorId = donor.DonorId,
    //            IdentificationTypeId = donor.IdentificationTypeId,
    //            IdNo = donor.IdNo,
    //            FirstName = donor.FirstName,
    //            LastName = donor.LastName,
    //            AddressLine1 = donor.AddressLine1,
    //            AddressLine2 = donor.AddressLine2,
    //            AddressLine3 = donor.AddressLine3,
    //            PostalCode = donor.PostalCode,
    //            EmailAddress = donor.EmailAddress,
    //            ContactNo = donor.ContactNo
    //        };
    //        return View(model);
    //    }

    //    // POST: Donor/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit(DonorViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var donor = new Donor()
    //            {
    //                DonorId = model.DonorId,
    //                IdentificationTypeId = model.IdentificationTypeId,
    //                IdNo = model.IdNo,
    //                FirstName = model.FirstName,
    //                LastName = model.LastName,
    //                AddressLine1 = model.AddressLine1,
    //                AddressLine2 = model.AddressLine2,
    //                AddressLine3 = model.AddressLine3,
    //                PostalCode = model.PostalCode,
    //                EmailAddress = model.EmailAddress,
    //                ContactNo = model.ContactNo
    //            };
    //            db.Entry(donor).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(model);
    //    }

    //    // GET: Donor/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        Donor donor = db.Donors.Find(id);
    //        if (donor == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        var model = new DonorViewModel()
    //        {
    //            DonorId = donor.DonorId,
    //            IdentificationTypeId = donor.IdentificationTypeId,
    //            IdNo = donor.IdNo,
    //            FirstName = donor.FirstName,
    //            LastName = donor.LastName,
    //            AddressLine1 = donor.AddressLine1,
    //            AddressLine2 = donor.AddressLine2,
    //            AddressLine3 = donor.AddressLine3,
    //            PostalCode = donor.PostalCode,
    //            EmailAddress = donor.EmailAddress,
    //            ContactNo = donor.ContactNo
    //        };
    //        return View(model);
    //    }

    //    // POST: Donor/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        Donor donor = db.Donors.Find(id);
    //        db.Donors.Remove(donor);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
}
