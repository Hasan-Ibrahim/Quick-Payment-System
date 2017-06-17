using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdmissionFinal.Entity;
using System.Data.Entity;
using AdmissionFinal.Models;

namespace AdmissionFinal.Controllers
{
    [Authorize(Roles = "Bank")]
    public class BankController : Controller
    {
        //
        // GET: /Bank/

        AdmissionContext db = new AdmissionContext();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ExamFeeReceive(string searchString)
        {
            List<StudentYearAmount> data = new List<StudentYearAmount>();

            
            var studentYearStatus = db.StudentYearStatus.Include(s => s.Student).
                Where(m => m.ExamPaid == 0 && m.AdmissionPaid == 1).ToList();
            
            
            
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                studentYearStatus = studentYearStatus.Where(s => 
                 s.Student.Name.Contains(searchString)).ToList();
            }

            foreach (var item in studentYearStatus)
            {
                var examFee = db.ExamFees.Include(s => s.Hall).
                    Single(s => s.HallId == item.Student.HallId);
                int dateRange = (DateTime.Now.Day - examFee.LastDate.Day);
                decimal fine;
                if (dateRange <= 0)
                {
                    fine = 0;
                }
                else
                {
                    fine = dateRange * 10;
                }

                var studentYearAmount = new StudentYearAmount();

                studentYearAmount.StudentName = item.Student.Name;
                Guid studentId = item.Student.StudentId;
                studentYearAmount.StudentYearStatusId = item.StudentYearStatusId;
                Guid hallId = db.Students.Find(studentId).HallId;
                studentYearAmount.Amount = (decimal)db.ExamFees.Single(x => x.HallId == hallId).Total + fine;
                studentYearAmount.Year = item.Year;
                studentYearAmount.Fine = fine;
                studentYearAmount.RegistrationNumber = item.Student.RegistrationNumber;

                
                data.Add(studentYearAmount);
            }
         
            return View(data);
        }

        public ActionResult AdmissionFeeReceive(string searchString)
        {
            var data = new List<StudentYearAmount>();

            var studentYearStatus = db.StudentYearStatus.Include(s => s.Student).
                Where(m => m.AdmissionPaid == 0).ToList();
            
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                studentYearStatus = studentYearStatus.Where(s =>
                 s.Student.Name.Contains(searchString)).ToList();
            }
            foreach (var item in studentYearStatus)
            {
                var admissionFee = db.AdmissionFees.Include(h => h.Hall).
                    Single(m => m.HallId == item.Student.HallId);
                var dateRange = DateTime.Now.Day - admissionFee.LastDate.Day;
                decimal fine = 0 ;
                if(dateRange > 0)
                {
                    fine = dateRange*10;
                }
                var studenYearamount = new StudentYearAmount();

                studenYearamount.StudentName = item.Student.Name;
                Guid studentId = item.Student.StudentId;
                studenYearamount.StudentYearStatusId = item.StudentYearStatusId;
                Guid hallId = db.Students.Find(studentId).HallId;
                studenYearamount.Amount = (decimal)db.AdmissionFees.Single(x => x.HallId == hallId).Total + fine ;
                studenYearamount.Year = item.Year;
                studenYearamount.RegistrationNumber = item.Student.RegistrationNumber;
                studenYearamount.Fine = fine;

                data.Add(studenYearamount);
            }
            return View(data);
        }

        public ActionResult ExamAmountReceived(Guid id)
        {
            var studentYearStatus = db.StudentYearStatus.Include(s => s.Student).
                Single(m => m.StudentYearStatusId == id);
            studentYearStatus.ExamPaid = 1;

            db.Entry(studentYearStatus).State = EntityState.Modified;


            var transaction = new Transaction();
            transaction.TransactionId = Guid.NewGuid();
            transaction.Date = DateTime.Now;
            transaction.Student = studentYearStatus.Student;
            transaction.StudentId = transaction.Student.StudentId;

            AdmissionFee amount = db.AdmissionFees.Include(s => s.Hall).
                Single(m => m.HallId == studentYearStatus
                .Student.HallId);

            transaction.Amount = amount.Total;
            db.Transactions.Add(transaction);

            db.SaveChanges();

            return View();
        }

        public ActionResult AdmissionAmountReceived(Guid id)
        {
            var studentYearStatus = db.StudentYearStatus.Include(s => s.Student).
                Single(m => m.StudentYearStatusId == id);
            studentYearStatus.AdmissionPaid = 1;
            db.Entry(studentYearStatus).State = EntityState.Modified;
            

            var transaction = new Transaction();
            transaction.TransactionId = Guid.NewGuid();
            transaction.Date = DateTime.Now;
            transaction.Student = studentYearStatus.Student;
            transaction.StudentId = transaction.Student.StudentId;

            AdmissionFee amount = db.AdmissionFees.Include(s => s.Hall).
                Single(m => m.HallId == studentYearStatus.Student.HallId);

            transaction.Amount = amount.Total;
            db.Transactions.Add(transaction);

            db.SaveChanges();
            

            return View();
        }

        public ActionResult ViewTransactions(string searchString)
        {
            var transaction = db.Transactions.Include(s => s.Student).ToList();
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                transaction = transaction.Where(s => 
                    s.Student.Name.Contains(searchString)).ToList();
            }
            return View(transaction);
        }
    }
}
