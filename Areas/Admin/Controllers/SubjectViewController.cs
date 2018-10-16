using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vidhyalaya.Areas.Admin.Models;
using Vidhyalaya.DB;

namespace Vidhyalaya.Areas.Admin.Controllers
{
    [Authorize]
    public class SubjectViewController : Controller
    {
        SchoolDatabaseEntities db = new SchoolDatabaseEntities();
        /// <summary>
        ///  for getting list of all Subjects
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSubject()
        {
            List<SubjectViewModel> objSubjectViewModel = new List<SubjectViewModel>();
            var getSubject = (from sub in db.Subjects select sub).ToList();
            foreach (var item in getSubject)
            {
                SubjectViewModel subject = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName
                };
                objSubjectViewModel.Add(subject);
            };
            return View(objSubjectViewModel);
        }

        /// <summary>
        ///  GET :Admin => Subject => To Create Subject
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubject()
        {
            return View();
        }

        /// <summary>
        /// POST : Admin => Subject => To Create Subject
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
                    db.Subjects.Add(objSubjects);
                    db.SaveChanges();
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
        /// GET :Admin => Subject => Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCourse(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);

            //var data = from d in db.Subjects where d.SubjectId == id select d;
            //var tempList = db.Subjects.ToList();

            SubjectViewModel subjectView = new SubjectViewModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName
            };
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subjectView);
        }

        /// <summary>
        ///  POST: Admin => Subject => Edit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCourse(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
