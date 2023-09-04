using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using System.Data;
using FMDSS.Filters;

namespace FMDSS.Controllers.UserProfiling
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(UserProfile login)
        {
            UserProfile user = new UserProfile();
            UserProfile userObj = user.AuthenticateUser_Local(login.EmailId);
            if (userObj != null)
            {
                Session["SSODetail"] = userObj;
                Session["loggedin"] = true;
                Session["SSOid"] = userObj.SSOId.ToString();
                Session["User"] = userObj.FullName.ToString();
                Session["Role"] = userObj.Roles.ToString();
                Session["UserId"] = userObj.UserId.ToString();
              //  Response.Redirect("~/WebForm1.aspx?ssoToken=" + Session["SSOTOKEN"].ToString() + "&val=logout", false);
               Response.Redirect("~/WebForm1.aspx", false);
                //Response.Redirect("~/dashboard/dashboard");
                return null;
            }
            else
                return View("Login");
        }

        [MyAuthorization]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Login", false);

        }

    }
}
