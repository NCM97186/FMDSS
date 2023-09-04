using FMDSS.Models;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class CustomizedDashbaordController : Controller
    {
        //
        // GET: /CustomizedDashbaord/

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<CustomizedDashbaordModel> model = new List<CustomizedDashbaordModel>();
            CustomizedDashbaordRepo repo = new CustomizedDashbaordRepo();
            try
            {
                model = repo.ModuleName("GET", UserID, model);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(List<CustomizedDashbaordModel> list)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<CustomizedDashbaordModel> model = new List<CustomizedDashbaordModel>();
            CustomizedDashbaordRepo repo = new CustomizedDashbaordRepo();
            try
            {
                model = repo.ModuleName("Save", UserID, list);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return RedirectToAction("ForestDashboard", "SystemAdmin");
        }
    }
}
