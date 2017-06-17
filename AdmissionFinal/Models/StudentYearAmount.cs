using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using AdmissionFinal.Entity;

namespace AdmissionFinal.Models
{
    public class StudentYearAmount
    {
        public Guid StudentYearStatusId { get; set; }
        public string StudentName { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public decimal Fine { get; set; }
        public string RegistrationNumber { get; set;}
    }
}