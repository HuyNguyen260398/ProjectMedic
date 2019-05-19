using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;
using System.Data.Entity;
using PagedList;

namespace ProjectMedic.Controllers
{
    public class VisitByDoctorController : Controller
    {
        MediPlusEntities db = new MediPlusEntities();
        // GET: VisitByDoctor
        public ActionResult Index(int? page_no, string search, string ddl1, string ddl2, string ddl3)
        {
            if (Session["P_ID"] == null)
            {
                return RedirectToAction("TermOfService", "Home");
            }

            Session["action"] = "selectdoctor";

            List<SelectListItem> list1 = new List<SelectListItem>();
            list1.Add(new SelectListItem { Text = "View All", Value = "refresh" });
            list1.Add(new SelectListItem { Text = "Male", Value = "Male" });
            list1.Add(new SelectListItem { Text = "Female", Value = "Female" });

            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "View All", Value = "refresh" });
            foreach (var item in db.Academic_Title)
            {
                list2.Add(new SelectListItem()
                {
                    Text = item.AcademicTitle_Means.ToString(),
                    Value = item.AcademicTitle_Code.ToString()
                });
            }

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "View All", Value = "refresh" });
            foreach (var item in db.Medical_Specialty)
            {
                list3.Add(new SelectListItem()
                {
                    Text = item.MedicalSpecialty_Name.ToString(),
                    Value = item.MedicalSpecialty_Name.ToString()
                });
            }

            ViewBag.Gender = list1;
            ViewBag.AcademicTitle = list2;
            ViewBag.MedicalSpecialty = list3;

            var doctors = db.vDoctors.AsQueryable();
            
            if (!String.IsNullOrEmpty(search))
            {
                doctors = doctors.Where(d => d.Doctor_Name.Contains(search));
            }

            if (ddl1 != null && ddl1 != "refresh")
            {
                doctors = doctors.Where(d => d.Doctor_Gender.Equals(ddl1));
            }

            if (ddl2 != null && ddl2 != "refresh")
            {
                doctors = doctors.Where(d => d.AcademicTitle_Code.Equals(ddl2));
            }

            if (ddl3 != null && ddl3 != "refresh")
            {
                doctors = doctors.Where(d => d.MedicalSpecialty_Name.Equals(ddl3));
            }

            return View(doctors.OrderBy(x => x.Doctor_Name).ToPagedList(page_no ?? 1, 10));
        }

        public ActionResult SelectDay(int id)
        {
            Session["action"] = "selectws";
            ViewBag.ID = id;
            return View();
        }

        public ActionResult GetCalendarData(int id)
        {
            JsonResult result = new JsonResult();

            try
            {
                List<Schedule> data = this.LoadData(id);
                
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return result;
        }

        private List<Schedule> LoadData(int id)
        {
            List<Schedule> lst = new List<Schedule>();

            try
            {
                var events = db.Working_Schedule.Where(a => a.Doctor_ID == id).ToList();
                
                foreach (var item in events)
                {
                    Schedule sch = new Schedule();

                    sch.Id = item.WorkingSchedule_ID;
                    sch.Title = item.Working_Time.WoringTime_Period;
                    sch.Desc = String.Concat(item.Doctor.Medical_Specialty.MedicalSpecialty_Name, " - ", item.Working_Day.WorkingDay_Period, " - ", 
                        item.Working_Time.WoringTime_Period, " - Room ", item.Working_Room.WorkingRoom_Number);
                    sch.Start_Date = item.Start.ToString();
                    sch.End_Date = item.End.ToString();
                    sch.Color = item.ThemeColor;
                    sch.AllDay = item.IsFullDay;

                    lst.Add(sch);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return lst;
        }
    }
}