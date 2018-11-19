
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
    [SessionController]
    public class SuperAdminController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        /// <summary>
        /// Welcome for super admin
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// Get Action Method for all users details
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUserDetails(string searching)
        {
            var users = from usr in db.UserRegistrations where usr.RoleId != 1 select usr;
            if (!String.IsNullOrEmpty(searching))
            {
                users = users.Where(usr => usr.Role.RoleName.Contains(searching) && usr.RoleId != 1);
            }
            return View(users.ToList());
        }

        /// <summary>
        /// Get Action for Existing User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserDetails(int? id)
        {
            // for roles
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            // for country
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            //for state
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");
            // for city
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


        /// <summary>
        /// Get Action Method for Create New User
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewUser()
        {
            // for roles
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            // for course
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");
            // for country
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");
            return View();
        }

        /// <summary>
        /// Post Action Method for Create New User
        /// </summary>
        /// <param name="objUserRegistrationViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewUser(UserRegistrationViewModel objUserRegistrationViewModel)
        {
            objUserRegistrationViewModel.UserId = 1;
            objUserRegistrationViewModel.AddressId = 1;
            using (var Transaction = db.Database.BeginTransaction())
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
                            IsEmailVerified = objUserRegistrationViewModel.IsEmailVerified,
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

                        Transaction.Commit();
                        return RedirectToAction("AllUserDetails");
                    }

                    return View(objUserRegistrationViewModel);
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    ModelState.AddModelError(string.Empty, ex.Message);
                    throw ex;
                }

            }

        }

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
            UserRegistrationViewModel objUserRegistrationViewModel = new UserRegistrationViewModel
            {
                FirstName = objUserRegistration.FirstName,
                LastName = objUserRegistration.LastName,
                EmailId = objUserRegistration.EmailId,
                Password = objUserRegistration.Password,
                IsEmailVerified = objUserRegistration.IsEmailVerified,
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
                UserRegistration userData = db.UserRegistrations.Find(id);

                objUserRegistrationViewModel.Password = userData.Password;
                //if (ModelState.IsValid)
                //{
                    userData.FirstName = objUserRegistrationViewModel.FirstName;
                    userData.LastName = objUserRegistrationViewModel.LastName;
                    userData.Gender = objUserRegistrationViewModel.Gender;
                    userData.Hobby = objUserRegistrationViewModel.Hobby;
                    userData.EmailId = objUserRegistrationViewModel.EmailId;
                    userData.Password = objUserRegistrationViewModel.Password;
                    userData.IsEmailVerified = objUserRegistrationViewModel.IsEmailVerified;
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
                   // db.SaveChanges();

                //}
                int latestRoleId = objUserRegistrationViewModel.RoleId;
                UserInRole objUserInRole = new UserInRole();
                objUserInRole.RoleId = latestRoleId;
                objUserInRole.UserId = objUserRegistrationViewModel.UserId;
                db.SaveChanges();
                return RedirectToAction("AllUserDetails");

                //return View(objUserRegistrationViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// GET: Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(int? id)
        {
            return View();
        }

        /// <summary>
        /// POST: Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserInRole objUserInRole = db.UserInRoles.Where(m => m.UserId == id).FirstOrDefault();
                    UserRegistration objUser = db.UserRegistrations.Where(m => m.UserId == id).FirstOrDefault();
                    Address objAddress = db.Addresses.Where(m => m.AddressId == objUser.AddressId).FirstOrDefault();

                    //for removing address from address table
                    db.Addresses.Remove(objAddress);
                    //for removing User from User Table
                    db.UserRegistrations.Remove(objUser);
                    //for removing User from UserInRole table.
                    db.UserInRoles.Remove(objUserInRole);
                    db.SaveChanges();
                }
                return RedirectToAction("AllUserDetails");
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
        public static List<Role> GetRoles()
        {
            using (var db = new SchoolDatabaseEntities())
            {
                var k = db.Roles.Where(x => x.RoleId != 1);
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