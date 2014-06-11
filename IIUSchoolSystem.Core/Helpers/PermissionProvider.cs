using IIUSchoolSystem.Core.Entities;

namespace IIUSchoolSystem.Core.Helpers
{
    public class PermissionProvider
    {
        public static readonly PermissionRecord ErrorView = new PermissionRecord { Name = "View Application Errors", SystemName = "ErrorView", Category = "Standard" };
        public static readonly PermissionRecord PaymentView = new PermissionRecord { Name = "View Payments", SystemName = "PaymentView", Category = "Standard" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Access and Manage Settings", SystemName = "ManageSettings", Category = "Standard" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Access and Manage Users List", SystemName = "ManageUsers", Category = "Standard" };
        public static readonly PermissionRecord ManageRoles = new PermissionRecord { Name = "Access and Manage Roles  List", SystemName = "ManageRoles", Category = "Standard" };
        public static readonly PermissionRecord ManagePermissions = new PermissionRecord { Name = "Access and Permissions List", SystemName = "ManagePermissions", Category = "Standard" };
    }
}
