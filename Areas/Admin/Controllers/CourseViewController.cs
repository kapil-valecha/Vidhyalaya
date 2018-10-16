using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.Areas.Admin.Models;
using Vidhyalaya.DB;

namespace Vidhyalaya.Areas.Admin.Controllers
{
    public class CourseViewController : Controller
    {
        SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        /// <summary>
        ///  Get list of all Courses Added via Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCourse()
        {
            //get list of courses from view model (in Area)
            List<CourseViewModel> objCourseViewModel = new List<CourseViewModel>();
            var getCourse = (from course in db.Courses select course).ToList();
            foreach (var item in getCourse)
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
        /// GET: Create Method For Course
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewCourse()
        {
            var getSubject = (from subj in db.Subjects select subj).ToList();
            var courseModel = new CourseViewModel
            {
                //this Subject is of list type in SubjectViewModel
                Subject = getSubject.Select(sub => new SelectListItem
                {
                    Value = sub.SubjectId.ToString(),
                    Text = sub.SubjectName
                })
            };
            return View(courseModel);
        }

        /// <summary>
        /// POST :Create Method For Course
        /// </summary>
        /// <param name="objCourseViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewCourse(CourseViewModel objCourseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Course objCourse = new Course
                    {
                        CourseName = objCourseViewModel.CourseName,
                        SubjectId = objCourseViewModel.SubjectId
                    };
                    var testCourse = db.Courses.Add(objCourse);
                    db.SaveChanges();
                    //to get CourseId
                    var courseId = objCourse.CourseId;

                    //for adding Subject in Course in SubjectInCourse Table
                    SubjectInCourse objSubjectInCourse = new SubjectInCourse
                    {
                        SubjectId = objCourseViewModel.SubjectId,
                        CourseId = courseId
                    };
                    db.SubjectInCourses.Add(objSubjectInCourse);
                    db.SaveChanges();
                };
                return RedirectToAction("GetCourse", "Course", objCourseViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}