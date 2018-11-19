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
        SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        [SessionController]
        /// <summary>
        /// for showing details of Logged in Student
        /// </summary>
        /// <returns></returns>
       
        public ActionResult StudentsProfile()

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

        [SessionController]
        public ActionResult TeachersCourseList(int ?id)
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

        public JsonResult CheckEmailExists(string emailid)
        {
            try
            {
                bool isValid = !db.UserRegistrations.ToList().Exists(p => p.EmailId.Equals(emailid, StringComparison.CurrentCultureIgnoreCase));
                return Json(isValid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Source);
                return null;
            }
        }

        [SessionController]
        /// <summary>
        /// Action for Edit Existing User Get method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(int id)
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

            UserRegistration objUserRegistration = db.UserRegistrations.Find(id);

            var data = from p in db.UserRegistrations where p.UserId == id select p;
          EditRegistrationViewModel objEditRegistrationViewModel = new EditRegistrationViewModel
          {
                FirstName = objUserRegistration.FirstName,
                LastName = objUserRegistration.LastName,
                EmailId = objUserRegistration.EmailId,
                //Password = objUserRegistration.Password,
                //IsEmailVerified = objUserRegistration.IsEmailVerified,
                Gender = objUserRegistration.Gender,
                DOB = objUserRegistration.DOB,
                Hobby = objUserRegistration.Hobby,
                CourseId = objUserRegistration.CourseId,
                RoleId = objUserRegistration.RoleId,
                CountryId = objUserRegistration.Address.CountryId,
                StateId = objUserRegistration.Address.StateId,
                CityId = objUserRegistration.Address.CityId,
                Pincode = objUserRegistration.Address.Pincode,
                AddAddressTextBox1 = objUserRegistration.Address.AddressTextBox1,
                AddAddressTextBox2 = objUserRegistration.Address.AddressTextBox2,
                //IsActive = objUserRegistration.IsActive,
                DateCreated = objUserRegistration.DateCreated,
                DateModified = objUserRegistration.DateModified
            };
            if (objUserRegistration == null)
            {
                return HttpNotFound();
            }
            return View(objEditRegistrationViewModel);
        }

        /// <summary>
        /// Action for Edit Existing User Post method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objUserRegistrationViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionController]
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
                UserRegistration userData = db.UserRegistrations.Find(id);

                objUserRegistrationViewModel.Password = userData.Password;
                //if (ModelState.IsValid)
                //{
                userData.FirstName = objUserRegistrationViewModel.FirstName;
                userData.LastName = objUserRegistrationViewModel.LastName;
                userData.Gender = objUserRegistrationViewModel.Gender;
                userData.Hobby = objUserRegistrationViewModel.Hobby;
                userData.EmailId = objUserRegistrationViewModel.EmailId;
                //userData.Password = objUserRegistrationViewModel.Password;
                //userData.IsEmailVerified = objUserRegistrationViewModel.IsEmailVerified;
                userData.DOB = objUserRegistrationViewModel.DOB;
                userData.RoleId = objUserRegistrationViewModel.RoleId;
                userData.Address.CountryId = objUserRegistrationViewModel.CountryId;
                userData.Address.StateId = objUserRegistrationViewModel.StateId;
                userData.Address.CityId = objUserRegistrationViewModel.CityId;
                userData.Address.Pincode = objUserRegistrationViewModel.Pincode;
                userData.Address.AddressTextBox1 = objUserRegistrationViewModel.AddAddressTextBox1;
                userData.Address.AddressTextBox2 = objUserRegistrationViewModel.AddAddressTextBox2;
                //userData.IsActive = objUserRegistrationViewModel.IsActive;
                userData.DateModified = DateTime.Now;
                // db.SaveChanges();

                //}
                int latestRoleId = objUserRegistrationViewModel.RoleId;
                UserInRole objUserInRole = new UserInRole();
                objUserInRole.RoleId = latestRoleId;
                objUserInRole.UserId = objUserRegistrationViewModel.UserId;
                db.SaveChanges();
                return RedirectToAction("StudentsProfile");

                //return View(objUserRegistrationViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all Roles
        /// </summary>
        /// <returns></returns>
        /// 
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

        public ActionResult SubjectList(int id)
        { 
            var subjectList = db.SubjectInCourses.Where(subj => subj.CourseId == id).ToList();

            return View(subjectList.ToList());

        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Thankyou()
        {
            return View();
        }
    }
}