using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.ProductionServices;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class BuyerRegisterController : BaseController
    {
        //
        // GET: /RevenueManage/
        #region global variables

        Location location = new Location();

        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> revinueItem = new List<SelectListItem>();
        List<SelectListItem> produce = new List<SelectListItem>();
        List<SelectListItem> rangeCode = new List<SelectListItem>();
        int ModuleID = 3;
        Int64 UserID = 0;
        NoticeManagement notice = new NoticeManagement();
        NoticeManagement obj_notice = new NoticeManagement();
        #endregion

        public ActionResult BuyerRegister()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dt = new DataTable();
            try
            {
                dt = location.BindDivision();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }

                ViewBag.DivisionCode = items;



                DataTable dtp = new DataTable();
                dtp = obj_notice.BindProductType();
                ViewBag.fname1 = dtp;
                foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                {
                    produce.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.ForestProduceID = produce;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }



        /// <summary>
        /// Function to bind all Depot to dropdownlist
        /// </summary>
        /// <param name="divCode"></param>
        /// <param name="ranCode"></param>
        /// <param name="villCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getRange(string divisionCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {

                if (!String.IsNullOrEmpty(divisionCode))
                {
                    DataTable dt = new DataTable();
                    dt = location.BindRangeBydivisionCode(divisionCode);



                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));

        }


        /// <summary>
        /// Function to bind all Depot to dropdownlist
        /// </summary>
        /// <param name="divCode"></param>
        /// <param name="ranCode"></param>
        /// <param name="villCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getDepot(string divisionCode, string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {

                if (!String.IsNullOrEmpty(divisionCode) && !String.IsNullOrEmpty(rangeCode))
                {
                    DataTable dt = new DataTable();
                    dt = location.BindDepot(divisionCode, rangeCode);



                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Depot_Name"].ToString(), Value = @dr["Depot_Id"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));

        }



        /// <summary>
        /// dtToViewBagJSON
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    //DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }





        /// <summary>
        /// Function to Save depot details to database 
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns>Saved depot ID</returns>
        [HttpPost]
        public ActionResult addBuyers(FormCollection fm, HttpPostedFileBase IDProof, string Command)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string DocMOMfile = string.Empty;
            var DocMOMfilepath = "";
            ActionResult actionResult = null;

            try
            {

                BuyerObject _objBuyer = new BuyerObject();

                _objBuyer.BuyerID = 0;
                DateTime now = DateTime.Now;
                string requesteId = now.Ticks.ToString();

                _objBuyer.RequestedId = requesteId;

                if (Session["UserId"] != null)
                {
                    _objBuyer.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                if (Convert.ToString(fm["IsTendupatta"]) == "")
                {
                    _objBuyer.IsTendupatta = "";
                }
                else
                {
                    _objBuyer.IsTendupatta = fm["IsTendupatta"].ToString();
                }

                if (Convert.ToString(fm["DivisionCode"]) == "")
                {
                    _objBuyer.DivisionCode = "";
                }
                else
                {
                    _objBuyer.DivisionCode = fm["DivisionCode"].ToString();
                }

                if (Convert.ToString(fm["RangeCode"]) == "" || fm["RangeCode"] == null)
                {
                    _objBuyer.RangeCode = "";
                }
                else
                {
                    _objBuyer.RangeCode = fm["RangeCode"].ToString();
                }

                if (Convert.ToString(fm["DepotId"]) == "" || fm["DepotId"] == null)
                {
                    _objBuyer.DepotId = 0;
                }
                else
                {
                    _objBuyer.DepotId = Convert.ToInt64(fm["DepotId"].ToString());
                }

                if (Convert.ToString(fm["Durationfrom"]) == "")
                {
                    _objBuyer.DurationFrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    _objBuyer.DurationFrom = DateTime.ParseExact(fm["Durationfrom"].ToString(), "dd/MM/yyyy", null);
                }
                if (Convert.ToString(fm["Durationto"]) == "")
                {
                    _objBuyer.DurationTo = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    _objBuyer.DurationTo = DateTime.ParseExact(fm["Durationto"].ToString(), "dd/MM/yyyy", null);
                }

                if (IDProof != null && IDProof.ContentLength > 0)
                {
                    DocMOMfile = Path.GetFileName(IDProof.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DocMOMfile;
                    DocMOMfilepath = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    _objBuyer.IDProof = FileFullName;

                    _objBuyer.IDProofPath = @"~/PermissionDocument/" + FileFullName.Trim();
                    IDProof.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    _objBuyer.IDProof = "";
                    _objBuyer.IDProofPath = "";
                }

                if (Command == "Submit")
                {

                    Int64 status = _objBuyer.InsertBuyerDetails();

                    if (status > 0)
                    {

                        TempData["Buyer_Status"] = "Your request for Buyer Registartion has been saved successfully with the Requested Id:" + requesteId;
                        actionResult = RedirectToAction("BuyerRegister", "BuyerRegister");
                    }
                    else
                    {
                        TempData["Buyer_Status"] = "You are already registered into the system as Buyer!";
                        actionResult = RedirectToAction("BuyerRegister", "BuyerRegister");
                    }

                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult; ;
        }


    }
}
