using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;

namespace ProjectMedic.Controllers
{
    public class AppointmentController : Controller
    {
        MediPlusEntities db = new MediPlusEntities();
        // GET: AppointmentList
        public List<Appointment> getAppoimentList()
        {
            List<Appointment> listApp = Session["Appointment"] as List<Appointment>;
            if (listApp == null)
            {
                listApp = new List<Appointment>();
                Session["Appointment"] = listApp;
            }
            return listApp;
        }

        public ActionResult AddAppointment(int eventID)
        {
            Session["action"] = "viewlist";
            Working_Schedule ws = db.Working_Schedule.SingleOrDefault(a => a.WorkingSchedule_ID == eventID);
            if(Session["date"] == null)
            {
                Session["date"] = ws.Start.Value.ToShortDateString();
            }
            if(Session["dayofweek"] == null)
            {
                Session["dayofweek2"] = ws.Working_Day.WorkingDay_Period.ToString();
            }
            if (ws == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Appointment> listApp = getAppoimentList();
            Appointment app = listApp.Find(a => a.Appointment_ID == eventID);
            if (app == null)
            {
                app = new Appointment(eventID);
                listApp.Add(app);
            }
            return RedirectToAction("Appointment");
        }

        public ActionResult AddSchedule(string ms, string dt, int ddl, int ro, int dt_id)
        {
            Session["action"] = "viewlist";

            string day, start, end;
            var ms_id = db.Medical_Specialty.SingleOrDefault(m => m.MedicalSpecialty_Name.Equals(ms));
            var wt = db.Working_Time.SingleOrDefault(n => n.WorkingTime_ID == ddl);

            if(Session["dayofweek"] != null)
            {
                if (wt.WorkingTime_ID > 6)
                {
                    day = String.Concat(Session["dayofweek"].ToString(), " Morning");
                }
                else
                {
                    day = String.Concat(Session["dayofweek"].ToString(), " Evening");
                }
            }
            else
            {
                day = Session["dayofweek2"].ToString();
            }

            start = String.Concat(Session["date"], " ", wt.WoringTime_Period.Substring(0, 5));
            end = String.Concat(Session["date"], " ", wt.WoringTime_Period.Substring(8, 5));

            var wd = db.Working_Day.SingleOrDefault(d => d.WorkingDay_Period.Equals(day));
            Working_Schedule ws = new Working_Schedule
            {
                Doctor_ID = Convert.ToInt32(dt_id),
                WorkingTime_ID = Convert.ToInt32(ddl),
                WorkingRoom_ID = ro,
                WorkingDay_ID = wd.WorkingDay_ID,
                Start = Convert.ToDateTime(start),
                End = Convert.ToDateTime(end)
            };
            db.Working_Schedule.Add(ws);
            db.SaveChanges();

            List<Appointment> listApp = getAppoimentList();
            Appointment app = listApp.Find(a => a.Medical_Specialty.Equals(ms));
            if (app == null)
            {
                app = new Appointment(ws.WorkingSchedule_ID);
                listApp.Add(app);
            }
            return RedirectToAction("Appointment");
        }

        public ActionResult DeleteAppointment(int appId)
        {
            Working_Schedule ws = db.Working_Schedule.SingleOrDefault(a => a.WorkingSchedule_ID == appId);
            if (ws == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Appointment> listApp = getAppoimentList();
            Appointment app = listApp.SingleOrDefault(i => i.Appointment_ID == appId);
            if (app != null)
            {
                listApp.RemoveAll(c => c.Appointment_ID == appId);
            }
            return RedirectToAction("Appointment");
        }

        [HttpGet]
        public ActionResult Appointment()
        {
            Session["action"] = "viewlist";
            int id = Convert.ToInt32(Session["P_ID"]);

            var patient = db.Patients.SingleOrDefault(a => a.Patient_ID == id);
            ViewBag.Patient = patient;

            List<Appointment> listApp = getAppoimentList();
            Session["Payment"] = TotalMoney();
            return View(listApp);
        }

        public ActionResult AppointmentList()
        {
            Session["action"] = "viewlist";
            List<Appointment> listApp = getAppoimentList();
            return View(listApp);
        }

        public ActionResult Payment()
        {
            Session["action"] = "checkout";

            List<SelectListItem> bhyt = new List<SelectListItem>();
            bhyt.Add(new SelectListItem { Text = "Select Medical Insurance type", Value = "refresh" });
            bhyt.Add(new SelectListItem { Text = "Provided by Tan Phu hospital", Value = "Yes" });
            bhyt.Add(new SelectListItem { Text = "Provided by another hospital", Value = "Yes" });
            bhyt.Add(new SelectListItem { Text = "None above/Do not have", Value = "None" });
            ViewBag.BHYT = bhyt;

            var payment = db.Payment_Method.ToList();
            ViewBag.Payment = payment;

            return View();
        }

        [HttpPost]
        public ActionResult Payment(Medical_Invoice mi, string bhyt, string option)
        {
            string message1 = null;
            string message2 = null;
            string message3 = null;

            List<Appointment> listApp = getAppoimentList();

            List<SelectListItem> bh = new List<SelectListItem>();
            bh.Add(new SelectListItem { Text = "Select Medical Insurance type", Value = "refresh" });
            bh.Add(new SelectListItem { Text = "Provided by Tan Phu hospital", Value = "Yes" });
            bh.Add(new SelectListItem { Text = "Provided by another hospital", Value = "Yes" });
            bh.Add(new SelectListItem { Text = "None above/Do not have", Value = "None" });
            ViewBag.BHYT = bh;

            var payment = db.Payment_Method.ToList();
            ViewBag.Payment = payment;

            if (ModelState.IsValid)
            {
                if (bhyt == "refresh")
                {
                    message1 = "Please choose a Medical Insurance type!";
                    ViewBag.Message1 = message1;
                    return View(mi);
                }

                if (mi.PaymentMethod_ID == 0)
                {
                    message2 = "Please choose a payment method!";
                    ViewBag.Message2 = message2;
                    return View(mi);
                }

                db.Medical_Invoice.Add(mi);
                db.SaveChanges();

                foreach (var item in listApp)
                {
                    Medical_Invoice_Detail mid = new Medical_Invoice_Detail
                    {
                        MedicalInvoice_ID = mi.MedicalInvoice_ID,
                        MedicalSpecialty_ID = item.MS_ID,
                        WorkingSchedule_ID = item.Appointment_ID,
                        HealthInsuranceStatus = bhyt
                    };
                    db.Medical_Invoice_Detail.Add(mid);
                }
                db.SaveChanges();

                var email = db.Patients.SingleOrDefault(p => p.Patient_ID == mi.Patient_ID).Patient_Email;

                SendEmail(email);

                message3 = "Appointment booking succeed. Details of your appointment hae been sent to your provided email: " + email;

                listApp.Clear();
            }
            else
            {
                message3 = "Something went wrong, please try again!";
            }
            ViewBag.Message3 = message3;
            return View(mi);
        }

        public ActionResult Chart()
        {
            var list = db.vAppointments.ToList();
            List<int> repartitions = new List<int>();
            var ms = list.Select(a => a.MedicalSpecialty_Name).Distinct();

            foreach(var item in ms)
            {
                repartitions.Add(list.Count(a => a.MedicalSpecialty_Name == item));
            }

            var rep = repartitions;
            ViewBag.MS = ms;
            ViewBag.REP = repartitions.ToList();
            return View();
        }

        private double TotalMoney()
        {
            double totalMoney = 0;
            List<Appointment> listApp = Session["Appointment"] as List<Appointment>;
            if (listApp != null)
            {
                totalMoney = listApp.Sum(q => q.Price);
            }
            return totalMoney;
        }

        public void SendEmail(string email)
        {
            List<Appointment> listApp = getAppoimentList();

            var fromEmail = new MailAddress("huynggcs16364@fpt.edu.vn");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "asusrog260398";
            string subject = "You have successfully booking an appointment at Tan Phu hospital!";
            string title = "<br/><br/>Your appointment information:" + "<br/><br/>";
            string body = "";
            string form = "";

            foreach (var item in listApp)
            {
                string detail = "<table class=" + "'table table-borderless'" + ">" +
                                    "<tr><td>Medical Specialty: " + item.Medical_Specialty + "</td></tr>" +
                                    "<tr><td>Visit Date: " + item.Date + "</td></tr>" +
                                    "<tr><td>Doctor: " + item.Doctor + "</td></tr>" +
                                    "<tr><td>Room: " + item.Room + "</td></tr>" +
                                    "<tr><td>Serive Price: $ " + String.Format("{0:0,0}", item.Price) + "</td></tr>" +
                                "</table><br/>";
                form = String.Concat(detail);
            }

            body = String.Concat(title, form);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
                smtp.Send(message);
        }
    }
}