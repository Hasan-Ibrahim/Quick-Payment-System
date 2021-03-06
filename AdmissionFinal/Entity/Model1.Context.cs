﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace AdmissionFinal.Entity
{
    public partial class AdmissionContext : DbContext
    {
        public AdmissionContext()
            : this(false) { }
    
        public AdmissionContext(bool proxyCreationEnabled)	    
            : base("name=AdmissionContext")
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
    
        public AdmissionContext(string connectionString)
          : this(connectionString, false) { }
    
        public AdmissionContext(string connectionString, bool proxyCreationEnabled)
            : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }	
    
        public ObjectContext ObjectContext
        {
          get { return ((IObjectContextAdapter)this).ObjectContext; }
        }
    
    	protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    	
        public DbSet<AdmissionFee> AdmissionFees { get; set; }
        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ExamFee> ExamFees { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentYearStatu> StudentYearStatus { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
