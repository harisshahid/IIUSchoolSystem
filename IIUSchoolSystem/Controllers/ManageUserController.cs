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
    public class ManageUserController : BaseController
    {
        public ManageUserController()
        {
            ViewBag.States = GetStates();
            ViewBag.Roles = GetRoles();
        }
        //
        // GET: /ManageUser/
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [GridAction(EnableCustomBinding = true)]
        public ActionResult Index(GridCommand command, UserModel model)
        {
            if (!PermissionService.Authorize(PermissionProvider.ManageUsers))
                return AccessDeniedView();

            int totalRecords = 0;
            var query = _unitOfWork.UserRepository.GetAsQuerable(x => !x.Deleted).OrderByDescending(x => x.CreatedOn);

            var users = query.ApplyGridCommandsWithPaging(command, out totalRecords);

            var modelList = new List<UserModel>();

            foreach (var user in users)
            {
                var newUser = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Username,
                    Email = user.Email,
                    Role = user.Role.Name,
                    Active = user.Active
                };
                var temp = _unitOfWork.UserRepository.GetById(user.CreatedByUserId);
                if (temp != null)
                {
                    newUser.CreatedBy = temp.FirstName + " " + temp.LastName;
                }
                newUser.CreatedOn = user.CreatedOn.ToString("MM/dd/yyyy");
                modelList.Add(newUser);
            }

            var gridModel = new GridModel<UserModel>
            {
                Data = modelList,
                Total = totalRecords
            };

            return View(gridModel);
        }

        public JsonResult CreateOrUpdateUser(RegisterModel model)
        {
            try
            {
                var id = Convert.ToInt64(model.Id);

                //update the user
                if (id != 0)
                {
                    var user = _unitOfWork.UserRepository.GetSingle(x => x.Id.Equals(id) && !x.Deleted);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Phone = model.Phone;
                        user.Cell = model.Cell;
                        user.Username = model.UserName.Trim();
                        user.Password = model.Password;
                        user.Email = model.Email.Trim();
                        user.Address = model.Address;
                        user.City = model.City.Trim();
                        user.ZipCode = model.Zip.ToString();
                        user.StateId = model.StateId;
                        user.RoleId = Convert.ToInt32(model.RoleId);
                        user.Active = model.Active;
                        user.LastUpdatedOn = DateTime.Now;
                        user.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                        user.UserGuid = new Guid(model.GUID);

                        _unitOfWork.UserRepository.Update(user);

                        //DebugChangeTracker(id, _unitOfWork, "UpdateUser", "User");

                        _unitOfWork.Save();
                        return Json(new { success = true, message = "User updated successfully." });
                    }
                }
                else
                {
                    // new user
                    var newUserAvailable = _unitOfWork.UserRepository.Get(x => x.Username.Equals(model.UserName.Trim()));
                    if (newUserAvailable.Count > 0)
                    {
                        return Json(new { success = false, message = "User name already exist. Please try another one." });
                    }
                    var newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Cell = model.Cell,
                        Username = model.UserName.Trim(),
                        Email = model.Email.Trim(),
                        Password = model.Password,
                        Address = model.Address,
                        City = model.City.Trim(),
                        ZipCode = model.Zip.ToString(),
                        StateId = model.StateId,
                        RoleId = Convert.ToInt32(model.RoleId),
                        Active = model.Active,
                        UserGuid = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now,
                        CreatedByUserId = MembershipContext.Current.User.Id,
                        LastUpdatedByUserId = MembershipContext.Current.User.Id
                    };
                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    return Json(new { success = true, message = "User created successfully." });
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, message = exception.Message });
            }
            return null;
        }

        public JsonResult GetUser(string id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(Convert.ToInt32(id));
                var editUser = new RegisterModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    Cell = user.Cell,
                    Address = user.Address,
                    City = user.City,
                    Zip = Convert.ToInt32(user.ZipCode),
                    StateId = Convert.ToInt32(user.StateId ?? 0),
                    Password = user.Password,
                    RoleId = user.RoleId,
                    Active = user.Active,
                    GUID = user.UserGuid.ToString()
                };
                return Json(new { success = true, User = editUser });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { success = false, error = exception.Message });
            }
        }

        public JsonResult Delete(string Id)
        {
            try
            {
                long id = Convert.ToInt32(Id);
                if (id == MembershipContext.Current.User.Id)
                {
                    return Json(new { message = "Sorry! you are logged-in at the moment.", success = false });
                }

                var user = _unitOfWork.UserRepository.GetById(id);
                user.Deleted = false;
                user.LastUpdatedOn = DateTime.Now;
                user.LastUpdatedByUserId = MembershipContext.Current.User.Id;
                _unitOfWork.UserRepository.Update(user);
                //DebugChangeTracker(id, _unitOfWork, "DeleteUser", "User");
                _unitOfWork.Save();

                return Json(new { message = "User deletion successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return Json(new { message = exception.Message, success = false });
            }
        }
    }
}
