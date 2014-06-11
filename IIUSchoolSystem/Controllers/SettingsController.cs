using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SettingsController : BaseController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //
        // GET: /Settings/

        [GridAction(EnableCustomBinding = true)]
        public ActionResult Index(GridCommand command, UserModel model)
        {
            if (!PermissionService.Authorize(PermissionProvider.ManageSettings))
                return AccessDeniedView();

            int totalRecords = 0;
            var query = _unitOfWork.SettingRepository.GetAsQuerable(x => !x.Deleted).OrderByDescending(x => x.CreatedOn);
            var settings = query.ApplyGridCommandsWithPaging(command, out totalRecords);

            var listModel = new List<SettingModel>();

            foreach (var topic in settings)
            {
                var setting = new SettingModel
                {
                    Id = topic.Id,
                    SettingName = topic.SettingName,
                    Value = topic.Value,
                    Description = topic.Description
                };
                var user1 = _unitOfWork.UserRepository.GetById(topic.CreatedByUserId);
                if (user1 != null)
                {
                    setting.CreatedBy = user1.FirstName;
                }
                listModel.Add(setting);
            }

            var gridModel = new GridModel<SettingModel>
            {
                Data = listModel,
                Total = totalRecords
            };

            return View(gridModel);
        }

        public JsonResult GetSettings(string id)
        {
            try
            {
                var setting = _unitOfWork.SettingRepository.GetById(Convert.ToInt32(id));
                var editSetting = new SettingModel
                {
                    Id = setting.Id,
                    SettingName = setting.SettingName,
                    Value = setting.Value,
                    Description = setting.Description
                };
                return Json(new { success = true, Setting = editSetting });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, error = exception.Message });
            }
        }

        public JsonResult CreateOrUpdateSettings(SettingModel model)
        {
            try
            {
                //update the Setting
                if (model.Id != 0)
                {
                    var setting = _unitOfWork.SettingRepository.GetById(model.Id);
                    if (setting != null)
                    {
                        setting.SettingName = model.SettingName.Trim();
                        setting.Value = model.Value.Trim();
                        setting.Description = model.Description;
                        setting.LastUpdatedOn = DateTime.Now;
                        setting.LastUpdatedByUserId = MembershipContext.Current.User.Id;

                        _unitOfWork.SettingRepository.Update(setting);
                        //DebugChangeTracker(model.Id, _unitOfWork, "UpateSetting", "Settings");
                        _unitOfWork.Save();
                        return Json(new { success = true, message = "Setting updated successfully." });
                    }
                }
                else
                {
                    // new user
                    var settings = _unitOfWork.SettingRepository.Get(x => x.SettingName.Equals(model.SettingName.Trim()));
                    if (settings.Count > 0)
                    {
                        return Json(new { success = false, message = "Setting already exist. Please try another one." });
                    }
                    var newSettings = new Setting
                    {
                        SettingName = model.SettingName.Trim(),
                        Value = model.Value.Trim(),
                        Description = model.Description,
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now,
                        CreatedByUserId = MembershipContext.Current.User.Id,
                        LastUpdatedByUserId = MembershipContext.Current.User.Id
                    };
                    _unitOfWork.SettingRepository.Insert(newSettings);
                    _unitOfWork.Save();

                    return Json(new { success = true, message = "Setting created successfully." });
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, message = exception.Message });
            }
            return null;
        }

        public JsonResult Delete(string Id)
        {
            try
            {
                int id = Convert.ToInt32(Id);
                var setting = _unitOfWork.SettingRepository.GetById(id);
                setting.Deleted = true;
                setting.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                setting.LastUpdatedOn = DateTime.Now;
                _unitOfWork.SettingRepository.Update(setting);
                //DebugChangeTracker(id, _unitOfWork, "DeleteSetting", "Settings");
                _unitOfWork.Save();


                return Json(new { message = "Setting deleted successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { message = exception.Message, success = false });
            }
        }
    }
}
