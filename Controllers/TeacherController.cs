using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        public ActionResult AllStudentDetails()
        {
            var returnStudentDetailsList = db.UserRegistrations.Where(user => user.RoleId != 1 && user.RoleId != 2 && user.RoleId != 3).ToList();
            return View(returnStudentDetailsList);
        }

        public ActionResult TeacherProfile()
        {
            //for logged in Teacher.
            UserRegistration objUser = (UserRegistration)Session["User"];
            var usr = db.UserRegistrations.Find(objUser.UserId);
            if (Session["User"] != null)
            {
                var userDetails = db.UserRegistrations.Where(user => user.UserId == objUser.UserId);
                if (usr != null)
                    return View(userDetails);
            }
            return View(usr);
        }

        /// <summary>
        /// Action for Edit Existing User Get method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(int id)
        {
            //for roles
            //List<Role> objRoleList = GetRoles();
            //ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            //for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");
            //for country
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            //for state
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");
            //for city
            List<City> citiesList = db.Cities.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "CityName");

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserRegistration objUserRegistration = db.UserRegistrations.Find(id);

            var data = from p in db.UserRegistrations where p.UserId == id select p;
            UserRegistrationViewModel objUserRegistrationViewModel = new UserRegistrationViewModel
            {
                UserId = objUserRegistration.UserId,
                FirstName = objUserRegistration.FirstName,
                LastName = objUserRegistration.LastName,
                Password = objUserRegistration.Password,
                EmailId = objUserRegistration.EmailId,
                IsEmailVerified = objUserRegistration.IsEmailVerified,
                Gender = objUserRegistration.Gender,
                DOB = objUserRegistration.DOB,
                Hobby = objUserRegistration.Hobby,
                CourseId = objUserRegistration.CourseId,
                //RoleId = objUserRegistration.RoleId,
                CountryId = objUserRegistration.Address.CountryId,
                StateId = objUserRegistration.Address.StateId,
                CityId = objUserRegistration.Address.CityId,
                Pincode = objUserRegistration.Address.Pincode,
                AddAddressTextBox1 = objUserRegistration.Address.AddressTextBox1,
                AddAddressTextBox2 = objUserRegistration.Address.AddressTextBox2,
                IsActive = objUserRegistration.IsActive,
                DateCreated = objUserRegistration.DateCreated,
                DateModified = objUserRegistration.DateModified
            };
            if (objUserRegistration == null)
            {
                return HttpNotFound();
            }
            return View(objUserRegistrationViewModel);
        }

        /// <summary>
        /// Action for Edit Existing User Post method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objUserRegistrationViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, UserRegistrationViewModel objUserRegistrationViewModel)
        {
            //for roles
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            //for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            //for country
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            //for country
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");
            //for city    
            List<City> citiesList = db.Cities.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "CityName");

            try
            {
                UserRegistration userData = db.UserRegistrations.Find(objUserRegistrationViewModel.UserId);
                if (ModelState.IsValid)
                {
                    userData.FirstName = objUserRegistrationViewModel.FirstName;
                    userData.LastName = objUserRegistrationViewModel.LastName;
                    userData.Gender = objUserRegistrationViewModel.Gender;
                    userData.Hobby = objUserRegistrationViewModel.Hobby;
                    userData.EmailId = objUserRegistrationViewModel.EmailId;
                    userData.Password = objUserRegistrationViewModel.Password;
                    userData.DOB = objUserRegistrationViewModel.DOB;
                    userData.RoleId = objUserRegistrationViewModel.RoleId;
                    userData.Address.CountryId = objUserRegistrationViewModel.CountryId;
                    userData.Address.StateId = objUserRegistrationViewModel.StateId;
                    userData.Address.CityId = objUserRegistrationViewModel.CityId;
                    userData.Address.Pincode = objUserRegistrationViewModel.Pincode;
                    userData.Address.AddressTextBox1 = objUserRegistrationViewModel.AddAddressTextBox1;
                    userData.Address.AddressTextBox2 = objUserRegistrationViewModel.AddAddressTextBox2;
                    userData.IsActive = objUserRegistrationViewModel.IsActive;
                    userData.DateModified = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("AllUserDetails");

                }
                return View(objUserRegistrationViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult StudentDetails(int id)
        {
            //for role
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            // for countries
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            //for state
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");
            //for city
            List<City> citiesList = db.Cities.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "CityName");

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserRegistration objUserRegistration = db.UserRegistrations.Find(id);

            UserRegistrationViewModel objUserRegistrationViewModel = new UserRegistrationViewModel();
            objUserRegistrationViewModel.FirstName = objUserRegistration.FirstName;
            objUserRegistrationViewModel.LastName = objUserRegistration.LastName;
            objUserRegistrationViewModel.Gender = objUserRegistration.Gender;
            objUserRegistrationViewModel.Hobby = objUserRegistration.Hobby;
            objUserRegistrationViewModel.EmailId = objUserRegistration.EmailId;
            objUserRegistrationViewModel.Password = objUserRegistration.Password;
            objUserRegistrationViewModel.DOB = objUserRegistration.DOB;
            objUserRegistrationViewModel.RoleId = objUserRegistration.RoleId;
            objUserRegistrationViewModel.CourseId = objUserRegistration.CourseId;
            objUserRegistrationViewModel.IsActive = objUserRegistration.IsActive;
            objUserRegistrationViewModel.DateCreated = objUserRegistration.DateCreated;
            objUserRegistrationViewModel.DateModified = objUserRegistration.DateModified;
            objUserRegistrationViewModel.AddAddressTextBox1 = objUserRegistration.Address.AddressTextBox1;
            objUserRegistrationViewModel.AddAddressTextBox2 = objUserRegistration.Address.AddressTextBox2;
            objUserRegistrationViewModel.CountryId = objUserRegistration.Address.CountryId;
            objUserRegistrationViewModel.StateId = objUserRegistration.Address.StateId;
            objUserRegistrationViewModel.CityId = objUserRegistration.Address.CityId;
            objUserRegistrationViewModel.Pincode = objUserRegistration.Address.Pincode;

            if (objUserRegistration == null)
            {
                return HttpNotFound();
            }
            return View(objUserRegistrationViewModel);
        }
        
        public ActionResult Welcome()
        {
            return View();
        }

        public static List<Role> GetRoles()
        {
            using (var db = new SchoolDatabaseEntities())
            {
                // condition not to Display SuperAdmin
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 4);
                return roleList.ToList();
            }
        }
    }
}
