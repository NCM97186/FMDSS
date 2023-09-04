using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models;
using System.Configuration;
using Newtonsoft.Json;
using FMDSS.Models.CitizenService.PermissionService;
using System.Net;
using FMDSS.LIBS;
using System.IO;
using FMDSS.Models.NP_ChoiceGuideVehicleBoat;
namespace FMDSS.Controllers.BookOnlineTicket
{
    public class NationalParkController : BaseController
    {
        FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
        int ModuleID = 3;
        string captchaPrefix = "npbooking";
        private BookingType BookingType
        {
            get
            {
                if (Session["IsDepartmentalKioskUser"] != null && Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
                {
                    return BookingType.DepartmentBooking;
                }
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]))
                {
                    return BookingType.EmitraKioskBooking;
                }
                else
                {
                    return BookingType.OnlineCitizenBooking;
                }
            }
        }

        public ActionResult PlaceList()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable DT = GetUserPlaces(UserID);
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows.Count == 1)
                {
                    return Redirect("~/onlinebooking/" + Convert.ToString(DT.Rows[0]["URL"]));
                }
                else
                {
                    ViewBag.PlaceList = DT.AsEnumerable().Select(a => new PlaceList
                    {
                        PlaceId = a.Field<Int64>("PlaceId"),
                        PlaceName = a.Field<string>("PlaceName"),
                        URL = a.Field<string>("URL")
                    }).ToList();
                }
            }
            else
            {
                //Unauthenticate access
            }
            return View();
        }

        public ActionResult Index()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string route = Convert.ToString(RouteData.Values["id"]);
            if ((BookingType == BookingType.OnlineCitizenBooking && string.IsNullOrEmpty(route)) || (BookingType != BookingType.OnlineCitizenBooking && !string.IsNullOrEmpty(route)))
            {
                DataSet DS = GetUserBookings(UserID, 500, Convert.ToString(RouteData.Values["id"]), BookingType);
                var bookingList = new List<UserBooking>();
                if (DS.Tables[0].Rows.Count > 0)
                {
                    bookingList = DS.Tables[0].AsEnumerable().Select(a => new UserBooking
                    {
                        TicketId = a.Field<Int64>("Id"),
                        RequestId = a.Field<string>("RequestId"),
                        VisitDate = a.Field<DateTime>("VisitingDate"),
                        BookingDate = a.Field<DateTime>("BookingDateTime"),
                        TotalMember = a.Field<int>("TotalMember"),
                        TotalPaidAmount = a.Field<decimal>("TotalPaidAmount"),
                        PlaceName = a.Field<string>("PlaceName"),
                        BookingStatus = (BookingStatus)a.Field<byte>("BookingStatus"),
                        //COVIDStatus = a.Field<int>("COVIDStatus"),
                        //RefundStatus = a.Field<int>("RefundStatus"),
                        //TicketMemberBordingStatus = a.Field<int>("TicketMemberBordingStatus"),
                    }).ToList();
                }
                if (bookingList.Count > 0)
                    ViewBag.BookingList = bookingList;

                ViewBag.Btype = (byte)BookingType;
                ViewBag.BookingType =Convert.ToInt16(BookingType.OnlineCitizenBooking);
                if (BookingType != BookingType.OnlineCitizenBooking)
                {
                    if (DS.Tables.Count > 1)
                    {
                        if (DS.Tables[1].Rows.Count == 0)
                        {
                            return RedirectToAction("placelist");
                        }
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("placelist");
            }
        }

        [HttpPost]
        public ActionResult getTickets(int? placeid, int? zoneid, int? shiftid, int? vehicleid, int? visitors, string selDate)
        { 
            DataSet DS = GetMemberRows(placeid, zoneid, shiftid, vehicleid, selDate, BookingType);
            DataSet DS2 = GetNormalOdhiAvailibility(placeid, zoneid, shiftid, vehicleid, selDate, BookingType);
            if (DS.Tables.Count > 3)
            {
                int totalRows = 0, RowsAllotedVehicleWise = 0, actualRow = 0;

                if (visitors == null)
                {

                     totalRows = DS.Tables[0].Rows.Count > 0 ? Convert.ToInt32(DS.Tables[0].Rows[0]["Seats"]) : (visitors.HasValue ? visitors.Value : 6);
                     RowsAllotedVehicleWise = DS.Tables[0].Rows.Count > 0 ? Convert.ToInt32(DS.Tables[0].Rows[0]["SeatAlloted"]) : 0;

                     actualRow = totalRows > RowsAllotedVehicleWise ? RowsAllotedVehicleWise : totalRows;
                    ViewBag.totalRows = totalRows;
                }
                else 
                {
                    totalRows = DS.Tables[0].Rows.Count > 0 ? Convert.ToInt32(DS.Tables[0].Rows[0]["Seats"]) : (visitors.HasValue ? visitors.Value : 6);
                    actualRow = totalRows > (visitors.HasValue ? visitors.Value : 6) ? (visitors.HasValue ? visitors.Value : 6) : totalRows;
                    ViewBag.totalRows = totalRows;
                }
                List<TicketData> lst = Enumerable.Range(1, actualRow).Select(n => new TicketData { Gender = 0, VisitorTypeId = 0, NofVideoCamera = 0, NoOfStillCamera = 0, IDType = 0 }).ToList();

                if (DS.Tables[1].Rows.Count > 0)
                {
                    var columnList = DS.Tables[1].AsEnumerable().Select(a => new DisplayColumn
                    {
                        ItemParent = (ItemParent)a.Field<byte>("ItemParent"),
                        ItemId = a.Field<int>("ItemId"),
                        PlaceId = a.Field<int>("PlaceId")
                    }).ToList();
                    ViewBag.DisplayColumn = columnList;
                }

                var IDType = DS.Tables[2].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("NAME"),
                    Value = a.Field<Int16>("ID").ToString()

                }).ToList();
                ViewBag.IDType = IDType;

                var VisitorType = DS.Tables[3].AsEnumerable().Where(a => a.Field<int>("Display") > 0).ToList();
                if (VisitorType.Count == 0)
                    VisitorType = DS.Tables[3].AsEnumerable().ToList();



                if (shiftid == 16 || shiftid == 17)
                {
                    ViewBag.VisitorType = VisitorType.Select(a => new SelectListItem
                    {
                        Text = a.Field<string>("NAME"),
                        Value = a.Field<int>("ID").ToString()
                    }).ToList().Where(c => c.Value != "3"); //Student Not Occurs in Half day
                }
                else
                {
                    ViewBag.VisitorType = VisitorType.Select(a => new SelectListItem
                    {
                        Text = a.Field<string>("NAME"),
                        Value = a.Field<int>("ID").ToString()
                    }).ToList();
                }

                ViewBag.BookingType = BookingType;
                ViewBag.CaptchaPrefix = captchaPrefix;
                ViewBag.NormalOdhiAvailibility = DS2.Tables[0].Rows[0][0].ToString();
                ViewBag.OdhiCharge = DS2.Tables[1].Rows[0][0].ToString();
                ViewBag.PlaceId = placeid;
                return PartialView("_getTickets", lst);
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Ticket tkt, List<TicketData> lst, FormCollection FC)
        {
            string error = "";
            string IsSunday = "";
            bool isInDateRange = true;
            DateTime vDate = DateTime.Parse(tkt.VisitingDate);
            string currentYear = DateTime.Now.Year.ToString();
            DateTime fdate = DateTime.Parse("01/07/" + currentYear);
            DateTime tdate = DateTime.Parse("30/09/" + currentYear);
            if (vDate >= fdate && vDate <= tdate && (tkt.PlaceId == 75 || tkt.PlaceId == 49))
            {
                isInDateRange = false;
            }
            if (((BookingType == BookingType.OnlineCitizenBooking && !string.IsNullOrEmpty(Convert.ToString(Session["Captcha" + captchaPrefix])) && Convert.ToString(Session["Captcha" + captchaPrefix]) == FC["captchavalue"]) // Check only for citizen booking
                || (BookingType != BookingType.OnlineCitizenBooking)) && isInDateRange == true) // Bypass captcha check and process booking if booking type other than citizen)
            {
                try
                {
                    #region Added by shaan to check arrival date is not equal to sunday ---29-06-2021
                   
                    if (BookingType.OnlineCitizenBooking == BookingType)
                    {
                        DateTime bookingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                        var diff = vDate - bookingDate;
                        int dayDiff = diff.Days;
                        if (tkt.PlaceId == 74 && dayDiff < 2) //Palighat
                        {
                            error = "Visiting date must be greater or more than 2 days ahead from the current date";
                            return Json(new { error = error, status = false, respone = "" }, JsonRequestBehavior.AllowGet);
                        }
                        if (tkt.PlaceId == 19 && dayDiff < 1) //Palighat
                        {
                            error = "Visiting date must be greater or more than 1 days ahead from the current date";
                            return Json(new { error = error, status = false, respone = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    #endregion

                  

                    List<TicketData> tickets = new List<TicketData>();
                    foreach (var item in lst)
                    {
                        if (item.Name != null && item.Name.Trim() != "" && item.Gender > 0 && item.VisitorTypeId > 0 && item.IDType > 0 && item.IDNo != null && item.IDNo.Trim() != "")
                        {
                            tickets.Add(item);
                        }
                    }
                    //if (string.IsNullOrEmpty(IsSunday))
                    //{
                        if (tickets.Count() > 0)
                        {
                            tkt.BookingType = (byte)BookingType;
                            tkt.VisitingDate = Convert.ToDateTime(tkt.VisitingDate).ToString("MM/dd/yyyy");

                            tkt.EnteredBy = Convert.ToInt32(Convert.ToString(Session["UserId"]));
                            string result = string.Empty;
                            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (!string.IsNullOrEmpty(ip))
                            {
                                string[] ipRange = ip.Split(',');
                                int le = ipRange.Length - 1;
                                result = ipRange[0];
                            }
                            else
                            {
                                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            }
                            tkt.IPAddress = result;
                        int IsOdhi = 0;
                        int SafariType = 0; //0=Normal Booking,1 Premium Type
                        if (tkt.ShiftId == 16 || tkt.ShiftId == 17)
                        {
                            // When Half Day then Odhi Charges Included Automatically
                            if (tkt.ZoneId == 17)
                            {
                                IsOdhi = 1;
                            }

                            DataTable dt = new DataTable("TktData");
                            dt.Columns.Add(new DataColumn("PlaceId", typeof(int)));
                            dt.Columns.Add(new DataColumn("VehicleId", typeof(int)));
                            dt.Columns.Add(new DataColumn("VisitingDate", typeof(string)));
                            dt.Columns.Add(new DataColumn("BookingType", typeof(string)));
                            dt.Columns.Add(new DataColumn("ZoneId", typeof(int)));
                            dt.Columns.Add(new DataColumn("ShiftId", typeof(int)));
                            dt.Columns.Add(new DataColumn("UserId", typeof(long)));
                            dt.Columns.Add(new DataColumn("IPAddress", typeof(string)));
                            dt.Columns.Add(new DataColumn("ReserveStatus", typeof(char)));


                            //						@PlaceId = PlaceID, @VehicleId = IIF(VehicleId = 0, NULL, VehicleId), @VisitingDate = VisitingDate, @BookingType = BookingType, 				
                            //@ZoneId = IIF(ZoneId = 0, NULL, ZoneId), @ShiftId = IIF(ShiftId = 0, NULL, ShiftId), @UserId = EnteredBy, @IPAddress = IPAddress

                            DataRow row = dt.NewRow();
                            row["PlaceId"] = tkt.PlaceId;
                            row["VehicleId"] = tkt.VehicleId;
                            row["VisitingDate"] = tkt.VisitingDate;
                            row["BookingType"] = tkt.BookingType;
                            row["ZoneId"] = tkt.ZoneId;
                            row["ShiftId"] = tkt.ShiftId;
                            row["UserId"] = tkt.EnteredBy;
                            row["IPAddress"] = tkt.IPAddress;
                            row["ReserveStatus"] = tkt.ReserveStatus;
                            dt.Rows.Add(row);

                            StringWriter sw = new StringWriter();
                            dt.WriteXml(sw);
                            string xmlTkt = sw.ToString();
                            //----------------------------------------------------------------
                            //	[VisitorTypeId] [int] NULL,
                            //[Name] [nvarchar] (50) NULL,
                            //[Gender] [tinyint] NULL,
                            //[IDType] [tinyint] NULL,
                            //[IDNo] [nvarchar] (50) NULL,
                            //[NoOfStillCamera] [int] NULL,
                            //[StillCameraAmount] [decimal](18, 2) NULL,
                            //[NofVideoCamera] [int] NULL,
                            //[VideoCameraAmount] [decimal](18, 2) NULL,
                            //[MemberFees] [decimal](18, 2) NULL,
                            //[VechileFees] [decimal](18, 2) NULL,
                            //[VechileFeesGST] [decimal](18, 2) NULL,
                            //[VechileFeesGSTAmount] [decimal](18, 2) NULL,
                            //[GuideFees] [decimal](18, 2) NULL,
                            //[GuideFeesGST] [decimal](18, 2) NULL,
                            //[GuideFeesGSTAmount] [decimal](18, 2) NULL
                            DataTable dtTicketDtl = new DataTable("TicketDetail");
                            dtTicketDtl.Columns.Add(new DataColumn("VisitorTypeId", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("Name", typeof(string)));
                            dtTicketDtl.Columns.Add(new DataColumn("Gender", typeof(Int16)));
                            dtTicketDtl.Columns.Add(new DataColumn("IDType", typeof(Int16)));
                            dtTicketDtl.Columns.Add(new DataColumn("IDNo", typeof(string)));
                            dtTicketDtl.Columns.Add(new DataColumn("NoOfStillCamera", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("StillCameraAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("NofVideoCamera", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("VideoCameraAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("MemberFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFeesGST", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFeesGSTAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFeesGST", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFeesGSTAmount", typeof(decimal)));
                            foreach (var obj2 in tickets)
                            {
                                DataRow row2 = dtTicketDtl.NewRow();
                                row2["VisitorTypeId"] = obj2.VisitorTypeId;
                                row2["Name"] = obj2.Name;
                                row2["Gender"] = obj2.Gender;
                                row2["IDType"] = obj2.IDType;
                                row2["IDNo"] = obj2.IDNo;
                                row2["NoOfStillCamera"] = obj2.NoOfStillCamera;
                                row2["StillCameraAmount"] = obj2.StillCameraAmount;
                                row2["NofVideoCamera"] = obj2.NofVideoCamera;
                                row2["VideoCameraAmount"] = obj2.VideoCameraAmount;
                                row2["MemberFees"] = obj2.MemberFees;
                                row2["VechileFees"] = obj2.VechileFees;
                                row2["VechileFeesGST"] = obj2.VechileFeesGST;
                                row2["VechileFeesGSTAmount"] = obj2.VechileFeesGSTAmount;
                                row2["GuideFees"] = obj2.GuideFees;
                                row2["GuideFeesGST"] = obj2.GuideFeesGST;
                                row2["GuideFeesGSTAmount"] = obj2.GuideFeesGSTAmount;
                                dtTicketDtl.Rows.Add(row2);
                            }

                            StringWriter sw2 = new StringWriter();
                            dtTicketDtl.WriteXml(sw2);
                            string xmlTktList = sw2.ToString();

                            //SqlParameter[] param = {new SqlParameter("@TicketData",Utility.ObjectToData(tkt)),
                            //					new SqlParameter("@TicketDetail",Utility.ToDataTable<TicketData>(tickets)),
                            //					new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0))
                            //					};
                            SqlParameter[] param = {new SqlParameter("@xmlTkt",xmlTkt),
                                                new SqlParameter("@xmlTktList",xmlTktList),
                                                new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0))
                                                };
                            //Book Ticket

                            dl.Fill_WithoutCommit2(ds, "spNP_BookTicketHalfDay", param);
                        }
                        else
                        {
                            DataTable dt = new DataTable("TktData");
                            dt.Columns.Add(new DataColumn("PlaceId", typeof(int)));
                            dt.Columns.Add(new DataColumn("VehicleId", typeof(int)));
                            dt.Columns.Add(new DataColumn("VisitingDate", typeof(string)));
                            dt.Columns.Add(new DataColumn("BookingType", typeof(string)));
                            dt.Columns.Add(new DataColumn("ZoneId", typeof(int)));
                            dt.Columns.Add(new DataColumn("ShiftId", typeof(int)));
                            dt.Columns.Add(new DataColumn("UserId", typeof(long)));
                            dt.Columns.Add(new DataColumn("IPAddress", typeof(string)));
                            dt.Columns.Add(new DataColumn("ReserveStatus", typeof(char)));


                            //						@PlaceId = PlaceID, @VehicleId = IIF(VehicleId = 0, NULL, VehicleId), @VisitingDate = VisitingDate, @BookingType = BookingType, 				
                            //@ZoneId = IIF(ZoneId = 0, NULL, ZoneId), @ShiftId = IIF(ShiftId = 0, NULL, ShiftId), @UserId = EnteredBy, @IPAddress = IPAddress

                            DataRow row = dt.NewRow();
                            row["PlaceId"] = tkt.PlaceId;
                            row["VehicleId"] = tkt.VehicleId;
                            row["VisitingDate"] = tkt.VisitingDate;
                            row["BookingType"] = tkt.BookingType;
                            row["ZoneId"] = tkt.ZoneId;
                            row["ShiftId"] = tkt.ShiftId;
                            row["UserId"] = tkt.EnteredBy;
                            row["IPAddress"] = tkt.IPAddress;
                            row["ReserveStatus"] = tkt.ReserveStatus;
                            dt.Rows.Add(row);

                            StringWriter sw = new StringWriter();
                            dt.WriteXml(sw);
                            string xmlTkt = sw.ToString();
                            //----------------------------------------------------------------

                            DataTable dtTicketDtl = new DataTable("TicketDetail");
                            dtTicketDtl.Columns.Add(new DataColumn("VisitorTypeId", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("Name", typeof(string)));
                            dtTicketDtl.Columns.Add(new DataColumn("Gender", typeof(Int16)));
                            dtTicketDtl.Columns.Add(new DataColumn("IDType", typeof(Int16)));
                            dtTicketDtl.Columns.Add(new DataColumn("IDNo", typeof(string)));
                            dtTicketDtl.Columns.Add(new DataColumn("NoOfStillCamera", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("StillCameraAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("NofVideoCamera", typeof(int)));
                            dtTicketDtl.Columns.Add(new DataColumn("VideoCameraAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("MemberFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFeesGST", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("VechileFeesGSTAmount", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFees", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFeesGST", typeof(decimal)));
                            dtTicketDtl.Columns.Add(new DataColumn("GuideFeesGSTAmount", typeof(decimal)));
                            foreach (var obj2 in tickets)
                            {
                                DataRow row2 = dtTicketDtl.NewRow();
                                row2["VisitorTypeId"] = obj2.VisitorTypeId;
                                row2["Name"] = obj2.Name;
                                row2["Gender"] = obj2.Gender;
                                row2["IDType"] = obj2.IDType;
                                row2["IDNo"] = obj2.IDNo;
                                row2["NoOfStillCamera"] = obj2.NoOfStillCamera;
                                row2["StillCameraAmount"] = obj2.StillCameraAmount;
                                row2["NofVideoCamera"] = obj2.NofVideoCamera;
                                row2["VideoCameraAmount"] = obj2.VideoCameraAmount;
                                row2["MemberFees"] = obj2.MemberFees;
                                row2["VechileFees"] = obj2.VechileFees;
                                row2["VechileFeesGST"] = obj2.VechileFeesGST;
                                row2["VechileFeesGSTAmount"] = obj2.VechileFeesGSTAmount;
                                row2["GuideFees"] = obj2.GuideFees;
                                row2["GuideFeesGST"] = obj2.GuideFeesGST;
                                row2["GuideFeesGSTAmount"] = obj2.GuideFeesGSTAmount;
                                dtTicketDtl.Rows.Add(row2);
                            }

                            StringWriter sw2 = new StringWriter();
                            dtTicketDtl.WriteXml(sw2);
                            string xmlTktList = sw2.ToString();
                            if (tkt.PlaceId == 19)
                            {
                                IsOdhi = Convert.ToInt16(FC["IsOdhiPlace"].ToString());

                                SqlParameter[] param = {new SqlParameter("@xmlTkt",@xmlTkt),
                                                new SqlParameter("@xmlTktList",xmlTktList),
                                                new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0))
                                                };
                                dl.Fill_WithoutCommit2(ds, "spNP_BookTicketNew", param);
                            }
                            else if (tkt.PlaceId == 74)
                            {
                                IsOdhi = Convert.ToInt16(FC["IsOdhiPlace"].ToString());

                                SqlParameter[] param = {new SqlParameter("@xmlTkt",xmlTkt),
                                                new SqlParameter("@xmlTktList",xmlTktList),
                                                new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0))
                                                };
                                dl.Fill_WithoutCommit2(ds, "spNP_BookTicketPalighat", param);
                            }
                            else if (tkt.PlaceId == 36 || tkt.PlaceId == 12 || tkt.PlaceId == 49 || tkt.PlaceId == 75 || tkt.PlaceId == 77 || tkt.PlaceId == 78)
                            {
                                IsOdhi = Convert.ToInt16(FC["IsOdhiPlace"].ToString());
                                //SafariType = Convert.ToInt16(FC["SafariType"].ToString());
                                SqlParameter[] param = {new SqlParameter("xmlTkt",@xmlTkt),
                                                new SqlParameter("@xmlTktList",xmlTktList),
                                                new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0)),
                                                new SqlParameter("@SafariType",0)
                                                };
                                dl.Fill_WithoutCommit2(ds, "spNP_BookTicketTalChhapar", param);
                            }
                            else
                            {
                                IsOdhi = Convert.ToInt16(FC["IsOdhiPlace"].ToString());
                                SqlParameter[] param = {new SqlParameter("@TicketData",Utility.ObjectToData(tkt)),
                                                        new SqlParameter("@TicketDetail",Utility.ToDataTable<TicketData>(tickets)),
                                                        new SqlParameter("@IsOdhiExist",(IsOdhi==1?1:0))
                                                        };

                                dl.Fill_WithoutCommit2(ds, "spNP_BookTicket", param);
                            }
                        }



                        if (ds != null && ds.Tables[0] != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Int64 ticketId = 0;
                                    ticketId = Convert.ToInt64(ds.Tables[0].Rows[0]["TicketId"]);
                                    if (ticketId > 0)
                                    {
                                        DataRow dr = ds.Tables[0].Rows[0];
                                        TicketPayment ticketPayment = new TicketPayment
                                        {
                                            BookingType = BookingType,
                                            TicketId = Convert.ToInt64(dr["TicketId"].ToString()),
                                            RequestId = dr["RequestId"].ToString(),
                                            VisitingDate = Convert.ToDateTime(dr["VisitingDate"].ToString()),
                                            EnteredBy = Convert.ToInt32(dr["EnteredBy"].ToString()),
                                            Name = dr["Name"].ToString(),
                                            TotalMember = Convert.ToInt32(dr["TotalMember"].ToString()),
                                            TotalMemberFees = Convert.ToDecimal(dr["TotalMemberFees"].ToString()),
                                            TotalCameraFees = Convert.ToDecimal(dr["TotalCameraFees"].ToString()),
                                            TotalVehicleFees = Convert.ToDecimal(dr["TotalVehicleFees"].ToString()),
                                            TotalGuideFees = Convert.ToDecimal(dr["TotalGuideFees"].ToString()),
                                            TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString()),
                                            TotalGSTAmount = Convert.ToDecimal(dr["TotalGSTAmount"].ToString()),
                                            OdhiCharge = Convert.ToDecimal(dr["OdhiCharge"].ToString()),
                                            IsOdhiExist = Convert.ToBoolean(dr["IsOdhiExist"]),
                                            FacilityCharge = Convert.ToDecimal(dr["FacilityCharge"]),
                                            MaintenanceCharge= (tkt.PlaceId == 36? Math.Round( Convert.ToDecimal(dr["MaintenanceCharge"]),2) : Convert.ToDecimal(dr["MaintenanceCharge"])),
                                            VehicleRent = Convert.ToDecimal(dr["VehicleRent"]),
                                            TotalAmountBePay = Convert.ToDecimal(dr["TotalAmountBePay"].ToString())
                                            
                                        };

                                        return PartialView("payData", ticketPayment);
                                    }
                                    else if (ticketId == -1)
                                    {
                                        error = "Sorry, tickets are not available for the entered visit date, please book for another date.";
                                    }
                                    else if (ticketId == -2)
                                    {
                                        error = "Sorry, booking quota exhausted for today, please try again!!!.";
                                    }
                                    else if (ticketId == -3)
                                    {
                                        error = "Sorry, you are not authenticated to do the bookings for this place.";
                                    }
                                    else if (ticketId == -4)
                                    {
                                        error = "Please select valid date of visit.";
                                    }
                                    else if (ticketId == -5 || ticketId == -6)
                                    {
                                        error = "Please select correct Shift.";
                                    }
                                    else if (ticketId == -7)
                                    {
                                        error = "Safari time is over for today.";
                                    }

                                }

                            }
                        }
                        else
                        {
                            error = "Please enter all the details of all visitors.";
                        }
                    //}
                    //else
                    //{
                    //    error = "You have selected invalid date.";
                    //}
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                error = "Invalid captcha, please enter valid captcha.";
            }
            return Json(new { error = error, status = false, respone = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvalidRequest()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }


        [HttpGet]
        public ActionResult Refund(string ticketid)
        {
            BookOnTicket cs = new BookOnTicket();
            DataSet DS = new DataSet();
            long TicketId = Convert.ToInt64(Encryption.decrypt(ticketid).ToString());
            DS = GetTicketRefundData(TicketId);
            cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));
            if (DS.Tables[0].Rows.Count > 0)
            {
                cs.TicketID = Convert.ToInt64(DS.Tables[0].Rows[0]["TicketID"]);
                cs.RequestId = Convert.ToString(DS.Tables[0].Rows[0]["RequestId"]);
                cs.DateOfArrival = Convert.ToString(DS.Tables[0].Rows[0]["DateOfArrival"]);
                cs.TotalMember = Convert.ToInt32(DS.Tables[0].Rows[0]["TotalMember"]);
                cs.TotalMemberFees = Convert.ToString(DS.Tables[0].Rows[0]["TotalMemberFees"]);
                cs.TotalCameraFees = Convert.ToString(DS.Tables[0].Rows[0]["TotalCameraFees"]);
                cs.VehicleFees_Total = Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleFees_Total"]);
                cs.GuideFee = Convert.ToString(DS.Tables[0].Rows[0]["GuideFee"]);
                cs.TotalGSTAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["TotalGSTAmount"]);
                cs.TotalAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["TicketAmount"]);
                cs.EmitraCharges = Convert.ToString(DS.Tables[0].Rows[0]["EMitraCharges"]);
                cs.GrandTotal = Convert.ToString(DS.Tables[0].Rows[0]["TotalAmount"]);
                //cs.SSOID = Convert.ToString(DS.Tables[0].Rows[0]["Ssoid"]);
                cs.RefundAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["RefundAmount"]);

            }
            return View(cs);


        }

        [HttpPost]
        public JsonResult Refund(BookOnTicket cs)
        {
            DataTable dtf = CheckTicketStatus(cs);
            var isValidRequest = new BookOnTicket();
            DataTable DT = new DataTable();
            string msg = string.Empty;
            isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.RequestId == cs.RequestId).FirstOrDefault();
            if (isValidRequest != null)
            {
                if (isValidRequest.COVIDStatus == 1 && isValidRequest.TicketMemberBordingStatus == 0 && isValidRequest.RefundStatus == 0)
                {
                    DT = SubmitToRefund(cs);
                    if (DT.Rows.Count > 0)
                    {
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        #region Email and SMS

                        //objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                        #endregion


                        //TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
                        msg = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course."; ;
                    }
                }
                else
                {
                    msg = "Invalid request";
                }
            }
            else
            {
                msg = "Invalid request";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);


        }

        private DataSet GetTicketRefundData(long TicketId)
        {
            SqlParameter[] param = { new SqlParameter("@Action", "GetRefundRequestData"), new SqlParameter("@TicketID", TicketId) };
            DataSet DS = new DataSet();
            dl.Fill(DS, "TB_BookTicket_NP_RefundProcess", param);
            return DS;
        }
        private DataTable CheckTicketStatus(BookOnTicket cs)
        {
            SqlParameter[] param = { new SqlParameter("@Action", "IsValidForRefund"), new SqlParameter("@RequestId", cs.RequestId), new SqlParameter("@UserId", Convert.ToInt64(HttpContext.Session["UserID"].ToString())) };
            DataTable DT = new DataTable();
            dl.Fill(DT, "TB_BookTicket_NP_RefundProcess", param);
            return DT;
        }
        private DataTable SubmitToRefund(BookOnTicket cs)
        {
            SqlParameter[] parameters = {new SqlParameter("@Action", "SubmitToRefund"),
            new SqlParameter("@RequestId", cs.RequestId),
             new SqlParameter("@AccountNo", cs.AccountNo),
              new SqlParameter("@BankName", cs.BankName),
               new SqlParameter("@BranchName", cs.BranchName),
                new SqlParameter("@IFSCCode", cs.IFSCCode),
                 new SqlParameter("@AccountType", cs.AccountType),
                  new SqlParameter("@AccountHolderName", cs.AccountHolderName) };
            DataTable DT = new DataTable();
            dl.Fill(DT, "TB_BookTicket_NP_RefundProcess", parameters);
            return DT;
        }


        [HttpPost]
        public JsonResult getData(string option, int? placeid, int? zoneid, int? shiftid, string route, string seldate)
        {
            DataSet ds = GetData(option, placeid, zoneid, shiftid, route, seldate);
            if (ds != null && ds.Tables[0] != null)
            {
                if (option == "place")
                {
                    var lst = ds.Tables[0].AsEnumerable().Select(x => new
                    {
                        Id = (dynamic)(x["Id"]),
                        Name = (string)(x["Name"]),
                        Zone = (int)(x["Zone"]),
                        Shift = (int)(x["Shift"]),
                        Vehicle = (int)(x["Vehicle"]),
                        MaxSeats = (int)(x["MaxSeats"])
                    });
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lst = ds.Tables[0].AsEnumerable().Select(x => new
                    {
                        Id = (dynamic)(x["Id"]),
                        Name = (string)(x["Name"])
                    });
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getDate()
        {
            string exDate = "";
            List<string> excludeDateList = new List<string>();
            string currentYear = DateTime.Now.Year.ToString();
            int cYear = Convert.ToInt32(currentYear);
            for (int y = cYear; y <= cYear + 1; y++)
            {
                for (int i = 7; i < 10; i++)
                {
                    for (int j = 1; j <= (i == 9 ? 30 : 31); j++)
                    {
                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + "0" + i + "/" + y.ToString();
                        excludeDateList.Add(exDate);
                    }
                }
            }
             ;

            string myDate = dl.ExecuteScalar("NPspGetDate").ToString();
            var data = new { cDate = myDate, ExcludeDateList = excludeDateList };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getFeeByVisitor(int placeid, int? vehicleId, int visitorTypeId)
        {
            SqlParameter[] param = {new SqlParameter("@PlaceID",placeid),
                                        new SqlParameter("@VehicleId",vehicleId),
                                        new SqlParameter("@VisitorTypeId",visitorTypeId)
                                   };
            //dl.Fill(ds, "spNP_GetBookingFeesDetails", param);
            dl.Fill_WithoutCommit2(ds, "spNP_GetBookingFeesDetails", param);

            if (ds != null && ds.Tables[0] != null)
            {
                var lst = ds.Tables[0].AsEnumerable().Select(x => new
                {
                    MemberFee = (dynamic)(x["MemberFee"]),
                    StillCameraFee = (dynamic)(x["StillCameraFee"]),
                    VideoCamereFees = (dynamic)(x["VideoCamereFees"]),
                    VehicleFees = (dynamic)(x["VehicleFees"]),
                    GuideFee = (dynamic)(x["GuideFee"])
                });

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getIdType()
        {
            return Json(dl.ExecuteScalar("NPspGetIdType").ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Pay(TicketPayment model)
        {
            if (!string.IsNullOrEmpty(model.RequestId))
            {
                if (BookingType == BookingType.OnlineCitizenBooking)
                {
                    RedirectCitizenToEmitra(model);
                    return null;
                }
                else if (BookingType == BookingType.DepartmentBooking)
                {
                    DepartmentKioskPayment updateBooking = RedirectDepartmentUser(model);
                    if (updateBooking != null)
                    {
                        return View("DepartmentUserPayment", updateBooking);
                    }
                }
                else if (BookingType == BookingType.EmitraKioskBooking)
                {
                    TicketPayment ticketPayment = ProcessEmitraKioskPayment(model);
                    if (ticketPayment != null)
                    {
                        return View("TicketConfirmation", ticketPayment);
                    }
                }
            }
            return RedirectToAction("index");
        }

        private TicketPayment ProcessEmitraKioskPayment(TicketPayment model)
        {
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            if (Session["EmitrServiceId"] != null)
            {
                DataSet DS = GetHeadWiseDetail(model.RequestId, Convert.ToInt32(Session["EmitrServiceId"]));
                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0 && Convert.ToInt64(DS.Tables[0].Rows[0]["TicketId"]) > 0 && DS.Tables[2].Rows.Count > 0)
                    {
                        decimal TotalAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["TotalAmountBePay"]);
                        EmitraKisokPayment emitraKisokPayment = new EmitraKisokPayment();


                        string REVENUEHEAD = string.Empty;
                        foreach (DataRow dr in DS.Tables[1].Rows)
                        {
                            REVENUEHEAD = REVENUEHEAD + dr["HeadAmount"] + "|";
                        }
                        REVENUEHEAD = REVENUEHEAD.Trim('|');

                        EmitraKioskRequest request = new EmitraKioskRequest
                        {
                            BASEURL = Convert.ToString(DS.Tables[2].Rows[0]["BaseUrl"]),
                            VERIFICAION_URL = Convert.ToString(DS.Tables[2].Rows[0]["VerificationUrl"]),
                            SERVICERESPONSETIME = Convert.ToInt16(DS.Tables[2].Rows[0]["MAX_RESPONSE_TIME_SEC"]),
                            MERCHANTCODE = Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                            REQUESTID = model.RequestId,
                            REQTIMESTAMP = DateTime.Now.Ticks.ToString(),
                            SERVICEID = Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                            SUBSERVICEID = "",
                            REVENUEHEAD = REVENUEHEAD,
                            CONSUMERKEY = model.RequestId,
                            CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper(),
                            SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper(),
                            OFFICECODE = Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]),
                            //COMMTYPE = "2",
                            COMMTYPE = "3", ////Change by Amit on 02/09/2020 for Ematra changes 
                            SSOTOKEN = Convert.ToString(Session["SSOTOKEN"])
                        };
                        if (liveUat == 0)
                        {
                            request.BASEURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";
                            request.VERIFICAION_URL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestIdWithEncryption";
                            request.OFFICECODE = "FORESTHQ";
                        }
                        if (!string.IsNullOrEmpty(request.OFFICECODE))
                        {
                            EmitraKioskResponse emitraKisokResponse = emitraKisokPayment.ProcessPayment(request);

                            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

                            UpdateTicketBooking updateBooking = new UpdateTicketBooking
                            {
                                UserId = UserID,
                                EmitraResponse = emitraKisokResponse.RESPONSE,
                                EmitraTransactionId = emitraKisokResponse.TRANSACTIONID,
                                PaymentMode = PaymentMode.Online,
                                RequestId = emitraKisokResponse.REQUESTID,
                                EmitraAmount = Convert.ToDecimal(emitraKisokResponse.TRANSAMT) > 0 ? (Convert.ToDecimal(emitraKisokResponse.TRANSAMT) - Convert.ToDecimal(TotalAmount)) : 0
                            };

                            if (emitraKisokResponse.TRANSACTIONSTATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                            {
                                updateBooking.TransactionStatus = TransactionStatus.Paid;
                            }
                            else //PAYMENT FAILED
                            {
                                updateBooking.TransactionStatus = TransactionStatus.Failed;
                            }

                            DataTable bookingData = UpdateTicketBooking(updateBooking);
                            Int64 TicketID = 0;
                            if (bookingData.Rows.Count > 0)
                            {
                                TicketID = Convert.ToInt64(bookingData.Rows[0]["TicketId"]);
                                TicketPayment ticketPayment = GetBookingConfirmationSummary(TicketID);
                                if (ticketPayment != null)
                                {
                                    return ticketPayment;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void RedirectCitizenToEmitra(TicketPayment model)
        {
            DataSet DS = GetHeadWiseDetail(model.RequestId, 0); //Service ID to be decided in stored procedure
            if (DS != null && DS.Tables.Count > 0)
            {
                if (DS.Tables[0].Rows.Count > 0 && Convert.ToInt64(DS.Tables[0].Rows[0]["TicketId"]) > 0)
                {
                    string REVENUEHEAD = string.Empty;
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        REVENUEHEAD = REVENUEHEAD + dr["HeadAmount"] + "|";
                    }
                    REVENUEHEAD = REVENUEHEAD.Trim('|');
                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                    string forms = ObjEmitraPayRequest.PayRequest(false, model.RequestId,
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        ReturnUrl + "NationalPark/Payment", ReturnUrl + "NationalPark/Payment",
                        Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["TotalAmountBePay"]), REVENUEHEAD, Session["User"].ToString());

                    Response.Write(forms);
                }
            }
        }
        private DepartmentKioskPayment RedirectDepartmentUser(TicketPayment model)
        {
            DataTable DT = GetTicketSummaryByRequestId(model.RequestId);
            if (DT.Rows.Count > 0)
            {
                DepartmentKioskPayment updateTicket = new DepartmentKioskPayment
                {
                    RequestId = Convert.ToString(DT.Rows[0]["RequestId"]),
                    TotalAmount = Convert.ToDecimal(DT.Rows[0]["TotalAmount"]),
                    VehicleName = Convert.ToString(DT.Rows[0]["Vehicle"])
                };
                return updateTicket;
            }
            return null;
        }

        [HttpPost]
        public ActionResult DepartmentKioskPayment(DepartmentKioskPayment model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                UpdateTicketBooking updateBooking = new UpdateTicketBooking
                {
                    UserId = UserID,
                    EmitraTransactionId = "",
                    PaymentMode = model.PaymentMode,
                    RequestId = model.RequestId,
                    EmitraAmount = 0,
                    BankName = model.BankName,
                    VehicleNumber = model.VehicleNumber,
                    GuideName = model.GuideName,
                    ChequeNo = model.ChequeNo,
                    IFSCCode = model.IFSCCode,
                    TransactionStatus = TransactionStatus.Paid
                };

                if (model.PaymentMode != PaymentMode.Cash)
                    updateBooking.ChequeIssueDate = Convert.ToDateTime(model.ChequeIssueDate);

                DataTable bookingData = UpdateTicketBooking(updateBooking);
                Int64 TicketID = 0;
                if (bookingData.Rows.Count > 0)
                {
                    TicketID = Convert.ToInt64(bookingData.Rows[0]["TicketId"]);
                }
                TicketPayment ticketPayment = GetBookingConfirmationSummary(TicketID);
                if (ticketPayment != null)
                {
                    return View("TicketConfirmation", ticketPayment);
                }
                else
                {
                    //Something went wrong.
                    return RedirectToAction("index");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("index");
        }

        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";
                if (Request.Form["MERCHANTCODE"] != null)
                    MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                if (Request.Form["PRN"] != null)
                    PRN = Request.Form["PRN"].ToString();
                if (Request.Form["STATUS"] != null)
                    STATUS = Request.Form["STATUS"].ToString();
                if (Request.Form["ENCDATA"] != null)
                    ENCDATA = Request.Form["ENCDATA"].ToString();




                //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
                EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

                string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                UpdateTicketBooking updateBooking = new UpdateTicketBooking
                {
                    UserId = UserID,
                    EmitraResponse = DecryptedData,
                    EmitraTransactionId = ObjPGResponse.TRANSACTIONID,
                    PaymentMode = PaymentMode.Online,
                    RequestId = PRN,
                    EmitraAmount = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) > 0 ? (Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT)) : 0
                };

                //****************************** for test only

                //ObjPGResponse.STATUS = "SUCCESS";
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                //****************************** for test only;

                if (ObjPGResponse.STATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                {
                    updateBooking.TransactionStatus = TransactionStatus.Paid;
                }
                else //PAYMENT FAILED
                {
                    updateBooking.TransactionStatus = TransactionStatus.Failed;
                }

                DataTable bookingData = UpdateTicketBooking(updateBooking);
                Int64 TicketID = 0;
                if (bookingData.Rows.Count > 0)
                {
                    TicketID = Convert.ToInt64(bookingData.Rows[0]["TicketId"]);
                    BookingStatus bookingStatus = (BookingStatus)Convert.ToByte(bookingData.Rows[0]["BookingStatus"]);
                    if (bookingStatus == BookingStatus.Booked)
                    {
                        //Send Email OR SMS
                        //SendSMSEmailForSuccessTransaction(bookingData);
                    }
                }

                TicketPayment ticketPayment = GetBookingConfirmationSummary(TicketID);
                if (ticketPayment != null)
                {
                    return View("TicketConfirmation", ticketPayment);
                }
                else
                {
                    //Something went wrong.
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        private TicketPayment GetBookingConfirmationSummary(Int64 TicketID)
        {
            DataTable dtTicktSummary = GetTicketSummary(TicketID);
            if (dtTicktSummary != null && dtTicktSummary.Rows.Count > 0)
            {
                DataRow dr = dtTicktSummary.Rows[0];
                TicketPayment ticketPayment = new TicketPayment
                {
                    TicketId = Convert.ToInt64(dr["TicketId"]),
                    RequestId = dr["RequestId"].ToString(),
                    VisitingDate = Convert.ToDateTime(dr["VisitingDate"]),
                    BookingDate = Convert.ToDateTime(dr["BookingDateTime"]),
                    EnteredBy = Convert.ToInt32(dr["EnteredBy"]),
                    Name = dr["Name"].ToString(),
                    TotalMember = Convert.ToInt32(dr["TotalMember"]),
                    TotalMemberFees = Convert.ToDecimal(dr["TotalMemberFees"]),
                    TotalCameraFees = Convert.ToDecimal(dr["TotalCameraFees"]),
                    TotalVehicleFees = Convert.ToDecimal(dr["TotalVehicleFees"]),
                    TotalGuideFees = Convert.ToDecimal(dr["TotalGuideFees"]),
                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                    TotalGSTAmount = Convert.ToDecimal(dr["TotalGSTAmount"]),
                    TotalAmountBePay = Convert.ToDecimal(dr["TotalAmountBePay"]),
                    TransactionStatus = (TransactionStatus)Convert.ToByte(dr["TranactionStatus"]),
                    bookingStatus = (BookingStatus)Convert.ToByte(dr["BookingStatus"]),
                    TotalPaidAmount = Convert.ToDecimal(dr["TotalPaidAmount"]),
                    EmitraTransactionID = Convert.ToString(dr["EmitraTransactionId"]),
                    EmitraAmount = Convert.ToDecimal(dr["EmitraAmount"]),
                    PlaceName = Convert.ToString(dr["PlaceName"]),
                    Vehicle = Convert.ToString(dr["Vehicle"]),
                    BookingType = (BookingType)Convert.ToByte(dr["BookingType"]),
                    PaymentMode = (PaymentMode)Convert.ToByte(dr["PaymentMode"]),
                    BankName = Convert.ToString(dr["BankName"]),
                    IFSCCode = Convert.ToString(dr["IFSCCode"]),
                    ChequeNo = Convert.ToString(dr["ChequeNo"])

                };
                if (!string.IsNullOrEmpty(Convert.ToString(dr["ChequeIssueDate"])))
                {
                    ticketPayment.ChequeIssueDate = Convert.ToDateTime(dr["ChequeIssueDate"]);
                }
                return ticketPayment;
            }
            return null;

        }

        #region PrintTicket
        [HttpGet]
        public FileResult PrintTicket(Int64 ticketId)
        {
            int PlaceId = 0;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                string filepath = string.Empty;

                DataSet DS = new DataSet();
                if (IsNp_HeadWiseDetailExist(ticketId,out PlaceId) > 0)
                {
                    DS = GetPrintTicketData(ticketId, UserID,true, PlaceId);
                    filepath = TicketPDFGenerate.NP_GenerateTicket_New(DS);
                }
                else
                {
                    DS = GetPrintTicketData(ticketId, UserID, false, PlaceId);
                    filepath = TicketPDFGenerate.NP_GenerateTicket(DS);
                }

                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        [HttpGet]
        public FileResult PrintBoardingPaas(string RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataSet DS = GetBoardingPass(RequestId);
                string filepath = TicketPDFGenerate.NP_GenerateBoardingPaas(DS);

                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        private int IsNp_HeadWiseDetailExist(Int64 TicketId, out int PlaceId)
        {

            PlaceId = 0;
            SqlParameter[] param = { new SqlParameter("@TicketId", TicketId) };
            DataTable DS = new DataTable();
            dl.Fill(DS, "spGet_NP_HeadwiseTransactionExist", param);
            if (DS != null)
            {
                if (DS.Rows.Count > 0)
                {
                    PlaceId = Convert.ToInt32(DS.Rows[0]["PlaceId"].ToString());
                    return DS.Rows.Count;
                }
                else
                    return 0;
            }
            else
                return 0;
            //SqlParameter[] param = { new SqlParameter("@TicketId", TicketId) };
            //DataTable DS = new DataTable();
            //dl.Fill(DS, "spGet_NP_HeadwiseTransactionExist", param);
            //if (DS != null)
            //{
            //    if (DS.Rows.Count > 0)
            //        return DS.Rows.Count;
            //    else
            //        return 0;
            //}
            //else
            //    return 0;
        }
        #endregion

        #region Send email and SMS
        public void SendSMSEmailForSuccessTransaction(DataTable DT)
        {
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();
            string body = string.Empty;
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                    objSMS_EMail_Services.sendEMail("Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                    body = string.Empty;

                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                    body = string.Empty;
                }

            }
        }
        #endregion

        #region Get Data from Database
        private DataSet GetData(string option, int? placeid, int? zoneid, int? shiftid, string route, string seldate)
        {
            long UserId = Convert.ToInt64(Session["UserID"].ToString());
            bool IsKisoskUser = false;
            bool IsDeptUser = false;
            bool IsCitizenUser = false;
            if (Session["IsDepartmentalKioskUser"] != null && Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                IsDeptUser = true;
            }
            else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]))
            {
                IsKisoskUser = true;
            }
            else
            {
                IsCitizenUser = true;
            }

            SqlParameter[] param = {new SqlParameter("@Option",option),
                                        new SqlParameter("@PlaceId",placeid),
                                        new SqlParameter("@ZoneId",zoneid),
                                        new SqlParameter("@ShiftId",shiftid),
                                        new SqlParameter("@route",route),
                                        new SqlParameter("@seldate",seldate),
                                        new SqlParameter("@UserId",UserId),
                                        new SqlParameter("@IsDeptUser",(IsDeptUser==true?1:0)),
                                        new SqlParameter("@IsKisoskUser",(IsKisoskUser==true?1:0)),
                                        new SqlParameter("@IsCitizenUser",(IsCitizenUser==true?1:0)),
                                   };

            dl.Fill(ds, "NPspBookingData", param);
            return ds;
        }
        private DataSet GetHeadWiseDetail(string RequestId, int serviceId)
        {
            SqlParameter[] param = {new SqlParameter("@RequestId",RequestId),
                                            new SqlParameter("@UserID",Convert.ToInt32(Convert.ToString(Session["UserId"]))),
                                            new SqlParameter("@ServiceId",serviceId)
                                            };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_GetHeadWiseAmountForEmitra", param);
            return DS;
        }
        private DataSet GetEmitraHeadForChoiceVehicleOrBoat(string RequestId, int serviceId, int ChoiceGV, int liveUat)
        {
            SqlParameter[] param = {new SqlParameter("@RequestId",RequestId),
                                            new SqlParameter("@UserID",Convert.ToInt32(Convert.ToString(Session["UserId"]))),
                                            new SqlParameter("@ServiceId",serviceId),
                                            new SqlParameter("@ChoiceGV",ChoiceGV),
                                            new SqlParameter("@IsLive ",liveUat)
                                            };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_GetEmitraHeadForChoiceVehicleOrBoat", param);
            return DS;
        }
        private DataTable UpdateTicketBooking(UpdateTicketBooking updateBooking)
        {
            BookingStatus bookingStatus = BookingStatus.Pending;
            SqlParameter[] param = {
                                       new SqlParameter("@RequestId",updateBooking.RequestId),
                                       new SqlParameter("@UserID",updateBooking.UserId),
                                       new SqlParameter("@PaymentMode",(byte)updateBooking.PaymentMode),
                                       new SqlParameter("@EmitraAmount",updateBooking.EmitraAmount),
                                       new SqlParameter("@TransactionStatus",(byte)updateBooking.TransactionStatus),
                                       new SqlParameter("@EmitraTransactionId",updateBooking.EmitraTransactionId),
                                       new SqlParameter("@BankName",updateBooking.BankName),
                                       new SqlParameter("@IFSCCode",updateBooking.IFSCCode),
                                       new SqlParameter("@ChequeNo",updateBooking.ChequeNo),
                                       new SqlParameter("@ChequeIssueDate",updateBooking.ChequeIssueDate),
                                       new SqlParameter("@EmitraResponse",updateBooking.EmitraResponse),
                                       new SqlParameter("@VehicleNumber",updateBooking.GuideName),
                                       new SqlParameter("@GuideName",updateBooking.VehicleNumber)
                                    };
            DataTable DT = new DataTable();
            dl.Fill(DT, "spNP_UpdateTicket", param);
            return DT;
        }
        private DataTable GetTicketSummary(Int64 TicketId)
        {
            SqlParameter[] param = {new SqlParameter("@TicketId",TicketId)
                                            };
            DataTable DT = new DataTable();
            dl.Fill(DT, "spNP_GetBookingSummary", param);
            return DT;
        }
        private DataTable GetTicketSummaryByRequestId(string RequestId)
        {
            SqlParameter[] param = {new SqlParameter("@RequestID",RequestId)
                                            };
            DataTable DT = new DataTable();
            dl.Fill(DT, "spNP_GetBookingSummaryByRequestId", param);
            return DT;
        }
        private DataSet GetPrintTicketData(Int64 TicketId, Int64 UserId, bool isNewHeadWise, int PlaceId)
        {

            SqlParameter[] param = { new SqlParameter("@TicketId", TicketId), new SqlParameter("@UserId", UserId) };
            DataSet DS = new DataSet();
            if (PlaceId == 19)
                dl.Fill(DS, "spNP_GetPrintTicketData_KumbhalGarhHeadwise", param);
            else if (PlaceId == 12 || PlaceId == 36 || PlaceId == 49 || PlaceId == 74 || PlaceId == 49 || PlaceId == 75 || PlaceId == 77 || PlaceId == 78)
                dl.Fill(DS, "spNP_GetPrintTicketData_Palighatwise", param);
            else
                dl.Fill(DS, (isNewHeadWise == true ? "spNP_GetPrintTicketData_NewHeadwise" : "spNP_GetPrintTicketData"), param);
            return DS;
        }
        //private DataSet GetPrintTicketData(Int64 TicketId, Int64 UserId, bool isNewHeadWise, int PlaceId)
        //{
        //    SqlParameter[] param = { new SqlParameter("@TicketId", TicketId), new SqlParameter("@UserId", UserId) };
        //    DataSet DS = new DataSet();
        //    if (PlaceId == 19)
        //        dl.Fill(DS, "spNP_GetPrintTicketData_KumbhalGarhHeadwise", param);
        //    else
        //        dl.Fill(DS, (isNewHeadWise == true ? "spNP_GetPrintTicketData_NewHeadwise" : "spNP_GetPrintTicketData"), param);
        //    return DS;

        //}
        private DataSet GetUserBookings(Int64 UserId, int TopCount, string route, BookingType bookingType)
        {
            SqlParameter[] param = { new SqlParameter("@UserId", UserId), new SqlParameter("@TopCount", TopCount), new SqlParameter("@route", route), new SqlParameter("@BookingType", (byte)bookingType) };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_GetUserBookings", param);
            return DS;
        }
        private DataTable GetUserPlaces(Int64 UserId)
        {
            SqlParameter[] param = { new SqlParameter("@UserId", UserId) };
            DataTable DT = new DataTable();
            dl.Fill(DT, "spNP_GetKioskUserPlace", param);
            return DT;
        }
        private DataSet GetBoardingPass(string RequestId)
        {
            SqlParameter[] param = { new SqlParameter("@RequestId", RequestId) };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_GetBoardingPassData", param);
            return DS;
        }
        private DataSet GetMemberRows(int? placeid, int? zoneid, int? shiftid, int? vehicleid, string SelDate, BookingType bookingType)
        {
            DateTime dt = Convert.ToDateTime(SelDate);
            SqlParameter[] param = {   
                                        new SqlParameter("@PlaceId",placeid),
                                        new SqlParameter("@ZoneId",zoneid),
                                        new SqlParameter("@ShiftId",shiftid),
                                        new SqlParameter("@vehicleid",vehicleid),
                                        new SqlParameter("@VisitingDate",dt),
                                        new SqlParameter("@BookingType",bookingType)
                                   };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_GetMemberRows", param);
            return DS;
        }
        private DataSet GetNormalOdhiAvailibility(int? placeid, int? zoneid, int? shiftid, int? vehicleid, string SelDate, BookingType bookingType)
        {
            DateTime dt = Convert.ToDateTime(SelDate);
            SqlParameter[] param = {
                                        new SqlParameter("@ZoneId",zoneid),
                                        new SqlParameter("@ShiftId",shiftid),
                                        new SqlParameter("@VehicleId",vehicleid),
                                        new SqlParameter("@VisitDate",dt),
                                   };
            DataSet DS = new DataSet();
            dl.Fill(DS, "sp_OdhiForNormalShiftwisePlaceAvailibility", param);
            return DS;
        }
        #endregion

        #region Boarding Pass Operaton
        public ActionResult BoardingPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBoardingPassDetails(int? placeid, int? zoneid, int? shiftid, int? vehicleid, int? visitors, string selDate)
        {
            DataSet ds = GetBoardingPassDetails(placeid, zoneid, shiftid, vehicleid, selDate);
            if (ds.Tables.Count > 0)
            {
                var boardingPassDetails = ds.Tables[0].AsEnumerable().Select(a => new BoardingPassDetails
                {
                    TicketId = a.Field<Int64>("Id"),
                    RequestId = a.Field<string>("RequestId"),
                    VisitDate = a.Field<DateTime>("VisitingDate"),
                    BookingDate = a.Field<DateTime>("BookingDateTime"),
                    TotalMember = a.Field<int>("TotalMember"),
                    TotalPaidAmount = a.Field<decimal>("TotalPaidAmount"),
                    PlaceName = a.Field<string>("PlaceName"),
                    GuideName = a.Field<string>("GuideName"),
                    VehicleNumber = a.Field<string>("VehicleNumber"),
                    BoardingPassStatus = a.Field<bool>("BoardingPassStatus"),
                    BookingStatus = (BookingStatus)a.Field<byte>("BookingStatus")
                }).ToList();
                return PartialView("_BoardingPass", boardingPassDetails);
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveAndDownloadTicket(Int64 ticketId, Boolean boardingPassStatus, string guideName, string vehicleNumber, Int32 controlIndex)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataSet DS = SaveAndDownloadTicketData(ticketId, guideName, vehicleNumber, UserID);
                string filepath = TicketPDFGenerate.NP_GenerateBoardingPaas(DS);
                if (System.IO.File.Exists(filepath))
                {
                    filepath = GetVirtualPath(filepath);
                }

                return Json(new { isError = false, pageURL = filepath, controlIndex = controlIndex }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return Json(new { isError = true, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isError = true, msg = "" }, JsonRequestBehavior.AllowGet);
        }
        private DataSet SaveAndDownloadTicketData(Int64 TicketId, string guideName, string vehicleNumber, Int64 UserId)
        {
            SqlParameter[] param = { 
                                       new SqlParameter("@ActionCode", 1), 
                                       new SqlParameter("@TicketId", TicketId), 
                                       new SqlParameter("@guideName", guideName), 
                                       new SqlParameter("@vehicleNumber", vehicleNumber), 
                                       new SqlParameter("@UserId", UserId) };
            DataSet DS = new DataSet();
            dl.Fill(DS, "spNP_UpdateBooking", param);
            return DS;
        }
        private string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");

            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");

            return physicalPath;
        }

        private DataSet GetBoardingPassDetails(int? placeid, int? zoneid, int? shiftid, int? vehicleid, string SelDate = null)
        {
            DateTime dt = Convert.ToDateTime(SelDate);
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode",1), 
                                        new SqlParameter("@PlaceId",placeid),
                                        new SqlParameter("@ZoneId",zoneid),
                                        new SqlParameter("@ShiftId",shiftid),
                                        new SqlParameter("@vehicleid",vehicleid),
                                        new SqlParameter("@SelDate",dt)
                                   };
            DataSet ds = new DataSet();
            dl.Fill(ds, "spNP_GetBookingDetails", param);
            return ds;
        }
        #endregion
        #region Choice Guide/Vehicle/Boat       
        public ActionResult ChoiceGuideVehicle()
        {
            //ViewBag.PayStatus = -1;
            //ViewBag.PayMsg = "";

            //if (TempData["PayMsg"] != null)
            //{
            //    if (TempData["PayMsg"].ToString() != "")
            //    {
            //        ViewBag.PayStatus = Convert.ToInt32(TempData["PayStatus"]);
            //        ViewBag.PayMsg = TempData["PayMsg"].ToString();
            //    }
            //}
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            INpChoice npChoice = new NPChoiceService();
            NP_ChoiceView np_Choice = new NP_ChoiceView();
            np_Choice.NpChoiceList = new List<NPChoice>();
            np_Choice.NpChoiceList = npChoice.GetNpChoiceTransactionList(UserID);
            //Session["eMitraResponse"] = null;
            //Session["nPChoice"] = null;
            return View(np_Choice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChoiceGuideVehicle(NPChoice nPChoice)
        {
            DataSet ds = null;
            Np_PayStatus eMitraPGResponse = null;
            Np_PayStatus eMitraKioskResponse = null;
            bool isValidResponse = false;
            TempData["PayStatus"] = -1;
            TempData["PayMsg"] = "";

            //Np_PayStatus np_PayStatus = new Np_PayStatus();
            if (ModelState.IsValid == true && (nPChoice.ChoiceType == 1 && nPChoice.GuideId > 0) || (nPChoice.ChoiceType == 2 && nPChoice.VehicleOrBoatId > 0) || (nPChoice.ChoiceType == 3 && nPChoice.GuideId > 0 && nPChoice.VehicleOrBoatId > 0))
            {
                if (Session["nPChoice"] == null)
                {
                    Session["nPChoice"] = nPChoice;
                }
                if (Session["eMitraResponse"] != null)
                {
                    eMitraPGResponse = Session["eMitraPGResponse"] as Np_PayStatus;
                    nPChoice = Session["nPChoice"] as NPChoice;
                }
                if (!string.IsNullOrEmpty(nPChoice.RequestId) && (eMitraKioskResponse == null || eMitraPGResponse == null))
                {
                    if (BookingType == BookingType.OnlineCitizenBooking)
                    {
                        RedirectPGEmitra(nPChoice);

                        return new EmptyResult();
                    }
                    else if (BookingType == BookingType.DepartmentBooking)
                    {

                    }
                    else if (BookingType == BookingType.EmitraKioskBooking)
                    {
                        eMitraKioskResponse = EmitraKioskPayment(nPChoice);
                    }
                }


                if (eMitraKioskResponse.isValidResponse == true)
                {
                    INpChoice npChoice = new NPChoiceService();
                    ds = npChoice.UpdateChoiceDetails(nPChoice, eMitraKioskResponse.ChoiceRequestId, eMitraKioskResponse.EmitraResponse, eMitraKioskResponse.ResponseStaus, eMitraKioskResponse.isValidResponse);
                }
                else if (eMitraPGResponse.isValidResponse == true)
                {
                    INpChoice npChoice = new NPChoiceService();
                    ds = npChoice.UpdateChoiceDetails(nPChoice, eMitraPGResponse.ChoiceRequestId, eMitraPGResponse.EmitraResponse, eMitraPGResponse.ResponseStaus, eMitraKioskResponse.isValidResponse);
                }

                if (eMitraKioskResponse != null)
                {

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewBag.PayStatus = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]);
                            ViewBag.PayMsg = ds.Tables[0].Rows[0]["msg"].ToString();
                            //TempData["PayStatus"] = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]); ;
                            //TempData["PayMsg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                        }
                    }
                    else
                    {
                        ViewBag.PayStatus = 0;
                        ViewBag.PayMsg = eMitraKioskResponse;
                        //TempData["PayStatus"] = 0;
                        //TempData["PayMsg"] = eMitraKioskResponse;
                    }
                }
                else if (eMitraPGResponse != null)
                {
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ViewBag.PayStatus = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]);
                            ViewBag.PayMsg = ds.Tables[0].Rows[0]["msg"].ToString();
                            //TempData["PayStatus"] = Convert.ToInt16(ds.Tables[0].Rows[0]["MsgStatus"]); ;
                            //TempData["PayMsg"] = ds.Tables[0].Rows[0]["msg"].ToString();
                        }
                    }
                    else
                    {
                        ViewBag.PayStatus = 0;
                        ViewBag.PayMsg = eMitraKioskResponse;
                        //TempData["PayStatus"] = 0;
                        //TempData["PayMsg"] = eMitraKioskResponse;
                    }
                }
                //return RedirectToAction("ChoiceGuideVehicle", "NationalPark");
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());


                INpChoice npChoice2 = new NPChoiceService();
                NP_ChoiceView np_Choice = new NP_ChoiceView();
                np_Choice.NpChoiceList = new List<NPChoice>();
                np_Choice.NpChoiceList = npChoice2.GetNpChoiceTransactionList(UserID);
                //return RedirectToActionPermanent("ChoiceGuideVehicle", "NationalPark");
                return View(np_Choice);

                //return PartialView("_GVChoice", np_Choice);
                //return new EmptyResult();
            }
            else
            {
                return View(nPChoice);
            }
        }

        [HttpGet]
        public FileResult GetChoiceReceipt(string strid)
        {

            string strId = FMDSS.Models.MySecurity.SecurityCode.DecodeUrl(strid);
            string[] spl = null;
            spl = strId.Split('|');
            string requestId = spl[0];
            string choiceRequestId = spl[1];

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                INpChoice npChoice = new NPChoiceService();
                NP_ChoiceView np_Choice = new NP_ChoiceView();
                np_Choice.NpChoiceList = new List<NPChoice>();
                np_Choice.NpChoiceList = npChoice.GetNpChoiceForReceiptList(UserID, requestId, choiceRequestId);

                string filepath = TicketPDFGenerate.NP_GenerateGuideOrBoatReceipt(np_Choice.NpChoiceList);
                if (System.IO.File.Exists(filepath))
                {
                    // string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(filepath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetRequestDetails(string RequestId, int ChoiceType)
        {
            NP_ChoiceView nP = new NP_ChoiceView();
            INpChoice npChoice = new NPChoiceService();
            DataSet ds = npChoice.GetChoiceDetails(RequestId);
            if (ds.Tables.Count > 0)
            {
                DataColumnCollection columns = ds.Tables[0].Columns;
                if (columns.Contains("validStatus"))
                {
                    var res1 = new { status = Convert.ToInt16(ds.Tables[0].Rows[0]["validStatus"]), respone = ds.Tables[0].Rows[0]["validMsg"].ToString() };
                    return Json(res1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    nP.NpChoiceList = new List<NPChoice>();

                    nP.NpChoiceList = ds.Tables[0].AsEnumerable().Select(a => new NPChoice
                    {
                        TicketId = a.Field<Int64>("Id"),
                        RequestId = a.Field<string>("RequestId"),
                        VisitDate = a.Field<DateTime>("VisitingDate").ToShortDateString(),
                        BookingDate = a.Field<DateTime>("BookingDateTime").ToShortDateString(),
                        TotalMember = a.Field<int>("TotalMember"),
                        PlaceId = a.Field<long>("PlaceId"),
                        PlaceName = a.Field<string>("PlaceName"),
                        ZoneId = a.Field<long>("ZoneId"),
                        ZoneName = a.Field<string>("ZoneName"),
                        ShiftId = a.Field<int>("ShiftId"),
                        ShiftName = a.Field<string>("ShiftName"),
                        VehicleId = a.Field<int>("VehicleId"),
                        VehicleName = a.Field<string>("VehicleName"),
                        GuideName = (String.IsNullOrEmpty(a.Field<string>("GuideName")) ? "" : a.Field<string>("GuideName")),
                        GuideChoiceAmt = a.Field<decimal>("GuideChoiceAmt"),
                        VehicleNumber = (String.IsNullOrEmpty(a.Field<string>("VehicleNumber")) ? "" : a.Field<string>("VehicleNumber")),
                        VehileChoiceAmt = a.Field<decimal>("VehileChoiceAmt"),
                        IsVehicleChoice = a.Field<bool>("IsChoiceVehicle"),
                        IsGuideChoice = a.Field<bool>("IsChoiceGuide"),
                        GuideChoiceGSTAmt = a.Field<decimal>("GuideChoiceGSTAmt"),
                        VehileChoiceGSTAmt = a.Field<decimal>("VehileChoiceGSTAmt"),
                        status = 1,
                        respone = "Valid",
                        ChoiceType = ChoiceType
                    }).ToList();

                    nP.NpGuideSelectList = new List<SelectListItem>();
                    nP.NpBoatSelectList = new List<NpBoatProp>();

                    var obChoice = nP.NpChoiceList.ToList().FirstOrDefault();
                    nP.NpGuideSelectList = npChoice.GetNpGuideList(obChoice.PlaceId);
                    nP.NpBoatSelectList = npChoice.GetNpBoatNumberList(obChoice.PlaceId);
                    return PartialView("_GVChoice", nP);
                }


            }
            else
            {
                var res1 = new { status = 0, respone = "No Data Found" };
                return Json(res1, JsonRequestBehavior.AllowGet);
            }

            // return null;
        }
        /////Emitra Payment Section Start
        private void RedirectPGEmitra(NPChoice model)
        {
            string forms = "";
            decimal AmountToBePay = 0;
            string ChoiceRequestId = string.Empty;
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            DataSet DS = GetEmitraHeadForChoiceVehicleOrBoat(model.RequestId, 0, model.ChoiceType, liveUat); //Service ID to be decided in stored procedure
            if (DS != null && DS.Tables.Count > 0)
            {
                if (DS.Tables[0].Rows.Count > 0 && DS.Tables[0].Rows[0]["TicketId"].ToString().Length > 0)
                {
                    string REVENUEHEAD = string.Empty;
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        REVENUEHEAD = REVENUEHEAD + dr["EmitraHeadCode"] + "-" + dr["HeadAmount"] + "|";
                        AmountToBePay = AmountToBePay + Convert.ToDecimal(dr["HeadAmount"]);
                    }
                    REVENUEHEAD = REVENUEHEAD + Convert.ToString(DS.Tables[0].Rows[0]["ZeroAmtHead"]) + "|";
                    REVENUEHEAD = REVENUEHEAD.Trim('|');
                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                    ChoiceRequestId = DS.Tables[0].Rows[0]["TicketId"].ToString();
                    Session["ChoiceRequestId"] = ChoiceRequestId;

                    if (liveUat == 1)
                    {
                        //model.RequestId
                        forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(DS.Tables[0].Rows[0]["TicketId"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        ReturnUrl + "NationalPark/GVBPayment", ReturnUrl + "NationalPark/GVBPayment",
                        Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                        Convert.ToString(AmountToBePay), REVENUEHEAD, Session["User"].ToString());
                    }
                    else
                    { //model.RequestId
                        forms = ObjEmitraPayRequest.PayRequestLive(false, Convert.ToString(DS.Tables[0].Rows[0]["TicketId"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["ChecksumKey"]),
                        Convert.ToString(DS.Tables[0].Rows[0]["EncryptionKey"]),
                        ReturnUrl + "NationalPark/GVBPayment", ReturnUrl + "NationalPark/GVBPayment",
                        Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]), Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                        Convert.ToString(AmountToBePay), REVENUEHEAD, Session["User"].ToString(), "", Convert.ToString(DS.Tables[0].Rows[0]["ComType"]));
                    }
                    Response.Write(forms);
                }
            }
        }
        public ActionResult GVBPayment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";
                if (Request.Form["MERCHANTCODE"] != null)
                    MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                if (Request.Form["PRN"] != null)
                    PRN = Request.Form["PRN"].ToString();
                if (Request.Form["STATUS"] != null)
                    STATUS = Request.Form["STATUS"].ToString();
                if (Request.Form["ENCDATA"] != null)
                    ENCDATA = Request.Form["ENCDATA"].ToString();


                string ResponseStr = MERCHANTCODE + "|" + PRN + "|" + STATUS + "|" + ENCDATA;

                EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");

                string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                Np_PayStatus updateBooking = new Np_PayStatus
                {
                    UserId = UserID,
                    EmitraResponse = DecryptedData,
                    TransactionId = ObjPGResponse.TRANSACTIONID,
                    PaymentMode = (int)PaymentMode.Online,
                    RequestId = PRN,
                    EmitraAmount = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) > 0 ? (Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT)) : 0
                };

                //****************************** for test only

                //ObjPGResponse.STATUS = "SUCCESS";
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                //****************************** for test only;

                if (ObjPGResponse.STATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                {
                    updateBooking.ResponseStaus = (int)TransactionStatus.Paid;
                    updateBooking.isValidResponse = true;
                }
                else //PAYMENT FAILED
                {
                    updateBooking.isValidResponse = false;
                    updateBooking.ResponseStaus = (int)TransactionStatus.Failed;
                }


                Session["eMitraPGResponse"] = updateBooking;


                //Something went wrong.
                return RedirectToAction("ChoiceGuideVehicle", "NationalPark");

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }
        private Np_PayStatus EmitraKioskPayment(NPChoice model)
        {
            decimal AmountToBePay = 0;
            string ChoiceRequestId = string.Empty;
            Np_PayStatus np_PayStatus = new Np_PayStatus();
            int liveUat = Convert.ToInt16(ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            if (Session["EmitrServiceId"] != null)
            {
                DataSet DS = GetEmitraHeadForChoiceVehicleOrBoat(model.RequestId, Convert.ToInt32(Session["EmitrServiceId"]), model.ChoiceType, liveUat);
                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0 && DS.Tables[0].Rows[0]["TicketId"].ToString().Length > 0 && DS.Tables[2].Rows.Count > 0)
                    {
                        //decimal TotalAmount = Convert.ToDecimal(DS.Tables[0].Rows[0]["TotalAmountBePay"]);
                        EmitraKisokPayment emitraKisokPayment = new EmitraKisokPayment();
                        ChoiceRequestId = DS.Tables[0].Rows[0]["TicketId"].ToString();

                        string REVENUEHEAD = string.Empty;
                        foreach (DataRow dr in DS.Tables[1].Rows)
                        {
                            REVENUEHEAD = REVENUEHEAD + dr["EmitraHeadCode"] + "-" + dr["HeadAmount"] + "|";
                            AmountToBePay = AmountToBePay + Convert.ToDecimal(dr["HeadAmount"]);
                        }
                        REVENUEHEAD = REVENUEHEAD + Convert.ToString(DS.Tables[0].Rows[0]["ZeroAmtHead"]) + "|";
                        REVENUEHEAD = REVENUEHEAD.Trim('|');
                        decimal TotalAmount = AmountToBePay;

                        EmitraKioskRequest request = new EmitraKioskRequest
                        {
                            BASEURL = Convert.ToString(DS.Tables[2].Rows[0]["BaseUrl"]),
                            VERIFICAION_URL = Convert.ToString(DS.Tables[2].Rows[0]["VerificationUrl"]),
                            SERVICERESPONSETIME = Convert.ToInt16(DS.Tables[2].Rows[0]["MAX_RESPONSE_TIME_SEC"]),
                            MERCHANTCODE = Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]),
                            REQUESTID = DS.Tables[0].Rows[0]["TicketId"].ToString(),//model.RequestId,
                            REQTIMESTAMP = DateTime.Now.Ticks.ToString(),
                            SERVICEID = Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]),
                            SUBSERVICEID = "",
                            REVENUEHEAD = REVENUEHEAD,
                            CONSUMERKEY = DS.Tables[0].Rows[0]["TicketId"].ToString(),//,model.RequestId,
                            CONSUMERNAME = Convert.ToString(Session["KioskCtznName"]).ToUpper(),
                            SSOID = Convert.ToString(Session["KioskSSOId"]).ToUpper(),
                            OFFICECODE = Convert.ToString(DS.Tables[0].Rows[0]["DivCode"]),
                            //COMMTYPE = "2",
                            COMMTYPE = "3", ////Change by Amit on 02/09/2020 for Ematra changes 
                            SSOTOKEN = Convert.ToString(Session["SSOTOKEN"])
                        };
                        if (liveUat == 0)
                        {

                            request.BASEURL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/backtobackTransactionWithEncryptionA";
                            request.VERIFICAION_URL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/getTokenVerifyNewProcessByRequestIdWithEncryption";
                            request.OFFICECODE = "FORESTHQ";
                        }
                        if (!string.IsNullOrEmpty(request.OFFICECODE))
                        {
                            EmitraKioskResponse emitraKisokResponse = emitraKisokPayment.ProcessPayment(request, 1, "Choice Boat Service Call");

                            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

                            np_PayStatus.UserId = UserID;
                            np_PayStatus.EmitraResponse = emitraKisokResponse.RESPONSE;
                            np_PayStatus.TransactionId = emitraKisokResponse.TRANSACTIONID;
                            np_PayStatus.PaymentMode = (int)PaymentMode.Online;
                            np_PayStatus.RequestId = emitraKisokResponse.REQUESTID;
                            np_PayStatus.ChoiceRequestId = ChoiceRequestId;
                            np_PayStatus.EmitraAmount = Convert.ToDecimal(emitraKisokResponse.TRANSAMT) > 0 ? (Convert.ToDecimal(emitraKisokResponse.TRANSAMT) - Convert.ToDecimal(TotalAmount)) : 0;

                            if (emitraKisokResponse.TRANSACTIONSTATUS == "SUCCESS") //PAYMENT SUCCESSFUL
                            {
                                np_PayStatus.ResponseStaus = (int)TransactionStatus.Paid;
                                np_PayStatus.isValidResponse = true;
                            }
                            else //PAYMENT FAILED
                            {
                                np_PayStatus.ResponseStaus = (int)TransactionStatus.Failed;
                                np_PayStatus.isValidResponse = false;
                            }

                        }
                    }
                }
            }
            return np_PayStatus;
        }
        /// Emitra Payment Section End
        #endregion
    }
}
