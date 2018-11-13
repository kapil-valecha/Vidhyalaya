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
    public class StudentController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// for showing details of Logged in Student
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentProfile()
        {
            UserRegistration objUser = (UserRegistration)Session["User"];
            var varUser = db.UserRegistrations.Find(objUser.UserId);
            if (Session["User"] != null)
            {
                var userDetails = db.UserRegistrations.Where(user => user.UserId == objUser.UserId);
                if (varUser != null)
                    return View(userDetails);
            }
            return View(varUser);
        }

        /// <summary>
        /// for getting list of subjects
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectsList(int id)
        {
            var subjectList = db.SubjectInCourses.Where(subj => subj.CourseId == id).ToList();
            return View(subjectList.ToList());

        }

        public ActionResult TeachersList(int id)
        {
            var teachersList = db.TeacherInSubjects.Where(teach => teach.UserId == id).ToList();
            return View(teachersList);
        }

        public ActionResult TeachersCourseList(int id)
        {
            var teachersList = db.UserRegistrations.Where(teach => teach.CourseId == id && teach.RoleId == 3 /*&& teach.IsActive == true*/).ToList();
            return View(teachersList);
        }

        /// <summary>
        /// get method for new student registration
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterStudent()
        {
            //for roles
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            //for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");
            //for country
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
            using (var transaction = db.Database.BeginTransaction())
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
                        address.Pincode = objUserRegistrationViewModel.Pincode;
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
                            IsEmailVerified = true,
                            RoleId = objUserRegistrationViewModel.RoleId,
                            CourseId = objUserRegistrationViewModel.CourseId,
                            Password = objUserRegistrationViewModel.Password,
                            DOB = objUserRegistrationViewModel.DOB,
                            AddressId = latestAddressId,

                            IsActive = true,
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
                        transaction.Commit();
                        return RedirectToAction("Thankyou");
                    }
                    return View(objUserRegistrationViewModel);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                    throw ex;
                }
            }
        }


        /// <summary>
        /// To Edit Teacher Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditStudent(int id)
        {
            //for roles
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
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
            try
            {
                UserRegistration objUserRegistration = db.UserRegistrations.Find(id);
                UserRegistrationViewModel objUserRegistrationViewModel = new UserRegistrationViewModel();
                if (ModelState.IsValid)
                {
                    objUserRegistrationViewModel.FirstName = objUserRegistration.FirstName;
                    objUserRegistrationViewModel.LastName = objUserRegistration.LastName;
                    objUserRegistrationViewModel.Gender = objUserRegistration.Gender;
                    objUserRegistrationViewModel.Hobby = objUserRegistration.Hobby;
                    objUserRegistrationViewModel.EmailId = objUserRegistration.EmailId;
                    objUserRegistrationViewModel.IsEmailVerified = true;
                    objUserRegistrationViewModel.Password = objUserRegistration.Password;
                    objUserRegistrationViewModel.DOB = objUserRegistration.DOB;
                    objUserRegistrationViewModel.RoleId = objUserRegistration.RoleId;
                    objUserRegistrationViewModel.CourseId = objUserRegistration.CourseId;
                    objUserRegistrationViewModel.IsActive = true;

                    objUserRegistrationViewModel.DateCreated = objUserRegistration.DateCreated;
                    objUserRegistrationViewModel.DateModified = objUserRegistration.DateModified;
                    objUserRegistrationViewModel.AddAddressTextBox1 = objUserRegistration.Address.AddressTextBox1;
                    objUserRegistrationViewModel.AddAddressTextBox2 = objUserRegistration.Address.AddressTextBox2;
                    objUserRegistrationViewModel.CountryId = objUserRegistration.Address.CountryId;
                    objUserRegistrationViewModel.StateId = objUserRegistration.Address.StateId;
                    objUserRegistrationViewModel.CityId = objUserRegistration.Address.CityId;
                    objUserRegistrationViewModel.Pincode = objUserRegistration.Address.Pincode;


                }
                return View(objUserRegistrationViewModel);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw ex;
            }

        }

        /// <summary>
        ///  To Edit User Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditStudent(int id, UserRegistrationViewModel objUserRegistrationViewModel)
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
                UserRegistration objUserRegistration = db.UserRegistrations.Find(id);

                if (ModelState.IsValid)
                {
                    objUserRegistration.FirstName = objUserRegistrationViewModel.FirstName;
                    objUserRegistration.LastName = objUserRegistrationViewModel.LastName;
                    objUserRegistration.Gender = objUserRegistrationViewModel.Gender;
                    objUserRegistration.Hobby = objUserRegistrationViewModel.Hobby;
                    objUserRegistration.EmailId = objUserRegistrationViewModel.EmailId;
                    objUserRegistration.IsEmailVerified = true;
                    objUserRegistration.Password = objUserRegistrationViewModel.Password;
                    objUserRegistration.DOB = objUserRegistrationViewModel.DOB;
                    objUserRegistration.CourseId = objUserRegistrationViewModel.CourseId;
                    objUserRegistration.RoleId = objUserRegistrationViewModel.RoleId;
                    objUserRegistration.Address.AddressTextBox1 = objUserRegistrationViewModel.AddAddressTextBox1;
                    objUserRegistration.Address.AddressTextBox2 = objUserRegistrationViewModel.AddAddressTextBox2;
                    objUserRegistration.Address.CountryId = objUserRegistrationViewModel.CountryId;
                    objUserRegistration.Address.StateId = objUserRegistrationViewModel.StateId;
                    objUserRegistration.Address.CityId = objUserRegistrationViewModel.CityId;
                    objUserRegistration.Address.Pincode = objUserRegistrationViewModel.Pincode;
                    objUserRegistration.IsActive = true;
                    objUserRegistration.DateModified = DateTime.Now;

                    //User Data is saved in the user table

                    db.SaveChanges();
                    return RedirectToAction("StudentProfile");

                }
                return View(objUserRegistrationViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw ex;
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

        public ActionResult Thankyou()
        {
            return View();
        }
    }
}