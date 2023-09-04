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
using FMDSS.Models.BookOnlineZoo;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.ForestFire;
using FMDSS.Repository.Interface;
using FMDSS.Repository;

namespace FMDSS.Controllers.MIS
{
    public class MISFPMController : BaseController
    {
        List<SelectListItem> lstCircle = new List<SelectListItem>();
        List<SelectListItem> itemsRange = new List<SelectListItem>();
        List<SelectListItem> itemsDivision = new List<SelectListItem>();

        MISFPM ObjMisFpm = new MISFPM();

        CitizenModel Model = new CitizenModel();

        string status = string.Empty;

        public ActionResult MISParivadDetailsStatus()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMisFpm.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMisFpm.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMisFpm.RangeStatus = Convert.ToString(status.Split(',')[2]);


                //ObjMisFpm.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);
                List<MISFPM> ObjListMisFpm = new List<MISFPM>();
                ViewData["ListMISParivadDetailsStatus"] = ObjListMisFpm;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMisFpm);
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
                    CircleAllDataListCommanSprated += Convert.ToString(dr["CIRCLE_CODE"]) + ",";
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
                if (Convert.ToString(dr["DIV_CODE"]).ToLower() != "select")
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
                if (Convert.ToString(dr["RANGE_CODE"]).ToLower() != "select")
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
                                if (Convert.ToString(dr["DIV_CODE"]).ToLower() != "select")
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
                                if (Convert.ToString(dr["RANGE_CODE"]).ToLower() != "select")
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
                                if (Convert.ToString(dr["RANGE_CODE"]).ToLower() != "select")
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
                                if (Convert.ToString(dr["DIV_CODE"]).ToLower() != "select")
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
                                if (Convert.ToString(dr["RANGE_CODE"]).ToLower() != "select")
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
                       string DivisionAllDataListCommanSprated = string.Empty;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            if (Convert.ToString(dr["DIV_CODE"]).ToLower() != "select")
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
                       string RangeAllDataListCommanSprated = string.Empty;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            if (Convert.ToString(dr["RANGE_CODE"]).ToLower() != "select")
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
        public ActionResult MISParivadDetailsStatus(MISFPM OBJ)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                status = LoadData(UserID, "SUBMIT", OBJ.Circle, OBJ.Division, OBJ.Range);


                DataTable DT;

                DT = ObjMisFpm.BASE_DETAILS(OBJ);

                int count = 1;
                List<MISFPM> ObjListMisFpm = new List<MISFPM>();
                foreach (DataRow dr in DT.Rows)
                {
                    ObjListMisFpm.Add(
                        new MISFPM()
                        {
                            Index = count,
                            OffenseStatus = Convert.ToString(dr["OffenseStatus"].ToString()),
                            OffenseStatusText = Convert.ToString(dr["OffenseStatusText"].ToString()),

                        });
                    count += 1;
                }

                //   OBJ.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);


                ViewData["ListMISParivadDetailsStatus"] = ObjListMisFpm;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(OBJ);

        }

        public JsonResult GetApplicationNo(string HIERARCHY_CODE, string FromDate, string ToDate, string OffenseStatus)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISFPM> ObjList = new List<MISFPM>();


            try
            {

                DataTable DT;
                ObjMisFpm.HIERARCHY_CODE = HIERARCHY_CODE;
                ObjMisFpm.FromDate = FromDate;
                ObjMisFpm.ToDate = ToDate;
                ObjMisFpm.OffenseStatus = OffenseStatus;

                DT = ObjMisFpm.GET_OFFENSE_CODE(ObjMisFpm);

                int count = 1;

                foreach (DataRow dr in DT.Rows)
                {
                    ObjList.Add(
                        new MISFPM()
                        {
                            Index = count,
                            OFFENSE_CODE = Convert.ToString(dr["OFFENSE_CODE"].ToString()),
                            AssignTo = Convert.ToString(dr["Assign To"].ToString()),
                            AssignDate = Convert.ToString(dr["Assign Date"].ToString()),
                        });
                    count += 1;
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(ObjList);
        }

        public ActionResult GetApplicationDetails(string ids)
        {
            StringBuilder SB = new StringBuilder();
            if (ids != string.Empty)
            {
                DataSet DS = new DataSet();

                ObjMisFpm.OFFENSE_CODE = ids;
                DS = ObjMisFpm.COMPLETE_DETAILS(ObjMisFpm);



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

                SB.Append("<tr><td colspan='2'> <b style='fort-size:14px;' > Vehicle Details</b>");
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

                //======================================================

                SB.Append("<tr><td colspan='2'><b style='fort-size:14px;' > Offender Details</b>");
                SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                SB.Append("<thead><tr>");

                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    index = 0;
                    while (DS.Tables[2].Columns.Count > index)
                    {
                        SB.Append("<th>" + DS.Tables[2].Columns[index].ColumnName + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    break;
                }

                SB.Append("</tr></thead>");
                SB.Append("<tbody><tr>");

                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    SB.Append("<tr>");
                    index = 0;
                    while (DS.Tables[2].Columns.Count > index)
                    {
                        SB.Append("<th>" + dr[index].ToString() + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    SB.Append("</tr>");

                }
                SB.Append("</tbody></table>");
                SB.Append("</td></tr>");

                //======================================================

                SB.Append("<tr><td colspan='2'><b style='fort-size:14px;' > Compounding Details</b>");
                SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                SB.Append("<thead><tr>");

                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    index = 0;
                    while (DS.Tables[3].Columns.Count > index)
                    {
                        SB.Append("<th>" + DS.Tables[3].Columns[index].ColumnName + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    break;
                }

                SB.Append("</tr></thead>");
                SB.Append("<tbody><tr>");

                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    SB.Append("<tr>");
                    index = 0;
                    while (DS.Tables[3].Columns.Count > index)
                    {
                        SB.Append("<th>" + dr[index].ToString() + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    SB.Append("</tr>");

                }
                SB.Append("</tbody></table>");
                SB.Append("</td></tr>");

                //======================================================

                SB.Append("<tr><td colspan='2'><b style='fort-size:14px;' > Court Case Details</b>");
                SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                SB.Append("<thead><tr>");

                foreach (DataRow dr in DS.Tables[4].Rows)
                {
                    index = 0;
                    while (DS.Tables[4].Columns.Count > index)
                    {
                        SB.Append("<th>" + DS.Tables[4].Columns[index].ColumnName + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    break;
                }

                SB.Append("</tr></thead>");
                SB.Append("<tbody><tr>");

                foreach (DataRow dr in DS.Tables[4].Rows)
                {
                    SB.Append("<tr>");
                    index = 0;
                    while (DS.Tables[4].Columns.Count > index)
                    {
                        SB.Append("<th>" + dr[index].ToString() + "</th>");
                        index = Convert.ToInt16(index + 1);
                    }
                    SB.Append("</tr>");

                }
                SB.Append("</tbody></table>");
                SB.Append("</td></tr>");

                //======================================================
            }

            ViewBag.ListParivadDetails = SB.ToString();

            return PartialView("FPMParivadDetails");
        }

        #region Forest Fire Report
        private ICommonRepository _commonRepository;
        private IProtectionRepository _ProtectionRepository;
        int ModuleID = 4;
        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }
        #region [Constructor]
        public MISFPMController()
        {
            _commonRepository = new CommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }
        #endregion


        public ActionResult ForestFire_Report()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var dsData = _commonRepository.GetDropdownData2(14, string.Empty);
                ViewBag.FinacialYearList = _commonRepository.GetDropdownData(13, string.Empty);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    ViewBag.CircleList = dsData.Tables[0].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("CIRCLE_CODE"),
                        Text = x.Field<string>("CIRCLE_NAME")
                    });

                    if (Globals.Util.isValidDataSet(dsData, 1, true))
                    {
                        ViewBag.DivList = dsData.Tables[1].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("DIV_CODE"),
                            Text = x.Field<string>("DIV_NAME")
                        });

                        if (Globals.Util.isValidDataSet(dsData, 2, true))
                        {
                            ViewBag.RangeList = dsData.Tables[2].AsEnumerable().Select(x => new SelectListItem
                            {
                                Value = x.Field<string>("RANGE_CODE"),
                                Text = x.Field<string>("RANGE_NAME")
                            });

                            if (Globals.Util.isValidDataSet(dsData, 3, true))
                            {
                                ViewBag.NakaList = dsData.Tables[3].AsEnumerable().Select(x => new SelectListItem
                                {
                                    Value = Convert.ToString(x.Field<long>("NakaID")),
                                    Text = x.Field<string>("NakaName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        [HttpPost]
        public ActionResult ForestFire_Report(ForestFire_AddDetailsReport param)
        {
            ForestFire_AddDetailsVM_Total model = new ForestFire_AddDetailsVM_Total();
            DataSet ds = _ProtectionRepository.ForestFire_AddDetailsReport(param);
            if (Globals.Util.isValidDataSet(ds))
            {
                var oDetails = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM>(ds, 0);
                model = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM_Total>(ds, 1).FirstOrDefault();
                model.ForestFire_AddDetailsVMReportList = oDetails;
                //model.ForestFire_AddDetailsVMReportList = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM>(ds, 0);
                return PartialView("_ForestFireSummaryReport", model);
            }
            return null;
        }
        #endregion


    }
}
