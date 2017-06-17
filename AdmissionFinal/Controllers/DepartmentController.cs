using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AdmissionFinal.Entity;

namespace AdmissionFinal.Controllers
{
    [Authorize(Roles = "Department")]
    public class DepartmentController : Controller
    {
        //
        // GET: /Department/
        AdmissionContext db = new AdmissionContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PromoteStudent(int? i)
        {

            var studentYear = db.StudentYearStatus.Include(s => s.Student).
                Where(s => s.Student.Department.DepartmentName == User.Identity.Name 
                    && s.AdmissionPaid == 1 && s.ExamPaid == 1).ToList();
 
            if(!string.IsNullOrWhiteSpace(i.ToString()))
            {
                studentYear = studentYear.Where(m => m.Year == i).ToList();
            }
            return View(studentYear);
        }

        public ActionResult Promote(Guid id)
        {

            var studentYear = db.StudentYearStatus.Find(id);

            studentYear.AdmissionPaid = 0;
            studentYear.ExamPaid = 0;
            studentYear.Year++;
            db.Entry(studentYear).State = EntityState.Modified;
            db.SaveChanges();

            return View();
        }

        public ActionResult ReAdd(Guid id)
        {
            var studentYear = db.StudentYearStatus.Find(id);

            studentYear.AdmissionPaid = 0;
            studentYear.ExamPaid = 0;
            db.Entry(studentYear).State = EntityState.Modified;
            db.SaveChanges();

            return View();
        }

        public ActionResult ViewStudentInformation(string searchString)
        {
            var students = db.Students.Include(s => s.aspnet_Users).
                Include(s => s.Department).Include(s => s.Hall).
                Where(m => m.Department.DepartmentName == User.Identity.Name);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = db.Students.Include(s => s.aspnet_Users).
                 Include(s => s.Department).Include(s => s.Hall).
                 Where(m => m.Department.DepartmentName == User.Identity.Name &&
                     m.Name.Contains(searchString));
            }

            return View(students.ToList());     
        }

        public ActionResult ShowDetails(Guid id)
        {
            var student = db.Students.Include(s => s.aspnet_Users).
               Include(s => s.Department).Include(s => s.Hall).
               Single(m => m.StudentId == id);
            return View(student);
        }

    }
}
