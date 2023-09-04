//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FDMACTIVITYCONTROLLER
//  Description  : File contains calling functions from UI
//  Date Created : 26-Jan-2016
//  History      : 
//  Version      : 1.0
//  Author       : Gaurav Pandey
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************


using FMDSS.GenericClass;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class fdmActivityController : BaseController
    {
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> Activity = new List<SelectListItem>();
        Workorder _objWO = new Workorder();
        // 
        // GET: /fdmActivity/
        #region "Property"
        int ModuleID = 1;
        Int64 UserID = 0;
        Activity _objModel = new Activity();
        List<Activity> ModelList = new List<Activity>();
        List<SubActivity> ModelList1 = new List<SubActivity>();
        #endregion

        //History: Code Update with ref. to bug ID: 274,275 gaurav
        /// <summary>
        /// Page load function to bind the UI controls
        /// </summary>
        /// <returns></returns>
        public ActionResult fdmActivity()
        {
            List<SelectListItem> DefineModel = new List<SelectListItem>();
            List<SelectListItem> ActivityYear = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());


                    DataTable dt = null;
                    dt = _objModel.BindActivityYear();

                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ActivityYear.Add(new SelectListItem { Text = @dr["YearName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.Sub_Activity_Unit = new SelectList(Common.GetMeasurementUnits(), "Value", "Text");
                    ViewBag.ActivityYear = ActivityYear;
                    ViewBag.Activity_Type = new SelectList(Common.GetActivityCategoryTypeWildlife(), "Value", "Text");
                    ViewBag.BSR_Type = new SelectList(Common.GetSubActivityBSRType(), "Value", "Text");
                    _objModel.ConditionFileEditMode = true;

                    DataSet dtf = _objModel.GetAllRecords();
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[0].Rows)
                            ModelList.Add(
                                new Activity()

                                {
                                    Index = Convert.ToInt64(dr["Index"].ToString()),
                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    Activity_Name = dr["Activity_Name"].ToString(),
                                    //Activity_BSR_Per_Unit = Convert.ToDecimal(dr["Activity_BSR_Per_Unit"]),
                                    Activity_StartDate = dr["Activity_StartDate"].ToString(),
                                    Activity_EndDate = dr["Activity_EndDate"].ToString(),
                                    Activity_Desc = dr["Activity_Desc"].ToString(),
                                    Activity_TotalCost = Convert.ToDecimal(dr["Activity_TotalCost"])


                                });

                    }
                    ViewData["ActivityList"] = ModelList;

                    DataSet dsSubactivity = new SubActivity().GetAllRecords();
                    for (int i = 0; i < dsSubactivity.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dsSubactivity.Tables[0].Rows)
                            ModelList1.Add(
                                new SubActivity()

                                {

                                    ID = Convert.ToInt64(dr["ID"].ToString()),
                                    Sub_Activity_BSRType = dr["Sub_Activity_BSRType"].ToString(),
                                    Sub_Activity_Name = dr["Sub_Activity_Name"].ToString(),
                                    Sub_Activity_Unit = dr["Sub_Activity_Unit"].ToString(),
                                    Sub_Activity_totalCost = Convert.ToDecimal(dr["Sub_Activity_TotalCost"].ToString())
                                });

                    }
                    ViewData["SubActivityList"] = ModelList1;

                    return View(_objModel);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        public ActionResult fdmCircleActivity()
        {
            List<SelectListItem> DefineModel = new List<SelectListItem>();
            List<SelectListItem> ActivityYear = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    _objModel.SSOID =  Session["SSOID"].ToString() ;
                    DataTable dt = _objModel.Division(_objModel);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    DataTable dtActivity = _objModel.GetAllRecords().Tables[0];
                    ViewBag.fname = dtActivity;
                    foreach (DataRow dr in ViewBag.fname.Rows)
                    {
                        Activity.Add(new SelectListItem { Text = @dr["Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.Activity = Activity;
                }
                ViewBag.ddlDivision1 = Division;
                ViewBag.ddlActivity1 = Activity;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return View();
        }
       /// <summary>
        /// Save the data of activity
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="command"></param>
        /// <param name="fCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveActivityData(Activity Model, string command, FormCollection fCollection)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string FilePath = "~/PermissionDocument/";// ConfigurationManager.AppSettings["PermissionDocument"].ToString();
            Int64 id = 0;
            try
            {
                if (Session["UserID"] != null)
                {
                    if (command == "")
                    {
                        _objModel.UserID = Convert.ToInt64(Session["UserID"].ToString());
                        _objModel.Model_ID = Model.Model_ID;
                        _objModel.Activity_Name = string.IsNullOrEmpty(Model.Activity_Name) ? "" : Model.Activity_Name;
                        _objModel.Activity_Desc = string.IsNullOrEmpty(Model.Activity_Desc) ? "" : Model.Activity_Desc;
                        //_objModel.Activity_BSR_Per_Unit = Model.Activity_BSR_Per_Unit;
                        _objModel.Sub_Activity_Unit = string.IsNullOrEmpty(Model.Sub_Activity_Unit) ? "" : Model.Sub_Activity_Unit;
                        _objModel.Activity_BSR_Material_Cost = Model.Activity_BSR_Material_Cost;
                        _objModel.Activity_BSR_Labour_Cost = Model.Activity_BSR_Labour_Cost;
                        _objModel.Activity_StartDate = string.IsNullOrEmpty(Model.Activity_StartDate) ? "" : Model.Activity_StartDate;
                        _objModel.Activity_EndDate = string.IsNullOrEmpty(Model.Activity_EndDate) ? "" : Model.Activity_EndDate;
                        _objModel.Activity_RefNo = string.IsNullOrEmpty(Model.Activity_RefNo) ? "" : Model.Activity_RefNo;
                        _objModel.Activity_Type = string.IsNullOrEmpty(Model.Activity_Type) ? "" : Model.Activity_Type;
                        _objModel.Activity_Year = Convert.ToInt32(fCollection["ActivityYear"]);
                        _objModel.Activity_TotalCost = Model.Activity_TotalCost;
                        _objModel.IsChkSubActivity = fCollection["IsSubActivity"].ToString();
                        _objModel.RatePerUnit = Convert.ToDecimal(fCollection["RatePerUnit"]); 
                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objModel.Activity_DocumentPath = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objModel.Activity_DocumentPath = ""; }

                        _objModel.Activity_EndDate = string.IsNullOrEmpty(Model.Activity_EndDate) ? "" : Model.Activity_EndDate;

                        id = _objModel.SubmitActivity(_objModel);
                        if (id > 0)
                        {
                            if (Session["SubActData"] != null && _objModel.IsChkSubActivity == "1")
                            {
                                List<SubActivity> list = (List<SubActivity>)Session["SubActData"];
                                if (list != null)
                                {
                                    list.ForEach(t => t.Activity_ID = id);
                                    _objModel.SaveActSubActMapping(list, _objModel.UserID);
                                    Session["SubActData"] = null;
                                }
                            }
                            else { Session["SubActData"] = null; }
                            TempData["ViewMessage"] = "Record Save Successfully!!";
                            return RedirectToAction("fdmActivity", "fdmActivity");

                        }
                        else
                        {

                            return View("Error");
                        }
                    }
                    else
                    {
                        _objModel.Model_ID = Model.Model_ID;
                        _objModel.Activity_Name = string.IsNullOrEmpty(Model.Activity_Name) ? "" : Model.Activity_Name;
                        _objModel.Activity_Desc = string.IsNullOrEmpty(Model.Activity_Desc) ? "" : Model.Activity_Desc;
                        //_objModel.Activity_BSR_Per_Unit = Model.Activity_BSR_Per_Unit;
                        _objModel.Sub_Activity_Unit = string.IsNullOrEmpty(Model.Sub_Activity_Unit) ? "" : Model.Sub_Activity_Unit;
                        _objModel.Activity_BSR_Material_Cost = Model.Activity_BSR_Material_Cost;
                        _objModel.Activity_BSR_Labour_Cost = Model.Activity_BSR_Labour_Cost;
                        _objModel.Activity_Type = string.IsNullOrEmpty(Model.Activity_Type) ? "" : Model.Activity_Type;
                        //_objModel.BSR_Type = string.IsNullOrEmpty(fCollection["BSR_Type"]) ? "" : fCollection["BSR_Type"];
                        _objModel.Activity_RefNo = string.IsNullOrEmpty(Model.Activity_RefNo) ? "" : Model.Activity_RefNo;
                        _objModel.Activity_TotalCost = Model.Activity_TotalCost;
                        _objModel.IsChkSubActivity = fCollection["IsSubActivity"].ToString();
                        _objModel.Activity_Year = Convert.ToInt32(fCollection["ActivityYear"]);
                        _objModel.RatePerUnit = Model.RatePerUnit;
                    }


                    if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                    {

                        FileName = Path.GetFileName(Request.Files[0].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objModel.Activity_DocumentPath = path;
                        Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                    }
                    else
                    { _objModel.Activity_DocumentPath = fCollection["hdRefDocument"].ToString(); }
                    _objModel.Activity_StartDate = string.IsNullOrEmpty(Model.Activity_StartDate) ? "" : Model.Activity_StartDate;
                    _objModel.Activity_EndDate = string.IsNullOrEmpty(Model.Activity_EndDate) ? "" : Model.Activity_EndDate;
                    _objModel.ID = Convert.ToInt64(command);
                    id = _objModel.UpdateActivity(_objModel);
                    if (id > 0)
                    {
                        if (Session["SubActData"] != null && _objModel.IsChkSubActivity == "1")
                        {
                            List<SubActivity> list = (List<SubActivity>)Session["SubActData"];
                            if (list != null)
                            {
                                list.ForEach(t => t.Activity_ID = id);
                                _objModel.SaveActSubActMapping(list, _objModel.UserID);
                                Session["SubActData"] = null;
                            }
                        }
                        else { Session["SubActData"] = null; }
                        TempData["ViewMessage"] = "Record Updated Successfully!!";
                        return RedirectToAction("fdmActivity", "fdmActivity");

                    }
                    else
                    {

                        return View("Error");
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
        /// Edit the Activity data for specific ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult EditActivityData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<DefineModel> ModelList = new List<DefineModel>();
            Session["EditMode"] = null;
            Session["activity_ID"] = null;
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet ds = _objModel.GetAllRecords(Convert.ToInt64(ID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _objModel.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);

                        _objModel.Activity_Name = ds.Tables[0].Rows[0]["Activity_Name"].ToString();
                        //DateTime datefrom = new DateTime();
                        //datefrom = Convert.ToDateTime(ds.Tables[0].Rows[0]["Activity_StartDate"].ToString());
                        //_objModel.Activity_StartDate = datefrom.ToString("dd/MM/yyyy");
                        //DateTime dateto = new DateTime();
                        //dateto = Convert.ToDateTime(ds.Tables[0].Rows[0]["Activity_EndDate"].ToString());
                        //_objModel.Activity_EndDate = dateto.ToString("dd/MM/yyyy");
                        _objModel.Activity_Desc = ds.Tables[0].Rows[0]["Activity_Desc"].ToString();
                        _objModel.Sub_Activity_Unit = ds.Tables[0].Rows[0]["Sub_Activity_Unit"].ToString();
                        // _objModel.Activity_BSR_Per_Unit = Convert.ToDecimal(ds.Tables[0].Rows[0]["Activity_BSR_Per_Unit"].ToString());
                        _objModel.Activity_BSR_Material_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Activity_BSR_Material_Cost"].ToString());
                        _objModel.Activity_BSR_Labour_Cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Activity_BSR_Labour_Cost"].ToString());
                        _objModel.Activity_RefNo = ds.Tables[0].Rows[0]["Activity_RefNo"].ToString();
                        _objModel.Activity_DocumentPath = ds.Tables[0].Rows[0]["Activity_DocumentPath"].ToString();
                        _objModel.Activity_Type = ds.Tables[0].Rows[0]["Activity_Type"].ToString();
                        _objModel.BSR_Type = ds.Tables[0].Rows[0]["BSR_Type"].ToString();
                        _objModel.IsChkSubActivity = ds.Tables[0].Rows[0]["IsSubActvity"].ToString();
                        _objModel.Activity_TotalCost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Activity_TotalCost"].ToString());
                        _objModel.Activity_Year = Convert.ToInt32(ds.Tables[0].Rows[0]["Activity_Year"].ToString());

                        _objModel.RatePerUnit = Convert.ToDecimal(ds.Tables[0].Rows[0]["RatePerUnit"].ToString());

                        ViewData["EditMode"] = "1";
                        Session["EditMode"] = "1";
                        Session["activity_ID"] = _objModel.ID;
                    }
                    return Json(new
                    {

                        Model_ID = _objModel.Model_ID,
                        Activity_StartDate = _objModel.Activity_StartDate,
                        Activity_EndDate = _objModel.Activity_EndDate,
                        Activity_Desc = _objModel.Activity_Desc,
                        Activity_Name = _objModel.Activity_Name,
                        //Activity_BSR_Per_Unit = _objModel.Activity_BSR_Per_Unit,
                        Activity_BSR_Material_Cost = _objModel.Activity_BSR_Material_Cost,
                        Activity_BSR_Labour_Cost = _objModel.Activity_BSR_Labour_Cost,
                        Activity_RefNo = _objModel.Activity_RefNo,
                        DocumentPath = _objModel.Activity_DocumentPath,
                        ActvityType = _objModel.Activity_Type,
                        //BSR_Type = _objModel.BSR_Type,
                        IsChkSubActivity = _objModel.IsChkSubActivity,
                        ID = Convert.ToInt64(ID),
                        Activity_TotalCost = _objModel.Activity_TotalCost,
                        Activity_Year = _objModel.Activity_Year,
                        Sub_Activity_Unit = _objModel.Sub_Activity_Unit,
                        RatePerUnit = _objModel.RatePerUnit
                    });


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;


        }
        /// <summary>
        /// To Decactivate the Activity
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult DeleteActivityData(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    if (ID != "")
                    {
                        _objModel.TableName = Common.TableName.tbl_mst_FDM_Activity.ToString();
                        _objModel.ID = (Convert.ToInt64(ID));
                        Int64 id = _objModel.DeleteActivity(_objModel);
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
        //Comment:- Code change for Bug ID:-234 by gaurav pandey on 25 feb

        /// <summary>
        /// Save the Subactivity data with Activity
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// 


        public JsonResult SaveSubActivityMapping(string ID)
        {

            List<SubActivity> _objData = new List<SubActivity>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["SubActData"] != null)
                {
                    List<SubActivity> list = (List<SubActivity>)Session["SubActData"];

                    if (list != null)
                    {
                        SubActivity obj = new SubActivity { ID = Convert.ToInt64(ID) };
                        list.Add(obj);
                        Session["SubActData"] = list;
                    }

                }
                else
                {

                    SubActivity obj = new SubActivity { ID = Convert.ToInt64(ID) };
                    _objData.Add(obj);
                    Session["SubActData"] = _objData;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }
        /// <summary>
        /// Delete the Mapping betweem Subactivity and Activity if use change prefences. 
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
                List<SubActivity> list = (List<SubActivity>)Session["SubActData"];
                Int64 subactid = 0;

                for (int i = 0; i < list.Count; i++)
                {

                    if (list[i].ID == Convert.ToInt64(sid))
                    {
                        subactid = Convert.ToInt64(sid);
                        list.RemoveAll(item => item.ID == subactid);

                        if (Session["EditMode"] != null && Session["activity_ID"] != null)
                        {
                            new Activity().DeleteActSubActMapping(Convert.ToInt64(Session["activity_ID"]), subactid);
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
        /// View/Display the All the data which saved corresponding to activity
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public JsonResult GetListData(string ActID)
        {
            Session["SubActData"] = null;
            DataSet dsSubactivity = new SubActivity().GetMapRecords(Convert.ToInt64(ActID));
            List<SubActvityMap> listdata = new List<SubActvityMap>();
            List<SubActivity> _objData = new List<SubActivity>();
            for (int i = 0; i < dsSubactivity.Tables.Count; i++)
            {
                foreach (DataRow dr in dsSubactivity.Tables[0].Rows)
                {
                    listdata.Add(new SubActvityMap()
                    {
                        ID = dr["ID"].ToString(),
                        Sub_Activity_BSRType = dr["Sub_Activity_BSRType"].ToString(),
                        Sub_Activity_Name = dr["Sub_Activity_Name"].ToString(),
                        Sub_Activity_Unit = dr["Sub_Activity_Unit"].ToString(),
                        BitStaus = dr["BitStaus"].ToString(),
                        RatePerUnit = Convert.ToDecimal(dr["RatePerUnit"]),
                        Sub_Activity_totalCost = dr["Sub_Activity_TotalCost"].ToString()
                    });

                }

            }
            if (Session["SubActData"] == null)
            {



                for (int i = 0; i < dsSubactivity.Tables[0].Rows.Count; i++)
                {
                    if (dsSubactivity.Tables[0].Rows[i]["BitStaus"].ToString() == "1")
                    {
                        SubActivity obj = new SubActivity { ID = Convert.ToInt64(dsSubactivity.Tables[0].Rows[i]["ID"]) };
                        _objData.Add(obj);

                    }

                }
                Session["SubActData"] = _objData;


            }



            return Json(listdata, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetBSRByDivActivity(string Div_Code,Int64 Activity_ID)
        {
            GenericClasses<CircleActivity> genCA = new GenericClasses<CircleActivity>();
          
            //GetBSRByDivActivity(Div_Code, Activity_ID);
            return Json(genCA.lstmodel(_objModel.GetBSRByDivActivity(Div_Code, Activity_ID)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveCircleActivityData(Activity Model, string command, FormCollection fCollection)
        {
            int totalSubActivity = Convert.ToInt32(fCollection["totalSubActivity"]);
             DataTable dtSubActivity = new DataTable("Table");
                dtSubActivity.Columns.Add("ID", typeof(String));                
                dtSubActivity.Columns.Add("BSRLabour", typeof(String));
                dtSubActivity.Columns.Add("BSRMat", typeof(String));                
                dtSubActivity.AcceptChanges();
              for (int i = 1; i <= totalSubActivity; i++)
                {
                    DataRow dr = dtSubActivity.NewRow();
                    string ID  = Convert.ToString(fCollection["saID_" + i.ToString()]).Split('#')[0];
                    string type = Convert.ToString(fCollection["saID_" + i.ToString()]).Split('#')[1];
                    string BSRLabour  = fCollection["sadivbsr_" + i.ToString()];           
                    string BSRMat  = fCollection["divbsrMat_" + i.ToString()];
                    _objModel.SubmitDivwiseActivity(fCollection["division"], fCollection["Activity"], ID, BSRLabour, BSRMat, type, Convert.ToInt64(Session["UserId"]));
                }
        

            return RedirectToAction("fdmCircleActivity");
        }

        #region Developed By Rajveer 
        public ActionResult AddActivity()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActivityModel model = new ActivityModel();
            try
            {

                ActivityRepo repo = new ActivityRepo();
                DataTable Category = new DataTable();
                ViewBag.Activity_Type = new SelectList(Common.GetActivityCategoryTypeWildlife(), "Value", "Text");
                Category = repo.BindActivityYear();
                ViewBag.Category = new SelectList(Category.AsDataView(), "ID", "YearName");
                ViewBag.CampaCategory = new SelectList(Common.GetCampaCategory(), "Value", "Text");
                DataSet ds = new DataSet();
                ds = repo.InsertActivity(model, "LIST", Convert.ToInt64(Session["UserID"]));

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region Marge Data
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ActivityModels>>(str);
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
        public ActionResult AddActivity(ActivityModel model, HttpPostedFileBase Activity_DocumentPath)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                ActivityRepo repo = new ActivityRepo();
                DataSet ds = new DataSet();
                string FilePath = "~/PermissionDocument/";
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {

                    string FileName = Path.GetFileName(Request.Files[0].FileName);
                    string FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    string path = Path.Combine(FilePath, FileFullName);
                    model.ReferenceDoc = path.Remove(0, 1);
                    Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                }
                else
                {
                    model.ReferenceDoc = model.ReferenceDoc;
                }

                if (!string.IsNullOrEmpty(model.ID))
                {
                    ds = repo.InsertActivity(model, "UPDATE", Convert.ToInt64(Session["UserID"]));
                }
                else
                {
                    ds = repo.InsertActivity(model, "INSERT", Convert.ToInt64(Session["UserID"]));
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
            return RedirectToAction("AddActivity");
        }

        #endregion
    }
}
