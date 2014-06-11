using System;
using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string GUID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public GridModel<UserModel> Users { get; set; }
    }

    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public Nullable<int> LastUpdatedByUserId { get; set; }
        public GridModel<RoleModel> Roles { get; set; }
        public string Description { get; set; }
    }
}