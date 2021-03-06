//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IIUSchoolSystem.Core.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public long Id { get; set; }
        public System.Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public string Cell { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string LastIpAddress { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedByUserId { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public long LastUpdatedByUserId { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
