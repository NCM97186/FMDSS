//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : SurveyReportController
//  Description  : File contains calling functions from UI
//  Date Created : 13-06-2016
//  History      :
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************



using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForesterDevelopment;
using FMDSS.Models;
using System.Xml;
using System.IO;
using System.Data.SqlTypes;
using System.Configuration;


namespace FMDSS.Controllers.ForesterDevelopment
{
    public class SurveyReportController : BaseController
    {
        Int32 ModuleID = 2;
        List<SurveyReport> reportList = new List<SurveyReport>();
        List<SelectListItem> RangeName = new List<SelectListItem>();
        List<SelectListItem> FileType = new List<SelectListItem>();
    
        /// <summary>
        /// Get the survey list
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyReport()
        {
            SurveyReport sr = new SurveyReport();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
 
            try
            {
                if (Session["UserID"] != null)
                {
                 
                    ViewBag.UserName = Session["User"].ToString();
                    sr.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (Session["DesignationDes"] != null)
                    {
                        ViewBag.Designation = Session["DesignationDes"].ToString();
                    }
                    else
                    {
                        ViewBag.Range = RangeName;

                    }
                    DataTable dtd = new DataTable();

                    DataTable dt = sr.Select_Range();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.Range = RangeName;

                    DataTable dt_File = sr.Select_FileTypes();
                    foreach (DataRow dr in dt_File.Rows)
                    {
                        FileType.Add(new SelectListItem { Text = @dr["FileType"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.FileTypes = FileType;
                    
                    DataTable dtf = new DataTable();
                    dtf = sr.Select_SurveyReports();
                    if (dtf != null)
                    {
                        if (dtf.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtf.Rows)
                            {
                                reportList.Add(new SurveyReport()
                                {

                                    Village = dr["VILL_NAME"].ToString(),
                                    AreaName =  dr["AreaName"].ToString(),
                                    Description = dr["Description"].ToString(),
                                    ComplitionYear = dr["ComplitionYear"].ToString(),
                                    SDate = dr["SurveyDate"].ToString(),
                                    PhotoURL = dr["PhotoURL_FileName"].ToString(),
                                    SurveyorName = dr["Name"].ToString(),
                                    //Quantity = Convert.ToInt32(dr["Quantity"].ToString()),
                                });
                            }

                        }
                        else
                        {
                            reportList.Add(new SurveyReport()
                            {
                                Village = "",
                                Project = "",
                                ActivityPercentage = 0,
                                ComplitionYear = "",
                                SDate = "",
                                SurveyorName = ""
                            });
                        }
                    }
                    else
                    {
                        reportList.Add(new SurveyReport()
                        {
                            Village = "",
                            Project = "",
                            Activity = "",
                            ComplitionYear = "",
                            SDate = "",
                            SurveyorName = ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View(reportList);
        }


        /// <summary>
        /// Use to bind village dropdown
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Village(string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(RangeCode))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.RangeCode = RangeCode;
                    DataTable dt = sr.Select_Villages();
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.MicroPlan = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Use to bind Micro Plan dropdown
        /// </summary>
        /// <param name="VillageID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Plan(string VillageID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(VillageID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.VillageCode = VillageID;
                    DataTable dt = sr.Select_MicroPlan_By_Village();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["MicroPlanName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.MicroPlan = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        } 

        /// <summary>
        /// Use to Bind work order dropdown
        /// </summary>
        /// <param name="VillageID"></param>
        /// <returns></returns>
        public JsonResult Bind_WorkOrder(string VillageID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(VillageID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.VillageCode = VillageID;
                    DataTable dt = sr.Select_WorkOrder_By_Village();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["WorkOrder_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.WorkOrder = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Use to bind project dropdown
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Project(string PlanID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> proj = new List<SelectListItem>();
            List<SelectListItem> workOrder = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(PlanID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.MicroPlanID = Convert.ToInt64(PlanID);
                    DataTable dt = sr.Select_Project();
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        proj.Add(new SelectListItem { Text = @dr["Project_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    DataTable dt1 = sr.Select_WorkOrder();
                    foreach (System.Data.DataRow dr in dt1.Rows)
                    {
                        workOrder.Add(new SelectListItem { Text = @dr["WorkOrder_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = proj, list2 = workOrder, JsonRequestBehavior.AllowGet });
            //return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// Use to bind scheme dropdown
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Scheme(string ProjectID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(ProjectID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.ProjectID = Convert.ToInt64(ProjectID);
                    DataTable dt = sr.Select_Scheme();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Scheme_Name"].ToString(), Value = @dr["Scheme_ID"].ToString() });
                    }
                    ViewBag.Scheme = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }


        /// <summary>
        /// Use to bind activity dropdown
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Activity(string WorkOrderID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(WorkOrderID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.WorkOrderID = Convert.ToInt64(WorkOrderID);
                    // DataTable dt = sr.Select_Activity_by_Scheme();
                    DataTable dt = sr.Select_Activity_by_WorkOrder();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    // ViewBag.Scheme = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult Bind_SubActivity(string WorkOrderID,string ActivityID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(WorkOrderID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.WorkOrderID = Convert.ToInt64(WorkOrderID);
                    sr.ActivityID = Convert.ToInt64(ActivityID);
                     
                    DataTable dt = sr.Select_SubActivity_by_WorkOrder();
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.fname = dt;
                        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                        {
                            items.Add(new SelectListItem { Text = @dr["Sub_Activity_Name"].ToString(), Value = @dr["ID"].ToString() });
                        }
                    }
                    else {
                        items.Add(new SelectListItem { Text = "NA", Value = "0" });
                    }
                    // ViewBag.Scheme = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// Use to get remaining quantity of work (Code added on 4 Nov 2016)
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult Get_Quantity(string ActivityID, string WorkOrderID,string SubActivityID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Int64 quantity = 0;
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(ActivityID))
                {
                    SurveyReport sr = new SurveyReport();
                    sr.ActivityID = Convert.ToInt64(ActivityID);
                    sr.WorkOrderID = Convert.ToInt64(WorkOrderID);
                    sr.SubActivty = Convert.ToInt64(SubActivityID);
                    // DataTable dt = sr.Select_Activity_by_Scheme();
                      quantity = sr.GetQuantity(sr);
                    
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = quantity }, JsonRequestBehavior.AllowGet);
           
        }

        /// <summary>
        /// use to get the activity xml data
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult activityData(SurveyReport survey)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            XmlDocument xmldoc = new XmlDocument();
            string Status = "0";
            if (survey != null)
            {
                try
                {
                    if (Session["surveyInfo"] != null)
                    {
                 
                        DataSet ds = new DataSet();
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (Convert.ToInt64(ds.Tables[0].Rows[i]["WorkOrderID"].ToString()) == survey.WorkOrderID && Convert.ToInt64(ds.Tables[0].Rows[i]["activityID"].ToString()) == survey.ActivityID && Convert.ToInt64(ds.Tables[0].Rows[i]["SubActivityID"].ToString()) == survey.SubActivty)
                                    {
                                       // survey.Village = "";

                                        Status = "1";
                                    
                                    }

                                }
                            }
                
                        }
                    }
                  


   

                        string filename = "Survey_" + DateTime.Now.Ticks.ToString();

                        if (Session["surveyInfo"] == null)
                        {
                            XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                            xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                            XmlElement RootNode = xmldoc.CreateElement("SurveyActivityInfo");
                            xmldoc.AppendChild(RootNode);
                            xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                            Session["surveyInfo"] = filename;
                        }
                        else
                        {
                            xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                        }


                        if (Status == "0")
                        {
                            XmlElement activityInfo = xmldoc.CreateElement("ActivityDetails");
                            XmlElement villageID = xmldoc.CreateElement("VillageCode");
                            XmlElement villageName = xmldoc.CreateElement("Village");

                            XmlElement activityID = xmldoc.CreateElement("ActivityID");
                            XmlElement activity = xmldoc.CreateElement("Activity");
                            XmlElement SubActivityID = xmldoc.CreateElement("SubActivityID");
                            XmlElement SubActivity = xmldoc.CreateElement("SubActivity");
                            XmlElement workID = xmldoc.CreateElement("WorkOrderID");
                            XmlElement work = xmldoc.CreateElement("WorkOrder");
                            XmlElement activityPer = xmldoc.CreateElement("ActivityPercentage");
                            XmlElement QuantityPer = xmldoc.CreateElement("Quantity");

                            villageID.InnerText = survey.VillageCode;
                            villageName.InnerText = survey.Village;
                            activityID.InnerText = Convert.ToString(survey.ActivityID);
                            activity.InnerText = Convert.ToString(survey.Activity);
                            SubActivityID.InnerText = Convert.ToString(survey.SubActivty);
                            SubActivity.InnerText = Convert.ToString(survey.SubActivtyName);
                            workID.InnerText = Convert.ToString(survey.WorkOrderID);
                            work.InnerText = Convert.ToString(survey.WorkOrder);
                            activityPer.InnerText = Convert.ToString(survey.ActivityPercentage);
                            QuantityPer.InnerText = Convert.ToString(survey.Quantity);
                            activityInfo.AppendChild(villageName);
                            activityInfo.AppendChild(villageID);
                            activityInfo.AppendChild(activity);
                            activityInfo.AppendChild(activityID);
                            activityInfo.AppendChild(SubActivity);
                            activityInfo.AppendChild(SubActivityID);
                            activityInfo.AppendChild(work);
                            activityInfo.AppendChild(workID);
                            activityInfo.AppendChild(activityPer);
                            activityInfo.AppendChild(QuantityPer);

                            xmldoc.DocumentElement.AppendChild(activityInfo);
                            xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                        }
                     
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(survey, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete the xml data by id
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="WId"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deleteactivityData(SurveyReport activity, string WId, string Aid, string SubActID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Session["surveyInfo"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["WorkOrderID"].ToString() == WId && ds.Tables[0].Rows[i]["activityID"].ToString() == Aid && ds.Tables[0].Rows[i]["SubActivityID"].ToString() == SubActID)
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                                        Session["surveyInfo"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(activity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Use to get total amount activity
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_ActivityAmount(string ActivityID, string SubActivty, string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SurveyReport> items = new List<SurveyReport>();
            try
            {
                if (!String.IsNullOrEmpty(ActivityID))
                {

                    SurveyReport sr = new SurveyReport();
               
                    sr.ActivityID = Convert.ToInt64(ActivityID);
                    sr.SubActivty = Convert.ToInt32(SubActivty);
                    sr.RangeCode = RangeCode;
                    DataTable dt = sr.Select_Activity_BSRAmount();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SurveyReport()
                        {
                            Unit =  dr["Sub_Activity_Unit"].ToString(),
                            TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                            //ActivityID = Convert.ToInt64(dr["ActivityID"].ToString()),
                            

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Used to submit the survey detail
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="Command"></param>
        /// <param name="form"></param>
        /// <param name="Message"></param>
        /// <param name="PhotoURL"></param>
        /// <returns></returns>
        public ActionResult SubmitDetails(SurveyReport sr, string Command, FormCollection form, string Message, HttpPostedFileBase PhotoURL)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["PermissionDocument"].ToString();
            //string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            Session["Status"] = null;
            try
            {
                if (Session["UserId"] != null)
                {
                    sr.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }
                string option = string.Empty;
                if (Command == "Submit")
                {
                    // option = "Insert";
                    //if (PhotoURL != null && PhotoURL.ContentLength > 0)
                    //{
                    //    string PhotoFile = Path.GetFileName(PhotoURL.FileName);
                    //    String FileFullName = DateTime.Now.Ticks + "_" + PhotoFile;
                    //    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    //    sr.PhotoURL = @"~/PermissionDocument/" + FileFullName.Trim();
                    //    PhotoURL.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                    //}
                    //else
                    //{ sr.PhotoURL = ""; }
                    if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                    {

                        FileName = Path.GetFileName(Request.Files[0].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        sr.PhotoURL = path;
                        Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                    }
                    else
                    { sr.PhotoURL = ""; }
                    if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                    {

                        FileName = Path.GetFileName(Request.Files[1].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        sr.FileURL = path;
                        Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                    }
                    else
                    { sr.FileURL = ""; }
                    if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                    {

                        FileName = Path.GetFileName(Request.Files[2].FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        sr.ShapeFileURL = path;
                        Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));

                    }
                    else
                    { sr.ShapeFileURL = ""; }

                    if (form["SurveyDate"].ToString() == "")
                    {
                        sr.SurveyDate = Convert.ToDateTime(SqlDateTime.Null);
                    }
                    else
                    {
                        sr.SurveyDate = DateTime.ParseExact(form["SurveyDate"].ToString(), "dd/MM/yyyy", null);
                    }
                    DataSet dsm = new DataSet();
                    if (Session["surveyInfo"] != null)
                    {
                        dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                    }
                    if (dsm != null)
                    {
                        //for (int i = 5; i >= 0; i--)
                        //{
                        dsm.Tables[0].Columns.Remove("Village");
                        dsm.Tables[0].Columns.Remove("activity");
                        dsm.Tables[0].Columns.Remove("SubActivity");

                        dsm.Tables[0].Columns.Remove("WorkOrder");
                       // }

                        dsm.Tables[0].AcceptChanges();
                    }
                    Int64 status = sr.AddSurveyDetails(dsm.Tables[0]);
                    if (Session["surveyInfo"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["surveyInfo"].ToString() + ".xml"));
                            Session["surveyInfo"] = null;
                        }
                    }
                    if (status > 0)
                    {
                        Session["Status"] = "Saved Successfully";
                    }
                    else
                    {
                        Session["Status"] = "Not inserted";
                    }
                }
                else if (Command == "Update")
                {
                    option = "Update";
                    Int64 status = 0;// am.AddAsset(option);
                    if (status > 0)
                    {
                        Session["Status"] = "Updated Successfully";
                    }
                    else
                    {
                        Session["Status"] = "Error Occured";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("SurveyReport", "SurveyReport");
        }
    }
}