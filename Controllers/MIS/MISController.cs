
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.Common;
using System.Web.UI.WebControls;
using FMDSS.Models;
using System.Data;
using System.Collections;
using System.Configuration;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using FMDSS.Models.OnlineBooking;
using FMDSS.Filters;
using System.IO;
using System.Web.UI;
using System.Text;
using FMDSS.Models.BookOnlineTicket;
using System.Net;
using System.Text.RegularExpressions;

namespace FMDSS.Controllers.MIS
{


    //[NoDirectAccess]
    [MyAuthorization]
    public class MISController : BaseController
    {
        //
        // GET: /MIS/
        int ModuleID = 1;

        CitizenModel Model = new CitizenModel();
        List<CS_BoardingDetails> ListBoarding = new List<CS_BoardingDetails>();
        List<MISExceptionUserLog> ListLOG = new List<MISExceptionUserLog>();

        List<MISTicketTransactionDetails> ListTicketTransactionDetails = new List<MISTicketTransactionDetails>();

        List<HalfDayFullDayDetailModel> ListHalfDayFullDayDetails = new List<HalfDayFullDayDetailModel>();

        List<EventLogDetailModel> EventLogDetails = new List<EventLogDetailModel>();

       List<MISNurseryDetails> MISNurseryDetails = new List<MISNurseryDetails>();

        [NonAction]
        public void getLoadData(Int16 status)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                List<SelectListItem> lstDuration = new List<SelectListItem>();

                if (status == 1) // with Cumulative
                {
                    lstDuration.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    lstDuration.Add(new SelectListItem { Text = "From-to Dates", Value = "From-to Dates" });
                    lstDuration.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });
                    lstDuration.Add(new SelectListItem { Text = "Quarterly", Value = "Quarterly" });
                    lstDuration.Add(new SelectListItem { Text = "Monthly", Value = "Monthly" });

                    lstDuration.Add(new SelectListItem { Text = "Cumulative", Value = "Cumulative" });
                }
                else if (status == 2) // no Cumulative
                {
                    lstDuration.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    lstDuration.Add(new SelectListItem { Text = "From-to Dates", Value = "From-to Dates" });
                    lstDuration.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });
                    lstDuration.Add(new SelectListItem { Text = "Quarterly", Value = "Quarterly" });
                    lstDuration.Add(new SelectListItem { Text = "Monthly", Value = "Monthly" });

                }
                else if (status == 3) // Year and Month Only
                {
                    lstDuration.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    lstDuration.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });
                    lstDuration.Add(new SelectListItem { Text = "Monthly", Value = "Monthly" });


                }

                ViewBag.Duration = lstDuration;

                List<SelectListItem> lstCircle = new List<SelectListItem>();

                DataTable dt1 = Model.GetCircle();

                lstCircle.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstCircle.Add(new SelectListItem { Text = "ALL", Value = "ALL" });

                foreach (System.Data.DataRow dr in dt1.Rows)
                {
                    lstCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["Circle_Code"].ToString() });
                }
                ViewBag.CIRCLE = lstCircle;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

        }
        [NonAction]
        public void getPermissionData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                List<SelectListItem> lstPermissionStatus = new List<SelectListItem>();


                DataTable dt = Model.GetPermissionStatus();
                lstPermissionStatus.Add(new SelectListItem { Text = "--Select--", Value = "0" });

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstPermissionStatus.Add(new SelectListItem { Text = dr["StatusDesc"].ToString(), Value = dr["StatusID"].ToString() });
                }
                ViewBag.PermissionStatus = lstPermissionStatus;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        [NonAction]
        public void getFYData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                List<SelectListItem> lstFY = new List<SelectListItem>();


                lstFY.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstFY.Add(new SelectListItem { Text = "2014-2015", Value = "2014-2015" });
                lstFY.Add(new SelectListItem { Text = "2015-2016", Value = "2015-2016" });







                ViewBag.FY = lstFY;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        [NonAction]
        public void UserLogExceptionData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                List<SelectListItem> lstReportType = new List<SelectListItem>();

                lstReportType.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstReportType.Add(new SelectListItem { Text = "UserLog", Value = "UserLog" });
                lstReportType.Add(new SelectListItem { Text = "Exception", Value = "Exception" });
                ViewBag.ReportType = lstReportType;



                List<SelectListItem> lstModules = new List<SelectListItem>();


                DataTable dt = Model.GetModule();
                lstModules.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstModules.Add(new SelectListItem { Text = "ALL", Value = "ALL" });

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstModules.Add(new SelectListItem { Text = dr["ModuleDesc"].ToString(), Value = dr["ModuleId"].ToString() });
                }
                ViewBag.Modules = lstModules;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        [NonAction]
        public void SessionForJson()
        {
            //if (Session["UserID"].ToString() == string.Empty || Session["UserID"] == null)
            //{
            //    RedirectToAction("login", "login");
            //}

        }

        public JsonResult DivisionData(string circleCode)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();



            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(circleCode)))
                {

                    if (circleCode == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetDivision(Convert.ToString(circleCode));

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }


        public JsonResult RangeData(string DivisionCode)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();



            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DivisionCode)))
                {
                    if (DivisionCode == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetRange(Convert.ToString(DivisionCode));

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult BlockData(string DivisionCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DivisionCode)))
                {
                    if (DivisionCode == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetBLOCK(Convert.ToString(DivisionCode));
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public void PermissionTypeCategoryData()
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                items.Add(new SelectListItem { Text = "Fixed Land Permission", Value = "Fixed Land Permission" });
                items.Add(new SelectListItem { Text = "Education Permission", Value = "Education Permission" });
                items.Add(new SelectListItem { Text = "Misc Permission", Value = "Misc Permission" });
                items.Add(new SelectListItem { Text = "Online Booking Permission", Value = "Online Booking Permission" });

                ViewBag.PermissionType = items;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


        }


        public JsonResult PermissionTypeSubCategoryData(string PermissionType)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(PermissionType)))
                {
                    if (PermissionType == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else if (PermissionType == "Fixed Land Permission")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetFIXEDPermissionSubCategory();
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["Name"].ToString() });
                        }
                    }
                    else if (PermissionType == "Education Permission")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else if (PermissionType == "Misc Permission")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        items.Add(new SelectListItem { Text = "CAMP", Value = "CAMP" });
                        items.Add(new SelectListItem { Text = "SHOOTING", Value = "SHOOTING" });
                    }
                    else if (PermissionType == "Online Booking Permission")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }



        public void getGetProgramSData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                List<SelectListItem> lstProgram = new List<SelectListItem>();

                DataTable dt = Model.GetProgramS();
                lstProgram.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstProgram.Add(new SelectListItem { Text = "ALL", Value = "ALL" });

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstProgram.Add(new SelectListItem { Text = dr["Program_Name"].ToString(), Value = dr["ID"].ToString() });
                }
                ViewBag.Program = lstProgram;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        public JsonResult GetSchemeData(string Program)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();



            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(Program)))
                {
                    if (Program == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetScheme(Program);

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["Scheme_Name"].ToString(), Value = dr["ID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult GetProjectData(string Scheme)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(Scheme)))
                {
                    if (Scheme == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetProject(Scheme);

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["Project_Name"].ToString(), Value = dr["ID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult GetModelData(string Project)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(Project)))
                {
                    if (Project == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                        DataTable dt = Model.GetModel(Project);

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["Project_Name"].ToString(), Value = dr["ID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult GetActivityData(string Models)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(Models)))
                {
                    if (Models == "ALL")
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                        items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });

                        DataTable dt = Model.GetActivity(Models);

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = dr["Activity_Name"].ToString(), Value = dr["ActivityID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }


        [NonAction]
        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }

        //========================================Citizen Service Reports

        [HttpGet]

        public ActionResult CSDivisionWise()
        {

            getLoadData(1);
            getPermissionData();
            return View();
        }

        [HttpPost]

        public ActionResult CSDivisionWise(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;

                if (fm["Duration"].ToString() == "Yearly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Quarterly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Monthly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Cumulative")
                {
                    FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else
                {
                    ToDate = fm["ToDate"].ToString();
                    FromDate = fm["FromDate"].ToString();
                }



                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;
                reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportCitizenServiceDivisionWise";
                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();
                reportParam.Add(CreateReportParameter("DURATION", fm["Duration"].ToString()));
                reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));
                reportParam.Add(CreateReportParameter("CIRCLE_CODE", fm["Circle"].ToString()));
                reportParam.Add(CreateReportParameter("DIVISON_CODE", fm["Division"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_CODE", fm["Range"].ToString()));
                reportParam.Add(CreateReportParameter("ServicesTATUS", fm["PermissionType"].ToString()));

                reportParam.Add(CreateReportParameter("DIVISION_NAME", fm["Division_Text"].ToString()));
                reportParam.Add(CreateReportParameter("CIRCLE_NAME", fm["Circle_Text"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_NAME", fm["Range_Text"].ToString()));
                reportParam.Add(CreateReportParameter("SERVICE_STATUS_NAME", fm["PermissionType_Text"].ToString()));

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }


                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;
                getLoadData(1);
                getPermissionData();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        [HttpGet]
        public ActionResult CSRevenue()
        {

            getLoadData(1);

            return View();
        }
        [HttpPost]
        public ActionResult CSRevenue(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;

                if (fm["Duration"].ToString() == "Yearly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Quarterly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Monthly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Cumulative")
                {
                    FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else
                {
                    ToDate = fm["ToDate"].ToString();
                    FromDate = fm["FromDate"].ToString();
                }

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;
                reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportCitizenServiceRevenueGeneration";
                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();
                reportParam.Add(CreateReportParameter("DURATION", fm["Duration"].ToString()));
                reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));
                reportParam.Add(CreateReportParameter("CIRCLE_CODE", fm["Circle"].ToString()));
                reportParam.Add(CreateReportParameter("DIVISON_CODE", fm["Division"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_CODE", fm["Range"].ToString()));

                reportParam.Add(CreateReportParameter("CIRCLE_NAME", fm["Circle_Text"].ToString()));
                reportParam.Add(CreateReportParameter("DIVISION_NAME", fm["Division_Text"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_NAME", fm["Range_Text"].ToString()));

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }


                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;
                getLoadData(1);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }


        [HttpGet]
        public ActionResult CSPermissionDD()
        {

            getLoadData(1);
            PermissionTypeCategoryData();
            return View();
        }

        [HttpPost]
        public ActionResult CSPermissionDD(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;

                if (fm["Duration"].ToString() == "Yearly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Quarterly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Monthly")
                {
                    FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (fm["Duration"].ToString() == "Cumulative")
                {
                    FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString(); //"01/01/2010"; 
                    ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else
                {
                    ToDate = fm["ToDate"].ToString();
                    FromDate = fm["FromDate"].ToString();
                }

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;
                // reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/MIS_ReportCitizenServiceGroupWiseDrillDown";
                reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/NewReportCitizenServiceGroupWiseDrillDown";

                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();
                reportParam.Add(CreateReportParameter("DURATION", fm["Duration"].ToString()));
                reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));
                reportParam.Add(CreateReportParameter("CIRCLE_CODE", fm["Circle"].ToString()));
                reportParam.Add(CreateReportParameter("DIVISON_CODE", fm["Division"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_CODE", fm["Range"].ToString()));
                reportParam.Add(CreateReportParameter("ServiceCategory", fm["PermissionType"].ToString()));
                reportParam.Add(CreateReportParameter("ServiceName", fm["PermissionTypeSubCategory"].ToString()));

                reportParam.Add(CreateReportParameter("DIVISION_NAME", fm["Division_Text"].ToString()));
                reportParam.Add(CreateReportParameter("CIRCLE_NAME", fm["Circle_Text"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_NAME", fm["Range_Text"].ToString()));
                reportParam.Add(CreateReportParameter("ServiceCategoryName", fm["PermissionType_Text"].ToString()));
                reportParam.Add(CreateReportParameter("ServiceNameView", fm["PermissionTypeSubCategory_Text"].ToString()));

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }


                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;
                getLoadData(1);
                PermissionTypeCategoryData();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }


        //========================================Citizen Service Reports



        //========================================Development Reports

        [HttpGet]
        public ActionResult DMExpenditureOnDevActivities()
        {

            getLoadData(3);
            getGetProgramSData();
            getFYData();
            return View();
        }

        [HttpPost]
        public ActionResult DMExpenditureOnDevActivities(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;

                if (Convert.ToString(fm["Duration"]) == "Monthly")
                {
                    reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/MonthWiseReportDevelopmentModuleExpenditureOnDevelopmentActivities";
                }
                else
                {
                    reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportDevelopmentModuleExpenditureOnDevelopmentActivities";
                }
                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();
                reportParam.Add(CreateReportParameter("DURATION", fm["Duration"].ToString()));

                reportParam.Add(CreateReportParameter("CIRCLE_CODE", fm["Circle"].ToString()));
                reportParam.Add(CreateReportParameter("DIVISON_CODE", fm["Division"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_CODE", fm["Range"].ToString()));
                reportParam.Add(CreateReportParameter("PROGRAM", fm["DDLProgram"].ToString()));
                reportParam.Add(CreateReportParameter("SCHEME", fm["DDLScheme"].ToString()));
                reportParam.Add(CreateReportParameter("PROJECT", fm["DDLProject"].ToString()));
                reportParam.Add(CreateReportParameter("MODELS", fm["DDLModel"].ToString()));
                reportParam.Add(CreateReportParameter("ACTIVITIES", fm["DDLActivity"].ToString()));

                reportParam.Add(CreateReportParameter("DIVISION_NAME", fm["Division_Text"].ToString()));
                reportParam.Add(CreateReportParameter("CIRCLE_NAME", fm["Circle_Text"].ToString()));
                reportParam.Add(CreateReportParameter("RANGE_NAME", fm["Range_Text"].ToString()));

                reportParam.Add(CreateReportParameter("PROGRAM_NAME", fm["Program_Text"].ToString()));
                reportParam.Add(CreateReportParameter("SCHEME_NAME", fm["Scheme_Text"].ToString()));
                reportParam.Add(CreateReportParameter("PROJECT_NAME", fm["Project_Text"].ToString()));
                reportParam.Add(CreateReportParameter("MODELS_NAME", fm["Model_Text"].ToString()));
                reportParam.Add(CreateReportParameter("ACTIVITIES_NAME", fm["Activity_Text"].ToString()));
                reportParam.Add(CreateReportParameter("FY", fm["FY"].ToString()));

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }


                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;

                getLoadData(3);
                getGetProgramSData();
                getFYData();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        //========================================Development Reports





































        //======================================== FMDSS User Log & Exception Reports
        // BEGIN
        [HttpGet]
        public ActionResult UserLogExceptionReport()
        {
            UserLogExceptionData();
            return View();

        }
        [HttpPost]


        public ActionResult UserLogExceptionReport(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                UserLogExceptionData();

                string FromDate = string.Empty;
                string ToDate = string.Empty;

                ToDate = fm["ToDate"].ToString();
                FromDate = fm["FromDate"].ToString();

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;

                if (Convert.ToString(fm["ReportType"]) == "Exception")
                {
                    reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportFMDSSException";
                }
                else if (Convert.ToString(fm["ReportType"]) == "UserLog")
                {
                    reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportFMDSSUserLog";
                }
                //reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                //reportviewer.Width = 1090;
                //reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();


                if (Convert.ToString(fm["ReportType"]) == "Exception")
                {
                    reportParam.Add(CreateReportParameter("ACTION", Convert.ToString(fm["ReportType"])));
                    reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                    reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));
                    reportParam.Add(CreateReportParameter("ModuleId", Convert.ToString(fm["Modules"])));
                    reportParam.Add(CreateReportParameter("ModuleName", Convert.ToString(fm["Modules_Text"])));

                }
                else if (Convert.ToString(fm["ReportType"]) == "UserLog")
                {
                    reportParam.Add(CreateReportParameter("ACTION", Convert.ToString(fm["ReportType"])));
                    reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                    reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));
                    reportParam.Add(CreateReportParameter("Moduleid", Convert.ToString(fm["Modules"])));
                    reportParam.Add(CreateReportParameter("SSOid", Convert.ToString(fm["txt_SSOID"])));
                    reportParam.Add(CreateReportParameter("ModuleName", Convert.ToString(fm["Modules_Text"])));
                }




                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }

                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();

        }

        // END
        //======================================== FMDSS User Log & Exception Reports

        [HttpGet]
        public ActionResult DepartmentalKioskDayCloserReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DepartmentalKioskDayCloserReport(FormCollection fm)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;

                ToDate = fm["ToDate"].ToString();
                FromDate = fm["FromDate"].ToString();

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;


                reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportDepartmentalKioskDayCloser";

                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();



                reportParam.Add(CreateReportParameter("DATETIME_FROM", FromDate));
                reportParam.Add(CreateReportParameter("DATETIME_TO", ToDate));


                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }

                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;

                UserLogExceptionData();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();

        }


        public void onloadTicketBooking()
        {
            ResearchPlantsAnimals obj = new ResearchPlantsAnimals();
            List<SelectListItem> Places = new List<SelectListItem>();
            List<SelectListItem> BookingType = new List<SelectListItem>();
            List<SelectListItem> ShiftType = new List<SelectListItem>();
            List<SelectListItem> ShiftTypeRemaingSeats = new List<SelectListItem>();
            List<SelectListItem> DATEtYPE = new List<SelectListItem>();
            List<SelectListItem> RemarksList = new List<SelectListItem>();

            List<SelectListItem> lstTRNS_Status = new List<SelectListItem>();

            List<SelectListItem> lstModeOfBooking = new List<SelectListItem>();


            Zone cst = new Zone();


            DataTable dtPlace = new DataTable();

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                FMDSS.Models.OnlineBooking.CS_BoardingDetails obj1 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                DataSet Ds = new DataSet();
                Ds = obj1.BindDptKioskPLACES(Session["SSOid"].ToString());

                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                lstModeOfBooking.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Kiosk Booking", Value = "2" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Counter Booking", Value = "1" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Online Booking", Value = "0" });
            }
            else
            {
                dtPlace = cst.SelectPlaces();
                foreach (System.Data.DataRow dr in dtPlace.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                lstModeOfBooking.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Kiosk Booking", Value = "2" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Counter Booking", Value = "1" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Online Booking", Value = "0" });
            }

            DataTable dtRemarks = new DataTable();
            dtRemarks = cst.SelectOnlineBookingRefundRemarks();
            foreach (System.Data.DataRow dr in dtRemarks.Rows)
            {
                RemarksList.Add(new SelectListItem { Text = @dr["RemarksName"].ToString(), Value = @dr["RemarksName"].ToString() });
            }
            ViewBag.RemarksList = RemarksList;

            BookingType.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            BookingType.Add(new SelectListItem { Text = "Online", Value = "Online" });
            BookingType.Add(new SelectListItem { Text = "Kiosk User", Value = "Kiosk User" });
            BookingType.Add(new SelectListItem { Text = "Departmental Kiosk User", Value = "Departmental Kiosk User" });

            ShiftType.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            ShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

            ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Evening", Value = "2" });


            DATEtYPE.Add(new SelectListItem { Text = "Date of Visit", Value = "DATEOFVISITE" });
            DATEtYPE.Add(new SelectListItem { Text = "Date of Booking", Value = "DATEOFBOOKING" });

            lstTRNS_Status.Add(new SelectListItem { Text = "All", Value = "-1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Success", Value = "1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Failed", Value = "0" });

            ViewBag.ddlShiftType1 = ShiftType;
            ViewBag.ddlShiftTypeRemaingSeats = ShiftTypeRemaingSeats;
            ViewBag.ddlBookingType1 = BookingType;
            ViewBag.ddlPlace1 = Places;
            ViewBag.ddlDATEtYPE1 = DATEtYPE;
            ViewBag.ddlTRNS_Status1 = lstTRNS_Status;

            ViewBag.ddlModeOfBooking = lstModeOfBooking;
        }
        [HttpGet]
        public ActionResult TicketBookingChart()
        {
            onloadTicketBooking();

            return View();
        }
        [HttpPost]
        public ActionResult TicketBookingChart(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                ReportViewer reportviewer = new ReportViewer();
                reportviewer.ProcessingMode = ProcessingMode.Remote;

                reportviewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportTicketBookingChart";

                reportviewer.ServerReport.ReportServerUrl = new Uri("http://win-250o0fi11gj/ReportServer_FMDSSSERVER");
                reportviewer.Width = 1090;
                reportviewer.Height = 600;

                ArrayList reportParam = new ArrayList();

                reportParam.Add(CreateReportParameter("DATETIME_FROM", MIS.FromDate));
                reportParam.Add(CreateReportParameter("DATETIME_TO", MIS.ToDate));

                reportParam.Add(CreateReportParameter("PlaceID", MIS.Place));
                reportParam.Add(CreateReportParameter("SHIFT_TYPE", MIS.SHIFT_TYPE));
                reportParam.Add(CreateReportParameter("BOOKING_TYPE", MIS.BOOKING_TYPE));
                reportParam.Add(CreateReportParameter("PLACE_NAME", MIS.PLACE_NAME));
                reportParam.Add(CreateReportParameter("SHIFTName", MIS.SHIFTName));


                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }

                reportviewer.ServerReport.SetParameters(param);
                reportviewer.ServerReport.Refresh();

                ViewBag.ReportViewer = reportviewer;

                UserLogExceptionData();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }


        public ActionResult TicketBookingDetails()
        {

            onloadTicketBooking();
            ViewData["ListBoarding"] = ListBoarding;
            ViewData["ListBoardingSummary"] = "";
            return View();

        }
        [HttpPost]
        public ActionResult TicketBookingDetails(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                MISCommonModel obj = new MISCommonModel();


                DataTable DT;
                DT = obj.GetBookingData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.BOOKING_TYPE, MIS.DATETYPE, MIS.TRNS_Status, MIS.ModeOfBooking);
                int count = 1;

                Session["DownloadRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            DisplayBookingId = Convert.ToString(dr["BookingId"].ToString()),
                            //HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                            //PlaceName = Convert.ToString(dr["PlaceName"]),
                            NameOfVisitor = Convert.ToString(dr["Name"]),
                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                            //ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                            //ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                            //ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),

                            FeesPerPerson = Convert.ToString(dr["PerPersonfees"]),

                            GuidName = Convert.ToString(dr["GuidName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),

                            Trn_Status_Code = Convert.ToString(dr["Trn_Status_Code"]),
                            AmountTobePaid = Convert.ToString(dr["AmountTobePaid"]),
                            Nationality = Convert.ToString(dr["Nationality"]),
                            ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),

                        });
                    count += 1;
                }



                if (DT.Rows.Count > 0)
                {
                    StringBuilder SBListBoardingSummary = new StringBuilder();

                    SBListBoardingSummary.Append("Total : " + Convert.ToString(ListBoarding.Count()));
                    SBListBoardingSummary.Append(" Success : " + Convert.ToString(ListBoarding.Count(item => item.Trn_Status_Code == "Success")));
                    SBListBoardingSummary.Append(" Failed : " + Convert.ToString(ListBoarding.Count(item => item.Trn_Status_Code == "Failed")));

                    ViewData["ListBoardingSummary"] = SBListBoardingSummary.ToString();
                }
                else
                {
                    ViewData["ListBoardingSummary"] = "";

                }

                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListBoarding"] = ListBoarding;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        [HttpGet]
        public ActionResult UserLogExceptionDetails()
        {
            UserLogExceptionData();
            MISCommonModel MIS = new MISCommonModel();
            ViewData["ListLOG"] = ListLOG;
            return View(MIS);

        }
        [HttpPost]

        public ActionResult UserLogExceptionDetails(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                string FromDate = string.Empty;
                string ToDate = string.Empty;


                UserLogExceptionData();
                MISCommonModel obj = new MISCommonModel();
                DataTable DT;
                DT = obj.GetLogData(MIS.ReportType, MIS.FromDate, MIS.ToDate, MIS.Modules, MIS.SSOID);
                int count = 1;

                if (MIS.ReportType == "UserLog")
                {

                    foreach (DataRow dr in DT.Rows)
                    {
                        ListLOG.Add(
                            new MISExceptionUserLog()
                            {
                                Index = count,
                                ClientIPAddress = Convert.ToString(dr["ClientIPAddress"].ToString()),
                                ssoid = Convert.ToString(dr["ssoid"].ToString()),
                                ModuleName = Convert.ToString(dr["ModuleName"]),
                                ServiceTypeDesc = Convert.ToString(dr["ServiceTypeDesc"]),
                                SubPermissionDesc = Convert.ToString(dr["SubPermissionDesc"]),

                                ActivityDate = Convert.ToString(dr["ActivityDate"].ToString()),
                                ActivityStartTime = Convert.ToString(dr["ActivityStartTime"].ToString()),
                                ActivityEndTime = Convert.ToString(dr["ActivityEndTime"]),
                                ActivityDuration = Convert.ToString(dr["ActivityDuration"]),

                            });
                        count += 1;
                    }
                }
                else if (MIS.ReportType == "Exception")
                {

                    foreach (DataRow dr in DT.Rows)
                    {
                        ListLOG.Add(
                            new MISExceptionUserLog()
                            {
                                Index = count,
                                ErrorDate = Convert.ToString(dr["ErrorDate"].ToString()),
                                ErrorTime = Convert.ToString(dr["ErrorTime"].ToString()),
                                Module = Convert.ToString(dr["Module"]),
                                FunctionName = Convert.ToString(dr["FunctionName"]),
                                ErrorMsg = Convert.ToString(dr["ErrorMsg"]),
                            });
                        count += 1;
                    }
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListLOG"] = ListLOG;





            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult UserLogExceptionExport(string ReportType, string FromDate, string ToDate, string Modules, string SSOID)
        {
            MISCommonModel obj = new MISCommonModel();
            DataTable dtf = new DataTable();
            dtf = obj.GetLogData(Encryption.decrypt(ReportType), Encryption.decrypt(FromDate), Encryption.decrypt(ToDate), Encryption.decrypt(Modules), Encryption.decrypt(SSOID));
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + Encryption.decrypt(ReportType) + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;
        }

        //public ActionResult TicketBookingDetailsExport(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string BOOKING_TYPE, string DATETYPE)
        //{
        //    MISCommonModel obj = new MISCommonModel();
        //    DataTable dtf = new DataTable();

        //    dtf = obj.GetBookingData(Encryption.decrypt(FromDate), Encryption.decrypt(ToDate), Encryption.decrypt(Place), Encryption.decrypt(SHIFT_TYPE), Encryption.decrypt(BOOKING_TYPE), Encryption.decrypt(DATETYPE));
        //    dtf.Columns.Remove("DisplayRequest");
        //    dtf.Columns.Remove("cnt");
        //    dtf.Columns.Remove("HDNRequestID");
        //    dtf.Columns.Remove("RequestID");
        //    if (dtf != null)
        //    {
        //        GridView gv = new GridView();
        //        gv.DataSource = dtf;
        //        gv.DataBind();
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment; filename=TicketBookingDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        //        Response.ContentType = "application/ms-excel";
        //        Response.Charset = "";
        //        StringWriter sw = new StringWriter();
        //        HtmlTextWriter htw = new HtmlTextWriter(sw);
        //        gv.RenderControl(htw);
        //        Response.Output.Write(sw.ToString());
        //        Response.Flush();
        //        Response.End();
        //    }
        //    return null;
        //}

        public ActionResult BookingDetailsExport()
        {
            DataTable dtf = (DataTable)Session["DownloadRPT"];






            if (dtf != null)
            {



                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=BookingDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }

        public ActionResult TicketBookingTransactionDetails()
        {
            Session["DownloadTransactionRPT"] = null;

            onloadTicketBooking();
            ViewData["ListTransaction"] = ListTicketTransactionDetails;
            return View();

        }
        [HttpPost]
        public ActionResult TicketBookingTransactionDetails(MISCommonModel MIS)
        {
            Session["DownloadTransactionRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                MISCommonModel obj = new MISCommonModel();


                DataTable DT;
                DT = obj.GetBookingTransactionData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.BOOKING_TYPE, MIS.DATETYPE);
                int count = 1;

                Session["DownloadTransactionRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {
                            Index = count,
                            TicketID = Convert.ToInt64(dr["TicketID"]),
                            BookingID = Convert.ToString(dr["BookingID"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            Ssoid = Convert.ToString(dr["DateDifference"]),
                            Mobile = Convert.ToString(dr["Mobile"]),
                            EmailId = Convert.ToString(dr["EmailId"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerMembers"]),
                            NoOfIndianMembers = Convert.ToString(dr["NoOfIndianMembers"]),
                            TotalMembers = Convert.ToString(dr["TotalMembers"]),
                            TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            AmountTobePaid = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dr["AmountTobePaid"])),
                            TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                            EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                        });
                    count += 1;
                }

                ListTicketTransactionDetails.Add(
                       new MISTicketTransactionDetails()
                       {

                           BookingID = "Grand Total",
                           DateOfBooking = "",
                           DateOfVisit = "",
                           Ssoid = "",
                           Mobile = "",
                           EmailId = "",
                           PlaceName = "",
                           ZoneName = "",
                           ShiftTime = "",
                           NoOfForeignerMembers = Convert.ToString(ListTicketTransactionDetails.Sum(item => Convert.ToInt64(item.NoOfForeignerMembers))),
                           NoOfIndianMembers = Convert.ToString(ListTicketTransactionDetails.Sum(item => Convert.ToInt64(item.NoOfIndianMembers))),
                           TotalMembers = Convert.ToString(ListTicketTransactionDetails.Sum(item => Convert.ToInt64(item.TotalMembers))),
                           TotalNoOfCamera = "",
                           VehicleName = "",
                           AmountTobePaid = ListTicketTransactionDetails.Sum(item => item.AmountTobePaid),
                           TransactionStatus = "",
                           EmitraTransactionID = "",
                       });




                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListTransaction"] = ListTicketTransactionDetails;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult BookingDetailsTransactionExport()
        {
            DataTable dtf = (DataTable)Session["DownloadTransactionRPT"];
            if (dtf != null)
            {

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=TicketBookingTransactionDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }

        public ActionResult NegativeBookingTransactionDetails()
        {
            Session["DownloadNegativeBookingRPT"] = null;

            onloadTicketBooking();
            ViewData["ListNegativeBooking"] = ListTicketTransactionDetails;
            return View();

        }

        [HttpPost]
        public ActionResult NegativeBookingTransactionDetails(MISCommonModel MIS)
        {
            Session["DownloadNegativeBookingRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.GetNegativeBookingTransactionData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE);
                int count = 1;
                Session["DownloadNegativeBookingRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {
                            Index = count,
                            TicketID = Convert.ToInt64(dr["TicketID"]),
                            BookingID = Convert.ToString(dr["BookingID"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),

                            Mobile = Convert.ToString(dr["Mobile"]),
                            EmailId = Convert.ToString(dr["EmailId"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerMembers"]),
                            NoOfIndianMembers = Convert.ToString(dr["NoOfIndianMembers"]),
                            TotalMembers = Convert.ToString(dr["TotalMembers"]),
                            TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ActualTicketDifference = Convert.ToString(dr["TotalNegativeTickets"]),
                            //AmountTobePaid = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dr["AmountTobePaid"])), DUE CALCULATE FORM PROC AND NOW NEW DATA CALCULATING FORM DB 
                            AmountTobePaid = Convert.ToDecimal(dr["AmountTobePaid"]),
                            TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                            EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                        });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListNegativeBooking"] = ListTicketTransactionDetails;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }
        public ActionResult NegativeBookingTransactionExport()
        {
            DataTable dtf = (DataTable)Session["DownloadNegativeBookingRPT"];
            if (dtf != null)
            {

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=OverBookingTransactionDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }

        public ActionResult SixPlusTransactionDetails()
        {
            Session["SixPlusTransactionDetailsRPT"] = null;

            onloadTicketBooking();
            ViewData["ListSixPlusTransaction"] = ListTicketTransactionDetails;
            return View();

        }
        [HttpPost]
        public ActionResult SixPlusTransactionDetails(MISCommonModel MIS)
        {
            Session["SixPlusTransactionDetailsRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.SixPlusTransactionDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.DATETYPE);
                int count = 1;
                Session["SixPlusTransactionDetailsRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {
                            Index = count,
                            TicketID = Convert.ToInt64(dr["TicketID"]),
                            BookingID = Convert.ToString(dr["BookingID"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            Mobile = Convert.ToString(dr["Mobile"]),
                            EmailId = Convert.ToString(dr["EmailId"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerTicketRequested"]),
                            NoOfIndianMembers = Convert.ToString(dr["NoOfIndianTicketRequested"]),
                            TotalMembers = Convert.ToString(dr["ActualNoOfTicketRequested"]),
                            ActualTicketDifference = Convert.ToString(dr["ActualTicketDifference"]),
                            TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            AmountTobePaid = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dr["AmountTobePaid"])),
                            TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                            EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                        });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListSixPlusTransaction"] = ListTicketTransactionDetails;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }
        public ActionResult SixPlusTransactionExport()
        {
            DataTable dtf = (DataTable)Session["SixPlusTransactionDetailsRPT"];
            if (dtf != null)
            {

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MismatchMemberDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }
        public ActionResult TicketCancellationDetails()
        {
            Session["TicketCancellationRPT"] = null;

            onloadTicketBooking();
            ViewData["ListTicketCancellation"] = ListTicketTransactionDetails;
            return View();

        }
        [HttpPost]
        public ActionResult TicketCancellationDetails(MISCommonModel MIS)
        {
            Session["TicketCancellationRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.TicketCancellationDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.DATETYPE);
                int count = 1;
                Session["TicketCancellationRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                       new MISTicketTransactionDetails()
                       {
                           Index = count,
                           // TicketID = dr["requestId"],
                           BookingID = Convert.ToString(dr["requestId"]),
                           DateOfBooking = Convert.ToString(dr["DateofBooking"]),
                           DateOfVisit = Convert.ToString(dr["DateofVisit"]),
                           Mobile = Convert.ToString(dr["Mobile"]),
                           EmailId = Convert.ToString(dr["EmailId"]),
                           PlaceName = Convert.ToString(dr["PlaceName"]),
                           /// // ZoneName = Convert.ToString(dr["ZoneName"]),
                           ///// ShiftTime = Convert.ToString(dr["ShiftTime"]),
                           ///// NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerTicketRequested"]),
                           ///// NoOfIndianMembers = Convert.ToString(dr["NoOfIndianTicketRequested"]),
                           TotalMembers = Convert.ToString(dr["TotalMembers"]),

                           ////// TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
                           ////// VehicleName = Convert.ToString(dr["VehicleName"]),
                           AmountTobePaid = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dr["AmountTobePaid"])),
                           TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                           EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                           Manual_Status = Convert.ToString(dr["Manual_Status"]),
                           Manual_Remarks = Convert.ToString(dr["Manual_Remarks"]),
                           ResendStatus = Convert.ToString(dr["ResendStatus"]),
                       });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListTicketCancellation"] = ListTicketTransactionDetails;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

		public ActionResult TicketCancellationExport()
		{
			DataTable dtf = (DataTable)Session["TicketCancellationRPT"];
			if (dtf != null)
			{

				GridView gv = new GridView();
				gv.DataSource = dtf;
				gv.DataBind();
				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=TicketCancellation_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
				Response.ContentType = "application/ms-excel";
				Response.Charset = "";
				StringWriter sw = new StringWriter();
				HtmlTextWriter htw = new HtmlTextWriter(sw);
				gv.RenderControl(htw);
				Response.Output.Write(sw.ToString());
				Response.Flush();
				Response.End();

			}
			return null;

		}

        //shaan created on 25-11-2020 jhanla refund report
        public ActionResult TicketRefundDetailsReport()
        {
            Session["TicketRefundRPT"] = null;
            List<SelectListItem> Places = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            Zone cst = new Zone();
            dtPlace = cst.SelectJhalanaPlaces();

            foreach (System.Data.DataRow dr in dtPlace.Rows)
            {
                Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }
            ViewBag.ddlPlace1 = Places;
            ViewData["ListTicketRefund"] = ListTicketTransactionDetails;
            return View();

        }
        [HttpPost]
        public ActionResult TicketRefundDetailsReport(MISCommonModel MIS)
        {
            Session["TicketRefundRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            //onloadTicketBooking();
            try
            {
                List<SelectListItem> Places = new List<SelectListItem>();
                DataTable dtPlace = new DataTable();
                Zone cst = new Zone();
                dtPlace = cst.SelectJhalanaPlaces();

                foreach (System.Data.DataRow dr in dtPlace.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }
                ViewBag.ddlPlace1 = Places;


                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.TicketRefundDetailsDataJhalana(MIS.FromDate, MIS.ToDate, MIS.Place);
                int count = 1;
                Session["TicketRefundRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                       new MISTicketTransactionDetails()
                       {
                           Index = count,
                           // TicketID = dr["requestId"],
                           BookingID = Convert.ToString(dr["requestId"]),
                           DateOfBooking = Convert.ToString(dr["DateofBooking"]),
                           DateOfVisit = Convert.ToString(dr["DateofVisit"]),
                           Mobile = Convert.ToString(dr["Mobile"]),
                           EmailId = Convert.ToString(dr["EmailId"]),
                           PlaceName = Convert.ToString(dr["PlaceName"]),
                           /// // ZoneName = Convert.ToString(dr["ZoneName"]),
                           ///// ShiftTime = Convert.ToString(dr["ShiftTime"]),
                           ///// NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerTicketRequested"]),
                           ///// NoOfIndianMembers = Convert.ToString(dr["NoOfIndianTicketRequested"]),
                           TotalMembers = Convert.ToString(dr["TotalMember"]),

                           ////// TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
                           ////// VehicleName = Convert.ToString(dr["VehicleName"]),
                           AmountTobePaid = Convert.ToDecimal(dr["AmountTobePaid"]),
                           TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                           EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                           Manual_Status = Convert.ToString(dr["Manual_Status"]),
                           Manual_Remarks = Convert.ToString(dr["Manual_Remarks"]),
                           Refund_Status = Convert.ToString(dr["Refund_Status"]),
                           Payment_Mode = Convert.ToString(dr["Payment_Mode"] == null ? "NA" : dr["Payment_Mode"]),
                           Payment_Reference_Id = Convert.ToString(dr["Payment_Reference_Id"] == null ? "NA" : dr["Payment_Reference_Id"]),
                           Date_Of_Refund = Convert.ToString(dr["Date_Of_Refund"] == null ? "NA" : dr["Date_Of_Refund"]),
                           Remark = Convert.ToString(dr["Remark"])
                       });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListTicketRefund"] = ListTicketTransactionDetails;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        [HttpPost]
        public JsonResult SubmiteToRefundJhalana(MISCommonModel model)
        {
            MISCommonModel Obj = new MISCommonModel();
            DataTable dt = new DataTable();
            dt = Obj.saveRefundedDataJhalana(model);
            int result = Convert.ToInt32(dt.Rows[0][0].ToString());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //end

        public ActionResult TicketRefundDetails()
		{
			Session["TicketRefundRPT"] = null;

			onloadTicketBooking();
			ViewData["ListTicketRefund"] = ListTicketTransactionDetails;
			return View();

		}
		[HttpPost]
		public ActionResult TicketRefundDetails(MISCommonModel MIS)
		{
			Session["TicketRefundRPT"] = null;
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
			onloadTicketBooking();
			try
			{
				MISCommonModel obj = new MISCommonModel();

				DataTable DT;
				DT = obj.TicketRefundDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.DATETYPE);
				int count = 1;
				Session["TicketRefundRPT"] = DT;

				foreach (DataRow dr in DT.Rows)
				{
					ListTicketTransactionDetails.Add(
					   new MISTicketTransactionDetails()
					   {
						   Index = count,
						   // TicketID = dr["requestId"],
						   BookingID = Convert.ToString(dr["requestId"]),
						   DateOfBooking = Convert.ToString(dr["DateofBooking"]),
						   DateOfVisit = Convert.ToString(dr["DateofVisit"]),
						   Mobile = Convert.ToString(dr["Mobile"]),
						   EmailId = Convert.ToString(dr["EmailId"]),
						   PlaceName = Convert.ToString(dr["PlaceName"]),
						   /// // ZoneName = Convert.ToString(dr["ZoneName"]),
						   ///// ShiftTime = Convert.ToString(dr["ShiftTime"]),
						   ///// NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerTicketRequested"]),
						   ///// NoOfIndianMembers = Convert.ToString(dr["NoOfIndianTicketRequested"]),
						   TotalMembers = Convert.ToString(dr["TotalMembers"]),

						   ////// TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),
						   ////// VehicleName = Convert.ToString(dr["VehicleName"]),
						   AmountTobePaid =Convert.ToDecimal(dr["AmountTobePaid"]),
						   TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
						   EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
						   Manual_Status = Convert.ToString(dr["Manual_Status"]),
						   Manual_Remarks = Convert.ToString(dr["Manual_Remarks"]),
						   Refund_Status = Convert.ToString(dr["Refund_Status"]),
						   Payment_Mode = Convert.ToString(dr["Payment_Mode"]==null? "NA": dr["Payment_Mode"]),
						   Payment_Reference_Id = Convert.ToString(dr["Payment_Reference_Id"] == null ? "NA" : dr["Payment_Reference_Id"]),
						   Date_Of_Refund= Convert.ToString(dr["Date_Of_Refund"] == null ? "NA" : dr["Date_Of_Refund"]),
						   Remark = Convert.ToString(dr["Remark"])
					   });
					count += 1;
				}


				string SSOID = Session["SSOid"].ToString();
				UserLogExceptionData();
				ViewData["ListTicketRefund"] = ListTicketTransactionDetails;
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
			}
			return View(MIS);

		}

		

		public ActionResult TicketRefundExport()
		{
			DataTable dtf = (DataTable)Session["TicketRefundRPT"];
			if (dtf != null)
			{

				GridView gv = new GridView();
				gv.DataSource = dtf;
				gv.DataBind();
				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=TicketRefund_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
				Response.ContentType = "application/ms-excel";
				Response.Charset = "";
				StringWriter sw = new StringWriter();
				HtmlTextWriter htw = new HtmlTextWriter(sw);


				for (int i = 0; i < gv.Rows.Count; i++)
				{
					GridViewRow row = gv.Rows[i];
					//ApplytextstyletoeachRow
					row.Cells[24].Attributes.Add("class", "textmode");
				}
				gv.RenderControl(htw);
				string style = @"<style>.textmode{mso-number-format:\@;}.textmode1{mso-number-format:'0';}</style>";
				Response.Write(style);
				Response.Output.Write(sw.ToString());


				Response.Flush();
				Response.End();

			}
			return null;

		}

		[HttpPost]
		public JsonResult SubmiteToRefund(MISCommonModel model)
		{
			MISCommonModel Obj = new MISCommonModel();
			DataTable dt = new DataTable();
			dt = Obj.saveRefundedData(model);
			int result = Convert.ToInt32(dt.Rows[0][0].ToString());
			return Json(result,JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
        public ActionResult TicketCancellationDetailsResend(string TicketID,string RemarkStatus,string Remark,int IsPartialOrFullCanelation)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SMSandEMAILtemplate Sendobj = new SMSandEMAILtemplate();
            int IsMsgSend = 0;
            string Message = string.Empty;
            try
            {
                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.TicketCancellationDetailsDataResend("FULL_GETUSERDETAILS", IsPartialOrFullCanelation, TicketID, RemarkStatus, Remark, UserID);
                if (DT != null && DT.Rows.Count > 0)
                {
                    try
                    {
                        //Sendobj.SendMailComman("ALL", "OnlineBookingResentAccountDetails",Convert.ToString(DT.Rows[0]["RequestID"]), RemarkStatus, Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), Remark, string.Empty);
                        Message = "message send successfully!!!";
                        IsMsgSend = 1;
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                        Message = "Message Not send. Some Error Occured!!!!!!";
                        IsMsgSend = 0;
                    }
                }
                else
                {
                    Message = "Message Not send. Some Error Occured!!!!!!";
                    IsMsgSend = 0;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                Message = "Message Not send. Some Error Occured!!!!!!";
                IsMsgSend = 0;
            }
            return Json(new { IsMsgSend = IsMsgSend, Message = Message }, JsonRequestBehavior.AllowGet);

        }

		[HttpPost]
		public ActionResult TicketRefundUserDetailsDataResend(string TicketID, string RemarkStatus, string Remark, int IsPartialOrFullCanelation)
		{
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
			SMSandEMAILtemplate Sendobj = new SMSandEMAILtemplate();
			int IsMsgSend = 0;
			string Message = string.Empty;
			try
			{
				string TicketIDs = Regex.Match(TicketID, @"\d+").Value;
				MISCommonModel obj = new MISCommonModel();

				DataTable DT;
				
				DT = obj.TicketRefundUserDetailsDataResend("InsertUserDetailsForRefund", IsPartialOrFullCanelation, TicketIDs, RemarkStatus, Remark, UserID);
				if (DT != null && DT.Rows.Count > 0)
				{
					try
					{
						//Sendobj.SendMailComman("ALL", "OnlineBookingResentAccountDetails",Convert.ToString(DT.Rows[0]["RequestID"]), RemarkStatus, Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), Remark, string.Empty);
						Message = "message send successfully!!!";
						IsMsgSend = 1;
					}
					catch (Exception ex)
					{
						new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
						Message = "Message Not send. Some Error Occured!!!!!!";
						IsMsgSend = 0;
					}
				}
				else
				{
					Message = "Message Not send. Some Error Occured!!!!!!";
					IsMsgSend = 0;
				}

			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
				Message = "Message Not send. Some Error Occured!!!!!!";
				IsMsgSend = 0;
			}
			return Json(new { IsMsgSend = IsMsgSend, Message = Message }, JsonRequestBehavior.AllowGet);

		}


		public ActionResult TicketPartialCancellationDetails()
        {
            Session["TicketCancellationPartailRPT"] = null;

            onloadTicketBooking();
            ViewData["ListTicketPartialCancellation"] = ListTicketTransactionDetails;
            return View();

        }
        [HttpPost]
        public ActionResult TicketPartialCancellationDetails(MISCommonModel MIS)
        {
            Session["TicketCancellationPartailRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.TicketPartialCancellationDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place);
                int count = 1;
                Session["TicketCancellationPartailRPT"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                       new MISTicketTransactionDetails()
                       {
                           Index = count,
                           // TicketID = dr["requestId"],
                           BookingID = Convert.ToString(dr["RequestId"]),
                           DateOfBooking = Convert.ToString(dr["DateofBooking"]),
                           DateOfVisit = Convert.ToString(dr["DateofVisit"]),
                           Mobile = Convert.ToString(dr["Mobile"]),
                           EmailId = Convert.ToString(dr["EmailId"]),
                           PlaceName = Convert.ToString(dr["placeName"]),
                           TotalMembers = Convert.ToString(dr["TotalMembers"]),
                           AmountTobePaid = Convert.ToDecimal(dr["TicketAmount"]),
                           EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                           //AmountTobePaid = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dr["AmountTobePaid"])),
                           RefundAmount = Convert.ToDecimal(dr["RefundAmount"]),
                           ResendStatus = Convert.ToString(dr["ResendStatus"])
                       });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListTicketPartialCancellation"] = ListTicketTransactionDetails;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult TicketPartialCancellationExport()
        {
            DataTable dtf = (DataTable)Session["TicketCancellationPartailRPT"];
            if (dtf != null)
            {

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=TicketCancellation_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }



        public ActionResult HeadWiseDepositDetails()
        {
            //Session["HeadWiseDepositDetailRPT"] = null;

            onloadTicketBooking();
            //ViewData["ListHeadWiseDepositDetail"] = ListTicketTransactionDetails;
            return View(new MISCommonModel { DownloadExcel = "False" });

        }



        [HttpPost]
        public ActionResult HeadWiseDepositDetails(MISCommonModel MIS)
        {
            DateTime startTime = DateTime.Now;
            //Session["HeadWiseDepositDetailRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            MISCommonModel obj = new MISCommonModel();
            DataTable DT;
            string Action = "Detail";
            DT = obj.GetHeadWiseDepositDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, Action, MIS.ModeOfBooking, MIS.DATETYPE);
            if (MIS.DownloadExcel == "False")
            {
                try
                {
                    int count = 1;
                    //Session["HeadWiseDepositDetailRPT"] = DT;
                    foreach (DataRow dr in DT.Rows)
                    {
                        ListTicketTransactionDetails.Add(
                            new MISTicketTransactionDetails()
                            {
                                Index = count,
                                RequestID = Convert.ToString(dr["RequestID"]),
                                DateOfBooking = Convert.ToString(dr["DateOfBooking"]),

                                DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                                ZoneName = Convert.ToString(dr["ZoneName"]),
                                ShiftType = Convert.ToString(dr["ShiftType"]),
                                Name = Convert.ToString(dr["VehicalName"]),
                                NonIndian = Convert.ToString(dr["NonIndian"]),
                                Indian = Convert.ToString(dr["Indian"]),
                                TotalMembers = Convert.ToString(dr["TotalMembers"]),
                                CameraForIndian = Convert.ToString(dr["CameraForIndian"]),
                                CameraForNonIndian = Convert.ToString(dr["CameraForNonIndian"]),
                                IncomeFromTourismIndianMemberEntryFee = Convert.ToString(dr["IncomeFromTourismIndianMemberEntryFee"]),
                                IncomeFromTourismNonIndianMemberEntryFee = Convert.ToString(dr["IncomeFromTourismNonIndianMemberEntryFee"]),
                                IncomeFromTourismGypsyEntryFee = Convert.ToString(dr["IncomeFromTourismGypsyEntryFee"]),
                                IncomeFromTourismCanterEntryFee = Convert.ToString(dr["IncomeFromTourismCanterEntryFee"]),

                                IncomeFromTourismIndianCameraEntryFee = Convert.ToString(dr["IncomeFromTourismIndianCameraEntryFee"]),
                                IncomeFromTourismNonIndianCameraEntryFee = Convert.ToString(dr["IncomeFromTourismNonIndianCameraEntryFee"]),

                                IncomeFromTourismIndianHDFDCharge= Convert.ToString(dr["IncomeFromTourismIndianHDFDCharge"]),
                                IncomeFromTourismNonIndianHDFDCharge= Convert.ToString(dr["IncomeFromTourismNonIndianHDFDCharge"]),

                                TOTALIncomeFromTourism = Convert.ToString(dr["TOTALIncomeFromTourism"]),
                                IncomeFromECODEVIndianMemberEntryFee = Convert.ToString(dr["IncomeFromECODEVIndianMemberEntryFee"]),

                                IncomeFromECODEVNonIndianMemberEntryFee = Convert.ToString(dr["IncomeFromECODEVNonIndianMemberEntryFee"]),
                                IncomeFromECODEVGypsyEntryFee = Convert.ToString(dr["IncomeFromECODEVGypsyEntryFee"]),
                                IncomeFromECODEVCanterEntryFee = Convert.ToString(dr["IncomeFromECODEVCanterEntryFee"]),
                                IncomeFromECODEVIndianCameraEntryFee = Convert.ToString(dr["IncomeFromECODEVIndianCameraEntryFee"]),
                                IncomeFromECODEVNonIndianCameraEntryFee = Convert.ToString(dr["IncomeFromECODEVNonIndianCameraEntryFee"]),

                                IncomeFromEcoDevIndianHDFdCharge= Convert.ToString(dr["IncomeFromEcoDevIndianHDFdCharge"]),
                                IncomeFromEcoDevNonIndianHDFdCharge= Convert.ToString(dr["IncomeFromEcoDevNonIndianHDFdCharge"]),

                                TOTALIncomeFromECODEV = Convert.ToString(dr["TOTALIncomeFromECODEV"]),
                                FoundationIndianMemberEntryFee = Convert.ToString(dr["FoundationIndianMemberEntryFee"]),
                                FoundationNonIndianMemberEntryFee = Convert.ToString(dr["FoundationNonIndianMemberEntryFee"]),
                                FoundationForVehicleEntryFee = Convert.ToString(dr["FoundationForVehicleEntryFee"]),
                                FoundationForGuidFee = Convert.ToString(dr["FoundationForGuidFee"]),
                                TOTALFoundation = Convert.ToString(dr["TOTALFoundation"]),


                                VehicleRentFees = Convert.ToString(dr["VehicleRentFees"]),
                                VehicleRentFeesGSTPercentage = Convert.ToString(dr["VehicleRentFeesGSTPercentage"]),
                                VehicleRentFeesGSTAmount = Convert.ToString(dr["VehicleRentFeesGSTAmount"]),
                                TOTALVehicleRentFees = Convert.ToString(dr["TOTALVehicleRentFees"]),

                                GuideFees = Convert.ToString(dr["GuideFees"]),
                                GuideFeesGSTPercentage = Convert.ToString(dr["GuideFeesGSTPercentage"]),
                                GuideFeesGSTAmount = Convert.ToString(dr["GuideFeesGSTAmount"]),
                                TOTALGuideFees = Convert.ToString(dr["TOTALGuideFees"]),

                                // AmountTobePaid = Convert.ToDecimal(dr["AmountTobePaid"]),  don't need to show on UI
                                TotalFeeHeadwise = Convert.ToString(dr["TotalFeeHeadwise"]),



                                EmitraCharges = Convert.ToString(dr["@2.25% on total FEE"]),
                                TaxOnEmitraCharges = Convert.ToString(dr["ST on 2.25% @ 15%"]),
                                EMitraTotalCharges = Convert.ToString(dr["E-Mitra Total Charges"]),

                                // AmountWithServiceCharges = Convert.ToString(dr["AmountWithServiceCharges"]),don't need to show on UI
                                TotalPayment = Convert.ToString(dr["TotalPayment"]),
                                //  AMOUNT_STATUS = Convert.ToString(dr["TotalPayment"]),don't need to show on UI
                                // AMOUNT_DIFFERENCE = Convert.ToString(dr["AMOUNT_DIFFERENCE"]), don't need to show on UI
                                ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),
                            });
                        count += 1;

                    }

                    string SSOID = Session["SSOid"].ToString();
                    UserLogExceptionData();

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }

                DateTime endTime = DateTime.Now;

                MIS.TimeDifference = (endTime - startTime).TotalSeconds.ToString();

                return PartialView("_HeadWiseDetailReport", ListTicketTransactionDetails);

            }
            else
            {
                DT.Columns.Remove("AMOUNT_STATUS");
                DT.Columns.Remove("AMOUNT_DIFFERENCE");
                DT.Columns.Remove("AmountWithServiceCharges");
                DT.Columns.Remove("AmountTobePaid");

                DownloadExcel(DT);
                return null;
            }
        }

        /// <summary>
        /// No use of this action. 
        /// </summary>
        /// <returns></returns>
        public ActionResult HeadWiseDepositDetailsExport()
        {
            DataTable dtf = (DataTable)Session["HeadWiseDepositDetailRPT"];
            if (dtf != null)
            {
                dtf.Columns.Remove("AMOUNT_STATUS");
                dtf.Columns.Remove("AMOUNT_DIFFERENCE");
                dtf.Columns.Remove("AmountWithServiceCharges");
                dtf.Columns.Remove("AmountTobePaid");

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=HeadWiseDepositDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }
            return null;

        }

        public ActionResult HeadWiseDepositSummary()
        {
            //Session["HeadWiseDepositSummaryRPT"] = null;

            onloadTicketBooking();
            ViewData["ListHeadWiseDepositSummary"] = ListTicketTransactionDetails;
            return View(new MISCommonModel { DownloadExcel = "False" });

        }
        [HttpPost]
        public ActionResult HeadWiseDepositSummary(MISCommonModel MIS)
        {
            DateTime startTime = DateTime.Now;
            // Session["HeadWiseDepositSummaryRPT"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            MISCommonModel obj = new MISCommonModel();
            DataTable DT;
            string Action = "Summary";
            DT = obj.GetHeadWiseDepositDetailsData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, Action, MIS.ModeOfBooking,MIS.DATETYPE);
            if (MIS.DownloadExcel == "False")
            {
                int count = 1;
                try
                {

                    //Session["HeadWiseDepositSummaryRPT"] = DT;

                    foreach (DataRow dr in DT.Rows)
                    {
                        ListTicketTransactionDetails.Add(
                            new MISTicketTransactionDetails()
                            {
                                Index = count,
                                Heads = Convert.ToString(dr["Heads"]),
                                Fees = Convert.ToString(dr["Fees"]),
                            });
                        count += 1;
                    }


                    string SSOID = Session["SSOid"].ToString();
                    UserLogExceptionData();
                    ViewData["ListHeadWiseDepositSummary"] = ListTicketTransactionDetails;

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                DateTime endTime = DateTime.Now;

                MIS.TimeDifference = (endTime - startTime).TotalSeconds.ToString();
                return View(MIS);
            }
            else
            {
                DownloadExcel(DT);
                return null;
            }
        }

        private void DownloadExcel(DataTable DT)
        {
            GridView gv = new GridView();
            gv.DataSource = DT;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=HeadWiseDepositSummary_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public ActionResult HeadWiseDepositSummaryExport()
        {
            DataTable dtf = (DataTable)Session["HeadWiseDepositSummaryRPT"];
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=HeadWiseDepositSummary_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        public ActionResult TicketBookingSummaryRequestIDWise()
        {
            Session["TicketBookingSummaryRequestIDWiseDownload"] = null;

            onloadTicketBooking();
            ViewData["ListTicketBookingSummaryRequestIDWise"] = ListTicketTransactionDetails;

            return View();

        }
        [HttpPost]
        public ActionResult TicketBookingSummaryRequestIDWise(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.GetTicketBookingSummaryRequestIDWise(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE, MIS.BOOKING_TYPE, MIS.DATETYPE, MIS.TRNS_Status, MIS.ModeOfBooking);
                int count = 1;

                Session["TicketBookingSummaryRequestIDWiseDownload"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),

                            AmountTobePaid = Convert.ToDecimal(dr["AmountWithServiceCharges"]),
                            TicketID = Convert.ToInt64(dr["TicketID"]),
                            TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                            EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]),
                            Manual_Status = Convert.ToString(dr["Manual_Status"]),
                            Manual_Remarks = Convert.ToString(dr["Manual_Remarks"]),
                            ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),
                        });
                    count += 1;
                }





                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListTicketBookingSummaryRequestIDWise"] = ListTicketTransactionDetails;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult TicketBookingSummaryRequestIDWiseExport()
        {
            
            DataTable dtf = (DataTable)Session["TicketBookingSummaryRequestIDWiseDownload"];
            if (dtf != null)
            {
                if (dtf.Columns.Contains("Trn_Status_Code"))
                {
                    dtf.Columns.Remove("Trn_Status_Code");
                }

                if (dtf != null)
                {
                    GridView gv = new GridView();
                    gv.DataSource = dtf;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=TicketBookingSummaryRequestIDWise_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            

            return null;
        }

        [HttpGet]
        public FileResult PrintWildLifeTicketForOffice(string ticketid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                BookOnTicket cs = new BookOnTicket();
                cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));

                ds = cs.Select_TicketData_For_Print();

                DataTable dtTC = new DataTable();
                dtTC = cs.Select_TermandConditionbyTicketId(Convert.ToInt64(Encryption.decrypt(ticketid)));

                string filepath = htmlToPdfDownloadTickets.WildlifeDownloadPdfForOfficeUse(ds, dtTC);

                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpGet]
        public FileResult PrintBoardingPassForOffice(string id)
        {
            //if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            //{
            //    Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            //}


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();


            DataTable DT_DptUser;
            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);


                DataTable DT;
                DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 18));
                //DT = BoardingDetails.BoardingPassForOne("636094514727149772");


                string filepath = htmlToPdfDownloadTickets.WildlifeBoardingPassDownloadPdfForOffice(DT, DT_DptUser);

                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public ActionResult CurrentBookingDayCloserDetails()
        {
            Session["CurrentBookingDayCloserDetailsDownload"] = null;


            onloadTicketBooking();
            ViewData["ListCurrentBookingDayCloserDetails"] = ListTicketTransactionDetails;

            return View();

        }
        [HttpPost]
        public ActionResult CurrentBookingDayCloserDetails(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.GetCurrentBookingDayColserDetails("CURRENT_COUNTER_BOOKING_DETAILS", MIS.FromDate, MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, "", "");
                int count = 1;

                Session["CurrentBookingDayCloserDetailsDownload"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            Ssoid = Convert.ToString(dr["Ssoid"].ToString()),
                            EmitraTransactionID = Convert.ToString(dr["IP_ADDRESS"].ToString()),

                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),

                            NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerMembers"]),
                            NoOfIndianMembers = Convert.ToString(dr["NoOfIndianMembers"]),
                            TotalMembers = Convert.ToString(dr["TotalMembers"]),
                            TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),

                            TOTALIncomeFromTourism = Convert.ToString(dr["TOTALIncomeFromTourism"]),
                            TOTALIncomeFromECODEV = Convert.ToString(dr["TOTALIncomeFromECODEV"]),
                            TOTALFoundation = Convert.ToString(dr["TOTALFoundation"]),

                            TotalFeeHeadwise = Convert.ToString(dr["TotalFeeHeadwise"]),

                        });
                    count += 1;
                }





                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListCurrentBookingDayCloserDetails"] = ListTicketTransactionDetails;






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult CurrentBookingDayCloserDetailsExport()
        {
            DataTable dtf = (DataTable)Session["CurrentBookingDayCloserDetailsDownload"];

            dtf.Columns.Remove("Trn_Status_Code");

            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CurrentBookingDayColserDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        List<SelectListItem> lstSSOID = new List<SelectListItem>();
        List<SelectListItem> lstIPAddress = new List<SelectListItem>();
        public ActionResult SSOandIPWISECurrentBookingDayCloserDetails()
        {
            Session["SSOandIPWISECurrentBookingDayCloserDetailsDownload"] = null;


            onloadTicketBooking();
            ViewData["ListSSOandIPWISECurrentBookingDayCloserDetails"] = ListTicketTransactionDetails;


            lstSSOID.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            lstIPAddress.Add(new SelectListItem { Text = "ALL", Value = "ALL" });

            ViewBag.ddlSSOID1 = lstSSOID;
            ViewBag.ddlIPAddress1 = lstIPAddress;

            return View();


        }

        public JsonResult GETALLSSOIDsByPlaceDateShiftTime(string DateOfBooking, string PLACE, string ShiftTime)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            MISCommonModel obj = new MISCommonModel();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                DataTable dt = obj.GETALLSSOIDsByPlaceDateShift(DateOfBooking, PLACE, ShiftTime);

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = dr["SSOID"].ToString(), Value = dr["SSOID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public JsonResult GETALLIPsBySSOID(string DateOfBooking, string PLACE, string ShiftTime, string SSOID)
        {
            SessionForJson();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            MISCommonModel obj = new MISCommonModel();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                DataTable dt = obj.GETALLIPsBySSOID(DateOfBooking, PLACE, ShiftTime, SSOID);

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    items.Add(new SelectListItem { Text = dr["IP_ADDRESS"].ToString(), Value = dr["IP_ADDRESS"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }
        [HttpPost]
        public ActionResult SSOandIPWISECurrentBookingDayCloserDetails(MISCommonModel MIS)
        {




            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                MISCommonModel obj = new MISCommonModel();
                DataTable DT;
                DT = obj.GetCurrentBookingDayColserDetails("CURRENT_COUNTER_BOOKING_SSO_IPADDRESS_WISE_DETAILS", MIS.FromDate, MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, MIS.SSOID, MIS.IP_ADDRESS);
                int count = 1;

                Session["SSOandIPWISECurrentBookingDayCloserDetailsDownload"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListTicketTransactionDetails.Add(
                        new MISTicketTransactionDetails()
                        {

                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            Ssoid = Convert.ToString(dr["Ssoid"].ToString()),
                            EmitraTransactionID = Convert.ToString(dr["IP_ADDRESS"].ToString()),

                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),

                            NoOfForeignerMembers = Convert.ToString(dr["NoOfForeignerMembers"]),
                            NoOfIndianMembers = Convert.ToString(dr["NoOfIndianMembers"]),
                            TotalMembers = Convert.ToString(dr["TotalMembers"]),
                            TotalNoOfCamera = Convert.ToString(dr["TotalNoOfCamera"]),

                            TOTALIncomeFromTourism = Convert.ToString(dr["TOTALIncomeFromTourism"]),
                            TOTALIncomeFromECODEV = Convert.ToString(dr["TOTALIncomeFromECODEV"]),
                            TOTALFoundation = Convert.ToString(dr["TOTALFoundation"]),
                            TotalFeeHeadwise = Convert.ToString(dr["TotalFeeHeadwise"]),

                        });
                    count += 1;
                }





                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListSSOandIPWISECurrentBookingDayCloserDetails"] = ListTicketTransactionDetails;


                DataTable dt = obj.GETALLSSOIDsByPlaceDateShift(MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE);
                lstSSOID.Add(new SelectListItem { Text = "ALL", Value = "ALL" });



                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstSSOID.Add(new SelectListItem { Text = dr["SSOID"].ToString(), Value = dr["SSOID"].ToString() });
                }

                dt = obj.GETALLIPsBySSOID(MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, MIS.SSOID);
                lstIPAddress.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lstIPAddress.Add(new SelectListItem { Text = dr["IP_ADDRESS"].ToString(), Value = dr["IP_ADDRESS"].ToString() });
                }

                ViewBag.ddlSSOID1 = lstSSOID;
                ViewBag.ddlIPAddress1 = lstIPAddress;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult SSOandIPWISECurrentBookingDayCloserDetailsExport()
        {
            DataTable dtf = (DataTable)Session["SSOandIPWISECurrentBookingDayCloserDetailsDownload"];

            dtf.Columns.Remove("Trn_Status_Code");

            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SSOandIPWISECurrentBookingDayColserDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        public ActionResult BoardingIssueStatus()
        {

            onloadTicketBooking();
            ViewData["ListBoardingIssueStatus"] = ListBoarding;

            return View();

        }
        [HttpPost]
        public ActionResult BoardingIssueStatus(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                MISCommonModel obj = new MISCommonModel();


                DataTable DT;
                DT = obj.GetBoardingIssueStatusData(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.SHIFT_TYPE);
                int count = 1;

                Session["DownloadBoardingIssueStatusExport"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            DisplayBookingId = Convert.ToString(dr["BookingId"].ToString()),


                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                            NameOfVisitor = Convert.ToString(dr["Name"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                            Nationality = Convert.ToString(dr["Nationality"]),
                            ContactNoDeptUser = Convert.ToString(dr["BoardingIssueStatus"]),
                            Trn_Status_Code = Convert.ToString(dr["ArrivalStatus"]), // ArrivelStatus                
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),

                            ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),

                        });
                    count += 1;
                }

                string SSOID = Session["SSOid"].ToString();
                UserLogExceptionData();
                ViewData["ListBoardingIssueStatus"] = ListBoarding;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult BoardingIssueStatusExport()
        {
            DataTable dtf = (DataTable)Session["DownloadBoardingIssueStatusExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=BoardingIssueStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        public ActionResult ViewTicket(string id)
        {
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;  //Convert.ToInt64(Session["UserID"].ToString());
            ViewTicketDT1 Ticket = new ViewTicketDT1();
            try
            {
                DataSet ds = new DataSet();
                CS_Ticket cs = new CS_Ticket();
                cs.TicketID = Convert.ToInt64(id);
                ds = cs.Select_TicketData_For_Print();
                //sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><div class='col-xs-12 col-sm-4 centr'><img src='../images/risl-logo-small.png' alt='RISL' >   </div>		<div class='col-xs-12 col-sm-4'> <span class='pull-right pdate' align= 'center'>Department of Forest, <br>Goverment of<br> Rajasthan</span> </div><img src='../images/e-mitra_logo.png' alt='E-Mitra' >  </div>  <div class='divider'></div></div>");
                sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><img src='../../images/risl-logo-small.png' alt='RISL' ></div><div class='col-xs-12 col-sm-4 centr'>Department of Forest, <br>Government of<br> Rajasthan</span></div><div class='col-xs-12 col-sm-4' style='padding: 0'> <span class='pull-right pdate'><img src='../../images/e-mitra_logo.png' alt='E-Mitra' > </div>  <div class='divider'></div></div>");

                if (ds != null)
                {
                    Ticket.PlaceName = Convert.ToString(ds.Tables[0].Rows[0]["PlaceName"]);
                    Ticket.EnteredOn = Convert.ToString(ds.Tables[0].Rows[0]["EnteredOn"]);
                    Ticket.DateOfArrival = Convert.ToString(ds.Tables[0].Rows[0]["DateOfArrival"]);
                    Ticket.NoofTicket = Convert.ToString(ds.Tables[0].Rows[0]["NoofTicket"]);
                    Ticket.finalAmnt = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountTobePaid"]));
                    Ticket.RequestID = Convert.ToString(ds.Tables[0].Rows[0]["RequestID"]);
                    Ticket.Boarding_Point = Convert.ToString(ds.Tables[0].Rows[0]["Boarding_Point"]);
                    Ticket.contactperson = Convert.ToString(ds.Tables[0].Rows[0]["contactperson"]);
                    Ticket.PhoneNo = Convert.ToString(ds.Tables[0].Rows[0]["PhoneNo"]);
                    Ticket.Address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]);

                    List<ViewTicketDT2> ViewTicketD2 = new List<ViewTicketDT2>();
                    int count = 1;
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ViewTicketD2.Add(
                            new ViewTicketDT2()
                            {
                                index = count,
                                Name = Convert.ToString(dr["Name"]),
                                Nationality = Convert.ToString(dr["Nationality"]),
                                IDProof = Convert.ToString(dr["IDProof"]),
                                NoOfCamera = Convert.ToString(dr["NoOfCamera"]),
                                Shift = Convert.ToString(dr["Shift"]),
                                VName = Convert.ToString(dr["VName"]),
                            });
                    }

                    List<ViewTicketDT3> ViewTicketD3 = new List<ViewTicketDT3>();

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ViewTicketD3.Add(
                            new ViewTicketDT3()
                            {
                                Period = Convert.ToString(dr["Period"]),
                                MorningTrip = Convert.ToString(dr["MorningTrip"]),
                                AfterNoonTrip = Convert.ToString(dr["AfterNoonTrip"]),
                            });
                    }
                    Ticket.ViewTicket2 = ViewTicketD2;
                    Ticket.ViewTicket3 = ViewTicketD3;

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }




            return View(Ticket);
        }


        List<SocialAuditorVehiclewiseReport> SocialAuditorVehiclewise = new List<SocialAuditorVehiclewiseReport>();
        public ActionResult SocialAuditorDataVehiclewise()
        {
            SocialAuditorVehiclewiseReport obj = new SocialAuditorVehiclewiseReport();

            DataTable DT;

            DT = obj.GetPlace();

            obj.ListPlace = new SelectList(DT.AsDataView(), "PlaceID", "PlaceName").ToList();


            ViewData["ListSocialAuditorVehiclewise"] = SocialAuditorVehiclewise;
            return View(obj);

        }

        [HttpPost]
        public ActionResult SocialAuditorDataVehiclewise(SocialAuditorVehiclewiseReport obj)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {

                DataTable DT1;
                DT1 = obj.GetPlace();

                obj.ListPlace = new SelectList(DT1.AsDataView(), "PlaceID", "PlaceName").ToList();


                DataTable DT;

                DT = obj.GetListData(obj);

                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    SocialAuditorVehiclewise.Add(
                        new SocialAuditorVehiclewiseReport()
                        {
                            index = count,
                            ZoneName = Convert.ToString(dr["ZoneName"].ToString()),

                            MGypsy = Convert.ToString(dr["Morning-Gypsy"]),
                            MCanter = Convert.ToString(dr["Morning-Canter"]),
                            EGypsy = Convert.ToString(dr["Evening-Gypsy"]),
                            ECanter = Convert.ToString(dr["Evening-Canter"]),

                        });
                    count += 1;
                }

                ViewData["ListSocialAuditorVehiclewise"] = SocialAuditorVehiclewise;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(obj);

        }

        #region Check Release Ticket in Online Booking Developed by Rajveer

        public ActionResult OnlineBookingReleaseTicketSchedular()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<OnlineBookingReleaseTicketSchedularModel> model = new List<OnlineBookingReleaseTicketSchedularModel>();
            try
            {
                OnlineBookingReleaseTicketSchedularRepo repo = new OnlineBookingReleaseTicketSchedularRepo();
                DataTable dt = new DataTable();
                dt = repo.GetListData(UserID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region DeSerialized Datatable to model
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineBookingReleaseTicketSchedularModel>>(str);
                    #endregion
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(model);
        }



        #endregion

        #region Create Online Ticket Booking Details Report by Rajveer
        public ActionResult TicketBookingSSO()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            OnlineTicketBookingSSOListModel model = new OnlineTicketBookingSSOListModel();
            model.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            model.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            try
            {

                OnlineTicketBoolingSSORepo obj = new OnlineTicketBoolingSSORepo();
                onloadTicketBooking();

                DataTable DT = new DataTable();
                DT = obj.GetTicketBookingBySSO(model, "LIST");
                if (DT != null)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineTicketBookingSSOModel>>(str);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult TicketBookingSSO(OnlineTicketBookingSSOListModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            try
            {
                OnlineTicketBoolingSSORepo obj = new OnlineTicketBoolingSSORepo();
                DataTable DT = new DataTable();
                DT = obj.GetTicketBookingBySSO(model, "LIST");
                if (DT != null)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineTicketBookingSSOModel>>(str);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(model);

        }


        public ActionResult TicketBookingRefundDetails(string RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            OnlineTicketBookingSSOListModel model = new OnlineTicketBookingSSOListModel();
            model.RequestID = RequestId;
            try
            {
                OnlineTicketBoolingSSORepo obj = new OnlineTicketBoolingSSORepo();
                DataTable DT = new DataTable();
                DT = obj.GetTicketBookingBySSO(model, "Refund");
                if (DT != null)
                {
                    model.ReconsilationResponse = Newtonsoft.Json.JsonConvert.SerializeObject(DT);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineTicketBookingSSOModel>>(model.ReconsilationResponse);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TicketBookingSSODetails(string RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            OnlineTicketBookingSSOListModel model = new OnlineTicketBookingSSOListModel();
            model.RequestID = RequestId;
            try
            {
                OnlineTicketBoolingSSORepo obj = new OnlineTicketBoolingSSORepo();
                DataTable DT = new DataTable();
                DT = obj.GetTicketBookingBySSO(model, "DETAILS");
                if (DT != null)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineTicketBookingSSOModel>>(str);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Get Online Booking Information
        [HttpGet]
        public ActionResult OnlineInformationBooking()
        {
            onloadTicketBooking();
            return View();
        }
        [HttpPost]
        public ActionResult OnlineInformationBooking(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                DataTable DT;
                StringBuilder SB = new StringBuilder();
                MISCommonModel obj = new MISCommonModel();
                onloadTicketBooking();
                DT = obj.OnlineInformationBooking(MIS.FromDate, MIS.ToDate, Convert.ToInt32(MIS.Place), "LIST");
                int count = 1;
                if (DT.Rows.Count > 0)
                {
                    SB.Append("<table class='table table-striped table-bordered table-hover table-responsive gridtable' id='tbl'><thead><tr>");
                    SB.Append("<th>#</th>");
                    foreach (DataColumn DC in DT.Columns)
                    {
                        SB.Append("<th>" + DC.ColumnName + "</th>");
                    }
                    SB.Append("</tr></thead><tbody>");
                    foreach (DataRow DR in DT.Rows)
                    {
                        SB.Append("<tr>");
                        SB.Append("<th>" + count + "</th>");
                        foreach (DataColumn column in DT.Columns)
                        {
                            SB.Append("<td>" + Convert.ToString(DR[column] == null ? "" : DR[column]) + "</td>");
                        }
                        count = count + 1;
                        SB.Append("</tr>");
                    }
                    SB.Append("</tbody> </table> ");
                    ViewBag.ListData = SB.ToString();
                }
                else
                {
                    ViewBag.ListData = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }
        #endregion

        #region Get Guide name with zone wise in wildlife booking
        public ActionResult CurrentBookingZoneDetails()
        {
            Session["CurrentBookingZoneDetailsDownload"] = null;
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();

            onloadTicketBooking();

            TempData["ListCurrentBookingZoneDetails"] = List;

            return View();

        }
        [HttpPost]
        public ActionResult CurrentBookingZoneDetails(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();
            try
            {

                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.GetCurrentBookingZoneDetails("GETZONEWISEGUIDENAME", MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, UserID);
                int count = 1;

                Session["CurrentBookingZoneDetailsDownload"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    List.Add(
                        new ViewTicketRemaningDT1()
                        {
                            Index = count,
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            ShiftTime = Convert.ToString(dr["ShiftTime"]),
                            GuideName = Convert.ToString(dr["GuidName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                            NoOfMember = Convert.ToInt64(dr["TotalMembers"]),
                            RequestID = Convert.ToString(dr["RequestID"]),

                        });
                    count += 1;
                }

                TempData["ListCurrentBookingZoneDetails"] = List;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult CurrentBookingZoneDetailsExport()
        {
            DataTable dtf = (DataTable)Session["CurrentBookingZoneDetailsDownload"];


            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CurrentBookingDayColserDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }
        #endregion

        #region GetRemaing Seats in wildlife booking
        public ActionResult CurrentBookingRemainingDetails()
        {
            Session["CurrentBookingRemainingDetailsDownload"] = null;
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();

            onloadTicketBooking();

            TempData["ListCurrentBookingRemainingDetails"] = List;

            return View();

        }
        [HttpPost]
        public ActionResult CurrentBookingRemainingDetails(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadTicketBooking();
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();
            try
            {

                MISCommonModel obj = new MISCommonModel();

                DataTable DT;
                DT = obj.GetCurrentBookingRemainingSeatsDetails("GETREMAININGSEAT", MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, UserID);
                int count = 1;

                Session["CurrentBookingRemainingDetailsDownload"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    List.Add(
                        new ViewTicketRemaningDT1()
                        {
                            Index = count,
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            seatsForCitizen = Convert.ToInt64(dr["SeatForOnline"]),
                            CurrentBookingSeat = Convert.ToInt64(dr["SeatForCurrent"]),
                            TotalSeats = Convert.ToInt64(dr["TotalSeats"]),
                            CitizenbookedSeats = Convert.ToInt64(dr["OnlineBooked"]),
                            CurrentbookedSeats = Convert.ToInt64(dr["CurrentBooked"]),
                            TotalBookedSeat = Convert.ToInt64(dr["TotalBookedSeat"]),
                            CitizenRemainingSeat = Convert.ToInt64(dr["OnlineRemaining"]),
                            CurrentRemainingSeat = Convert.ToInt64(dr["CurrentRemaining"]),
                            RemainingSeat = Convert.ToInt64(dr["RemainingSeat"])
                        });
                    count += 1;
                }

                TempData["ListCurrentBookingRemainingDetails"] = List;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }

        public ActionResult CurrentBookingRemainingDetailsExport()
        {
            DataTable dtf = (DataTable)Session["CurrentBookingRemainingDetailsDownload"];


            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CurrentBookingDayColserDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }
        #endregion

        List<MISCommonModel> ListOnlineVehicleReport = new List<MISCommonModel>();
        #region add by sunny vehicleWiseReport
        public ActionResult OnlineVechileWiseReport()
        {
            List<SelectListItem> ShiftTypeForReport = new List<SelectListItem>();
            ShiftTypeForReport.Add(new SelectListItem { Text = "ALL", Value = "0" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Evening", Value = "2" });
            ViewBag.ddlShiftTypeForReport = ShiftTypeForReport;
            onloadTicketBooking();

            ViewData["ListBoarding"] = ListOnlineVehicleReport;
            return View();

        }

        [HttpPost]
        public ActionResult OnlineVechileWiseReport(MISCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> ShiftTypeForReport = new List<SelectListItem>();
            ShiftTypeForReport.Add(new SelectListItem { Text = "ALL", Value = "0" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftTypeForReport.Add(new SelectListItem { Text = "Evening", Value = "2" });
            ViewBag.ddlShiftTypeForReport = ShiftTypeForReport;
            onloadTicketBooking();

            try
            {
                MISCommonModel obj = new MISCommonModel();
                DataTable DT;
                DT = obj.OnlineVehicleWiseReport(MIS.FromDate, MIS.ToDate, MIS.PlaceID, MIS.Zone, MIS.ShiftID, MIS.VehicleType, MIS.IndianForeigner);
                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ListOnlineVehicleReport.Add(
                        new MISCommonModel()
                        {
                            Index = count,
                            DateOfArrival = Convert.ToString(dr["DateOfArrival"].ToString()),
                            PLACE_NAME = Convert.ToString(dr["PlaceName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            SHIFTName = Convert.ToString(dr["ShiftName"]),
                            VehicleName = Convert.ToString(dr["VehicleName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                            IndianCount = Convert.ToString(dr["IndianCount"]),
                            NonIndianCount = Convert.ToString(dr["NonIndianCount"]),
                            IndianCameraCount = Convert.ToString(dr["IndianCameraCount"]),
                            NonIndianCameraCount = Convert.ToString(dr["NonIndianCameraCount"]),

                        });
                    count += 1;
                }
                string SSOID = Session["SSOid"].ToString();
                ViewData["ListBoarding"] = ListOnlineVehicleReport;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MIS);

        }
        public JsonResult getZone(long PlaceId)
        {
            Session["CurrentBookingOrAdvanceBooking"] = "1";
            List<SelectListItem> lstZone = new List<SelectListItem>();
            DataSet dsSafariAccomodation = new DataSet();
            BookOnTicket bot = new BookOnTicket();
            bot.PlaceId = Convert.ToInt64(PlaceId);
            dsSafariAccomodation = bot.chkSafariAccomo();
            if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
            {
                foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                }
            }
            return Json(lstZone, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HalfDayFullDayDetails()
        {
            MISCommonModel misObject = new MISCommonModel();
            misObject.ToDate = Convert.ToString(DateTime.Now.ToShortDateString());
            misObject.FromDate = Convert.ToString(DateTime.Now.ToShortDateString());
            onloadTicketBooking();
            ViewData["ListHalfDayFullDayDetails"] = ListHalfDayFullDayDetails;
            return View(misObject);
        }
        /// <summary>
        /// To Fetch HalfDay FullDay day Details Form Database
        /// </summary>
        /// <param name="misCommonModel">Mis Common Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HalfDayFullDayDetails(MISCommonModel misCommonModel)
        {
            onloadTicketBooking();
            DataTable dt = misCommonModel.HalfDayFullDayDetail(misCommonModel.DATETYPE, misCommonModel.FromDate, misCommonModel.ToDate);
            Session["TicketBookingSummaryRequestIDWiseDownload"] = null;
            Session["TicketBookingSummaryRequestIDWiseDownload"] = dt;
            foreach (DataRow dataRow in dt.Rows)
            {
                ListHalfDayFullDayDetails.Add(new HalfDayFullDayDetailModel
                {
                    SerialNumber = Convert.ToString(dataRow["SerialNumber"]),
                    AmountTobePaid = Convert.ToString(dataRow["AmountTobePaid"]),
                    FirstEnterOn = Convert.ToString(dataRow["FirstEnterOn"]),
                    FirstRefPlaceName = Convert.ToString(dataRow["FirstRefPlaceName"]),
                    FirstRefRequestID = Convert.ToString(dataRow["FirstRefRequestID"]),
                    FullDayFullDayVisitDate = Convert.ToString(dataRow["FullDayFullDayVisitDate"]),
                    HalfDayFullDayBookingPlace = Convert.ToString(dataRow["HalfDayFullDayBookingPlace"]),
                    HalfDayFullDayEnterOn = Convert.ToString(dataRow["HalfDayFullDayEnterOn"]),
                    HalfDayFullDayRequestID = Convert.ToString(dataRow["HalfDayFullDayRequestID"]),
                    OldIdProof = Convert.ToString(dataRow["OldIdProof"]),
                    RefVisitDate = Convert.ToString(dataRow["RefVisitDate"]),
                    RefVisitDate2 = Convert.ToString(dataRow["RefVisitDate2"]),
                    SecEnterOn = Convert.ToString(dataRow["SecEnterOn"]),
                    SecoudReqID = Convert.ToString(dataRow["SecoudReqID"]),
                    SecoundRefPlaceName = Convert.ToString(dataRow["SecoundRefPlaceName"]),
                    ShiftType = Convert.ToString(dataRow["ShiftType"]),
                    Ssoid = Convert.ToString(dataRow["Ssoid"])
                });
            }

            ViewData["ListHalfDayFullDayDetails"] = ListHalfDayFullDayDetails;

            return View();
        }



        [HttpGet]
        public ActionResult EventLog()
        {
            EventLogDetailModel misObject = new EventLogDetailModel();
            misObject.ToDate = Convert.ToString(DateTime.Now.ToShortDateString());
            misObject.FromDate = Convert.ToString(DateTime.Now.ToShortDateString());
            List<SelectListItem> seven = new List<SelectListItem> {
                                                                    new SelectListItem { Text= "--ALL--", Value= "0" },
                                                                    new SelectListItem { Text= "CREATE_PROCEDURE", Value= "CREATE_PROCEDURE" },
                                                                    new SelectListItem { Text = "ALTER_PROCEDURE", Value = "ALTER_PROCEDURE" },
                                                                    new SelectListItem { Text = "DROP_PROCEDURE", Value = "DROP_PROCEDURE" },
                                                                    new SelectListItem { Text = "CREATE_TABLE", Value = "CREATE_TABLE" } ,
                                                                    new SelectListItem { Text = "ALTER_TABLE", Value = "ALTER_TABLE" } ,
                                                                    new SelectListItem { Text = "DROP_TABLE", Value = "DROP_TABLE" } ,
                                                                    new SelectListItem { Text = "CREATE_FUNCTION", Value = "CREATE_FUNCTION" } ,
                                                                    new SelectListItem { Text = "ALTER_FUNCTION", Value = "ALTER_FUNCTION" } ,
                                                                    new SelectListItem { Text = "DROP_FUNCTION", Value = "DROP_FUNCTION" } ,
                                                                    new SelectListItem { Text = "CREATE_VIEW", Value = "CREATE_VIEW" } ,
                                                                    new SelectListItem { Text = "ALTER_VIEW", Value = "ALTER_VIEW" } ,
                                                                    new SelectListItem { Text = "DROP_VIEW", Value = "DROP_VIEW" } };
                                                                    //new SelectListItem { Text = "CREATE_TRIGGER", Value = "CREATE_TRIGGER" } ,
                                                                    //new SelectListItem { Text = "ALTER_TRIGGER", Value = "ALTER_TRIGGER" } ,
                                                                    //new SelectListItem { Text = "DROP_TRIGGER", Value = "DROP_TRIGGER" } ,

                                                                    //new SelectListItem { Text = "CREATE_XML_SCHEMA_COLLECTION", Value = "CREATE_XML_SCHEMA_COLLECTION" } ,
                                                                    //new SelectListItem { Text = "ALTER_XML_SCHEMA_COLLECTION", Value = "ALTER_XML_SCHEMA_COLLECTION" } ,
                                                                    //new SelectListItem { Text = "DROP_XML_SCHEMA_COLLECTION", Value = "DROP_XML_SCHEMA_COLLECTION" } };

            



            ViewData["eventType"] = seven; 
            ViewData["EventLogDetails"] = EventLogDetails;
            return View(misObject);
            
        }

        [HttpPost]
        public ActionResult EventLog(EventLogDetailModel model)
        {
            EventLogDetails = new List<EventLogDetailModel>();



            EventLogDetailModel misObject = new EventLogDetailModel();
            misObject.ToDate = Convert.ToString(DateTime.Now.ToShortDateString());
            misObject.FromDate = Convert.ToString(DateTime.Now.ToShortDateString());
            List<SelectListItem> seven = new List<SelectListItem> {
                                                                    new SelectListItem { Text= "--ALL--", Value= "0" },
                                                                    new SelectListItem { Text= "CREATE_PROCEDURE", Value= "CREATE_PROCEDURE" },
                                                                    new SelectListItem { Text = "ALTER_PROCEDURE", Value = "ALTER_PROCEDURE" },
                                                                    new SelectListItem { Text = "DROP_PROCEDURE", Value = "DROP_PROCEDURE" },
                                                                    new SelectListItem { Text = "CREATE_TABLE", Value = "CREATE_TABLE" } ,
                                                                    new SelectListItem { Text = "ALTER_TABLE", Value = "ALTER_TABLE" } ,
                                                                    new SelectListItem { Text = "DROP_TABLE", Value = "DROP_TABLE" } ,
                                                                    new SelectListItem { Text = "CREATE_FUNCTION", Value = "CREATE_FUNCTION" } ,
                                                                    new SelectListItem { Text = "ALTER_FUNCTION", Value = "ALTER_FUNCTION" } ,
                                                                    new SelectListItem { Text = "DROP_FUNCTION", Value = "DROP_FUNCTION" } ,
                                                                    new SelectListItem { Text = "CREATE_VIEW", Value = "CREATE_VIEW" } ,
                                                                    new SelectListItem { Text = "ALTER_VIEW", Value = "ALTER_VIEW" } ,
                                                                    new SelectListItem { Text = "DROP_VIEW", Value = "DROP_VIEW" } };
                                                                    //new SelectListItem { Text = "CREATE_TRIGGER", Value = "CREATE_TRIGGER" } ,
                                                                    //new SelectListItem { Text = "ALTER_TRIGGER", Value = "ALTER_TRIGGER" } ,
                                                                    //new SelectListItem { Text = "DROP_TRIGGER", Value = "DROP_TRIGGER" } ,

                                                                    //new SelectListItem { Text = "CREATE_XML_SCHEMA_COLLECTION", Value = "CREATE_XML_SCHEMA_COLLECTION" } ,
                                                                    //new SelectListItem { Text = "ALTER_XML_SCHEMA_COLLECTION", Value = "ALTER_XML_SCHEMA_COLLECTION" } ,
                                                                    //new SelectListItem { Text = "DROP_XML_SCHEMA_COLLECTION", Value = "DROP_XML_SCHEMA_COLLECTION" } };


            ViewData["eventType"] = seven;

            sqlEventType oObjEvent = new sqlEventType();
            DataTable dt= oObjEvent.GetEventLogs(model); 
            foreach (DataRow dataRow in dt.Rows)
            {
                EventLogDetails.Add(new EventLogDetailModel
                {
                    SerialNumber = Convert.ToString(dataRow["SerialNumber"]),
                    DatabaseName = Convert.ToString(dataRow["DatabaseName"]),
                    EventType = Convert.ToString(dataRow["EventType"]),
                    ObjectName = Convert.ToString(dataRow["ObjectName"]),
                    ObjectType = Convert.ToString(dataRow["ObjectType"]),
                    SqlCommand = Convert.ToString(dataRow["SqlCommand"]),
                    EventDate = Convert.ToString(dataRow["EventDate"]),
                    LoginName = Convert.ToString(dataRow["LoginName"])
                });
            }

            ViewData["EventLogDetails"] = EventLogDetails; 
            Session["EventLogDetails"] = dt;
            return View(); 
        }


        public ActionResult EventLogExport()
        {

            DataTable dtf = (DataTable)Session["EventLogDetails"];
            if (dtf != null)
            { 

                if (dtf != null)
                {
                    GridView gv = new GridView();
                    gv.DataSource = dtf;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=EventLog" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }


            return null;
        }


        #endregion



        public ActionResult NurseryDetails()
        {
            MISNurseryDetails obj = new MISNurseryDetails();
            getLoadData(1);
            return View(obj);
        }

        [HttpPost]
        public ActionResult NurseryDetails(MISNurseryDetails obj)
        {
            DataSet DS = obj.MIS_NurseryDetails(obj.Circle, obj.Division, obj.Range);

            var details = Globals.Util.GetListFromTable<FMDSS.Models.MIS.MISNurseryDetails>(DS.Tables[0]);
            ViewBag.Details = details;
            //getLoadData(1);
            return View(details);
        }
    }
}
