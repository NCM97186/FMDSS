//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Apply Auction
//  Date Created : 05-Jun-2016
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  : Arvind Srivastava  
//  Modified On  : 26-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
// Bug No-456

//*********************************************************************************************************@


using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.CitizenService.ProductionServices;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class AuctionController : BaseController
    {
        //
        // GET: /Auction/
        SMS_EMail_Services _objMail = new SMS_EMail_Services();
        NoticeManagement notice = new NoticeManagement();

        List<SelectListItem> items = new List<SelectListItem>();
        Int64 UserID = 0;
        int ModuleID = 1;
        /// <summary>
        /// Call when request come from Auction view Bind Notice dropdown
        /// </summary>
        /// <returns></returns>
        public ActionResult TenduPattaAuction(string aid)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<NoticeManagement> listAu = new List<NoticeManagement>();
            List<SelectListItem> rangeList = new List<SelectListItem>();
            try
            {
                aid = Encryption.decrypt(aid);
                if (aid == "1")
                    Session["AuctionType"] = "TenduPatta";
                else
                    Session["AuctionType"] = "Timber and Fuelwood";

                #region Notice

                DataTable dt = new DataTable();
                dt = notice.BindDropdownNoticeNo(Session["AuctionType"].ToString(), Convert.ToInt64(Session["UserId"]), false);
                if (dt != null && dt.Rows.Count >= 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        listAu.Add(new NoticeManagement()
                        {
                            RowID = Convert.ToInt64(dr["RowID"].ToString()),
                            NoticeNo = dr["NOTICE_NUMBER"].ToString(),
                            RangeCode = dr["RANGE_NAME"].ToString(),
                            DepotName = dr["Depot_Name"].ToString(),
                            prodName = dr["ProductName"].ToString(),
                            ProduceUnit = dr["Unit"].ToString(),
                            DateFrom = dr["DurationFrom"].ToString(),
                            DateTo = dr["DurationTo"].ToString(),
                        });
                    }

                    #region Product Type
                    string[] TobeDistinct = { "RANGE_CODE", "RANGE_NAME" };

                    DataTable dtUniqRecords = new DataTable();
                    dtUniqRecords = dt.DefaultView.ToTable(true, TobeDistinct);

                    ViewBag.fname1 = dtUniqRecords;
                    foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                    {
                        rangeList.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }
                    ViewBag.RangeCode = rangeList;
                
                   

                    #endregion
                }
                else
                {
                    rangeList.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    ViewBag.RangeCode = rangeList;
                    TempData["Status"] = "No active Auction noitice found into the system!";
                    return View();
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return View(listAu);


        }


        /// <summary>
        /// Call when request come from Auction view Bind Notice dropdown
        /// </summary>
        /// <returns></returns>
        public ActionResult Auction(string aid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {

                aid = Encryption.decrypt(aid);
                if (aid == "1")
                    Session["AuctionTypeold"] = "TenduPatta";
                else
                    Session["AuctionTypeold"] = "Timber and Fuelwood";

                #region Notice
                DataTable dt = new DataTable();
                dt = notice.BindPublishedNoticeNo(Session["AuctionTypeold"].ToString());
                if (dt != null && dt.Rows.Count >= 1)
                {

                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Notice_Number"].ToString(), Value = @dr["Id"].ToString() });
                    }

                    ViewBag.NoticeId = items;
                }
                else
                {
                    TempData["Status"] = "No Currentaly any Notice Publish";
                    return View();
                }

                ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");

                #endregion


                if (Session["User"] != null)
                {

                    ViewData["Name"] = Session["User"];
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return View();


        }

        //************************************Done by Rajkumar for Online Tendu Patta Auction**********************************

        /// <summary>
        /// Function for online auction of tendu patta
        /// </summary>
        /// <returns></returns>
        public ActionResult TenduPattaOnlineAuction()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> RangeName = new List<SelectListItem>();
            try
            {

                DataTable dtRange = new Auction().Select_TenduPattaOnlineAuction("1");
                foreach (DataRow dr in dtRange.Rows)
                {
                    RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }
                ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");
                ViewBag.ddlRange = RangeName;
                ViewData["Name"] = Session["User"];
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }



        /// <summary>
        /// Bind Forest Produce Dropdown
        /// </summary>
        /// <param name="depotId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getNoticeRangeWise(string RangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                DataTable dtRange = new Auction().Select_TenduPattaOnlineAuction("2", RangeCode);
                foreach (DataRow dr in dtRange.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["Notice_Number"].ToString(), Value = @dr["Notice_Number"].ToString() });
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
        public JsonResult getNoticeDetails(string NoticeId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<Auction> lstNotice = new List<Auction>();
            try
            {
                DataTable dtNotice = new Auction().Select_NoticeDetails(NoticeId);
                foreach (DataRow dr in dtNotice.Rows)
                {
                    lstNotice.Add(new Auction
                    {
                        SiteId = dr["Id"].ToString(),
                        SiteName = dr["Site_Name"].ToString(),
                        VillageName = dr["VillageName"].ToString(),
                        ReservedPrice = Convert.ToDecimal(dr["ReservePrice"].ToString())

                    });

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(lstNotice, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowPartialView(string NoticeId)
        {

            Auction obj = new Auction();
            decimal reserveAmount = 0;
            List<Auction> lstNotice = new List<Auction>();
            DataTable dtNotice = new Auction().Select_NoticeDetails(NoticeId);
            foreach (DataRow dr in dtNotice.Rows)
            {
                lstNotice.Add(new Auction()
                {
                    SiteId = dr["Id"].ToString(),
                    SiteName = dr["Site_Name"].ToString(),
                    VillageName = dr["VillageName"].ToString(),
                    ReservedPrice = Convert.ToDecimal(dr["ReservePrice"].ToString()),
                    BiddingAmount = 0,
                });
                reserveAmount = Convert.ToDecimal(dr["ReservePrice"].ToString());
                obj.TotalReservePrice += reserveAmount;
            }

            obj.EmdPaybleAmount = ((obj.TotalReservePrice * 2) / 100);
            var TenduPattaPartialView = RenderRazorViewToString(this.ControllerContext, "TenduPattaOnline", lstNotice);
            var json = Json(new { TenduPattaPartialView, list1 = obj.TotalReservePrice, list2 = obj.EmdPaybleAmount });
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
        /// Save auction data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitTenduPattaOnline(List<Auction> lstNotice, Auction Model, FormCollection fm, HttpPostedFileBase DdchkFile)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            decimal finalAmount = 0;
            ActionResult actionResult = null;
            Auction _obj = new Auction();
            string DdchkFileOrg = string.Empty;
            var path = "";
            try
            {

                //foreach (var item in lstNotice) {
                //    if (item.SiteId != string.Empty && item.SiteName != string.Empty && item.VillageName != string.Empty && Convert.ToString(item.ReservedPrice) != string.Empty && item.BiddingPrice != string.Empty && item.BiddingPrice != "0")
                //    { 


                //    }                
                //}
                _obj.RequestId = Convert.ToString(DateTime.Now.Ticks);
                DataTable dtNoticeInfo = new DataTable();
                dtNoticeInfo = NoticeInformation(lstNotice, _obj.RequestId);
                if (dtNoticeInfo.Rows.Count == 0)
                {
                    TempData["RowCheck"] = "Enter member details";
                    return RedirectToAction("TenduPattaOnlineAuction", "TenduPattaOnlineAuction");
                }
                else
                {
                    if (dtNoticeInfo.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtNoticeInfo.Rows.Count; k++)
                        {
                            finalAmount += Convert.ToDecimal(dtNoticeInfo.Rows[k]["BiddingAmount"].ToString());
                        }
                    }
                }
                _obj.Applicant_Type = Model.Applicant_Type;
                _obj.BidderName = Model.BidderName;
                _obj.NoticeNo = Model.NoticeNo;
                _obj.RangeCode = Model.RangeCode;
                _obj.BiddingAmount = finalAmount;

                if (fm["PaymentMode"].ToString() == "")
                {
                    _obj.PaymentMode = "";
                }
                else
                {
                    _obj.PaymentMode = fm["PaymentMode"].ToString();
                }

                if (fm["OfflinePaymentMode"].ToString() == "")
                {
                    _obj.OfflinePaymentMode = "";
                }
                else
                {
                    _obj.OfflinePaymentMode = fm["OfflinePaymentMode"].ToString();
                }

                if (fm["BankName"].ToString() == "")
                {
                    _obj.BankName = "";
                }
                else
                {
                    _obj.BankName = fm["BankName"].ToString();
                }

                if (fm["BranchName"].ToString() == "")
                {
                    _obj.BranchName = "";
                }
                else
                {
                    _obj.BranchName = fm["BranchName"].ToString();
                }
                if (fm["DdchkIssuesDate"].ToString() == "")
                {
                    _obj.DdchkIssuesDate = "";

                }
                else
                {
                    _obj.DdchkIssuesDate = fm["DdchkIssuesDate"].ToString();

                }

                if (fm["DdChkNumber"].ToString() == "")
                {
                    _obj.DdChkNumber = "";
                }
                else
                {
                    _obj.DdChkNumber = fm["DdChkNumber"].ToString();
                }

                if (DdchkFile != null && DdchkFile.ContentLength > 0)
                {
                    DdchkFileOrg = Path.GetFileName(DdchkFile.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DdchkFileOrg;
                    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    _obj.DdchkFile = FileFullName;

                    _obj.DdchkFilepth = @"~/PermissionDocument/" + FileFullName.Trim();
                    DdchkFile.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    _obj.DdchkFile = "";
                    _obj.DdchkFilepth = "";
                }


                if (fm["EmdPaybleAmount"].ToString() == "")
                {
                    _obj.EmdPaybleAmount = 0;
                }
                else
                {
                    _obj.EmdPaybleAmount = Convert.ToDecimal(fm["EmdPaybleAmount"].ToString());
                }
                Int64 status = _obj.SubmitTenduPattaOnline(dtNoticeInfo);


                if (!String.IsNullOrEmpty(status.ToString()))
                {
                    TempData["Status"] = "Your Request id Successfully added in Database and Request Id: " + _obj.RequestId;
                    if (_obj.PaymentMode != null && _obj.PaymentMode == "Online")
                    {
                        _obj.OfflinePaymentMode = "";
                        #region payment
                        DataTable dtColmn = new DataTable();
                        if (dtColmn.Rows.Count == 0)
                        {
                            dtColmn.Columns.Add("Request_Id");
                            dtColmn.Columns.Add("NoticeNo");
                            dtColmn.Columns.Add("Name");
                            dtColmn.Columns.Add("PaidAmt");
                            dtColmn.Columns.Add("Status");
                        }
                        //decimal biddingAmt = 0;

                        DataRow dtrow = dtColmn.NewRow();
                        Session["paymentType"] = "EMD";
                        dtrow["Request_Id"] = _obj.RequestId;
                        Session["requestId"] = _obj.RequestId;
                        dtrow["NoticeNo"] = _obj.NoticeNo;
                        dtrow["Name"] = Session["User"].ToString(); ;
                        dtrow["PaidAmt"] = _obj.EmdPaybleAmount;
                        Session["totalprice"] = _obj.EmdPaybleAmount;
                        dtrow["Status"] = "Pending";

                        dtColmn.Rows.Add(dtrow);
                        ViewData.Model = dtColmn.AsEnumerable();

                        #endregion
                        actionResult = View("AuctionPayment");
                    }
                    else
                    {
                        actionResult = RedirectToAction("TenduPattaOnlineAuction", "Auction");
                    }
                }
                else
                {

                    TempData["Status"] = "Not inserted";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;
        }


        public DataTable NoticeInformation(List<Auction> lstNoticeInfo, string requestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("SiteID", typeof(String));
                objDt2.Columns.Add("RequestedID", typeof(String));
                objDt2.Columns.Add("BiddingAmount", typeof(String));

                objDt2.AcceptChanges();
                foreach (var item in lstNoticeInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.SiteId != string.Empty && item.SiteName != string.Empty && item.VillageName != string.Empty && Convert.ToString(item.ReservedPrice) != string.Empty && item.BiddingPrice != null && item.BiddingPrice != string.Empty && item.BiddingPrice != "0")
                    {
                        dr["SiteID"] = item.SiteId;
                        dr["RequestedID"] = requestId;
                        dr["BiddingAmount"] = item.BiddingPrice;
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return objDt2;
        }

        //*********************************************************************************************************************


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
                    DataTable dt = noticeManagement.BindProduceforauction(Convert.ToInt64(depotId), Session["AuctionType"].ToString());
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
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
                    DataTable dt = noticeManagement.BindForestProductforauction(Convert.ToInt64(depotId), Convert.ToInt64(producetype), Session["AuctionType"].ToString());
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }





        ///// <summary>
        ///// Call when request come from Auction view Bind Notice dropdown
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Auction()
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    try
        //    {
        //        #region Notice
        //        DataTable dt = new DataTable();
        //        dt = notice.BindDropdownNoticeNo("OTHERS", Convert.ToInt64(Session["UserId"]));
        //        if (dt != null && dt.Rows.Count >= 1)
        //        {

        //            ViewBag.fname = dt;
        //            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
        //            {
        //                items.Add(new SelectListItem { Text = @dr["Notice_Number"].ToString(), Value = @dr["Id"].ToString() });
        //            }

        //            ViewBag.NoticeId = items;
        //        }
        //        else
        //        {
        //            TempData["Status"] = "No Currentaly any Notice Publish";
        //            return View();
        //        }

        //        ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");

        //        #endregion


        //        if (Session["User"] != null)
        //        {

        //            ViewData["Name"] = Session["User"];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }


        //    return View("Auction");


        //}



        /// <summary>
        /// For Auction Payment
        /// </summary>
        /// <param name="ReqId"></param>
        /// <param name="noticeNo"></param>
        /// <param name="bidderName"></param>
        /// <param name="biddingAmt"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult AuctionPayment(string ReqId, string noticeNo, string bidderName, string biddingAmt, string status)
        {
            #region payment
            DataTable dtColmn = new DataTable();
            if (dtColmn.Rows.Count == 0)
            {
                dtColmn.Columns.Add("Request_Id");
                dtColmn.Columns.Add("NoticeNo");
                dtColmn.Columns.Add("Name");
                dtColmn.Columns.Add("PaidAmt");
                dtColmn.Columns.Add("Status");
            }
            //decimal biddingAmt = 0;
            Session["paymentType"] = "PS";
            Session["totalprice"] = biddingAmt;
            Session["requestId"] = ReqId;
            DataRow dtrow = dtColmn.NewRow();
            dtrow["Request_Id"] = ReqId;
            dtrow["NoticeNo"] = noticeNo;
            dtrow["Name"] = bidderName;
            dtrow["PaidAmt"] = biddingAmt;
            dtrow["Status"] = status;

            dtColmn.Rows.Add(dtrow);
            ViewData.Model = dtColmn.AsEnumerable();
            return View("AuctionPayment");
            #endregion

        }

        /// <summary>
        /// Save auction data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        public ActionResult SubmitAucDetail(FormCollection fm, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                #region payment
                DataTable dtColmn = new DataTable();
                if (dtColmn.Rows.Count == 0)
                {
                    dtColmn.Columns.Add("Request_Id");
                    dtColmn.Columns.Add("NoticeNo");
                    dtColmn.Columns.Add("Name");
                    dtColmn.Columns.Add("PaidAmt");
                    dtColmn.Columns.Add("Status");
                }
                //decimal biddingAmt = 0;
                DataRow dtrow = dtColmn.NewRow();
                if (fm["Hdn_ReqId"].ToString() != "")
                {
                    dtrow["Request_Id"] = fm["Hdn_ReqId"].ToString();
                    Session["requestId"] = fm["Hdn_ReqId"].ToString();
                }

                if (fm["Hdn_Notice"].ToString() != "")
                {
                    dtrow["NoticeNo"] = fm["Hdn_Notice"].ToString();
                }
                if (fm["Hdn_Name"].ToString() != "")
                {
                    dtrow["Name"] = fm["Hdn_Name"].ToString();
                }

                if (fm["Hdn_PaidAmt"].ToString() != "")
                {
                    dtrow["PaidAmt"] = fm["Hdn_PaidAmt"].ToString();
                    Session["totalprice"] = fm["Hdn_PaidAmt"].ToString();
                }
                if (fm["Hdn_Status"].ToString() != "")
                {
                    dtrow["Status"] = fm["Hdn_Status"].ToString();
                }


                dtColmn.Rows.Add(dtrow);
                ViewData.Model = dtColmn.AsEnumerable();

                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("AuctionPayment");

        }

        /// <summary>
        /// use for bind bidding Amount
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="BiddingAmt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReqBiddingAmt(string RequestId, string BiddingAmt)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {

                Session["ReqId"] = RequestId;
                Session["BiddAmt"] = BiddingAmt;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json("Sucess", JsonRequestBehavior.AllowGet);
        }

        //History: Code Update with ref. to bug ID: 213,214 Arvind
        /// <summary>
        /// Integration with E-mitra
        /// </summary>

        [HttpPost]
        public void Pay()
        {
            //EM33172142@5488
            Payment pay = new Payment();
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            string encrypt = pay.RequestString("EM33172142@5488", Session["requestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "Auction/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        }

        /// <summary>
        /// check payment status
        /// </summary>
        /// <returns></returns>
        public ActionResult Payment()
        {
            if (Session["requestId"] != null)
            {
                //TicketBooking cs = new TicketBooking();
                int status1 = 0;
                Auction au = new Auction();
                Payment pay = new Payment();
                DataTable dt = new DataTable();
                #region Datarow defination
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("TRANSACTION STATUS");
                    dt.Columns.Add("REQUEST ID");
                    dt.Columns.Add("EMITRA TRANSACTION ID");
                    dt.Columns.Add("TRANSACTION TIME");
                    dt.Columns.Add("TRANSACTION AMOUNT");
                    dt.Columns.Add("USER NAME");
                    dt.Columns.Add("TRANSACTION BANK DETAILS");
                }
                #endregion

                string response = Request.QueryString["trnParams"].ToString();
                string ResponseResult = pay.ProcesTranscationresponce(response);

                #region Response decryption

                string str1, str2;
                str1 = ResponseResult.Replace("<RESPONSE ", "");
                str2 = str1.Replace("></RESPONSE>", "");
                string[] Responsearr = str2.Split(' ');
                string checkFail = "STATUS='FAILED'";
                string checkSucess = "STATUS='SUCCESS'";
                string rowstatus1 = "";
                foreach (var item in Responsearr)
                {
                    if (item.Equals(checkFail))
                    {
                        string[] status2 = item.Split('=');
                        rowstatus1 = status2[1].ToString();
                    }
                    if (item.Equals(checkSucess))
                    {
                        string[] status2 = item.Split('=');
                        rowstatus1 = status2[1].ToString();
                    }
                }
                int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 2);
                #endregion
                #region Response Status
                if (finalstatus1 == "FAILED")
                {
                    string[] emitratransid = Responsearr[0].Split('=');
                    string[] transtime = Responsearr[1].Split('=');
                    string[] reqid = Responsearr[2].Split('=');
                    string[] reqamt = Responsearr[3].Split('=');
                    string[] username = Responsearr[4].Split('=');
                    string[] status = Responsearr[7].Split('=');


                    DataRow dtrow = dt.NewRow();
                    string rowstatus = status[1].ToString();
                    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    string rowreqid = reqid[1].ToString();
                    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    string rawemitra = emitratransid[1].ToString();
                    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    au.TransactionID = "0";
                    au.RequestId = finalreqid;
                    string rawtransamount = reqamt[1].ToString();
                    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    string rawusername = username[1].ToString();
                    int usernamelen = Convert.ToInt32(rawusername.Length);
                    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);

                    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    dtrow["REQUEST ID"] = finalreqid;
                    dtrow["EMITRA TRANSACTION ID"] = "";
                    dtrow["TRANSACTION TIME"] = "";//transtime[1];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;

                    if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                    {
                        au.Trn_Status_Code = 0;
                    }
                    dt.Rows.Add(dtrow);
                }
                else if (finalstatus1 == "SUCCESS")
                {
                    string[] emitratransid = Responsearr[0].Split('=');
                    string[] transtime = Responsearr[1].Split('=');
                    string[] reqid = Responsearr[3].Split('=');
                    string[] reqamt = Responsearr[4].Split('=');
                    string[] username = Responsearr[5].Split('=');
                    string[] status = Responsearr[8].Split('=');
                    string[] bank = Responsearr[9].Split('=');
                    string[] bankbidno = Responsearr[13].Split('=');

                    DataRow dtrow = dt.NewRow();
                    string rowstatus = status[1].ToString();
                    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    string rowreqid = reqid[1].ToString();
                    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    string rawemitra = emitratransid[1].ToString();
                    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    au.TransactionID = finalreqid;
                    string rawtransamount = reqamt[1].ToString();
                    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    string rawusername = username[1].ToString();
                    int usernamelen = Convert.ToInt32(rawusername.Length);
                    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
                    string rawbank = bank[1].ToString();
                    int banklen = Convert.ToInt32(rawbank.Length);
                    string finalbank = rawbank.ToString().Substring(1, banklen - 2);
                    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    dtrow["REQUEST ID"] = finalreqid;
                    dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                    au.TransactionID = finalemitraid;
                    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;
                    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    {
                        au.Trn_Status_Code = 1;
                        status1 = 1;
                    }
                    dt.Rows.Add(dtrow);
                    if (Session["requestId"] != null)
                    {
                        if (Session["UserId"] != null)
                        {
                            au.CreatedBy = Convert.ToInt64(Session["UserID"]);
                        }
                        au.RequestId = Session["requestId"].ToString();
                        au.PaymentType = Session["paymentType"].ToString();
                        au.UpdateTransactionStatus();
                    }
                }
                #endregion
                //SMS_EMail_Services SE = new SMS_EMail_Services();

                //if (Session["PaymentType"].ToString() == "FilmShooting")
                //{                 
                //    DataTable dtf = new DataTable();
                //    dtf=fs.UpdateTransactionStatus("1");
                //    if (dtf != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            string subject = "Request for Film Shooting Permission Review";
                //            string body = Common.GenerateReviwerBody(dtf.Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), "Film Shooting Permission");
                //            SE.sendEMail(subject, body, dtf.Rows[0]["EmailId"].ToString(), "");
                //            // SMS_EMail_Services.sendBulkSMS(dt.Rows[0]["Mobile"].ToString(), "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            if (Session["SSODetail"] != null)
                //            {
                //                UserProfile up = (UserProfile)Session["SSODetail"];
                //                SMS_EMail_Services.sendSingleSMS(up.MobileNumber, "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            }
                //        }
                //    }
                //}

                //cs.UpdateTransactionStatus("3");

                ViewData.Model = dt.AsEnumerable();
                return View("TransactionStatus");
            } return View();
        }



        /// <summary>
        /// For drop out Auction
        /// </summary>
        /// <returns></returns>
        public ActionResult DropOutAuction()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Auction auction = new Auction();
            Auction auObj = null;
            List<Auction> winnerlist = new List<Auction>();

            try
            {

                DataTable aucdt = new DataTable();
                aucdt = auction.BindAuctionWinner(Convert.ToInt64(Session["UserId"]));
                if (aucdt != null)
                {
                    if (aucdt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in aucdt.Rows)
                        {
                            auObj = new Auction()
                            {
                                AuctionId = Convert.ToInt64(dr["RowID"].ToString()),
                                NoticeNo = Convert.ToString(dr["NOTICE_NUMBER"]),
                                RangeName = Convert.ToString(dr["RANGE_NAME"]),
                                PlaceofAuction = Convert.ToString(dr["DEPOT_NAME"]),
                                DepotIncharge = Convert.ToString(dr["DEPOT_INCHARGE"]),
                                ProduceFor = Convert.ToString(dr["PRODUCETYPE"]),
                                ForestProduce = Convert.ToString(dr["PRODUCTNAME"]),
                                StockQuantity = Convert.ToString(dr["QUANTITY"]),
                                ProductUnit = Convert.ToString(dr["UNIT"]),
                                Qty = Convert.ToString(dr["LEFTQUANTITY"]),
                                BiddingAmount = Convert.ToDecimal(dr["BIDDINGAMOUNT"]),
                            };
                            winnerlist.Add(auObj);
                        }
                    }
                }

                ViewData["winnerList"] = winnerlist;
                ViewBag.DropOut_Reason = new SelectList(Common.GetDropOutReason(), "Value", "Text");

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View();
        }


        /// <summary>
        /// Save dropout Data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitDropout(FormCollection fm, string Command)
        {
            ActionResult actionResult = null;
            try
            {

                Auction auction = new Auction();

                if (Session["UserID"] != null)
                {

                    auction.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["Hdn_AuId"].ToString() == "")
                {
                    auction.AuctionId = 0;
                }
                else
                {
                    auction.AuctionId = Convert.ToInt64(fm["Hdn_AuId"].ToString());
                }

                if (fm["Notice_No"].ToString() == "")
                {
                    auction.NoticeNo = "";
                }
                else
                {
                    auction.NoticeNo = fm["Notice_No"].ToString();
                }

                if (fm["DropOut_Reason"].ToString() == "")
                {
                    auction.DropOutReason = "";
                }
                else
                {
                    auction.DropOutReason = fm["DropOut_Reason"].ToString();
                }

                if (Command == "Save")
                {
                    Int64 status = auction.DropoutAuction();
                    if (!String.IsNullOrEmpty(status.ToString()))
                    {
                        TempData["Status"] = "Your Request Successfully Drop-Out";
                    }
                    else
                    {
                        TempData["Status"] = "Not inserted";
                    }

                    actionResult = RedirectToAction("DropOutAuction", "Auction");


                    //actionResult = View("Payment", tp);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actionResult;

        }

        /// <summary>
        /// select Auction detail
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult GetAuctionDetail(string noticeId)
        {


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<NoticeManagement> result = new List<NoticeManagement>();
            List<AuctionRevenueMaster> reveList = new List<AuctionRevenueMaster>();
            NoticeManagement obj_no = null;
            AuctionRevenueMaster _objRev = null;
            NoticeManagement obj = new NoticeManagement();
            NoticeManagement noticeManagement = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(noticeId))
                {
                    DataTable dt = obj.BindNoticeNo(Convert.ToInt64(noticeId), "VIEW");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        obj_no = new NoticeManagement();
                        foreach (DataRow dr in dt.Rows)
                        {

                            obj_no.NoticeNo = dr["Notice_Number"].ToString();
                            //obj_no.RegionCode = dr["REG_NAME"].ToString();
                            //obj_no.CircleCode = dr["CIRCLE_NAME"].ToString();
                            //obj_no.DivisionCode = dr["DIV_NAME"].ToString();
                            obj_no.RangeCode = dr["RANGE_NAME"].ToString();
                            obj_no.DepotName = dr["Depot_Name"].ToString();
                            obj_no.ForestProduce = dr["ProduceType"].ToString();
                            obj_no.ForestProduct = dr["ProductName"].ToString();
                            obj_no.Qty = dr["Quantity"].ToString();
                            obj_no.ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString());
                            DateTime _date1 = DateTime.Parse(dr["DurationFrom"].ToString());
                            DateTime _date2 = DateTime.Parse(dr["DurationTo"].ToString());
                            obj_no.BiddOpeningDate = _date1.ToString("dd/MM/yyyy");
                            obj_no.BidClosingDate = _date2.ToString("dd/MM/yyyy");
                            obj_no.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");


                        }

                        DataTable dtview = obj.PublishNoticeview(Convert.ToInt64(noticeId), "View");
                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            string notice = dtview.Rows[0][0].ToString();
                            obj_no.NoticeView = notice;
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

                        DataTable reveDt = obj.BindNoticeNo(Convert.ToInt64(noticeId), "REVENUE");

                        if (reveDt != null)
                        {
                            if (reveDt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in reveDt.Rows)
                                {
                                    _objRev = new AuctionRevenueMaster()
                                    {

                                        RevenueYear = dr["Revenue_Year"].ToString(),
                                        Qty = dr["Revenue_Qty"].ToString(),
                                        RevinueAmount = dr["Revenue_Amount"].ToString(),

                                    };
                                    reveList.Add(_objRev);
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

            return Json(new { list1 = obj_no, list2 = sb.ToString(), list3 = reveList }, JsonRequestBehavior.AllowGet);


        }


        /// <summary>
        /// select Auction detail
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult GetRevenueDetail(string rangeCode, string depotId, string producetype, string product, bool IsAuctionClosed)
        {


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<AuctionRevenueMaster> reveList = new List<AuctionRevenueMaster>();
            NoticeManagement obj_no = null;
            AuctionRevenueMaster _objRev = null;
            NoticeManagement obj = new NoticeManagement();
            List<NoticeManagement> listAu = new List<NoticeManagement>();

            StringBuilder sb = new StringBuilder();
            try
            {


                DataTable reveDt = obj.LastThreeYearReveneu(rangeCode, Convert.ToInt64(depotId), Convert.ToInt64(producetype), Convert.ToInt64(product));

                if (reveDt != null)
                {
                    if (reveDt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in reveDt.Rows)
                        {
                            _objRev = new AuctionRevenueMaster()
                            {

                                RevenueYear = dr["Revenue_Year"].ToString(),
                                Qty = dr["Revenue_Qty"].ToString(),
                                ProduceUnit = dr["UnitName"].ToString(),
                                RevinueAmount = dr["Revenue_Amount"].ToString(),

                            };
                            reveList.Add(_objRev);
                        }

                    }


                }

                DataTable dt = notice.BindDropdownNoticeNotice(Session["AuctionType"].ToString(), rangeCode, Convert.ToInt64(depotId), Convert.ToInt64(producetype), Convert.ToInt64(product), IsAuctionClosed);
                if (dt != null && dt.Rows.Count >= 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        listAu.Add(new NoticeManagement()
                        {
                            RowID = Convert.ToInt64(dr["RowID"].ToString()),
                            NoticeId = Convert.ToInt64(dr["id"]),
                            NoticeNo = dr["NOTICE_NUMBER"].ToString(),
                            RangeCode = dr["RANGE_NAME"].ToString(),
                            DepotName = dr["Depot_Name"].ToString(),
                            prodName = dr["ProductName"].ToString(),
                            ProduceUnit = dr["Unit"].ToString(),
                            DateFrom = dr["DurationFrom"].ToString(),
                            DateTo = dr["DurationTo"].ToString(),

                            // IsActive = Convert.ToInt16(dr["IsActive"].ToString()),

                        });
                    }



                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { list = reveList, list1 = listAu }, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// Drop Auction
        /// </summary>
        /// <param name="AuctionId"></param>
        /// <returns></returns>
        public JsonResult AddDropout(string AuctionId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            NoticeManagement obj_no = null;
            NoticeManagement obj = new NoticeManagement();
            try
            {
                if (!String.IsNullOrEmpty(AuctionId))
                {
                    DataTable dt = obj.BindNoticeNo(Convert.ToInt64(AuctionId), "AUC");
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            obj_no = new NoticeManagement();
                            obj_no.BidderName = dr["BidderName"].ToString();
                            obj_no.BiddingAmount = dr["BiddingAmount"].ToString();
                            obj_no.NoticeNo = dr["Notice_Number"].ToString();
                            obj_no.RegionCode = dr["REG_NAME"].ToString();
                            obj_no.CircleCode = dr["CIRCLE_NAME"].ToString();
                            obj_no.DivisionCode = dr["DIV_NAME"].ToString();
                            obj_no.RangeCode = dr["RANGE_NAME"].ToString();
                            obj_no.DepotName = dr["Depot_Name"].ToString();
                            obj_no.ForestProduce = dr["ProduceType"].ToString();
                            obj_no.Qty = dr["Quantity"].ToString();
                            obj_no.ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString());
                            DateTime _date1 = DateTime.Parse(dr["DurationFrom"].ToString());
                            DateTime _date2 = DateTime.Parse(dr["DurationTo"].ToString());
                            obj_no.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");


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
        /// Save data into database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitAuctionForm(FormCollection fm, HttpPostedFileBase DdchkFile, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            ActionResult actionResult = null;
            try
            {
                string DdchkFileOrg = string.Empty;
                var path = "";
                Auction auction = new Auction();
                auction.AuctionId = 0;
                DateTime now = DateTime.Now;
                string requesteId = now.Ticks.ToString();

                if (Session["UserID"] != null)
                {

                    auction.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (requesteId == "")
                {
                    auction.RequestId = "";
                }
                else
                {
                    auction.RequestId = requesteId;
                }

                if (fm["Applicant_type"].ToString() == "")
                {
                    auction.Applicant_Type = "";
                }
                else
                {
                    auction.Applicant_Type = fm["Applicant_type"].ToString();

                }

                if (fm["NoticeId"].ToString() == "")
                {
                    auction.NoticeId = 0;
                }
                else
                {
                    auction.NoticeId = Convert.ToInt64(fm["NoticeId"].ToString());
                }

                if (fm["Hd_NO"].ToString() == "")
                {
                    auction.NoticeNo = "";
                }
                else
                {
                    auction.NoticeNo = fm["Hd_NO"].ToString();
                }

                if (fm["BidderName"].ToString() == "")
                {
                    auction.BidderName = "";
                }
                else
                {
                    auction.BidderName = fm["BidderName"].ToString();
                }

                if (fm["BiddingAmount"].ToString() == "")
                {
                    auction.BiddingAmount = 0;
                }
                else
                {
                    auction.BiddingAmount = Convert.ToDecimal(fm["BiddingAmount"].ToString());
                }
                if (fm["BiddOpeningDate"].ToString() == "")
                {
                    auction.DurationFrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    auction.DurationFrom = DateTime.ParseExact(fm["BiddOpeningDate"].ToString(), "dd/MM/yyyy", null);

                }
                if (fm["BidClosingDate"].ToString() == "")
                {
                    auction.DurationTo = Convert.ToDateTime(SqlDateTime.Null);

                }
                else
                {
                    auction.DurationTo = DateTime.ParseExact(fm["BidClosingDate"].ToString(), "dd/MM/yyyy", null);

                }


                if (fm["PaymentMode"].ToString() == "")
                {
                    auction.PaymentMode = "";
                }
                else
                {
                    auction.PaymentMode = fm["PaymentMode"].ToString();
                }

                if (fm["OfflinePaymentMode"].ToString() == "")
                {
                    auction.OfflinePaymentMode = "";
                }
                else
                {
                    auction.OfflinePaymentMode = fm["OfflinePaymentMode"].ToString();
                }

                if (fm["BankName"].ToString() == "")
                {
                    auction.BankName = "";
                }
                else
                {
                    auction.BankName = fm["BankName"].ToString();
                }

                if (fm["BranchName"].ToString() == "")
                {
                    auction.BranchName = "";
                }
                else
                {
                    auction.BranchName = fm["BranchName"].ToString();
                }
                if (fm["DdchkIssuesDate"].ToString() == "")
                {
                    auction.DdchkIssuesDate = "";

                }
                else
                {
                    auction.DdchkIssuesDate = fm["DdchkIssuesDate"].ToString();

                }

                if (fm["DdChkNumber"].ToString() == "")
                {
                    auction.DdChkNumber = "";
                }
                else
                {
                    auction.DdChkNumber = fm["DdChkNumber"].ToString();
                }

                if (DdchkFile != null && DdchkFile.ContentLength > 0)
                {
                    DdchkFileOrg = Path.GetFileName(DdchkFile.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DdchkFileOrg;
                    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    auction.DdchkFile = FileFullName;

                    auction.DdchkFilepth = @"~/PermissionDocument/" + FileFullName.Trim();
                    DdchkFile.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    auction.DdchkFile = "";
                    auction.DdchkFilepth = "";
                }


                if (fm["EmdPaybleAmount"].ToString() == "")
                {
                    auction.EmdPaybleAmount = 0;
                }
                else
                {
                    auction.EmdPaybleAmount = Convert.ToDecimal(fm["EmdPaybleAmount"].ToString());
                }
                if (fm["PsPaybleAmount"].ToString() == "")
                {
                    auction.PsPaybleAmount = 0;
                }
                else
                {
                    auction.PsPaybleAmount = Convert.ToDecimal(fm["PsPaybleAmount"].ToString());
                }

                if (Command == "Save")
                {
                    DataTable dt = auction.IsApplyedAuction(auction.CreatedBy, auction.NoticeId);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TempData["Status"] = "You have Allready Apply Application of Selected Notice Number.";
                        actionResult = RedirectToAction("Auction", "Auction");
                    }
                    else
                    {
                        Int64 status = auction.SubmitAuction();
                        if (!String.IsNullOrEmpty(status.ToString()))
                        {
                            TempData["Status"] = "Your Request id Successfully added in Database and Request Id: " + requesteId;
                            if (auction.PaymentMode != null && auction.PaymentMode == "Online")
                            {
                                auction.OfflinePaymentMode = "";
                                #region payment
                                DataTable dtColmn = new DataTable();
                                if (dtColmn.Rows.Count == 0)
                                {
                                    dtColmn.Columns.Add("Request_Id");
                                    dtColmn.Columns.Add("NoticeNo");
                                    dtColmn.Columns.Add("Name");
                                    dtColmn.Columns.Add("PaidAmt");
                                    dtColmn.Columns.Add("Status");
                                }
                                //decimal biddingAmt = 0;

                                DataRow dtrow = dtColmn.NewRow();
                                Session["paymentType"] = "EMD";
                                dtrow["Request_Id"] = auction.RequestId;
                                Session["requestId"] = auction.RequestId;
                                dtrow["NoticeNo"] = auction.NoticeNo;
                                dtrow["Name"] = Session["User"].ToString(); ;
                                dtrow["PaidAmt"] = auction.EmdPaybleAmount;
                                Session["totalprice"] = auction.EmdPaybleAmount;
                                dtrow["Status"] = "Pending";

                                dtColmn.Rows.Add(dtrow);
                                ViewData.Model = dtColmn.AsEnumerable();

                                #endregion

                                actionResult = View("AuctionPayment");
                            }
                            else
                            {
                                actionResult = RedirectToAction("Auction", "Auction");
                            }

                            if (Session["SSODetail"] != null)
                            {
                                UserProfile user = (UserProfile)Session["SSODetail"];

                                if (user != null)
                                {
                                    if (!String.IsNullOrEmpty(user.EmailId) && !String.IsNullOrEmpty(user.MobileNumber))
                                    {
                                        //string smsBody = Common.GenerateSMSBody(Session["User"].ToString(), auction.RequestId, "Auction");
                                        //SMS_EMail_Services.sendSingleSMS(user.MobileNumber, smsBody);
                                        //string CitizenMailBody = Common.GenerateBody(0, Session["User"].ToString(), auction.RequestId, "Auction");
                                        //_objMail.sendEMail("Request for " + "Auction" + " Apply", CitizenMailBody, user.EmailId.ToString(), "");

                                    }
                                }

                            }

                        }
                        else
                        {
                            TempData["Status"] = "Not inserted";
                        }


                    }


                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return actionResult;

        }


    }
}
