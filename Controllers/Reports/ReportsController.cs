using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FMDSS.Models;
using FMDSS.Models.Admin;
using System.IO;

namespace FMDSS.Controllers.Reports
{
    public class ReportsController : BaseController
    {
        //
        // GET: /Reports/
        Models.Reports.Report _ModelReport = new Models.Reports.Report();
        List<SelectListItem> ddlReports = new List<SelectListItem>();
        List<SelectListItem> FinancialYear = new List<SelectListItem>();
        List<SelectListItem> SubPermissionType = new List<SelectListItem>();
        List<SelectListItem> ddlCircles = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> ReptData = new List<SelectListItem>();
        public ActionResult GetReport()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                DataTable dt = _ModelReport.GetFinancialYears();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    FinancialYear.Add(new SelectListItem { Text = @dr["FinancialYear"].ToString(), Value = @dr["FinancialYear"].ToString() });
                }
                ViewBag.Finyear = FinancialYear;

                DataTable dt1 = _ModelReport.GetReportNames();
                foreach (System.Data.DataRow dr in dt1.Rows)
                {
                    ddlReports.Add(new SelectListItem { Text = @dr["Report_Name"].ToString(), Value = @dr["Report_id"].ToString() });
                }
                ViewBag.Reports = ddlReports;

                Location objLoc = new Location();
                dt1 = objLoc.BindCircle();
                foreach (System.Data.DataRow dr in dt1.Rows)
                {
                    ddlCircles.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                }
                ViewBag.Circles = ddlCircles;
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        //[HttpPost]
        //public JsonResult Get_SubPremission(string ModuleID)
        //{
        //    _ModelReport.Type = "2";
        //    _ModelReport.ModuleId = ModuleID;
        //    DataTable dt1 = _ModelReport.GetPermissionType(_ModelReport);
        //    foreach (System.Data.DataRow dr in dt1.Rows)
        //    {
        //        SubPermissionType.Add(new SelectListItem { Text = @dr["SubPermissionDesc"].ToString(), Value = @dr["SubPermissionId"].ToString() });
        //    }

        //    return Json(SubPermissionType, JsonRequestBehavior.AllowGet);
        //    //ViewBag.SubPermissionType = SubPermissionType;
        //}


        [HttpPost]
        public ActionResult GetReport(FormCollection frm, string Command)
        {
            _ModelReport.FinancialYear = frm["ddlFinancialYear"].ToString();
            _ModelReport.ReportId = frm["ddlReports"].ToString();
            _ModelReport.MonthId = frm["ddlMonths"].ToString();
            _ModelReport.MonthName = frm["hdnMonth"].ToString();
            _ModelReport.CircleCode = frm["ddlCircles"].ToString();
            string variables = "FinancialYear=" + Encryption.encrypt(_ModelReport.FinancialYear) + "&ReportId=" + Encryption.encrypt(_ModelReport.ReportId) + "&MonthId=" + Encryption.encrypt(_ModelReport.MonthId) + "&MonthName=" + Encryption.encrypt(_ModelReport.MonthName) + "&CircleCode=" + Encryption.encrypt(_ModelReport.CircleCode);
            return Redirect("../MISReports/Reports.aspx?" + variables);
        }

        //public ActionResult GetExcelReport(string fYear, string moduleId, string subModuleId, string distCode)
        //{
        //    // string filePath = Server.MapPath(@"~/FMDSS_Reports/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);

        //    try
        //    {
        //        _ModelReport.ModuleId = Encryption.decrypt(moduleId);
        //        _ModelReport.SubModuleId = Encryption.decrypt(subModuleId);
        //        _ModelReport.FinancialYear = Encryption.decrypt(fYear);
        //        _ModelReport.DistCode = Encryption.decrypt(distCode);

        //        DataTable DT1 = _ModelReport.GetReport(_ModelReport);
        //        DT1.AcceptChanges();
        //        _ModelReport.DT = DT1;

        //        HttpResponse Response = System.Web.HttpContext.Current.Response;
        //        Response.ClearHeaders();
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", "attachment; filename = " + DT1.Rows[0][1].ToString() + "_" + DateTime.Now.ToFileTimeUtc() + ".xls");
        //        Response.ContentType = "application/vnd.ms-excel";
        //        string tab = "";
        //        foreach (DataColumn dc in DT1.Columns)
        //        {
        //            Response.Write(tab + dc.ColumnName);
        //            tab = "\t";
        //        }
        //        Response.Write("\n");

        //        foreach (DataRow dr in DT1.Rows)
        //        {
        //            tab = "";
        //            for (int i = 0; i < DT1.Columns.Count; i++)
        //            {
        //                Response.Write(tab + dr[i].ToString());
        //                tab = "\t";
        //            }
        //            Response.Write("\n");
        //        }

        //        Response.End();
        //        Response.Flush();
        //        return RedirectToAction("GetReport", "Reports");
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
