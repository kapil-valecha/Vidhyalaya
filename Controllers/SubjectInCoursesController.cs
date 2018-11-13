using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.DB;

namespace Vidhyalaya.Controllers
{
    public class SubjectInCoursesController : Controller
    {
        public SchoolDatabaseEntities db = new SchoolDatabaseEntities();

        /// <summary>
        /// for assigning subject with courses
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMapCourseSubject()
        {
            //query for list
            var subjectInCourse = db.SubjectInCourses.Include(s => s.Course).Include(s => s.Subject);
            return View(subjectInCourse.ToList());
        }

        /// <summary>
        /// GET: for creating subjects in course
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubjectInCourse()
        {
            //for getting Course
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //for getting Subject
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        /// <summary>
        /// POST: for creating subjects in course
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubjectInCourse([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.SubjectInCourses.Add(subjectInCourse);
                db.SaveChanges();
                return RedirectToAction("GetMapCourseSubject");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        /// <summary>
        /// GET: for edit subject in course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSubjectInCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        /// <summary>
        /// POST: for edit subject in course
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSubjectInCourse([Bind(Include = "SubjectCourseId,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectInCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetMapCourseSubject");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);

            return View(subjectInCourse);
        }

        /// <summary>
        /// GET: for delete subject in course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            return View(subjectInCourse);
        }

        /// <summary>
        /// POST: for delete subject in course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            db.SubjectInCourses.Remove(subjectInCourse);
            db.SaveChanges();
            return RedirectToAction("GetMapCourseSubject");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: SubjectInCourses

    }
}
