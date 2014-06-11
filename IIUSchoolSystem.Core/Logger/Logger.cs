using IIUSchoolSystem.Core.Authentication;
using IIUSchoolSystem.Core.Entities;
using IIUSchoolSystem.Core.Helpers;
using IIUSchoolSystem.Core.Repository;
using System;
using System.Web;

namespace IIUSchoolSystem.Core.Logger
{
    public static class Logger
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public static void LogException(Exception exception)
        {
            var errorLog = new ErrorLog
            {
                ShortMessage = exception.Message,
                FullMessage = exception.ToString(),
                IpAddress = HttpContext.Current == null ? "N/A" : HttpContext.Current.Request.UserHostAddress,
                PageUrl = HttpContext.Current == null ? "N/A" : HttpContext.Current.Request.Url.AbsoluteUri,
                UserId = (MembershipContext.Current == null || MembershipContext.Current.User == null) ? 0 : MembershipContext.Current.User.Id,
                CreatedOn = DateTime.Now
            };

            UnitOfWork.ErrorRepository.Insert(errorLog);
            UnitOfWork.Save();

            var sendErrorEmail = SettingManager.GetSettingValue("SendErrorEmail");
            if (sendErrorEmail.ToLower() == "true")
            {
                //var email = SettingManager.GetSettingValue("Email.ErrorEmailAddress");
                //EmailUtility.SendEmail("Error Email", ErrorEmailMessage(exception), new MailAddress(email), EmailType.Error);
            }
        }

        private static string ErrorEmailMessage(Exception exception)
        {
            string message = "<b>An exception is occured:</b><br/>" + exception.Message;

            return message;
        }
    }
}
