using FMDSS.Models;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace FMDSS.Controllers.MIS
{
    public class MISDevelopmentController : BaseController
    {
        List<SelectListItem> lstCircle = new List<SelectListItem>();
        List<SelectListItem> itemsRange = new List<SelectListItem>();
        List<SelectListItem> itemsDivision = new List<SelectListItem>();

        MISFPM ObjMisFpm = new MISFPM();
        CitizenModel Model = new CitizenModel();

        string status = string.Empty;
        public ActionResult MISWorkOrderAndContractDetails()
        {
            Session.Remove("DownloadMISWorkOrderAndContractDetailsExport");
            MISDevelopment ObjMISDevelopment = new MISDevelopment();

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                status = LoadData(UserID, "load", "", "", "");

                ObjMISDevelopment.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISDevelopment.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISDevelopment.RangeStatus = Convert.ToString(status.Split(',')[2]);

                // ObjMISDevelopment.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);
                List<MISDevelopment> ObjListMISDevelopment = new List<MISDevelopment>();
                ViewData["ListMISDevelopment"] = ObjListMISDevelopment;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISDevelopment);
        }
        public List<SelectListItem> FPM_HIERARCHY_LEVEL(Int64 UserID)
        {


            List<SelectListItem> items = new List<SelectListItem>();
            DataTable DT;
            DT = ObjMisFpm.GET_FPM_HIERARCHY_LEVEL(UserID);

            foreach (System.Data.DataRow dr in DT.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["CODE"].ToString() });
            }

            if (DT.Rows.Count > 0)
            {
                ViewBag.getlevel = Convert.ToString(DT.Rows[0]["CODE"]).Substring(0, 3) == "CIR" ? "Select Circle" : Convert.ToString(DT.Rows[0]["CODE"]).Substring(0, 3) == "DIV" ? "Select Division" : Convert.ToString(DT.Rows[0]["CODE"]).Substring(0, 3) == "RNG" ? "Select Range" : Convert.ToString(DT.Rows[0]["CODE"]).Substring(0, 3) == "ST0" ? "Select Head Quarter" : "";

            }

            return items;




        }

        public string LoadData(Int64 UserID, string TYPE, string Circle, string Division, string Range)
        {
            MISPurchaseHistory ObjMISPurchaseHistory = new MISPurchaseHistory();
            DataSet DS = new DataSet();
            DS = ObjMISPurchaseHistory.GET_USER_LEVEL(UserID);

            string CircleAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[0].Rows)
            {
                if (Convert.ToString(dr["CIRCLE_CODE"]).ToLower() != "select")
                {
                    CircleAllDataListCommanSprated += Convert.ToString(dr["CIRCLE_CODE"]) + ",";
                }
                lstCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["CIRCLE_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(CircleAllDataListCommanSprated) && DS.Tables[0].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                lstCircle.Insert(1, itm);
                lstCircle[1].Value = CircleAllDataListCommanSprated.Remove(CircleAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion

            ViewBag.CIRCLE = lstCircle;


            string DivisionAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[1].Rows)
            {
                DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                itemsDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && DS.Tables[1].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                itemsDivision.Insert(1, itm);
                itemsDivision[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion

            ViewBag.Division = itemsDivision;

            string RangeAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[2].Rows)
            {
                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                itemsRange.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && DS.Tables[2].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                itemsRange.Insert(1, itm);
                itemsRange[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion

            ViewBag.Range = itemsRange;

            string ReturnData = string.Empty;
            ReturnData = DS.Tables[0].Rows.Count > 1 ? "False" : "True";
            ReturnData = ReturnData + "," + (DS.Tables[1].Rows.Count > 1 || Convert.ToString(DS.Tables[1].Rows[0][0]) == "SELECT" ? "False" : "True");
            ReturnData = ReturnData + "," + (DS.Tables[2].Rows.Count > 1 || Convert.ToString(DS.Tables[2].Rows[0][0]) == "SELECT" ? "False" : "True");




            string Code = Convert.ToString(DS.Tables[3].Rows[0][0]);

            if (TYPE == "SUBMIT")
            {

                if (Code == "CIR")
                {

                    List<SelectListItem> items = new List<SelectListItem>();

                    if ((!String.IsNullOrEmpty(Circle)))
                    {

                        if (Circle == "SELECT")
                        {
                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetDivision(Convert.ToString(Circle));
                            DivisionAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                                items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 1)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items.Insert(1, itm);
                                items[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                        ViewBag.Division = items;

                    }

                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetRange(Convert.ToString(Division));


                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 1)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                           
                        }
                    }

                    ViewBag.Range = items1;
                }
                else if (Code == "DIV")
                {
                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });

                            DataTable dt = Model.GetRange(Convert.ToString(Division));

                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 1)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                    }

                    ViewBag.Range = items1;

                }
                else if (Code == "RNG")
                {

                }
                else if (Code == "ST0")
                {
                    List<SelectListItem> items = new List<SelectListItem>();

                    if ((!String.IsNullOrEmpty(Circle)))
                    {

                        if (Circle == "SELECT")
                        {
                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetDivision(Convert.ToString(Circle));

                            DivisionAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                                items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 1)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items.Insert(1, itm);
                                items[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                        ViewBag.Division = items;

                    }

                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });

                            DataTable dt = Model.GetRange(Convert.ToString(Division));

                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 1)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                    }

                    ViewBag.Range = items1;
                }
            }
            return ReturnData;
        }

        public JsonResult DivisionData(string circleCode)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();



            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if ((!String.IsNullOrEmpty(circleCode)))
                {

                    if (circleCode == "SELECT")
                    {
                        items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        DataTable dt = Model.GetDivision(Convert.ToString(circleCode));
                       string  DivisionAllDataListCommanSprated = string.Empty;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                            items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                        }
                        #region Set Comman Sprated Value in ALL field
                        if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 1)
                        {
                            SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                            items.Insert(1, itm);
                            items[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
                        }
                        #endregion
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

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(DivisionCode)))
                {
                    if (DivisionCode == "SELECT")
                    {
                        items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                    }
                    else
                    {

                        items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        DataTable dt = Model.GetRange(Convert.ToString(DivisionCode));

                       string  RangeAllDataListCommanSprated = string.Empty;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                            items.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                        }
                        #region Set Comman Sprated Value in ALL field
                        if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 1)
                        {
                            SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                            items.Insert(1, itm);
                            items[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult MISWorkOrderAndContractDetails(MISDevelopment OBJ)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                status = LoadData(UserID, "SUBMIT", OBJ.Circle, OBJ.Division, OBJ.Range);


                DataTable DT;

                DT = OBJ.BASE_DETAILS(OBJ);
                Session["DownloadMISWorkOrderAndContractDetailsExport"] = DT;
                int count = 1;
                List<MISDevelopment> ObjListMISDevelopment = new List<MISDevelopment>();
                foreach (DataRow dr in DT.Rows)
                {
                    ObjListMISDevelopment.Add(
                        new MISDevelopment()
                        {
                            Index = count,
                            RANGE_NAME = Convert.ToString(dr["RANGE_NAME"].ToString()),
                            WorkOrder_Code = Convert.ToString(dr["WorkOrder_Code"].ToString()),
                            WorkOrder_Name = Convert.ToString(dr["WorkOrder_Name"].ToString()),
                            Placeofwork = Convert.ToString(dr["Placeofwork"].ToString()),
                            IFMC_WorkOrder_Code = Convert.ToString(dr["IFMC_WorkOrder_Code"].ToString()),
                            ContractAgencyType = Convert.ToString(dr["ContractAgencyType"].ToString()),
                            WorkOrderType = Convert.ToString(dr["WorkOrderType"].ToString()),
                            EnteredOn = Convert.ToString(dr["EnteredOn"].ToString()),
                            StartDate = Convert.ToString(dr["Duration of Work Order"].ToString()),
                            WorkorderStatus = Convert.ToString(dr["WorkorderStatus"].ToString()),
                            AmountofWorkorder = Convert.ToString(dr["Amount of Work order"].ToString()),
                            SurveyReportStatus = Convert.ToString(dr["SurveyReportStatus"].ToString()),
                            WorkOrderProgressEntry = Convert.ToString(dr["WorkOrderProgressEntry"].ToString()),
                            PaymentStatus = Convert.ToString(dr["PaymentStatus"].ToString()),
                            PaymentAmount = Convert.ToString(dr["PaymentAmount"].ToString()),

                        });
                    count += 1;
                }

                OBJ.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);


                ViewData["ListMISDevelopment"] = ObjListMISDevelopment;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(OBJ);

        }

        public ActionResult CallWorkOrderDetails(string ids, string DetailsType)
        {
            StringBuilder SB = new StringBuilder();
            MISDevelopment ObjMISDevelopment = new MISDevelopment();

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            LoadData(UserID, "SUBMIT", ObjMISDevelopment.Circle, ObjMISDevelopment.Division, ObjMISDevelopment.Range);

            if (ids != string.Empty)
            {
                DataSet DS = new DataSet();

                ObjMISDevelopment.WorkOrder_Code = ids;
                ObjMISDevelopment.DetailsType = DetailsType;

                DS = ObjMISDevelopment.GetWorkorderDetails(ObjMISDevelopment);

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

                SB.Append("<tr><td colspan='2'> <b style='fort-size:14px;' > Details</b>");
                SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                SB.Append("<thead><tr>");

                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    index = 0;
                    while (DS.Tables[1].Columns.Count > index)
                    {
                        SB.Append("<th>" + DS.Tables[1].Columns[index].ColumnName + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    break;
                }

                SB.Append("</tr></thead>");
                SB.Append("<tbody><tr>");

                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    SB.Append("<tr>");
                    index = 0;
                    while (DS.Tables[1].Columns.Count > index)
                    {
                        SB.Append("<th>" + dr[index].ToString() + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    SB.Append("</tr>");
                }
                SB.Append("</tbody></table>");
                SB.Append("</td></tr>");

            }

            ViewBag.ListParivadDetails = SB.ToString();

            return PartialView("FPMParivadDetails");
        }

        public ActionResult MISWorkOrderAndContractDetailsExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISWorkOrderAndContractDetailsExport"];

            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISWorkOrderAndContractDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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


    }
}
