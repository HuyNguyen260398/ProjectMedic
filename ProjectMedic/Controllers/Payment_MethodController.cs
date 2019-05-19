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
    public class Payment_MethodController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Payment_Method
        public ActionResult Index()
        {
            return View(db.Payment_Method.ToList());
        }

        // GET: Payment_Method/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment_Method payment_Method = db.Payment_Method.Find(id);
            if (payment_Method == null)
            {
                return HttpNotFound();
            }
            return View(payment_Method);
        }

        // GET: Payment_Method/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment_Method/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentMethod_ID,PaymentMethod_Name,PaymentMethod_Description")] Payment_Method payment_Method)
        {
            if (ModelState.IsValid)
            {
                db.Payment_Method.Add(payment_Method);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment_Method);
        }

        // GET: Payment_Method/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment_Method payment_Method = db.Payment_Method.Find(id);
            if (payment_Method == null)
            {
                return HttpNotFound();
            }
            return View(payment_Method);
        }

        // POST: Payment_Method/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentMethod_ID,PaymentMethod_Name,PaymentMethod_Description")] Payment_Method payment_Method)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment_Method).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment_Method);
        }

        // GET: Payment_Method/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment_Method payment_Method = db.Payment_Method.Find(id);
            if (payment_Method == null)
            {
                return HttpNotFound();
            }
            return View(payment_Method);
        }

        // POST: Payment_Method/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment_Method payment_Method = db.Payment_Method.Find(id);
            db.Payment_Method.Remove(payment_Method);
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
