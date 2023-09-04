//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI Manage  Auction
//  Date Created : 05-Jun-2016
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  : Arvind Srivastava  
//  Modified On  : 26-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//Bug no: 407,411,412,413


//*********************************************************************************************************@



using FMDSS.Models;
using FMDSS.Models.CitizenService.ProductionServices;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestProduction
{
    public class ManageAuctionController : BaseController
    {
        //  //Note: Code Updated with Ref. to bug ID 456
        // GET: /ManageAuction/
        NoticeManagement notice = new NoticeManagement();

        List<SelectListItem> items = new List<SelectListItem>();
        List<SelectListItem> items1 = new List<SelectListItem>();
        List<NoticeManagement> listAu = new List<NoticeManagement>();
        List<SelectListItem> rangeList = new List<SelectListItem>();
        Int64 UserID = 0;
        int ModuleID = 1;
        /// <summary>
        /// Call when request come from ManageAuction view Bind Notice dropdown
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageAuction(string aid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                aid = Encryption.decrypt(aid);
                if (aid == "1")
                    Session["AuctionType"] = "TenduPatta";
                else
                    Session["AuctionType"] = "Timber and Fuelwood";

                #region Notice

                DataTable dt = new DataTable();
                Auction au = new Auction();
                dt = au.FetchWinners(Convert.ToString(Session["AuctionType"]), "VIEW");
                foreach (DataRow dr in dt.Rows)
                {
                    listAu.Add(new NoticeManagement()
                    {
                        RowID = Convert.ToInt64(dr["ROWID"].ToString()),
                        RangeCode = dr["RANGE_NAME"].ToString(),
                        DepotName = dr["DEPOT_NAME"].ToString(),
                        NoticeNo = dr["Notice_Number"].ToString(),
                        prodName = dr["PRODUCTNAME"].ToString(),
                        Qty = dr["QUANTITY"].ToString(),
                        ProduceUnit = dr["UNIT"].ToString(),
                        DateFrom = dr["NAME"].ToString(),
                        DateTo = dr["BIDDINGAMOUNT"].ToString(),
                    });
                }
                ViewData["lstInventory"] = listAu;
               
                dt = notice.BindDropdownNoticeNo(Session["AuctionType"].ToString(), Convert.ToInt64(Session["UserId"]), true);
                if (dt != null && dt.Rows.Count >= 1)
                {
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        listAu.Add(new NoticeManagement()
                    //        {
                    //            RowID = Convert.ToInt64(dr["RowID"].ToString()),
                    //            NoticeNo = dr["NOTICE_NUMBER"].ToString(),
                    //            RangeCode = dr["RANGE_NAME"].ToString(),
                    //            DepotName = dr["Depot_Name"].ToString(),
                    //            prodName = dr["ProductName"].ToString(),
                    //            ProduceUnit = dr["Unit"].ToString(),
                    //            DateFrom = dr["DurationFrom"].ToString(),
                    //            DateTo = dr["DurationTo"].ToString(),
                    //        });
                    //    }

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
                    rangeList.Add(new SelectListItem { Text = "", Value = "-1" });
                    ViewBag.RangeCode = rangeList;
                    TempData["Status"] = "There is no active Auction Notice in the system to select Winner!";
                }

                #endregion

                DataTable bidderdt = notice.BindBidder(Session["AuctionType"].ToString());
                ViewBag.fname1 = bidderdt;
                foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                {
                    items1.Add(new SelectListItem { Text = @dr["userName"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.BidderName = items1;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        /// <summary>
        /// Bind  All Auction bidder
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuctionManagementForm(FormCollection fm, HttpPostedFileBase Receiptdoc, string Command)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null;
            string DocMOMfile = string.Empty;
            var DocMOMfilepath = "";

            try
            {

                Auction au = new Auction();

                au.AuctionId = 0;
                DateTime now = DateTime.Now;
                string requesteId = now.Ticks.ToString();
                au.RequestId = requesteId;

                if (Session["UserId"] != null)
                {
                    au.CreatedBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["NoticeId"].ToString() == "")
                {
                    au.NoticeId = 0;
                }
                else
                {
                    au.NoticeId = Convert.ToInt64(fm["NoticeId"].ToString());
                }

                if (fm["Bidder_ID"].ToString() == "")
                {
                    au.Bidder_ID = 0;
                }
                else
                {
                    au.Bidder_ID = Convert.ToInt64(fm["Bidder_ID"].ToString());
                }

                if (fm["BiddingAmount"].ToString() == "")
                {
                    au.BiddingAmount = 0;
                }
                else
                {
                    au.BiddingAmount = Convert.ToDecimal(fm["BiddingAmount"].ToString());
                }

                if (Receiptdoc != null && Receiptdoc.ContentLength > 0)
                {
                    DocMOMfile = Path.GetFileName(Receiptdoc.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + DocMOMfile;
                    DocMOMfilepath = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    au.ReceiptDoc = FileFullName;

                    au.ReceipDocpath = @"~/PermissionDocument/" + FileFullName.Trim();
                    Receiptdoc.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                {
                    au.ReceiptDoc = "";
                    au.ReceipDocpath = "";
                }


                if (Command == "Submit")
                {

                    Int64 status = au.SubmitAuctionWinnerDetail(Convert.ToString(Session["AuctionType"]), "INSERT");

                    if (status > 0)
                    {
                        TempData["Winner_Status"] = "Auction Winner has been selected Sucessfully";
                        actionResult = RedirectToAction("ManageAuction", "ManageAuction");
                    }
                    else
                    {
                        TempData["Winner_Status"] = "Winner has already been updated into the system!";
                        actionResult = RedirectToAction("ManageAuction", "ManageAuction");
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
