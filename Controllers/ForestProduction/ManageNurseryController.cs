//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageNurseryController
//  Description  : File contains calling functions from UI
//  Date Created : 29-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using FMDSS.Models.Admin;
using FMDSS.Filters;
using FMDSS.Models.ForestProduction;
using FMDSS.Models;

namespace FMDSS.Controllers.ForestProduction
{
    [MyAuthorization]
    public class ManageNurseryController : BaseController
    {
        #region Data Members

        Location objLocation = new Location();
        List<SelectListItem> Range = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<NurseryManagement> lstNurseries = new List<NurseryManagement>();
        List<SelectListItem> district = new List<SelectListItem>();

        #endregion

        #region Member Functions

        /// <summary>
        /// Renders UI for Nursery Management
        /// </summary>
        /// <returns>View for  Nursery Management</returns>
        public ActionResult ManageNursery()
        {
            NurseryManagement _objNursery = new NurseryManagement();
            try
            {
                Location location = new Location();
                DataTable dt = location.BindCircle();
                ViewBag.fname = dt;

                ViewBag.ddlVillagesdropdown = new List<SelectedListItem>();

                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                }
                ViewBag.CircleCode = Range;

                //nEEDS WORK DIPAK
                Range = new List<SelectListItem>();
                DataTable dt1 = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
                foreach (System.Data.DataRow dr in dt1.Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.ddlRange = Range;




                dt = _objNursery.FetchNurseries();

                foreach (DataRow dr in dt.Rows)
                {
                    lstNurseries.Add(
                        new NurseryManagement()
                        {
                            ddlDistricts = dr["DIST_CODE"].ToString(),
                            districtName = dr["DIST_NAME"].ToString(),
                            //ddlBlocks = dr["BLK_CODE"].ToString(),
                            //blkName = dr["BLK_NAME"].ToString(),
                            // ddlGPs = dr["GP_CODE"].ToString(),
                            // gpName = dr["GP_NAME"].ToString(),
                            RangeName = dr["RANGE_NAME"].ToString(),
                            ddlVillages = dr["VILL_CODE"].ToString(),
                            villName = dr["VILL_NAME"].ToString(),
                            nurseryCode = dr["NURSERY_CODE"].ToString(),
                            nurseryName = dr["NURSERY_NAME"].ToString(),
                            nurseryType = dr["NURSERY_TYPE"].ToString(),
                            // statusid = dr["StatusID"].ToString(),
                            statusDesc = dr["StatusDesc"].ToString(),
                            nurseryNumber = Convert.ToInt32(dr["Rowid"]),
                            ActiveStatus = Convert.ToBoolean(dr["ActiveStatus"])
                        });
                }
                ViewData["lstNurseries"] = lstNurseries;

                _objNursery.ActiveStatusCitizenANDDeptUser = new List<CitizenOrDeptModel>{
                 new CitizenOrDeptModel{ID = 1, Name = "Citizen", Checked = false},
                 new CitizenOrDeptModel{ID = 2, Name = "Department", Checked = false}

            };

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_objNursery);
        }


        public ActionResult ManageNurseryReport()
        {
            string reportlavel = "";
            NurseryManagement _objNursery = new NurseryManagement();
            try
            {
                if (Session["DesignationId"].ToString() == "8")
                    reportlavel = "Range Level";
                else if (Session["DesignationId"].ToString() == "6")
                    reportlavel = "Division Level";
                else if (Session["DesignationId"].ToString() == "4")
                    reportlavel = "Circle Level";

                DataTable dt = _objNursery.Select_NURSERY_LEVELWISE(Convert.ToInt64(Session["UserID"].ToString()));

                foreach (DataRow dr in dt.Rows)
                {
                    lstNurseries.Add(
                        new NurseryManagement()
                        {
                            districtName = dr["DIV_NAME"].ToString(),
                            RangeName = dr["RANGE_NAME"].ToString(),
                            villName = dr["VILL_NAME"].ToString(),
                            nurseryName = dr["NURSERY_NAME"].ToString(),
                            product = dr["PRODUCTNAME"].ToString(),
                            produceQty = dr["PRODUCE_QTY"].ToString(),
                            d_produceQty = dr["PRODUCE_QTY_Department"].ToString(),
                            pur_produceQty = dr["PurchaseQuantity"].ToString(),

                        });
                }
                ViewData["lstNurseries"] = lstNurseries;
                ViewData["Report_Level"] = reportlavel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }


        /// <summary>
        /// Function searches out for Nursery(s) on the basis on Nurrsery Code
        /// </summary>
        /// <returns>Json Result for Nurseries</returns>
        [HttpPost]
        public JsonResult FindFilterdNursery(string NurseryCode)
        {
            try
            {
                Location location = new Location();
                List<SelectListItem> items = new List<SelectListItem>();

                NurseryManagement _objNursery = new NurseryManagement();
                DataTable dt = _objNursery.FetchNurseries(NurseryCode);

                DataTable dtCircle = _objNursery.GetCircleByRangeCode(Convert.ToString(dt.Rows[0]["RANGE_CODE"]));
                DataTable dtDivision = _objNursery.GetDivisionByRangeCode(Convert.ToString(dt.Rows[0]["RANGE_CODE"]));


                _objNursery = new NurseryManagement
                {
                    districtName = dt.Rows[0]["DIST_NAME"].ToString(),
                    //blkName = dt.Rows[0]["BLK_NAME"].ToString(),                  
                    //gpName = dt.Rows[0]["GP_NAME"].ToString(),
                    ddlRange = Convert.ToString(dt.Rows[0]["RANGE_CODE"]),
                    RangeName = Convert.ToString(dt.Rows[0]["RANGE_NAME"]),
                    ddlVillages = Convert.ToString(dt.Rows[0]["VILL_CODE"]),
                    villName = Convert.ToString(dt.Rows[0]["VILL_NAME"]),
                    address = Convert.ToString(dt.Rows[0]["NURSERY_ADRESS"]),
                    landmark = Convert.ToString(dt.Rows[0]["NURSERY_LANDMARK"]),
                    nurseryCode = dt.Rows[0]["NURSERY_CODE"].ToString(),
                    nurseryName = dt.Rows[0]["NURSERY_NAME"].ToString(),
                    nurseryType = dt.Rows[0]["NURSERY_TYPE"].ToString(),
                    latitude = Convert.ToDouble(dt.Rows[0]["LATITUDE"]),
                    longitude = Convert.ToDouble(dt.Rows[0]["LONGITUDE"]),
                    statusDesc = dt.Rows[0]["StatusDesc"].ToString(),
                    ActiveStatus = Convert.ToBoolean(dt.Rows[0]["ActiveStatus"]),
                    NurseryInchargeSSOID = dt.Rows[0]["NurseryInchargeSSOID"].ToString(),
                    IsCitizenOrDeptEndOpenNusery = Convert.ToInt32(dt.Rows[0]["IsCitizenOrDeptEndOpenNusery"]),
                    CircleCode = Convert.ToString(dtCircle.Rows[0]["CIRCLE_CODE"]),
                    DivisionCode = Convert.ToString(dtDivision.Rows[0]["DIV_CODE"])

                };

                return Json(_objNursery, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult DivisionData(string circleCode)
        {
            Location location = new Location();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(circleCode)))
                {
                    DataTable dt = location.BindDivision(circleCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    ViewBag.DivisionCode = items;
                }
            }
            catch (Exception ex)
            {
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Function to get Range with id from database to bind range dropdownlist 
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <returns>Range ID and Name</returns>
        [HttpPost]
        public JsonResult RangeData(string divisionCode)
        {
            Location location = new Location();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(divisionCode)))
                {

                    DataTable dt = location.BindRangeBydivisionCode(divisionCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.RangeCode = items;
                }
            }
            catch (Exception ex)
            {
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult GetBlocks(string distCode)
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    DataTable dt = objLocation.BindBlockName(distCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }
                    ViewBag.ddlBlocks = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                // new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }




        [HttpPost]
        public JsonResult GetSSOIDbyRange(string RangeCode)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    DataTable dt = new Common().Select_SSOIDbyRange(RangeCode);
                    ViewBag.NurseryInchargeSSOIDS = new SelectList(dt.AsDataView(), "UserID", "Ssoid");
                    return Json(new SelectList(ViewBag.NurseryInchargeSSOIDS, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                // new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }


        /// <summary>
        /// Function searches out for GP(s) on the basis on Dist Code and block code
        /// </summary>
        /// <returns>Json Result for GramPanchayats</returns>
        [HttpPost]
        public JsonResult GetGramPanchayats(string distCode, string blockCode)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    DataTable dt = objLocation.BindGramPanchayatName(distCode, blockCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPs = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                // new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// Function searches out for Village(s) on the basis on Dist Code and block code and gp code
        /// </summary>
        /// <returns>Json Result for Villages</returns>
        [HttpPost]
        public JsonResult GetVillages(string distCode, string blockCode, string gpCode)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    DataTable dt = objLocation.BindVillageName(distCode, blockCode, gpCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.ddlVillages = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                //  new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// Function responsible for Add/Edit a Nursery detail into db
        /// </summary> 
        /// <returns>View for Nursery Mgmt.</returns>

        [HttpPost]
        public ActionResult AddEditNurseryDetail(NurseryManagement NurseryObj, FormCollection formUser, string Command)
        {
            try
            {

                #region Check Nursery Open Citizen End And Dept End Developed by Rajveer
                NurseryObj.IsCitizenOrDeptEndOpenNusery = 0;
                Boolean isActive;
                if (NurseryObj.ActiveStatusCitizenANDDeptUser != null && NurseryObj.ActiveStatusCitizenANDDeptUser.Count > 0)
                {
                    foreach (var itm in NurseryObj.ActiveStatusCitizenANDDeptUser)
                    {
                        if (itm.Checked)
                        {
                            NurseryObj.IsCitizenOrDeptEndOpenNusery += itm.ID;
                        }
                    }
                }

                if (NurseryObj.ddlVillages == "0")
                    NurseryObj.ddlVillages = null;
                #endregion
                isActive = NurseryObj.ActiveStatus;
                Int64 result = NurseryObj.InsertUpdateNursery(Command,isActive);
                if (result > 0)
                {
                    if (Command.Equals("UPDATE"))
                        Session["ActionStatus"] = "Nursery detail has been updated sucessfully!!";
                    if (Command.Equals("DELETE"))
                        Session["ActionStatus"] = "Nursery has been de-activated sucessfully!!";
                    if (Command.Equals("INSERT"))
                        Session["ActionStatus"] = "Nursery Created sucessfully!!";
                    return RedirectToAction("ManageNursery", "ManageNursery", false);
                }
                else
                {
                    Session["ActionStatus"] = "There is some techincal issue while performing action!!";
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Function responsible for Add/Edit a Nursery detail into db
        /// </summary> 
        /// <returns>View for Nursery Mgmt.</returns>

        [HttpGet]
        public ActionResult DeleteNurseryDetail(string nurseryCode, string Command,Boolean IsActive)
        {
            try
            {
                NurseryManagement NurseryObj = new NurseryManagement();
                NurseryObj.nurseryCode = nurseryCode;

                Int64 result = NurseryObj.InsertUpdateNursery(Command,IsActive);
                if (result > 0)
                {
                    if (Command.Equals("UPDATE"))
                        Session["ActionStatus"] = "Nursery detail has been updated sucessfully!!";
                    if (Command.Equals("Delete"))
                        Session["ActionStatus"] = "Nursery Status Updated Successfully!!";
                    if (Command.Equals("INSERT"))
                        Session["ActionStatus"] = "Nursery Created sucessfully!!";
                    return RedirectToAction("ManageNursery", "ManageNursery", false);
                }
                else
                {
                    Session["ActionStatus"] = "There is some techincal issue while performing action!!";
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion
    }
}


