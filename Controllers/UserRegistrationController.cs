using System;
using System.Collections.Generic;
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
            List<UserRegistrationModel> objUserRegisterModel = new List<UserRegistrationModel>();
            var data = (from p in db.UserRegistrations select p).ToList();
            foreach (var item in data)
            {
                UserRegistrationModel userRegistration = new UserRegistrationModel
                {
                    UserId = item.UserId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    Hobby = item.Hobby,
                    EmailId = item.EmailId,
                    Password = item.Password,
                    DOB = item.DOB,
                    IsActive = item.IsActive,
                    DateCreated = item.DateCreated,

                };

                objUserRegisterModel.Add(userRegistration);

            };
            return View(objUserRegisterModel);
        }

        /// <summary>
        /// Get method for Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post method for Create
        /// </summary>
        /// <param name="objUserRegistrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRegistrationModel objUserRegistrationModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UserRegistration userRegistration = new UserRegistration
                    {
                        UserId = objUserRegistrationModel.UserId,
                        FirstName = objUserRegistrationModel.FirstName,
                        LastName = objUserRegistrationModel.LastName,
                        Gender = objUserRegistrationModel.Gender,
                        Hobby = objUserRegistrationModel.Hobby,
                        EmailId = objUserRegistrationModel.EmailId,
                        Password = objUserRegistrationModel.Password,
                        DOB = objUserRegistrationModel.DOB,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now

                    };
                    db.UserRegistrations.Add(userRegistration);

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
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserRegistration objUserRegistration = db.UserRegistrations.Find(id);
                var data = from p in db.UserRegistrations
                           where p.UserId == id
                           select p;
                var TempList = db.UserRegistrations.ToList();


                 UserRegistrationModel objUserRegistrationsModel = new UserRegistrationModel
                {
                    UserId = objUserRegistration.UserId,
                    FirstName = objUserRegistration.FirstName,
                    LastName = objUserRegistration.LastName,
                    Gender = objUserRegistration.Gender,
                    Hobby = objUserRegistration.Hobby,
                    EmailId = objUserRegistration.EmailId,
                    Password = objUserRegistration.Password,
                    DOB = objUserRegistration.DOB,
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
        }

        /// <summary>
        /// Action for Edit Post method
        /// </summary>
        /// <param name="objUserRegistrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRegistrationModel objUserRegistrationModel)
        {
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
                        userData.IsActive = objUserRegistrationModel.IsActive;
                        userData.DateCreated = DateTime.Now;
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

    }
}