using System.Web.Mvc;
using IIUSchoolSystem.Core.Authentication;

namespace IIUSchoolSystem.Controllers
{
    public class SecurityController : Controller
    {
        public ActionResult AccessDenied(string pageUrl)
        {
            var currentUser = MembershipContext.Current.User;
            if (currentUser != null)
            {
                //Logger.LogException(string.Format("Access denied to anonymous request on {0}", pageUrl), "UnAuthorized Access action performed by user: " + string.Format("Access denied to user #{0} '{1}' on {2}", currentUser.Email, currentUser.Email, pageUrl));
                return View();
            }
            return View();
        }
    }
}
