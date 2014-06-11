using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Models
{
    public class PermissionRecordModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public bool Allow { get; set; }
        public int RoleId { get; set; }
        public GridModel<PermissionRecordModel> PermissionRecord { get; set; }
    }
}