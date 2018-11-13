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
    public class SubjectsController : Controller
    {
        SchoolDatabaseEntities objEntities = new SchoolDatabaseEntities();

        public ActionResult GetSubject()
        {
            List<SubjectViewModel> objSubjectViewModel = new List<SubjectViewModel>();
            var data = (from p in objEntities.Subjects select p).ToList();
            foreach (var item in data)
            {
                SubjectViewModel subject = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName,

                };
                objSubjectViewModel.Add(subject);
            };
            return View(objSubjectViewModel);
        }

        /// <summary>
        /// GET: for create new subject
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubject()
        {
            return View();
        }

        /// <summary>
        /// POST: for create new subject
        /// </summary>
        /// <param name="objSubjectViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubject(SubjectViewModel objSubjectViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Subject objSubjects = new Subject
                    {
                        SubjectName = objSubjectViewModel.SubjectName
                    };
                    objEntities.Subjects.Add(objSubjects);
                    objEntities.SaveChanges();
                    return RedirectToAction("GetSubject");
                };
                return View(objSubjectViewModel);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// GET: for edit subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSubject(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subjects = objEntities.Subjects.Find(id);
            var data = from d in objEntities.Subjects
                       where d.SubjectId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            SubjectViewModel subjectView = new SubjectViewModel
            {
                SubjectId = subjects.SubjectId,
                SubjectName = subjects.SubjectName
            };
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjectView);
        }

        /// <summary>
        /// PST: for edit subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objSubjects"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSubject(int id, Subject objSubjects)
        {
            {
                if (ModelState.IsValid)
                {
                    Subject objSubject = new Subject
                    {
                        SubjectId = id,
                        SubjectName = objSubjects.SubjectName
                    };
                    objEntities.Entry(objSubject).State = EntityState.Modified;
                    objEntities.SaveChanges();
                    return RedirectToAction("GetSubject");
                }
                return View(objSubjects);
            }
        }

        /// <summary>
        /// GET: for delete subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subjects = objEntities.Subjects.Find(id);
            var data = from d in objEntities.Subjects
                       where d.SubjectId == id
                       select d;
            SubjectViewModel subjectView = new SubjectViewModel
            {
                SubjectId = subjects.SubjectId,
                SubjectName = subjects.SubjectName
            };
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjectView);
        }

        /// <summary>
        ///  POST: for delete subject
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objSubjectViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSubject(int id, SubjectViewModel objSubjectViewModel)
        {
            try
            {
                Subject subjects = objEntities.Subjects.Find(id);
                objEntities.Subjects.Remove(subjects);
                objEntities.SaveChanges();
                return RedirectToAction("GetSubject");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0} user is failed to register", ex.Message);
                return View(ex);
            }
        }
    }
}
