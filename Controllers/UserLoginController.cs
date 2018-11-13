using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class UserLoginController : Controller
    {
        public SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        /// <summary>
        /// Get method for User Login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
                var model = new LoginViewModel();
                ViewBag.Title = "Login";
                model.EmailId = CheckLoginCookie();
                model.RememberMe = !string.IsNullOrEmpty(model.EmailId);
                return View("Login", model);

            }
        [HttpGet]
        private string CheckLoginCookie()
        {
            if (Request.Cookies.Get("EmailId") != null)
            {
                return Request.Cookies["EmailId"].Value;
            }
            return string.Empty;
        }
        
        /// <summary>
        ///  Post method for User Login
        /// </summary>
        /// <param name="objUserRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl )
        {
            if (model.RememberMe)
            {
                HttpCookie ckEmail = new HttpCookie("Email");
                ckEmail.Expires = DateTime.Now.AddDays(30);
                ckEmail.Value = model.EmailId;
                Response.Cookies.Add(ckEmail);
            }

            var userDetails = db.UserRegistrations.Where(x => x.EmailId == model.EmailId && x.Password == model.Password).FirstOrDefault();
            //Code to Authenticate Identity Of user.
            if (userDetails != null)
            {
                Session["UserId"] = userDetails.UserId.ToString();
                Session["UserName"] = userDetails.EmailId.ToString();

                if (userDetails.RoleId == 1)
                {
                    return RedirectToAction("Welcome", "SuperAdmin");
                }
                else if (userDetails.RoleId == 2)
                {
                    return RedirectToAction("Welcome", "Admin");
                }
                else if (userDetails.RoleId == 3)
                {
                    Session["User"] = userDetails;
                    return RedirectToAction("Welcome", "Teacher");
                }
                else if (userDetails.RoleId == 4)
                {
                    Session["User"] = userDetails;

                    return RedirectToAction("Welcome", "Student");
                }
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password is wrong");
            }
            return View();
        }

        /// <summary>
        /// for logging out current user
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
          
                Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                Session.Abandon();
                Session.Clear();
                Response.Cookies.Clear();
                Session.RemoveAll();
                Session["Login"] = null;
                FormsAuthentication.SignOut();
           return RedirectToAction("Login", "UserLogin");
        }

        /// <summary>
        /// Thankyou page
        /// </summary>
        /// <returns></returns>
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}