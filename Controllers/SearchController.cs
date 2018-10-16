using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.DB;
using Vidhyalaya.Models;

namespace Vidhyalaya.Controllers
{
    public class SearchController : Controller
    {
        SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        // GET: Search
        public ActionResult SearchIndex()
        {
            List<UserSearchViewModel> userList = db.UserRegistrations.Select(x => new UserSearchViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Hobby = x.Hobby,
                EmailId = x.EmailId,
                DOB = x.DOB,
                AddressId = x.AddressId,
                CountryName = x.Address.Country.CountryName,
                CityId = x.Address.CityId,
                CityName = x.Address.City.CityName,
                StateId = x.Address.StateId,
                StateName = x.Address.State.StateName,
                RoleId = x.RoleId,
                CourseId = x.CourseId,
                IsActive = x.IsActive,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            }).ToList();

            ViewBag.UserDetailsList = userList;
            return View();
        }

        public ActionResult GetSearchRecord(string SearchText)
        {
            SchoolDatabaseEntities db = new SchoolDatabaseEntities();
            List<UserSearchViewModel> userList = db.UserRegistrations.Where(x => x.FirstName.Contains(SearchText) || x.LastName.Contains(SearchText)).Select(x => new UserSearchViewModel
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Hobby = x.Hobby,
                EmailId = x.EmailId,
                IsEmailVerified = true,
                Password = x.Password,
                DOB = x.DOB,
                AddressId = x.AddressId,
                CountryName = x.Address.Country.CountryName,
                CityId = x.Address.CityId,
                CityName = x.Address.City.CityName,
                StateId = x.Address.StateId,
                StateName = x.Address.State.StateName,
                RoleId = x.RoleId,
                CourseId = x.CourseId,
                IsActive = x.IsActive,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified
            }).ToList();
            return PartialView("_SearchPartial", userList);
        }
    }
}