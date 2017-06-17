//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace AdmissionFinal.Entity
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Hall))]
    public partial class AdmissionFee
    {
        [DataMember]
        public System.Guid AdmissionFeeId { get; set; }
        [DataMember]
        [Display(Name = "Registration Fee")]
        public decimal RegistrationFee { get; set; }
        [DataMember]
        [Display(Name = "Tution Fee")]
        public decimal TutionFee { get; set; }
        [DataMember]
        [Display(Name = "Transport Fee")]
        public decimal TransportFee { get; set; }
        [DataMember]
        [Display(Name = "Sports Fee")]
        public decimal SportsFee { get; set; }
        [DataMember]
        [Display(Name = "Hall Union Fee")]
        public decimal HallUnionFee { get; set; }
        [DataMember]
        [Display(Name = "Student Welfare Fee")]
        public decimal StudentWelfareFee { get; set; }
        [DataMember]
        [Display(Name = "Development Fee")]
        public decimal DevelopmentFee { get; set; }
        [DataMember]
        [Display(Name = "Others Fee")]
        public decimal OthersFee { get; set; }
        [DataMember]
        [Display(Name = "Hall Name")]
        public System.Guid HallId { get; set; }
        [DataMember]
        [Display(Name = "Last Date")]
        public System.DateTime LastDate { get; set; }
        [DataMember]
        [Display(Name = "Total")]
        public Nullable<decimal> Total { get; set; }
    
        [DataMember]
        public virtual Hall Hall { get; set; }
    }
    
}