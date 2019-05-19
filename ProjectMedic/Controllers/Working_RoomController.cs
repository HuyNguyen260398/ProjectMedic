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
    public class Working_RoomController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        // GET: Working_Room
        public ActionResult Index()
        {
            return View(db.Working_Room.ToList());
        }

        // GET: Working_Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Room working_Room = db.Working_Room.Find(id);
            if (working_Room == null)
            {
                return HttpNotFound();
            }
            return View(working_Room);
        }

        // GET: Working_Room/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Working_Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkingRoom_ID,WorkingRoom_Number")] Working_Room working_Room)
        {
            if (ModelState.IsValid)
            {
                db.Working_Room.Add(working_Room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(working_Room);
        }

        // GET: Working_Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Room working_Room = db.Working_Room.Find(id);
            if (working_Room == null)
            {
                return HttpNotFound();
            }
            return View(working_Room);
        }

        // POST: Working_Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkingRoom_ID,WorkingRoom_Number")] Working_Room working_Room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(working_Room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(working_Room);
        }

        // GET: Working_Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Working_Room working_Room = db.Working_Room.Find(id);
            if (working_Room == null)
            {
                return HttpNotFound();
            }
            return View(working_Room);
        }

        // POST: Working_Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Working_Room working_Room = db.Working_Room.Find(id);
            db.Working_Room.Remove(working_Room);
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
