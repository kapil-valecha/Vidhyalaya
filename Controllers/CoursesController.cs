using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.Areas.Admin.Models;
using Vidhyalaya.DB;

namespace Vidhyalaya.Controllers
{
    [SessionController]
    public class CoursesController : Controller
    {
        SchoolDatabaseEntities objEntities = new SchoolDatabaseEntities();
        /// <summary>
        ///  GET: for getting course
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCourse()
        {
            List<CourseViewModel> objCourseViewModel = new List<CourseViewModel>();
            var data = (from p in objEntities.Courses select p).ToList();
            foreach (var item in data)
            {
                CourseViewModel course = new CourseViewModel
                {
                    CourseId = item.CourseId,
                    CourseName = item.CourseName

                };
                objCourseViewModel.Add(course);
            };
            return View(objCourseViewModel);
        }

        /// <summary>
        ///  GET: for creating new course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateCourse(string id)

        {

            return View();

        }

        /// <summary>
        /// POST: for creating new course
        /// </summary>
        /// <param name="objCourseViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CourseViewModel objCourseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Course objCourses = new Course
                    {
                        CourseName = objCourseViewModel.CourseName,

                    };
                    var test = objEntities.Courses.Add(objCourses);
                    objEntities.SaveChanges();

                };
                return RedirectToAction("GetCourse", "Courses", objCourseViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GET: for edit course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCourse(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course courses = objEntities.Courses.Find(id);
            var data = from d in objEntities.Courses
                       where d.CourseId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            CourseViewModel courseView = new CourseViewModel
            {
                CourseId = Convert.ToInt32(courses.CourseId),
                CourseName = courses.CourseName
            };
            if (courses == null)
            {
                return HttpNotFound();
            }

            return View(courseView);
        }

        /// <summary>
        /// POST for edit course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objCourses"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCourse(int id, Course objCourses)
        {
            {
                if (ModelState.IsValid)
                {
                    Course objCourse = new Course
                    {
                        CourseId = id,
                        CourseName = objCourses.CourseName
                    };
                    objEntities.Entry(objCourse).State = EntityState.Modified;
                    objEntities.SaveChanges();
                    return RedirectToAction("GetCourse");
                }
                return View(objCourses);
            }

        }

        /// <summary>
        /// GET: for delete course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course courses = objEntities.Courses.Find(id);
            var data = from d in objEntities.Courses
                       where d.CourseId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            CourseViewModel courseView = new CourseViewModel
            {
                CourseId = Convert.ToInt32(courses.CourseId),
                CourseName = courses.CourseName
            };
            if (courses == null)
            {
                return HttpNotFound();
            }
            return View(courseView);
        }

        /// <summary>
        ///POST: for delete  course 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            try
            {
                Course courses = objEntities.Courses.Find(id);
                objEntities.Courses.Remove(courses);
                objEntities.SaveChanges();
                return RedirectToAction("GetCourse");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: delete failed", ex.Message);
                return View(ex);
            }
        }
    }
}
