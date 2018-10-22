using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Welcome()
        {
            return View();
        }

        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        /// <summary>
        /// Get Action Method for Create New Student
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewStudent()
        {
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");

            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");

            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            return View();
        }

        /// <summary>
        /// Post Action Method for Create New Student
        /// </summary>
        /// <param name="objUserRegistrationViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewStudent(UserRegistrationViewModel objUserRegistrationViewModel)
        {
            objUserRegistrationViewModel.UserId = 1;
            objUserRegistrationViewModel.AddressId = 1;
            using (var dbTransaction = db.Database.BeginTransaction())
            {
                try
                {

                    if (ModelState.IsValid)
                    {
                        Address address = new Address();
                        address.AddressId = objUserRegistrationViewModel.AddressId;
                        address.AddressTextBox1 = objUserRegistrationViewModel.AddAddressTextBox1;
                        address.AddressTextBox2 = objUserRegistrationViewModel.AddAddressTextBox2;
                        address.CountryId = objUserRegistrationViewModel.CountryId;
                        address.StateId = objUserRegistrationViewModel.StateId;
                        address.CityId = objUserRegistrationViewModel.CityId;
                        db.Addresses.Add(address);
                        db.SaveChanges();
                        int latestAddressId = address.AddressId;

                        UserRegistration userRegistration = new UserRegistration
                        {
                            UserId = objUserRegistrationViewModel.UserId,
                            FirstName = objUserRegistrationViewModel.FirstName,
                            LastName = objUserRegistrationViewModel.LastName,
                            Gender = objUserRegistrationViewModel.Gender,
                            Hobby = objUserRegistrationViewModel.Hobby,
                            EmailId = objUserRegistrationViewModel.EmailId,
                            RoleId = objUserRegistrationViewModel.RoleId,
                            CourseId = objUserRegistrationViewModel.CourseId,
                            Password = objUserRegistrationViewModel.Password.GetHashCode().ToString(),
                            DOB = objUserRegistrationViewModel.DOB,
                            AddressId = latestAddressId,
                            IsActive = objUserRegistrationViewModel.IsActive,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };
                        db.UserRegistrations.Add(userRegistration);
                        db.SaveChanges();

                        int latestUserId = userRegistration.UserId;
                        UserInRole objUserInRole = new UserInRole();
                        objUserInRole.RoleId = objUserRegistrationViewModel.RoleId;
                        objUserInRole.UserId = latestUserId;
                        db.UserInRoles.Add(objUserInRole);
                        db.SaveChanges();
                        return RedirectToAction("AllUserDetails");
                    }
                    dbTransaction.Commit();

                    return View(objUserRegistrationViewModel);
                }
                catch
                {
                    dbTransaction.Rollback();
                    throw;
                }

            }
        }

        /// <summary>
        /// Get all Roles
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoles()
        {
            using (var db = new SchoolDatabaseEntities())
            {
                var k = db.Roles.Where(x => x.RoleId != 1);
                return k.ToList();
            }
        }

    }
}