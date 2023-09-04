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

            //UserProfile user = new UserProfile();
            //UserProfile userObj = user.AuthenticateUser_Local("fmdss.deptkiosk@gmail.com");
            ////if (Session["User"] != null)
            ////{

            //if (userObj != null)
            //{
            //    Session["SSODetail"] = userObj;
            //    Session["loggedin"] = true;
            //    Session["SSOid"] = userObj.SSOId.ToString();
            //    Session["User"] = userObj.FullName.ToString();
            //    Session["Role"] = userObj.Roles.ToString();
            //    Session["UserId"] = userObj.UserId.ToString();
            //    Response.Redirect("~/WebForm1.aspx?val=Staging", false);
            //    return null;
            //}
            //else
            //{ return View("Login"); }

           
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(UserProfile login)
        {

            UserProfile user = new UserProfile();
            UserProfile userObj = user.AuthenticateUser_Local(login.EmailId);

            //if (Session["User"] != null)
            //{

            if (userObj != null)
            {
                Session["SSODetail"] = userObj;
                Session["loggedin"] = true;
                Session["SSOid"] = userObj.SSOId.ToString();
                Session["User"] = userObj.FullName.ToString();
                Session["Role"] = userObj.Roles.ToString();
                Session["UserId"] = userObj.UserId.ToString();
                Session["AadharID"] = userObj.AadharId.ToString();
                Response.Redirect("~/WebForm1.aspx?val=Staging", false);
                return null;

            }
            else
            { return View("Login"); }
           // }
            //else
            //    return View("Login");
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
