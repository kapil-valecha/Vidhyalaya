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
    public class StudentController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Thankyou()
        {
            return View();
        }

        /// <summary>
        /// for getting list of courses
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseList()
        {
            return View(db.Courses.ToList());
        }

        /// <summary>
        /// for getting list of subjects
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectList()
        {
            return View(db.Subjects.ToList());
        }

        /// <summary>
        /// get method for new student registration
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterStudent()
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
        /// post method for new student registration
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStudent(UserRegistrationViewModel objUserRegistrationViewModel)
        {
            objUserRegistrationViewModel.UserId = 1;
            objUserRegistrationViewModel.AddressId = 1;
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
                            Password = objUserRegistrationViewModel.Password,
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
                        return RedirectToAction("Thankyou");
                    }
                    return View(objUserRegistrationViewModel);
                }
                catch (Exception)
                {
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
                var k = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2);
                return k.ToList();
            }
        }

        /// <summary>
        /// For getting all states
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public DataSet GetStates(string countryId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDatabaseEntities"].ConnectionString);
            SqlCommand com = new SqlCommand("Select * from State where CountryId=@catid", con);
            com.Parameters.AddWithValue("@catid", countryId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        /// <summary>
        /// For binding states with countries
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public JsonResult StateBind(int countryId)
        {
            List<StateModel> data = new List<StateModel>();
            var stateData = db.States.Where(x => x.CountryId == countryId).ToList();
            foreach (var item in stateData)
            {
                StateModel ds = new StateModel
                {
                    StateId = item.StateId,
                    StateName = item.StateName
                };
                data.Add(ds);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// For getting all Cities
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public DataSet GetCity(string stateId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolDatabaseEntities"].ConnectionString);
            SqlCommand com = new SqlCommand("Select * from City where StateId=@staid", con);
            com.Parameters.AddWithValue("@staid", stateId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        /// <summary>
        /// For binding cities with states
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public JsonResult CityBind(int stateId)
        {
            List<CityModel> data = new List<CityModel>();
            var cityData = db.Cities.Where(x => x.StateId == stateId).ToList();
            foreach (var item in cityData)
            {
                CityModel ds = new CityModel
                {
                    CityId = item.CityId,
                    CityName = item.CityName
                };
                data.Add(ds);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}