using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.BookOnlineTicket
{
    public class OnlineBookingSeatsController : Controller
    {
        //
        // GET: /OnlineBookingSeats/

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            OnlineBookingSeatsModel model = new OnlineBookingSeatsModel();
            OnlineBookingSeatsRepo repo = new OnlineBookingSeatsRepo();
            try
            {
                model = repo.Select_BookedTicketSeats(model, "GET");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(OnlineBookingSeatsModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            OnlineBookingSeatsRepo repo = new OnlineBookingSeatsRepo();
            try
            {
                model = repo.Save_BookedTicketSeats(model, "SAVE");
                TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> (" + model.msg + ") </div>";
                SMSandEMAILtemplate obj = new SMSandEMAILtemplate();
                if (model != null && model.Status == 1)
                {
                        try
                        {
                            obj.SendMailComman("ALL", "OnlineBookingSeatsChanges", DateTime.Now.ToString(), model.msg, string.Empty, string.Empty, string.Empty, string.Empty, model.TotalNoOfGypsyFD.ToString(), model.TotalNoOfGypsyHD.ToString(), model.TotalNoOfGypsyVIP.ToString());
                        }
                        catch (Exception ex)
                        {
                            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                        }
                }
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("Index");
        }
    }
}
