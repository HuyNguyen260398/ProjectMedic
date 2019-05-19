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
    public class Academic_TitleController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Academic_Title
        public ActionResult Index()
        {
            return View(db.Academic_Title.ToList());
        }

        // GET: Academic_Title/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academic_Title academic_Title = db.Academic_Title.Find(id);
            if (academic_Title == null)
            {
                return HttpNotFound();
            }
            return View(academic_Title);
        }

        // GET: Academic_Title/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Academic_Title/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AcademicTitle_ID,AcademicTitle_Code,AcademicTitle_Means")] Academic_Title academic_Title)
        {
            if (ModelState.IsValid)
            {
                db.Academic_Title.Add(academic_Title);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(academic_Title);
        }

        // GET: Academic_Title/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academic_Title academic_Title = db.Academic_Title.Find(id);
            if (academic_Title == null)
            {
                return HttpNotFound();
            }
            return View(academic_Title);
        }

        // POST: Academic_Title/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AcademicTitle_ID,AcademicTitle_Code,AcademicTitle_Means")] Academic_Title academic_Title)
        {
            if (ModelState.IsValid)
            {
                db.Entry(academic_Title).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(academic_Title);
        }

        // GET: Academic_Title/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academic_Title academic_Title = db.Academic_Title.Find(id);
            if (academic_Title == null)
            {
                return HttpNotFound();
            }
            return View(academic_Title);
        }

        // POST: Academic_Title/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Academic_Title academic_Title = db.Academic_Title.Find(id);
            db.Academic_Title.Remove(academic_Title);
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
