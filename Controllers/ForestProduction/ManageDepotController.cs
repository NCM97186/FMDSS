//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Depot management
//  Date Created : 28-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Vandana Gupta  
//  Modified On  : 24-Jun-2016
//  Modified purpose : Code clean-up and alignment as per coding guidelines
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************

using FMDSS.Models;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;


namespace FMDSS.Controllers.ForestProduction
{
    public class ManageDepotController : BaseController
    {
        #region global variables
        DepotManagement obj_depot = new DepotManagement();
        Location location = new Location();
        List<SelectListItem> items = new List<SelectListItem>();
        List<DepotManagement> depotList = new List<DepotManagement>();
        List<SelectListItem> CircleCode = new List<SelectListItem>();
        List<SelectListItem> divisionCode = new List<SelectListItem>();
        List<SelectListItem> rangeCode = new List<SelectListItem>();
        int ModuleID = 3;
        #endregion

        #region member functions
        /// <summary>
        /// Renders UI for Manage Depot
        /// </summary>
        /// <returns>Bind all depot details to table</returns>
        public ActionResult ManageDepot()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dt = new DataTable();
            try
            { 
                DataTable dt1 = location.BindCircle();
                ViewBag.fname = dt1;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                }

                ViewBag.CircleCode = items;
                
                //ViewBag.CircleCode = CircleCode;
                ViewBag.DivisionCode = divisionCode;
                //dt = new Common().Select_Range(UserID);
                //ViewBag.fname = dt;
                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                //}
                ViewBag.RangeCode = rangeCode;
                
                DepotManagement dm = new DepotManagement();
                DataTable dtf = new DataTable();
                if (Session["UserId"] != null)
                {
                    dm.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    dtf = dm.Select_Depot();

                    foreach (DataRow dr in dtf.Rows)
                    {
                        depotList.Add(new DepotManagement()
                        {
                            RowID = Convert.ToInt64(dr["RowID"].ToString()),
                            DepotId = Convert.ToInt64(dr["Depot_Id"].ToString()),
                            DivisionName = dr["DIV_NAME"].ToString(),
                            RangeName = dr["RANGE_NAME"].ToString(),
                            DepotName = dr["Depot_Name"].ToString(),
                            DepotIncharge = dr["Depot_InCharge"].ToString(),
                            DepotType = dr["Depot_Type"].ToString(),
                            RegionCode = dr["REG_CODE"].ToString(),
                            CircleCode = dr["CIRCLE_CODE"].ToString(),
                            DivisionCode = dr["DIV_CODE"].ToString(),
                            RangeCode = dr["RANGE_CODE"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(depotList);
        }

        /// <summary>
        /// Function to get all circle with id from database to bind circle dropdownlist
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns>circle name and circle id</returns>
        //[HttpPost]
        //public JsonResult CircleData()
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    try
        //    {


        //        DataTable dt1 = location.BindCircle();
        //        ViewBag.fname = dt1;
        //        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
        //        {
        //            items.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
        //        }

        //        ViewBag.CircleCode = items;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }

        //    return Json(new SelectList(items, "Value", "Text"));
        //}
        /// <summary>
        ///  Function to get all Division with id from database to bind circle dropdownlist
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <returns>Division ID and Name</returns>
        [HttpPost]
        public JsonResult DivisionData(string circleCode)
        {
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
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
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
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        ///  Function to get designation with id from database to bind designation dropdownlist rangewise
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DesignationByRange(string rangeCode)
        {
            DepotManagement dm = new DepotManagement();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(rangeCode))
                {

                    DataTable dt = dm.BindRangeWiseDesignation(rangeCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
                    }
                   // ViewBag.RangeCode = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        ///  Function to get Employee from database to bind Depot Incharge dropdownlist by range wise designation
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <param name="desigID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmpByRangeDesignation(string rangeCode, string desigID)
        {
            DepotManagement dm = new DepotManagement();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(rangeCode))
                {

                    DataTable dt = dm.EmpByRangeDesignation(rangeCode,desigID);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["EmpName"].ToString(), Value = @dr["SSO_ID"].ToString() });
                    }
                    ViewBag.RangeCode = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

       
        /// <summary>
        /// Function to get Village with id from database to bind circle dropdownlist 
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns>Village Name and ID</returns>
        [HttpPost]
        public JsonResult getVillage(string divisionCode, string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(divisionCode) && !String.IsNullOrEmpty(rangeCode))
                {
                    DataTable dt = location.BindVillage(divisionCode, rangeCode);
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
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
        /// Function to Save depot details to database 
       /// </summary>
       /// <param name="fm"></param>
       /// <param name="Command"></param>
       /// <returns>Saved depot ID</returns>
        [HttpPost]
        public ActionResult SubmitDepotForm(FormCollection fm, string Command)
        {
            Session["Depot_Status"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                DepotManagement depot = new DepotManagement();

                if (Session["UserId"] != null)
                {
                    depot.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                // adding two columns Division and Circle 


                if (fm["DivisionCode"].ToString() == "")
                {
                    depot.DivisionCode = "";
                }
                else
                {
                    depot.DivisionCode = fm["DivisionCode"].ToString();
                }


                if (fm["CircleCode"].ToString() == "")
                {
                    depot.CircleCode = "";
                }
                else
                {
                    depot.CircleCode = fm["CircleCode"].ToString();
                }             

                if (fm["DepotType"].ToString() == "")
                {
                    depot.DepotType = "";
                }
                else
                {
                    depot.DepotType = fm["DepotType"].ToString();
                }             

                if (fm["RangeCode"].ToString() == "")
                {
                    depot.RangeCode = "";
                }
                else
                {
                    depot.RangeCode = fm["RangeCode"].ToString();
                }

                if (fm["DepotName"].ToString() == "")
                {
                    depot.DepotName = "";
                }
                else
                {
                    depot.DepotName = fm["DepotName"].ToString();
                }

                if (fm["DepotIncharge"].ToString() == "")
                {
                    depot.DepotIncharge = "";
                }
                else
                {
                    depot.DepotIncharge = fm["DepotIncharge"].ToString();
                }

                if (fm["Designation"].ToString() == "")
                {
                    depot.DesignationID = "";
                }
                else
                {
                    depot.DesignationID = fm["Designation"].ToString();
                }

                string option = string.Empty;
                if (Command == "Save")
                {
                    option = "Insert";
                    depot.DepotId = 0;
                    Int64 status = depot.AddDepotData(option);
                    if (status > 0)
                    {
                        Session["Depot_Status"] = "Depot Created Sucessfully";
                    }
                    else
                    {
                        Session["Depot_Status"] = "Depot Allready Exists!";
                    }


                }
                else if (Command == "Update")
                {
                    option = "Update";
                    if (fm["DepotId"].ToString() == "")
                    {
                        depot.DepotId = 0;
                    }
                    else
                    {
                        depot.DepotId = Convert.ToInt64(fm["DepotId"].ToString());
                    }

                    Int64 status = depot.AddDepotData(option);
                    if (status > 0)
                    {
                        Session["Depot_Status"] = "Depot Updated Sucessfully";
                    }
                    else
                    {
                        Session["Depot_Status"] = "Error Occured";
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("ManageDepot", "ManageDepot");
        }

        /// <summary>
        /// Get depot details by ID for edit
        /// </summary>
        /// <param name="DepotID"></param>
        /// <returns>Depot name, Incharge circle id etc by depot ID</returns>
        [HttpPost]
        public JsonResult GetDataForEdit(string DepotID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DepotManagement dm = null;
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();


                //obj.DepotId = Convert.ToInt64(DepotID);
                if (!String.IsNullOrEmpty(DepotID))
                {

                    DataTable dt = obj_depot.Select_DepotByID(DepotID);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                dm = new DepotManagement();
                                dm.RowID = Convert.ToInt64(dr["RowID"].ToString());
                                dm.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                                dm.DivisionName = dr["DIV_NAME"].ToString();
                                dm.RangeName = dr["RANGE_NAME"].ToString();
                                dm.DepotName = dr["Depot_Name"].ToString();
                                dm.DepotIncharge = dr["Depot_InCharge"].ToString();
                                dm.DepotType = dr["Depot_Type"].ToString();
                                dm.RegionCode = dr["REG_CODE"].ToString();
                                dm.CircleCode = dr["CIRCLE_CODE"].ToString();
                                dm.DivisionCode = dr["DIV_CODE"].ToString();
                                dm.RangeCode = dr["RANGE_CODE"].ToString();

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(dm, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get depot details by ID for edit
        /// </summary>
        /// <param name="DepotID"></param>
        /// <returns>Depot name, Incharge circle id etc by depot ID</returns>
        [HttpPost]
        public JsonResult EditDetails(string DepotID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            DepotManagement obj_dm = null;

            try
            {


                if (!String.IsNullOrEmpty(DepotID))
                {

                    DataTable dt = obj_depot.Select_DepotByID(DepotID);
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            obj_dm = new DepotManagement();
                            obj_dm.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            obj_dm.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                            obj_dm.DivisionName = dr["DIV_NAME"].ToString();
                            obj_dm.RangeName = dr["RANGE_NAME"].ToString();
                            obj_dm.DepotName = dr["Depot_Name"].ToString();
                            obj_dm.DepotIncharge = dr["Depot_InCharge"].ToString();
                            obj_dm.DepotType = dr["Depot_Type"].ToString();
                            obj_dm.RegionCode = dr["REG_CODE"].ToString();
                            obj_dm.CircleCode = dr["CIRCLE_CODE"].ToString();
                            obj_dm.DivisionCode = dr["DIV_CODE"].ToString();
                            obj_dm.RangeCode = dr["RANGE_CODE"].ToString();
                            obj_dm.CircleName = dr["CIRCLE_NAME"].ToString();
                            obj_dm.RegionName = dr["REG_NAME"].ToString();
                            obj_dm.DepotType = dr["Depot_Type"].ToString();
                            obj_dm.DesignationID = dr["DesignationID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(obj_dm, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Depot from database by Depot ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteRecord(int id = 0)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DepotManagement dm = new DepotManagement();

                if (Session["UserId"] != null)
                {
                    dm.DepotId = Convert.ToInt64(id);
                    dm.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                    Int64 i = dm.DeleteDepot();
                }
                //Int64 i = cst.DeleteMasterFeesEntry();
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("ManageDepot", "ManageDepot");
        }

        #endregion
    }
}
