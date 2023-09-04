//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Notice Number
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//Bug No-393,406,407,413,414,415,421,432

//*********************************************************************************************************@


using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace FMDSS.Controllers.ForestProduction
{
    public class ManageNoticeController : BaseController
    {
        //
        // GET: /ManageNotice/
        Location location = new Location();
        SMS_EMail_Services _objMail = new SMS_EMail_Services();
        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> produce = new List<SelectListItem>();
        NoticeManagement obj_notice = new NoticeManagement();
        Int64 UserID = 0;
        int ModuleID = 3;
        /// <summary>
        /// Call when request come from ManageNotice view Bind Region  dropdown
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageNotice()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<NoticeManagement> result = new List<NoticeManagement>();
            List<SelectListItem> itemsrange = new List<SelectListItem>();
            List<SelectListItem> itemsregion = new List<SelectListItem>();
            try
            {
                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }

                DataTable dtr = new Common().Select_Range(UserID);
                foreach (System.Data.DataRow dr in dtr.Rows)
                {
                    itemsrange.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.RangeCode = itemsrange;
                ViewBag.RangeCode = new FMDSS.Models.ForestDevelopment.TransitPermit().SetDropdownData(6, string.Empty);

                #region Bind Notice Detail

                NoticeManagement noticeManagement = null;
                DataTable noticedt = obj_notice.BindNoticeData("OTHERPRODUCT");
                if (noticedt != null)
                {
                    if (noticedt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in noticedt.Rows)
                        {
                            noticeManagement = new NoticeManagement()
                            {

                                NoticeId = Convert.ToInt64(dr["Id"].ToString()),
                                NoticeNo = dr["Notice_Number"].ToString(),
                                No_Status = dr["Notice_Status"].ToString(),
                                ReqAction = dr["StatusDesc"].ToString(),
                                InventoryID = dr["InventoryID"].ToString(),
                                DurationTo = Convert.ToDateTime(dr["DurationTo"]),
                                DepotName = dr["Depot_Name"].ToString()
                            };
                            result.Add(noticeManagement);
                        }

                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View(result);

        }

        public ActionResult GetDODProductDetails(string parentID)
        {
            NoticeManagement model = new NoticeManagement();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {

                if (!string.IsNullOrEmpty(parentID))
                {
                    DataTable dt = new FMDSS.Models.ForestDevelopment.TransitPermit().GetDetailsByInventory(parentID);
                    model.DODProductList = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dt);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return PartialView("_NoticeProductDetails", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageTendupattaNotice()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> itemsrange = new List<SelectListItem>();
            List<NoticeManagement> result = new List<NoticeManagement>();
            try
            {
                #region Region
                DataTable dtr = new Common().Select_Range(UserID);
                ViewBag.fname1 = dtr;
                foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                {
                    itemsrange.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.RangeCode = itemsrange;
                #endregion


                #region Bind Notice Detail

                NoticeManagement noticeManagement = null;
                DataTable noticedt = obj_notice.BindNoticeData("TENDUPATTA");
                if (noticedt != null)
                {
                    if (noticedt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in noticedt.Rows)
                        {


                            noticeManagement = new NoticeManagement()
                            {

                                NoticeId = Convert.ToInt64(dr["Id"].ToString()),
                                NoticeNo = dr["Notice_Number"].ToString(),
                                Rangename = dr["RANGE_NAME"].ToString(),
                                No_Status = dr["Notice_Status"].ToString(),
                                ReqAction = dr["StatusDesc"].ToString(),
                                IsActive = Convert.ToInt32(dr["IsActive"].ToString()),
                                ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString()),

                            };
                            result.Add(noticeManagement);
                        }

                    }
                }
                #endregion


            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return View(result);
        }

        public ActionResult GetSiteInfo(string rangeCode)
        {
            NoticeManagement _objNotice = null;
            List<NoticeManagement> result = new List<NoticeManagement>();
            if (!String.IsNullOrEmpty(rangeCode))
            {

                DataTable dt = obj_notice.GetSiteDetail(rangeCode);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _objNotice = new NoticeManagement()
                            {
                                SiteID = Int64.Parse(dr["ID"].ToString()),
                                SiteName = dr["Site_Name"].ToString(),
                                Villagename = dr["VillageName"].ToString(),

                            };
                            result.Add(_objNotice);


                        }
                    }
                }
            }

            var TenduPattaPartialView = RenderRazorViewToString(this.ControllerContext, "TenduPattaSite", result);
            var json = Json(new { TenduPattaPartialView });
            return json;
        }

        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// Save Notice data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SubmitTendupatta(List<NoticeManagement> siteList, FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null;
            TempData["No_Status"] = null;

            try
            {

                NoticeManagement notice = new NoticeManagement();
                DataTable dtSite = new DataTable();
                dtSite.Columns.Add("Site_ID", typeof(Int64));
                dtSite.Columns.Add("Site_price", typeof(double));

                if (siteList != null && siteList.Count > 0)
                {
                    foreach (NoticeManagement site in siteList)
                    {
                        bool IsCheckedsite = site.IsCheckedSite;
                        if (IsCheckedsite)
                        {
                            DataRow dtrow = dtSite.NewRow();
                            dtrow["Site_ID"] = Convert.ToInt64(site.SiteID);
                            dtrow["Site_price"] = Convert.ToInt64(site.SitePrice);
                            dtSite.Rows.Add(dtrow);
                        }
                    }
                }

                if (Session["UserID"] != null)
                {

                    notice.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["RangeCode"].ToString() == "")
                {
                    notice.RangeCode = "";
                }
                else
                {
                    notice.RangeCode = fm["RangeCode"].ToString();
                }

                if (fm["Durationfrom"].ToString() == "")
                {
                    notice.DurationFrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    notice.DurationFrom = DateTime.ParseExact(fm["Durationfrom"].ToString(), "dd/MM/yyyy", null);

                }
                if (fm["Durationto"].ToString() == "")
                {
                    notice.DurationTo = Convert.ToDateTime(SqlDateTime.Null);

                }
                else
                {
                    notice.DurationTo = DateTime.ParseExact(fm["Durationto"].ToString(), "dd/MM/yyyy", null);

                }

                if (fm["ReservedPrice"].ToString() == "")
                {
                    notice.ReservedPrice = 0;
                }
                else
                {
                    notice.ReservedPrice = Convert.ToDecimal(fm["ReservedPrice"].ToString());
                }

                if (Command == "Submit")
                {
                    notice.ActionFlag = "INSERT";
                    string status = notice.CreateNotice(dtSite);
                    if (!String.IsNullOrEmpty(status))
                    {
                        Session["SchedularDetails"] = null;
                        TempData["No_Status"] = "Notice No:#" + status + " Created Successfully";

                        //if (Session["SSODetail"] != null)
                        //{
                        //    UserProfile user = (UserProfile)Session["SSODetail"];

                        //    if (user != null)
                        //    {
                        //        if (!String.IsNullOrEmpty(user.EmailId) && !String.IsNullOrEmpty(user.MobileNumber))
                        //        {
                        //            string smsBody = Common.GenerateSMSBody(Session["User"].ToString(), notice.RequestId, "Notice");
                        //            SMS_EMail_Services.sendSingleSMS(user.MobileNumber, smsBody);
                        //            string CitizenMailBody = Common.GenerateBody(0, Session["User"].ToString(), notice.RequestId, "Notice");
                        //            _objMail.sendEMail("Request for " + "Notice" + " Creation", CitizenMailBody, user.EmailId.ToString(), "");

                        //        }
                        //    }

                        //}

                    }
                    else
                    {
                        TempData["No_Status"] = "Not inserted Because for Auction date:" + notice.FinalDate + " Notice Already Created.";
                    }

                    actionResult = RedirectToAction("ManageTendupattaNotice", "ManageNotice");
                }

                if (Command == "Update")
                {
                    notice.ActionFlag = "UPDATE";
                    string status = notice.CreateNotice();
                    if (!String.IsNullOrEmpty(status))
                    {
                        TempData["No_Status"] = "Notice No:#" + status + "Updated Successfully";
                    }
                    else
                    {
                        TempData["No_Status"] = "Not Updated";
                    }

                    actionResult = RedirectToAction("ManageTendupattaNotice", "ManageNotice");
                }

                if (Command == "Re-New")
                {
                    notice.ActionFlag = "RENOTICE";
                    string status = notice.CreateNotice();
                    if (!String.IsNullOrEmpty(status))
                    {
                        TempData["No_Status"] = "Notice No:#" + status + "Re-Newed Successfully"; ;
                    }
                    else
                    {
                        TempData["No_Status"] = "Not Re-Newed";
                    }

                    actionResult = RedirectToAction("ManageTendupattaNotice", "ManageNotice");
                }



                if (Command == "Cancel")
                {
                    actionResult = RedirectToAction("Dashboard", "Dashboard");
                }
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;

        }



        public ActionResult ManageAuctionSchedular()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<NoticeManagement> result = new List<NoticeManagement>();

            try
            {
                #region Region
                DataTable dt = new DataTable();
                dt = location.BindRegion();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["REG_NAME"].ToString(), Value = @dr["REG_CODE"].ToString() });
                }

                ViewBag.RegionCode = items;
                // ViewBag.ToLocation = items;
                #endregion


            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return View();

        }

        public JsonResult BindGridData(NoticeManagement notice)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (notice != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = "Auction_Notice_Schedular" + DateTime.Now.Ticks.ToString();

                    if (Session["AuNoticeschedularInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("AuNoticeschedularInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["AuNoticeschedularInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["AuNoticeschedularInfo"].ToString() + ".xml"));
                    }

                    XmlElement schedularInfo = xmldoc.CreateElement("SchedularDetails");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    //XmlElement regionCode = xmldoc.CreateElement("RegionCode");
                    //XmlElement circleCode = xmldoc.CreateElement("CircleCode");
                    //XmlElement divisionCode = xmldoc.CreateElement("DivisionCode");
                    //XmlElement rangeCode = xmldoc.CreateElement("RangeCode");
                    //XmlElement depotId = xmldoc.CreateElement("DepotId");
                    //XmlElement forestProduceID = xmldoc.CreateElement("ForestProduceID");
                    //XmlElement forestProductID = xmldoc.CreateElement("ForestProductID");
                    //XmlElement qty = xmldoc.CreateElement("Qty");
                    XmlElement durationfrom = xmldoc.CreateElement("Durationfrom");
                    XmlElement durationto = xmldoc.CreateElement("Durationto");
                    //XmlElement reservedPrice = xmldoc.CreateElement("ReservedPrice");

                    ID.InnerText = notice.ID.ToString();
                    //regionCode.InnerText = notice.RegionCode;
                    //circleCode.InnerText = notice.CircleCode;
                    //divisionCode.InnerText = notice.DivisionCode;
                    //rangeCode.InnerText = notice.RangeCode;
                    // depotId.InnerText = Convert.ToString(notice.DepotId);
                    //forestProduceID.InnerText = Convert.ToString(notice.ForestProduceID);
                    //forestProductID.InnerText = Convert.ToString(notice.ForestProductID);
                    //qty.InnerText = notice.Qty;
                    durationfrom.InnerText = Convert.ToString(notice.BiddOpeningDate);
                    durationto.InnerText = Convert.ToString(notice.BidClosingDate);
                    //reservedPrice.InnerText = Convert.ToString(notice.ReservedPrice);

                    schedularInfo.AppendChild(ID);
                    //schedularInfo.AppendChild(regionCode);
                    //schedularInfo.AppendChild(circleCode);
                    //schedularInfo.AppendChild(divisionCode);
                    //schedularInfo.AppendChild(rangeCode);
                    //schedularInfo.AppendChild(depotId);
                    //schedularInfo.AppendChild(forestProduceID);
                    //schedularInfo.AppendChild(forestProductID);
                    //schedularInfo.AppendChild(qty);
                    schedularInfo.AppendChild(durationfrom);
                    schedularInfo.AppendChild(durationto);
                    //schedularInfo.AppendChild(reservedPrice);

                    xmldoc.DocumentElement.AppendChild(schedularInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["AuNoticeschedularInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(notice, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SubmitAuctionschedularForm(FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null;
            TempData["AUSCH_Status"] = null;
            NoticeManagement notice = new NoticeManagement();
            string status = "";
            try
            {
                DateTime now = DateTime.Now;
                string id = now.Ticks.ToString();
                notice.RequestId = id;

                if (Session["UserID"] != null)
                {

                    notice.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }
                if (fm["SchedulerPeriod"].ToString() == "")
                {
                    notice.SchedulerPeriod = "";
                }
                else
                {
                    notice.SchedulerPeriod = fm["SchedulerPeriod"].ToString();
                }

                if (fm["Hd_RegionCode"].ToString() == "")
                {
                    notice.RegionCode = "";
                }
                else
                {
                    notice.RegionCode = fm["Hd_RegionCode"].ToString();
                }

                if (fm["Hd_CircleCode"].ToString() == "")
                {
                    notice.CircleCode = "";
                }
                else
                {
                    notice.CircleCode = fm["Hd_CircleCode"].ToString();
                }

                if (fm["Hd_DivisionCode"].ToString() == "")
                {
                    notice.DivisionCode = "";
                }
                else
                {
                    notice.DivisionCode = fm["Hd_DivisionCode"].ToString();
                }

                if (fm["Hd_RangeCode"].ToString() == "")
                {
                    notice.RangeCode = "";
                }
                else
                {
                    notice.RangeCode = fm["Hd_RangeCode"].ToString();
                }

                if (fm["Hd_DepotId"].ToString() == "")
                {
                    notice.DepotId = 0;
                }
                else
                {
                    notice.DepotId = Convert.ToInt64(fm["Hd_DepotId"].ToString());
                }
                if (fm["Hd_ForestProduceID"].ToString() == "")
                {
                    notice.ForestProduceID = 0;
                }
                else
                {
                    notice.ForestProduceID = Convert.ToInt64(fm["Hd_ForestProduceID"].ToString());
                }
                if (fm["Hd_ForestProductID"].ToString() == "")
                {
                    notice.ForestProductID = 0;
                }
                else
                {
                    notice.ForestProductID = Convert.ToInt64(fm["Hd_ForestProductID"].ToString());
                }
                if (fm["Hd_Qty"].ToString() == "")
                {
                    notice.Qty = "";
                }
                else
                {
                    notice.Qty = fm["Hd_Qty"].ToString();
                }
                if (fm["EmdAmount"].ToString() == "")
                {
                    notice.EmdAmount = "";
                }
                else
                {
                    notice.EmdAmount = fm["EmdAmount"].ToString();
                }
                if (fm["Description"].ToString() == "")
                {
                    notice.Description = "";
                }
                else
                {
                    notice.Description = fm["Description"].ToString();
                }

                if (Command == "Submit")
                {
                    DataSet ds = new DataSet();
                    if (Session["AuNoticeschedularInfo"] != null)
                    {
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["AuNoticeschedularInfo"].ToString() + ".xml"));
                        //status = notice.AddAuctionSchedularDetails(ds.Tables[0]);
                    }

                    if (Session["AuNoticeschedularInfo"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["AuNoticeschedularInfo"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["AuNoticeschedularInfo"].ToString() + ".xml"));
                            Session["AuNoticeschedularInfo"] = null;
                        }
                    }

                    if (status != "")
                    {
                        TempData["AUSCH_Status"] = "Scheduler:#" + status + " Created Successfully";
                        actionResult = RedirectToAction("ManageAuctionSchedular", "ManageNotice");
                    }
                    else
                    {
                        TempData["AUSCH_Status"] = "No inserted";
                    }

                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;
        }



        /// <summary>
        /// Delete Notice Number.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteNoticeData(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            NoticeManagement obj = new NoticeManagement();
            Int64 UpdatedBy = 0;

            try
            {
                if (Session["UserID"] != null)
                {

                    UpdatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (!String.IsNullOrEmpty(id))
                {

                    var msg = obj.RemoveNotice(Convert.ToInt64(id));

                    TempData["ReturnMsg"] = msg.ReturnMsg; TempData["IsError"] = msg.IsError;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return RedirectToAction("ManageNotice", "ManageNotice");
        }

        /// <summary>
        /// Get All Details Of a scheduler to create notice
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSchedularDetails(string sID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string notice = "";
            NoticeManagement obj_no = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                if (!String.IsNullOrEmpty(sID))
                {
                    DataSet dsScheduler = obj_notice.GetSchedularDetails(Convert.ToInt64(sID));
                    if (dsScheduler != null && dsScheduler.Tables[0].Rows.Count > 0)
                    {
                        Session["SchedularDetails"] = dsScheduler;
                        foreach (DataRow dr in dsScheduler.Tables[0].Rows)
                        {
                            obj_no = new NoticeManagement();
                            obj_no.RegionCode = dr["REG_CODE"].ToString();
                            obj_no.CircleCode = dr["CIRCLE_CODE"].ToString();
                            obj_no.DivisionCode = dr["DIV_CODE"].ToString();
                            obj_no.RangeCode = dr["RANGE_CODE"].ToString();
                            obj_no.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                        }
                    }

                    NoticeManagement noticeManagement = new NoticeManagement();


                    DataTable dnt = noticeManagement.PublishNoticeview(Convert.ToInt64(sID), "AUCSHE");
                    if (dnt != null && dnt.Rows.Count > 0)
                    {
                        notice = dnt.Rows[0][0].ToString();
                    }


                    DataSet dsDepotSchedulers = noticeManagement.BindScheduler(Convert.ToInt64(sID));
                    DataView dv = null; DataTable dtMonths = null; DataTable dtSchedulerData = null;

                    foreach (DataTable dtDepotSchedular in dsDepotSchedulers.Tables)
                    {
                        dv = new DataView(dtDepotSchedular);
                        dtMonths = dv.ToTable(true, new string[] { "Month" });
                        dtSchedulerData = new DataTable();
                        dtSchedulerData.Columns.Add("depot", typeof(string));
                        dtSchedulerData.Columns.Add("product", typeof(string));

                        Dictionary<int, string> strMonths = new Dictionary<int, string>();
                        string item = string.Empty;

                        foreach (DataRow dr in dtMonths.Rows)
                        {
                            item = string.Empty;
                            dtSchedulerData.Columns.Add(dr["Month"].ToString(), typeof(string));

                            DataRow[] dr1 = dtDepotSchedular.Select("Month ='" + dr["Month"].ToString() + "'");
                            foreach (DataRow drs in dr1)
                            { item = item + "," + drs["Day"].ToString(); }

                            item = item + " " + dr["Month"].ToString();
                            strMonths.Add(strMonths.Count, item.TrimStart(','));
                        }
                        dtSchedulerData.Columns.Add("MobileNo", typeof(string));

                        DataRow drNewRow = dtSchedulerData.NewRow();
                        drNewRow["depot"] = dtDepotSchedular.Rows[0][0].ToString();
                        drNewRow["product"] = dtDepotSchedular.Rows[0][1].ToString();
                        drNewRow["MobileNo"] = dtDepotSchedular.Rows[0]["MobileNo"].ToString();

                        for (int i = 0; i < strMonths.Count; i++)
                        {
                            drNewRow[i + 2] = strMonths.Values.ToList()[i].ToString();
                        }
                        dtSchedulerData.Rows.Add(drNewRow);


                        sb.Append("<tr>");
                        foreach (DataColumn dc in dtSchedulerData.Columns)
                        {
                            sb.Append("<td col-lg-3>" + dtSchedulerData.Rows[0][dc] + "</td>");
                        }
                        sb.Append("</tr>");
                    }


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = obj_no, list2 = notice, list3 = sb.ToString() }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetProductTypeForScheduler(string sID, string dID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            StringBuilder sb = new StringBuilder();
            sb.Append("<label>Date(s) of Auction: <span class='mandatory'>*</span></label><table id='tblAuctionDates'><tr>");

            try
            {
                if (!String.IsNullOrEmpty(sID) && !String.IsNullOrEmpty(dID))
                {
                    Int64 DepotID = Convert.ToInt64(dID);
                    Int64 SID = Convert.ToInt64(sID);
                    DataSet dsScheduler = new DataSet();
                    dsScheduler = Session["SchedularDetails"] as DataSet;
                    if (Session["SchedularDetails"] != null)
                    {
                        DataTable orders = dsScheduler.Tables[1];
                        EnumerableRowCollection<DataRow> query = from order in orders.AsEnumerable()
                                                                 where order.Field<Int64>("Depot_Id") == DepotID && order.Field<Int64>("AucScdID") == SID
                                                                 select order;
                        DataView view = query.AsDataView();
                        DataTable dt = view.ToTable();
                        ViewBag.fname = dt;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                        }
                    }

                    DataTable datedt = obj_notice.BindDateScheduler(SID, DepotID);

                    for (int i = 0; i < datedt.Rows.Count; i++)
                    {
                        sb.Append("<td><input type='radio' class='radioBtnClass' id='" + i + "' name='rdDates' value='" + datedt.Rows[i][0] + "' /></td><td>" + string.Format(datedt.Rows[i][0].ToString(), "dd/MMM/yyyy") + "</td>");
                    }
                    sb.Append("</tr></table>");
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = items, list2 = sb.ToString() }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetProductForScheduler(string sID, string dID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                if (!String.IsNullOrEmpty(sID) && !String.IsNullOrEmpty(dID))
                {
                    Int64 TypeID = Convert.ToInt64(dID);
                    Int64 SID = Convert.ToInt64(sID);
                    DataSet dsScheduler = new DataSet();
                    if (Session["SchedularDetails"] != null)
                    {
                        dsScheduler = Session["SchedularDetails"] as DataSet;
                        DataTable orders = dsScheduler.Tables[2];
                        EnumerableRowCollection<DataRow> query = from order in orders.AsEnumerable()
                                                                 where order.Field<Int64>("ProduceTypeID") == TypeID && order.Field<Int64>("AucScdID") == SID
                                                                 select order;
                        DataView view = query.AsDataView();
                        DataTable dt = view.ToTable();
                        ViewBag.fname = dt;
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["ID"].ToString() });
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


        [HttpPost]
        public JsonResult GetProduct_Qty_ForScheduler(string sID, string dID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            string Qty = string.Empty;
            string Unit = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(sID) && !String.IsNullOrEmpty(dID))
                {
                    Int64 TypeID = Convert.ToInt64(dID);
                    Int64 SID = Convert.ToInt64(sID);
                    DataSet dsScheduler = new DataSet();
                    if (Session["SchedularDetails"] != null)
                    {
                        dsScheduler = Session["SchedularDetails"] as DataSet;
                        DataTable orders = dsScheduler.Tables[2];
                        EnumerableRowCollection<DataRow> query = from order in orders.AsEnumerable()
                                                                 where order.Field<Int64>("ProduceTypeID") == TypeID && order.Field<Int64>("AucScdID") == SID
                                                                 select order;
                        DataView view = query.AsDataView();
                        DataTable dt = view.ToTable();
                        ViewBag.fname = dt;

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            Qty = Qty + dt.Rows[0]["EstimatedQty"].ToString();
                            Unit = Unit + dt.Rows[0]["ProductUnit"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = Qty, list2 = Unit }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// bind data for update notice Number
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult EditDetails(string noticeId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            NoticeManagement obj_no = null;

            try
            {


                if (!String.IsNullOrEmpty(noticeId))
                {

                    DataTable dt = obj_notice.BindNoticeNo(Convert.ToInt64(noticeId), "UPDATE");
                    if (dt != null && dt.Rows.Count > 0)
                    {


                        foreach (DataRow dr in dt.Rows)
                        {
                            obj_no = new NoticeManagement();
                            obj_no.NoticeId = Convert.ToInt64(noticeId);
                            obj_no.NoticeNo = dr["Notice_Number"].ToString();
                            //obj_no.RegionCode = dr["REG_CODE"].ToString();
                            //obj_no.CircleCode = dr["CIRCLE_CODE"].ToString();
                            //obj_no.DivisionCode = dr["DIV_CODE"].ToString();
                            obj_no.RangeCode = dr["RANGE_CODE"].ToString();
                            obj_no.DepotId = Convert.ToInt64(dr["Depot_Id"].ToString());
                            obj_no.ForestProduce = dr["Forest_Produce"].ToString();
                            obj_no.ForestProduceName = dr["ProduceType"].ToString();
                            obj_no.ForestProductID = Convert.ToInt64(dr["Forest_Product"].ToString());
                            obj_no.ForestProduct = dr["ProductName"].ToString();
                            obj_no.ProduceUnit = dr["Unit"].ToString();
                            obj_no.Qty = dr["Quantity"].ToString();
                            obj_no.SchedulerId = Convert.ToInt64(dr["SchedulerID"].ToString());
                            obj_no.SchedulerPeriod = dr["SchedulerType"].ToString();
                            obj_no.ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString());
                            DateTime _date1 = DateTime.Parse(dr["DurationFrom"].ToString());
                            DateTime _date2 = DateTime.Parse(dr["DurationTo"].ToString());
                            obj_no.DateFrom = _date1.ToString("dd/MM/yyyy");
                            obj_no.DateTo = _date2.ToString("dd/MM/yyyy");
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(obj_no, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bind dropdown Of schedular by period(Monthly, Yearly etc.)
        /// </summary>
        /// <param name="Period"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SchedularByPeriod(string Period)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(Period))
                {

                    DataTable dt = location.BindSchedular(Period);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Auction_SchedularNo"].ToString(), Value = @dr["ID"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// bind dropdown of circle by reagion Id
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CircleData(string regionCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(regionCode))
                {

                    DataTable dt = location.BindCircle(regionCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["CIRCLE_NAME"].ToString(), Value = @dr["CIRCLE_CODE"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }


        /// <summary>
        /// Bind Division Drop down
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult DivisionData(string regionCode, string circleCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if ((!String.IsNullOrEmpty(regionCode)) && (!String.IsNullOrEmpty(circleCode)))
                {
                    //items.Add(new SelectListItem { Text = "---Select---", Value = "0" });
                    DataTable dt = location.BindDivision(regionCode, circleCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }

                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Bind Range Drop Down
        /// </summary>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RangeData(string regionCode, string circleCode, string divisionCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if ((!String.IsNullOrEmpty(regionCode)) && (!String.IsNullOrEmpty(circleCode)) && (!String.IsNullOrEmpty(divisionCode)))
                {

                    DataTable dt = location.BindRange(regionCode, circleCode, divisionCode);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Bind Village Dropdown
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string divisionCode, string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(divisionCode) && !String.IsNullOrEmpty(rangeCode))
                {
                    DataTable dt = location.BindVillage(divisionCode, rangeCode);
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Bind Depot dropDown
        /// </summary>
        /// <param name="divCode"></param>
        /// <param name="ranCode"></param>
        /// <param name="villCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getDepotData(string regionCode, string circleCode, string divisionCode, string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                if (!String.IsNullOrEmpty(regionCode) && !String.IsNullOrEmpty(circleCode) && !String.IsNullOrEmpty(divisionCode) && !String.IsNullOrEmpty(rangeCode))
                {
                    DataTable dt = location.BindDepot(regionCode, circleCode, divisionCode, rangeCode);
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Depot_Name"].ToString(), Value = @dr["Depot_Id"].ToString() });
                    }



                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));

        }

        /// <summary>
        /// Bind Depot dropDown
        /// </summary>
        /// <param name="divCode"></param>
        /// <param name="ranCode"></param>
        /// <param name="villCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getDepotDatanotice(string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            EnumerableRowCollection<SelectListItem> data = null;

            try
            {
                if (!String.IsNullOrWhiteSpace(rangeCode))
                {
                    data = new FMDSS.Models.ForestDevelopment.TransitPermit().SetDropdownData(7, rangeCode);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(data, "Value", "Text"));

        }

        /// <summary>
        /// Bind Forest Produce Dropdown
        /// </summary>
        /// <param name="depotId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForesProducescd(string depotId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            string notice = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(depotId))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindProduce(Convert.ToInt64(depotId));
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["PRODUCE_TYPE"].ToString() });
                    }



                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }

        /// <summary>
        /// Bind Forest Produce Dropdown
        /// </summary>
        /// <param name="depotId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForesProduce(string depotId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> pType = new List<SelectListItem>();

            StringBuilder sb = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(depotId))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindProduce(Convert.ToInt64(depotId));

                    pType = dt.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("ID")),
                        Text = x.Field<string>("ProduceType")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new { ProduceTypeList = pType }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bind Product Dropdown 
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="producetype"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getForesProduct(string depotId, string producetype)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(depotId) && !String.IsNullOrEmpty(producetype))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindForestProduct(Convert.ToInt64(depotId), Convert.ToInt64(producetype));

                    items = dt.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("ID")),
                        Text = x.Field<string>("ProductName")
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }


        // //History: Code Update with ref. to bug ID: 206,207,208 Arvind
        /// <summary>
        /// For get Produce Quantity and rate
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="producetype"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getForesProduceqty(string rangeCode, string depotId, string producetype, string product)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            NoticeManagement noticeManagement = null;
            NoticeManagement notice = new NoticeManagement();
            StringBuilder sb = new StringBuilder();

            try
            {
                if (!String.IsNullOrEmpty(depotId) && !String.IsNullOrEmpty(producetype) && !String.IsNullOrEmpty(product))
                {
                    noticeManagement = new NoticeManagement();
                    DataTable dt = notice.BindProducestock(Convert.ToInt64(depotId), Convert.ToInt64(producetype), Convert.ToInt64(product));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            noticeManagement.ProduceUnit = dr["UnitName"].ToString();
                            noticeManagement.Qty = dr["PRODUCE_QTY"].ToString();
                            noticeManagement.ProductRate = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                        }
                    }
                    else
                    {
                        noticeManagement.ProduceUnit = "";
                        noticeManagement.Qty = "0";
                        noticeManagement.ProductRate = 0;
                    }

                    DataTable reveDt = notice.LastThreeYearReveneu(rangeCode, Convert.ToInt64(depotId), Convert.ToInt64(producetype), Convert.ToInt64(product));
                    if (reveDt.Rows.Count == 0)
                    {
                        sb.Append("Kindly update last 3 Years Revenue data before creating an Auction Notice for the selected Depot/Product!!");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = noticeManagement, list2 = sb.ToString() }, JsonRequestBehavior.AllowGet);
            //return Json(noticeManagement, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Bind Notice view details
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult ViewDetails(string noticeId)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    NoticeManagement obj_no = new NoticeManagement();
        //    NoticeManagement obj = new NoticeManagement();
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(noticeId))
        //        {
        //            DataTable dt = obj.BindNoticeNo(Convert.ToInt64(noticeId), "VIEW");
        //            if (dt != null && dt.Rows.Count > 0)
        //            {

        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    obj_no = new NoticeManagement();
        //                    obj_no.NoticeNo = dr["Notice_Number"].ToString();
        //                    obj_no.RegionName = dr["REG_NAME"].ToString();
        //                    obj_no.CircleName = dr["CIRCLE_NAME"].ToString();
        //                    obj_no.DivisionName = dr["DIV_NAME"].ToString();
        //                    obj_no.Rangename = dr["RANGE_NAME"].ToString();
        //                    obj_no.DepotName = dr["Depot_Name"].ToString();
        //                    obj_no.ForestProduce = dr["ProduceType"].ToString();
        //                    obj_no.ForestProduct = dr["ProductName"].ToString();
        //                    obj_no.ProduceUnit = dr["Unit"].ToString();
        //                    obj_no.Qty = dr["Quantity"].ToString();
        //                    obj_no.ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString());
        //                    DateTime _date1 = DateTime.Parse(dr["DurationFrom"].ToString());
        //                    DateTime _date2 = DateTime.Parse(dr["DurationTo"].ToString());
        //                    obj_no.Durations = _date1.ToString("dd/MM/yyyy") + " To " + _date2.ToString("dd/MM/yyyy");

        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }

        //    return Json(obj_no, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult ViewDetailsCommon(string noticeId, string actionType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            NoticeManagement obj = new NoticeManagement();
            ViewBag.ActionType = actionType;
            try
            {
                if (!String.IsNullOrEmpty(noticeId))
                {
                    DataSet dsResult = obj.GetNoticeDetails(Convert.ToInt64(noticeId), "4");
                    if (Util.isValidDataSet(dsResult, 0, true))
                    {
                        DataRow dr = dsResult.Tables[0].Rows[0];
                        obj.NoticeId = Convert.ToInt64(dr["NoticeID"].ToString());
                        obj.NoticeNo = dr["Notice_Number"].ToString();
                        obj.RegionName = dr["REG_NAME"].ToString();
                        obj.CircleName = dr["CIRCLE_NAME"].ToString();
                        obj.DivisionName = dr["DIV_NAME"].ToString();
                        obj.Rangename = dr["RANGE_NAME"].ToString();
                        obj.DepotName = dr["Depot_Name"].ToString();
                        DateTime _date1 = DateTime.Parse(dr["DurationFrom"].ToString());
                        DateTime _date2 = DateTime.Parse(dr["DurationTo"].ToString());
                        obj.Durations = _date1.ToString("dd/MM/yyyy HH:mm") + " To " + _date2.ToString("dd/MM/yyyy HH:mm");
                        obj.BidderName = dr["BidderName"].ToString();
                        obj.BiddingAmount = dr["BiddingAmount"].ToString();
                        obj.EmdAmount = dr["EmdAmount"].ToString();
                        obj.PaidAmount = dr["PaidAmount"].ToString();
                        obj.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dsResult, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return PartialView("_EditNotice", obj);

        }

        //History: Code Update with ref. to bug ID: 209,210 Arvind
        /// <summary>
        /// Publish Notice
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult PublishNotice(string noticeId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            NoticeManagement obj = new NoticeManagement();
            string notice = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                DataTable dt = obj.PublishNoticeview(Convert.ToInt64(noticeId), "Publish");
                if (dt != null && dt.Rows.Count > 0)
                {
                    notice = dt.Rows[0][0].ToString();
                }
                DataTable adt = obj.AmmedmentNoticeview(Convert.ToInt64(noticeId));
                if (adt != null)
                {
                    if (adt.Rows.Count > 0)
                    {
                        sb.Append("<tr>");
                        foreach (DataRow dr in adt.Rows)
                        {
                            sb.Append("<tr><td col-lg-3>" + dr["ROWID"].ToString() + "</td><td col-lg-3>" + dr["Depot_Name"].ToString() + "</td><td col-lg-3>" + dr["DisplayLotNumber"].ToString() + "</td><td col-lg-3>" + dr["PRODUCT"].ToString() + "</td><td col-lg-3>" +
                                dr["QTY"].ToString() + "</td><td col-lg-3>" + dr["DATEFROM"].ToString() + "</td><td col-lg-3>" + dr["DATETO"].ToString() +
                                "</td></tr>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = notice, list2 = sb.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Viewscheduler()
        {
            StringBuilder sb = new StringBuilder();
            List<NoticeManagement> result = new List<NoticeManagement>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            NoticeManagement obj = new NoticeManagement();
            NoticeManagement noticeManagement = null;
            string notice = "";
            try
            {



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list1 = notice, list2 = sb.ToString() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Save Notice data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>

        public ActionResult SaveNotice(NoticeDetails model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            bool isError = false;
            try
            {
                string status = new NoticeManagement().SaveNotice(model);
                if (!String.IsNullOrEmpty(status))
                {
                    TempData["ReturnMsg"] = "Notice No:#" + status + " Created Successfully";
                }
                else
                {
                    isError = true;
                    TempData["ReturnMsg"] = "Not inserted Because for this Auction date Notice Already Created.";
                }
                TempData["IsError"] = isError;
            }
            catch (Exception ex)
            {
                TempData["IsError"] = true; isError = true;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { IsError = isError }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SubmitNoticeForm(NoticeManagement notice, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null;
            TempData["No_Status"] = null;

            try
            {
                DateTime now = DateTime.Now;
                string id = now.Ticks.ToString();
                notice.RequestId = id;

                if (Session["UserID"] != null)
                {
                    notice.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (Command == "Update")
                {
                    notice.ActionFlag = "UPDATE";
                    string status = notice.CreateNotice();
                    if (!String.IsNullOrEmpty(status))
                    {
                        TempData["No_Status"] = "Notice No:#" + status + "Updated Successfully";
                    }
                    else
                    {
                        TempData["No_Status"] = "Not Updated";
                    }

                    actionResult = RedirectToAction("ManageNotice", "ManageNotice");
                }
                else if (Command == "Re-New")
                {
                    notice.ActionFlag = "RENOTICE";
                    string status = notice.CreateNotice();
                    if (!String.IsNullOrEmpty(status))
                    {
                        TempData["No_Status"] = "Notice No:#" + status + "Re-Newed Successfully"; ;
                    }
                    else
                    {
                        TempData["No_Status"] = "Not Re-Newed";
                    }

                    actionResult = RedirectToAction("ManageNotice", "ManageNotice");
                }

                if (Command == "Cancel")
                {
                    actionResult = RedirectToAction("Dashboard", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;

        }

        [HttpPost]
        public JsonResult BindTP(Int64 depotID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> itms = new List<SelectListItem>();
            try
            {
                NoticeManagement noticeManagement = new NoticeManagement();
                DataTable dt = noticeManagement.BindTP(depotID);

                itms = dt.AsEnumerable().Select(x => new SelectListItem
                {
                    Value = Convert.ToString(x.Field<long>("ID")),
                    Text = x.Field<string>("TransitPermitName")
                }).ToList();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new { data = itms }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateRemainingQty(Int64 tpID)
        {
            var dt = new NoticeManagement().GetDetailsByTP(tpID);
            var data = dt.AsEnumerable().Select(x => new
            {
                RemainingQty = x.Field<decimal>("RemainingQty"),
                RatePerUnit = x.Field<double>("RatePerUnit"),
                UnitName = x.Field<string>("UnitName"),
                ProductID = x.Field<long>("ProductID"),
                ProductName = x.Field<string>("ProductName"),
                ProductTypeID = x.Field<long>("ProductTypeID"),
                ProduceType = x.Field<string>("ProduceType")
            }).FirstOrDefault();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPartialCommon(string partialViewName, long objID, string type)
        {
            dynamic _obj = null;

            switch (type)
            {
                case "AddNewNotice":
                    ViewBag.RangeCode = new FMDSS.Models.ForestDevelopment.TransitPermit().SetDropdownData(6, string.Empty);
                    break;
            }
            return PartialView(partialViewName, _obj);
        }
    }
}
