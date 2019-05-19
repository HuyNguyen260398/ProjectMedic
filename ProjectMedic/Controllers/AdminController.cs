using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectMedic.Models;
using System.Web.Security;

namespace ProjectMedic.Controllers
{
    public class AdminController : Controller
    {
        MediPlusEntities db = new MediPlusEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminLogin login, string ReturnUrl)
        {
            var admin = db.Admins.Where(a => a.Admin_Username == login.Admin_Username).FirstOrDefault();
            if (admin != null)
            {
                if (string.Compare(login.Admin_Password, admin.Admin_Password) == 0)
                {
                    int timeout = login.RememberMe ? 525600 : 1;
                    var ticket = new FormsAuthenticationTicket(login.Admin_Username, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AdminIndex", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("PasswordError", "Incorrect Password!");
                    return View(login);
                }
            }
            else
            {
                ModelState.AddModelError("UsernameError", "Incorrect Username!");
                return View(login);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Admin");
        }
    }
}