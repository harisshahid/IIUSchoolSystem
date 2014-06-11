using System.Collections.Generic;
using IIUSchoolSystem.Core.Helpers;
using IIUSchoolSystem.Core.Repository;
using IIUSchoolSystem.Models;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace IIUSchoolSystem.Controllers
{
    [Authorize]
    public class ErrorLogController : BaseController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        //
        // GET: /ErrorLog/

        [GridAction(EnableCustomBinding = true)]
        public ActionResult Index(GridCommand command, ErrorLogModel model)
        {
            if (!PermissionService.Authorize(PermissionProvider.ErrorView))
                return AccessDeniedView();

            int totalRecords = 0;
            var query = _unitOfWork.ErrorRepository.GetAsQuerable().OrderByDescending(x => x.CreatedOn);
            var errorLog = query.ApplyGridCommandsWithPaging(command, out totalRecords);

            var errorList = errorLog.Select(error => new ErrorLogModel
            {
                Id = error.Id,
                IpAddress = error.IpAddress,
                PageUrl = error.PageUrl,
                ShortMessage = error.ShortMessage,
                FullMessage = error.FullMessage,
                CreatedOn = error.CreatedOn
            }).ToList();

            var gridModel = new GridModel<ErrorLogModel>
            {
                Data = errorList,
                Total = totalRecords
            };

            return View(gridModel);
        }
    }
}
