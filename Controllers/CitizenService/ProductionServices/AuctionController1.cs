using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.ProductionServices;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class AuctionController : Controller
    {
        //
        // GET: /Auction/

        NoticeManagement notice = new NoticeManagement();

        List<SelectListItem> items = new List<SelectListItem>();
        
        public ActionResult Auction()
        {
            #region Notice
            DataTable dt = new DataTable();
            dt = notice.BindDropdownNoticeNo();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["Notice_Number"].ToString(), Value = @dr["Id"].ToString() });
            }

            ViewBag.NoticeId = items;

            ViewBag.Applicant_Type = new SelectList(Common.GetApplicantType(), "Value", "Text");

            #endregion


            if (Session["User"] != null)
            {

                ViewData["Name"] = Session["User"];
            }

            return View();
           
        }


        public ActionResult DropOutAuction()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
         [HttpPost]
        public JsonResult GetAuctionDetail(string noticeId)
        {


            NoticeManagement obj_no = null;
            NoticeManagement obj = new NoticeManagement();
            try
            {
                if (!String.IsNullOrEmpty(noticeId))
                {
                    DataTable dt = obj.BindNoticeNo(Convert.ToInt64(noticeId), "VIEW");
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            obj_no = new NoticeManagement();
                            obj_no.NoticeNo = dr["Notice_Number"].ToString();
                            obj_no.RegionCode = dr["REG_NAME"].ToString();
                            obj_no.CircleCode = dr["CIRCLE_NAME"].ToString();
                            obj_no.DivisionCode = dr["DIV_NAME"].ToString();
                            obj_no.RangeCode = dr["RANGE_NAME"].ToString();
                            obj_no.DepotName = dr["Depot_Name"].ToString();
                            obj_no.ForestProduce = dr["Forest_Produce"].ToString();
                            obj_no.Qty = dr["Quantity"].ToString();
                            obj_no.ReservedPrice = Convert.ToDecimal(dr["ReservedPrice"].ToString());

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return Json(obj_no, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
         public ActionResult SubmitAuctionForm(FormCollection fm, string Command)
         {
             ActionResult actionResult = null;
             try
             {
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
                     auction.BiddingAmount = "";
                 }
                 else
                 {
                     auction.BiddingAmount = fm["BiddingAmount"].ToString();
                 }

                 if (Command == "Save")
                 {
                     Int64 status = auction.SubmitAuction();
                     if (!String.IsNullOrEmpty(status.ToString()))
                     {
                         TempData["Status"] = "Your Request id Successfully added in Database and Request Id: " + requesteId;
                     }
                     else
                     {
                         TempData["Status"] = "Not inserted";
                     }

                     actionResult = RedirectToAction("Auction", "Auction");
                     

                     //actionResult = View("Payment", tp);
                 }

                 
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return actionResult;

         }


    }
}
