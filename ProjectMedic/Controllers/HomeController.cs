using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMedic.Controllers
{
    public class HomeController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            Session["action"] = "";
            return View();
        }

        public ActionResult TermOfService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TermOfService(string option)
        {
            if (option != null)
            {
                return RedirectToAction("SignIn", "Patient");
            }
            else
            {
                string message = "You are not angree with the Term of Service!";
                ViewBag.Message = message;
                return View();
            }
        }
        
        
        public ActionResult AdminIndex()
        {
            return View();
        }
    }
}