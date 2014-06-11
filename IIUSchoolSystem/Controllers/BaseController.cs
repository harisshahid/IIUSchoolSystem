using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IIUSchoolSystem.Core.Helpers;
using IIUSchoolSystem.Core.Repository;

namespace IIUSchoolSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected PermissionService PermissionService = new PermissionService();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        protected ActionResult AccessDeniedView()
        {
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }

        protected List<SelectListItem> GetStates()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var states = _unitOfWork.StateRepository.Get();
            if (states.Any())
            {
                items.AddRange(states.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }));
            }

            return items;
        }

        protected List<SelectListItem> GetRoles()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var roles = _unitOfWork.RoleRepository.Get(x => x.Active);

            if (roles.Any())
            {
                items.AddRange(roles.Select(x => new SelectListItem
                {
                    Text = x.Name.CamelCaseToWords(),
                    Value = x.Id.ToString()
                }));
            }

            return items;
        }
    }
}
