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
using System.Runtime.Serialization;


namespace AdmissionFinal.Entity
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(aspnet_Applications))]
    [KnownType(typeof(aspnet_Users))]
    public partial class aspnet_Roles
    {
        public aspnet_Roles()
        {
            this.aspnet_Users = new HashSet<aspnet_Users>();
        }
    
        [DataMember]
        public System.Guid ApplicationId { get; set; }
        [DataMember]
        public System.Guid RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string LoweredRoleName { get; set; }
        [DataMember]
        public string Description { get; set; }
    
        [DataMember]
        public virtual aspnet_Applications aspnet_Applications { get; set; }
        [DataMember]
        public virtual ICollection<aspnet_Users> aspnet_Users { get; set; }
    }
    
}