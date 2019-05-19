using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;

namespace ProjectMedic.Controllers
{
    public class Working_DayController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Working_Day
        public ActionResult Index()
        {
            return View(db.Working_Day.ToList());
        }

        // GET: Working_Day/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Day working_Day = db.Working_Day.Find(id);
            if (working_Day == null)
            {
                return HttpNotFound();
            }
            return View(working_Day);
        }

        // GET: Working_Day/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Working_Day/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkingDay_ID,WorkingDay_Period")] Working_Day working_Day)
        {
            if (ModelState.IsValid)
            {
                db.Working_Day.Add(working_Day);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(working_Day);
        }

        // GET: Working_Day/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Day working_Day = db.Working_Day.Find(id);
            if (working_Day == null)
            {
                return HttpNotFound();
            }
            return View(working_Day);
        }

        // POST: Working_Day/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkingDay_ID,WorkingDay_Period")] Working_Day working_Day)
        {
            if (ModelState.IsValid)
            {
                db.Entry(working_Day).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(working_Day);
        }

        // GET: Working_Day/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Day working_Day = db.Working_Day.Find(id);
            if (working_Day == null)
            {
                return HttpNotFound();
            }
            return View(working_Day);
        }

        // POST: Working_Day/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Working_Day working_Day = db.Working_Day.Find(id);
            db.Working_Day.Remove(working_Day);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
