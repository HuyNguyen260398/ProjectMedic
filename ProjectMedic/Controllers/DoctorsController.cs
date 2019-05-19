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
    public class DoctorsController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Doctors
        public ActionResult Index(int? page)
        {
            var doctors = db.Doctors.Include(d => d.Academic_Title).Include(d => d.Medical_Specialty);
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.AcademicTitle_ID = new SelectList(db.Academic_Title, "AcademicTitle_ID", "AcademicTitle_Code");
            ViewBag.MedicalSpecialty_ID = new SelectList(db.Medical_Specialty, "MedicalSpecialty_ID", "MedicalSpecialty_Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Doctor_ID,Doctor_Name,Doctor_Gender,AcademicTitle_ID,MedicalSpecialty_ID")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AcademicTitle_ID = new SelectList(db.Academic_Title, "AcademicTitle_ID", "AcademicTitle_Code", doctor.AcademicTitle_ID);
            ViewBag.MedicalSpecialty_ID = new SelectList(db.Medical_Specialty, "MedicalSpecialty_ID", "MedicalSpecialty_Name", doctor.MedicalSpecialty_ID);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.AcademicTitle_ID = new SelectList(db.Academic_Title, "AcademicTitle_ID", "AcademicTitle_Code", doctor.AcademicTitle_ID);
            ViewBag.MedicalSpecialty_ID = new SelectList(db.Medical_Specialty, "MedicalSpecialty_ID", "MedicalSpecialty_Name", doctor.MedicalSpecialty_ID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Doctor_ID,Doctor_Name,Doctor_Gender,AcademicTitle_ID,MedicalSpecialty_ID")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AcademicTitle_ID = new SelectList(db.Academic_Title, "AcademicTitle_ID", "AcademicTitle_Code", doctor.AcademicTitle_ID);
            ViewBag.MedicalSpecialty_ID = new SelectList(db.Medical_Specialty, "MedicalSpecialty_ID", "MedicalSpecialty_Name", doctor.MedicalSpecialty_ID);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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
