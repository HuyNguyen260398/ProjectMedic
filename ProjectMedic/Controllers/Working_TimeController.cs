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
    public class Working_TimeController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Working_Time
        public ActionResult Index()
        {
            return View(db.Working_Time.ToList());
        }

        // GET: Working_Time/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Time working_Time = db.Working_Time.Find(id);
            if (working_Time == null)
            {
                return HttpNotFound();
            }
            return View(working_Time);
        }

        // GET: Working_Time/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Working_Time/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkingTime_ID,WoringTime_Period")] Working_Time working_Time)
        {
            if (ModelState.IsValid)
            {
                db.Working_Time.Add(working_Time);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(working_Time);
        }

        // GET: Working_Time/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Time working_Time = db.Working_Time.Find(id);
            if (working_Time == null)
            {
                return HttpNotFound();
            }
            return View(working_Time);
        }

        // POST: Working_Time/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkingTime_ID,WoringTime_Period")] Working_Time working_Time)
        {
            if (ModelState.IsValid)
            {
                db.Entry(working_Time).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(working_Time);
        }

        // GET: Working_Time/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Time working_Time = db.Working_Time.Find(id);
            if (working_Time == null)
            {
                return HttpNotFound();
            }
            return View(working_Time);
        }

        // POST: Working_Time/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Working_Time working_Time = db.Working_Time.Find(id);
            db.Working_Time.Remove(working_Time);
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
