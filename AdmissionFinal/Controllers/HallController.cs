using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdmissionFinal.Entity;
using AdmissionFinal.Helper;

namespace AdmissionFinal.Controllers
{
    [Authorize(Roles = "Hall")]
    public class HallController : Controller
    {
        //
        // GET: /Hall/
        AdmissionContext db = new AdmissionContext();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentInformation()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        public ActionResult StudentInformation(Student student)
        {
            if (ModelState.IsValid)
            {
                var user = db.aspnet_Users.Find(UserIdSaver.UserId);
                student.StudentId = Guid.NewGuid();
                student.aspnet_Users = user;
                student.UserId = UserIdSaver.UserId;

               // var currentuser = db.aspnet_Users.Single(m => m.UserName == User.Identity.Name);
                student.Hall = db.Halls.Single(m => m.HallName == User.Identity.Name);
                student.HallId = student.Hall.HallId;
                student.RegistrationNumber = (User.Identity.Name.ElementAt(0).ToString(CultureInfo.InvariantCulture) +
                    User.Identity.Name.ElementAt(1).ToString(CultureInfo.InvariantCulture)).ToUpper()
                    + db.Students.Count().ToString(CultureInfo.InvariantCulture);

                student.Session = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture) + "-"
                                  + (DateTime.Now.Year + 1).ToString(CultureInfo.InvariantCulture);

                db.Students.Add(student);

                var studentYearStatus = new StudentYearStatu();
                studentYearStatus.StudentYearStatusId = Guid.NewGuid();
                studentYearStatus.Year = 1;
                studentYearStatus.AdmissionPaid = 1;
                studentYearStatus.ExamPaid = 0;
                studentYearStatus.Student = student;
                studentYearStatus.StudentId = studentYearStatus.Student.StudentId;
                db.StudentYearStatus.Add(studentYearStatus);

                string userName = user.UserName;
                string password = UserIdSaver.Password;
                string message="User Name:"+userName+" \n"+"Password:"+password;
                new MessageSender(student.ContactNumber, message);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        public ActionResult AdmissionFeeIndex()
        {
            var admissionfee = db.AdmissionFees.
                Single(m => m.Hall.HallName == User.Identity.Name);

            return View(admissionfee);
        }

        public ActionResult AdmissionFeeEdit(Guid id)
        {
            var admissionFee = db.AdmissionFees.Find(id);
            return View(admissionFee);
        }

        [HttpPost]
        public ActionResult AdmissionFeeEdit(AdmissionFee admissionFee)
        {
            if(ModelState.IsValid)
            {
                admissionFee.Total = admissionFee.DevelopmentFee + admissionFee.HallUnionFee
                                     + admissionFee.RegistrationFee + admissionFee.OthersFee
                                     + admissionFee.SportsFee + admissionFee.StudentWelfareFee
                                     + admissionFee.TransportFee + admissionFee.TutionFee;
                db.Entry(admissionFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdmissionFeeIndex");    
            }

            return View(admissionFee);
        }

        public ActionResult ExamFeeIndex()
        {
            var examFee = db.ExamFees.Single(m => m.Hall.HallName == User.Identity.Name);
            return View(examFee);
        }

        public ActionResult ExamFeeEdit(Guid id)
        {
            var examFee = db.ExamFees.Find(id);
            return View(examFee);
        }

        [HttpPost]
        public ActionResult ExamFeeEdit(ExamFee examFee)
        {
            if(ModelState.IsValid)
            {
                examFee.Total = examFee.ExamFee1 + examFee.TranscriptFee 
                    + examFee.CentreFee;
                db.Entry(examFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ExamFeeIndex");
            }
            return View(examFee);
        }

        public ActionResult ShowStudentsIndex(string searchString)
        {
            var students = db.Students.Include(s => s.aspnet_Users).
                Include(s => s.Department).Include(s => s.Hall).
                Where(m => m.Hall.HallName == User.Identity.Name);

            if(!string.IsNullOrWhiteSpace(searchString))
            {
                students = db.Students.Include(s => s.aspnet_Users).
                 Include(s => s.Department).Include(s => s.Hall).
                 Where(m => m.Hall.HallName == User.Identity.Name && 
                     m.Name.Contains(searchString));     
            }
            
            return View(students.ToList());
        }

        public ActionResult EditStudent(Guid id)
        {
            var student = db.Students.Find(id);
           
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
           
            return View(student);
        }

        [HttpPost]
        public ActionResult EditStudent(Student student)
        {
            if(ModelState.IsValid)
            {

                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ShowStudentsIndex");
            }
            
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            
            return View(student);
        }

        public ActionResult StudentDetails(Guid id)
        {
            var student = db.Students.Include(s => s.aspnet_Users).
               Include(s => s.Department).Include(s => s.Hall).
               Single(m => m.StudentId == id);
            return View(student);
        }


    }
}
