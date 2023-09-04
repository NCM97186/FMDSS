using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FMDSS.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateHeaderAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string clientToken = filterContext.RequestContext.HttpContext.Request.Headers.Get(KEY_NAME);
            if (clientToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Header does not contain {0}", KEY_NAME));
            }

            string serverToken = filterContext.HttpContext.Request.Cookies.Get(KEY_NAME).Value;
            if (serverToken == null)
            {
                throw new HttpAntiForgeryException(string.Format("Cookies does not contain {0}", KEY_NAME));
            }

            System.Web.Helpers.AntiForgery.Validate(serverToken, clientToken);
        }

        private const string KEY_NAME = "__RequestVerificationToken";
    }

    //if (filterContext == null)
    //{
    //    throw new ArgumentNullException("filterContext");
    //}

    //var httpContext = filterContext.HttpContext;
    //var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
    //AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
}

