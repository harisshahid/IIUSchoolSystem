using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IIUSchoolSystem.Core.Entities;
using IIUSchoolSystem.Core.Helpers;
using IIUSchoolSystem.Core.Logger;
using IIUSchoolSystem.Core.Repository;
using IIUSchoolSystem.Models;
using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Controllers
{
    [Authorize]
    public class ManagePermissionsController : BaseController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /ManagePermissions/
        public ActionResult Index()
        {
            if (!PermissionService.Authorize(PermissionProvider.ManagePermissions))
                return AccessDeniedView();

            var roles = _unitOfWork.RoleRepository.Get();
            ViewBag.Roles = roles;
            var permissionRecords = _unitOfWork.PermissionRecordRepository.Get();
            var query1 = new PagedList<PermissionRecord>(permissionRecords.ToList(), 0, 100);
            PermissionRecordModel model = new PermissionRecordModel();
            model.RoleId = roles.FirstOrDefault().Id;
            model.PermissionRecord = new GridModel<PermissionRecordModel>
            {
                Data = query1.Select(g => new PermissionRecordModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    SystemName = g.SystemName,
                    Category = g.Category,
                    Allow = g.Roles.Any(x => x.Id == (model.RoleId)),
                    RoleId = model.RoleId
                }),
                Total = permissionRecords.Count
            };
            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Index(GridCommand command, PermissionRecordModel model)
        {
            if (!PermissionService.Authorize(PermissionProvider.ManagePermissions))
                return AccessDeniedView();

            try
            {
                var roles = _unitOfWork.RoleRepository.Get();
                ViewBag.Roles = roles;
                var permissionRecords = _unitOfWork.PermissionRecordRepository.Get();
                List<PermissionRecordModel> listPermissions = new List<PermissionRecordModel>();
                foreach (var item in permissionRecords)
                {
                    PermissionRecordModel newItem = new PermissionRecordModel();
                    newItem.Id = item.Id;
                    newItem.Name = item.Name;
                    newItem.SystemName = item.SystemName;
                    newItem.Category = item.Category;
                    newItem.Allow = item.Roles.Any(x => x.Id == (model.RoleId));
                    newItem.RoleId = model.RoleId;
                    listPermissions.Add(newItem);
                }

                return View(new GridModel<PermissionRecordModel>() { Data = listPermissions, Total = permissionRecords.Count });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return new JsonResult() { Data = new PermissionRecordModel().PermissionRecord };
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult UpdatePermissionRecord(int id, PermissionRecordModel model)
        {
            var prs = _unitOfWork.PermissionRecordRepository.GetSingle(x => x.Id == model.Id);
            var role = _unitOfWork.RoleRepository.GetSingle(x => x.Id == model.RoleId);
            if (model.Allow)
            {
                if (prs.Roles.FirstOrDefault(x => x.Id == model.RoleId) == null)
                {
                    prs.Roles.Add(role);
                    _unitOfWork.PermissionRecordRepository.Update(prs);
                    //DebugChangeTracker(id, _unitOfWork, "UpdatePermissions", "PermissionRecord");
                    _unitOfWork.Save();
                }
            }
            else
            {
                if (prs.Roles.FirstOrDefault(x => x.Id == model.RoleId) != null)
                {
                    prs.Roles.Remove(role);
                    _unitOfWork.PermissionRecordRepository.Update(prs);
                    //DebugChangeTracker(id, _unitOfWork, "UpdatePermissions", "PermissionRecord");
                    _unitOfWork.Save();
                }
            }
            IEnumerable<PermissionRecord> permissionRecords = null;
            permissionRecords = _unitOfWork.PermissionRecordRepository.Get();
            var enumerable = permissionRecords as List<PermissionRecord> ?? permissionRecords.ToList();
            var query1 = new PagedList<PermissionRecord>(enumerable.ToList(), 0, 10);
            PermissionRecordModel gridModel = new PermissionRecordModel();

            gridModel.PermissionRecord = new GridModel<PermissionRecordModel>
            {
                Data = query1.Select(g => new PermissionRecordModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    SystemName = g.SystemName,
                    Category = g.Category,
                    Allow = g.Roles.Any(x => x.Id == role.Id),
                    RoleId = role.Id
                }),
                Total = enumerable.Count()
            };
            return View(new GridModel(gridModel.PermissionRecord.Data));
        }
    }
}
