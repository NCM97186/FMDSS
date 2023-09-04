    //********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FDMSUBACTIVITYCONTROLLER
//  Description  : File contains calling functions from UI
//  Date Created : 02-Feb-2016
//  History      : 
//  Version      : 1.0
//  Author       : Gaurav Pandey
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using FMDSS.Models;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using System.Data.Entity;
using System.Linq;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
namespace FMDSS.Controllers.ForesterDevelopment
{
    public class fdmSubActivityController : BaseController
    {
        //
        // GET: /fdmSubActivity/
        #region "Property Intilization"
        int ModuleID = 1;
        SubActivity _objModel = new SubActivity();
        Activity _objActivityModel = new Activity();
        List<SelectListItem> Activity = new List<SelectListItem>();
        List<SubActivity> ModelList = new List<SubActivity>();
        #endregion
        public FmdssContext dbContext;
        public fdmSubActivityController()
        {
            dbContext = new FmdssContext();
        }

        //History: Code Update with ref. to bug ID: 273,233 by gaurav
        /// <summary>
        /// Bind the Page load controls/data
        /// </summary>
        /// <returns></returns>
        public ActionResult fdmSubActivity()
        {

            List<SelectListItem> DefineModel = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());

                    ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                    ViewBag.Sub_Activity_BSRType = new SelectList(Common.GetActivityType(), "Value", "Text");
                    _objModel.ConditionFileEditMode = true;

                    DataSet dtf = _objModel.GetAllRecords();
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[0].Rows)
                            ModelList.Add(
                                new SubActivity()

                                {
                                    Index = Convert.ToInt64(dr["Index"].ToString()),
                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    Sub_Activity_BSRType = dr["Sub_Activity_BSRType"].ToString(),
                                    Sub_Activity_Name = dr["Sub_Activity_Name"].ToString(),
                                    Sub_Activity_totalCost = Convert.ToDecimal(dr["Sub_Activity_TotalCost"].ToString()),
                                    Sub_Activity_BSR_Material_Cost = Convert.ToDecimal(dr["Sub_Activity_BSR_Material_Cost"].ToString()),
                                    Sub_Activity_BSR_Labour_Cost = Convert.ToDecimal(dr["Sub_Activity_BSR_Labour_Cost"].ToString())
                                    //Sub_Activity_RatePerUnit = Convert.ToDecimal(dr["Sub_Activity_RatePerUnit"].ToString())

                                });

                    }
                    ViewData["SubActivityList"] = ModelList;
                    return View(_objModel);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        /// <summary>
        /// Save the all the model which is user selected or fill.
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSubActivityData(SubActivity Model, string command, FormCollection fCollection)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string FilePath = "~/PermissionDocument/";
            try
            {
                if (Session["UserID"] != null)
                {
                    if (command == "")
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.Sub_Activity_Name = string.IsNullOrEmpty(Model.Sub_Activity_Name) ? "" : Model.Sub_Activity_Name;
                        _objModel.Sub_Activity_RatePerUnit = Model.Sub_Activity_RatePerUnit;
                        _objModel.Sub_Activity_Unit = string.IsNullOrEmpty(Model.Sub_Activity_Unit) ? "" : Model.Sub_Activity_Unit;
                        _objModel.Sub_Activity_totalCost = Model.Sub_Activity_totalCost;
                        _objModel.Sub_Activity_BSR_Material_Cost = Model.Sub_Activity_BSR_Material_Cost;
                        _objModel.Sub_Activity_BSR_Labour_Cost = Model.Sub_Activity_BSR_Labour_Cost;
                        //_objModel.Sub_Activity_BSRType = string.IsNullOrEmpty(Model.Sub_Activity_BSRType) ? "" : Model.Sub_Activity_BSRType;
                        _objModel.Sub_Activity_RefNo = string.IsNullOrEmpty(Model.Sub_Activity_RefNo) ? "" : Model.Sub_Activity_RefNo;
                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objModel.Sub_Activity_DocumentPath = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objModel.Sub_Activity_DocumentPath = ""; }


                        Int64 id = _objModel.SubmitSubActivity(_objModel);
                        if (id > 0)
                        {
                            TempData["ViewMessage"] = "Record Save Successfully!!";
                            return RedirectToAction("fdmSubActivity", "fdmSubActivity");

                        }
                        else
                        {

                            return View("Error");
                        }
                    }
                    else
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.ID = Convert.ToInt64(command);
                        _objModel.Sub_Activity_Name = string.IsNullOrEmpty(Model.Sub_Activity_Name) ? "" : Model.Sub_Activity_Name;
                        _objModel.Sub_Activity_BSR_Material_Cost = Model.Sub_Activity_BSR_Material_Cost;
                        _objModel.Sub_Activity_BSR_Labour_Cost = Model.Sub_Activity_BSR_Labour_Cost;
                        _objModel.Sub_Activity_RatePerUnit = Model.Sub_Activity_RatePerUnit;
                        _objModel.Sub_Activity_Unit = string.IsNullOrEmpty(Model.Sub_Activity_Unit) ? "" : Model.Sub_Activity_Unit;
                        _objModel.Sub_Activity_totalCost = Model.Sub_Activity_totalCost;
                        //_objModel.Sub_Activity_BSRType = string.IsNullOrEmpty(Model.Sub_Activity_BSRType) ? "" : Model.Sub_Activity_BSRType;
                        _objModel.Sub_Activity_RefNo = string.IsNullOrEmpty(Model.Sub_Activity_RefNo) ? "" : Model.Sub_Activity_RefNo;
                        if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[0].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objModel.Sub_Activity_DocumentPath = path;
                            Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objModel.Sub_Activity_DocumentPath = fCollection["hdRefDocument"].ToString(); }

                        Int64 id = _objModel.UpdateSubActivity(_objModel);
                        if (id > 0)
                        {
                            TempData["ViewMessage"] = "Record Update Successfully!!";
                            return RedirectToAction("fdmSubActivity", "fdmSubActivity");

                        }
                        else
                        {

                            return View("Error");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        /// <summary>
        /// Display the data for specific Sub-Activity 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult EditSubActivityData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SubActivity> ModelList = new List<SubActivity>();
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet ds = _objModel.GetAllRecords(Convert.ToInt64(ID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Json(new
                        {

                            ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]),
                            Sub_Activity_Name = ds.Tables[0].Rows[0]["Sub_Activity_Name"].ToString(),
                            Sub_Activity_RatePerUnit = Convert.ToDecimal(ds.Tables[0].Rows[0]["Sub_Activity_RatePerUnit"]),
                            Sub_Activity_Unit = ds.Tables[0].Rows[0]["Sub_Activity_Unit"].ToString(),
                            //Sub_Activity_BSRType = ds.Tables[0].Rows[0]["Sub_Activity_BSRType"].ToString(),
                            Sub_Activity_BSR_Material_Cost = ds.Tables[0].Rows[0]["Sub_Activity_BSR_Material_Cost"].ToString(),
                            Sub_Activity_BSR_Labour_Cost = ds.Tables[0].Rows[0]["Sub_Activity_BSR_Labour_Cost"].ToString(),
                            Sub_Activity_BSRType = ds.Tables[0].Rows[0]["Sub_Activity_BSRType"].ToString(),
                            Sub_Activity_totalCost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Sub_Activity_TotalCost"]),
                            Sub_Activity_DocumentPath = ds.Tables[0].Rows[0]["Sub_Activity_DocumentPath"].ToString(),
                            Sub_Activity_RefNo = ds.Tables[0].Rows[0]["Sub_Activity_RefNo"].ToString()
                        });

                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        /// <summary>
        /// Deactivated the Sub-Activity
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult DeleteSubActivityData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    if (ID != "")
                    {
                        _objModel.TableName = Common.TableName.tbl_mst_FDM_Sub_Activity.ToString();
                        _objModel.ID = (Convert.ToInt64(ID));
                        Int64 id = _objModel.DeleteSubActivity(_objModel);
                        if (id > 0)
                        {
                            return Json(new
                            {
                                ID = id

                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                ID = id

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        public ActionResult AddSubActivity()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SubActivityModel model = new SubActivityModel();
            try
            {

                SubActivityRepo repo = new SubActivityRepo();
                ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");

                DataTable ActivityDataset = new DataTable();
                ActivityDataset = repo.BindDDlBudgetMasterModel("Activity", 0);
                ViewBag.Activity = new SelectList(ActivityDataset.AsDataView(), "value", "text");

                DataSet ds = new DataSet();
                ds = repo.InsertSubActivity(model, "LIST", Convert.ToInt64(Session["UserID"]));

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region Marge Data
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubActivityModels>>(str);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddSubActivity(SubActivityModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                SubActivityRepo repo = new SubActivityRepo();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(model.ID))
                {
                    ds = repo.InsertSubActivity(model, "UPDATE", Convert.ToInt64(Session["UserID"]));
                }
                else
                {
                    ds = repo.InsertSubActivity(model, "INSERT", Convert.ToInt64(Session["UserID"]));
                }

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + Convert.ToString(ds.Tables[0].Rows[0]["Status"]) + "</div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("AddSubActivity");
        }


        [HttpPost]
        public JsonResult GetSubActivityDetails(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SubActivityRepo repo = new SubActivityRepo();
            DataSet ds = new DataSet();
            SubActivityModel model = new SubActivityModel();
            model.ID = ID;
            try
            {
                if (Session["UserID"] != null)
                {
                    ds = repo.InsertSubActivity(model, "DETAILS", Convert.ToInt64(Session["UserID"]));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                        model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubActivityModels>>(str);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return Json(model.List, JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetReferenceNumber(int ActivityId)
        {
            try
            {
                tbl_mst_ActivityForWidelife tbl = dbContext.tbl_mst_ActivityForWidelife.FirstOrDefault(i => i.ID == ActivityId);
                if (tbl != null)
                {

                    return Json(tbl.ActivityDescription, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
