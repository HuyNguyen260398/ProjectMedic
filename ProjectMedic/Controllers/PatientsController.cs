using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;
using PagedList;

namespace ProjectMedic.Controllers
{
    public class PatientsController : Controller
    {
        private MediPlusEntities db = new MediPlusEntities();

        [HttpGet]
        public ActionResult AllAppointment(string option, string dateoption, string search, int? pageNumber, string sort, DateTime? date, string ddl1, string ddl2, string ddl3)
        {
            List<SelectListItem> list1 = new List<SelectListItem>();
            list1.Add(new SelectListItem { Text = "Payment Method", Value = "refresh" });
            foreach (var item in db.Payment_Method)
            {
                list1.Add(new SelectListItem()
                {
                    Text = item.PaymentMethod_Name.ToString(),
                    Value = item.PaymentMethod_Name.ToString()
                });
            }

            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Invoice Status", Value = "refresh" });
            list2.Add(new SelectListItem { Text = "Waiting", Value = "Waiting" });
            list2.Add(new SelectListItem { Text = "Visited", Value = "Visited" });
            list2.Add(new SelectListItem { Text = "Canceled", Value = "Canceled" });
            list2.Add(new SelectListItem { Text = "Missed", Value = "Missed" });

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "Payment Status", Value = "refresh" });
            list3.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            list3.Add(new SelectListItem { Text = "Done", Value = "Done" });

            ViewBag.PM = list1;
            ViewBag.IS = list2;
            ViewBag.PS = list3;

            ViewBag.SortByPatient = string.IsNullOrEmpty(sort) ? "sort patient" : "";
            ViewBag.SortByDate = string.IsNullOrEmpty(sort) ? "sort date" : "";
            ViewBag.SortByPayment = string.IsNullOrEmpty(sort) ? "sort payment" : "";
            ViewBag.SortByIStatus = string.IsNullOrEmpty(sort) ? "sort istatus" : "";
            ViewBag.SortByPStatus = string.IsNullOrEmpty(sort) ? "sort pstatus" : "";

            var records = db.vInvoices.AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                records = records.Where(d => d.Patient_Firstname.Contains(search));
            }

            if (ddl1 != null && ddl1 != "refresh")
            {
                records = records.Where(d => d.PaymentMethod_Name.Equals(ddl1));
            }

            if (ddl2 != null && ddl2 != "refresh")
            {
                records = records.Where(d => d.Invoice_Status.Equals(ddl2));
            }

            if (ddl3 != null && ddl3 != "refresh")
            {
                records = records.Where(d => d.Payment_Status.Equals(ddl3));
            }

            if (!String.IsNullOrEmpty(date.ToString()))
            {
                records = records.Where(d => d.MedicalInvoice_Date == date);
            }

            switch (sort)
            {
                case "sort patient":
                    records = records.OrderBy(x => x.Patient_Lastname);
                    break;

                case "sort date":
                    records = records.OrderBy(x => x.MedicalInvoice_Date.ToString());
                    break;

                case "sort payment":
                    records = records.OrderByDescending(x => x.PaymentMethod_Name);
                    break;

                case "sort istatus":
                    records = records.OrderBy(x => x.Invoice_Status.ToString());
                    break;

                case "sort pstatus":
                    records = records.OrderBy(x => x.Payment_Status.ToString());
                    break;

                default:
                    records = records.OrderByDescending(x => x.MedicalInvoice_Date.ToString());
                    break;
            }

            return View(records.ToPagedList(pageNumber ?? 1, 10));
        }

        [HttpGet]
        public ActionResult EditAppointment(int id)
        {
            List<SelectListItem> list1 = new List<SelectListItem>();
            list1.Add(new SelectListItem { Text = "Payment Method", Value = "refresh" });
            foreach (var item in db.Payment_Method)
            {
                list1.Add(new SelectListItem()
                {
                    Text = item.PaymentMethod_Name.ToString(),
                    Value = item.PaymentMethod_Name.ToString()
                });
            }

            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Invoice Status", Value = "refresh" });
            list2.Add(new SelectListItem { Text = "Waiting", Value = "Waiting" });
            list2.Add(new SelectListItem { Text = "Visited", Value = "Visited" });
            list2.Add(new SelectListItem { Text = "Canceled", Value = "Canceled" });
            list2.Add(new SelectListItem { Text = "Missed", Value = "Missed" });

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "Payment Status", Value = "refresh" });
            list3.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            list3.Add(new SelectListItem { Text = "Done", Value = "Done" });

            ViewBag.PM = list1;
            ViewBag.IS = list2;
            ViewBag.PS = list3;

            var app = db.vInvoices.SingleOrDefault(a => a.MedicalInvoice_ID == id);
            var lissApp = db.vAppointments.Where(b => b.MedicalInvoice_ID == id).ToList();
            ViewBag.ListApp = lissApp;
            return View(app);
        }
        [HttpPost]
        public ActionResult EditAppointment(int id, string ddl2, string ddl3)
        {

            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Invoice Status", Value = "refresh" });
            list2.Add(new SelectListItem { Text = "Waiting", Value = "Waiting" });
            list2.Add(new SelectListItem { Text = "Visited", Value = "Visited" });
            list2.Add(new SelectListItem { Text = "Canceled", Value = "Canceled" });
            list2.Add(new SelectListItem { Text = "Missed", Value = "Missed" });

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "Payment Status", Value = "refresh" });
            list3.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            list3.Add(new SelectListItem { Text = "Done", Value = "Done" });

            ViewBag.IS = list2;
            ViewBag.PS = list3;

            var lissApp = db.vAppointments.Where(b => b.MedicalInvoice_ID == id).ToList();
            ViewBag.ListApp = lissApp;

            var item = db.Medical_Invoice.SingleOrDefault(a => a.MedicalInvoice_ID == id);
            var invoice = db.vInvoices.SingleOrDefault(a => a.MedicalInvoice_ID == id);

            if (ddl2 != null && ddl2 != "refresh")
            {
                item.Invoice_Status = ddl2;
            }

            if (ddl3 != null && ddl3 != "refresh")
            {
                item.Payment_Status = ddl3;
            }
            db.SaveChanges();
            return RedirectToAction("AllAppointment");
        }

    }
}
