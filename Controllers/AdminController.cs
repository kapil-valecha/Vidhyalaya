using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class AdminController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        public ActionResult Welcome()
        {            
            return View();
        }

        /// <summary>
        /// Get Action Method for all users details
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUserDetails()
        {
            var returnUserDetailsList = db.UserRegistrations.ToList();
            return View(returnUserDetailsList);
        }

        /// <summary>
        /// Get Action Method for Create New User
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewUser()
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
        /// Action for Edit Existing User Get method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(int id)
        {
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserRegistration objUserRegistration = db.UserRegistrations.Find(id);
            List<State> statesList = db.States.Where(x => x.CountryId == objUserRegistration.Address.CountryId).ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");
            List<City> citiesList = db.Cities.Where(x => x.StateId == objUserRegistration.Address.StateId).ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "CityName");

            var data = from p in db.UserRegistrations where p.UserId == id select p;
            UserRegistrationViewModel objUserRegistrationViewModel = new UserRegistrationViewModel
            {
                UserId = objUserRegistration.UserId,
                FirstName = objUserRegistration.FirstName,
                LastName = objUserRegistration.LastName,
                Password = objUserRegistration.Password,
                EmailId = objUserRegistration.EmailId,
                Gender = objUserRegistration.Gender,
                DOB = objUserRegistration.DOB,
                Hobby = objUserRegistration.Hobby,
                CourseId = objUserRegistration.CourseId,
                RoleId = objUserRegistration.RoleId,
                AddressId = objUserRegistration.Address.AddressId,
                CountryId = objUserRegistration.Address.CountryId,
                StateId = objUserRegistration.Address.StateId,
                CityId = objUserRegistration.Address.CityId,
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
            List<Role> objRoleList = GetRoles();
            //ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            ViewBag.Role = objRoleList;
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;

            try
            {
                UserRegistration userData = db.UserRegistrations.Find(objUserRegistrationViewModel.UserId);
              
                if (ModelState.IsValid)
                {
                    userData.UserId = objUserRegistrationViewModel.UserId;
                    userData.FirstName = objUserRegistrationViewModel.FirstName;
                    userData.LastName = objUserRegistrationViewModel.LastName;
                    userData.Gender = objUserRegistrationViewModel.Gender;
                    userData.Hobby = objUserRegistrationViewModel.Hobby;
                    userData.EmailId = objUserRegistrationViewModel.EmailId;
                    userData.Password = objUserRegistrationViewModel.Password;
                    userData.DOB = objUserRegistrationViewModel.DOB;
                    userData.RoleId = objUserRegistrationViewModel.RoleId;
                    userData.AddressId = objUserRegistrationViewModel.AddressId;
                    userData.Address.CountryId = objUserRegistrationViewModel.CountryId;
                    userData.Address.StateId = objUserRegistrationViewModel.StateId;
                    userData.Address.CityId = objUserRegistrationViewModel.CityId;
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

        /// <summary>
        /// Get Action for Existing User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userRegistration = db.UserRegistrations.Find(id);

            if (userRegistration == null)
            {
                return HttpNotFound();
            }
            return View();

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
                    UserRegistration objUserRegistration = db.UserRegistrations.Find(id);
                    db.UserRegistrations.Remove(objUserRegistration);
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

        /// <summary>
        /// for logging out current user
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {

            return RedirectToAction("Login", "UserLogin");
        }
    }
}