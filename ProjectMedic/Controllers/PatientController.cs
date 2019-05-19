using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;
using System.Web.Security;
using System.Data.Entity;
using PagedList;

namespace ProjectMedic.Controllers
{
    public class PatientController : Controller
    {
        MediPlusEntities db = new MediPlusEntities();
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            gender.Add(new SelectListItem { Text = "Female", Value = "Female" });
            ViewBag.Gender = gender;
            return View();
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified, ActivationCode")] Patient patient, string gender)
        {
            bool Status = false;
            string message = "";

            List<SelectListItem> listgender = new List<SelectListItem>();
            listgender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            listgender.Add(new SelectListItem { Text = "Female", Value = "Female" });
            ViewBag.Gender = listgender;

            //Model Validation
            if (ModelState.IsValid)
            {
                #region Email is already exist
                var isEmailExist = IsEmailExist(patient.Patient_Email);
                if (isEmailExist)
                {
                    ModelState.AddModelError("EmailExist", "Email is already exsist!");
                    return View(patient);
                }
                #endregion

                #region Username is already exist
                var isUsernameExist = IsUsernameExist(patient.Patient_Username);
                if (isUsernameExist)
                {
                    ModelState.AddModelError("UsernameExist", "Username is already exist!");
                    return View(patient);
                }
                #endregion

                #region Gender
                patient.Patient_Gender = gender;
                #endregion

                #region Generate activation code
                patient.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password hashing
                patient.Patient_Password = Crypto.Hash(patient.Patient_Password);
                patient.ConfirmPassword = Crypto.Hash(patient.ConfirmPassword);
                #endregion

                patient.IsEmailVerified = false;

                #region Save to database
                db.Patients.Add(patient);
                db.SaveChanges();
                #endregion

                #region Send email to user
                SendVerificationLinkEmail(patient.Patient_Email, patient.ActivationCode.ToString());
                message = " Registration is successfully done! Activation link has been sent to your email: " + patient.Patient_Email;
                Status = true;
                #endregion

            }
            else
            {
                message = "Invalid Request!";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(patient);
        }

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var v = db.Patients.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IsEmailVerified = true;
                db.SaveChanges();
                status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request!";
            }
            ViewBag.Status = status;
            return View();
        }

        //Sign In
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        //Sign In POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(PatientLogin login)
        {
            var patient = db.Patients.Where(a => a.Patient_Username == login.Patient_Username).FirstOrDefault();
            if (patient != null)
            {
                if (string.Compare(Crypto.Hash(login.Patient_Password), patient.Patient_Password) == 0)
                {
                    Session["P_ID"] = patient.Patient_ID.ToString();
                    Session["P_Username"] = patient.Patient_Username.ToString();
                    Session["P_DOB"] = patient.Patient_DoB.Value.ToShortDateString();
                    Session["P_Pass"] = patient.Patient_Password.ToString();
                    Session["P_Fname"] = patient.Patient_Firstname.ToString();
                    Session["P_Lname"] = patient.Patient_Lastname.ToString();
                    Session["P_Email"] = patient.Patient_Email.ToString();
                    Session["P_Gender"] = patient.Patient_Gender.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("PasswordError", "Mật Khẩu không đúng!");
                    return View(login);
                }
            }
            else
            {
                ModelState.AddModelError("UsernameError", "Tài Khoản không đúng!");
                return View(login);
            }
        }

        //Sign Out
        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("SignIn");
        }

        public ActionResult ViewAccount()
        {
            List<SelectListItem> listgender = new List<SelectListItem>();
            listgender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            listgender.Add(new SelectListItem { Text = "Female", Value = "Female" });
            ViewBag.Gender = listgender;

            int id = Convert.ToInt32(Session["P_ID"]);
            var patient = db.Patients.SingleOrDefault(p => p.Patient_ID == id);
            return View(patient);
        }

        [HttpPost]
        public ActionResult EditAccount(Patient patient, string gender)
        {
            List<SelectListItem> listgender = new List<SelectListItem>();
            listgender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            listgender.Add(new SelectListItem { Text = "Female", Value = "Female" });
            ViewBag.Gender = listgender;

            int id = Convert.ToInt32(Session["P_ID"]);
            var p = db.Patients.SingleOrDefault(a => a.Patient_ID == id);

            if (patient.Patient_DoB == null)
            {
                p.Patient_DoB = Convert.ToDateTime(Session["P_DOB"]);
            }
            else
            {
                p.Patient_DoB = patient.Patient_DoB;
            }
            if (patient.Patient_Firstname != null)
            {
                p.Patient_Firstname = patient.Patient_Firstname;
            }
            if (patient.Patient_Lastname != null)
            {
                p.Patient_Lastname = patient.Patient_Lastname;
            }

            p.Patient_Gender = gender;

            if (patient.Patient_IDCardNumber != null)
            {
                p.Patient_IDCardNumber = patient.Patient_IDCardNumber;
            }
            if (patient.Patient_Occupation != null)
            {
                p.Patient_Occupation = patient.Patient_Occupation;
            }
            if (patient.Patient_Nationality != null)
            {
                p.Patient_Nationality = patient.Patient_Nationality;
            }
            if (patient.Patient_Ethnic != null)
            {
                p.Patient_Ethnic = patient.Patient_Ethnic;
            }
            if (patient.Patient_Phone != null)
            {
                p.Patient_Phone = patient.Patient_Phone;
            }
            if (patient.Patient_Email != null)
            {
                p.Patient_Email = patient.Patient_Email;
            }
            if (patient.Patient_Address != null)
            {
                p.Patient_Address = patient.Patient_Address;
            }

            p.Patient_Password = patient.Patient_Password;
            p.ConfirmPassword = patient.ConfirmPassword;

            db.SaveChanges();

            return RedirectToAction("ViewAccount");
        }
        
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Patient patient, string old_pass)
        {
            //string message = null;
            int id = Convert.ToInt32(Session["P_ID"]);
            var p = db.Patients.SingleOrDefault(a => a.Patient_ID == id);

            if(old_pass != "")
            {
                if (string.Compare(Crypto.Hash(old_pass), p.Patient_Password) == 0)
                {
                    if (ModelState.IsValid)
                    {
                        p.Patient_Password = Crypto.Hash(patient.Patient_Password);
                        p.ConfirmPassword = Crypto.Hash(patient.Patient_Password);

                        db.SaveChanges();
                        return PartialView();
                    }
                        
                }
                else
                {
                    //message = "Current Password Not Correct!";
                    ModelState.AddModelError("IncorrectPass", "Curent Password is incorrect!");
                    return PartialView(patient);
                }
            }
            else
            {
                //message = "Current Password Not Correct!!";
                ModelState.AddModelError("Blank", "Current Password Cannot Be Blank!");
                return PartialView(patient);
            }
            return PartialView();
        }

        public ActionResult MedicalInvoiceList(int? page_no)
        {
            int id = Convert.ToInt32(Session["P_ID"]);
            string username = Session["P_Username"].ToString();

            var listMI = db.vInvoices.Where(a => a.Patient_ID == id);
            var listApp = db.vAppointments.Where(a => a.Patient_Username.Equals(username));
            
            ViewBag.ListApp = listApp.ToList();
            return View(listMI.OrderBy(a => a.MedicalInvoice_Date).ToPagedList(page_no ?? 1, 5));
        }

        public ActionResult CancelAppointment(int id)
        {
            var app = db.Medical_Invoice.SingleOrDefault(a => a.MedicalInvoice_ID == id);
            app.Invoice_Status = "Canceled";
            db.SaveChanges();
            return RedirectToAction("MedicalInvoiceList");
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            var p = db.Patients.Where(x => x.Patient_Email == email).FirstOrDefault();
            return p != null;
        }

        [NonAction]
        public bool IsUsernameExist(string username)
        {
            var p = db.Patients.Where(x => x.Patient_Username == username).FirstOrDefault();
            return p != null;
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode)
        {
            var verifyUrl = "/Patient/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("huynguyen260398@gmail.com");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "01233419773";
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>Click the below link to verify your account" + "<br/><br/><a href='" + link + "'>" + link + "</a>";

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