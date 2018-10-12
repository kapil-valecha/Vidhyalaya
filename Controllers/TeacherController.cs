using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class TeacherController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        /// <summary>
        /// Get Action Method for all users details
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUserDetails()
        {
            var returnUserDetailsList = db.UserRegistrations.Where(user => user.RoleId != 1 && user.RoleId != 2 && user.RoleId != 3).ToList();
            return View(returnUserDetailsList);
        }
    }
}