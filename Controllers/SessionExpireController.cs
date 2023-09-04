using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class SessionExpireController : Controller
    {
        //
        // GET: /SessionExpire/

        public void Index()
        {
            Session.Clear();
            Response.Redirect("https://sso.rajasthan.gov.in/signin", true);
        }
        [HttpPost]
        public ActionResult PageSessionExpire() {

            Session.Clear();
            return View("_SessionExpire");
        }


        public ActionResult Information()
        {

            #region OnlineBookingPopUp Developed by Rajveer

            DataSet ds = new DataSet();
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            OnlineBookingPopUp model = new OnlineBookingPopUp();
            model.ModuleName = "LandingPage";
            ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
            //Ticker obj1 = new Ticker();
            // DataTable dt = obj1.OnlineBookingPopUp();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                ViewData["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
            }
            else
            {
                ViewData["PopUpContent"] = string.Empty;
                ViewData["PopUpContentStatus"] = string.Empty;
                ViewData["Title"] = string.Empty;

            }
            #endregion
            return View();
        }

    }
}
