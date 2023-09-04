using FMDSS.Entity.NPVM;
using FMDSS.Models.BookOnlineTicket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.MIS
{
    public class MISNationalParkController : BaseController
    {
        FMDSS.Models.DAL dl = new Models.DAL();

        #region Report

        #region Booking Details
        public ActionResult BookingReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtGetPlaces = GetDropDownData(Convert.ToInt32(FMDSS.NPDropDownActionCode.Place));
            if (dtGetPlaces != null && dtGetPlaces.Rows.Count > 0)
            {
                ViewBag.PlaceList = dtGetPlaces.AsEnumerable().Select(a => new PlaceList
                {
                    PlaceId = a.Field<Int64>("PlaceId"),
                    PlaceName = a.Field<string>("PlaceName")
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult BookingReport(OBookingReportVM param)
        {
            DataSet ds = GetBookingDetails(param);
            if (ds.Tables.Count > 0)
            {
                var bookingPassDetails = ds.Tables[0].AsEnumerable().Select(a => new OBookingDetails
                {
                    TicketId = a.Field<Int64>("Id"),
                    RequestId = a.Field<string>("RequestId"),
                    BookingType = a.Field<string>("BookingType"),
                    VisitDate = a.Field<DateTime>("VisitingDate"),
                    BookingDate = a.Field<DateTime>("BookingDateTime"),
                    TotalMember = a.Field<int>("TotalMember"),
                    TotalAmountBePay = a.Field<decimal>("TotalAmountBePay"),
                    EmitraAmount = a.Field<decimal>("EmitraAmount"),
                    TotalPaidAmount = a.Field<decimal>("TotalPaidAmount"),
                    EmitraTransactionId = a.Field<string>("EmitraTransactionId"),
                    PlaceName = a.Field<string>("PlaceName"),
                    GuideName = a.Field<string>("GuideName"),
                    VehicleNumber = a.Field<string>("VehicleNumber"),
                    BoardingPassStatus = a.Field<bool>("BoardingPassStatus"),
                    BookingStatus = (BookingStatusForReport)a.Field<byte>("BookingStatus"),
                    Shift = a.Field<string>("Shift"),
                    SSOID = a.Field<string>("SSOID")
                }).ToList();
                return PartialView("_BookingReport", bookingPassDetails);
            }
            return null;
        }

        private DataSet GetBookingDetails(OBookingReportVM param)
        {
            SqlParameter[] prms = {
                                        new SqlParameter("@ActionCode",1),
                                        new SqlParameter("@DateType",param.DateType),
                                        new SqlParameter("@PlaceId",param.PlaceId),
                                        new SqlParameter("@FromDate",param.FromDate),
                                        new SqlParameter("@ToDate",param.ToDate),
                                        new SqlParameter("@BookingType",param.BookingType),
                                        new SqlParameter("@BookingStatus",param.BookingStatus),
                                   };
            DataSet ds = new DataSet();
            dl.Fill(ds, "spNP_RPTBookingDetails", prms);
            return ds;
        }
        private DataTable GetDropDownData(int actionCode)
        {
            SqlParameter[] param = { new SqlParameter("@ActionCode", actionCode) };
            DataTable DT = new DataTable();
            dl.Fill(DT, "NP_GetDropdownData", param);
            return DT;
        }

        #endregion

        #region Member Details of booking
        public ActionResult MemberDetailsReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtGetPlaces = GetDropDownData(Convert.ToInt32(FMDSS.NPDropDownActionCode.Place));
            if (dtGetPlaces != null && dtGetPlaces.Rows.Count > 0)
            {
                ViewBag.PlaceList = dtGetPlaces.AsEnumerable().Select(a => new PlaceList
                {
                    PlaceId = a.Field<Int64>("PlaceId"),
                    PlaceName = a.Field<string>("PlaceName")
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult MemberDetailsReport(OBookingReportVM param)
        {
            DataSet ds = GetMemberDetails(param);
            if (ds.Tables.Count > 0)
            {
                var bookingPassDetails = ds.Tables[0].AsEnumerable().Select(a => new OBookingMemberDetails
                {
                    TicketId = a.Field<Int64>("Id"),
                    MemberName = a.Field<string>("MemberName"),
                    RequestId = a.Field<string>("RequestId"),
                    VisitDate = a.Field<DateTime>("VisitingDate"),
                    BookingDate = a.Field<DateTime>("BookingDateTime"),
                    TotalMember = a.Field<int>("TotalMember"),
                    TotalAmountBePay = a.Field<decimal>("TotalAmountBePay"),
                    EmitraAmount = a.Field<decimal>("EmitraAmount"),
                    TotalPaidAmount = a.Field<decimal>("TotalPaidAmount"),
                    EmitraTransactionId = a.Field<string>("EmitraTransactionId"),
                    VisitorType = a.Field<string>("VisitorType"),
                    PlaceName = a.Field<string>("PlaceName"),
                    GuideName = a.Field<string>("GuideName"),
                    VehicleNumber = a.Field<string>("VehicleNumber"),
                    BoardingPassStatus = a.Field<bool>("BoardingPassStatus"),
                    BookingStatus = (BookingStatusForReport)a.Field<byte>("BookingStatus")
                }).ToList();
                return PartialView("_MemberDetailsReport", bookingPassDetails);
            }
            return null;
        }

        private DataSet GetMemberDetails(OBookingReportVM param)
        {
            SqlParameter[] prms = {
                                        new SqlParameter("@ActionCode",1),
                                        new SqlParameter("@DateType",param.DateType),
                                        new SqlParameter("@PlaceId",param.PlaceId),
                                        new SqlParameter("@FromDate",param.FromDate),
                                        new SqlParameter("@ToDate",param.ToDate),
                                        new SqlParameter("@BookingType",param.BookingType),
                                        new SqlParameter("@BookingStatus",param.BookingStatus),
                                         new SqlParameter("@VisitorType",param.VisitorType)
                                   };
            DataSet ds = new DataSet();
            dl.Fill(ds, "spNP_RPTMemberDetails", prms);
            return ds;
        }

        #endregion

        #region Booking Summary Details
        public ActionResult BookingSummaryReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtGetPlaces = GetDropDownData(Convert.ToInt32(FMDSS.NPDropDownActionCode.Place));
            if (dtGetPlaces != null && dtGetPlaces.Rows.Count > 0)
            {
                ViewBag.PlaceList = dtGetPlaces.AsEnumerable().Select(a => new PlaceList
                {
                    PlaceId = a.Field<Int64>("PlaceId"),
                    PlaceName = a.Field<string>("PlaceName")
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult BookingSummaryReport(OBookingReportVM param)
        {
            DataSet ds = GetBooingSummaryDetails(param);
            if (ds.Tables.Count > 0)
            {
                var bookingPassDetails = ds.Tables[0].AsEnumerable().Select(a => new OBookingSummaryDetails
                {
                    StudentVisitors = a.Field<Int32>("StudentVisitors"),
                    IndianVisitors = a.Field<Int32>("IndianVisitors"),
                    NonIndianVisitors = a.Field<Int32>("NonIndianVisitors"),
                    StudentVisitorsHeadAmt = a.Field<decimal>("StudentVisitorsHeadAmt"),
                    IndianVisitorsHeadAmt = a.Field<decimal>("IndianVisitorsHeadAmt"),
                    NonIndianVisitorsHeadAmt = a.Field<decimal>("NonIndianVisitorsHeadAmt"),
                    EmitraCharges = a.Field<decimal>("EmitraCharges"),
                    HeadAmount = a.Field<decimal>("HeadAmount"),
                    HeadName = a.Field<string>("HeadName"),
                    GrandTotal = a.Field<decimal>("GrandTotal"),
                    FromDate = param.FromDate,
                    ToDate = param.ToDate
                }).FirstOrDefault();
                return PartialView("_BookingSummaryReport", bookingPassDetails);
            }
            return null;
        }

        private DataSet GetBooingSummaryDetails(OBookingReportVM param)
        {
            SqlParameter[] prms = {
                                        new SqlParameter("@ActionCode",2),
                                        new SqlParameter("@DateType",param.DateType),
                                        new SqlParameter("@PlaceId",param.PlaceId),
                                        new SqlParameter("@FromDate",param.FromDate),
                                        new SqlParameter("@ToDate",param.ToDate),
                                        new SqlParameter("@BookingType",param.BookingType),
                                        new SqlParameter("@BookingStatus",param.BookingStatus),
                                   };
            DataSet ds = new DataSet();
            dl.Fill(ds, "spNP_RPTBookingDetails", prms);
            return ds;
        }
        #endregion

        #endregion

    }
}
