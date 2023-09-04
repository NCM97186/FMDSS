using System;
using System.Data.Entity;
using System.Threading;
using System.Web.Mvc;
namespace FMDSS.Filters
{ 
    public class MyAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var isLoggedIn = Convert.ToBoolean(session["loggedin"]);
            if (!isLoggedIn)
            {
                var action = filterContext.ActionDescriptor;
                if (!action.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    filterContext.Result = new HttpUnauthorizedResult();                   
                }
            }
        }
    }
}
