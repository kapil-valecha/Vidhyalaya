using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class UserLoginController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        /// <summary>
        /// Get method for User Login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        ///  Post method for User Login
        /// </summary>
        /// <param name="objUserRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserRegistration objUserRegistration)
        {
            var userDetails = db.UserRegistrations.Where(x => x.EmailId == objUserRegistration.EmailId && x.Password == objUserRegistration.Password).FirstOrDefault();
            //Code to Authenticate Identity Of user.
            if (userDetails != null)
            {
                if (userDetails.RoleId == 1)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();
                    return RedirectToAction("AllUserDetails", "MainAdmin");
                }
                else if (userDetails.RoleId == 2)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();

                    return RedirectToAction("AllUserDetails", "MainAdmin");
                }
                else if (userDetails.RoleId == 3)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();
                    return RedirectToAction("AllUserDetails", "Teacher");
                }
                else if (userDetails.RoleId == 4)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();
                    return RedirectToAction("AllUserDetails", "Teacher");
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