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
        public SchoolDatabaseEntities objSchoolDatabaseEntities = new SchoolDatabaseEntities();
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            var model = new LoginViewModel();
            ViewBag.Title = "Login";

            model.RememberMe = !string.IsNullOrEmpty(model.EmailId);
            return View("Login", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(LoginViewModel model, string returnUrl)
        {
            UserRegistration objUserRegistration = new UserRegistration();
            try
            {
                if (model.EmailId != null && model.Password != null)
                {
                    using (SchoolDatabaseEntities objSchoolDatabaseEntities = new SchoolDatabaseEntities())
                    {
                        //To check EmailId & Password From DB
                        var obj = objSchoolDatabaseEntities.UserRegistrations.Where
                             (u => u.EmailId == model.EmailId && u.Password == model.Password)
                                                      .FirstOrDefault();

                        if (obj != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.EmailId, true);
                            var isAdmin = (from role in objSchoolDatabaseEntities.Roles
                                           join user in objSchoolDatabaseEntities.UserInRoles
                                                                                      on role.RoleId equals user.RoleId
                                           where user.UserId == obj.UserId
                                           select role.RoleName).FirstOrDefault();

                            if (isAdmin == "Super Admin")
                            {
                                Session["RoleId"] = 1;
                                Session["RoleName"] = "SuperAdmin";
                                return RedirectToAction("Welcome", "SuperAdmin");
                            }
                            else if (isAdmin == "Admin")

                            {
                                Session["RoleId"] = 2;
                                Session["RoleName"] = "Admin";
                                return RedirectToAction("Welcome", "Admin");
                            }
                            else if (isAdmin == "Teacher")
                            {
                                Session["User"] = obj;
                                Session["RoleId"] = 3;
                                Session["RoleName"] = "Teacher";
                                return RedirectToAction("Welcome", "Teacher", new { id = obj.UserId });
                            }
                            else if (isAdmin == "Student")
                            {
                                Session["User"] = obj;
                                Session["RoleId"] = 4;
                                Session["RoleName"] = "Student";
                                return RedirectToAction("Welcome", "Student", new { id = obj.UserId });
                            }
                            else
                            {
                                Session["EmailId"] = null;
                                Session["Password"] = null;
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("EmailId", "Email and Password not found or matched");
                            return View(model);
                        }
                    }
                }

                else return View(model);
                {
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0} Login Failed", ex.Message);
                return View();
            }
        }

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

    }
}