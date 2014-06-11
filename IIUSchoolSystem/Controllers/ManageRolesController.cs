using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IIUSchoolSystem.Core.Authentication;
using IIUSchoolSystem.Core.Entities;
using IIUSchoolSystem.Core.Helpers;
using IIUSchoolSystem.Core.Logger;
using IIUSchoolSystem.Core.Repository;
using IIUSchoolSystem.Models;
using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Controllers
{
    [Authorize]
    public class ManageRolesController : BaseController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [GridAction(EnableCustomBinding = true)]
        public ActionResult Index(GridCommand command, UserModel model)
        {
            if (!PermissionService.Authorize(PermissionProvider.ManageRoles))
                return AccessDeniedView();

            int totalRecords = 0;
            var query = _unitOfWork.RoleRepository.GetAsQuerable(x => !x.Deleted).OrderByDescending(x => x.CreatedOn);
            var roles = query.ApplyGridCommandsWithPaging(command, out totalRecords);

            List<RoleModel> modelList = new List<RoleModel>();

            foreach (var role in roles)
            {
                RoleModel newRole = new RoleModel();
                newRole.Id = role.Id;
                newRole.Name = role.Name;
                newRole.Description = role.Description;
                newRole.Active = role.Active;
                newRole.CreatedOn = role.CreatedOn.ToString("MM/dd/yyyy");

                var temp = _unitOfWork.UserRepository.GetById(role.CreatedByUserId);
                if (temp != null)
                {
                    newRole.CreatedBy = temp.FirstName + " " + temp.LastName;
                }

                modelList.Add(newRole);
            }

            var gridModel = new GridModel<RoleModel>
            {
                Data = modelList,
                Total = totalRecords
            };
            return View(gridModel);
        }

        public JsonResult GetRole(string id)
        {
            try
            {
                var role = _unitOfWork.RoleRepository.GetById(Convert.ToInt32(id));
                Role editRole = new Role();
                editRole.Id = role.Id;
                editRole.Name = role.Name;
                editRole.Description = role.Description;
                editRole.Active = role.Active;
                return Json(new { success = true, Role = editRole });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, message = exception.Message });
            }
        }

        public JsonResult CreateOrUpdateRole(string Id, string Name, string Description, string Active)
        {
            try
            {
                int id = Convert.ToInt32(Id);
                if (id == 0)
                {
                    Role newRole = new Role();
                    newRole.Name = Name;
                    newRole.Description = Description;
                    newRole.Active = Convert.ToBoolean(Active);
                    newRole.LastUpdatedOn = DateTime.Now;
                    newRole.CreatedOn = DateTime.Now;
                    newRole.CreatedByUserId = MembershipContext.Current.User.Id;
                    newRole.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                    _unitOfWork.RoleRepository.Insert(newRole);
                    //DebugChangeTracker(id, _unitOfWork, "UpdateRoles", "Role");
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "Role created successfully." });
                }
                else
                {
                    var role = _unitOfWork.RoleRepository.GetSingle(x => x.Id == id);
                    role.Name = Name;
                    role.Description = Description;
                    role.Active = Convert.ToBoolean(Active);
                    role.LastUpdatedOn = DateTime.Now;
                    role.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                    _unitOfWork.RoleRepository.Update(role);
                    _unitOfWork.Save();

                }
                return Json(new { success = true, message = "Role updated successfully." });

            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, message = exception.Message });
            }
        }

        public JsonResult DeleteRole(string Id)
        {
            try
            {
                int id = Convert.ToInt32(Id);
                var role = _unitOfWork.RoleRepository.GetById(id);
                if (role.Users.Any())
                {
                    return Json(new { message = "The role cannot be deleted because it is assigned to a user.", success = false });
                }
                role.Deleted = true;
                role.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                role.LastUpdatedOn = DateTime.Now;
                _unitOfWork.RoleRepository.Update(role);
                //DebugChangeTracker(id, _unitOfWork, "DeleteRoles", "Role");
                _unitOfWork.Save();
                return Json(new { message = "Role deletion successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { message = exception.Message, success = false });
            }
        }
    }
}
