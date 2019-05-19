using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;
using PagedList;

namespace ProjectMedic.Controllers
{
    public class VisitByDateController : Controller
    {
        MediPlusEntities db = new MediPlusEntities();
        // GET: VisitByDate
        public ActionResult Index()
        {
            if(Session["P_ID"] == null)
            {
                return RedirectToAction("TermOfService", "Home");
            }
            else
            {
                Session["action"] = "selectday";
                return View();
            }
        }

        public ActionResult GetDate(string date, int dayofweek)
        {
            Session["date"] = date;
            switch (dayofweek)
            {
                case 0:
                    Session["dayofweek"] = "Sunday";
                    break;
                case 1:
                    Session["dayofweek"] = "Monday";
                    break;
                case 2:
                    Session["dayofweek"] = "Tuesday";
                    break;
                case 3:
                    Session["dayofweek"] = "Wednesday";
                    break;
                case 4:
                    Session["dayofweek"] = "Thursday";
                    break;
                case 5:
                    Session["dayofweek"] = "Friday";
                    break;
                case 6:
                    Session["dayofweek"] = "Saturday";
                    break;
            }
            return null;
        }

        public ActionResult SelectMedicalSpecialty(int? page_no, string search)
        {
            Session["action"] = "selectms";

            var ms = db.Medical_Specialty.AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                ms = ms.Where(m => m.MedicalSpecialty_Name.ToLower().Contains(search));
            }

            return View(ms.OrderBy(a => a.MedicalSpecialty_Name).ToPagedList(page_no ?? 1, 5));
        }

        public ActionResult SelectDoctor(string ms)
        {
            Session["action"] = "getdoctor";
            var doctors = db.vDoctors.Where(d => d.MedicalSpecialty_Name.Equals(ms));

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in db.Working_Time)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.WoringTime_Period.ToString(),
                    Value = item.WorkingTime_ID.ToString()
                });
            }
            ViewBag.WT = list;

            return View(doctors.ToList());
        }

        public ActionResult SaveShedule()
        {
            return RedirectToAction("");
        }
    }
}