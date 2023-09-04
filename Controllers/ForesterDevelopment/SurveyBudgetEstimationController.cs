//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Budget Estimation Survey Report
//  Date Created : 26-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  : Arvind Srivastava  
//  Modified On  : 26-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using FMDSS.Filters;
using FMDSS.Models;
using FMDSS.Models.ForesterDevelopment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FMDSS.Controllers.ForesterDevelopment
{

    public class SurveyBudgetEstimationController : BaseController
    {
        //
        // GET: /SurveyBudgetEstimation/
        // //History: Code Update with ref. to bug ID: 278 Arvind

        Int32 ModuleID = 2;
        List<SurveyBudget> reportList = new List<SurveyBudget>();
        List<SelectListItem> RangeName = new List<SelectListItem>();
        SurveyBudget sb = new SurveyBudget();

        /// <summary>
        /// Call when request come SurveyBudgetEstimation view Bind Range Dropdown
        /// </summary>
        /// <returns>View for SurveyBudgetEstimation</returns>
        public ActionResult SurveyBudgetEstimation()
        {

            SurveyBudget sb = new SurveyBudget();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //Session["DesignationId"].ToString() 
            try
            {
              
                    RangeName.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    DataTable dtd = new DataTable();
                    sb.UserName = Session["User"].ToString();
                    sb.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    sb.Designation = Session["DesignationDes"].ToString();
                    DataTable dt = sb.Select_Range();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.Range = RangeName;

                    DataTable dtf = sb.Select_Bgt_Estimate_Survey(Convert.ToInt64(Session["UserId"]));

                    foreach (DataRow dr in dtf.Rows)
                    {
                        reportList.Add(new SurveyBudget()
                        {
                            SurveyID = Convert.ToInt64(dr["ID"].ToString()),
                            SurveyorName = dr["BudgetEsSurvey"].ToString(),
                            SDate = dr["SurveyDate"].ToString(),
                            TotalAmount = Convert.ToDecimal(dr["Total_Survey_Amount"].ToString()),
                            ComplitionYear = dr["CompletionYear"].ToString(),
                            Description = dr["Discription"].ToString(),
                            Area = dr["Area"].ToString(),
                            AreaName = dr["AreaName"].ToString(),
                            PhotoURL = dr["Photo"].ToString(),
                            Status = Convert.ToInt32(dr["SurveyUserd"].ToString())
                            //RangeCode = dr["RANGE_NAME"].ToString(),
                            //Village = dr["VILL_NAME"].ToString()

                        });
                    }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }

            return View("SurveyBudgetEstimation", reportList);
        }




       
        //History: Code Update with ref. to bug ID: 232 Arvind
        /// <summary>
        /// Bind survey Details
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {

            DataTable dtf = sb.Select_Bgt_Estimate_Survey(Convert.ToInt64(Session["UserId"]));

            foreach (DataRow dr in dtf.Rows)
            {
                reportList.Add(new SurveyBudget()
                {

                    SurveyID = Convert.ToInt64(dr["ID"].ToString()),
                    SurveyorName = dr["BudgetEsSurvey"].ToString(),
                    SDate = dr["SurveyDate"].ToString(),
                    TotalAmount = Convert.ToDecimal(dr["Total_Survey_Amount"].ToString()),
                    ComplitionYear = dr["CompletionYear"].ToString(),
                    Description = dr["Discription"].ToString(),
                    Area = dr["Area"].ToString(),
                    AreaName = dr["AreaName"].ToString(),
                    PhotoURL = dr["Photo"].ToString(),
                    Status = Convert.ToInt32( dr["SurveyUserd"].ToString())
                    //RangeCode = dr["RANGE_NAME"].ToString(),
                    //Village = dr["VILL_NAME"].ToString()

                });
            }
            //return dtf;
            return View("index", reportList);
            //return View();
        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult Bind_Village(string RangeCode)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.RangeCode = RangeCode;
            DataTable dt = new Common().Select_VillagesbyRange(RangeCode);
            return dtToViewBagJSON(dt, "VILL_NAME", "VILL_CODE");
        }

        /// <summary>
        /// Function to bind all Plan to dropdownlist
        /// </summary>
        /// <param name="VillageID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Plan(string VillageID)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.VillageCode = VillageID;
            DataTable dt = sb.Select_MicroPlan_By_Village();
            return dtToViewBagJSON(dt, "MicroPlanName", "ID");
        }

        /// <summary>
        /// Function to bind all Project to dropdownlist
        /// </summary>
        /// <param name="MicroPlanID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Project(string MicroPlanID)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.MicroPlanID = Convert.ToInt64(MicroPlanID);
            DataTable dt = sb.Select_Project_By_MicroPlanID();
            return dtToViewBagJSON(dt, "Project_Name", "ID");
        }

        /// <summary>
        /// Function to bind all Model to dropdownlist
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_Modal(string ProjectID)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.ProjectID = Convert.ToInt64(ProjectID);
            DataTable dt = sb.Select_Model_By_ProjectID();
            return dtToViewBagJSON(dt, "Model_Name", "ID");
        }

        /// <summary>
        /// Function to bind all Model to dropdownlist
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Bind_ModalName(string MicroPlanID)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.MicroPlanID = Convert.ToInt64(MicroPlanID);
            DataTable dt = sb.Select_Model_By_MicroId();
            return dtToViewBagJSON(dt, "Model_Name", "ID");
        }

        /// <summary>
        /// Function to bind all Activity to dropdownlist
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult Bind_Activity(string ModelID)
        {
            SurveyBudget sb = new SurveyBudget();
            sb.ModelID = Convert.ToInt64(ModelID);
            DataTable dt = sb.Select_Activity_By_ModelID();
            return dtToViewBagJSON(dt, "Activity_Name", "ID");
        }

        /// <summary>
        /// Function to bind all dropdown
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
            Int64 UserID = 0;

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
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

        /// <summary>
        /// Function to Add multiple Activity
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult activityData(SurveyBudget sb)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (sb != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "Survey_Budget_Estimation" + DateTime.Now.Ticks.ToString();

                    if (Session["budgetEstimatedInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("budgetEstimatedInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["budgetEstimatedInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                    }

                    XmlElement activityInfo = xmldoc.CreateElement("ActivityDetails");
                    //XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement rangeCode = xmldoc.CreateElement("RangeCode");
                    XmlElement villageID = xmldoc.CreateElement("VillageCode");
                    //XmlElement villageName = xmldoc.CreateElement("Village");
                    XmlElement microPlanID = xmldoc.CreateElement("MicroPlanID");
                    //XmlElement microPlanName = xmldoc.CreateElement("MicroPlan");
                    XmlElement schemeID = xmldoc.CreateElement("ModelID");
                    // XmlElement scheme = xmldoc.CreateElement("Model");
                    XmlElement projectID = xmldoc.CreateElement("ProjectID");
                    // XmlElement project = xmldoc.CreateElement("Project");
                    XmlElement activityID = xmldoc.CreateElement("ActivityID");
                    //XmlElement activity = xmldoc.CreateElement("Activity");
                    XmlElement SubActivityID = xmldoc.CreateElement("SubActivityID");
                    XmlElement estimatedAmt = xmldoc.CreateElement("Amount");

                    rangeCode.InnerText = sb.RangeCode;
                    villageID.InnerText = sb.VillageCode;
                    //villageName.InnerText = sb.Village;
                    microPlanID.InnerText = Convert.ToString(sb.MicroPlanID);
                    //microPlanName.InnerText = sb.MicroPlan;
                    schemeID.InnerText = Convert.ToString(sb.ModelID);
                    //scheme.InnerText = Convert.ToString(sb.ModelName);
                    projectID.InnerText = Convert.ToString(sb.ProjectID);
                    //project.InnerText = Convert.ToString(sb.Project);
                    activityID.InnerText = Convert.ToString(sb.ActivityID);
                    // activity.InnerText = Convert.ToString(sb.Activity);
                    SubActivityID.InnerText = Convert.ToString(sb.SubActivityID);
                    estimatedAmt.InnerText = Convert.ToString(sb.EstimatedAmt);

                    // activityInfo.AppendChild(villageName);
                    // activityInfo.AppendChild(microPlanName);
                    //activityInfo.AppendChild(scheme);
                    // activityInfo.AppendChild(project);
                    // activityInfo.AppendChild(activity);
                    //activityInfo.AppendChild(estimatedAmt);
                    activityInfo.AppendChild(rangeCode);
                    activityInfo.AppendChild(villageID);
                    activityInfo.AppendChild(microPlanID);
                    activityInfo.AppendChild(schemeID);
                    activityInfo.AppendChild(projectID);
                    activityInfo.AppendChild(activityID);
                    activityInfo.AppendChild(SubActivityID);
                    activityInfo.AppendChild(estimatedAmt);

                    xmldoc.DocumentElement.AppendChild(activityInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(sb, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult deleteSurveyData(SurveyBudget sb, string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Session["budgetEstimatedInfo"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i + 1 == Convert.ToInt32(Id))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                                        Session["budgetEstimatedInfo"] = null;
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
            return Json(sb, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Function to save all data to database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitBudgetSurvey(FormCollection fm, string Command)
        {
          
            string status = "";
            Session["Depot_Status"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());


            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["PermissionDocument"].ToString();
            try
            {

                SurveyBudget sb = new SurveyBudget();

                if (fm["hdSurveyID"].ToString() == "")
                {
                    sb.SurveyID = 0;
                }
                else
                {
                    sb.SurveyID = Convert.ToInt64(fm["hdSurveyID"].ToString());
                }

                if (Session["UserId"] != null)
                {
                    sb.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["TotalAmount"].ToString() == "")
                {
                    sb.TotalAmount = 0;
                }
                else
                {
                    sb.TotalAmount = Convert.ToDecimal(fm["TotalAmount"].ToString());
                }

                if (fm["SurveyDate"].ToString() == "")
                {
                    sb.SurveyDate = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    sb.SurveyDate = DateTime.ParseExact(fm["SurveyDate"].ToString(), "dd/MM/yyyy", null);

                }

                if (fm["ComplitionYear"].ToString() == "")
                {
                    sb.ComplitionYear = "";
                }
                else
                {
                    sb.ComplitionYear = fm["ComplitionYear"].ToString();
                }

                if (fm["Description"].ToString() == "")
                {
                    sb.Description = "";
                }
                else
                {
                    sb.Description = fm["Description"].ToString();
                }


                if (fm["txt_areaName"].ToString() == "")
                {
                    sb.AreaName = "";
                }
                else
                {
                    sb.AreaName = fm["txt_areaName"].ToString();
                }
                if (fm["txt_area"].ToString() == "")
                {

                    sb.Area = "";
                }
                else
                {
                    sb.Area = fm["txt_area"].ToString();

                }


                if (fm["txt_latitude"].ToString() == "")
                {
                    sb.Latitude = "";
                }
                else
                {
                    sb.Latitude = fm["txt_latitude"].ToString();
                }



                if (fm["txt_longitude"].ToString() == "")
                {
                    sb.Longitude = "";
                }
                else
                {
                    sb.Longitude = fm["txt_longitude"].ToString();
                }
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {

                    FileName = Path.GetFileName(Request.Files[0].FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    sb.PhotoURL = path;
                    Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                }
                else
                { sb.PhotoURL = ""; }
                if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                {

                    FileName = Path.GetFileName(Request.Files[1].FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    sb.FileURL = path;
                    Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                }
                else
                { sb.FileURL = ""; }
                if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                {

                    FileName = Path.GetFileName(Request.Files[2].FileName);
                    FileFullName = DateTime.Now.Ticks + "_" + FileName;
                    path = Path.Combine(FilePath, FileFullName);
                    sb.ShapeFileURL = path;
                    Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));

                }
                else
                { sb.ShapeFileURL = ""; }





                string option = string.Empty;
                if (Command == "Submit")
                {
                    DataSet ds = new DataSet();
                    if (Session["budgetEstimatedInfo"] != null)
                    {
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));

                        status = sb.AddSurveyDetails(ds.Tables[0]);
                    }


                    if (Session["budgetEstimatedInfo"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                            Session["budgetEstimatedInfo"] = null;
                        }
                    }

                    if (status != "")
                    {
                        TempData["BGS_Status"] = "Survey No:#" + status + " Created Successfully";
                        //return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
                    }
                    else
                    {
                        TempData["BGS_Status"] = "No inserted";
                    }

                }



                if (Command == "Update")
                {
                    DataSet ds = new DataSet();
                    if (Session["budgetEstimatedInfo"] != null)
                    {
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));

                        status = sb.UpdateSurveyDetails(ds.Tables[0]);
                    }


                    if (Session["budgetEstimatedInfo"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                            Session["budgetEstimatedInfo"] = null;
                        }
                    }

                    if (status != "")
                    {
                        TempData["BGS_Status"] = "Survey No:#" + status + " Created Successfully";
                        //return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
                    }
                    else
                    {
                        TempData["BGS_Status"] = "No inserted";
                    }

                }



            }
            catch (Exception ex)
            {
                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
                Session["budgetEstimatedInfo"] = null;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
        }

        public JsonResult GetEditDetails(string SurveyID)
        {
            SurveyBudget sb = new SurveyBudget();
            DataSet ds = new DataSet();
            sb.SurveyID = Convert.ToInt64(SurveyID);
            ds = sb.EditSurveyDetails();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["Total_Survey_Amount"].ToString());
                    DateTime _date1 = DateTime.Parse(ds.Tables[0].Rows[0]["SurveyDate"].ToString());
                    sb.SDate = _date1.ToString("dd/MM/yyyy");
                    sb.ComplitionYear = ds.Tables[0].Rows[0]["CompletionYear"].ToString();
                    sb.Description = ds.Tables[0].Rows[0]["Discription"].ToString();
                    sb.AreaName = ds.Tables[0].Rows[0]["AreaName"].ToString();
                    sb.Area = ds.Tables[0].Rows[0]["Area"].ToString();
                    sb.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    sb.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    sb.FileURL = ds.Tables[0].Rows[0]["FileURL"].ToString();
                    sb.ShapeFileURL = ds.Tables[0].Rows[0]["ShapeFileURL"].ToString();
                    sb.PhotoURL = ds.Tables[0].Rows[0]["PhotoURL"].ToString();
                
                  
                        foreach (DataRow dr in ds.Tables[1].Rows)
                            reportList.Add(
                                new SurveyBudget()
                                {
                                    Village = dr["VILL_NAME"].ToString(),
                                    MicroPlan = dr["MicroPlanName"].ToString(),
                                    ModelName = dr["Model_Name"].ToString(),
                                  //  Project = dr["Project_Name"].ToString(),
                                    Activity = dr["Activity_Name"].ToString(),                                   
                                    RangeCode = dr["RANGE_NAME"].ToString(),
                                    VillageCode = dr["Vill_Code"].ToString(),
                                    MicroPlanID =Convert.ToInt64(dr["Micro_PanID"].ToString()),
                                  //  ProjectID = Convert.ToInt64(dr["ProjectID"].ToString()),
                                    ModelID = Convert.ToInt64(dr["ModelID"].ToString()),
                                    ActivityID = Convert.ToInt64(dr["ActivityID"].ToString()),
                                    SubActivityID = Convert.ToInt64(dr["ActivityID"].ToString()),
                                    SubActivity=dr["Sub_Activity_Name"].ToString(),
                                    Range_Code =  dr["Range_Code"].ToString(),
                                    EstimatedAmt = Convert.ToDecimal(dr["EstimatedAmt"].ToString())
               

                                });

              
                    
                }
            }
            return Json(new { TotalAmount = sb.TotalAmount, SDate = sb.SDate, ComplitionYear = sb.ComplitionYear, Description = sb.Description, AreaName = sb.AreaName, Latitude = sb.Latitude, Longitude = sb.Longitude, FileURL = sb.FileURL, ShapeFileURL = sb.ShapeFileURL, PhotoURL = sb.PhotoURL, Area = sb.Area, list2 = reportList }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteFile()
        {
            ModelState.Clear();
            if (Session["budgetEstimatedInfo"]!=null)
            { 
            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["budgetEstimatedInfo"].ToString() + ".xml"));
            Session["budgetEstimatedInfo"] = null;
            }
        }
 
    }
}
