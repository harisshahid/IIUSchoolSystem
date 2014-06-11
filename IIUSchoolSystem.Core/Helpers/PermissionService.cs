using System;
using IIUSchoolSystem.Core.Authentication;
using IIUSchoolSystem.Core.Entities;

namespace IIUSchoolSystem.Core.Helpers
{
    public class PermissionService
    {
        public bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, MembershipContext.Current.User);
        }

        public virtual bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;
            return Authorize(permission.SystemName, user);
        }

        public virtual bool Authorize(string permissionRecordSystemName, User user)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var userRole = user.Role;
            if (userRole.Active)
            {
                if (Authorize(permissionRecordSystemName, userRole))
                    //yes, we have such permission
                    return true;
            }
            //no permission found
            return false;
        }

        protected virtual bool Authorize(string permissionRecordSystemName, Role userRole)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            foreach (var permission1 in userRole.PermissionRecords)
                if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;

        }
    }
}
