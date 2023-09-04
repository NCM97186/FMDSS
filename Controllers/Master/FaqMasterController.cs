//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FaqMasterController
//  Description  : FAQ Master
//  Date Created : 24-03-2017
//  History      :
//  Version      : 1.0
//  Author       : Arvind Kumar Sharma
//  Modified By  : Arvind Kumar Sharma
//  Modified On  : 24-03-2017
//  Reviewed By  : Amit singh rajput 
//  Reviewed On  : 24-03-2017
//********************************************************************************************************
using FMDSS.Models;
using FMDSS.Models.ForesterAction;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class FaqMasterController : Controller
    {

        List<FAQ> FAQLst = new List<FAQ>();
        FAQ objFAQ = new FAQ();


        #region "FAQ"
        public ActionResult Ticker(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
           
            try
            {

                DataTable dtf = objFAQ.Select_FAQs();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    FAQLst.Add(
                        new FAQ()
                        {
                            Index = count,
                            FAQId = Convert.ToInt32(dr["TickerId"]),
                            QUERY = Convert.ToString(dr["TickerMessage"].ToString()),
                            QUERYANSWER = Convert.ToString(dr["TickerMessage"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View();
        }
        public ActionResult AddUpdateTicker(Ticker oTicker)
        {
            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Ticker obj = new Ticker();

                DataTable dtf = obj.AddUpdateTicker(oTicker);
                //oTicker.LastUpdatedBy = UserID;
                //status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Ticker", new { RecordStatus = status });
        }
        public ActionResult GetTicker(string TickerId)
        {

            Ticker obj = new Ticker();
            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (TickerId == "0" ? "Add Ticker" : "Edit Ticker");


                DataTable dtf = obj.Select_Ticker(Convert.ToInt32(TickerId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Ticker
                    {
                        TickerId = Convert.ToInt64(dr["TickerId"].ToString()),
                        TickerMessage = Convert.ToString(dr["TickerMessage"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit Ticker"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialTicker", obj);
        }


        #endregion


    }
}
