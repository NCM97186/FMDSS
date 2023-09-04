using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMDSS.Models;
using FMDSS.Models.MIS;
using System.Data.SqlClient;

namespace FMDSS.Controllers.MIS
{
    public class MISCitizenController : BaseController
    {
        CitizenModel Model = new CitizenModel();
        List<SelectListItem> lstCircle = new List<SelectListItem>();
        List<SelectListItem> lstTransferCircle = new List<SelectListItem>();
        List<SelectListItem> itemsRange = new List<SelectListItem>();
        List<SelectListItem> itemsDivision = new List<SelectListItem>();
        List<SelectListItem> lstStatus = new List<SelectListItem>();
        List<MISCitizenModel> ListMISCitizenModelSummary = new List<MISCitizenModel>();
        List<MISCitizenModel> ListMISCitizenModelDetails = new List<MISCitizenModel>();
        MISCitizenModel OBJ = new MISCitizenModel();
        [NonAction]
        public void getLoadData(Int16 status)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
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
                DataTable dt1 = Model.GetCircle();
                lstCircle.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                lstCircle.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                foreach (System.Data.DataRow dr in dt1.Rows)
                {
                    lstCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["Circle_Code"].ToString() });
                    lstTransferCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["Circle_Code"].ToString() });

                }
                ViewBag.CIRCLE = lstCircle;
                ViewBag.TransferCircle = lstTransferCircle;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            ViewBag.Division = itemsDivision;
            ViewBag.Range = itemsRange;

            DataTable dt2 = Model.GetPermissionStatusForResearch();
            lstStatus.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            lstStatus.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            foreach (System.Data.DataRow dr in dt2.Rows)
            {
                lstStatus.Add(new SelectListItem { Text = dr["StatusDesc"].ToString(), Value = dr["StatusID"].ToString() });
            }
            ViewBag.Status = lstStatus;

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
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
        }
        public void SessionForJson()
        {
            //if (Session["UserID"].ToString() == string.Empty || Session["UserID"] == null)
            //{
            //    RedirectToAction("login", "login");
            //}
        }
        public void GetJsonDataStateNew(string Circle, string Division)
        {
            if ((!String.IsNullOrEmpty(Circle)))
            {
                if (Circle == "ALL")
                {
                    itemsDivision.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsDivision.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                }
                else
                {
                    itemsDivision.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsDivision.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    DataTable dt = Model.GetDivision(Convert.ToString(Circle));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        itemsDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                    }
                }
            }
            if ((!String.IsNullOrEmpty(Division)))
            {
                if (Division == "ALL")
                {
                    itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                }
                else
                {
                    itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    DataTable dt = Model.GetBLOCK(Convert.ToString(Division));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        itemsRange.Add(new SelectListItem { Text = dr["BLK_NAME"].ToString(), Value = dr["BLK_CODE"].ToString() });
                    }
                }
            }
            //if ((!String.IsNullOrEmpty(Division)))
            //{
            //    if (Division == "ALL")
            //    {
            //        itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //        itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            //    }
            //    else
            //    {
            //        itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //        itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            //        DataTable dt = Model.GetRange(Convert.ToString(Division));
            //        foreach (System.Data.DataRow dr in dt.Rows)
            //        {
            //            itemsRange.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
            //        }
            //    }
            //}
            //GetBLOCK
            ViewBag.Division = itemsDivision;
            ViewBag.Range = itemsRange;
        }
        public void GetJsonDataState(string Circle, string Division)
        {
            if ((!String.IsNullOrEmpty(Circle)))
            {
                if (Circle == "ALL")
                {
                    itemsDivision.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsDivision.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                }
                else
                {
                    itemsDivision.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsDivision.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    DataTable dt = Model.GetDivision(Convert.ToString(Circle));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        itemsDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                    }
                }
            }
            if ((!String.IsNullOrEmpty(Division)))
            {
                if (Division == "ALL")
                {
                    itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                }
                else
                {
                    itemsRange.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    itemsRange.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    DataTable dt = Model.GetRange(Convert.ToString(Division));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        itemsRange.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                    }
                }
            }
            //GetBLOCK
            ViewBag.Division = itemsDivision;
            ViewBag.Range = itemsRange;
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
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
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
        [HttpGet]
        public ActionResult CSDivisionWise()
        {
            getLoadData(1);
            getPermissionData();
            return View();
        }
        [HttpPost]
        public ActionResult CSDivisionWise(MISCitizenModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (MIS.Duration == "Yearly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Quarterly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Monthly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Cumulative")
                {
                    MIS.FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else
                {
                }
                DataTable DT;
                StringBuilder SB = new StringBuilder();
                DT = OBJ.GetCSDivisionWise(MIS.FromDate, MIS.ToDate, MIS.Circle, MIS.Division, MIS.Range, MIS.PermissionType);
                int count = 1;
                Session["CSDivisionWiseDownload"] = DT;
                if (DT.Rows.Count > 0)
                {
                    SB.Append("<table class='table table-striped table-bordered table-hover table-responsive gridtable'><thead><tr>");
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
                getLoadData(1);
                getPermissionData();
                GetJsonDataState(MIS.Circle, MIS.Division);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }
        public ActionResult CSDivisionWiseExport()
        {
            DataTable dtf = (DataTable)Session["CSDivisionWiseDownload"];
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CSDivisionWise_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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
        [HttpGet]
        public ActionResult CSRevenue()
        {
            getLoadData(1);
            getPermissionData();
            return View();
        }
        [HttpPost]
        public ActionResult CSRevenue(MISCitizenModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (MIS.Duration == "Yearly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Quarterly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Monthly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Cumulative")
                {
                    MIS.FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else
                {
                }
                DataTable DT;
                StringBuilder SB = new StringBuilder();
                DT = OBJ.GetCSRevenue(MIS.FromDate, MIS.ToDate, MIS.Circle, MIS.Division, MIS.BLKCode);
                int count = 1;
                Session["CSRevenueDownload"] = DT;
                if (DT.Rows.Count > 0)
                {
                    SB.Append("<table class='table table-striped table-bordered table-hover table-responsive gridtable'><thead><tr>");
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
                getLoadData(1);
                getPermissionData();
                GetJsonDataStateNew(MIS.Circle, MIS.Division);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }
        public ActionResult CSRevenueExport()
        {
            DataTable dtf = (DataTable)Session["CSRevenueDownload"];
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CSRevenue_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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
                // items.Add(new SelectListItem { Text = "Online Booking Permission", Value = "Online Booking Permission" });
                ViewBag.PermissionType = items;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
        }
        public JsonResult PermissionTypeSubCategoryData(string PermissionType)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                items = PremissionSubCat(PermissionType);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }
        public List<SelectListItem> PremissionSubCat(string PermissionType)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if ((!String.IsNullOrEmpty(PermissionType)))
            {
                if (PermissionType == "NOCs")
                {
                    items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    DataTable dt = Model.GetFIXEDPermissionSubCategory();
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["Name"].ToString() });
                    }
                }
                else if (PermissionType == "WildLifeAndForest")
                {
                    items.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                    items.Add(new SelectListItem { Text = "Research Study Permission in Wildlife", Value = "Research Study Permission in Wildlife" });
                    items.Add(new SelectListItem { Text = "Research Study Permission in Forest", Value = "Research Study Permission in Forest" });
                    items.Add(new SelectListItem { Text = "Education Visit Permission  in Wildlife", Value = "Education Visit Permission  in Wildlife" });
                    items.Add(new SelectListItem { Text = "Education Visit Permission in Forest", Value = "Education Visit Permission in Forest" });
                    //items.Add(new SelectListItem { Text = "Camp Permission", Value = "Camp Permission" });
                    //items.Add(new SelectListItem { Text = "Film Shooting Permission", Value = "Film Shooting Permission" });
                }
            }
            return items;
        }
        [HttpGet]
        public ActionResult CSPermissionsDrillDown()
        {
            getLoadData(1);
            getPermissionData();
            PermissionTypeCategoryData();
            ViewBag.PremissionSubCategory = PremissionSubCat("NOCs");
            ViewData["ListMISCitizenModelSummary"] = ListMISCitizenModelSummary;
            return View();
        }
        [HttpPost]
        public ActionResult CSPermissionsDrillDown(MISCitizenModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (MIS.Duration == "Yearly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Quarterly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Monthly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else if (MIS.Duration == "Cumulative")
                {
                    MIS.FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else
                {
                }
                DataSet DS = new DataSet();
                StringBuilder SB = new StringBuilder();
                DS = OBJ.CSPermissionsDrillDown("MIS_ReportCitizenService_NOCsDrillDown", MIS.FromDate, MIS.ToDate, MIS.Circle, MIS.Division, MIS.BLKCode, MIS.SubPermissionType);
                int count = 1;
                //Session["CSPermissionsDrillDownDownload"] = DS;
                ViewBag.ABS = string.Empty;
                string Category = string.Empty;
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    if (ViewBag.ABS == string.Empty)
                    {
                        ViewBag.ABS = Convert.ToString(dr["Category"]);
                        Category = "True";
                    }
                    else if (ViewBag.ABS == Convert.ToString(dr["Category"]))
                    {
                        Category = "False";
                    }
                    else
                    {
                        ViewBag.ABS = Convert.ToString(dr["Category"]);
                        Category = "True";
                    }
                    ListMISCitizenModelSummary.Add(
                        new MISCitizenModel()
                        {
                            index = count,
                            Category = Convert.ToString(dr["Category"]),
                            Status = Convert.ToString(dr["Status"]),
                            StatusName = Convert.ToString(dr["StatusName"]),
                            CategoryStatus = Category,
                        });
                    count += 1;
                }
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    ListMISCitizenModelDetails.Add(
                        new MISCitizenModel()
                        {
                            Category = Convert.ToString(dr["Category"]),
                            Status = Convert.ToString(dr["Status"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            Remark = Convert.ToString(dr["Remark"]),
                        });
                }
                TempData["NOCsData"] = ListMISCitizenModelDetails;
                ViewData["ListMISCitizenModelSummary"] = ListMISCitizenModelSummary;
                getLoadData(1);
                getPermissionData();
                GetJsonDataStateNew(MIS.Circle, MIS.Division);
                ViewBag.PremissionSubCategory = PremissionSubCat("NOCs");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }
        public JsonResult GetNOCsRejectApplicationData(string category, string status)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISCitizenModel> ObjList = new List<MISCitizenModel>();
            List<MISCitizenModel> ObjListFinal = new List<MISCitizenModel>();
            try
            {
                if (TempData["NOCsData"] != null)
                {
                    ObjList = TempData["NOCsData"] as List<MISCitizenModel>;
                    ObjListFinal = ObjList.Where(s => s.Category.Contains(category) && s.Status.Contains(status)).ToList();
                    TempData["NOCsData"] = ObjList;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(ObjListFinal);
        }
        public ActionResult CSPermissionsDrillDownExport()
        {
            List<MISCitizenModel> ObjList = new List<MISCitizenModel>();
            if (TempData["NOCsData"] != null)
            {
                ObjList = TempData["NOCsData"] as List<MISCitizenModel>;
                string ApplicationNos = string.Empty;
                foreach (var item in ObjList)
                    ApplicationNos = ApplicationNos + item.ApplicationNo + ",";
                ApplicationNos = ApplicationNos.Remove(ApplicationNos.Length - 1);
                DataSet DS = new DataSet();
                DataTable dtf = OBJ.GetDownLoadList("MIS_GetNOCApplicationDetails_DownLoad", ApplicationNos);
                TempData["NOCsData"] = ObjList;
                if (dtf != null)
                {
                    GridView gv = new GridView();
                    gv.DataSource = dtf;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=CSPermissionsDrillDown_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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
        public ActionResult CSPermissionsWildLifeAndForest()
        {
            ViewBag.Category_List = new SelectList(Common.GetCategory(), "Value", "Text");
            getLoadData(1);
            ViewData["ListMISCitizenWildLifeAndForestSummary"] = ListMISCitizenModelSummary;
            return View();
        }

        //Replace
        //Replace
        [HttpPost]
        public ActionResult CSPermissionsWildLifeAndForest(MISCitizenModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {

                if (MIS.Duration == "Yearly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-12).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (MIS.Duration == "Quarterly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-3).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (MIS.Duration == "Monthly")
                {
                    MIS.FromDate = DateTime.Now.Date.AddMonths(-1).ToString("dd/MM/yyyy").Substring(0, 10);
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);

                }
                else if (MIS.Duration == "Cumulative")
                {
                    MIS.FromDate = ConfigurationManager.AppSettings["ProjectStartDate"].ToString();
                    MIS.ToDate = DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 10);
                }
                else
                {

                }

                DataSet DS = new DataSet();
                StringBuilder SB = new StringBuilder();
                DS = OBJ.CSPermissionsGetData("MIS_ReportCitizenService_WildLifeAndForest", MIS.FromDate, MIS.ToDate, MIS.ResearchType, MIS.AreaCategory, MIS.PlaceForResearch, MIS.Status, "GetData");
                int count = 1;
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    ListMISCitizenModelSummary.Add(
                        new MISCitizenModel()
                        {
                            index = count,
                            RequestedId = Convert.ToString(dr["RequestedId"]),
                            RequestedType = Convert.ToString(dr["ResearchType"]),
                            PlaceForResearch = Convert.ToString(dr["PlaceForResearch"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            StatusName = Convert.ToString(dr["StatusName"]),
                            IsGORGOI = Convert.ToString(dr["IsGORGOI"]),
                        });
                    count += 1;
                }
                ViewData["ListMISCitizenWildLifeAndForestSummary"] = ListMISCitizenModelSummary;
                ViewBag.Category_List = new SelectList(Common.GetCategory(), "Value", "Text");
                getLoadData(1);
                ViewBag.PremissionSubCategory = PremissionSubCat("NOCs");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }



        public ActionResult GetNocDetail(string requestId)
        {
            Common cm = new Common();
            string origin = System.Configuration.ConfigurationManager.AppSettings["websiteUrl"].ToString() + "/PermissionDocument";
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                new SqlParameter("@RequestId", requestId),
                new SqlParameter("@UserID",Convert.ToInt64(Session["UserId"]))
            };
            cm.Fill(ds, "KN_GetResearchReqDetail", param);
            DataTable tbl = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTrail = new StringBuilder();
            StringBuilder sbCmd = new StringBuilder();
            Dictionary<string, string> cmds = new Dictionary<string, string>();
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn dc in tbl.Columns)
                {
                    string colVal = Convert.ToString(tbl.Rows[0][dc.ColumnName]);
                    if (!string.IsNullOrEmpty(colVal))
                    {
                        if (dc.ColumnName.Contains(" File"))
                        {
                            var lnk = "<a href='" + origin + "/" + colVal + "' target='_blank'  rel='noopener noreferrer'> Download </a>";
                            sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + lnk + "</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + colVal + "</td></tr>");
                        }
                    }
                }
                DataTable tblTrail = ds.Tables[1];
                sbTrail.Append("<thead>");
                foreach (DataColumn trailDc in tblTrail.Columns)
                {
                    sbTrail.Append("<th>" + trailDc.ColumnName + "</th>");
                }
                sbTrail.Append("</thead><tbody>");
                foreach (DataRow dr in tblTrail.Rows)
                {
                    sbTrail.Append("<tr>");
                    for (int i = 0; i < tblTrail.Columns.Count; i++)
                    {
                        sbTrail.Append("<td>" + dr[i] + "</td>");
                    }
                    sbTrail.Append("</tr>");
                }
                sbTrail.Append("</tbody>");
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    Session["UploadFile"] = null;
                    cmds.Add("cmd", ds.Tables[2].Rows[0]["Level"].ToString());
                    cmds.Add("cmdText", ds.Tables[2].Rows[0]["NextStep"].ToString());
                    cmds.Add("CurrentApprovalLevel", ds.Tables[2].Rows[0]["CurrentApprovalLevel"].ToString());
                }

            }
            else
            {
                cmds.Add("cmd", "0");
                cmds.Add("cmdText", "None");
            }
            ViewBag.Data = ds;
            return PartialView("_CSPermissionsWildLifeAndForest");
        }
        public JsonResult GetWildLifeAndForestRejectApplicationData(string category, string status)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISCitizenModel> ObjWildLifeAndForestSummaryDataList = new List<MISCitizenModel>();
            List<MISCitizenModel> ObjListWildLifeAndForestSummaryDataFinal = new List<MISCitizenModel>();
            try
            {
                if (TempData["WildLifeAndForestSummaryData"] != null)
                {
                    ObjWildLifeAndForestSummaryDataList = TempData["WildLifeAndForestSummaryData"] as List<MISCitizenModel>;
                    ObjListWildLifeAndForestSummaryDataFinal = ObjWildLifeAndForestSummaryDataList.Where(s => s.Category.Contains(category) && s.Status.Contains(status)).ToList();
                    TempData["WildLifeAndForestSummaryData"] = ObjWildLifeAndForestSummaryDataList;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(ObjListWildLifeAndForestSummaryDataFinal);
        }
        public ActionResult GetNOCApplicationDetails(string ids)
        {
            string[] ARR = ids.Split('_');
            StringBuilder SB = new StringBuilder();
            if (ARR.Length == 2)
            {
                DataSet DS = new DataSet();
                DS = OBJ.GetApplicationDetails("MIS_GetNOCApplicationDetails", ids.Split('_')[0].ToString(), ids.Split('_')[1].ToString());
                if (DS.Tables.Count == 2)
                {
                    Int16 index = 0;
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        while (DS.Tables[0].Columns.Count > index)
                        {
                            if (DS.Tables[0].Columns[index].ColumnName.Contains("DownloadNOC") == true)
                            {
                                if (DS.Tables[0].Rows[0]["Status"].ToString() == "Approved")
                                {
                                    SB.Append("<tr><td>");
                                    SB.Append(DS.Tables[0].Columns[index].ColumnName);
                                    SB.Append("</td><td>");
                                    SB.Append("<a href='" + Convert.ToString(DS.Tables[0].Rows[0][index]) + "' target='_blank' rel = 'noopener noreferrer'><img src='../images/jpeg.png' width='30'></a>");
                                    SB.Append("</td></tr>");
                                }
                            }
                            else if (DS.Tables[0].Columns[index].ColumnName.Contains("KML_Path") == true)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(DS.Tables[0].Rows[0][index])))
                                {
                                    SB.Append("<tr><td> KML </td><td> ");
                                    SB.Append("<a href='" + Convert.ToString(DS.Tables[0].Rows[0][index]) + "' target='_blank' rel = 'noopener noreferrer'><img src='../images/globe.png' width='30'></a>");
                                    SB.Append("</td></tr>");
                                }
                            }
                            else
                            {
                                SB.Append("<tr><td>");
                                SB.Append(DS.Tables[0].Columns[index].ColumnName);
                                SB.Append("</td><td>");
                                SB.Append(DS.Tables[0].Rows[0][index].ToString());
                                SB.Append("</td></tr>");
                            }
                            index = Convert.ToInt16(index + 1);
                        }
                    }
                    SB.Append("<tr><td>  <td><tr>");
                    SB.Append("<tr><td colspan='2'>");
                    SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                    SB.Append("<thead><tr><th>Action</th><th>Date</th><th>Pendency</th><th>ActionBy</th><th>Remarks</th></tr></thead>");
                    SB.Append("<tbody>");
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        SB.Append("<tr><td>" + Convert.ToString(dr["Action"]) + "</td><td>" + Convert.ToString(dr["Date"]) + "</td><td>" + Convert.ToString(dr["Pendency"]) + "</td><td>" + Convert.ToString(dr["ActionBy"]) + "</td><td>" + Convert.ToString(dr["ActionRemarks"]) + "</td></tr>");
                    }
                    SB.Append("</tbody></table>");

                    if (ARR.Length > 0)
                    {
                        Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                        DataTable dataTable = Model.GetHQLevelUser(UserID,ARR[0].ToString());
                        string DesignationId = "";
                        if (dataTable.Rows.Count > 0)
                            DesignationId = dataTable.Rows[0]["DesignationId"].ToString();

                        if (ARR[1].Equals("Pending") && dataTable.Rows.Count > 0 && DesignationId.Length > 0)
                        {
                            SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                            SB.Append("<thead><tr><th>Transfer NOC</th></tr></thead>");
                            SB.Append("<tbody>");
                            SB.Append("<tr><td><div class='col-md-3 col-lg-3 col-sm-3'> <button type='button' class='btn btn - info' id='btn_Transfer'  onclick='TransferNOC();'>Transfer NOC </button></div><div hidden='hidden' id='nocRequestId'>" + ARR[0] + "</div></td></tr>");
                            SB.Append("</tbody></table>");
                        }
                    }
                    SB.Append("</td></tr>");
                }
            }
            ViewBag.List = SB.ToString();
            return PartialView("ApplicationNoCurrentDetails");
        }
        public ActionResult GetWildLifeAndForestApplicationDetails(string ids)
        {
            string[] ARR = ids.Split('_');
            StringBuilder SB = new StringBuilder();
            if (ARR.Length == 2)
            {
                DataSet DS = new DataSet();
                DS = OBJ.GetApplicationDetails("MIS_GetWildLifeAndForestApplicationDetails", ids.Split('_')[0].ToString(), ids.Split('_')[1].ToString());
                if (DS.Tables.Count == 2)
                {
                    Int16 index = 0;
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        while (DS.Tables[0].Columns.Count > index)
                        {
                            SB.Append("<tr><td>");
                            SB.Append(DS.Tables[0].Columns[index].ColumnName);
                            SB.Append("</td><td>");
                            SB.Append(DS.Tables[0].Rows[0][index].ToString());
                            SB.Append("</td></tr>");
                            index = Convert.ToInt16(index + 1);
                        }
                    }
                    SB.Append("<tr><td colspan='2'>");
                    SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                    SB.Append("<thead><tr><th>Action</th><th>Date</th><th>Pendency</th><th>ActionBy</th><th>Remarks</th></tr></thead>");
                    SB.Append("<tbody>");
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        SB.Append("<tr><td>" + Convert.ToString(dr["Action"]) + "</td><td>" + Convert.ToString(dr["Date"]) + "</td><td>" + Convert.ToString(dr["Pendency"]) + "</td><td>" + Convert.ToString(dr["ActionBy"]) + "</td><td>" + Convert.ToString(dr["ActionRemarks"]) + "</td></tr>");
                    }
                    SB.Append("</tbody></table>");
                    SB.Append("</td></tr>");
                }
            }
            ViewBag.ListWildLifeAndForest = SB.ToString();
            return PartialView("ApplicationWildLifeAndForestCurrentDetails");
        }
        public ActionResult CSPermissionsWildLifeAndForestExport()
        {
            List<MISCitizenModel> ObjList = new List<MISCitizenModel>();
            if (TempData["WildLifeAndForestSummaryData"] != null)
            {
                ObjList = TempData["WildLifeAndForestSummaryData"] as List<MISCitizenModel>;
                string ApplicationNos = string.Empty;
                foreach (var item in ObjList)
                    ApplicationNos = ApplicationNos + item.ApplicationNo + ",";
                ApplicationNos = ApplicationNos.Remove(ApplicationNos.Length - 1);
                DataSet DS = new DataSet();
                DataTable dtf = OBJ.GetDownLoadList("MIS_GetWildLifeAndForestApplicationDetails_DownLoad", ApplicationNos);
                TempData["WildLifeAndForestSummaryData"] = ObjList;
                if (dtf != null)
                {
                    GridView gv = new GridView();
                    gv.DataSource = dtf;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=GetWildLifeAndForestApplicationDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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
        public ActionResult GetNOPDivisionbyStatusReport()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            NOCDivisionReportModel Parentmodel = new NOCDivisionReportModel();
            try
            {
                #region Fill DropDown
                getLoadData(1);
                getPermissionData();
                PermissionTypeCategoryData();
                #endregion
                NOCDivisionRepository obj = new NOCDivisionRepository();
                Parentmodel = obj.GetNOCReport(Parentmodel);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(Parentmodel);
        }
        [HttpPost]
        public ActionResult GetNOPDivisionbyStatusReport(NOCDivisionReportModel Parentmodel)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                #region Fill DropDown
                getLoadData(1);
                getPermissionData();
                PermissionTypeCategoryData();
                #endregion
                NOCDivisionRepository obj = new NOCDivisionRepository();
                Parentmodel = obj.GetNOCReport(Parentmodel);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(Parentmodel);
        }
        #region Get Transit Permit Citizen Report by Rajveer
        public ActionResult CitizenTransitPermitReports()
        {
            CitizenTransitPermitModel model = new CitizenTransitPermitModel();
            CitizenTransitPermitRepo repo = new CitizenTransitPermitRepo();
            DataSet ds = new DataSet();
            model = repo.GetCitizenReports(model);
            ViewBag.Division = repo.getDropdown("1");
            return View(model);
        }
        [HttpPost]
        public ActionResult CitizenTransitPermitReports(CitizenTransitPermitModel model)
        {
            CitizenTransitPermitRepo repo = new CitizenTransitPermitRepo();
            ViewBag.Division = repo.getDropdown("1");
            model = repo.GetCitizenReports(model);
            return View(model);
        }
        [HttpGet]
        public JsonResult GetCurrentCircleDivision(string RequestId)
        {
            Object CurCircleDiv = new { CIRCLE_CODE = "", CIRCLE_NAME = "Some thing Went Wrong", DIV_CODE = "", DIV_NAME = "Some thing Went Wrong", ApplicantName = "", TypeOfNoc = "" };
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataTable dt1 = Model.GetCurrentCircleDivision(RequestId);
                DataTable dt2 = Model.GetCurrentNocOtherDetail(RequestId);
                CurCircleDiv = new { CIRCLE_CODE = dt1.Rows[0]["CIRCLE_CODE"].ToString(), CIRCLE_NAME = dt1.Rows[0]["CIRCLE_NAME"].ToString(), DIV_CODE = dt1.Rows[0]["DIV_CODE"].ToString(), DIV_NAME = dt1.Rows[0]["DIV_NAME"].ToString(), ApplicantName = dt2.Rows[0]["ApplicantName"].ToString(), TypeOfNoc = dt2.Rows[0]["TypeOfNoc"].ToString() };
                //return Json(CurCircleDiv);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(CurCircleDiv, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TransferNocDivision(string RequestId, string tDiv_Code)
        {
            Object Msg = new
            {
                Msg = "Error"
            };
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataTable dataTable = Model.GetHQLevelUser(UserID, RequestId);
                string DesignationId = "";
                if (dataTable.Rows.Count > 0)
                    DesignationId = dataTable.Rows[0]["DesignationId"].ToString();

                if (dataTable.Rows.Count > 0 && DesignationId.Length > 0)
                {
                    DataTable dt1 = Model.TransferNocDivision(RequestId, tDiv_Code, UserID);
                    Msg = new { Msg = dt1.Rows[0]["Msg"].ToString() };
                }
                else
                {
                    Msg = new { Msg = "You are not authorize user to transfer NOC." };
                }
                //return Json(CurCircleDiv);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Get Amrita Devi Report by Rajveer
        public ActionResult AmritaDeviReports()
        {
            AmritaDeviReportModel models = new AmritaDeviReportModel();
            AmritaDeviReportRepo repo = new AmritaDeviReportRepo();
            DataSet ds = new DataSet();
            models = repo.GetAmritaDeviReports(models);
            return View(models);
        }
        [HttpPost]
        public ActionResult AmritaDeviReports(AmritaDeviReportModel model)
        {
            AmritaDeviReportRepo repo = new AmritaDeviReportRepo();
            model.userid = Session["UserId"].ToString();
            model = repo.GetAmritaDeviReports(model);
            return View(model);
        }
        #endregion

    }
}
