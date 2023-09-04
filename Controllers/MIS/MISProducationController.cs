using FMDSS.Models;
using FMDSS.Models.ForestProduction;
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

namespace FMDSS.Controllers.MIS
{
    public class MISProducationController : BaseController
    {
        CitizenModel Model = new CitizenModel();

        List<SelectListItem> lstCircle = new List<SelectListItem>();
        List<SelectListItem> itemsRange = new List<SelectListItem>();
        List<SelectListItem> itemsDivision = new List<SelectListItem>();


        MISProducationInventory ObjMISProducationInventory = new MISProducationInventory();
        MISProducationNotice ObjMISProducationNotice = new MISProducationNotice();
        MISPurchaseHistory ObjMISPurchaseHistory = new MISPurchaseHistory();

        List<SelectListItem> ListNurseryNames = new List<SelectListItem>();

        public List<SelectListItem> FPM_HIERARCHY_LEVEL(Int64 UserID)
        {
            MISFPM ObjMisFpm = new MISFPM();
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
        public List<SelectListItem> Select_Range(Int64 UserID)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            DataTable DT = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
            ViewBag.fname = DT;
            foreach (System.Data.DataRow Dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = Dr["RANGE_NAME"].ToString(), Value = Dr["RANGE_CODE"].ToString() });
            }

            if (DT.Rows.Count > 0)
            {
                ViewBag.getlevel = Convert.ToString(DT.Rows[0]["RANGE_CODE"]).Substring(0, 3) == "CIR" ? "Select Circle" : Convert.ToString(DT.Rows[0]["RANGE_CODE"]).Substring(0, 3) == "DIV" ? "Select Division" : Convert.ToString(DT.Rows[0]["RANGE_CODE"]).Substring(0, 3) == "RNG" ? "Select Range" : Convert.ToString(DT.Rows[0]["RANGE_CODE"]).Substring(0, 3) == "ST0" ? "Select Head Quarter" : "";

            }
            return items;

        }

        string status = string.Empty;


        public ActionResult MISProducationInventory()
        {

            Session.Remove("DownloadMISProducationInventoryExport");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISProducationInventory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISProducationInventory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISProducationInventory.RangeStatus = Convert.ToString(status.Split(',')[2]);


                // ObjMISProducationInventory.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);
                List<MISProducationInventory> Obj = new List<MISProducationInventory>();
                ViewData["ListMISProducationInventory"] = Obj;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISProducationInventory);

        }
        [HttpPost]
        public ActionResult MISProducationInventory(MISProducationInventory obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISProducationInventory> ObjlistMISProducationInventory = new List<MISProducationInventory>();

            try
            {


                status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);




                DataTable DT;
                DT = obj.GET_INVENTORY(obj.Range);
                int count = 1;

                Session["DownloadMISProducationInventoryExport"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ObjlistMISProducationInventory.Add(
                        new MISProducationInventory()
                        {
                            Index = count,
                            RANGE_NAME = Convert.ToString(dr["RANGE_NAME"].ToString()),
                            DEPOT_NAME = Convert.ToString(dr["DEPOT_NAME"]),
                            PRODUCETYPE = Convert.ToString(dr["PRODUCETYPE"]),
                            PRODUCTNAME = Convert.ToString(dr["PRODUCTNAME"]),
                            UNITNAME = Convert.ToString(dr["UNITNAME"]),
                            LOTCOUNT = Convert.ToString(dr["LOTCOUNT"]),
                            PRODUCE_QTY = Convert.ToString(dr["PRODUCE_QTY"]),


                        });
                    count += 1;
                }
                ViewData["ListMISProducationInventory"] = ObjlistMISProducationInventory;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }

        public ActionResult MISProducationInventoryExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISProducationInventoryExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISProducationInventory_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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

        public ActionResult MISProducationNotice()
        {

            Session.Remove("DownloadMISProducationInventoryExport");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISProducationNotice.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISProducationNotice.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISProducationNotice.RangeStatus = Convert.ToString(status.Split(',')[2]);


                ObjMISProducationNotice.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);
                List<MISProducationNotice> Obj = new List<MISProducationNotice>();
                ViewData["ListMISProducationNotice"] = Obj;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISProducationNotice);

        }
        [HttpPost]
        public ActionResult MISProducationNotice(MISProducationNotice obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISProducationNotice> ObjlistMISProducationNotice = new List<MISProducationNotice>();

            try
            {

                status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);
                DataTable DT;
                DT = obj.GET_Notice(obj);
                int count = 1;

                Session["DownloadMISProducationNoticeExport"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ObjlistMISProducationNotice.Add(
                        new MISProducationNotice()
                        {
                            Index = count,
                            RANGE_NAME = Convert.ToString(dr["RANGE_NAME"].ToString()),
                            Notice_Number = Convert.ToString(dr["Notice_Number"]),
                            Quantity = Convert.ToString(dr["Quantity"]),
                            ReservedPrice = Convert.ToString(dr["ReservedPrice"]),
                            DurationFrom = Convert.ToString(dr["Notice Duration"]),
                            NoticeApprovalStatus = Convert.ToString(dr["NoticeApprovalStatus"]),
                            NoticePublishStatus = Convert.ToString(dr["NoticePublishStatus"]),


                        });
                    count += 1;
                }
                ViewData["ListMISProducationNotice"] = ObjlistMISProducationNotice;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }

        public ActionResult MISProducationNoticeExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISProducationNoticeExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISProducationNotice_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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

        public string LoadData(Int64 UserID, string TYPE, string Circle, string Division, string Range)
        {

            DataSet DS = new DataSet();
            DS = ObjMISPurchaseHistory.GET_USER_LEVEL(UserID);


            string CIRCLEAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[0].Rows)
            {
                if (Convert.ToString(dr["CIRCLE_CODE"]).ToLower() != "select")
                {
                    CIRCLEAllDataListCommanSprated += Convert.ToString(dr["CIRCLE_CODE"]) + ",";
                }
                lstCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["CIRCLE_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(CIRCLEAllDataListCommanSprated) && DS.Tables[0].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                lstCircle.Insert(1, itm);
                lstCircle[1].Value = CIRCLEAllDataListCommanSprated.Remove(CIRCLEAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion
            ViewBag.CIRCLE = lstCircle;


            string DivisionAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[1].Rows)
            {
                if (Convert.ToString(dr["DIV_NAME"]).ToLower() != "select" && Convert.ToString(dr["DIV_NAME"]).ToLower() != "all")
                {
                    DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                }
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
            ReturnData = DS.Tables[0].Rows.Count > 1 || Convert.ToString(DS.Tables[1].Rows[0][0]) == "SELECT" ? "False" : "True";
            ReturnData = ReturnData + "," + (DS.Tables[1].Rows.Count > 1 || Convert.ToString(DS.Tables[1].Rows[0][0]) == "SELECT" ? "False" : "True");
            ReturnData = ReturnData + "," + (DS.Tables[2].Rows.Count > 1 || Convert.ToString(DS.Tables[2].Rows[0][0]) == "SELECT" ? "False" : "True");

            string Code = Convert.ToString(DS.Tables[3].Rows[0][0]);

            if (TYPE == "SUBMIT" || TYPE == "LOAD")
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
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 2)
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
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
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
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
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
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 2)
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
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
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
                        string DivisionAllDataListCommanSprated = string.Empty;
                        items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        DataTable dt = Model.GetDivision(Convert.ToString(circleCode));
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            DivisionAllDataListCommanSprated += dr["DIV_CODE"].ToString() + ",";
                            items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                        }

                        #region Set Comman Sprated Value in ALL field
                        if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated))
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
                        SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "ALL" };
                        DataTable dt = Model.GetRange(Convert.ToString(DivisionCode));
                        string RangeAllDataListCommanSprated = string.Empty;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                            items.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                        }

                        #region Set Comman Sprated Value in ALL field
                        if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated))
                        {
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

        public ActionResult MISPurchaseHistory()
        {

            Session.Remove("DownloadMISPurchaseHistoryExport");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);


                // DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE();

                List<MISPurchaseHistory> Obj = new List<MISPurchaseHistory>();
                ViewData["ListMISPurchaseHistory"] = Obj;
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISPurchaseHistory);

        }
        [HttpPost]
        public ActionResult MISPurchaseHistory(MISPurchaseHistory obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISPurchaseHistory> ObjlistMISPurchaseHistory = new List<MISPurchaseHistory>();

            try
            {

                status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);

                // obj.HIERARCHY_LEVEL_CODE = Select_Range(UserID);

                DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(obj.Range);
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                string NurseryAllDataListCommanSprated = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }
                #region Set Comman Sprated Value in ALL field
                if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                {
                    ListNurseryNames.Insert(1, itm);
                    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                }
                #endregion
                ViewBag.NurseryName = ListNurseryNames;

                DataTable DT;
                DT = obj.GET_NURSERY_INVENTORY();
                int count = 1;

                Session["DownloadMISPurchaseHistoryExport"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ObjlistMISPurchaseHistory.Add(
                        new MISPurchaseHistory()
                        {
                            Index = count,
                            OrderNo = Convert.ToString(dr["OrderNo"].ToString()),
                            NurseryName = Convert.ToString(dr["NurseryName"]),
                            ProduceType = Convert.ToString(dr["ProduceType"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            PurchaseQuantity = Convert.ToString(dr["PurchaseQuantity"]),
                            RatePerItem = Convert.ToString(dr["RatePerItem"]),
                            PaidAmount = Convert.ToString(dr["PaidAmount"]),
                            Ssoid = Convert.ToString(dr["Ssoid"]),
                            Discount = Convert.ToString(dr["Discount"]),
                            SaleDistributedStatus = Convert.ToString(dr["SaleDistributedStatus"]),
                            
                        });
                    count += 1;
                }
                ViewData["ListMISPurchaseHistory"] = ObjlistMISPurchaseHistory;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }

        #region Purchase History Citizen Users
        public ActionResult MISPurchaseHistoryCitizen()
        {

            Session.Remove("DownloadMISPurchaseHistoryExport");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                //status = LoadData(UserID, "LOAD", "", "", "");

                //ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                //ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                //ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);


                // DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE();

                List<MISPurchaseHistory> Obj = new List<MISPurchaseHistory>();
                ViewData["ListMISPurchaseHistoryCitizen"] = Obj;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISPurchaseHistory);

        }
        [HttpPost]
        public ActionResult MISPurchaseHistoryCitizen(MISPurchaseHistory obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISPurchaseHistory> ObjlistMISPurchaseHistory = new List<MISPurchaseHistory>();

            try
            {

                //status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);

                //// obj.HIERARCHY_LEVEL_CODE = Select_Range(UserID);

                //DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(obj.Range);
                //SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                //string NurseryAllDataListCommanSprated = string.Empty;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                //    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                //}
                //#region Set Comman Sprated Value in ALL field
                //if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                //{
                //    ListNurseryNames.Insert(1, itm);
                //    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                //}
                //#endregion
                //ViewBag.NurseryName = ListNurseryNames;

                #region CHeck Role

                ProducePurchase pp = new ProducePurchase();
                pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToInt32(Session["CurrentRoleID"]) != 8 && pp.GetNurseryInchargeOrNot(Convert.ToInt64(Session["UserId"].ToString())) > 0)
                {
                    pp.IsInChargeOrCitizen = 'I';
                }
                #endregion


                DataTable DT;
                DT = obj.GET_NURSERY_INVENTORYCitizen(UserID, pp.IsInChargeOrCitizen);
                int count = 1;

                Session["DownloadMISPurchaseHistoryExport"] = DT;

                foreach (DataRow dr in DT.Rows)
                {
                    ObjlistMISPurchaseHistory.Add(
                        new MISPurchaseHistory()
                        {
                            Index = count,
                            OrderNo = Convert.ToString(dr["OrderNo"].ToString()),
                            NurseryName = Convert.ToString(dr["NurseryName"]),
                            ProduceType = Convert.ToString(dr["ProduceType"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            PurchaseQuantity = Convert.ToString(dr["PurchaseQuantity"]),
                            RatePerItem = Convert.ToString(dr["RatePerItem"]),
                            PaidAmount = Convert.ToString(dr["PaidAmount"]),
                            Ssoid = Convert.ToString(dr["Ssoid"]),
                            Discount = Convert.ToString(dr["Discount"]),
                        });
                    count += 1;
                }
                ViewData["ListMISPurchaseHistoryCitizen"] = ObjlistMISPurchaseHistory;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }
        #endregion


        public ActionResult MISPurchaseHistoryExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISPurchaseHistoryExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISPurchaseHistory_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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

        [HttpPost]
        public JsonResult GETNURSERYSBYRANGE(string RANGE)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ObjMISPurchaseHistory.HIERARCHY_LEVEL_CODE = Select_Range(UserID);

            List<SelectListItem> NurseryName = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(RANGE))
                {

                    ObjMISPurchaseHistory.HIERARCHY_CODE = RANGE;
                    DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(ObjMISPurchaseHistory.HIERARCHY_CODE);

                    string NurseryAllDataListCommanSprated = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["NURSERY_CODE"].ToString().ToLower() != "0")
                        {
                            NurseryAllDataListCommanSprated += dr["NURSERY_CODE"].ToString() + ",";
                        }
                        NurseryName.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                    }
                    #region Set Comman Sprated Value in ALL field
                    if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                    {
                        SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "ALL" };
                        NurseryName.Insert(1, itm);
                        NurseryName[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(new SelectList(NurseryName, "Value", "Text"));
        }


        public ActionResult MISNurseryInventoryDetails()
        {

            Session.Remove("ListMISNurseryInventoryDetails");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);


                // DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE();

                List<MISPurchaseHistory> Obj = new List<MISPurchaseHistory>();
                ViewData["ListMISNurseryInventoryDetails"] = Obj;
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISPurchaseHistory);

        }
        [HttpPost]
        public ActionResult MISNurseryInventoryDetails(MISPurchaseHistory obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISPurchaseHistory> ObjlistMISPurchaseHistory = new List<MISPurchaseHistory>();

            try
            {

                status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);

                // obj.HIERARCHY_LEVEL_CODE = Select_Range(UserID);

                DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(obj.Range);

                string NurseryAllDataListCommanSprated = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }
                #region Set Comman Sprated Value in ALL field
                if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                {
                    SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                    ListNurseryNames.Insert(1, itm);
                    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                }
                #endregion

                ViewBag.NurseryName = ListNurseryNames;

                DataTable DT;
                DataTable dt2=new DataTable();
                dt2.Columns.Add("SNo");
                dt2.Columns.Add("Circle");
                dt2.Columns.Add("Division");
                dt2.Columns.Add("Range");
                dt2.Columns.Add("Nursery Name");
                dt2.Columns.Add("Produce Type");
                dt2.Columns.Add("Product Name");
                dt2.Columns.Add("Qty Sales/Distribution");
                dt2.Columns.Add("Qty For Plantation Use");
                dt2.Columns.Add("Total Stock");
                dt2.Columns.Add("Distribution Stock Utilized");
                dt2.Columns.Add("Plantation Stock Utilized");
                dt2.Columns.Add("Total Remaining Stock");

                DT = obj.GET_NURSERY_Stock();
                int count = 1;

                DataRow rw;
                foreach (DataRow row in DT.Rows)
                {
                    rw = dt2.NewRow();
                    rw["SNo"] = count;
                    rw["Circle"] = row["Circle_Name"].ToString();
                    rw["Division"] =row["DIV_NAME"].ToString();
                    rw["Range"] = row["RANGE_NAME"].ToString();
                    rw["Nursery Name"] = row["NurseryName"].ToString();
                    rw["Produce Type"] = row["ProduceType"].ToString();
                    rw["Product Name"] = row["ProductName"].ToString();
                    rw["Qty Sales/Distribution"] = row["Citizen_StockTotal"].ToString();
                    rw["Qty For Plantation Use"] = row["Dept_StockTotal"].ToString();
                    rw["Total Stock"] = Convert.ToInt32(row["Citizen_StockTotal"]) + Convert.ToInt32(row["Dept_StockTotal"]);
                    rw["Distribution Stock Utilized"] = row["Citizen_StockOut"].ToString();
                    rw["Plantation Stock Utilized"] = row["Dept_StockOut"].ToString();
                    rw["Total Remaining Stock"] = (Convert.ToInt32(row["Citizen_StockTotal"]) + Convert.ToInt32(row["Dept_StockTotal"])) - (Convert.ToInt32(row["Citizen_StockOut"]) + Convert.ToInt32(row["Dept_StockOut"]));
                    dt2.Rows.Add(rw);
                    count += 1;
                }

                    Session["DownloadMISNurseryInventoryDetailsExport"] = dt2;
                count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ObjlistMISPurchaseHistory.Add(
                        new MISPurchaseHistory()
                        {
                            Index = count,
                            Circle = Convert.ToString(dr["Circle_Name"]),
                            Division = Convert.ToString(dr["DIV_NAME"]),
                            Range = Convert.ToString(dr["RANGE_NAME"]),

                            NurseryName = Convert.ToString(dr["NurseryName"]),
                            ProduceType = Convert.ToString(dr["ProduceType"]),
                            ProductName = Convert.ToString(dr["ProductName"]),

                           

                            Citizen_StockTotal = Convert.ToInt32(dr["Citizen_StockTotal"]),
                            Dept_StockTotal = Convert.ToInt32(dr["Dept_StockTotal"]),
                            Citizen_StockOut = Convert.ToInt32(dr["Citizen_StockOut"]),
                            Dept_StockOut = Convert.ToInt32(dr["Dept_StockOut"]),
                            TotalStock = Convert.ToInt32(dr["Citizen_StockTotal"]) + Convert.ToInt32(dr["Dept_StockTotal"]),
                            RemaingStock= (Convert.ToInt32(dr["Citizen_StockTotal"]) + Convert.ToInt32(dr["Dept_StockTotal"]))-(Convert.ToInt32(dr["Citizen_StockOut"])+ Convert.ToInt32(dr["Dept_StockOut"]))
                        });
                    count += 1;
                }
                ViewData["ListMISNurseryInventoryDetails"] = ObjlistMISPurchaseHistory;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }

        public ActionResult MISNurseryInventoryDetailsExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISNurseryInventoryDetailsExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISNurseryInventoryDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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




        #region Create  Nursery Reports Developed by Rajveer

        public ActionResult MISNurseryReportDetails()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                MISNursueryRepo repo = new MISNursueryRepo();
                ViewData["ListMISNurseryInventoryDetailsReport1"] = repo.GetNursuryReport1(model,"REPORT1");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryReportDetails(MISNursueryModel model)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD",model.Circle, model.Division, model.Range);

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                #region Get Nursery List
                DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(model.Range);
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                string NurseryAllDataListCommanSprated = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }
                #region Set Comman Sprated Value in ALL field
                if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                {
                    ListNurseryNames.Insert(1, itm);
                    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                }
                #endregion
                ViewBag.NurseryName = ListNurseryNames;
                #endregion


                MISNursueryRepo repo = new MISNursueryRepo();
                ViewData["ListMISNurseryInventoryDetailsReport1"] = repo.GetNursuryReport1(model, "REPORT1");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }


        public ActionResult MISNurseryReportInfo()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                MISNursueryRepo repo = new MISNursueryRepo();
                ViewData["ListMISNurseryInventoryDetailsReport2"] = repo.GetNursuryReport1(model, "REPORT3");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryReportInfo(MISNursueryModel model)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD", model.Circle, model.Division, model.Range);

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                #region Get Nursery List
                DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(model.Range);
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                string NurseryAllDataListCommanSprated = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }
                #region Set Comman Sprated Value in ALL field
                if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                {
                    ListNurseryNames.Insert(1, itm);
                    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                }
                #endregion
                ViewBag.NurseryName = ListNurseryNames;
                #endregion

                MISNursueryRepo repo = new MISNursueryRepo();
                ViewData["ListMISNurseryInventoryDetailsReport2"] = repo.GetNursuryReport1(model, "REPORT3");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }



        public ActionResult MISNurseryReportSummary()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                MISNursueryRepo repo = new MISNursueryRepo();
                TempData["ListMISNurseryInventoryDetailsReport3"] = repo.GetNursuryReport2(model, "REPORT2");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryReportSummary(MISNursueryModel model)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            model.UserID = UserID;
            try
            {
                status = LoadData(UserID, "LOAD", model.Circle, model.Division, model.Range);

                ObjMISPurchaseHistory.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISPurchaseHistory.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISPurchaseHistory.RangeStatus = Convert.ToString(status.Split(',')[2]);

                #region Get Nursery List
                DataTable dt = ObjMISPurchaseHistory.Select_NURSERYSBYRANGE(model.Range);
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                string NurseryAllDataListCommanSprated = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    NurseryAllDataListCommanSprated += Convert.ToString(dr["NURSERY_CODE"]) + ",";
                    ListNurseryNames.Add(new SelectListItem { Text = @dr["NurseryName"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }
                #region Set Comman Sprated Value in ALL field
                if (!string.IsNullOrEmpty(NurseryAllDataListCommanSprated))
                {
                    ListNurseryNames.Insert(1, itm);
                    ListNurseryNames[1].Value = NurseryAllDataListCommanSprated.Remove(NurseryAllDataListCommanSprated.Length - 1, 1);
                }
                #endregion
                ViewBag.NurseryName = ListNurseryNames;
                #endregion

                MISNursueryRepo repo = new MISNursueryRepo();
                TempData["ListMISNurseryInventoryDetailsReport3"] = repo.GetNursuryReport2(model, "REPORT2");
                ViewBag.NurseryName = ListNurseryNames;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }
            return View(model);
        }


        public ActionResult MISNurseryInventoryLogReport()
        {
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISInventoryLog"] = repo.GetNursuryInventoryLogsReport(model,"ALL");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryInventoryLogReport(MISNursueryModel model)
        {
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());
            
            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISInventoryLog"] = repo.GetNursuryInventoryLogsReport(model, "ALL");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }


        public ActionResult MISNurseryOpeningCloseingReport()
        {
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISInventoryOpeningLog"] = repo.GetNursuryInventoryLogsReport(model, "OpeningCloseingReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryOpeningCloseingReport(MISNursueryModel model)
        {
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());

            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISInventoryOpeningLog"] = repo.GetNursuryInventoryLogsReport(model, "OpeningCloseingReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }


        public ActionResult MISNurseryDayWiseReport()
        {
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISDayWiseReport"] = repo.GetNursuryInventoryLogsReport(model, "DayWiseReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryDayWiseReport(MISNursueryModel model)
        {
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());

            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISDayWiseReport"] = repo.GetNursuryInventoryLogsReport(model, "DayWiseReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }


        public ActionResult MISNurseryHeadWiseReport()
        {
            MISNursueryModel model = new MISNursueryModel();
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());
            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISHeadWiseReport"] = repo.GetNursuryInventoryLogsReport(model, "HeadWiseReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MISNurseryHeadWiseReport(MISNursueryModel model)
        {
            model.UserID = Convert.ToInt64(Session["UserID"].ToString());

            MISNursueryRepo repo = new MISNursueryRepo();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                List<SelectListItem> Range = new List<SelectListItem>();
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();
                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                ViewBag.NurseryName = Range;
                ViewData["ListMISHeadWiseReport"] = repo.GetNursuryInventoryLogsReport(model, "HeadWiseReport");
                ViewBag.StockList = repo.GetStockNameList("GETStockName", model.UserID);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, model.UserID);
            }
            return View(model);
        }
        #endregion
    }
}
