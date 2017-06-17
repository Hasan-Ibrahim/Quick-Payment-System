using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdmissionFinal.Entity;

namespace AdmissionFinal.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        AdmissionContext db = new AdmissionContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowInfo()
        {
            var student = db.Students.Include(s => s.aspnet_Users).
                Include(s => s.Department).Include(s => s.Hall).
                Single(m => m.aspnet_Users.UserName ==User.Identity.Name);

            return View(student);
        }

        public ActionResult ShowDues()
        {
            var student = db.Students.Include(s => s.aspnet_Users).
               Include(s => s.Department).Include(s => s.Hall).
               Single(m => m.aspnet_Users.UserName == User.Identity.Name);
            var studentYearStatus = db.StudentYearStatus.
                Single(m => m.Student.StudentId == student.StudentId);

            if(studentYearStatus.AdmissionPaid == 0)
            {
                return RedirectToAction("ShowAdmissionDues");
            }
            else if (studentYearStatus.ExamPaid == 0)
            {

                return RedirectToAction("ShowExamDues");
            }
            return View();
        }

        public ActionResult ShowAdmissionDues()
        {
            var student = db.Students.Include(s => s.aspnet_Users).
               Include(s => s.Department).Include(s => s.Hall).
               Single(m => m.aspnet_Users.UserName == User.Identity.Name);

            var fee = db.AdmissionFees.Include(s => s.Hall).
                Single(m => m.Hall.HallId == student.Hall.HallId);
            int dateRange = DateTime.Now.Day - fee.LastDate.Day;
            decimal fine = 0;
            if(dateRange > 0)
            {
                fine = dateRange*10;
            }
            ViewBag.Fine = fine;
            return View(fee);
        }
        public ActionResult ShowExamDues()
        {
            var student = db.Students.Include(s => s.aspnet_Users).
               Include(s => s.Department).Include(s => s.Hall).
               Single(m => m.aspnet_Users.UserName == User.Identity.Name);

            var fee = db.ExamFees.Include(s => s.Hall).
                Single(m => m.Hall.HallId == student.Hall.HallId);

            int dateRange = DateTime.Now.Day - fee.LastDate.Day;
            decimal fine = 0;
            if(dateRange > 0)
            {
                fine = dateRange*10;
            }
            ViewBag.Fine = fine;
            return View(fee);
        }
    }
}
