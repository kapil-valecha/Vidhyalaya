﻿using System;
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
    public class UserRegistrationController : Controller
    {
        private SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        /// <summary>
        /// for list of details on Index
        /// </summary>
        /// <returns></returns>
        // GET: UserRegistration
        public ActionResult Index()
        {
            //List<UserRegistrationModel> objUserRegisterModel = new List<UserRegistrationModel>();
            //var data = (from p in db.UserRegistrations select p).ToList();
            //foreach (var item in data)
            //{
            //    UserRegistrationModel userRegistration = new UserRegistrationModel
            //    {
            //        UserId = item.UserId,
            //        FirstName = item.FirstName,
            //        LastName = item.LastName,
            //        Gender = item.Gender,
            //        Hobby = item.Hobby,
            //        EmailId = item.EmailId,
            //        Password = item.Password,
            //        DOB = item.DOB,
            //        RoleId = item.RoleId,
            //        CourseId = item.CourseId,
            //        AddressId = item.AddressId,
            //        IsActive = item.IsActive,
            //        DateCreated = item.DateCreated,

            //    };

            //    objUserRegisterModel.Add(userRegistration);

            //};
            var returnResult = db.UserRegistrations.Where(x => x.RoleId != 1 && x.RoleId != 2).ToList();
            return View(returnResult);
        }
        /// <summary>
        /// Get method for Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
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
        /// post for create
        /// </summary>
        /// <param name="objUserRegistrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRegistrationModel objUserRegistrationModel)
        {
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");

            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = new SelectList(objCourseList, "CourseId", "CourseName");

            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");

            objUserRegistrationModel.UserId = 1;
            objUserRegistrationModel.AddressId = 1;
            try
            {
                if (ModelState.IsValid)
                {

                    Address address = new Address();

                    address.AddressId = objUserRegistrationModel.AddressId;
                    address.AddressTextBox1 = objUserRegistrationModel.AddAddressTextBox1;
                    address.AddressTextBox2 = objUserRegistrationModel.AddAddressTextBox2;
                    address.CountryId = objUserRegistrationModel.CountryId;
                    address.StateId = objUserRegistrationModel.StateId;
                    address.CityId = objUserRegistrationModel.CityId;

                    db.Addresses.Add(address);
                    db.SaveChanges();

                    int latestAddressId = address.AddressId;
                    UserRegistration userRegistration = new UserRegistration
                    {

                        UserId = objUserRegistrationModel.UserId,
                        FirstName = objUserRegistrationModel.FirstName,
                        LastName = objUserRegistrationModel.LastName,
                        Gender = objUserRegistrationModel.Gender,
                        Hobby = objUserRegistrationModel.Hobby,
                        EmailId = objUserRegistrationModel.EmailId,
                        RoleId = objUserRegistrationModel.RoleId,
                        CourseId = objUserRegistrationModel.CourseId,
                        Password = objUserRegistrationModel.Password,
                        DOB = objUserRegistrationModel.DOB,
                        AddressId = latestAddressId,
                        IsActive = objUserRegistrationModel.IsActive,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,

                    };
                    db.UserRegistrations.Add(userRegistration);
                    db.SaveChanges();

                    int latestUserId = userRegistration.UserId;

                    UserInRole objUserInRole = new UserInRole();
                    objUserInRole.RoleId = objUserRegistrationModel.RoleId;
                    objUserInRole.UserId = latestUserId;
                    db.UserInRoles.Add(objUserInRole);

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

                return View(objUserRegistrationModel);
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Get method for Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
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
            var data = from p in db.UserRegistrations
                       where p.UserId == id
                       select p;

            UserRegistrationModel objUserRegistrationsModel = new UserRegistrationModel
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
                IsActive = objUserRegistration.IsActive,
                DateCreated = objUserRegistration.DateCreated,
                DateModified = objUserRegistration.DateModified
            };
            if (objUserRegistration == null)
            {
                return HttpNotFound();
            }
            return View(objUserRegistrationsModel);

        }

        /// <summary>
        /// Action for Edit Post method
        /// </summary>
        /// <param name="objUserRegistrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserRegistrationModel objUserRegistrationModel)
        {
            List<Role> objRoleList = GetRoles();
            //ViewBag.Role = new SelectList(objRoleList, "RoleId", "RoleName");
            ViewBag.Role = objRoleList;

            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;

          
            try
            {
                UserRegistration userData = db.UserRegistrations.Find(objUserRegistrationModel.UserId);
                var data = from p in db.UserRegistrations
                           where p.UserId == objUserRegistrationModel.UserId
                           select p;
                var TempList = db.UserRegistrations.FirstOrDefault();

                if (ModelState.IsValid)
                {

                    userData.UserId = objUserRegistrationModel.UserId;
                    userData.FirstName = objUserRegistrationModel.FirstName;
                    userData.LastName = objUserRegistrationModel.LastName;
                    userData.Gender = objUserRegistrationModel.Gender;
                    userData.Hobby = objUserRegistrationModel.Hobby;
                    userData.EmailId = objUserRegistrationModel.EmailId;
                    userData.Password = objUserRegistrationModel.Password;
                    userData.DOB = objUserRegistrationModel.DOB;
                    userData.RoleId = objUserRegistrationModel.RoleId;
                    userData.Address.AddressId = objUserRegistrationModel.AddressId;
                    userData.Address.CountryId = objUserRegistrationModel.CountryId;
                    userData.Address.StateId = objUserRegistrationModel.StateId;
                    userData.Address.CityId = objUserRegistrationModel.CityId;
                    userData.IsActive = objUserRegistrationModel.IsActive;
                    userData.DateModified = DateTime.Now;

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(objUserRegistrationModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// to get Details in list
        /// </summary>
        /// <param name="id"> brings record by userid</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserRegistration userRegistration = db.UserRegistrations.Find(id);
                var data = from p in db.UserRegistrations
                           where p.UserId == id
                           select p;
                var TempList = db.UserRegistrations.ToList();

                UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel
                {
                    UserId = userRegistration.UserId,
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    Gender = userRegistration.Gender,
                    EmailId = userRegistration.EmailId,
                    Password = userRegistration.Password,
                    Hobby = userRegistration.Hobby,
                    DOB = userRegistration.DOB,
                    RoleId = userRegistration.RoleId,
                    IsActive = userRegistration.IsActive,
                    DateCreated = userRegistration.DateCreated,
                    DateModified = userRegistration.DateModified
                };

                if (userRegistration == null)
                {
                    return HttpNotFound();
                }
                return View(objUserRegistrationModel);
            }
        }

        /// <summary>
        /// GET: Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Delete(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserRegistration userRegistration = db.UserRegistrations.Find(id);
                var data = from p in db.UserRegistrations
                           where p.UserId == id
                           select p;
                var TempList = db.UserRegistrations.ToList();


                UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel
                {
                    UserId = userRegistration.UserId,
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    Gender = userRegistration.Gender,
                    EmailId = userRegistration.EmailId,
                    Password = userRegistration.Password,
                    Hobby = userRegistration.Hobby,
                    DOB = userRegistration.DOB,
                    IsActive = userRegistration.IsActive,
                    DateCreated = userRegistration.DateCreated,
                    DateModified = userRegistration.DateModified
                };

                if (userRegistration == null)
                {
                    return HttpNotFound();
                }
                return View(objUserRegistrationModel);
            }
        }

        /// <summary>
        /// POST: Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UserRegistration objUserRegistration = db.UserRegistrations.Find(id);
                    db.UserRegistrations.Remove(objUserRegistration);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
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
        /// Get all Countries
        /// </summary>
        /// <returns></returns>
        //public static List<Country> GetCountry()
        //public DataSet GetCountry()
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

        //    SqlCommand com = new SqlCommand("Select * from Country", con);


        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);

        //    return ds;

        //    //using (var db = new SchoolDatabaseEntities())
        //    //{
        //    //    var k = db.Countries;
        //    //    return k.ToList();
        //    //}
        //}

        //public void CountryBind()
        //{

        //    DataSet ds = GetCountry();
        //    List<SelectListItem> countryList = new List<SelectListItem>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        countryList.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["CountryId"].ToString() });

        //    }
        //    ViewBag.Country = countryList;

        //}

        /// <summary>
        /// Get all states
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
        /// Get all Cities
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
        //public static List<State> GetState()
        //{
        //    using (var db = new SchoolDatabaseEntities())
        //    {
        //        var k = db.States;
        //        return k.ToList();
        //    }
        //}
        //public static List<City> GetCity()
        //{
        //    using (var db = new SchoolDatabaseEntities())
        //    {
        //        var k = db.Cities;
        //        return k.ToList();
        //    }
        //}
        //public static List<Course> GetCourse()
        //{
        //    using (var db = new SchoolDatabaseEntities())
        //    {
        //        var k = db.Courses;
        //        return k.ToList();
        //    }
        //}
    }
}