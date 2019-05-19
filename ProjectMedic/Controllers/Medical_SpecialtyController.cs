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
    public class Medical_SpecialtyController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Medical_Specialty
        public ActionResult Index()
        {
            return View(db.Medical_Specialty.ToList());
        }

        // GET: Medical_Specialty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical_Specialty medical_Specialty = db.Medical_Specialty.Find(id);
            if (medical_Specialty == null)
            {
                return HttpNotFound();
            }
            return View(medical_Specialty);
        }

        // GET: Medical_Specialty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medical_Specialty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicalSpecialty_ID,MedicalSpecialty_Name,MedicalSpecialty_ServicePrice")] Medical_Specialty medical_Specialty)
        {
            if (ModelState.IsValid)
            {
                db.Medical_Specialty.Add(medical_Specialty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medical_Specialty);
        }

        // GET: Medical_Specialty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical_Specialty medical_Specialty = db.Medical_Specialty.Find(id);
            if (medical_Specialty == null)
            {
                return HttpNotFound();
            }
            return View(medical_Specialty);
        }

        // POST: Medical_Specialty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicalSpecialty_ID,MedicalSpecialty_Name,MedicalSpecialty_ServicePrice")] Medical_Specialty medical_Specialty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medical_Specialty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medical_Specialty);
        }

        // GET: Medical_Specialty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical_Specialty medical_Specialty = db.Medical_Specialty.Find(id);
            if (medical_Specialty == null)
            {
                return HttpNotFound();
            }
            return View(medical_Specialty);
        }

        // POST: Medical_Specialty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medical_Specialty medical_Specialty = db.Medical_Specialty.Find(id);
            db.Medical_Specialty.Remove(medical_Specialty);
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
