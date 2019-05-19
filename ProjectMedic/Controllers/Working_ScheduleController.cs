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
    public class Working_ScheduleController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Working_Schedule
        public ActionResult Index()
        {
            var working_Schedule = db.Working_Schedule.Include(w => w.Doctor).Include(w => w.Working_Day).Include(w => w.Working_Room).Include(w => w.Working_Time);
            return View(working_Schedule.ToList());
        }

        // GET: Working_Schedule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Schedule working_Schedule = db.Working_Schedule.Find(id);
            if (working_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(working_Schedule);
        }

        // GET: Working_Schedule/Create
        public ActionResult Create()
        {
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "Doctor_ID", "Doctor_Name");
            ViewBag.WorkingDay_ID = new SelectList(db.Working_Day, "WorkingDay_ID", "WorkingDay_Period");
            ViewBag.WorkingRoom_ID = new SelectList(db.Working_Room, "WorkingRoom_ID", "WorkingRoom_Number");
            ViewBag.WorkingTime_ID = new SelectList(db.Working_Time, "WorkingTime_ID", "WoringTime_Period");
            return View();
        }

        // POST: Working_Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkingSchedule_ID,Doctor_ID,WorkingTime_ID,WorkingDay_ID,WorkingRoom_ID,Description,Start,End,ThemeColor,IsFullDay")] Working_Schedule working_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Working_Schedule.Add(working_Schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Doctor_ID = new SelectList(db.Doctors, "Doctor_ID", "Doctor_Name", working_Schedule.Doctor_ID);
            ViewBag.WorkingDay_ID = new SelectList(db.Working_Day, "WorkingDay_ID", "WorkingDay_Period", working_Schedule.WorkingDay_ID);
            ViewBag.WorkingRoom_ID = new SelectList(db.Working_Room, "WorkingRoom_ID", "WorkingRoom_Number", working_Schedule.WorkingRoom_ID);
            ViewBag.WorkingTime_ID = new SelectList(db.Working_Time, "WorkingTime_ID", "WoringTime_Period", working_Schedule.WorkingTime_ID);
            return View(working_Schedule);
        }

        // GET: Working_Schedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Schedule working_Schedule = db.Working_Schedule.Find(id);
            if (working_Schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "Doctor_ID", "Doctor_Name", working_Schedule.Doctor_ID);
            ViewBag.WorkingDay_ID = new SelectList(db.Working_Day, "WorkingDay_ID", "WorkingDay_Period", working_Schedule.WorkingDay_ID);
            ViewBag.WorkingRoom_ID = new SelectList(db.Working_Room, "WorkingRoom_ID", "WorkingRoom_Number", working_Schedule.WorkingRoom_ID);
            ViewBag.WorkingTime_ID = new SelectList(db.Working_Time, "WorkingTime_ID", "WoringTime_Period", working_Schedule.WorkingTime_ID);
            return View(working_Schedule);
        }

        // POST: Working_Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkingSchedule_ID,Doctor_ID,WorkingTime_ID,WorkingDay_ID,WorkingRoom_ID,Description,Start,End,ThemeColor,IsFullDay")] Working_Schedule working_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(working_Schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doctor_ID = new SelectList(db.Doctors, "Doctor_ID", "Doctor_Name", working_Schedule.Doctor_ID);
            ViewBag.WorkingDay_ID = new SelectList(db.Working_Day, "WorkingDay_ID", "WorkingDay_Period", working_Schedule.WorkingDay_ID);
            ViewBag.WorkingRoom_ID = new SelectList(db.Working_Room, "WorkingRoom_ID", "WorkingRoom_Number", working_Schedule.WorkingRoom_ID);
            ViewBag.WorkingTime_ID = new SelectList(db.Working_Time, "WorkingTime_ID", "WoringTime_Period", working_Schedule.WorkingTime_ID);
            return View(working_Schedule);
        }

        // GET: Working_Schedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Schedule working_Schedule = db.Working_Schedule.Find(id);
            if (working_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(working_Schedule);
        }

        // POST: Working_Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Working_Schedule working_Schedule = db.Working_Schedule.Find(id);
            db.Working_Schedule.Remove(working_Schedule);
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
