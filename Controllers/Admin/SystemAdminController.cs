//*********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS)
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : System Administration Controller
//  Description  : File contains calling functions from UI for System Administration
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  :
//  Modified On  :
//  Reviewed By  :
//  Reviewed On  :
//*********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Filters;
using FMDSS.Models.Home;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using Newtonsoft.Json;
using System.DirectoryServices;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using ClosedXML.Excel;
using FMDSS.Repository;
using FMDSS.Entity.Mob_BudgetVM;
using AutoMapper;
using System.Text;
using FMDSS.Globals;
using System.Web.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Routing;

namespace FMDSS.Controllers.Admin
{
    // [MyAuthorization]
    public class SystemAdminController : BaseController
    {
        #region Data Members

        // GET: /SystemAdmin/
        BindMasterData masterObj = new BindMasterData();
        UserApproval uAObj = new UserApproval();
        FmdssContext dbContext;
        private ICommonRepository _commonRepository;
        private IProtectionRepository _ProtectionRepository;
        public SystemAdminController()
        {
            dbContext = new FmdssContext();
             //_commonRepository = new ICommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }
        #endregion

        #region Member Functions
        #region Common
        //public ActionResult GetDashboardDetails(string moduleName, string parentID, string type)
        //{
        //    dynamic data;
        //    data = new clsForestDashboard().GetDataForDashboard(moduleName, parentID, type);
        //    ViewBag.ParentID = parentID.Replace('#', '_');
        //    switch (moduleName)
        //    {
        //        case "Budget":
        //            if (type == "BudgetCircle")
        //            {
        //                return PartialView("_DashboardBudgetCircle", data);
        //            }
        //            else if (type == "BudgetDivision")
        //            {
        //                return PartialView("_DashboardBudgetDivision", data);
        //            }
        //            else if (type == "BudgetSanctuary")
        //            {
        //                return PartialView("_DashboardBudgetSanctuary", data);
        //            }
        //            break;
        //        case "Offence":
        //            if (type == "OffenceDivisionList")
        //            {
        //                return PartialView("_DashboardOffenceDivision", data);
        //            }
        //            else if (type == "OffenceListByDivision")
        //            {
        //                return PartialView("_DashboardOffenceList", data);
        //            }
        //            else if (type == "OffenceDetailsByOffenceCode")
        //            {
        //                return PartialView("_DashboardOffenceDetails", data);
        //            }
        //            break;
        //        case "Rescue":
        //            if (type == "RescueListByDist")
        //            {
        //                return PartialView("_DashboardRescueList", data);
        //            }
        //            else if (type == "RescueDetailsByID")
        //            {
        //                return PartialView("_DashboardRescueDetails", data);
        //            }
        //            break;
        //        case "Research":
        //            if (type == "Place")
        //            {
        //                return PartialView("_DashboardResearchPlace", data);
        //            }
        //            else if (type == "ResearchListByPlace")
        //            {
        //                return PartialView("_DashboardResearchList", data);
        //            }
        //            break;
        //        //Edit By Sunny
        //        case "Enchorsment":
        //            if (type == "Division")
        //            {
        //                return PartialView("_EncroachmentList", data);
        //            }
        //            else if (type == "EnchorsmentDetailsByID")
        //            {
        //                return PartialView("_DashboardEnchorsmentDetails", data);
        //            }
        //            else if (type == "Flow")
        //            {
        //                return PartialView("_EnchorsmentFlow", data);
        //            }
        //            break;
        //        //Edit By Sunny
        //        case "ForestFireAlert":
        //            if (type == "District")
        //            {
        //                ViewBag.SSO_ID = Convert.ToString(Session["SSOID"]);
        //                return PartialView("_ForestFireDistrictList", data);
        //            }
        //            break;
        //    }
        //    return null;
        //}
        #endregion
        //Edit By Sunny
        [HttpGet]
        public FileResult GetFileEnchorsment(string id)
        {
            FileDetailsModel ObjFiles = new clsForestDashboard().GetFileList(id);
            return File(ObjFiles.FileContent, "application/pdf", "Decision");
        }

        #region Mudule Summary
        //public ActionResult ForestDashboard()
        //{
        //    clsForestDashboard FD = new Models.Admin.clsForestDashboard();
        //    var widget = FD.Forestwidgets();
        //    return View(widget);
        //}

        //public ActionResult GetModal(string ModuleId)
        //{
        //    //Edit by Sunny for EventManagement
        //    if (ModuleId == "5")
        //    {
        //        return View("EventManagement");
        //    }
        //    else
        //    {
        //        clsForestDashboard FD = new Models.Admin.clsForestDashboard();
        //        var widget = FD.GetDataForDashboard(ModuleId);
        //        switch (ModuleId)
        //        {
        //            case "1":
        //                ViewBag.ReportName = "Offence Details";
        //                return PartialView("_DashboardOffence", widget);
        //            case "2":
        //                ViewBag.ReportName = "Research Details";
        //                return PartialView("_DashboardResearch", widget);
        //            case "3":
        //                ViewBag.ReportName = "Budgeting Details";
        //                return PartialView("_DashboardBudget", widget);
        //            case "4":
        //                ViewBag.ReportName = "Rescue Details";
        //                return PartialView("_DashboardRescue", widget);
        //            //Edit by Sunny for EventManagement
        //            case "5":
        //                ViewBag.ReportName = "My Scheduler";
        //                return PartialView("EventManagement/_MyScheduler", widget);
        //            //Edit by Sunny for EncroachmentReport
        //            case "6":
        //                ViewBag.ReportName = "Encroachment Details";
        //                return PartialView("_DashboardEncroachment", widget);
        //            //Edit by Sunny for Forest Fire Alert
        //            case "7":
        //                ViewBag.ReportName = "Forest Fire Alert";
        //                return PartialView("_DashboardForestFireAlert", widget);
        //        }
        //    }
        //    return null;
        //}
        #endregion

        public ActionResult SystemAdmin()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["CURRENT_Menus"] == null)
            {
                Home obj = new Home();
                Session["CURRENT_Menus"] = obj.GetCurrentMenus(Convert.ToInt16(Session["CURRENT_ROLE"]));
            }
            return View();
        }

        //public JsonResult GetEmailStatus()
        //{
        //    GetMySchedularEmail obj = new GetMySchedularEmail();
        //    List<GetMySchedularEmail> oList = obj.GetEmailStatusContent();
        //    return Json(oList, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ReviewerApprover()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSubPermissionDetails(int PermissionId)
        {
            SuperAdminOperations obj = new SuperAdminOperations();
            List<SubPermission> oList = obj.GetSubPermission(PermissionId);
            return Json(oList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetForestBoundariesDetails(string OfficeLevel)
        {
            SuperAdminOperations obj = new SuperAdminOperations();
            List<ForestBoundares> oList = obj.GetForestBoundaryes(OfficeLevel);
            return Json(oList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetForestOfficesDetails(string Div_Code)
        {
            SuperAdminOperations obj = new SuperAdminOperations();
            List<ForestOFFICES> oObjList = obj.GetForestOFFICES(Div_Code);
            return Json(oObjList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetForestEmployeesDetails(string RowId, string SubPermissionId, string OfficeCode)
        {
            SuperAdminOperations obj = new SuperAdminOperations();
            List<ForestEmpDetails> oObjList = obj.GetForestEmployees(Convert.ToInt16(RowId), Convert.ToInt16(SubPermissionId), OfficeCode);
            return Json(oObjList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserApprovalDetail(UserApproval uA)
        {
            SuperAdminOperations obj = new SuperAdminOperations();
            Int64 result = obj.SaveUserApprovalDetails(uA);

            return Json(result, JsonRequestBehavior.AllowGet);

            //if (result > 0)
            //{
            //    Session["ActionStatus"] = "Reviewer/Approver for selected permission has been updated sucessfully!!";
            //    //return RedirectToAction("SystemAdmin", "SystemAdmin", false);
            //}
            //else
            //{
            //    Session["ActionStatus"] = "failed";
            //    // return null;
            //}
            //return Session["ActionStatus"].ToString();
            //return Json(uA, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Method responsisble for fetching Permissions
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPermission(int moduleId, int ServiceId)
        {
            DataTable dt = masterObj.GetPermission(moduleId, ServiceId);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching SubPermissions
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSubPermission(int moduleId, int ServiceId, int PermissionId)
        {
            DataTable dt = masterObj.GetSubPermission(moduleId, ServiceId, PermissionId);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching Office Levels
        /// </summary>
        /// <param name="OfficeLevel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetForestBoundaries(string OfficeLevel)
        {
            DataTable dt = masterObj.GetForestBoundaries(OfficeLevel);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching forest offices
        /// </summary>
        /// <param name="ForestCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetForestOffices(string ForestCode)
        {
            DataTable dt = masterObj.getForestOffices(ForestCode);
            ViewBag.ForestOffices = dt;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.ForestOffices.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["OfficeName"].ToString(), Value = @dr["Office_ID"].ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Method responsisble for fetching forest Employees for particular office
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="PermissionId"></param>
        /// <param name="SubPermissionId"></param>
        /// <param name="officeCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForestEmployees(int moduleId, int ServiceId, int PermissionId, int SubPermissionId, string officeCode)
        {
            List<UserApproval> uA = new List<UserApproval>();
            DataTable dt = masterObj.getForestEmployees(moduleId, ServiceId, PermissionId, SubPermissionId, officeCode);
            ViewBag.ForestOfficers = dt;
            foreach (DataRow dr in dt.Rows)
            {
                uA.Add(new UserApproval()
                {
                    UserId = dr["UserID"].ToString(),
                    Designation = dr["Designation"].ToString(),
                    EMPNAME = dr["EMPNAME"].ToString(),
                    EMPSSOID = dr["F_ID"].ToString(),
                    EMPDESIGNATION = dr["EMPDESIGNATION"].ToString(),
                    IsApprover = Convert.ToBoolean(dr["IsApprover"]),
                    IsReviewer = Convert.ToBoolean(dr["IsReviewer"])
                });
            }
            return Json(uA, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method responsisble to save reviewing / approving authority into DB
        /// </summary>
        /// <param name="uA"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveUserApproval(UserApproval uA)
        {
            Int64 result = uAObj.SaveUserApproval(uA);
            if (result > 0)
            {
                Session["ActionStatus"] = "Reviewer/Approver for selected permission has been updated sucessfully!!";
                //return RedirectToAction("SystemAdmin", "SystemAdmin", false);
            }
            else
            {
                Session["ActionStatus"] = "failed";
                // return null;
            }
            return Session["ActionStatus"].ToString();
            //return Json(uA, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Forest Dashboard
        //private int GetSiteCount(long UserId)
        //{
        //    int siteCount = 0;
        //    SqlConnection dbcon =  new SqlConnection(WebConfigurationManager.ConnectionStrings["FMDSSVER2"].ToString()); 
           
        //    try
        //    {
              
        //        dbcon.Open();

        //        SqlCommand cmd = new SqlCommand("sp_DashboardCattelGuard", dbcon);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ActionName", "2");
        //        cmd.Parameters.AddWithValue("@UserId", UserId);
              
        //        DataTable dt = new DataTable();
        //        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        //        adpt.Fill(dt);
        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //                siteCount = Convert.ToInt32(dt.Rows[0][0].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (dbcon != null)
        //        {
        //            if (dbcon.State == ConnectionState.Open)
        //            {
        //                dbcon.Close();
        //                dbcon.Dispose();
        //            }
        //        }
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {

        //        if (dbcon != null)
        //        {
        //            if (dbcon.State == ConnectionState.Open)
        //            {
        //                dbcon.Close();
        //                dbcon.Dispose();
        //            }
        //        }
        //    }
        //    return siteCount;
        //}
       
        public async Task<ActionResult> ForestDashboard()
        {
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            //SetDropdownData();
            var widget = FD.Forestwidgets();
            long UserId = Convert.ToInt64(Session["UserId"].ToString());
            string Url = Util.GetAppSettings("FMDSS2_API");
            int totalSite = 0;
            using (var client = new HttpClient())
            {
                //Passing service base url  
               
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(String.Format(Url+"/api/UserAccountApi/GetSiteCount?UserId=" + UserId));

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                     totalSite = JsonConvert.DeserializeObject<int>(response);
                }
            }

            ViewBag.SiteCount = totalSite;
            if (Session["returnURL"] != null)
            {
                if (Session["returnURL"].ToString() != "")
                    return RedirectToAction("SwitchRoles", new RouteValueDictionary(new { controller = "Home", action = "SwitchRoles", CurrentRole = 8 }));
                else
                    return View(widget);
              
                //Home / SwitchRoles ? CurrentRole = 8
                //Response.Redirect(Session["ReturnURL"].ToString(), false);
            }
            return View(widget);
        }
        public ActionResult GetModal(string ModuleId,string FinacialYear)
        {
            //Edit by Sunny for EventManagement
            if (ModuleId == "5")
            {
                return View("EventManagement");
            }
            else
            {
                clsForestDashboard FD = new Models.Admin.clsForestDashboard();
				if(string.IsNullOrEmpty(FinacialYear) && ModuleId=="8")
				{
					FinacialYear = "2021";
				}
                var widget = FD.GetDataForDashboard(ModuleId, "",FinacialYear, null,null,0);
				if (ModuleId == "8")
				{
					ViewBag.FinacialYearList = new SelectList(widget.finacialYearList, "YearId", "Years", 2021);
				}
				switch (ModuleId)
                {
                    case "1":
                        ViewBag.ReportName = "Offence Summary";
                        string json = JsonConvert.SerializeObject(widget);
                        ViewBag.ReportList = json;
                        return PartialView("_DashboardOffence", widget);
                    case "2":
                        ViewBag.ReportName = "Research Summary";
                        return PartialView("_DashboardResearch", widget);
                    case "3":
                        ViewBag.ReportName = "Budgeting Summary";
                        List<SelectListItem> FinanceYear = new List<SelectListItem>();
                        List<SelectListItem> SchemeList = new List<SelectListItem>();
                        #region FinanceYear Bind
                        var financialYear = dbContext.tbl_mst_FinancialYear.Select(i => new { i.FinancialYear, i.ID }).Distinct().ToList();
                        foreach (var item in financialYear)
                        {
                            FinanceYear.Add(new SelectListItem { Text = item.FinancialYear, Value = Convert.ToString(item.ID) });
                        }
                        ViewBag.Financial = FinanceYear;
                        #endregion
                        #region Scheme Bind
                        var SchemeLists = dbContext.tbl_FDM_SchemeForWidelife.OrderBy(s => s.Scheme_Name).Select(i => new { i.Scheme_Name, i.ID }).Distinct().ToList().OrderBy(s => s.Scheme_Name);
                        foreach (var item in SchemeLists)
                        {
                            SchemeList.Add(new SelectListItem { Text = item.Scheme_Name, Value = Convert.ToString(item.ID) });
                        }
                        ViewBag.SchemeList = SchemeList;
                        #endregion

                        return PartialView("_DashboardBudget", widget);
                    case "4":
                        ViewBag.ReportName = "Rescue Summary";
                        return PartialView("_DashboardRescue", widget);
                    //Edit by Sunny for EventManagement
                    case "5":
                        ViewBag.ReportName = "My Scheduler";
                        return PartialView("EventManagement/_MyScheduler", widget);
                    //Edit by Sunny for EncroachmentReport
                    case "6":
                        ViewBag.ReportName = "Encroachment Summary";
                        return PartialView("_DashboardEncroachment", widget);
                    //Edit by Sunny for Forest Fire Alert
                    case "7":
                        ViewBag.ReportName = "Forest Fire Alert";
                        return PartialView("_DashboardForestFireAlert", widget);
					case "8":
						ViewBag.ReportName = "MIS Nursery Summary Wise";
						ViewBag.SSO_ID = Convert.ToString(Session["SSOID"]);
						return PartialView("_DashboardMISNurseryInventory", widget);
				}
            }
            return null;
        }
        //[HttpPost]
        //public ActionResult GetForestFireAlertDetail(string fromDate, string toDate)
        //{
        //    clsForestDashboard FD = new Models.Admin.clsForestDashboard();
        //    fromDate = "1969/04/01";
        //    toDate = DateTime.Now.Date.Year + "/03/31";

        //    DateTime? FromDate = null;
        //    DateTime? ToDate = null;
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        FromDate = Convert.ToDateTime(fromDate);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        ToDate = Convert.ToDateTime(toDate);
        //    }
        //    var dataList = "";
        //    return Json(dataList, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult _DashboardCircleList(string fromDate, string toDate, string ModuleId)
        //{
        //    DateTime? FromDate = null;
        //    DateTime? ToDate = null;
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        FromDate = Convert.ToDateTime(fromDate);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        ToDate = Convert.ToDateTime(toDate);
        //    }
        //    clsForestDashboard FD = new Models.Admin.clsForestDashboard();
        //    var widget = FD.GetDataForDashboard(ModuleId, FromDate, ToDate);
        //    return PartialView(widget);
        //}


        public ActionResult _DashboardCircleList_Test(string fromDate, string toDate, string ModuleId,string offence_category)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetDataForDashboard(ModuleId, offence_category=="0"?"":offence_category,null, FromDate, ToDate,0);
            return PartialView(widget);
        }

		public ActionResult _DashboardCircleWiseStockList_Test(string fromDate, string toDate, string ModuleId, string FinacialYear)
		{
			DateTime? FromDate = null;
			DateTime? ToDate = null;
			if (!string.IsNullOrEmpty(fromDate))
			{
				FromDate = Convert.ToDateTime(fromDate);
			}
			if (!string.IsNullOrEmpty(toDate))
			{
				ToDate = Convert.ToDateTime(toDate);
			}
			clsForestDashboard FD = new Models.Admin.clsForestDashboard();
			var widget = FD.GetDataForDashboard(ModuleId,null, FinacialYear, FromDate, ToDate, 0);
			return PartialView(widget);
		}

		[HttpPost]
        public ActionResult _DashboardDivisionList_Test(string fromDate, string toDate, string DIV_CODE,string OffenceCategory, int flag)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetOffenceDashboardReportDivisionOffenceWise(fromDate, toDate, DIV_CODE, OffenceCategory == "0" ? "" : OffenceCategory, flag);
            //string json = JsonConvert.SerializeObject(widget);
            //ViewBag.ReportList = json;
            return PartialView(widget);
        }
        [HttpPost]
        public ActionResult _DashboardOffenceList_Test(string fromDate, string toDate, string DIV_CODE, string OffenceCategory, int flag)
        {
           
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetOffenceDashboardReportOffenceWise(fromDate, toDate, DIV_CODE, OffenceCategory == "0" ? "" : OffenceCategory, flag);
            //var widget = FD.GetOffenceDashboardReportOffenceWise(fromDate, toDate, DIV_CODE == "0" ? "": DIV_CODE, "", flag);
            //string json = JsonConvert.SerializeObject(widget);
            //ViewBag.ReportList = json;
            return PartialView(widget);
        }
        public ActionResult ExportToExcel( string fromDate, string toDate)
        {
            var gv = new GridView();
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetDataForDashboard("1", "",null, FromDate, ToDate,0);
            

            gv.DataSource = ToDataTable<CircleWise>(widget);
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CircleWiseReport.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return PartialView(widget);
        }
      
        public ActionResult ExportToExcelCircleList(string fromDate, string toDate, string ModuleId, string offence_category)
        {

            var gv = new GridView();
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetDataForDashboard(ModuleId, offence_category == "0" ? "" : offence_category,null, FromDate, ToDate,0);

            gv.DataSource = ToDataTable<CircleWise>(widget);
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=CircleWiseReport.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return PartialView(widget);
        }

		

		public ActionResult ExportToExcelNurseryStockCircleList(string Flag,string Code,string fromDate, string toDate, string ModuleId, string offence_category,string FinacialYear)
		{

			var gv = new GridView();
			DateTime? FromDate = null;
			DateTime? ToDate = null;
			if (!string.IsNullOrEmpty(fromDate))
			{
				FromDate = Convert.ToDateTime(fromDate);
			}
			if (!string.IsNullOrEmpty(toDate))
			{
				ToDate = Convert.ToDateTime(toDate);
			}
			clsForestDashboard FD = new Models.Admin.clsForestDashboard();
			var widget = FD.GetDataForDashboard(ModuleId, offence_category == "0" ? "" : offence_category, FinacialYear, FromDate, ToDate, 0);
			NurseryStockSummary nurseryStockSummary = new NurseryStockSummary();
			nurseryStockSummary.CircleWise = widget.CircleWise;
			nurseryStockSummary.DivWise = widget.DivWise;
			nurseryStockSummary.RangWise = widget.RangWise;
			nurseryStockSummary.NurseryWise = widget.NurseryWise;
			nurseryStockSummary.ProductWise = widget.ProductWise;
			List<CircleWiseXLS> CircleWiseXLS = new List<CircleWiseXLS>();
			List<DivisionWiseXLS> DivisionWiseXLS = new List<DivisionWiseXLS>();
			List<RangeWiseXLS> RangeWiseXLS = new List<RangeWiseXLS>();
			List<NurseryWiseXLS> NurseryWiseXLS = new List<NurseryWiseXLS>();
			List<ProductWiseXLS> ProductWiseXLS = new List<ProductWiseXLS>();
			if (Flag == "CircleWise")
			{
				int sNo = 0;
				foreach (var item in nurseryStockSummary.CircleWise)
				{
					sNo++;
					CircleWiseXLS.Add(new CircleWiseXLS()
					{
						SNo = sNo,
						//CIRCLE_CODE=item.CIRCLE_CODE,
						CIRCLE_NAME = item.CIRCLE_NAME,
						Nursery_Count = item.Nursery_Count,
						Citizen_StockTotal = item.Citizen_StockTotal,
						Dept_StockTotal = item.Dept_StockTotal,
						Total_StockTotal=item.Total_StockTotal,
						Citizen_StockOut = item.Citizen_StockOut,
						Dept_StockOut = item.Dept_StockOut,
						Total_StockOut=item.Total_StockOut
						//Citizen_RemainingQTY=item.Citizen_RemainingQTY,
						//Dept_RemainingQTY=item.Dept_RemainingQTY,
						//Total_RemainingQty=item.Total_RemainingQty
					
					});
				}


				gv.DataSource = ToDataTable<CircleWiseXLS>(CircleWiseXLS);
				gv.DataBind();
				gv.HeaderRow.Cells[0].Text = "S.No.";
				gv.HeaderRow.Cells[1].Text = "Circle Name";
				gv.HeaderRow.Cells[2].Text = "Total Nursery";
				gv.HeaderRow.Cells[3].Text = "Plants Available For Distribution";
				gv.HeaderRow.Cells[4].Text = "Plants Available For Departmental Use";
				gv.HeaderRow.Cells[5].Text = "Total Available Plants";
				gv.HeaderRow.Cells[6].Text = "Plants Used For Distribution";
				gv.HeaderRow.Cells[7].Text = "Plants Used For Departmental";
				gv.HeaderRow.Cells[8].Text = "Total Used Plants";
				decimal total_Nursery = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Nursery_Count));
				decimal total_Citizen_StockTotal = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockTotal));
				decimal total_Dept_StockTotal = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockTotal));
				decimal total_StockTotal = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockTotal));
				decimal total_Citizen_StockOut = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockOut));
				decimal total_Dept_StockOut = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockOut));
				decimal total_StockOut = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockOut));
				//decimal total_Citizen_RemainingQTY = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_RemainingQTY));
				//decimal total_Dept_RemainingQTY = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_RemainingQTY));
				//decimal total_RemainingQTY = CircleWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_RemainingQty));

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=CircleWiseReport.xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				Response.Write("<table><tr><td colspan='9' align='center' style='font-weight:bold'>Circle Wise Nursery Stock Report</td></tr>");
				Response.Write(objStringWriter.ToString());
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				gv.HeaderStyle.Font.Bold = true;
				gv.RenderControl(objHtmlTextWriter);
				Response.Write("<style> TABLE { border:solid 1px #000000; } " +
					   "TD { border:solid 1px #000000; text-align:center } </style>");
				Response.Write(objStringWriter.ToString());

				// ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
				Response.Write("<table><tr><td></td><td><b>Total: </b></td><td><b>" +
					total_Nursery.ToString() + "</b></td><td><b>" +
				total_Citizen_StockTotal.ToString() + "</b></td><td><b>" +
				total_Dept_StockTotal.ToString() + "</b></td><td><b>" +
				total_StockTotal.ToString() + "</b></td><td><b>" +
				total_Citizen_StockOut.ToString() + "</b></td><td><b>" +
				total_Dept_StockOut.ToString() + "</b></td><td><b> " +
				total_StockOut.ToString() + "</b></td></tr></table>");
				//total_Citizen_RemainingQTY.ToString() + "</b></td><td><b>" +
				//total_Dept_RemainingQTY.ToString() + "</b></td><td><b> " +
				//total_RemainingQTY.ToString() + "</b></td></tr></table>");
				//Response.Output.Write(objStringWriter.ToString());
			}
			else if(Flag=="DivisionWise")
			{
				int sNo = 0;
				foreach (var item in nurseryStockSummary.DivWise.Where(x=>x.CIRCLE_CODE==Code).ToList())
				{
					sNo++;
					DivisionWiseXLS.Add(new DivisionWiseXLS()
					{
						SNo = sNo,
						//CIRCLE_CODE=item.CIRCLE_CODE,
						DIV_NAME = item.DIV_NAME,
						Nursery_Count = item.Nursery_Count,
						Citizen_StockTotal = item.Citizen_StockTotal,
						Dept_StockTotal = item.Dept_StockTotal,
						Total_StockTotal = item.Total_StockTotal,
						Citizen_StockOut = item.Citizen_StockOut,
						Dept_StockOut = item.Dept_StockOut,
						Total_StockOut = item.Total_StockOut
						//Citizen_RemainingQTY = item.Citizen_RemainingQTY,
						//Dept_RemainingQTY = item.Dept_RemainingQTY,
						//Total_RemainingQty = item.Total_RemainingQty,
				
					});
				}


				gv.DataSource = ToDataTable<DivisionWiseXLS>(DivisionWiseXLS);
				gv.DataBind();
				gv.HeaderRow.Cells[0].Text = "S.No.";
				gv.HeaderRow.Cells[1].Text = "Division Name";
				gv.HeaderRow.Cells[2].Text = "Total Nursery";
				gv.HeaderRow.Cells[3].Text = "Plants Available For Distribution";
				gv.HeaderRow.Cells[4].Text = "Plants Available For Departmental Use";
				gv.HeaderRow.Cells[5].Text = "Total Available Plants";
				gv.HeaderRow.Cells[6].Text = "Plants Used For Distribution";
				gv.HeaderRow.Cells[7].Text = "Plants Used For Departmental";
				gv.HeaderRow.Cells[8].Text = "Total Used Plants";
				decimal total_Nursery = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Nursery_Count));
				decimal total_Citizen_StockTotal = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockTotal));
				decimal total_Dept_StockTotal = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockTotal));
				decimal total_StockTotal = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockTotal));
				decimal total_Citizen_StockOut = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockOut));
				decimal total_Dept_StockOut = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockOut));
				decimal total_StockOut = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockOut));
				//decimal total_Citizen_RemainingQTY = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_RemainingQTY));
				//decimal total_Dept_RemainingQTY = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_RemainingQTY));
				//decimal total_RemainingQTY = DivisionWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_RemainingQty));

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=DivisionWiseReport.xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				Response.Write("<table><tr><td colspan='9' align='center' style='font-weight:bold'>Division Wise Nursery Stock Report</td></tr>");
				Response.Write(objStringWriter.ToString());
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				gv.HeaderStyle.Font.Bold = true;
				gv.RenderControl(objHtmlTextWriter);
				Response.Write("<style> TABLE { border:solid 1px #000000; } " +
					   "TD { border:solid 1px #000000; text-align:center } </style>");
				Response.Write(objStringWriter.ToString());

				// ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
				Response.Write("<table><tr><td></td><td><b>Total: </b></td><td><b>" +
					total_Nursery.ToString() + "</b></td><td><b>" +
				total_Citizen_StockTotal.ToString() + "</b></td><td><b>" +
				total_Dept_StockTotal.ToString() + "</b></td><td><b>" +
				total_StockTotal.ToString() + "</b></td><td><b>" +
				total_Citizen_StockOut.ToString() + "</b></td><td><b>" +
				total_Dept_StockOut.ToString() + "</b></td><td><b> " +
				total_StockOut.ToString() + "</b></td></tr></table>");
				//total_Citizen_RemainingQTY.ToString() + "</b></td><td><b>" +
				//total_Dept_RemainingQTY.ToString() + "</b></td><td><b> " +
				//total_RemainingQTY.ToString() + "</b></td></tr></table>");
			}
			else if (Flag == "RangeWise")
			{
				int sNo = 0;
				foreach (var item in nurseryStockSummary.RangWise.Where(x => x.DIV_CODE == Code).ToList())
				{
					sNo++;
					RangeWiseXLS.Add(new RangeWiseXLS()
					{
						SNo = sNo,
						//CIRCLE_CODE=item.CIRCLE_CODE,
						RANG_NAME = item.RANG_NAME,
						Nursery_Count = item.Nursery_Count,
						Citizen_StockTotal = item.Citizen_StockTotal,
						Dept_StockTotal = item.Dept_StockTotal,
						Total_StockTotal = item.Total_StockTotal,
						Citizen_StockOut = item.Citizen_StockOut,
						Dept_StockOut = item.Dept_StockOut,
						Total_StockOut = item.Total_StockOut
						//Citizen_RemainingQTY = item.Citizen_RemainingQTY,
						//Dept_RemainingQTY = item.Dept_RemainingQTY,
						//Total_RemainingQty = item.Total_RemainingQty
						
					});
				}


				gv.DataSource = ToDataTable<RangeWiseXLS>(RangeWiseXLS);
				gv.DataBind();
				gv.HeaderRow.Cells[0].Text = "S.No.";
				gv.HeaderRow.Cells[1].Text = "Range Name";
				gv.HeaderRow.Cells[2].Text = "Total Nursery";
				gv.HeaderRow.Cells[3].Text = "Plants Available For Distribution";
				gv.HeaderRow.Cells[4].Text = "Plants Available For Departmental Use";
				gv.HeaderRow.Cells[5].Text = "Total Available Plants";
				gv.HeaderRow.Cells[6].Text = "Plants Used For Distribution";
				gv.HeaderRow.Cells[7].Text = "Plants Used For Departmental";
				gv.HeaderRow.Cells[8].Text = "Total Used Plants";
				decimal total_Nursery = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Nursery_Count));
				decimal total_Citizen_StockTotal = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockTotal));
				decimal total_Dept_StockTotal = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockTotal));
				decimal total_StockTotal = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockTotal));
				decimal total_Citizen_StockOut = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockOut));
				decimal total_Dept_StockOut = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockOut));
				decimal total_StockOut = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockOut));
				//decimal total_Citizen_RemainingQTY = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_RemainingQTY));
				//decimal total_Dept_RemainingQTY = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_RemainingQTY));
				//decimal total_RemainingQTY = RangeWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_RemainingQty));

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=RangeWiseReport.xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				Response.Write("<table><tr><td colspan='9' align='center' style='font-weight:bold'>Range Wise Nursery Stock Report</td></tr>");
				Response.Write(objStringWriter.ToString());
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				gv.HeaderStyle.Font.Bold = true;
				gv.RenderControl(objHtmlTextWriter);
				Response.Write("<style> TABLE { border:solid 1px #000000; } " +
					   "TD { border:solid 1px #000000; text-align:center } </style>");
				Response.Write(objStringWriter.ToString());

				// ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
				Response.Write("<table><tr><td></td><td><b>Total: </b></td><td><b>" +
				total_Nursery.ToString() + "</b></td><td><b>" +
				total_Citizen_StockTotal.ToString() + "</b></td><td><b>" +
				total_Dept_StockTotal.ToString() + "</b></td><td><b>" +
				total_StockTotal.ToString() + "</b></td><td><b>" +
				total_Citizen_StockOut.ToString() + "</b></td><td><b>" +
				total_Dept_StockOut.ToString() + "</b></td><td><b> " +
				total_StockOut.ToString() + "</b></td></tr></table>");
				//total_Citizen_RemainingQTY.ToString() + "</b></td><td><b>" +
				//total_Dept_RemainingQTY.ToString() + "</b></td><td><b> " +
				//total_RemainingQTY.ToString() + "</b></td></tr></table>");
			}
			else if (Flag == "NurseryWise")
			{
				int sNo = 0;
				foreach (var item in nurseryStockSummary.NurseryWise.Where(x => x.RANG_CODE == Code).ToList())
				{
					sNo++;
					NurseryWiseXLS.Add(new NurseryWiseXLS()
					{
						SNo = sNo,
						//CIRCLE_CODE=item.CIRCLE_CODE,
						NURSERY_NAME = item.NURSERY_NAME,
						Citizen_StockTotal = item.Citizen_StockTotal,
						Dept_StockTotal = item.Dept_StockTotal,
						Total_StockTotal = item.Total_StockTotal,
						Citizen_StockOut = item.Citizen_StockOut,
						Dept_StockOut = item.Dept_StockOut,
						Total_StockOut = item.Total_StockOut
						//Citizen_RemainingQTY = item.Citizen_RemainingQTY,
						//Dept_RemainingQTY = item.Dept_RemainingQTY,
						//Total_RemainingQty = item.Total_RemainingQty
					});
				}


				gv.DataSource = ToDataTable<NurseryWiseXLS>(NurseryWiseXLS);
				gv.DataBind();
				gv.HeaderRow.Cells[0].Text = "S.No.";
				gv.HeaderRow.Cells[1].Text = "Nursery Name";
				gv.HeaderRow.Cells[2].Text = "Plants Available For Distribution";
				gv.HeaderRow.Cells[3].Text = "Plants Available For Departmental Use";
				gv.HeaderRow.Cells[4].Text = "Total Available Plants";
				gv.HeaderRow.Cells[5].Text = "Plants Used For Distribution";
				gv.HeaderRow.Cells[6].Text = "Plants Used For Departmental";
				gv.HeaderRow.Cells[7].Text = "Total Used Plants";
				decimal total_Citizen_StockTotal = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockTotal));
				decimal total_Dept_StockTotal = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockTotal));
				decimal total_StockTotal = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockTotal));
				decimal total_Citizen_StockOut = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockOut));
				decimal total_Dept_StockOut = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockOut));
				decimal total_StockOut = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockOut));
				//decimal total_Citizen_RemainingQTY = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_RemainingQTY));
				//decimal total_Dept_RemainingQTY = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_RemainingQTY));
				//decimal total_RemainingQTY = NurseryWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_RemainingQty));

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=NurseryWiseReport.xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				Response.Write("<table><tr><td colspan='8' align='center' style='font-weight:bold'>Nursery Wise Stock Report</td></tr>");
				Response.Write(objStringWriter.ToString());
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				gv.HeaderStyle.Font.Bold = true;
				gv.RenderControl(objHtmlTextWriter);
				Response.Write("<style> TABLE { border:solid 1px #000000; } " +
					   "TD { border:solid 1px #000000; text-align:center } </style>");
				Response.Write(objStringWriter.ToString());

				// ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
				Response.Write("<table><tr><td></td><td><b>Total: </b></td><td><b>" +
				total_Citizen_StockTotal.ToString() + "</b></td><td><b>" +
				total_Dept_StockTotal.ToString() + "</b></td><td><b>" +
				total_StockTotal.ToString() + "</b></td><td><b>" +
				total_Citizen_StockOut.ToString() + "</b></td><td><b>" +
				total_Dept_StockOut.ToString() + "</b></td><td><b> " +
				total_StockOut.ToString() + "</b></td></tr></table>");
				//total_Citizen_RemainingQTY.ToString() + "</b></td><td><b>" +
				//total_Dept_RemainingQTY.ToString() + "</b></td><td><b> " +
				//total_RemainingQTY.ToString() + "</b></td></tr></table>");
			}
			else
			{
				int sNo = 0;
				foreach (var item in nurseryStockSummary.ProductWise.Where(x => x.NURSERY_CODE == Code).ToList())
				{
					sNo++;
					ProductWiseXLS.Add(new ProductWiseXLS()
					{
						SNo = sNo,
						//CIRCLE_CODE=item.CIRCLE_CODE,
						ProductName = item.ProductName,
						Citizen_StockTotal = item.Citizen_StockTotal,
						Dept_StockTotal = item.Dept_StockTotal,
						Total_StockTotal = item.Total_StockTotal,
						Citizen_StockOut = item.Citizen_StockOut,
						Dept_StockOut = item.Dept_StockOut,
						Total_StockOut = item.Total_StockOut
						//Citizen_RemainingQTY = item.Citizen_RemainingQTY,
						//Dept_RemainingQTY = item.Dept_RemainingQTY,
						//Total_RemainingQty = item.Total_RemainingQty
					});
				}


				gv.DataSource = ToDataTable<ProductWiseXLS>(ProductWiseXLS);
				gv.DataBind();
				gv.HeaderRow.Cells[0].Text = "S.No.";
				gv.HeaderRow.Cells[1].Text = "Product Name";
				gv.HeaderRow.Cells[2].Text = "Plants Available For Distribution";
				gv.HeaderRow.Cells[3].Text = "Plants Available For Departmental Use";
				gv.HeaderRow.Cells[4].Text = "Total Available Plants";
				gv.HeaderRow.Cells[5].Text = "Plants Used For Distribution";
				gv.HeaderRow.Cells[6].Text = "Plants Used For Departmental";
				gv.HeaderRow.Cells[7].Text = "Total Used Plants";
				decimal total_Citizen_StockTotal = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockTotal));
				decimal total_Dept_StockTotal = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockTotal));
				decimal total_StockTotal = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockTotal));
				decimal total_Citizen_StockOut = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_StockOut));
				decimal total_Dept_StockOut = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_StockOut));
				decimal total_StockOut = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_StockOut));
				//decimal total_Citizen_RemainingQTY = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Citizen_RemainingQTY));
				//decimal total_Dept_RemainingQTY = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Dept_RemainingQTY));
				//decimal total_RemainingQTY = ProductWiseXLS.AsEnumerable().Sum(row => Convert.ToDecimal(row.Total_RemainingQty));

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename=ProductWiseStockReport.xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				Response.Write("<table><tr><td colspan='8' align='center' style='font-weight:bold'>Product Wise Stock Report</td></tr>");
				Response.Write(objStringWriter.ToString());
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				gv.HeaderStyle.Font.Bold = true;
				gv.RenderControl(objHtmlTextWriter);
				Response.Write("<style> TABLE { border:solid 1px #000000; } " +
					   "TD { border:solid 1px #000000; text-align:center } </style>");
				Response.Write(objStringWriter.ToString());

				// ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
				Response.Write("<table><tr><td></td><td><b>Total: </b></td><td><b>" +
				total_Citizen_StockTotal.ToString() + "</b></td><td><b>" +
				total_Dept_StockTotal.ToString() + "</b></td><td><b>" +
				total_StockTotal.ToString() + "</b></td><td><b>" +
				total_Citizen_StockOut.ToString() + "</b></td><td><b>" +
				total_Dept_StockOut.ToString() + "</b></td><td><b> " +
				total_StockOut.ToString() + "</b></td></tr></table>");
				//total_Citizen_RemainingQTY.ToString() + "</b></td><td><b>" +
				//total_Dept_RemainingQTY.ToString() + "</b></td><td><b> " +
				//total_RemainingQTY.ToString() + "</b></td></tr></table>");
			}
			Response.Flush();
			Response.End();

			return PartialView(widget);
		}


		//public ActionResult ExportToExcelYearWise()
		//{
		//    var gv = new GridView();
		//    clsForestDashboard FD = new Models.Admin.clsForestDashboard();
		//    var widget = FD.GetYearWiseOffenceReport();

		//    gv.DataSource = ToDataTable<YearWiseOffenceReport>(widget);
		//    gv.DataBind();

		//    Response.ClearContent();
		//    Response.Buffer = true;
		//    Response.AddHeader("content-disposition", "attachment; filename=YearWiseOffenceReport.xls");
		//    Response.ContentType = "application/ms-excel";

		//    Response.Charset = "";
		//    StringWriter objStringWriter = new StringWriter();
		//    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

		//    gv.RenderControl(objHtmlTextWriter);

		//    Response.Output.Write(objStringWriter.ToString());
		//    Response.Flush();
		//    Response.End();

		//    return PartialView(widget);
		//}

		//Generic method to convert List to DataTable
		public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public ActionResult _DashboardChartView(string fromDate, string toDate, string ModuleId)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }
            clsForestDashboard FD = new Models.Admin.clsForestDashboard();
            var widget = FD.GetDataForDashboard(ModuleId, "",null, FromDate, ToDate,0);
            string json = JsonConvert.SerializeObject(widget);
            ViewBag.ReportList = json;
            return PartialView(widget);
        }
        public ActionResult GetDashboardDetails(string moduleName, string parentID, string type, string status, string fromDate, string toDate,int OffenceId)
        {
            dynamic data;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }

            data = new clsForestDashboard().GetDataForDashboard(moduleName, parentID, type, status, FromDate, ToDate, OffenceId,0);
            ViewBag.ParentID = parentID.Replace('#', '_');
            switch (moduleName)
            {
                case "Budget":
                    if (type == "BudgetCircle")
                    {
                        return PartialView("_DashboardBudgetCircle", data);
                    }
                    else if (type == "BudgetDivision")
                    {
                        return PartialView("_DashboardBudgetDivision", data);
                    }
                    else if (type == "BudgetSanctuary")
                    {
                        return PartialView("_DashboardBudgetSanctuary", data);
                    }
                    break;
                case "Offence":
                    if (type == "OffenceDivisionList")
                    {
                        return PartialView("_DashboardOffenceDivision", data);
                    }
                    else if (type == "OffenceListByDivision")
                    {
                        return PartialView("_DashboardOffenceList", data);
                    }
                    else if (type == "OffenceDetailsByOffenceCode")
                    {
                        return PartialView("_DashboardOffenceDetails", data);
                    }
                    else if (type == "OffenceRange")
                    {
                        return PartialView("_DashboardOffenceRange", data);
                    }
                    else if (type == "OffenceNaka")
                    {
                        return PartialView("_DashboardOffenceNaka", data);
                    }
                    break;
                case "Rescue":
                    if (type == "RescueListByDist")
                    {
                        return PartialView("_DashboardRescueList", data);
                    }
                    else if (type == "RescueDetailsByID")
                    {
                        return PartialView("_DashboardRescueDetails", data);
                    }
                    break;
                case "Research":
                    if (type == "Place")
                    {
                        return PartialView("_DashboardResearchPlace", data);
                    }
                    else if (type == "ResearchListByPlace")
                    {
                        return PartialView("_DashboardResearchList", data);
                    }
                    break;
                //Edit By Sunny
                case "Enchorsment":
                    if (type == "Division")
                    {
                        return PartialView("_EncroachmentList", data);
                    }
                    else if (type == "EnchorsmentDetailsByID")
                    {
                        return PartialView("_DashboardEnchorsmentDetails", data);
                    }
                    else if (type == "Flow")
                    {
                        return PartialView("_EnchorsmentFlow", data);
                    }
                    break;
                //Edit By Sunny
                case "ForestFireAlert":
                    if (type == "District")
                    {
                        ViewBag.SSO_ID = Convert.ToString(Session["SSOID"]);
                        return PartialView("_ForestFireDistrictList", data);
                    }
                    break;
            }
            return null;
        }



        public JsonResult GetEmailStatus()
        {
            GetMySchedularEmail obj = new GetMySchedularEmail();
            List<GetMySchedularEmail> oList = obj.GetEmailStatusContent();
            return Json(oList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _BudgetDashboardSummaryReport(string financialyear, string scheme)
        {
            MonthlyProgressReportModel model = new MonthlyProgressReportModel();
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            model.FinancialYear = financialyear;
            model.Scheme_Name = scheme;
            model.IsCoreOrBuffer = "BOTH";
            DataSet ReportList = new DataSet();
            List<MonthlyProgressReportModel> list = repo.GetMontlyProgressReport(model, "ALL");
            Session["List"] = list;
            List<MonthlyProgressReportModel> grpList = new List<MonthlyProgressReportModel>();
            var groups = list
             .GroupBy(n => n.Division_Name)
             .Select(n => new
             {
                 Division_Name = n.Key,
                 FinancialYear = n.FirstOrDefault().FinancialYear,
                 Scheme_Name = n.FirstOrDefault().Scheme_Name,
                 TotalAmount = n.Sum(x => x.TotalAmount),
                 AllocatedAmount = n.Sum(x => x.AllocatedAmount),
                 IsCoreOrBuffer = n.FirstOrDefault().IsCoreOrBuffer,
                 ExpenditureLastmonth = n.Sum(x => x.ExpenditureLastmonth),
                 Division_Code = n.FirstOrDefault().Division_Code,
             }
             )
             .OrderBy(n => n.Division_Name);
            groups.ToList().ForEach(a => grpList.Add(new MonthlyProgressReportModel
            {
                TotalAmount = a.TotalAmount,
                Division_Name = a.Division_Name,
                AllocatedAmount = a.AllocatedAmount,
                ExpenditureLastmonth = a.ExpenditureLastmonth,
                FinancialYear = a.FinancialYear,
                Scheme_Name = a.Scheme_Name,
                IsCoreOrBuffer = a.IsCoreOrBuffer,
                Division_Code = a.Division_Code

            }));

            return PartialView(grpList);
        }

        public ActionResult _BudgetDivisionWiseReport(string financialyear, string scheme, string division)
        {
            MonthlyProgressReportModel model = new MonthlyProgressReportModel();
            MonthlyProgressReportExpenditure repo = new MonthlyProgressReportExpenditure();
            model.FinancialYear = financialyear;
            model.Scheme_Name = scheme;
            model.IsCoreOrBuffer = "BOTH";
            DataSet ReportList = new DataSet();
            List<MonthlyProgressReportModel> list = new List<MonthlyProgressReportModel>();
            if (Session["List"] == null)
            {
                list = repo.GetMontlyProgressReport(model, "ALL");
            }
            else
            {
                list = (List<MonthlyProgressReportModel>)Session["List"];
            }
            list = list.Where(x => x.Division_Code == division).ToList();
            return PartialView(list);

        }
        #endregion
        public ActionResult YearWiseOffenceReport()
        {
            //List<YearWiseOffenceReport> list = new List<YearWiseOffenceReport>();
            //list = new clsForestDashboard().GetYearWiseOffenceReport(param);           
            return View();
        }

        [HttpPost]
        public JsonResult GetYearWiseOffenceReport(YearWiseOffenceReportParameters param)
        {
            List<YearWiseOffenceReport> list = new List<YearWiseOffenceReport>();
            list = new clsForestDashboard().GetYearWiseOffenceReport(param);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetOffences()
        {
            var data = _ProtectionRepository.OffenceDetails_GetDropdownData();
            var OffencesList = GetDropdownData(data.Tables[1]);
            return Json(OffencesList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFinancialYearList()
        {           
            var data = _ProtectionRepository.GetFinancialYear();
            var YearList = GetYearList(data.Tables[0]);
            return Json(YearList, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetDivision()
        {
            var data = _ProtectionRepository.DivisionList_GetDropdownData();
            
            var DivisionList = GetDropdownDataDiv(data.Tables[1]);
            return Json(DivisionList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOffenceDashboardReportDivisionOffenceWise(string fromDate, string toDate, string DIV_CODE, string OffenceCategory, int flag)
        {
            List<CircleWise> list = new List<CircleWise>();
            list = new clsForestDashboard().GetOffenceDashboardReportDivisionOffenceWise(fromDate, toDate, DIV_CODE, OffenceCategory, flag);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private EnumerableRowCollection<SelectListItem> GetDropdownData( DataTable dtDropdownData)
        {
           
            EnumerableRowCollection<SelectListItem> data = null;

            data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Field<int>("OffenceCategoryID")),
                Text = x.Field<string>("OffenceCategoryName")
            });
            return data;

        }
        private EnumerableRowCollection<SelectListItem> GetYearList(DataTable dtDropdownData)
        {

            EnumerableRowCollection<SelectListItem> data = null;

            data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Field<int>("YearId")),
                Text = x.Field<string>("Years")
            });
            return data;

        }
        private EnumerableRowCollection<SelectListItem> GetDropdownDataDiv(DataTable dtDropdownData)
        {

            EnumerableRowCollection<SelectListItem> data = null;

            data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Field<string>("DIV_CODE")),
                Text = x.Field<string>("DIV_NAME")
            });
            return data;

        }

        public ActionResult test()
        {
            return View();
        }
        [HttpPost]
        public FileResult Download(YearWiseOffenceReportParameters param)
        {
           
            string filePath= Server.MapPath("~/Content/TempData/rpt.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "rpt.xlsx";
            List<YearWiseOffenceReport> list = new List<YearWiseOffenceReport>();
            list = new clsForestDashboard().GetYearWiseOffenceReport(param);
            SaveExcel(list, param.ReportType);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public FileResult Download()
        {
            string filePath = Server.MapPath("~/Content/TempData/rpt.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "rpt.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public void SaveExcel(List<YearWiseOffenceReport> list,int ReportType)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Year Wise Offence Report");
            var headerRow = ws.Row(5);
            headerRow.Height = 20;


            //for fist row
            ws.Cell("A1").Value = "Office Principal Chief Conservtor of Forests, Rajasthan Jaipur";
            var titlerange = ws.Range("A1:W1");
            titlerange.Merge().Style.Font.SetBold().Font.FontSize = 14;
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //for second row
            ws.Cell("A2").Value = "Quarter Progress Report Of Forest Offences Cases ("+ list[0].StrOffenceCat.ToUpper() + ") From "+ list[0].StartDateQtr + " to "+ list[0].EndDateQtr;
            var addressrange = ws.Range("A2:W2");
            addressrange.Merge().Style.Font.SetBold().Font.FontSize = 13;
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //for third row
            ws.Cell("A3").Value = "S. No.";
            var S_No = ws.Range("A3:A4");
            S_No.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("B3").Value =(ReportType==1? "Circle Name": "Division Name") ;
            var Circle_Name = ws.Range("B3:B4");
            Circle_Name.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("B3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("C3").Value = "Last Quater Pending Cases";
            var LQPC = ws.Range("C3:E3");
            LQPC.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("C3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F3").Value = "Case Registerd in current Qtr";
            var CRCQ = ws.Range("F3:H3");
            CRCQ.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("F3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("I3").Value = "Total";
            var total = ws.Range("I3:K3");
            total.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("I3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("L3").Value = "Closed in this Year";
            var CTY = ws.Range("L3:N3");
            CTY.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("O3").Value = "Compounding Amount(Rs.)";
            var Com_Amt = ws.Range("O3:O4");
            Com_Amt.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("O3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("P3").Value = "Seized forest Produce cost (Rs.)";
            var Sei_Cost = ws.Range("P3:P4");
            Sei_Cost.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("P3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            ws.Cell("Q3").Value = "Pending at the end of this Qtr";
            var PTEOTQ = ws.Range("Q3:S3");
            PTEOTQ.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("Q3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("T3").Value = "Number of pending cases in the department    ";
            var NPCITD = ws.Range("T3:V3");
            NPCITD.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("T3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("W3").Value = "Remarks";
            var Remarks = ws.Range("W3:W4");
            Remarks.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("W3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //for fourth row
            ws.Cell("C4").Value = "Department";
            var LQPC_Department = ws.Range("C4");
            LQPC_Department.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("C4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("D4").Value = "Court";
            var LQPC_Court = ws.Range("D4");
            LQPC_Court.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("D4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("E4").Value = "Total (3+4)";
            var LQPC_TotalThreeFour = ws.Range("E4");
            LQPC_TotalThreeFour.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("E4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F4").Value = "Department";
            var CRCQ_Department = ws.Range("F4");
            CRCQ_Department.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("F4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("G4").Value = "Court";
            var CRCQ_Court = ws.Range("G4");
            CRCQ_Court.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("G4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("H4").Value = "Total (6+7)";
            var CRCQ_Total = ws.Range("H4");
            CRCQ_Total.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("H4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("I4").Value = "Department (3+6)";
            var Total_Department = ws.Range("I4");
            Total_Department.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("I4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("J4").Value = "Court (4+7)";
            var Total_Court = ws.Range("J4");
            Total_Court.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("J4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("K4").Value = "Total (9+10)";
            var Total_total = ws.Range("K4");
            Total_total.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("K4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("L4").Value = "Department";
            var CTY_Department = ws.Range("L4");
            CTY_Department.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("L4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("M4").Value = "Court";
            var CTY_Court = ws.Range("M4");
            CTY_Court.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("M4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("N4").Value = "Total (12+13)";
            var CTY_Total = ws.Range("N4");
            CTY_Total.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("N4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("Q4").Value = "Department (9-12)";
            var PTEOTQ_Department = ws.Range("Q4");
            PTEOTQ_Department.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("Q4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("R4").Value = "Court (10-13)";
            var PTEOTQ_Court = ws.Range("R4");
            PTEOTQ_Court.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("R4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("S4").Value = "Total (17+18)";
            var PTEOTQ_Total = ws.Range("S4");
            PTEOTQ_Total.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("S4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("T4").Value = "Pending In Dept < 1 Year";
            var NPCITD_PIDLOY = ws.Range("T4");
            NPCITD_PIDLOY.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("T4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("U4").Value = "Pending In Dept betwn 1 & 3 Yrs";
            var NPCITD_PIDOTY = ws.Range("U4");
            NPCITD_PIDOTY.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("U4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("V4").Value = "Pending In Dept > 3 Yrs";
            var NPCITD_PIDGTY = ws.Range("V4");
            NPCITD_PIDGTY.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("V4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //for fifth row
            ws.Cell("A5").Value = "1";
            var A5 = ws.Range("A5");
            A5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("B5").Value = "2";
            var B5 = ws.Range("B5");
            B5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("B5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("C5").Value = "3";
            var C5 = ws.Range("C5");
            C5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("C5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            ws.Cell("D5").Value = "4";
            var D5 = ws.Range("D5");
            D5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("D5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("E5").Value = "5";
            var E5 = ws.Range("E5");
            E5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("F5").Value = "6";
            var F5 = ws.Range("F5");
            F5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("F5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("G5").Value = "7";
            var G5 = ws.Range("G5");
            G5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("G5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("H5").Value = "8";
            var H5 = ws.Range("H5");
            H5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("H5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("I5").Value = "9";
            var I5 = ws.Range("I5");
            I5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("I5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("J5").Value = "10";
            var J5 = ws.Range("J5");
            J5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("J5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("K5").Value = "11";
            var K5 = ws.Range("K5");
            K5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("K5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("L5").Value = "12";
            var L5 = ws.Range("L5");
            L5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("L5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("M5").Value = "13";
            var M5 = ws.Range("M5");
            M5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("M5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("N5").Value = "14";
            var N5 = ws.Range("N5");
            N5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("N5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("O5").Value = "15";
            var O5 = ws.Range("O5");
            O5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("O5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("P5").Value = "16";
            var P5 = ws.Range("P5");
            P5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("P5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("Q5").Value = "17";
            var Q5 = ws.Range("Q5");
            Q5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("Q5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("R5").Value = "18";
            var R5 = ws.Range("R5");
            R5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("R5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("S5").Value = "19";
            var S5 = ws.Range("S5");
            S5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("S5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("T5").Value = "20";
            var T5 = ws.Range("T5");
            T5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("T5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("U5").Value = "21";
            var U5 = ws.Range("U5");
            U5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("U5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("V5").Value = "22";
            var V5 = ws.Range("V5");
            V5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("V5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell("W5").Value = "23";
            var W5 = ws.Range("W5");
            W5.Merge().Style.Font.SetBold().Font.FontSize = 12;
            ws.Cell("W5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            int i = 6;
            foreach(var obj in list)
            {
                ws.Cell("A" + i).Value = obj.RowNo;
                ws.Cell("B" + i).Value = obj.CommonName;
                ws.Cell("C" + i).Value = obj.PendingInDept_LastQtr;
                ws.Cell("D" + i).Value = obj.PendingInCourt_LastQtr;
                ws.Cell("E" + i).Value = obj.Total_LastQtr;
                ws.Cell("F" + i).Value = obj.CaseRegistration_Department_CurrentQtr;
                ws.Cell("G" + i).Value = obj.CaseRegistration_Court_CurrentQtr;
                ws.Cell("H" + i).Value = obj.Total_CaseRegistration_CurrentQTR;
                ws.Cell("I" + i).Value = obj.CaseRegistration_Department_Total;
                ws.Cell("J" + i).Value = obj.CaseRegistration_Court_Total;
                ws.Cell("K" + i).Value = obj.CaseRegistration_Total;
                ws.Cell("L" + i).Value = obj.Close_Depart_ThisYear;
                ws.Cell("M" + i).Value = obj.Close_Court_ThisYear;
                ws.Cell("N" + i).Value = obj.Close_Total_ThisYear;
                ws.Cell("O" + i).Value = obj.CurrentYearCompoundingAmount;
                ws.Cell("P" + i).Value = obj.TotalSeizedItem;
                ws.Cell("Q" + i).Value = obj.Pending_Dpt_at_the_end_of_this_Qtr;
                ws.Cell("R" + i).Value = obj.Pending_Court_at_the_end_of_this_Qtr;
                ws.Cell("S" + i).Value = obj.Pending_Total_at_the_end_of_this_Qtr;
                ws.Cell("T" + i).Value = obj.PendingInDept_LessThanOneYrs;
                ws.Cell("U" + i).Value = obj.PendingInDept_btwnOneAndThreeYrs;
                ws.Cell("V" + i).Value = obj.PendingInDept_GtrThanThreeYrs;
                ws.Cell("W" + i).Value = "";
                string First = "A" + i;
                string Last = ":W" + i;
                var panRange = ws.Range(First + Last);
                panRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                i++;
            }

            ws.Columns().Width = 100;
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();
            //Setting borders to each used cell in excel  
            ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
            ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
            ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
            ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
            ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black;

            string filePath = Server.MapPath("~/Content/TempData/Rpt.xlsx");
            
            workbook.SaveAs(filePath);
        }

        #region ConnectToFMDSS2.0
        [HttpGet]
        public ActionResult PostUserDetails(string DefaultDashboard)
        {

            //cls_userDetails oUser = new cls_userDetails { UserName = "Amit Singh", Address = "Railway Colony", mobileNumber = "07568246030", Pincode = "303313" };

            string Url = Util.GetAppSettings("FMDSS2_URL");
            Response.Clear();
            StringBuilder sb = new StringBuilder();
            UserProfile userProfile = new UserProfile();
            DataSet ds = userProfile.GetUserCDR(Convert.ToInt64(Session["UserId"].ToString()));
            string UserCDR = "";
            DataTable dtCdr = ds.Tables[0];
            DataTable dtEmp = ds.Tables[1];
            foreach (DataRow dr in dtCdr.Rows)
            {
                UserCDR = "" + dr["UserCDR"].ToString();
            }
            long UserId = 0;
            string DESIGNATION = "";
            string DESIG_NAME = "";
            string MOBILE = "";
            string AadharID = "";
            string EmailId = "";
            string OfficeName = "";
            string UserName = "";
            bool ISKIOSKUSER = false;
            bool ISDEPARTMENTALKIOSKUSER = false;
            string SSOToken = null;
            if (Session["SSOTOKEN"] != null)
            {
                SSOToken = Session["SSOTOKEN"].ToString();
            }
            UserProfile user = new UserProfile();
            foreach (DataRow dr in dtEmp.Rows)
            {
                UserId = (long)dr["UserId"];
                UserName = "" + dr["Name"].ToString();
                DESIGNATION = "" + dr["DESIGNATION"].ToString();
                DESIG_NAME = "" + dr["DESIG_NAME"].ToString();
                MOBILE = "" + dr["MOBILE"].ToString();
                AadharID = "" + dr["AadharID"].ToString();
                EmailId = "" + dr["EmailId"].ToString();
                OfficeName = "" + dr["OfficeName"].ToString();
                ISKIOSKUSER = (bool)dr["ISKIOSKUSER"];
                ISDEPARTMENTALKIOSKUSER = (bool)dr["ISDEPARTMENTALKIOSKUSER"];

                user.IsAgency = (bool)(dr["IsAgency"].ToString().Length == 0 ? false : dr["IsAgency"]);
                user.BhamashahId = "" + dr["Bhamashah_Id"].ToString();
                user.DatOFBirth = "" + dr["DOB"].ToString();
                user.Gender = "" + dr["Gender"].ToString();
                user.Address1 = "" + dr["Postal_Address1"].ToString();
                user.PINCode1 = "" + dr["Postal_Code1"].ToString();
                user.District1 = "" + dr["District1"].ToString();
                //user.PhotURL = "" + dr["PhotURL"].ToString();
                user.Address2 = "" + dr["Postal_Address2"].ToString();
                user.PINCode2 = "" + dr["Postal_Code2"].ToString();
                user.District2 = "" + dr["District2"].ToString();
                user.City2 = "" + dr["City2"].ToString();
                user.AgencyName = "" + dr["AgencyName"].ToString();
                user.AgencyDistrict = "" + dr["AgencyDistrict"].ToString();
                user.AgencyCity = "" + dr["AgencyCity"].ToString();
                user.AgencyAddress = "" + dr["AgencyAddress"].ToString();
                user.AgencySPOC = "" + dr["AgencySPOC"].ToString();
                user.AgencyContact = "" + dr["AgencyContact"].ToString();
                user.IsSSO = (bool)(dr["IsSSO"].ToString().Length == 0 ? false : dr["IsSSO"]);
                user.IsBhamashah = (bool)(dr["IsBhamashah"].ToString().Length == 0 ? false : dr["IsBhamashah"]);

            }
            userProfile = (UserProfile)Session["SSODetail"];
            bool loggedin = Convert.ToBoolean(Session["loggedin"]);
            string SSOid = Session["SSOid"].ToString();
            string role = Session["Role"].ToString();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", Url);

            

            sb.AppendFormat("<input type='hidden' name='loggedin' value='{0}'>", loggedin);
            sb.AppendFormat("<input type='hidden' name='SSOid' value='{0}'>", SSOid);
            sb.AppendFormat("<input type='hidden' name='User' value='{0}'>", UserName);
            sb.AppendFormat("<input type='hidden' name='Role' value='{0}'>", role);
            sb.AppendFormat("<input type='hidden' name='UserId' value='{0}'>", Session["UserId"].ToString());
            sb.AppendFormat("<input type='hidden' name='DesignationId' value='{0}'>", Session["DesignationId"].ToString());
            sb.AppendFormat("<input type='hidden' name='DesignationDes' value='{0}'>", Session["DesignationDes"].ToString());
            sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", Session["AadharID"].ToString());
            //sb.AppendFormat("<input type='hidden' name='AlertList' value='{0}'>", Session["AlertList"].ToString());
            sb.AppendFormat("<input type='hidden' name='IsKioskUser' value='{0}'>", ISKIOSKUSER);
            sb.AppendFormat("<input type='hidden' name='IsDepartmentalKioskUser' value='{0}'>", ISDEPARTMENTALKIOSKUSER);
            sb.AppendFormat("<input type='hidden' name='UserCDR' value='{0}'>", UserCDR);
            sb.AppendFormat("<input type='hidden' name='DESIGNATION' value='{0}'>", DESIGNATION);
            sb.AppendFormat("<input type='hidden' name='DESIG_NAME' value='{0}'>", DESIG_NAME);
            sb.AppendFormat("<input type='hidden' name='MOBILE' value='{0}'>", MOBILE);
            sb.AppendFormat("<input type='hidden' name='EmailId' value='{0}'>", EmailId);
            sb.AppendFormat("<input type='hidden' name='OfficeName' value='{0}'>", OfficeName);
            sb.AppendFormat("<input type='hidden' name='DefaultDashboard' value='{0}'>", DefaultDashboard);

            sb.AppendFormat("<input type='hidden' name='IsAgency' value='{0}'>", user.IsAgency);
            sb.AppendFormat("<input type='hidden' name='BhamashahId' value='{0}'>", user.BhamashahId);
            sb.AppendFormat("<input type='hidden' name='DatOFBirth' value='{0}'>", user.DatOFBirth);
            sb.AppendFormat("<input type='hidden' name='Gender' value='{0}'>", user.Gender);
            //sb.AppendFormat("<input type='hidden' name='TelephoneNumber' value='{0}'>", user.TelephoneNumber );
            sb.AppendFormat("<input type='hidden' name='Address1' value='{0}'>", user.Address1);
            sb.AppendFormat("<input type='hidden' name='PINCode1' value='{0}'>", user.PINCode1);
            sb.AppendFormat("<input type='hidden' name='District1' value='{0}'>", user.District1);
            //sb.AppendFormat("<input type='hidden' name='PhotURL' value='{0}'>", user.PhotURL );
            sb.AppendFormat("<input type='hidden' name='Address2' value='{0}'>", user.Address2);
            sb.AppendFormat("<input type='hidden' name='PINCode2' value='{0}'>", user.PINCode2);
            sb.AppendFormat("<input type='hidden' name='District2' value='{0}'>", user.District2);
            sb.AppendFormat("<input type='hidden' name='City2' value='{0}'>", user.City2);
            sb.AppendFormat("<input type='hidden' name='AgencyName' value='{0}'>", user.AgencyName);
            sb.AppendFormat("<input type='hidden' name='AgencyDistrict' value='{0}'>", user.AgencyDistrict);
            sb.AppendFormat("<input type='hidden' name='AgencyCity' value='{0}'>", user.AgencyCity);
            sb.AppendFormat("<input type='hidden' name='AgencyAddress' value='{0}'>", user.AgencyAddress);
            sb.AppendFormat("<input type='hidden' name='AgencySPOC' value='{0}'>", user.AgencySPOC);
            sb.AppendFormat("<input type='hidden' name='AgencyContact' value='{0}'>", user.AgencyContact);
            sb.AppendFormat("<input type='hidden' name='IsSSO' value='{0}'>", user.IsSSO);
            sb.AppendFormat("<input type='hidden' name='IsBhamashah' value='{0}'>", user.IsBhamashah);
            sb.AppendFormat("<input type='hidden' name='SSOToken' value='{0}'>", SSOToken);

            //sb.AppendFormat("<input type='hidden' name='KioskUserId' value='{0}'>", Session["KioskUserId"].ToString());
            //sb.AppendFormat("<input type='hidden' name='KioskSSOId' value='{0}'>", Session["KioskSSOId"].ToString());
            //sb.AppendFormat("<input type='hidden' name='SSOID' value='{0}'>", Session["SSOID"].ToString());
            // Other params go here
            sb.Append("</form>");

            // sb.Append("<iframe name='my-iframe' src='fmdss.html'></iframe>");
            //<iframe name="my-iframe" src="iframe.php"></iframe>

            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();

            return null;
        }
        public ActionResult iframindex()
        {
            return View();
        }



        #endregion

        #region "Online Users"
        [HttpGet]
        public ActionResult OnlineUser()
        {
            return View();
        }


        [HttpPost]
        public JsonResult OnlineUser(string name)
        {
            SuperAdminOperations oObj = new SuperAdminOperations();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //filterContext.ActionDescriptor.ActionName, DateTime.Now, "Start", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName + "?" + string.Join(",", actionParam), "", Convert.ToString(Session["SSOID"]), IPAddress

            var oValue = oObj.InsertOnlineUser(actionName, this.Request.UserHostAddress, Convert.ToString(Session["SSOID"]), controllerName);
            return Json(oValue, JsonRequestBehavior.AllowGet);
            //return View();
        }

        [HttpGet]
        public JsonResult GetOnlineUser()
        {
            SuperAdminOperations oObj = new SuperAdminOperations();
            var data = oObj.GetOnlineUser();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        #endregion



        #region ConnectToFMDSS2.0
        [HttpGet]
        public ActionResult PostUserDetails()
        {

            //cls_userDetails oUser = new cls_userDetails { UserName = "Amit Singh", Address = "Railway Colony", mobileNumber = "07568246030", Pincode = "303313" };

            string Url = ConfigurationManager.AppSettings["FMDSS2_URL"];
            Response.Clear();
            StringBuilder sb = new StringBuilder();
            UserProfile userProfile = new UserProfile();
            DataSet ds = userProfile.GetUserCDR(Convert.ToInt64(Session["UserId"].ToString()));
            string UserCDR = "";
            DataTable dtCdr = ds.Tables[0];
            DataTable dtEmp = ds.Tables[1];
            foreach (DataRow dr in dtCdr.Rows)
            {
                UserCDR = "" + dr["UserCDR"].ToString();
            }
            long UserId = 0;
            string DESIGNATION = "";
            string DESIG_NAME = "";
            string MOBILE = "";
            string AadharID = "";
            string EmailId = "";
            string OfficeName = "";
            string UserName = "";
            bool ISKIOSKUSER = false;
            bool ISDEPARTMENTALKIOSKUSER = false;
            string SSOToken = null;
            if (Session["SSOTOKEN"] != null)
            {
                SSOToken = Session["SSOTOKEN"].ToString();
            }
            UserProfile user = new UserProfile();
            foreach (DataRow dr in dtEmp.Rows)
            {
                UserId = (long)dr["UserId"];
                UserName = "" + dr["Name"].ToString();
                DESIGNATION = "" + dr["DESIGNATION"].ToString();
                DESIG_NAME = "" + dr["DESIG_NAME"].ToString();
                MOBILE = "" + dr["MOBILE"].ToString();
                AadharID = "" + dr["AadharID"].ToString();
                EmailId = "" + dr["EmailId"].ToString();
                OfficeName = "" + dr["OfficeName"].ToString();
                ISKIOSKUSER = (bool)dr["ISKIOSKUSER"];
                ISDEPARTMENTALKIOSKUSER = (bool)dr["ISDEPARTMENTALKIOSKUSER"];

                user.IsAgency = (bool)(dr["IsAgency"].ToString().Length == 0 ? false : dr["IsAgency"]);
                user.BhamashahId = "" + dr["Bhamashah_Id"].ToString();
                user.DatOFBirth = "" + dr["DOB"].ToString();
                user.Gender = "" + dr["Gender"].ToString();
                user.Address1 = "" + dr["Postal_Address1"].ToString();
                user.PINCode1 = "" + dr["Postal_Code1"].ToString();
                user.District1 = "" + dr["District1"].ToString();
                //user.PhotURL = "" + dr["PhotURL"].ToString();
                user.Address2 = "" + dr["Postal_Address2"].ToString();
                user.PINCode2 = "" + dr["Postal_Code2"].ToString();
                user.District2 = "" + dr["District2"].ToString();
                user.City2 = "" + dr["City2"].ToString();
                user.AgencyName = "" + dr["AgencyName"].ToString();
                user.AgencyDistrict = "" + dr["AgencyDistrict"].ToString();
                user.AgencyCity = "" + dr["AgencyCity"].ToString();
                user.AgencyAddress = "" + dr["AgencyAddress"].ToString();
                user.AgencySPOC = "" + dr["AgencySPOC"].ToString();
                user.AgencyContact = "" + dr["AgencyContact"].ToString();
                user.IsSSO = (bool)(dr["IsSSO"].ToString().Length == 0 ? false : dr["IsSSO"]);
                user.IsBhamashah = (bool)(dr["IsBhamashah"].ToString().Length == 0 ? false : dr["IsBhamashah"]);

            }
            userProfile = (UserProfile)Session["SSODetail"];
            bool loggedin = Convert.ToBoolean(Session["loggedin"]);
            string SSOid = Session["SSOid"].ToString();
            string role = Session["Role"].ToString();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.68.128.179/home/UserData");

            //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.70.231.190/Base/GetUserProfile");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", Url);

            //sb.AppendFormat("<form name='form' action='{0}' method='post' target='my-iframe'>", "http://10.70.231.190/Base/GetUserProfile");

            sb.AppendFormat("<input type='hidden' name='loggedin' value='{0}'>", loggedin);
            sb.AppendFormat("<input type='hidden' name='SSOid' value='{0}'>", SSOid);
            sb.AppendFormat("<input type='hidden' name='User' value='{0}'>", UserName);
            sb.AppendFormat("<input type='hidden' name='Role' value='{0}'>", role);

            sb.AppendFormat("<input type='hidden' name='UserId' value='{0}'>", Session["UserId"].ToString());
            sb.AppendFormat("<input type='hidden' name='DesignationId' value='{0}'>", (Session["DesignationId"] == null ? "0" : Session["DesignationId"].ToString()));
            sb.AppendFormat("<input type='hidden' name='DesignationDes' value='{0}'>", (Session["DesignationDes"] == null ? "" : Session["DesignationDes"].ToString()));
            //sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", Session["AadharID"].ToString());
            sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", AadharID);
            //sb.AppendFormat("<input type='hidden' name='AlertList' value='{0}'>", Session["AlertList"].ToString());
            sb.AppendFormat("<input type='hidden' name='IsKioskUser' value='{0}'>", ISKIOSKUSER);
            sb.AppendFormat("<input type='hidden' name='IsDepartmentalKioskUser' value='{0}'>", ISDEPARTMENTALKIOSKUSER);
            sb.AppendFormat("<input type='hidden' name='UserCDR' value='{0}'>", UserCDR);
            sb.AppendFormat("<input type='hidden' name='DESIGNATION' value='{0}'>", DESIGNATION);
            sb.AppendFormat("<input type='hidden' name='DESIG_NAME' value='{0}'>", DESIG_NAME);
            sb.AppendFormat("<input type='hidden' name='MOBILE' value='{0}'>", MOBILE);
            sb.AppendFormat("<input type='hidden' name='EmailId' value='{0}'>", EmailId);
            sb.AppendFormat("<input type='hidden' name='OfficeName' value='{0}'>", OfficeName);
            sb.AppendFormat("<input type='hidden' name='DefaultDashboard' value='{0}'>", "CitizenDashboard");

            sb.AppendFormat("<input type='hidden' name='IsAgency' value='{0}'>", user.IsAgency);
            sb.AppendFormat("<input type='hidden' name='BhamashahId' value='{0}'>", user.BhamashahId);
            sb.AppendFormat("<input type='hidden' name='DatOFBirth' value='{0}'>", user.DatOFBirth);
            sb.AppendFormat("<input type='hidden' name='Gender' value='{0}'>", user.Gender);
            //sb.AppendFormat("<input type='hidden' name='TelephoneNumber' value='{0}'>", user.TelephoneNumber );
            sb.AppendFormat("<input type='hidden' name='Address1' value='{0}'>", user.Address1);
            sb.AppendFormat("<input type='hidden' name='PINCode1' value='{0}'>", user.PINCode1);
            sb.AppendFormat("<input type='hidden' name='District1' value='{0}'>", user.District1);
            //sb.AppendFormat("<input type='hidden' name='PhotURL' value='{0}'>", user.PhotURL );
            sb.AppendFormat("<input type='hidden' name='Address2' value='{0}'>", user.Address2);
            sb.AppendFormat("<input type='hidden' name='PINCode2' value='{0}'>", user.PINCode2);
            sb.AppendFormat("<input type='hidden' name='District2' value='{0}'>", user.District2);
            sb.AppendFormat("<input type='hidden' name='City2' value='{0}'>", user.City2);
            sb.AppendFormat("<input type='hidden' name='AgencyName' value='{0}'>", user.AgencyName);
            sb.AppendFormat("<input type='hidden' name='AgencyDistrict' value='{0}'>", user.AgencyDistrict);
            sb.AppendFormat("<input type='hidden' name='AgencyCity' value='{0}'>", user.AgencyCity);
            sb.AppendFormat("<input type='hidden' name='AgencyAddress' value='{0}'>", user.AgencyAddress);
            sb.AppendFormat("<input type='hidden' name='AgencySPOC' value='{0}'>", user.AgencySPOC);
            sb.AppendFormat("<input type='hidden' name='AgencyContact' value='{0}'>", user.AgencyContact);
            sb.AppendFormat("<input type='hidden' name='IsSSO' value='{0}'>", user.IsSSO);
            sb.AppendFormat("<input type='hidden' name='IsBhamashah' value='{0}'>", user.IsBhamashah);
            sb.AppendFormat("<input type='hidden' name='SSOToken' value='{0}'>", SSOToken);

            //sb.AppendFormat("<input type='hidden' name='KioskUserId' value='{0}'>", Session["KioskUserId"].ToString());
            //sb.AppendFormat("<input type='hidden' name='KioskSSOId' value='{0}'>", Session["KioskSSOId"].ToString());
            //sb.AppendFormat("<input type='hidden' name='SSOID' value='{0}'>", Session["SSOID"].ToString());
            // Other params go here
            sb.Append("</form>");

            // sb.Append("<iframe name='my-iframe' src='fmdss.html'></iframe>");
            //<iframe name="my-iframe" src="iframe.php"></iframe>

            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();

            return null;
        }
        //public ActionResult PostUserDetails()
        //{

        //    //cls_userDetails oUser = new cls_userDetails { UserName = "Amit Singh", Address = "Railway Colony", mobileNumber = "07568246030", Pincode = "303313" };

        //    string Url = Util.GetAppSettings("FMDSS2_URL");
        //    Response.Clear();
        //    StringBuilder sb = new StringBuilder();
        //    UserProfile userProfile = new UserProfile();
        //    DataSet ds = userProfile.GetUserCDR(Convert.ToInt64(Session["UserId"].ToString()));
        //    string UserCDR = "";
        //    DataTable dtCdr = ds.Tables[0];
        //    DataTable dtEmp = ds.Tables[1];
        //    foreach (DataRow dr in dtCdr.Rows)
        //    {
        //        UserCDR = "" + dr["UserCDR"].ToString();
        //    }
        //    long UserId = 0;
        //    string DESIGNATION = "";
        //    string DESIG_NAME = "";
        //    string MOBILE= "";
        //    string AadharID= "";
        //    string EmailId= "";
        //    string OfficeName = "";
        //    bool ISKIOSKUSER =false;
        //    bool ISDEPARTMENTALKIOSKUSER = false;	
        //    foreach (DataRow dr in dtEmp.Rows)
        //    {
        //        UserId = (long)dr["UserId"];
        //        DESIGNATION = "" + dr["DESIGNATION"].ToString();
        //        DESIG_NAME = "" + dr["DESIG_NAME"].ToString();
        //        MOBILE = "" + dr["MOBILE"].ToString();
        //        AadharID = "" + dr["AadharID"].ToString();
        //        EmailId = "" + dr["EmailId"].ToString();
        //        OfficeName = "" + dr["OfficeName"].ToString();
        //        ISKIOSKUSER = (bool)dr["ISKIOSKUSER"];
        //        ISDEPARTMENTALKIOSKUSER = (bool)dr["ISDEPARTMENTALKIOSKUSER"];
        //        Session["AadharID"]= "" + dr["AadharID"].ToString(); 
        //    }
        //    userProfile = (UserProfile)Session["SSODetail"];
        //    bool loggedin = Convert.ToBoolean(Session["loggedin"]);
        //    string SSOid = Session["SSOid"].ToString();
        //    string role = Session["Role"].ToString();
        //    sb.Append("<html>");
        //    sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
        //    //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.68.128.179/home/UserData");

        //    //sb.AppendFormat("<form name='form' action='{0}' method='post'>", "http://10.70.231.190/Base/GetUserProfile");
        //    sb.AppendFormat("<form name='form' action='{0}' method='post'>",Url );

        //    //sb.AppendFormat("<form name='form' action='{0}' method='post' target='my-iframe'>", "http://10.70.231.190/Base/GetUserProfile");

        //    sb.AppendFormat("<input type='hidden' name='loggedin' value='{0}'>", loggedin);
        //    sb.AppendFormat("<input type='hidden' name='SSOid' value='{0}'>", SSOid);
        //    sb.AppendFormat("<input type='hidden' name='User' value='{0}'>", Session["User"].ToString());
        //    sb.AppendFormat("<input type='hidden' name='Role' value='{0}'>", role);
        //    sb.AppendFormat("<input type='hidden' name='UserId' value='{0}'>", Session["UserId"].ToString());
        //    sb.AppendFormat("<input type='hidden' name='DesignationId' value='{0}'>", Session["DesignationId"].ToString());
        //    sb.AppendFormat("<input type='hidden' name='DesignationDes' value='{0}'>", Session["DesignationDes"].ToString());           
        //    sb.AppendFormat("<input type='hidden' name='AadharID' value='{0}'>", Session["AadharID"].ToString());
        //    //sb.AppendFormat("<input type='hidden' name='AlertList' value='{0}'>", Session["AlertList"].ToString());
        //    sb.AppendFormat("<input type='hidden' name='IsKioskUser' value='{0}'>", ISKIOSKUSER);
        //    sb.AppendFormat("<input type='hidden' name='IsDepartmentalKioskUser' value='{0}'>", ISDEPARTMENTALKIOSKUSER);
        //    sb.AppendFormat("<input type='hidden' name='UserCDR' value='{0}'>", UserCDR);
        //    sb.AppendFormat("<input type='hidden' name='DESIGNATION' value='{0}'>", DESIGNATION);
        //    sb.AppendFormat("<input type='hidden' name='DESIG_NAME' value='{0}'>", DESIG_NAME);
        //    sb.AppendFormat("<input type='hidden' name='MOBILE' value='{0}'>", MOBILE);
        //    sb.AppendFormat("<input type='hidden' name='EmailId' value='{0}'>", EmailId);
        //    sb.AppendFormat("<input type='hidden' name='OfficeName' value='{0}'>", OfficeName);           
        //    //sb.AppendFormat("<input type='hidden' name='KioskUserId' value='{0}'>", Session["KioskUserId"].ToString());
        //    //sb.AppendFormat("<input type='hidden' name='KioskSSOId' value='{0}'>", Session["KioskSSOId"].ToString());
        //    //sb.AppendFormat("<input type='hidden' name='SSOID' value='{0}'>", Session["SSOID"].ToString());
        //    // Other params go here
        //    sb.Append("</form>");

        //   // sb.Append("<iframe name='my-iframe' src='fmdss.html'></iframe>");
        //    //<iframe name="my-iframe" src="iframe.php"></iframe>

        //    sb.Append("</body>");
        //    sb.Append("</html>");
        //    Response.Write(sb.ToString());
        //    Response.End();

        //    return null;
        //}

        #endregion

    }

       

}
