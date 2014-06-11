using System;
using System.Globalization;
using System.Threading;
using System.Web;
using IIUSchoolSystem.Core.Entities;
using IIUSchoolSystem.Core.Repository;

namespace IIUSchoolSystem.Core.Authentication
{
    public class MembershipContext
    {
        private readonly HttpContext _context = HttpContext.Current;

        public MembershipContext()
        {
            try
            {
                if (User == null)
                {
                    var unitOfWork = new UnitOfWork();

                    var user = unitOfWork.UserRepository.GetSingle(t => t.Username == HttpContext.Current.User.Identity.Name && t.Active);
                    if (user != null && user.Id > 0)
                    {
                        User = user;
                    }
                }
            }
            catch (Exception)
            {
                //Logger.Error(ex);
            }
        }


        #region Methods

        /// <summary>
        /// 	Sets cookie
        /// </summary>
        /// <param name="application"> Application </param>
        /// <param name="key"> Key </param>
        /// <param name="val"> Value </param>
        private static void SetCookie(HttpApplication application, string key, string val)
        {
            var cookie = new HttpCookie(key) { Value = val, Expires = string.IsNullOrEmpty(val) ? DateTime.UtcNow.AddMonths(-1) : DateTime.UtcNow.AddHours(128) };
            application.Response.Cookies.Remove(key);
            application.Response.Cookies.Add(cookie);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 	Gets an instance of the Click2MeContext, which can be used to retrieve information about current context.
        /// </summary>
        public static MembershipContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                if (HttpContext.Current.Items["MembershipContext"] == null)
                {
                    MembershipContext context2 = new MembershipContext();
                    HttpContext.Current.Items.Add("MembershipContext", context2);
                    return context2;
                }
                return (MembershipContext)HttpContext.Current.Items["MembershipContext"];
            }
        }


        /// <summary>
        /// 	Gets or sets a value indicating whether the context is running in admin-mode
        /// </summary>
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsNormal { get; set; }
        public User User { get; set; }


        /// <summary>
        /// 	Gets or sets an object item in the context by the specified key.
        /// </summary>
        /// <param name="key"> The key of the value to get. </param>
        /// <returns> The value associated with the specified key. </returns>
        public object this[string key]
        {
            get
            {
                if (_context == null)
                {
                    return null;
                }

                if (_context.Items[key] != null)
                {
                    return _context.Items[key];
                }
                return null;
            }
            set
            {
                if (_context != null)
                {
                    _context.Items.Remove(key);
                    _context.Items.Add(key, value);
                }
            }
        }


        /// <summary>
        /// 	Sets the CultureInfo
        /// </summary>
        /// <param name="culture"> Culture </param>
        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        #endregion
    }
}
