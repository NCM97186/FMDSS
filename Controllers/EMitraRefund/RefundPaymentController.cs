using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.EMitraReFund;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.Master;
using FMDSS.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using FMDSS.Models.CitizenRefunds;

namespace FMDSS.Controllers.EMitraRefund
{
    public class RefundPaymentController : Controller
    {
        //
        // GET: /RefundPayment/

        public ActionResult WildLifeCancellationBookingList()
        {
            bindDropdownsForWildLifeBookingList();
            return View();
        }
        public void bindDropdownsForWildLifeBookingList()
        {
            List<SelectListItem> DateTypeList = new List<SelectListItem>();
            List<SelectListItem> PlacesList = new List<SelectListItem>();
            List<SelectListItem> TypeOfBookingList = new List<SelectListItem>();
            List<SelectListItem> StatusList = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            Zone cst = new Zone();

            DateTypeList.Add(new SelectListItem { Text = "Date of Visit", Value = "DATEOFVISITE" });
            DateTypeList.Add(new SelectListItem { Text = "Date of Booking", Value = "DATEOFBOOKING" });

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                FMDSS.Models.OnlineBooking.CS_BoardingDetails obj1 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                DataSet Ds = new DataSet();
                Ds = obj1.BindDptKioskPLACES(Session["SSOid"].ToString());

                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                {
                    if (dr["PlaceID"].ToString() != "1" || dr["PlaceID"].ToString() != "62")
                    {
                        PlacesList.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                }

                TypeOfBookingList.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Advance Booking", Value = "1" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Current Booking", Value = "2" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Advance Half/Full Day Booking", Value = "3" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Current Half/Full Day Booking", Value = "4" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Tatkal Booking", Value = "5" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Special Category Booking", Value = "6" });

                StatusList.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                StatusList.Add(new SelectListItem { Text = "Success(To be visited)", Value = "1" });
                StatusList.Add(new SelectListItem { Text = "Success(Visited)", Value = "2" });
                StatusList.Add(new SelectListItem { Text = "Success(Not visited)", Value = "3" });
                StatusList.Add(new SelectListItem { Text = "Cancelled", Value = "4" });
                StatusList.Add(new SelectListItem { Text = "Failed", Value = "5" });
                StatusList.Add(new SelectListItem { Text = "Failed(In Reconcilation process)", Value = "6" });
                StatusList.Add(new SelectListItem { Text = "Covid Rescheduled", Value = "7" });
                StatusList.Add(new SelectListItem { Text = "Covid-19 Refund", Value = "8" });
            }
            else
            {
                dtPlace = cst.SelectPlaces();
                foreach (System.Data.DataRow dr in dtPlace.Rows)
                {
                    if (dr["PlaceID"].ToString() != "1" && dr["PlaceID"].ToString() != "62")
                    {
                        PlacesList.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                }

                TypeOfBookingList.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Advance Booking", Value = "1" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Current Booking", Value = "2" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Advance Half/Full Day Booking", Value = "3" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Current Half/Full Day Booking", Value = "4" });
                TypeOfBookingList.Add(new SelectListItem { Text = "Tatkal Booking", Value = "5" });

                StatusList.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                StatusList.Add(new SelectListItem { Text = "Success(To be visited)", Value = "1" });
                StatusList.Add(new SelectListItem { Text = "Success(Visited)", Value = "2" });
                StatusList.Add(new SelectListItem { Text = "Success(Not visited)", Value = "3" });
                StatusList.Add(new SelectListItem { Text = "Cancelled", Value = "4" });
                StatusList.Add(new SelectListItem { Text = "Failed", Value = "5" });
                StatusList.Add(new SelectListItem { Text = "Failed(In Reconcilation process)", Value = "6" });
                StatusList.Add(new SelectListItem { Text = "Covid Rescheduled", Value = "7" });
                StatusList.Add(new SelectListItem { Text = "Covid-19 Refund", Value = "8" });
            }
            ViewBag.ddlDateType = DateTypeList;
            ViewBag.ddlPlace = PlacesList;
            ViewBag.ddlTypeOfBooking = TypeOfBookingList;
            ViewBag.ddlStatus = StatusList;
        }

        [HttpPost]
        public JsonResult WildLifeCancellationBookingList(string DateType, string FromDate, string ToDate, string Place, string TypeOfBooking, string Status)
        {
            WildLifeBookingFilterModel WBFM = new WildLifeBookingFilterModel();
            WBFM.DateType = DateType;
            WBFM.FromDate = FromDate;
            WBFM.ToDate = ToDate;
            WBFM.Place = Place;
            WBFM.TypeOfBooking = TypeOfBooking;
            WBFM.Status = Status;
            JsonResult result = new JsonResult();
            //WildLifeBookingFilterModel WBFM = new WildLifeBookingFilterModel();
            BookOnTicket BOT = new BookOnTicket();
            DataSet dataSet = new DataSet();
            try
            {

                // Initialization.  
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                // Loading.
                int NextFetch = pageSize;
                if (startRec > 0)
                {
                    NextFetch = ((startRec / pageSize) + 1) * pageSize;
                }
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    WBFM.StartRow = 0;
                    WBFM.FetchRowsNext = 0;
                    WBFM.search = search;
                }
                else
                {
                    WBFM.StartRow = startRec + 1;
                    WBFM.FetchRowsNext = NextFetch;
                    WBFM.search = search;
                }

                dataSet = BOT.WildLifebookedTicketListForAdmin(WBFM);


                WBFM.WildLifeBookingListModel = Globals.Util.GetListFromTableInCitizenList<WildLifeBookingListModel>(dataSet.Tables[0]);
                DataTable dataTable = dataSet.Tables[1];
                int RowCount = Convert.ToInt32(dataSet.Tables[1].Rows[0][0].ToString());
                // Total record count.  
                int totalRecords = RowCount;
                int recFilter = 0;
                // Verification.  
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search                    
                    //data = data.Where(p => p.PlantationLocationNameEng.ToString().ToLower().Contains(search.ToLower()) || p.UserNameEng.ToString().ToLower().Contains(search.ToLower()) || p.MobileNo.ToString().ToLower().Contains(search.ToLower())).ToList();
                    recFilter = WBFM.WildLifeBookingListModel.Count;
                    WBFM.WildLifeBookingListModel = WBFM.WildLifeBookingListModel.Skip(startRec).Take(pageSize).ToList(); //ok code

                }
                else
                {
                    recFilter = RowCount;
                }

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = WBFM.WildLifeBookingListModel
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            // Return info.  
            return result;
        }

        [HttpGet]
        public ActionResult RefundRequest(string ticketid, string TypeOfActions = "0", string xctz = "", string fhdfdhddf = "")
        {

            BookOnTicket cs = new BookOnTicket();

            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();
            int fullRefund = 0;
            string isCitizen = "";
            string isFullRefund = "";
            if (fhdfdhddf.Length > 0)
            {
                isFullRefund = Encryption.decrypt(fhdfdhddf);
                fullRefund = 1;
            }
            else
                isFullRefund = "false";

            if (xctz.Length > 0)
                isCitizen = Encryption.decrypt(xctz);

            DT = cs.Get_BookTicket_ForRefundProcess(Encryption.decrypt(ticketid), fullRefund, isCitizen);
            cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));
            if (DT.Rows.Count > 0)
            {
                cs.RequestId = Convert.ToString(DT.Rows[0]["Transaction_Id"]);
                cs.TotalMemberFees = Convert.ToString(DT.Rows[0]["MemberFee"]);
                cs.TotalCameraFees = Convert.ToString(DT.Rows[0]["CameraFees"]);
                cs.TotalSafariFees = Convert.ToString(DT.Rows[0]["SafariFees"]);
                cs.VehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFee"]);
                cs.GSTonVehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFeeGST"]);
                cs.GuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideFee"]);
                cs.GSTGuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideRentFeeGST"]);
                cs.TicketAmount = Convert.ToString(DT.Rows[0]["TicketAmount"]);
                cs.EmitraCharges = Convert.ToString(DT.Rows[0]["EMitraCharges"]);
                cs.GrandTotal = Convert.ToString(DT.Rows[0]["TotalAmount"]);
                cs.SSOID = Convert.ToString(DT.Rows[0]["Ssoid"]);
                cs.RefundAmount = Convert.ToDecimal(DT.Rows[0]["RefundAmount"]);

            }
            cs.TypeOfActions = Convert.ToInt32(TypeOfActions);
            EmitraReFund obj = new EmitraReFund();
            DT2 = obj.Get_CancellationReasonList();
            List<SelectListItem> CancellationReasonList = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in DT2.Rows)
            {
                CancellationReasonList.Add(new SelectListItem { Text = @dr["CancelReason"].ToString(), Value = @dr["CancelReasonId"].ToString() });
            }
            ViewBag.CancellationReasonList = CancellationReasonList;

            ViewBag.IsCitizen = isCitizen;
            if (isCitizen.Length > 0)
            {
                cs.CancellationRemarks = "Refund Generated by Citizen";
                cs.CancellationReason = 1;
            }


            cs.IsFullRefund = false;

            if (isFullRefund == "true")
                cs.IsFullRefund = true;

            return View(cs);

            //}
            //else
            //{
            //    return RedirectToAction("~/DelayTicket.html"); //Response.Redirect("~/DelayTicket.html");
            //}

        }

        [HttpPost]
        public ActionResult RefundRequest(BookOnTicket cs)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();
            EmitraReFund obj = new EmitraReFund();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT = (liveUat == 0 ? false : true);
            cs.IsForcefullyAppliedByAdmin = false;

            //cs.IsFullRefund = false;
            DT = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);

            //DT = cs.SubmitFor_BookTicket_ForRefundProcess(cs); 

            if (DT.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                //objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");

                #endregion


                TempData["RefundSubmissionMsg"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }


            return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket", new { CT = "QWR2YW5jZQ ==" });


        }
        [HttpGet]
        public ActionResult FullRefundRequest(string ticketid, string TypeOfActions = "0")
        {

            BookOnTicket cs = new BookOnTicket();

            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();

            DT = cs.Get_BookTicket_ForRefundProcess(Encryption.decrypt(ticketid), 1);
            cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));
            if (DT.Rows.Count > 0)
            {
                cs.RequestId = Convert.ToString(DT.Rows[0]["Transaction_Id"]);
                cs.TotalMemberFees = Convert.ToString(DT.Rows[0]["MemberFee"]);
                cs.TotalCameraFees = Convert.ToString(DT.Rows[0]["CameraFees"]);
                cs.TotalSafariFees = Convert.ToString(DT.Rows[0]["SafariFees"]);
                cs.VehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFee"]);
                cs.GSTonVehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFeeGST"]);
                cs.GuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideFee"]);
                cs.GSTGuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideRentFeeGST"]);
                cs.TicketAmount = Convert.ToString(DT.Rows[0]["TicketAmount"]);
                cs.EmitraCharges = Convert.ToString(DT.Rows[0]["EMitraCharges"]);
                cs.GrandTotal = Convert.ToString(DT.Rows[0]["TotalAmount"]);
                cs.SSOID = Convert.ToString(DT.Rows[0]["Ssoid"]);
                cs.RefundAmount = Convert.ToDecimal(DT.Rows[0]["RefundAmount"]);

            }
            cs.TypeOfActions = Convert.ToInt32(TypeOfActions);
            EmitraReFund obj = new EmitraReFund();
            DT2 = obj.Get_CancellationReasonList();
            List<SelectListItem> CancellationReasonList = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in DT2.Rows)
            {
                CancellationReasonList.Add(new SelectListItem { Text = @dr["CancelReason"].ToString(), Value = @dr["CancelReasonId"].ToString() });
            }
            ViewBag.CancellationReasonList = CancellationReasonList;
            return View(cs);

            //}
            //else
            //{
            //    return RedirectToAction("~/DelayTicket.html"); //Response.Redirect("~/DelayTicket.html");
            //}

        }

        [HttpPost]
        public ActionResult FullRefundRequest(BookOnTicket cs)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();
            EmitraReFund obj = new EmitraReFund();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT = (liveUat == 0 ? false : true);
            cs.IsForcefullyAppliedByAdmin = false;
            cs.IsFullRefund = true;
            DT = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);
            //DT2 = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);
            //DT = cs.SubmitFor_BookTicket_ForRefundProcess(cs);

            if (DT.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                // objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                #endregion


                TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }

            return RedirectToAction("WildLifeCancellationBookingList");


        }
        [HttpGet]
        public ActionResult RefundRequestMemberWise(string ticketid, string TypeOfActions = "0")
        {

            DataTable DT = new DataTable();
            BookOnTicket cs = new BookOnTicket();
            cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));
            DT = cs.Get_BookTicket_ForRefundProcessMemberWise(Encryption.decrypt(ticketid));
            List<BookOnTicket> List = new List<BookOnTicket>();

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                List.Add(
                       new BookOnTicket()
                       {
                           RequestId = Convert.ToString(DT.Rows[i]["RequestID"]),
                           TotalMemberFees = Convert.ToString(DT.Rows[i]["TotalMemberFee"]),
                           TotalCameraFees = Convert.ToString(DT.Rows[i]["TotalCameraFee"]),
                           TotalSafariFees = Convert.ToString(DT.Rows[i]["SafariFees"]),
                           VehicleRent = Convert.ToString(DT.Rows[i]["BoardingVehicleRent"]),
                           GSTonVehicleRent = Convert.ToString(DT.Rows[i]["BoardingVehicleFeeGSTAmount"]),
                           GuideFee = Convert.ToString(DT.Rows[i]["BoardingGuideFee"]),
                           GSTGuideFee = Convert.ToString(DT.Rows[i]["BoardingGuideFeeGSTAmount"]),
                           TicketAmount = Convert.ToString(DT.Rows[i]["TicketAmount"]),
                           GrandTotal = Convert.ToString(DT.Rows[i]["TicketAmount"]),
                           SSOID = Convert.ToString(DT.Rows[i]["Ssoid"]),
                           TicketID = Convert.ToInt64(DT.Rows[i]["TicketID"]),
                           MemberName = Convert.ToString(DT.Rows[i]["MemberName"]),
                           MemberIdNo = Convert.ToString(DT.Rows[i]["MemberIdNo"]),
                           RefundAmount = Convert.ToDecimal(DT.Rows[i]["RefundAmount"]),
                           MemberTableID = Convert.ToInt64(DT.Rows[i]["MemberID"])
                       });
            }
            cs.RequestId = Convert.ToString(DT.Rows[0]["RequestID"]);
            EmitraReFund obj = new EmitraReFund();

            DataTable DT2 = new DataTable();
            DT2 = obj.Get_CancellationReasonList();
            List<SelectListItem> CancellationReasonList = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in DT2.Rows)
            {
                CancellationReasonList.Add(new SelectListItem { Text = @dr["CancelReason"].ToString(), Value = @dr["CancelReasonId"].ToString() });
            }
            ViewBag.CancellationReasonList = CancellationReasonList;
            TempData["ListCurrentBookingMemberDetails"] = List;
            cs.TypeOfActions = Convert.ToInt32(TypeOfActions);
            return View(cs);
        }

        [HttpPost]
        public ActionResult RefundRequestMemberWise(BookOnTicket cs)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();
            EmitraReFund obj = new EmitraReFund();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT = (liveUat == 0 ? false : true);
            cs.IsForcefullyAppliedByAdmin = false;
            cs.IsFullRefund = false;
            DT2 = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);
            //DT = cs.SubmitFor_BookTicket_ForRefundProcessMemberWise(cs);

            //if (DT.Rows.Count > 0)
            if (DT2.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                //objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                #endregion


                TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }

            return RedirectToAction("WildLifeCancellationBookingList");


        }
        [HttpGet]
        public ActionResult ForcefullyCancelRequest(string ticketid, string TypeOfActions = "0")
        {

            BookOnTicket cs = new BookOnTicket();

            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();

            DT = cs.Get_BookTicket_ForRefundProcess(Encryption.decrypt(ticketid), 2); //2 for cancelled without refund
            cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));
            if (DT.Rows.Count > 0)
            {
                cs.RequestId = Convert.ToString(DT.Rows[0]["Transaction_Id"]);
                cs.TotalMemberFees = Convert.ToString(DT.Rows[0]["MemberFee"]);
                cs.TotalCameraFees = Convert.ToString(DT.Rows[0]["CameraFees"]);
                cs.TotalSafariFees = Convert.ToString(DT.Rows[0]["SafariFees"]);
                cs.VehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFee"]);
                cs.GSTonVehicleRent = Convert.ToString(DT.Rows[0]["BoardingVehicleRentFeeGST"]);
                cs.GuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideFee"]);
                cs.GSTGuideFee = Convert.ToString(DT.Rows[0]["BoardingGuideRentFeeGST"]);
                cs.TicketAmount = Convert.ToString(DT.Rows[0]["TicketAmount"]);
                cs.EmitraCharges = Convert.ToString(DT.Rows[0]["EMitraCharges"]);
                cs.GrandTotal = Convert.ToString(DT.Rows[0]["TotalAmount"]);
                cs.SSOID = Convert.ToString(DT.Rows[0]["Ssoid"]);
                cs.RefundAmount = Convert.ToDecimal(DT.Rows[0]["RefundAmount"]);

            }
            cs.TypeOfActions = Convert.ToInt32(TypeOfActions);
            EmitraReFund obj = new EmitraReFund();
            DT2 = obj.Get_CancellationReasonList();
            List<SelectListItem> CancellationReasonList = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in DT2.Rows)
            {
                CancellationReasonList.Add(new SelectListItem { Text = @dr["CancelReason"].ToString(), Value = @dr["CancelReasonId"].ToString() });
            }
            ViewBag.CancellationReasonList = CancellationReasonList;
            return View(cs);

            //}
            //else
            //{
            //    return RedirectToAction("~/DelayTicket.html"); //Response.Redirect("~/DelayTicket.html");
            //}

        }

        [HttpPost]
        public ActionResult ForcefullyCancelRequest(BookOnTicket cs)
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();
            EmitraReFund obj = new EmitraReFund();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT = (liveUat == 0 ? false : true);
            cs.IsForcefullyAppliedByAdmin = true;
            cs.IsFullRefund = false;
            DT = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);
            //DT2 = obj.Save_RefundAmount(cs, IsLiveOrUAT, UserID);
            //DT = cs.SubmitFor_BookTicket_ForRefundProcess(cs);

            if (DT.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                // objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                #endregion


                TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }

            return RedirectToAction("WildLifeCancellationBookingList");


        }

        public ActionResult BookingCan_eMitraMarkedBookings()
        {
            ViewBag.msgStatus = "";
            ViewBag.msg = "";
            if (TempData["msgStatus"] != null)
            {
                ViewBag.msgStatus = TempData["msgStatus"].ToString();
                ViewBag.msg = TempData["msg"].ToString();
            }
            bindDropdownsForWildLifeBookingList();
            return View();
        }

        [HttpPost]
        public JsonResult BookingCan_eMitraMarkedBookings(string FromDate, string ToDate, string Place, string TypeOfBooking)
        {
            WildLifeBookingFilterModel WBFM = new WildLifeBookingFilterModel();

            WBFM.FromDate = FromDate;
            WBFM.ToDate = ToDate;
            WBFM.Place = Place;
            WBFM.TypeOfBooking = TypeOfBooking;

            JsonResult result = new JsonResult();
            //WildLifeBookingFilterModel WBFM = new WildLifeBookingFilterModel();
            EmitraReFund obj = new EmitraReFund();

            DataSet dataSet = new DataSet();
            try
            {

                // Initialization.  
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                // Loading.
                int NextFetch = pageSize;
                if (startRec > 0)
                {
                    NextFetch = ((startRec / pageSize) + 1) * pageSize;
                }
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    WBFM.StartRow = 0;
                    WBFM.FetchRowsNext = 0;
                    WBFM.search = search;
                }
                else
                {
                    WBFM.StartRow = startRec + 1;
                    WBFM.FetchRowsNext = NextFetch;
                    WBFM.search = search;
                }

                dataSet = obj.BookingCancelledMarkedList(WBFM);


                WBFM.WildLifeBookingListModel = Globals.Util.GetListFromTable<WildLifeBookingListModel>(dataSet.Tables[0]);

                foreach (var itm in WBFM.WildLifeBookingListModel)
                {
                    itm.Actions = itm.Actions.Replace("#", "'" + Encryption.encrypt(Convert.ToString(itm.ID)) + "'");
                }


                DataTable dataTable = dataSet.Tables[1];
                int RowCount = Convert.ToInt32(dataSet.Tables[1].Rows[0][0].ToString());
                // Total record count.  
                int totalRecords = RowCount;
                int recFilter = 0;
                // Verification.  
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search                    
                    //data = data.Where(p => p.PlantationLocationNameEng.ToString().ToLower().Contains(search.ToLower()) || p.UserNameEng.ToString().ToLower().Contains(search.ToLower()) || p.MobileNo.ToString().ToLower().Contains(search.ToLower())).ToList();
                    recFilter = WBFM.WildLifeBookingListModel.Count;
                    WBFM.WildLifeBookingListModel = WBFM.WildLifeBookingListModel.Skip(startRec).Take(pageSize).ToList(); //ok code

                }
                else
                {
                    recFilter = RowCount;
                }

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = WBFM.WildLifeBookingListModel
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            // Return info.  
            return result;
        }
        [HttpGet]
        public virtual ActionResult DownloadExcelCanRefund()
        {
            string fileName = TempData["fileName"].ToString();
            string fullPath = Path.Combine(Server.MapPath("~/Content/TempData/"), fileName);
            return File(fullPath, "application/vnd.ms-excel", fileName);
        }
        [HttpPost]
        public JsonResult ExportExcelCanRefund(int PlaceId, string FromDate, string ToDate)
        {
            string fileName = "CancelRefundDetails" + DateTime.Now.Ticks.ToString() + ".xls";
            EmitraReFund obj = new EmitraReFund();
            DataTable data = obj.Download_SavedRefundDetails(PlaceId, FromDate, ToDate);
            DownloadExcel(data, fileName);
            TempData["fileName"] = fileName;
            return Json(fileName, JsonRequestBehavior.AllowGet);
        }
        private void DownloadExcel(DataTable dt, string fileName)
        {
            string filePath = Server.MapPath("~/Content/TempData/");
            StreamWriter wr = new StreamWriter(filePath + fileName);

            try
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }

                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //GridView gv = new GridView();
            //gv.DataSource = DT;
            //gv.DataBind();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename="+ fileName + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gv.RenderControl(htw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
        }
        public ActionResult RefundProcess(string Id)
        {
            Int64 intId = Convert.ToInt64(Encryption.decrypt(Id));
            //RefundServices refundServerices = new RefundServices();
            EmitraReFund obj = new EmitraReFund();
            //BookOnTicket OBJ = new BookOnTicket();
            DataTable data = obj.BookingCancelledMarkedRecord(intId);
            //string RequestId = "";
            //string TicketId = "";
            DataSet DS = new DataSet();

            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();


            //refundServerices = obj.Get_RefundService(false); //IsLive =1 or 0 for UAT
            Int64 ticketId = 0;
            string PaymentModeBId = "";
            string UserSSOId = "";
            string newRequestId = "";
            string REVENUEHEAD = "";
            decimal TotalRefundableAmt = 0;
            string eMitraTransactionId = "";
            string OfficeDivCode = "";
            string RefundType = "";
            foreach (DataRow dr in data.Rows)
            {



                ////-----------------------------------------------Refundable Amount-----------------------------------------------------
                if (REVENUEHEAD.Length > 0)
                {
                    REVENUEHEAD = REVENUEHEAD + "|" + dr["SurchargeHead"].ToString();
                }
                else
                    REVENUEHEAD = dr["SurchargeHead"].ToString();

                TotalRefundableAmt = TotalRefundableAmt + (dr["RemainingSurchargeAmt"] == null ? 0 : Convert.ToDecimal(dr["RemainingSurchargeAmt"].ToString()));

                if (REVENUEHEAD.Length > 0)
                {
                    REVENUEHEAD = REVENUEHEAD + "|" + dr["TigerProjectHead"].ToString();
                }
                else
                    REVENUEHEAD = dr["TigerProjectHead"].ToString();

                TotalRefundableAmt = TotalRefundableAmt + (dr["RemainingTigerProjectAmt"] == null ? 0 : Convert.ToDecimal(dr["RemainingTigerProjectAmt"].ToString()));

                if (REVENUEHEAD.Length > 0)
                {
                    REVENUEHEAD = REVENUEHEAD + "|" + dr["FoundationHead"].ToString();
                }
                else
                    REVENUEHEAD = dr["FoundationHead"].ToString();

                TotalRefundableAmt = TotalRefundableAmt + (dr["RemainingFoundationAmt"] == null ? 0 : Convert.ToDecimal(dr["RemainingFoundationAmt"].ToString()));

                if (REVENUEHEAD.Length > 0)
                {
                    REVENUEHEAD = REVENUEHEAD + "|" + dr["VehicleRentandGuidFeesHead"].ToString();
                }
                else
                    REVENUEHEAD = dr["VehicleRentandGuidFeesHead"].ToString();

                TotalRefundableAmt = TotalRefundableAmt + (dr["RemainingVehicleRentandGuidFees"] == null ? 0 : Convert.ToDecimal(dr["RemainingVehicleRentandGuidFees"].ToString()));

                eMitraTransactionId = dr["TRANSACTIONID"].ToString();

                //// ----------------------------------------------------------------------------------------------------------------------
                //newRequestId = dr["Id"].ToString() + "" + dr["TicketId"].ToString();
                newRequestId = DateTime.Now.Ticks.ToString() + "_" + dr["TicketId"].ToString();
                ticketId = Convert.ToInt64(dr["TicketId"].ToString());
                if (string.IsNullOrEmpty(dr["PaymentModeBId"].ToString()) == false)
                {
                    PaymentModeBId = dr["PaymentModeBId"].ToString();
                }
                OfficeDivCode =  dr["Div_Code"].ToString();
                RefundType = dr["RefundType"].ToString(); 
            }

            //if(PaymentModeBId=="" || PaymentModeBId == null)
            //{
            string UserID = "";
            string UserName = "";
            string oldReceiptNo = "";
            string oldServiceId = "";
            DataTable dt = obj.GetPaymentModeBId(ticketId);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    PaymentModeBId = dt.Rows[0][0].ToString().Replace(@"\", "");
                    UserSSOId = "CITIZENOFRAJASTHAN";// dt.Rows[0][1].ToString();
                    UserID = dt.Rows[0][2].ToString();
                    UserName = dt.Rows[0][3].ToString();
                    oldReceiptNo = dt.Rows[0][4].ToString();
                    oldServiceId = dt.Rows[0][5].ToString();
                    eMitraTransactionId = dt.Rows[0][6].ToString();
                }

            }

            //}
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT = (liveUat == 0 ? false : true);
            string response = "";
            string[] spl ;
            spl = new string[2];
            string EmitraRefundResponse = "";
            if (RefundType== "PartialRefund")
            {
                response = obj.RefundRequestOldType(UserName, UserID, UserSSOId, newRequestId, REVENUEHEAD, eMitraTransactionId, PaymentModeBId, ticketId, TotalRefundableAmt, IsLiveOrUAT, oldReceiptNo, oldServiceId, OfficeDivCode,out EmitraRefundResponse);
                spl = response.Split('|');
            }
                 
            //PGBackToBackResponse response = obj.RefundRequest(UserSSOId, newRequestId, REVENUEHEAD,  eMitraTransactionId,PaymentModeBId,ticketId);
            
           

            ReconciliationRepo repo = new ReconciliationRepo();
            EmitraReponse emitraresponse = new EmitraReponse();
            if (RefundType == "FullRefund")
            {
                if(liveUat==1)
                    emitraresponse = repo.EmitraCancelationProcess(eMitraTransactionId, "2239", "SET",Convert.ToInt64( UserID));
                else
                    emitraresponse = repo.EmitraCancelationProcess(eMitraTransactionId, "2349", "SET", Convert.ToInt64(UserID));

                spl[0] = emitraresponse.message;
                spl[1] = "0";
                if (emitraresponse.status == "200")
                    spl[1] = "1";

                EmitraRefundResponse = Newtonsoft.Json.JsonConvert.SerializeObject(emitraresponse);
            }
             
            TempData["msgStatus"] = "0";
            if (Convert.ToInt16(spl[1]) == 1)
            {                
                TempData["msg"] = "Refund initiated successfully";
                TempData["msgStatus"] = "1";
            }
            TempData["msg"] = spl[0];
            obj.UpdateRefundResponse(ticketId, spl[0], spl[1], newRequestId, EmitraRefundResponse);
            return RedirectToAction("BookingCan_eMitraMarkedBookings", "RefundPayment");
            //return new EmptyResult();
        }
        public ActionResult RefundResponse(FormCollection formCollection)
        {
            var response = formCollection;
            return View();
        }
        //string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";


        //            if (Request.Form["MERCHANTCODE"] != null)
        //                MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
        //            if (Request.Form["PRN"] != null)
        //                PRN = Request.Form["PRN"].ToString();
        //            if (Request.Form["STATUS"] != null)
        //                STATUS = Request.Form["STATUS"].ToString();
        //            if (Request.Form["ENCDATA"] != null)
        //                ENCDATA = Request.Form["ENCDATA"].ToString();

        ////EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23"); //Live
        //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

        //string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
        //// string DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";

        //// string DecryptedData = "{'MERCHANTCODE':'FOREST0117', 'REQTIMESTAMP':'20190614131735000' ,'PRN':'636961150514796677' ,'AMOUNT':'1994.00' ,'PAIDAMOUNT':'2000.00', 'SERVICEID':'2239' ,'TRANSACTIONID':'190208589073', 'RECEIPTNO':'19199639988' ,'EMITRATIMESTAMP':'20190614131843682', 'STATUS':'SUCCESS', 'PAYMENTMODE':'BILLDESK(RPP)', 'PAYMENTMODEBID':'22889757' ,'PAYMENTMODETIMESTAMP':'20190614131741000' ,'RESPONSECODE':'200', 'RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.', 'UDF1':'BHUPENDRA SHARMA' ,'UDF2':'udf2', 'CHECKSUM':'c1b14b96b845327065053366e11abd39'}";

        //PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);
        [HttpGet]
        public ActionResult PartialRefundRequestList()
        {
            ViewBag.Msg = "";
            ViewBag.msgStatus = "-1";
            if(TempData["msgStatus"]!=null)
            {
                if (Convert.ToInt16(TempData["msgStatus"]) != -1)
                {
                    ViewBag.Msg = TempData["Msg"].ToString();
                    ViewBag.msgStatus = TempData["msgStatus"].ToString();                     
                }
            }
            CitizenRefunds citizenRefund = new CitizenRefunds();
            CitizenRefundViews citizen = new CitizenRefundViews();
            citizen.citizenRefundDetails = citizenRefund.GetCitizenRefundDetails(Session["SSOid"].ToString(), 7);//7 IS USED FOR Partial Cancelliation  
            return View(citizen);
        }
        [HttpGet]
        public ActionResult ApplyPartialRefundRequest(string Id)
        {
            
           

            Id = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(Id);
            string[] spl = Id.Split('|');
            CitizenRefundViews citizen = new CitizenRefundViews();
            citizen.bankDetail = new BankDetails();
            citizen.bankDetail.TicketId =Convert.ToInt64(spl[0]);
            citizen.bankDetail.RefundableAmount = Convert.ToDecimal(spl[1]);
            citizen.bankDetail.RequestId =  spl[2];
            return View(citizen);
        }
        
        [HttpPost]
        public ActionResult ApplyPartialRefundRequest(CitizenRefundViews citizen)
        {
            ViewBag.Msg = "";
            ViewBag.msgStatus = "-1";
            TempData["Msg"] = "";
            TempData["msgStatus"] =-1;
            bool msgStatus = false;
            if (ModelState.IsValid)
            {
                CitizenRefunds citizenRefund = new CitizenRefunds();
                string strMsg= citizenRefund.SaveBankDetails(Session["SSOid"].ToString(), citizen.bankDetail, out msgStatus);
                TempData["Msg"] = strMsg;
                ViewBag.Msg = strMsg;
                TempData["msgStatus"] = 0;
                ViewBag.msgStatus = "0";
                if (msgStatus == true)
                {
                    TempData["msgStatus"] = 1;                   
                    ViewBag.msgStatus = "1";
                }
                    
            }
            if (msgStatus == true)
                return RedirectToAction("PartialRefundRequestList");
            else
                return View(citizen);
        }
        [HttpGet]
        public ActionResult ViewBankDetails(string Id)
        {

            //GetAppliedPartialRefundDetail

            Id = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(Id);
            string[] spl = Id.Split('|');
            CitizenRefundViews citizen = new CitizenRefundViews();
            citizen.bankDetail = new BankDetails();
            citizen.bankDetail.TicketId = Convert.ToInt64(spl[0]);
            citizen.bankDetail.RefundableAmount = Convert.ToDecimal(spl[1]);
            CitizenRefunds citizenRefund = new CitizenRefunds();
            citizen.bankDetail = citizenRefund.GetBankDetails(Session["SSOid"].ToString(), Convert.ToInt64(spl[0]));
            return View(citizen.bankDetail);
        }
    }
}
