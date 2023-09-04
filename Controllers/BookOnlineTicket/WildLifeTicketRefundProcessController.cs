using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.BookOnlineTicket;
using System.Data;
using FMDSS.Models;

namespace FMDSS.Controllers.BookOnlineTicket
{
    public class WildLifeTicketRefundProcessController : Controller
    {
        //
        // GET: /WideLifeTicketRefundProcess/

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            WildLifeTicketRefundProcessListModel model = new WildLifeTicketRefundProcessListModel();
            try
            {

                WildLifeTicketRefundProcessRepository repo = new WildLifeTicketRefundProcessRepository();
                DataSet ds = new DataSet();
                ds = repo.GETWildLifeTicketRefundProcess(model, "LIST");
                model.List.RemoveAll(s => string.IsNullOrEmpty(s.RequestID));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                   
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        WildLifeTicketRefundProcessModel obj = new WildLifeTicketRefundProcessModel();
                        obj.RequestID = Convert.ToString(dr["RequestID"].ToString());
                        obj.SSOId = Convert.ToString(dr["Ssoid"].ToString());
                        obj.TicketAmount = Convert.ToString(dr["TicketAmount"].ToString());
                        obj.ServiceCharge = Convert.ToString(dr["ServiceCharge"].ToString());
                        obj.RefundAmount = Convert.ToString(dr["RefundAmount"].ToString());
                        obj.ApplicationLevel = Convert.ToString(dr["ApplicationLevel"].ToString());
                        obj.Checked = false;
                        model.ButtonName = Convert.ToString(dr["ApplicationLevelName"].ToString());
                        model.List.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.InnerException + "_" + ex.StackTrace);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(WildLifeTicketRefundProcessListModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (model.List.Where(s => s.Checked == true).ToList().Count > 0)
                {
                    WildLifeTicketRefundProcessRepository repo = new WildLifeTicketRefundProcessRepository();
                    model.List = model.List.Where(s => s.Checked == true).ToList();
                    DataSet ds = new DataSet();
                    ds = repo.GETWildLifeTicketRefundProcess(model, "SubmitReviewApporve");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0]["Success"]) == 0)
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["Messages"]) + "</div>";

                    }
                    else
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["Messages"]) + "</div>";
                    }
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>Please select atleast one Ticket! </div>";
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                //Response.Write(ex.InnerException + "_" + ex.StackTrace);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("Index");
        }





    }
}
