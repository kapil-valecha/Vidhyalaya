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

        // GET: UserLogin

        public ActionResult Login()
        {
            return View();
        }
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
                    return RedirectToAction("AllUserDetails", "Admin");
                    //return RedirectToAction("Index", "SuperAdmin");

                }
                else if (userDetails.RoleId == 2)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();

                    return RedirectToAction("AllUserDetails", "Admin");
                }
                else if (userDetails.RoleId == 3)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();
                    return RedirectToAction("StudentDetails", "Teacher");
                }
                else if (userDetails.RoleId == 4)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.EmailId.ToString();
                    return RedirectToAction("StudentDetails", "Student");
                }
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password is wrong");

            }


            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}