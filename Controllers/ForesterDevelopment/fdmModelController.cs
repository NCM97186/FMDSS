
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FDMMODELCONTROLLER
//  Description  : File contains calling functions from UI
//  Date Created : 08-Feb-2016
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
namespace FMDSS.Controllers.ForesterDevelopment
{
    public class fdmModelController : BaseController
    {
        #region "Property Intilization"
        int ModuleID = 1;
        Int64 UserID = 0;
        DefineModel _objModel = new DefineModel();
        List<DefineModel> ModelList = new List<DefineModel>();
        List<Activity> ModelList1 = new List<Activity>();
        #endregion
        //
        // GET: /fdmModel/
        //History: Code Update with ref. to bug ID: 277 by gaurav
        /// <summary>
        /// Load the page data/controls on UI
        /// </summary>
        /// <returns></returns>
        public ActionResult fdmModel()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {

                    //DefineModel _objModel = new DefineModel();
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objModel.ID = 0;
                    ViewBag.Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                    DataSet dtf = _objModel.GetAllRecords();
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[0].Rows)
                            ModelList.Add(
                                new DefineModel()

                                {
                                    Index = Convert.ToInt64(dr["Index"].ToString()),
                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    Model_Name = dr["Model_Name"].ToString(),
                                    CreatedDate = dr["EnteredOn"].ToString()
                                    //Model_FromDate = dr["Model_FromDate"].ToString(),
                                    //Model_ToDate = dr["Model_ToDate"].ToString()

                                });

                    }
                    ViewData["ModelList"] = ModelList;

                    DataSet dsSubactivity = new Activity().GetAllRecords();
                    for (int i = 0; i < dsSubactivity.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dsSubactivity.Tables[0].Rows)
                            ModelList1.Add(
                                new Activity()

                                {

                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    Activity_TotalCost = Convert.ToDecimal(dr["Activity_TotalCost"].ToString()),
                                    Activity_Name = dr["Activity_Name"].ToString(),
                                    Activity_Desc = dr["Activity_Desc"].ToString(),
                                    Activity_Type = dr["Activity_Type"].ToString(),
                                    Activity_Year_Name = dr["YearName"].ToString(),

                                });

                    }
                    ViewData["ActivityList"] = ModelList1;
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
        /// Save the all Records of Model
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveModelData(DefineModel Model, string command, FormCollection fCollection)
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
                        _objModel.Model_Name = string.IsNullOrEmpty(Model.Model_Name) ? "" : Model.Model_Name;
                        _objModel.Model_FromDate = string.IsNullOrEmpty(Model.Model_FromDate) ? "" : Model.Model_FromDate;
                        _objModel.Model_ToDate = string.IsNullOrEmpty(Model.Model_ToDate) ? "" : Model.Model_ToDate;
                        _objModel.Model_RefNo = string.IsNullOrEmpty(Model.Model_RefNo) ? "" : Model.Model_RefNo;
                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objModel.Model_DocumentPath = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objModel.Model_DocumentPath = ""; }


                        Int64 id = _objModel.SubmitDefineModel(_objModel);
                        if (id > 0)
                        {
                            if (Session["ActData"] != null)
                            {
                                List<Activity> list = (List<Activity>)Session["ActData"];
                                if (list != null)
                                {
                                    list.ForEach(t => t.Model_ID = id);
                                    _objModel.SaveActModelMapping(list, _objModel.UserID);
                                    Session["ActData"] = null;
                                }
                            }

                            TempData["ViewMessage"] = "Record Save Successfully!!";
                            return RedirectToAction("fdmModel", "fdmModel");


                        }
                        else
                        {

                            return View("Error");
                        }

                    }
                    else
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.Model_Name = string.IsNullOrEmpty(Model.Model_Name) ? "" : Model.Model_Name;
                        _objModel.Model_FromDate = string.IsNullOrEmpty(Model.Model_FromDate) ? "" : Model.Model_FromDate;
                        _objModel.Model_ToDate = string.IsNullOrEmpty(Model.Model_ToDate) ? "" : Model.Model_ToDate;
                        _objModel.ID = Convert.ToInt64(command);
                        _objModel.Model_RefNo = string.IsNullOrEmpty(Model.Model_RefNo) ? "" : Model.Model_RefNo;
                        if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[0].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objModel.Model_DocumentPath = path;
                            Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objModel.Model_DocumentPath = fCollection["hdRefDocument"].ToString(); }
                        Int64 id = _objModel.UpdateDefineModel(_objModel);
                        if (id > 0)
                        {
                            if (Session["ActData"] != null)
                            {
                                List<Activity> list = (List<Activity>)Session["ActData"];
                                if (list != null)
                                {
                                    list.ForEach(t => t.Model_ID = id);
                                    _objModel.SaveActModelMapping(list, _objModel.UserID);
                                    Session["ActData"] = null;
                                }
                            }
                            TempData["ViewMessage"] = "Record Updated Successfully!!";
                            return RedirectToAction("fdmModel", "fdmModel");


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
        /// Display the on edit mode based on specific ModelID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult EditModelData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<DefineModel> ModelList = new List<DefineModel>();
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet ds = _objModel.GetAllRecords(Convert.ToInt64(ID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            _objModel.Model_Name = ds.Tables[0].Rows[i]["Model_Name"].ToString();
                            //DateTime datefrom = new DateTime();
                            //datefrom = Convert.ToDateTime(ds.Tables[0].Rows[0]["Model_FromDate"].ToString());
                            //_objModel.Model_FromDate = datefrom.ToString("dd/MM/yyyy");
                            //DateTime dateto = new DateTime();
                            //dateto = Convert.ToDateTime(ds.Tables[0].Rows[0]["Model_ToDate"].ToString());
                            //_objModel.Model_ToDate = dateto.ToString("dd/MM/yyyy");
                            _objModel.Model_RefNo = ds.Tables[0].Rows[0]["Model_RefNo"].ToString();
                            _objModel.Model_DocumentPath = ds.Tables[0].Rows[0]["Model_DocumentPath"].ToString();

                        }
                        ViewData["EditMode"] = "1";
                        Session["EditMode"] = "1";
                        Session["act_ID"] = Convert.ToInt64(ID);
                    }
                    return Json(new
                    {

                        Model_Name = _objModel.Model_Name,
                        Model_FromDate = _objModel.Model_FromDate,
                        Model_ToDate = _objModel.Model_ToDate,
                        ID = Convert.ToInt64(ID),
                        Model_RefNo = _objModel.Model_RefNo,
                        DocumentPath = _objModel.Model_DocumentPath,

                    });
                    //return View("fdmModel", _objModel);

                }
                else
                {
                    //return View("Error");
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }

        /// <summary>
        /// Delete/Deactivated the Model Data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeleteModelData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    if (ID != "")
                    {
                        _objModel.TableName = Common.TableName.tbl_mst_FDM_Model.ToString();
                        _objModel.ID = (Convert.ToInt64(ID));
                        Int64 id = _objModel.DeleteDefineModel(_objModel);
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
        //Comment:- Code change for Bug ID:-235 by gaurav pandey on 24 feb
        /// <summary>
        /// Save the Activity Mapping with Model
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult SaveActivityMapping(string ID)
        {

            List<Activity> _objData = new List<Activity>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["ActData"] != null)
                {
                    List<Activity> list = (List<Activity>)Session["ActData"];

                    if (list != null)
                    {
                        Activity obj = new Activity { ID = Convert.ToInt64(ID) };
                        list.Add(obj);
                        Session["ActData"] = list;
                    }

                }
                else
                {

                    Activity obj = new Activity { ID = Convert.ToInt64(ID) };
                    _objData.Add(obj);
                    Session["ActData"] = _objData;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Delete the Earlier saved data Once user chages Preferences of Activity
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public JsonResult DeleteMapping(string sid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string data = Common.Status.Success.ToString();
            try
            {
                List<Activity> list = (List<Activity>)Session["ActData"];
                Int64 actid = 0;

                for (int i = 0; i < list.Count; i++)
                {

                    if (list[i].ID == Convert.ToInt64(sid))
                    {
                        actid = Convert.ToInt64(sid);
                        list.RemoveAll(item => item.ID == actid);

                        if (Session["EditMode"] != null && Session["act_ID"] != null)
                        {
                            new Activity().DeleteActSubActMapping(Convert.ToInt64(Session["act_ID"]), actid);
                        }

                    }
                }
                Session["EditMode"] = null;
                Session["activity_ID"] = null;
                Session["SubActData"] = list;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// To bind the Activity Grid
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult GetListData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Session["ActData"] = null;
            DataSet dsactivity = new Activity().GetMapRecords(Convert.ToInt64(ID));
            List<ActvityMap> listdata = new List<ActvityMap>();
            List<Activity> _objData = new List<Activity>();
            try
            {
                for (int i = 0; i < dsactivity.Tables.Count; i++)
                {
                    foreach (DataRow dr in dsactivity.Tables[0].Rows)
                    {
                        listdata.Add(new ActvityMap()
                        {
                            ID = dr["ID"].ToString(),
                            Activity_Name = dr["Activity_Name"].ToString(),
                            Activity_Type = dr["Activity_Type"].ToString(),
                            Activity_Desc = dr["Activity_Desc"].ToString(),
                            BitStaus = dr["BitStaus"].ToString(),
                            Activity_Year_Name = dr["YearName"].ToString(),
                        });

                    }

                }
                if (Session["ActData"] == null)
                {

                    for (int i = 0; i < dsactivity.Tables[0].Rows.Count; i++)
                    {
                        if (dsactivity.Tables[0].Rows[i]["BitStaus"].ToString() == "1")
                        {
                            Activity obj = new Activity { ID = Convert.ToInt64(dsactivity.Tables[0].Rows[i]["ID"]) };
                            _objData.Add(obj);

                        }

                    }
                    Session["ActData"] = _objData;


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(listdata, JsonRequestBehavior.AllowGet);

        }
    }
}
