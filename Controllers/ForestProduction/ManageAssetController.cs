//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageAssetController
//  Description  : File contains calling functions from UI
//  Date Created : 06-Jan-2016
//  History      : 
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Manoj Kumar
//  Modified On  : 06-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//  Bug No :-399,400,457


//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestProduction;
using System.Configuration;
using FMDSS.Filters;
using FMDSS.Models;


namespace FMDSS.Controllers.ForestProduction
{

    public class ManageAssetController : BaseController
    {
        //Note: Code Updated with Ref. to bug ID 286 in .cshtml file and alternate ID 457
        AssetManagement Obj_asset = new AssetManagement();
        int ModuleID = 3;
        DataTable dtAssets = null;
        #region member functions

        /// <summary>
        /// Renders UI for Manage Asset
        /// </summary>
        /// <returns>View for Manage Asset and bind all districts</returns>
        public ActionResult ManageAsset()
        {
            List<AssetManagement> assetList = new List<AssetManagement>();
            List<SelectListItem> District = new List<SelectListItem>();
            List<SelectListItem> AssetCategory = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataTable dtd = new DataTable();
                Location loc = new Location();
                dtd = loc.District("District");
                foreach (DataRow dr in dtd.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = District;

                AssetManagement am = new AssetManagement();
                //DataTable dtCat = new DataTable();
                //dtCat = am.BindAssetCategory();
                //foreach (DataRow dr in dtCat.Rows)
                //{
                //    AssetCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["AssetCategoryID"].ToString() });
                //}
                //ViewBag.AssetCategoryID = AssetCategory;

                DataTable dtf = new DataTable();
                dtf = am.Select_Asset();
                foreach (DataRow dr in dtf.Rows)
                {
                    assetList.Add(new AssetManagement()
                    {
                        RowID = Convert.ToInt64(dr["RowID"].ToString()),
                        AssetID = Convert.ToInt64(dr["ASSETID"].ToString()),
                        AssetCategoryName = dr["CategoryName"].ToString(),
                        //JfmcName = dr["Committee_Name"].ToString(),
                        //PlanID = dr["MICROPLANID"].ToString(),
                        WorkOrder_Code = dr["WorkOrder_Code"].ToString(),
                        AssetName = dr["ASSETNAME"].ToString(),
                        TotalAsset = Convert.ToInt32(dr["ASSET_COUNT"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(assetList);
        }

        /// <summary>
        /// Function is used to bind block dropdownlist
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <returns>json result with block name and code</returns>
        [HttpPost]
        public JsonResult BindBlock(string DistrictID)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Location loc = new Location();
            List<SelectListItem> Block = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(DistrictID))
                {
                    DataTable dt = loc.BindBlockName(DistrictID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Block.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Block, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind GramPanchayat dropdownlist on the basis of given param
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <param name="BlockID"></param>
        /// <returns>json result with Gram Panchayat name and code</returns>
        [HttpPost]
        public JsonResult BindGramPanchayat(string DistrictID, String BlockID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Location loc = new Location();
            List<SelectListItem> GP = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DistrictID)) && !String.IsNullOrEmpty(BlockID))
                {
                    DataTable dt = loc.BindGramPanchayatName(DistrictID, BlockID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        GP.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(GP, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind village dropdownlist on the basis of given param
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <param name="BlockID"></param>
        /// /// <param name="GpID"></param>
        /// <returns>json result with Village name and code</returns>
        [HttpPost]
        public JsonResult BindVillage(string DistrictID, string BlockID, string GpID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Location loc = new Location();
            List<SelectListItem> Village = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DistrictID)) && !String.IsNullOrEmpty(BlockID))
                {
                    DataTable dt = loc.BindVillageName(DistrictID, BlockID, GpID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Village.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                }
                else
                {
                    Village.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Village, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind JFMC dropdownlist on the basis of given param
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <param name="BlockID"></param>
        /// <param name="GpID"></param>
        /// <param name="villageID"></param>
        /// <returns>Return JFMC List</returns>
        [HttpPost]
        public JsonResult BindJFMC(string DistrictID, string BlockID, string GpID, string villageID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Location loc = new Location();
            List<SelectListItem> JFMC = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DistrictID)) && !String.IsNullOrEmpty(BlockID))
                {
                    DataTable dt = loc.BindJFMC(DistrictID, BlockID, GpID, villageID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        JFMC.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["Id"].ToString() });
                    }
                }
                else
                {
                    JFMC.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(JFMC, "Value", "Text"));
        }


        [HttpPost]
        public JsonResult assetListByWorkorderID(string workOrderID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> items1 = new List<SelectListItem>();
            var result = new List<AssetManagement>();
            List<AssetManagement> lstWOMilestone = new List<AssetManagement>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            AssetManagement asset = new AssetManagement();
            try
            {
                dtAssets = asset.Select_AssetDetail(Convert.ToInt64(workOrderID));
                DataView dv = new DataView(dtAssets);
                DataTable dtAssetCatgory = dv.ToTable(true, new string[] { "AssetCategoryID", "CategoryName" });
                DataTable dtAsset = dv.ToTable(true, new string[] { "ASSETID", "ASSETNAME" });




                foreach (System.Data.DataRow dr in dtAssetCatgory.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["AssetCategoryID"].ToString() });
                }


                foreach (System.Data.DataRow dr in dtAsset.Rows)
                {
                    items1.Add(new SelectListItem { Text = @dr["ASSETNAME"].ToString(), Value = @dr["ASSETID"].ToString() });
                }



                //if (dtAssets.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtAssets.Rows)
                //    {
                //        asobj = new AssetManagement()
                //        {
                //            AssetCatID = Convert.ToInt64(dr["AssetCategoryID"].ToString()),
                //            AssetCategoryName = dr["CategoryName"].ToString(),
                //            AssetID = Convert.ToInt64(dr["ASSETID"].ToString()),
                //            AssetName = dr["ASSETNAME"].ToString(),
                //            TotalAsset = Convert.ToInt32(dr["TotalQuantity"].ToString()),
                //            JFMCAgencyType = dr["ContractAgencyType"].ToString(),
                //            JfmcName = dr["Committee_Name"].ToString()

                //        };
                //        result.Add(asobj);
                //    }
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list = items, list1 = items1 }, JsonRequestBehavior.AllowGet);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getAssetNumber(string workOrderID, string assetCat, string assetId)
        {
            var result = new List<AssetManagement>();
            List<AssetManagement> lstWOMilestone = new List<AssetManagement>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            AssetManagement asset = new AssetManagement();
            try
            {
                dtAssets = asset.Select_AssetDetail(Convert.ToInt64(workOrderID));
                dtAssets = dtAssets.Select("AssetCategoryID =" + assetCat + "AND ASSETID =" + assetId).CopyToDataTable();

                if (dtAssets.Rows.Count > 0)
                {
                    asset = new AssetManagement()
                    {
                        TotalAsset = Convert.ToInt32(dtAssets.Rows[0]["TotalQuantity"].ToString()),
                        JFMCAgencyType = dtAssets.Rows[0]["ContractAgencyType"].ToString(),
                        JfmcName = dtAssets.Rows[0]["Committee_Name"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(asset, JsonRequestBehavior.AllowGet);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// </summary>
        /// <param name="jfmcID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BindMicroPlan(string jfmcID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Location loc = new Location();
            List<SelectListItem> JFMC = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(jfmcID))
                {
                    DataTable dt = loc.BindMicroPlan(jfmcID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        JFMC.Add(new SelectListItem { Text = @dr["MicroPlanName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                }
                else
                {
                    JFMC.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(JFMC, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to save asset details to database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitAsset(AssetManagement am, FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Session["Asset_Status"] = null;
            // AssetManagement am = new AssetManagement();
            try
            {
                if (Session["UserId"] != null)
                {
                    am.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }
                string option = string.Empty;
                if (Command == "Save")
                {
                    option = "Insert";
                    Int64 status = am.AddAsset(option);
                    if (status > 0)
                    {
                        Session["Asset_Status"] = "Asset Saved Sucessfully.";
                    }
                    else
                    {
                        Session["Asset_Status"] = "Asset exists already in database.";
                    }
                }
                else if (Command == "Update")
                {
                    option = "Update";
                    Int64 status = am.AddAsset(option);
                    if (status > 0)
                    {
                        Session["Asset_Status"] = "Asset Updated Sucessfully.";
                    }
                    else
                    {
                        Session["Asset_Status"] = "Error Occured.";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("ManageAsset", "ManageAsset");
        }

        /// <summary>
        /// Function is used to get asset details for edit on given param values
        /// </summary>
        /// <param name="AssetID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditDetails(string AssetID)
        {

            AssetManagement am = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (!String.IsNullOrEmpty(AssetID))
                {

                    DataTable dt = Obj_asset.Select_AssetByID(AssetID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            am = new AssetManagement();
                            am.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            am.AssetID = Convert.ToInt64(dr["ASSETID"].ToString());
                            am.WorkOrder_Code = dr["WorkOrder_Code"].ToString();
                            am.WorkOrder = Convert.ToInt64(dr["WorkOrderID"].ToString());
                            am.DistrictCode = dr["DIST_CODE"].ToString();
                            am.DistrictName = dr["DIST_NAME"].ToString();
                            am.BlockCode = dr["BLK_CODE"].ToString();
                            am.BlockName = dr["BLK_NAME"].ToString();
                            am.PanchayatCode = dr["GP_CODE"].ToString();
                            am.PanchayatName = dr["GP_NAME"].ToString();
                            am.VillageCode = dr["VILL_CODE"].ToString();
                            am.VillageName = dr["VILL_NAME"].ToString();
                            //am.JfmcID = dr["JFMC"].ToString();
                            //am.JfmcName = dr["Committee_Name"].ToString();
                            // am.PlanID = dr["MICROPLANID"].ToString();
                            //am.PlanName = dr["MicroPlanName"].ToString();
                            am.AssetCategoryID = dr["ASSET_CTGRY"].ToString();
                            am.AssetCategoryName = dr["CategoryName"].ToString();
                            am.TotalAsset = Convert.ToInt32(dr["ASSET_COUNT"].ToString());
                            am.AssetID = Convert.ToInt64(dr["assetsID"].ToString());
                            am.AssetName = dr["ASSETNAME"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(am, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Function is used to delete asset details from database by asset id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteRecord(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            AssetManagement am = new AssetManagement();
            try
            {
                if (Session["UserId"] != null)
                {
                    am.AssetID = Convert.ToInt64(id);
                    am.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                    Int64 i = am.DeleteAsset();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("ManageAsset", "ManageAsset");
        }

        #endregion
    }
}