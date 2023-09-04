//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : File contains calling functions from UI
//  Date Created : 22-08-2016
//  History      :
//  Version      : 1.0
//  Author       : Rajkumar
//  Modified By  : arvind k sharma  ( change Emitra Payment getway code )
//  Modified On  : 22/08/2016
//  Modified By  : arvind k sharma   ( new Emitra Payment getway code  )
//  Modified On  : 06/02/2017
//  Modified By  : arvind k sharma   ( add new TRDF Heads  )
//  Modified On  : 11/07/2017
//  Modified By  : arvind k sharma   ( Display Place wise new day info on header  )
//  Modified On  : 14/07/2017
//  Modified By  : arvind k sharma   ( add new Heads Vehicle Rent and Guide Fees  )
//  Modified On  : 25/07/2017
//  Reviewed By  : amit rajput
//  Reviewed On  : 14/07/2017
//********************************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using System.Configuration;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using FMDSS.Models.OnlineBooking;
using System.Globalization;
using FMDSS.Models.Master;
using System.IO;
using CaptchaMvc.HtmlHelpers;
using FMDSS.Models.CitizenService.PermissionService.EducationService;
using CaptchaLib;
using System.Threading;
using System.Threading.Tasks;
using FMDSS.Models.ReSchedule;

namespace FMDSS.Controllers.BookOnlineTicket
{
    [MyAuthorization]
    public class BookOnlineTicketController : BaseOnlinebookingRanthmboreController//BaseControllers //BaseOnlinebookingRanthmboreController
    {
        int ModuleID = 1;
        List<SelectListItem> lstPlace = new List<SelectListItem>();
        List<SelectListItem> Accomodation = new List<SelectListItem>();
        List<BookOnTicket> ticketList = new List<BookOnTicket>();
        BookOnTicket objBook = new BookOnTicket();
        WildLifeOnlineBooking objWildlifebooking = new WildLifeOnlineBooking();
        //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
        static object slno = new object();
        private static int ID = 0;
        static object emitraLock = new object();
       
        private string RequestId()
        {
            string requestid = DateTime.Now.Ticks.ToString();
            string RequestId =  objBook.GetRequestID(requestid);
            return RequestId;
        }


        public ActionResult ResendCancelationDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var MemberList = new List<BookOnTicket>();
            try
            {
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtf = new DataTable();
                dtf = Bok.GetResendCancelationDetails("CancelationDETAILS");

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        PlaceName = Convert.ToString(dr["PlaceName"]),
                        RequestId = Convert.ToString(dr["RequestID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["TicketAmount"]),
                        EmitraTransactionId = Convert.ToString(dr["EmitraTransactionID"]),
                        RefundAmount = Convert.ToDecimal(dr["RefundAmount"]),
                        Remark = Convert.ToString(dr["Remark"]),
                        IsPartialOrFullCancelation = Convert.ToInt32(dr["IsPartialOrFullCancelation"]),
                    });
                }

                ViewData["ticketCancelationDetaillist"] = ticketList;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResendCancelationDetails(BookOnTicket model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            BookOnTicket cs = new BookOnTicket();
            try
            {
                if (!string.IsNullOrEmpty(model.RequestId) && !string.IsNullOrEmpty(model.EmitraTransactionId))
                {
                    DataTable DT = new DataTable();
                    model.RequestId = Encryption.decrypt(model.RequestId);

                    DT = cs.SubmitFor_BookTicket_ForRefundProcessResend(model, "FULLCANCELATIONPROCESSREFUNDUPDATEDETAILS");
                    if (DT.Rows.Count > 0)
                    {
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        #region Email and SMS

                        objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                        #endregion


                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.</div>";
                    }

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("ResendCancelationDetails");
        }

        //********************************************************************************************************
        /// <summary>
        /// Returns online ticket user interface
        /// </summary>
        /// <returns></returns>
        public ActionResult BookOnlineTicket(string CT = "")
        {
            Session["CrossVarification"] = null;
            if (Session["LblPlaceName"] != null)
            {
                string PlaceLblName = Encryption.decrypt(CT);
                if (CT == "")
                {
                    CT = Encryption.encrypt(Session["LblPlaceName"].ToString());
                }
                if (Session["LblPlaceName"].ToString().Equals(PlaceLblName) != true && PlaceLblName != "")
                {
                    Session["CrossVarification"] = Session["LblPlaceName"].ToString();
                }
            }
            BookOnTicket Bok = new BookOnTicket();
            Session["ArrivalDateBackup"] = null;
            Session["ShiftBackup"] = null;
            Session["AvaliableTicket"] = null;
            Session["PlaceIdBackup"] = null;
            Session["ZoneIdBackup"] = null;
            Session["ShiftIdBackup"] = null;
            Session["VehicleIdBackup"] = null;
            Session["IsCurrentOrAdvanceBackup"] = null;


            Session["MemberFillingCount"] = null;
            Session["PlaceIdCurrentAdvanceSession"] = null;
            Session["ZoneIdCurrentAdvanceSession"] = null;
            Session["ArrivalDateCurrentAdvanceSession"] = null;
            Session["ShiftIdCurrentAdvanceSession"] = null;
            Session["VehicleIdCurrentAdvanceSession"] = null;
            Session["CheckAvailabilityTimeSessionStart"] = null;
            Session["CheckAvailabilityTimeSessionEnd"] = null;
            Session["NationalitySelectCount"] = null;
            Session["FirstNationalitySelectTimeSession"] = null;
            Session["NationlitySelectionInvalidSession"] = null;
            Session["strRows"] = null;
            Session["strRows_Time"] = null;
            Session["PlaceIdCurrentAdvanceTimeSession"] = null;
            Session["ArrivalDateCurrentAdvanceTimeSession"] = null;
            Session["VehicleIdCurrentAdvanceTimeSession"] = null;
            Session["CaptchaLoadTimeSession"] = null;
            Session["ServerTime"] = null;
            Session["CurrentToken"] = null;
            Session["ZoneCount"] = null;
            Session["DateOfArrivalCount"] = null;
            Session["ShiftCount"] = null;
            Session["VehicleCount"] = null;
            Session["SystemCurrentTime"] = DateTime.Now;
            Session["WTTime"] = null;
            Session["WTCl"] = null;
            Session["LblPlaceName"] = null;
            // Added By Hari Krishan BANJARA  ON 22/12/2022 ON THE BEHALF OF DINESHJI SUGGESTION
            Session["ArrivalDateBackupTatkal"] = null;
            Session["InvalidRequestTatkal"] = null;
            Session["CheckAvailabilityTimeSessionStartTatkal"] = null;
            Session["CheckAvailabilityTimeSessionEndTatkal"] = null;
            // Added By Hari Krishan BANJARA  ON 22/12/2022

            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).AddHours(-10).ToString("yyyy/MM/dd");//Change date 10 AM
            ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
            ViewBag.WT = "0";
            Session["AutomatedScript"] = null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            DataTable DT = Bok.IsValidUser(UserID);
            if (Convert.ToInt16(DT.Rows[0][0]) == 0)
            {
                Response.Redirect("~/UnAuthorizedUser.html", true);
            }


            // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
            if (Convert.ToString(Session["IsKioskUser"]).ToLower() == "true" || Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "true")  //|| Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "true" added on 12-04-2022 Mukesh Kumar Jangid
            {
                //this line added bcz kiosk user open direct current booking
                CT = "Q3VycmVudA==";
            }
            string ipAddress = "";
            var MemberList = new List<BookOnTicket>();
            try
            {
                #region Check this is current or Advance
                if (!string.IsNullOrEmpty(CT))
                {
                    string PlaceLblName = Encryption.decrypt(CT);
                    Session["LblPlaceName"] = PlaceLblName;
                    Session["CurrentBookingOrAdvanceBooking"] = PlaceLblName.ToLower() == "current" ? "1" : "2";
                    //To Check Is Agent booking count occurance Mukesh Kumar Jangid 19-11-2021
                    //if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1)
                    //{
                    //    string raw_ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    //    if (!string.IsNullOrEmpty(raw_ip))
                    //    {
                    //        string[] ipRange = raw_ip.Split(',');
                    //        int le = ipRange.Length - 1;
                    //        ipAddress = ipRange[0];
                    //    }
                    //    else
                    //    {
                    //        ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    //    }
                    //    var SetDayDif = ConfigurationManager.AppSettings["SetDayDif"].ToString();
                    //    DataTable dt = Bok.Is_AgentUsers(Session["SSOid"].ToString(), ipAddress, Convert.ToInt32(SetDayDif));
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        var bookingLimitCount = ConfigurationManager.AppSettings["BookingLimitCount"].ToString();
                    //        if (Convert.ToInt32(dt.Rows[0]["OccuranceCount"].ToString()) >= Convert.ToInt32(bookingLimitCount))
                    //        {

                    //            Random _random = new Random();
                    //            int waittime = _random.Next(5, 12);
                    //            waittime = waittime * 1000;
                    //            ViewBag.WT = "" + waittime;
                    //            Session["WTCl"] = waittime;
                    //            Session["WTTime"] = DateTime.Now;
                    //        }
                    //    }
                    //}

                    //above code to Check Is Agent booking count occurance Mukesh Kumar Jangid 19-11-2021
                }
                #endregion

                Session.Remove("AvaliableTicket");
                Session.Remove("VFeeTigerProject");
                Session.Remove("VFeeSurcharge");
                Session.Remove("TotaVechileFees");
                Session.Remove("totalprice");
                Session.Remove("RequestId");

                DataTable dtPlace = new DataTable();
                for (int i = 0; i < 20; i++)
                {
                    MemberList.Add(new BookOnTicket());
                }
                //shaan comment List 20-11-2020
                //DataTable dtf = new DataTable();
                //dtf = Bok.Select_BookedTicket();

                //foreach (DataRow dr in dtf.Rows)
                //{
                //    ticketList.Add(new BookOnTicket()
                //    {
                //        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                //        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                //        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                //        DateOfArrival = dr["DateOfArrival"].ToString(),
                //        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),

                //        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                //        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString()),
                //        CancelStatus = Convert.ToInt32(dr["CancelStatus"]),
                //        COVIDStatus = Convert.ToInt32(dr["COVIDStatus"]),
                //        TicketMemberBordingStatus = Convert.ToInt32(dr["TicketMemberBordingStatus"]),
                //        isDFOApproved = Convert.ToInt32(dr["isDFOApproved"]),
                //        RefundStatus = Convert.ToInt32(dr["RefundStatus"]),
                //        PlaceId = Convert.ToInt32(dr["PlaceId"])


                //    });
                //}



                //var list = ticketList.Take(5).ToList();

                //ViewData["ticketlist"] = list;
                //end 20-11-2020
                //*******************For Departmental Kiosk User********************************
                bool isPlace68 = false;
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACES(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(@dr["PlaceID"].ToString()) == 68)
                            isPlace68 = true;

                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                ////New change for emitra Kiosk 06-09-2018
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
                    //added by rajveer bcz kiosk user not immplemet in wildlife only dept user working so kiosk and dept usr working is same
                    Session["IsDepartmentalKioskUser"] = true;

                    #region KioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACES(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(@dr["PlaceID"].ToString()) == 68)
                            isPlace68 = true;
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ////added for removing 1st place from current booking
                    //if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1")
                    //{
                    //    lstPlace.RemoveAt(0);
                    //}
                    ViewBag.Place = lstPlace;
                    #endregion
                }

                else
                {
                    #region Place
                    dtPlace = Bok.Select_Place(UserID);
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        if (Convert.ToInt32(@dr["PlaceID"].ToString()) == 68)
                            isPlace68 = true;
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ////added for removing 1st place from current booking
                    //if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1")
                    //{
                    //    lstPlace.RemoveAt(0);
                    //}
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                //******************************************************************************
                string exDate = "";
                int IsLeapYear = 0;
                if (isPlace68 == true)
                {
                    var list = lstPlace.Where(c => c.Value == "68");
                    ViewBag.Place = list;
                    List<string> ExcludeDateList = new List<string>();
                   // ExcludeDateList.Add("");
                    string currentYear = DateTime.Now.Year.ToString();
                    int cYear = Convert.ToInt32(currentYear);
                    if (Convert.ToInt32(Session["CurrentBookingOrAdvanceBooking"]) == 1)
                    {
                        for (int y = cYear; y <= cYear; y++)
                        {
                           
                            IsLeapYear = y % 4;
                            for (int i = 7; i <= 9; i++)
                            {
                                int toDays = (i == 4 || i == 6 || i == 9 || i == 11 ? 30 : (IsLeapYear == 0 && i == 2 ? 29 : (IsLeapYear > 0 && i == 2 ? 28 : 31)));
                                for (int j = 1; j <= toDays; j++)
                                {                                    
                                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + (i < 10 ? "0" : "") + i + "/" + y.ToString();

                                        DateTime fdate = DateTime.Parse("30/06/2023");
                                        DateTime tdate = DateTime.Parse(exDate);

                                        if (tdate > fdate)
                                        {
                                            string dayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(exDate).DayOfWeek);
                                            if (dayName == "Wednesday")
                                                ExcludeDateList.Add(exDate);
                                        }                                   
                                }
                            }
                        }
                    }

                    ViewBag.ExcludeDateList = ExcludeDateList;
                    ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                   
                    List<string> ExcludeDateList = new List<string>();
                    string currentYear = DateTime.Now.Year.ToString();
                    int cYear = Convert.ToInt32(currentYear);
                    if (Convert.ToInt32(Session["CurrentBookingOrAdvanceBooking"]) == 2)
                    {
                        for (int y = cYear; y <= cYear + 1; y++)
                        {
                            //for (int i = 7; i < 10; i++)
                            //{
                            //    for (int j = 1; j <= (i == 9 ? 30 : 31); j++)
                            //    {
                            //        exDate = (j < 10 ? "0" + j : "" + j) + "/" + "0" + i + "/" + y.ToString();
                            //        ExcludeDateList.Add(exDate);
                            //    }
                            //}
                            IsLeapYear = y % 4;
                            for (int i = 1; i <= 12; i++)
                            {
                                int toDays = (i == 4 || i == 6 || i == 9 || i == 11 ? 30 : (IsLeapYear == 0 && i == 2 ? 29 : (IsLeapYear > 0 && i == 2 ? 28 : 31)));
                                for (int j = 1; j <= toDays; j++)
                                {
                                    if (i < 7 || i > 9)
                                    {

                                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + (i < 10 ? "0" : "") + i + "/" + y.ToString();

                                        DateTime fdate = DateTime.Parse("30/06/2023");
                                        DateTime tdate = DateTime.Parse(exDate);

                                        if (tdate > fdate)
                                        {
                                            string dayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(exDate).DayOfWeek);
                                            if (dayName == "Wednesday")
                                                ExcludeDateList.Add(exDate);
                                        }
                                    }
                                    else
                                    {
                                        exDate = (j < 10 ? "0" + j : "" + j) + "/" + (i < 10 ? "0" : "") + i + "/" + y.ToString();
                                        ExcludeDateList.Add(exDate);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        ExcludeDateList.Add(exDate);
                    }
                    String excludeList = Newtonsoft.Json.JsonConvert.SerializeObject(ExcludeDateList);

                    ViewBag.ExcludeDateList = ExcludeDateList;
                    ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                }

                #region OnlineBookingPopUp Developed by Rajveer

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "OnlineBooking";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                    ViewData["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;
                    ViewData["Title"] = string.Empty;

                }
                #endregion

                #region Booking Start 10.00 AM login    //changes done on 14-10-2020 by shaan
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1")
                    {
                        DateTime CurrentTime = DateTime.Now;
                        //DateTime CurrentTime = Convert.ToDateTime("10:00:01 AM");
                        DateTime t1 = Convert.ToDateTime("07:00:00 AM");
                        DateTime t2 = Convert.ToDateTime("10:00:00 AM");
                        //int i = DateTime.Compare(t1, t2);
                        if (CurrentTime > t1 && CurrentTime <= t2)
                        {
                            return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                        }


                        DataTable dtServerTime = Bok.GetServerTimeForCurrentBooking();
                        Session["ServerTime"] = dtServerTime.Rows[0]["OnlyTime"].ToString();
                        if (Session["ServerTime"] != null)
                        {
                            if (Convert.ToDateTime(Session["ServerTime"].ToString()) > Convert.ToDateTime("07:00 AM") && Convert.ToDateTime(Session["ServerTime"].ToString()) < Convert.ToDateTime("10:00 AM"))
                            {
                                return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                            }
                        }
                        else
                        {
                            return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                        }
                    }
                }
                else
                {
                    var PlacesIds = lstPlace.Select(x => x.Value).ToList();
                    if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1" && (!PlacesIds.Contains("53") || !PlacesIds.Contains("57")))

                    {
                        DateTime CurrentTime = DateTime.Now;
                        //DateTime CurrentTime = Convert.ToDateTime("10:00:01 AM");
                        DateTime t1 = Convert.ToDateTime("07:00:00 AM");
                        DateTime t2 = Convert.ToDateTime("10:00:00 AM");
                        //int i = DateTime.Compare(t1, t2);
                        if (CurrentTime > t1 && CurrentTime < t2)
                        {
                            return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                        }

                        DataTable dtServerTime = Bok.GetServerTimeForCurrentBooking();
                        Session["ServerTime"] = dtServerTime.Rows[0]["OnlyTime"].ToString();
                        if (Session["ServerTime"] != null)
                        {
                            if (Convert.ToDateTime(Session["ServerTime"].ToString()) > Convert.ToDateTime("07:00 AM") && Convert.ToDateTime(Session["ServerTime"].ToString()) < Convert.ToDateTime("10:00 AM"))
                            {
                                return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                            }
                        }
                        else
                        {
                            return RedirectToAction("BookingStartCounter", "BookOnlineTicket");
                        }
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MemberList);
        }

        public string GenerateEncToken()
        {
            string encOutputString = string.Empty;
            Random rndNumber = new Random();
            int number = rndNumber.Next();
            encOutputString = Common.Encrypt(number.ToString());
            return encOutputString;
        }
        public string GetDecToken(string encOutputString)
        {
            string decOutputString = string.Empty;
            decOutputString = Common.Decrypt(encOutputString);
            return decOutputString;
        }


        /// <summary>
        /// Get Complete WildLife Ticket History
        /// </summary>
        /// <param name="NA"></param>
        /// <returns></returns>
        /// 
        public ActionResult WildLifeTicketHistory()
        {

            ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var MemberList = new List<BookOnTicket>();
            try
            {
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicket();

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        CGTickets = Convert.ToInt64(dr["CGTickets"].ToString()),
                        CGTCount = Convert.ToInt32(dr["CGTCount"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),

                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString()),
                        CancelStatus = Convert.ToInt32(dr["CancelStatus"]),
                        COVIDStatus = Convert.ToInt32(dr["COVIDStatus"]),
                        TicketMemberBordingStatus = Convert.ToInt32(dr["TicketMemberBordingStatus"]),
                        isDFOApproved = Convert.ToInt32(dr["isDFOApproved"]),
                        RefundStatus = Convert.ToInt32(dr["RefundStatus"]),
                        PlaceId = Convert.ToInt32(dr["PlaceId"]),
                        RequestedId = "" + dr["RequestedId"],
                        OldRequestID=""  +dr["OldRequestId"].ToString(),
                        Reserve_Status = "" + dr["Reserve_Status"].ToString(),
                        WaitingStatus = "" + dr["WaitingStatus"].ToString(),
                    });
                }

                ViewData["ticketlist"] = ticketList;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        /// <summary>
        /// Function for check safari and accomodation avaliable
        /// </summary>
        /// <param name="PlaceID"></param>
        /// <returns></returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckSafariAccomoAvailability(int PlaceID)
        {
            //start for place select before 10 am gaurab
            TimeSpan currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Session["PlaceSelectTime"] =currentTime; // new TimeSpan(06, 00, 00); 
            // end for place select before 10 am gaurab
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> lstZone = new List<SelectListItem>();
            BookOnTicket bot1 = null;
            string CurrentBookingDays = "0";
            string NEWDATEOPEN = string.Empty;
            string OpenBookingDays = "0";
            string encString = string.Empty;
            string decString = string.Empty;
            try
            {
                if (PlaceID == 2 || PlaceID == 68)
                {
                    encString = GenerateEncToken();             
                    decString = "P-" + GetDecToken(encString);    
                    Session["CurrentToken"] = decString;
                }

                if (Session["CurrentBookingOrAdvanceBooking"] != null)
                {
                    Session["PlaceIdCurrentAdvanceTimeSession"] = DateTime.Now.ToString();
                    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                    {
                        Session["AvaliableTicket"] = null;
                        Session["ZoneIdCurrentAdvanceSession"] = null;
                        Session["ArrivalDateCurrentAdvanceSession"] = null;
                        Session["ShiftIdCurrentAdvanceSession"] = null;
                        Session["VehicleIdCurrentAdvanceSession"] = null;
                        Session["ArrivalDateCurrentAdvanceTimeSession"] = null;
                        Session["VehicleIdCurrentAdvanceTimeSession"] = null;
                        string pageType = Session["CurrentBookingOrAdvanceBooking"].ToString();
                        if (pageType == "1" && (PlaceID == 2 || PlaceID==68))
                        {
                            Session["PlaceIdCurrentAdvanceSession"] = PlaceID;
                        }
                        if (pageType == "2" && (PlaceID == 2 || PlaceID == 53 || PlaceID == 57))
                        {
                            Session["PlaceIdCurrentAdvanceSession"] = PlaceID;
                        }

                    }
                    else
                    {
                        //shaan changes 21-12-2020
                        if (PlaceID == 2)
                        {
                            Session["AvaliableTicket"] = null;
                            Session["ZoneIdCurrentAdvanceSession"] = null;
                            Session["ArrivalDateCurrentAdvanceSession"] = null;
                            Session["ShiftIdCurrentAdvanceSession"] = null;
                            Session["VehicleIdCurrentAdvanceSession"] = null;
                            Session["ArrivalDateCurrentAdvanceTimeSession"] = null;
                            Session["VehicleIdCurrentAdvanceTimeSession"] = null;
                            string pageType = Session["CurrentBookingOrAdvanceBooking"].ToString();
                            if (pageType == "1" || pageType == "2")
                            {
                                Session["PlaceIdCurrentAdvanceSession"] = PlaceID;
                            }

                        }
                        else
                        {
                            Session["PlaceIdCurrentAdvanceSession"] = PlaceID;
                        }
                        //end
                        //Session["PlaceIdCurrentAdvanceSession"] = PlaceID;
                    }


                }

                BookOnTicket bot = new BookOnTicket();
                DataSet dsSafariAccomodation = new DataSet();
                bot.PlaceId = Convert.ToInt64(PlaceID);
                dsSafariAccomodation = bot.chkSafariAccomo();
                if (dsSafariAccomodation.Tables.Count > 0)
                {
                    bot1 = new BookOnTicket();
                    if (dsSafariAccomodation.Tables[0].Rows.Count > 0)
                    {
                        bot1.isSafari = dsSafariAccomodation.Tables[0].Rows[0]["IsSafari"].ToString();
                        bot1.isAccomo = dsSafariAccomodation.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {
                        bot1.isSafari = "";
                        bot1.isAccomo = "";
                    }
                    if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }
                    if (dsSafariAccomodation.Tables[2].Rows.Count > 0)
                    {
                        bot1.DurationFrom = dsSafariAccomodation.Tables[2].Rows[0]["DurationFromDate"].ToString();
                        bot1.DurationTo = dsSafariAccomodation.Tables[2].Rows[0]["DurationToDate"].ToString();
                        TempData["DurationTo"] = bot1.DurationTo;
                    }
                    else
                    {

                        bot1.DurationFrom = "NF";
                        bot1.DurationTo = "NF";
                        TempData["DurationTo"] = "";
                    }
                    Session["NEWDATEOPEN"] = "0";
                    if (dsSafariAccomodation.Tables[3].Rows.Count > 0)
                    {
                        NEWDATEOPEN = Convert.ToString(dsSafariAccomodation.Tables[3].Rows[0]["NEWDATEOPEN"]);
                        ViewBag.NEWDATEOPEN = NEWDATEOPEN;

                        #region open Date 10 AM every day
                        Session["NEWDATEOPEN"] = null;
                        Session["NEWDATEOPEN"] = NEWDATEOPEN;
                        #endregion

                        //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
                        ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
                    }
                    if (dsSafariAccomodation.Tables[4].Rows.Count > 0)
                    {
                        OpenBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["OpenBookingDuration"].ToString();
                        CurrentBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["CurrentDateOpen"].ToString();
                    }




                }
                else
                {
                    bot1 = new BookOnTicket();
                    bot1.isSafari = "";
                    bot1.isAccomo = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = bot1.isSafari, list2 = bot1.isAccomo, list3 = lstZone, list4 = bot1.DurationFrom, list5 = bot1.DurationTo, NEWDATEOPEN = NEWDATEOPEN, list6 = OpenBookingDays, list7 = CurrentBookingDays, list8 = encString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckSafariAccomoAvailabilityNew(int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> lstZone = new List<SelectListItem>();
            BookOnTicket bot1 = null;
            string CurrentBookingDays = "0";
            string EndBookingDays = "0";
            string NEWDATEOPEN = string.Empty;
            string OpenBookingDays = "0";
            try
            {
                BookOnTicket bot = new BookOnTicket();
                DataSet dsSafariAccomodation = new DataSet();
                bot.PlaceId = Convert.ToInt64(PlaceID);
                dsSafariAccomodation = bot.chkSafariAccomo();
                if (dsSafariAccomodation.Tables.Count > 0)
                {
                    bot1 = new BookOnTicket();
                    if (dsSafariAccomodation.Tables[0].Rows.Count > 0)
                    {
                        bot1.isSafari = dsSafariAccomodation.Tables[0].Rows[0]["IsSafari"].ToString();
                        bot1.isAccomo = dsSafariAccomodation.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {
                        bot1.isSafari = "";
                        bot1.isAccomo = "";
                    }
                    if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                        //added below to remove modified route for adv booking
                        if ((PlaceID == 53 || PlaceID == 57) && Session["CurrentBookingOrAdvanceBooking"].ToString() == "2")
                        {
                            lstZone.Remove(lstZone.Where(c => c.Text.Contains("Modified Route")).FirstOrDefault());

                        }
                    }
                    if (dsSafariAccomodation.Tables[2].Rows.Count > 0)
                    {
                        bot1.DurationFrom = dsSafariAccomodation.Tables[2].Rows[0]["DurationFromDate"].ToString();
                        bot1.DurationTo = dsSafariAccomodation.Tables[2].Rows[0]["DurationToDate"].ToString();
                        TempData["DurationTo"] = bot1.DurationTo;
                    }
                    else
                    {

                        bot1.DurationFrom = "NF";
                        bot1.DurationTo = "NF";
                        TempData["DurationTo"] = "";
                    }
                    Session["NEWDATEOPEN"] = "0";
                    if (dsSafariAccomodation.Tables[3].Rows.Count > 0)
                    {
                        NEWDATEOPEN = Convert.ToString(dsSafariAccomodation.Tables[3].Rows[0]["NEWDATEOPEN"]);
                        ViewBag.NEWDATEOPEN = NEWDATEOPEN;

                        #region open Date 10 AM every day
                        Session["NEWDATEOPEN"] = null;
                        Session["NEWDATEOPEN"] = NEWDATEOPEN;
                        #endregion

                        //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
                        ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
                    }
                    if (dsSafariAccomodation.Tables[4].Rows.Count > 0)
                    {
                        OpenBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["OpenBookingDuration"].ToString();
                        //CurrentBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["CurrentDateOpen"].ToString();
                        CurrentBookingDays = DateTime.Now.Date.AddDays(2).ToString("dd/MM/yyyy"); //"01/08/2020";
                        EndBookingDays = "31/06/2022";
                    }




                }
                else
                {
                    bot1 = new BookOnTicket();
                    bot1.isSafari = "";
                    bot1.isAccomo = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = bot1.isSafari, list2 = bot1.isAccomo, list3 = lstZone, list4 = bot1.DurationFrom, list5 = bot1.DurationTo, NEWDATEOPEN = NEWDATEOPEN, list6 = OpenBookingDays, list7 = CurrentBookingDays, list8 = EndBookingDays }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckSafariAccomoAvailabilityNewHDFD(int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> lstZone = new List<SelectListItem>();
            BookOnTicket bot1 = null;
            string CurrentBookingDays = "0";
            string EndBookingDays = "0";
            string NEWDATEOPEN = string.Empty;
            string OpenBookingDays = "0";
            try
            {
                BookOnTicket bot = new BookOnTicket();
                DataSet dsSafariAccomodation = new DataSet();
                bot.PlaceId = Convert.ToInt64(PlaceID);
                dsSafariAccomodation = bot.chkSafariAccomoHDFD();
                if (dsSafariAccomodation.Tables.Count > 0)
                {
                    bot1 = new BookOnTicket();
                    if (dsSafariAccomodation.Tables[0].Rows.Count > 0)
                    {
                        bot1.isSafari = dsSafariAccomodation.Tables[0].Rows[0]["IsSafari"].ToString();
                        bot1.isAccomo = dsSafariAccomodation.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {
                        bot1.isSafari = "";
                        bot1.isAccomo = "";
                    }
                    if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }
                    if (dsSafariAccomodation.Tables[2].Rows.Count > 0)
                    {
                        bot1.DurationFrom = dsSafariAccomodation.Tables[2].Rows[0]["DurationFromDate"].ToString();
                        bot1.DurationTo = dsSafariAccomodation.Tables[2].Rows[0]["DurationToDate"].ToString();
                        TempData["DurationTo"] = bot1.DurationTo;
                    }
                    else
                    {

                        bot1.DurationFrom = "NF";
                        bot1.DurationTo = "NF";
                        TempData["DurationTo"] = "";
                    }
                    Session["NEWDATEOPEN"] = "0";
                    if (dsSafariAccomodation.Tables[3].Rows.Count > 0)
                    {
                        NEWDATEOPEN = Convert.ToString(dsSafariAccomodation.Tables[3].Rows[0]["NEWDATEOPEN"]);
                        ViewBag.NEWDATEOPEN = NEWDATEOPEN;

                        #region open Date 10 AM every day
                        Session["NEWDATEOPEN"] = null;
                        Session["NEWDATEOPEN"] = NEWDATEOPEN;
                        #endregion

                        //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
                        ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
                    }
                    if (dsSafariAccomodation.Tables[4].Rows.Count > 0)
                    {
                        OpenBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["OpenBookingDuration"].ToString();
                        //CurrentBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["CurrentDateOpen"].ToString();
                        CurrentBookingDays = DateTime.Now.Date.AddDays(2).ToString("dd/MM/yyyy");//"01/07/2020";
                        EndBookingDays = "31/07/2022";
                    }




                }
                else
                {
                    bot1 = new BookOnTicket();
                    bot1.isSafari = "";
                    bot1.isAccomo = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = bot1.isSafari, list2 = bot1.isAccomo, list3 = lstZone, list4 = bot1.DurationFrom, list5 = bot1.DurationTo, NEWDATEOPEN = NEWDATEOPEN, list6 = OpenBookingDays, list7 = CurrentBookingDays, list8 = EndBookingDays }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CurentTime()
        {
            DateTime start = new DateTime(Convert.ToInt32(DateTime.Now.Year), Convert.ToInt32(DateTime.Now.Month), Convert.ToInt32(DateTime.Now.Day), 23, 59, 0); //10 o'clock
            DateTime end = new DateTime(Convert.ToInt32(DateTime.Now.Year), Convert.ToInt32(DateTime.Now.Month), Convert.ToInt32(DateTime.Now.Day), 23, 59, 59); //12 o'clock
            DateTime now = DateTime.Now;

            string status = "0";

            if ((now > start) && (now < end))
            {
                status = "1";
            }
            return Json(new { Action = "BookOnlineTicket", status = status }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// check booking durations
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="Zone"></param>
        /// <returns></returns>

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckBookingDurations(int placeID, string Date)
        {
            string status = "0";
            string DMessage = string.Empty;

            BookOnTicket bkt = new BookOnTicket();
            DataTable DT = new DataTable();
            bkt.PlaceId = Convert.ToInt64(placeID);
            bkt.DateOfArrival = Date;

            DT = bkt.Select_CheckBookingDurations();

            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["STATUS"]) == "1")
                {
                    status = "1";

                }
                else if (Convert.ToString(DT.Rows[0]["STATUS"]) == "0")
                {
                    status = "0";
                    DMessage = "Date of Visit  must be between " + DT.Rows[0]["TicketDurationFromDate"] + " and " + DT.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September";
                }

            }

            //TicketDurationFromDate

            //TicketDurationToDate

            return Json(new { Dstatus = status, DMessage = DMessage, JsonRequestBehavior.AllowGet });
        }
        /// <summary>
        /// Function to bind shift based on place and zone
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="Zone"></param>
        /// <returns></returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult BindShiftByPlaceZone(int placeID, int Zone, string ArrivalDate, string authtk)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();
            string encString = string.Empty;
            string decString = string.Empty;
            try
            {

                // changes done by shaan 05/11/2020
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    Session["AvaliableTicket"] = null;
                    Session["ShiftIdCurrentAdvanceSession"] = null;
                    Session["VehicleIdCurrentAdvanceSession"] = null;
                    Session["VehicleIdCurrentAdvanceTimeSession"] = null;
                }
                else
                {
                    // changes by shaan 21-02-2020
                    if (placeID == 2 || placeID==68)
                    {
                        Session["AvaliableTicket"] = null;
                        Session["ShiftIdCurrentAdvanceSession"] = null;
                        Session["VehicleIdCurrentAdvanceSession"] = null;
                        Session["VehicleIdCurrentAdvanceTimeSession"] = null;
                    }
                    // end 
                }
                // changes done by shaan 05/11/2020
                if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null)
                {
                    if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString())
                    {
                        Session["ArrivalDateCurrentAdvanceSession"] = ArrivalDate;
                        Session["ArrivalDateCurrentAdvanceTimeSession"] = DateTime.Now.ToString();
                    }

                }
                //end

                BookOnTicket bkt = new BookOnTicket();
                DataTable dtShift = new DataTable();

                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    // === set default date and shift as per database 
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;
                    DataTable DT_BoardingDuration = new DataTable();
                    DT_BoardingDuration = obj.GetBoardingDuration(Convert.ToString(placeID));

                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                    {
                        // =========== EVENING_SHIFT
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "2";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "False",
                                isEvening = "True",
                                isFullDay = "False",
                            });
                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                    {
                        // =========== MORNING_SHIFT                       
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "1";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "True",
                                isEvening = "False",
                                isFullDay = "False",
                            });
                        }
                    }
                    // === set default date and shift as per database  
                    #endregion

                }
                else
                {
                    #region Citizen User
                    bkt.PlaceId = Convert.ToInt64(placeID);
                    bkt.ZoneId = Convert.ToInt64(Zone);
                    bkt.DateOfArrival = ArrivalDate;
                    dtShift = bkt.Select_Shift_By_PlacesZones();
                    if (dtShift.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtShift.Rows)
                        {
                            fees.Add(new BookOnTicket()
                            {
                                DateOfArrival = bkt.DateOfArrival,
                                isMorning = dr["isMorning"].ToString(),
                                isEvening = dr["isEvening"].ToString(),
                                isFullDay = dr["isFullDay"].ToString(),
                            });
                        }
                    }
                    else
                    {
                        fees.Add(new BookOnTicket()
                        {
                            DateOfArrival = bkt.DateOfArrival,
                            isMorning = "",
                            isEvening = "",
                            isFullDay = "",
                        });
                    }

                    #region Check Current Shift In Current Booking Restrict Inspect element Data Value Hard coded
                    Session["CheckShiftwiseValue"] = null;
                    if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1)
                    {
                        Session["CheckShiftwiseValue"] = fees;
                    }

                    #endregion
                    #endregion

                }
                DataTable dta = new DataTable();
                bkt.PlaceId = Convert.ToInt64(placeID);
                dta = bkt.GetAccomodationType();
                ViewBag.Accomo = dta;
                foreach (System.Data.DataRow dr in ViewBag.Accomo.Rows)
                {
                    Accomodation.Add(new SelectListItem { Text = @dr["RoomType"].ToString(), Value = @dr["AccommodationID"].ToString() });
                }
                if (placeID == 2 || placeID == 68)
                {
                    string CurrentToken = Session["CurrentToken"].ToString();
                    string CurrentTokenLast = CurrentToken.Split('-').Last();
                    if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("Z") && Session["DateOfArrivalCount"] == null)
                    {
                        Session["CurrentToken"] = null;
                        encString = GenerateEncToken();
                        decString = "D-" + GetDecToken(encString);
                        Session["CurrentToken"] = decString;
                        Session["DateOfArrivalCount"] = 1;

                        return Json(new { list1 = fees, list2 = Accomodation, list3 = DateTime.Now.ToString("dd/MM/yyyy"), list4 = encString, JsonRequestBehavior.AllowGet });
                    }
                    else if (Session["DateOfArrivalCount"] != null)
                    {
                        if (Convert.ToInt32(Session["DateOfArrivalCount"]) == 1)
                        {
                            Session["CurrentToken"] = null;
                            encString = GenerateEncToken();
                            decString = "D-" + GetDecToken(encString);
                            Session["CurrentToken"] = decString;
                            Session["DateOfArrivalCount"] = 1;
                            return Json(new { list1 = fees, list2 = Accomodation, list3 = DateTime.Now.ToString("dd/MM/yyyy"), list4 = encString, JsonRequestBehavior.AllowGet });
                        }
                        //vehiclelist = null;
                    }
                    else
                    {
                        return Json(new { list1 = "", list2 = "", list3 = "",list4="", JsonRequestBehavior.AllowGet });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = fees, list2 = Accomodation, list3 = DateTime.Now.ToString("dd/MM/yyyy"), list4 = "", JsonRequestBehavior.AllowGet });
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult BindShiftSession(int placeID, int Zone, string ArrivalDate, int ShiftTime, string authtk)
        {
            int ShiftIdCurrentAdvanceSession = 0;
            string encString = string.Empty;
            string decString = string.Empty;
            // changes done by shaan 05/11/2020
            if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
            {

                Session["VehicleIdCurrentAdvanceSession"] = null;
                Session["AvaliableTicket"] = null;
            }
            else
            {
                if (placeID == 2)
                {
                    Session["VehicleIdCurrentAdvanceSession"] = null;
                    Session["AvaliableTicket"] = null;
                }
            }
            if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null)
            {
                if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString() && Session["ArrivalDateCurrentAdvanceSession"].ToString() == ArrivalDate)
                {
                    Session["ShiftIdCurrentAdvanceSession"] = ShiftTime;
                    ShiftIdCurrentAdvanceSession = 1;
                }

            }
            if (placeID == 2 || placeID == 68)
            {
                string CurrentToken = Session["CurrentToken"].ToString();
                string CurrentTokenLast = CurrentToken.Split('-').Last();
                if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("D") && Session["ShiftCount"] == null)
                {
                    Session["CurrentToken"] = null;
                    encString = GenerateEncToken();
                    decString = "S-" + GetDecToken(encString);
                    Session["CurrentToken"] = decString;
                    Session["ShiftCount"] = 1;
                    return Json(new { obj1 = ShiftIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
                }
                else if (Session["ShiftCount"] != null)
                {
                    if (Convert.ToInt32(Session["ShiftCount"]) == 1)
                    {
                        Session["CurrentToken"] = null;
                        encString = GenerateEncToken();
                        decString = "S-" + GetDecToken(encString);
                        Session["CurrentToken"] = decString;
                        Session["ShiftCount"] = 1;
                        return Json(new { obj1 = ShiftIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
                    }
                    //vehiclelist = null;
                }
                else
                {
                    return Json(new { obj1 = "", obj2 = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            //end
            return Json(new { obj1 = ShiftIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult BindVehicleSession(int placeID, int Zone, string ArrivalDate, int ShiftTime, int vehicleID, string authtk)
        {
            int VehicleIdCurrentAdvanceSession = 0;
            string encString = string.Empty;
            string decString = string.Empty;
            if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
            {
                Session["AvaliableTicket"] = null;
            }
            else
            {
                //changes by shaan 21-02-2020
                if (placeID == 2)
                {
                    Session["AvaliableTicket"] = null;
                }
                //end 
            }
            if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null)
            {
                if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == ShiftTime.ToString())
                {
                    Session["VehicleIdCurrentAdvanceTimeSession"] = DateTime.Now.ToString();
                    Session["VehicleIdCurrentAdvanceSession"] = vehicleID;
                    VehicleIdCurrentAdvanceSession = 1;
                }

            }
            if (placeID == 2 || placeID == 68)
            {
                string CurrentToken = Session["CurrentToken"].ToString();
                string CurrentTokenLast = CurrentToken.Split('-').Last();
                if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("S") && Session["VehicleCount"] == null)
                {
                    Session["CurrentToken"] = null;
                    encString = GenerateEncToken();
                    decString = "V-" + GetDecToken(encString);
                    Session["CurrentToken"] = decString;
                    Session["VehicleCount"] = 1;
                    return Json(new { obj1 = VehicleIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
                }
                else if (Session["VehicleCount"] != null)
                {
                    if (Convert.ToInt32(Session["VehicleCount"]) == 1)
                    {
                        Session["CurrentToken"] = null;
                        encString = GenerateEncToken();
                        decString = "V-" + GetDecToken(encString);
                        Session["CurrentToken"] = decString;
                        Session["VehicleCount"] = 1;
                        return Json(new { obj1 = VehicleIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
                    }
                    //vehiclelist = null;
                }
                else
                {
                    return Json(new { obj1 = "", obj2 = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { obj1 = VehicleIdCurrentAdvanceSession, obj2 = encString }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Function for check ticket availability
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="shifttime"></param>
        /// <param name="Zone"></param>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CheckTicketAvailability(int placeID, string arrivaldate, string shifttime, int Zone, int vehicleID, string captcha, string authtk)
        {
            //       string captchaResponse = HttpContext.Request.Form["g-recaptcha"];
            // string captchaResponse1 = HttpContext.Request.Form["g-Recaptcha-Response"];
            // string captchaResponse2 = HttpContext.Request.Form["g-Recaptcha-Response"];
            Session["CheckAvailTime"] = DateTime.Now;
            Session["AvaliableTicket"] = null;
            if (Session["CurrentBookingOrAdvanceBooking"] != null)
            {
                Session["PlaceIdBackup"] = placeID;
                Session["ZoneIdBackup"] = Zone;
                Session["ShiftIdBackup"] = shifttime;
                Session["VehicleIdBackup"] = vehicleID;
                Session["IsCurrentOrAdvanceBackup"] = Session["CurrentBookingOrAdvanceBooking"];
            }
            else
            {
                Session["PlaceIdBackup"] = null;
                Session["ZoneIdBackup"] = null;
                Session["ShiftIdBackup"] = null;
                Session["VehicleIdBackup"] = null;
                Session["IsCurrentOrAdvanceBackup"] = null;
            }

            DateTime StartSession = Convert.ToDateTime(Session["CaptchaLoadTimeSession"]);
            DateTime EndSession = Convert.ToDateTime(DateTime.Now.ToString());
            var diffInSeconds = (EndSession - StartSession).TotalSeconds;

            string strStatus = string.Empty;

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtTicketdetails = new DataTable();
            string encString = string.Empty;
            string decString = string.Empty;
            int byPassCondition = 1; // this variable is create on 06-06-2022 by Mukesh as Per
                                     // Instructions from forest department to remove the captcha.
            try
            {
                string SessionCaptcha = System.Web.HttpContext.Current.Session["Captcha"].ToString();
                if (byPassCondition == 1 || (String.Equals(captcha, SessionCaptcha) && diffInSeconds > 7))
                {
                    if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null && Session["VehicleIdCurrentAdvanceSession"] != null)
                    {
                        if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == shifttime.ToString() && Session["VehicleIdCurrentAdvanceSession"].ToString() == vehicleID.ToString())
                        {
                            // Check 1 second difference between place/ arrivaldate / vehicle changes done by shaan 15 / 01 / 2021

                            DateTime StartSessionA = Convert.ToDateTime(Session["PlaceIdCurrentAdvanceTimeSession"]);
                            DateTime EndSessionB = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
                            var diffInSecondsbetweenPlace_ArrivalDate = (EndSessionB - StartSessionA).TotalSeconds;
                            DateTime StartSessionC = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
                            DateTime EndSessionD = Convert.ToDateTime(Session["VehicleIdCurrentAdvanceTimeSession"]);
                            var diffInSecondsbetweenArrivalDate_Vehicle = (EndSessionD - StartSessionC).TotalSeconds;
                            //end shaan 15/01/2021
                            //if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
                            //{

                            BookOnTicket bkt = new BookOnTicket();
                            bkt.PlaceId = placeID;
                            bkt.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                            bkt.ShiftTime = shifttime;
                            bkt.ZoneId = Zone;
                            bkt.vehicleID = vehicleID;
                            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                            //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false")
                            //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
                            if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                            {
                                bkt.KioskUserId = "0";

                            }
                            else
                            {
                                bkt.KioskUserId = Session["KioskUserId"].ToString();
                            }

                            string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
                            bool IsValidRequestForCurrentOrAdvanced = false;
                            bool IsStringFill = true;
                            if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                            {
                                if (placeID == 53 || placeID == 57)
                                {
                                    if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy"))
                                    {
                                        IsValidRequestForCurrentOrAdvanced = true;
                                    }
                                }
                                else
                                {
                                    if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
                                    {
                                        IsValidRequestForCurrentOrAdvanced = true;

                                    }
                                }

                                if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
                                {
                                    IsStringFill = false;
                                }
                                if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
                                {
                                    IsStringFill = false;
                                }
                                Session["CheckAvailabilityTimeSessionStart"] = null;
                                Session["CheckAvailabilityTimeSessionEnd"] = null;
                                Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
                                Session["NationlitySelectionInvalidSession"] = null;
                                Session["strRows"] = null;
                                Session["strRows_Time"] = null;
                                Session["ArrivalDateBackup"] = null;
                                Session["ShiftBackup"] = null;
                            }
                            else
                            {
                                // changes by shaan 21-02-2020
                                // Below Three line added on 11-04-2022 
                                Session["CheckAvailabilityTimeSessionStart"] = null;
                                Session["CheckAvailabilityTimeSessionEnd"] = null;
                                Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
                                // Above Three line added on 11-04-2022 
                                if (placeID == 2)
                                {
                                    if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
                                    {
                                        IsValidRequestForCurrentOrAdvanced = true;

                                    }
                                    if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
                                    {
                                        IsStringFill = false;
                                    }
                                    if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
                                    {
                                        IsStringFill = false;
                                    }
                                    Session["CheckAvailabilityTimeSessionStart"] = null;
                                    Session["CheckAvailabilityTimeSessionEnd"] = null;
                                    Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
                                    Session["NationlitySelectionInvalidSession"] = null;
                                    Session["strRows"] = null;
                                    Session["strRows_Time"] = null;
                                    Session["ArrivalDateBackup"] = arrivaldate;
                                    Session["ShiftBackup"] = shifttime;
                                }
                                // end
                            }

                            if (placeID == 2 || placeID == 68)
                            {
                                string CurrentToken = Session["CurrentToken"].ToString();
                                string CurrentTokenLast = CurrentToken.Split('-').Last();
                                if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("V"))
                                {
                                    Session["CurrentToken"] = null;
                                    encString = GenerateEncToken();
                                    decString = "CT-" + GetDecToken(encString);
                                    Session["CurrentToken"] = decString;
                                    IsStringFill = true;
                                }
                                else
                                {
                                    IsStringFill = false;
                                }
                            }
                            if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1" && placeID != 53 && placeID != 57)
                            {
                                if (Session["ServerTime"] != null)
                                {
                                    if (Convert.ToDateTime(Session["ServerTime"].ToString()) > Convert.ToDateTime("07:00 AM") && Convert.ToDateTime(Session["ServerTime"].ToString()) < Convert.ToDateTime("10:00 AM"))
                                    {
                                        IsStringFill = false;
                                    }
                                }
                                else
                                {
                                    IsStringFill = false;
                                }
                            }


                            if (IsStringFill)
                            {
                                //dtTicketdetails = bkt.CheckTicketAvailability();
                                dtTicketdetails = await bkt.CheckTicketAvailabilityWityPalaceOfWheel();

                                if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
                                {
                                    if (placeID == 2)
                                    {
                                        if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
                                        {
                                            if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
                                            {
                                                Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                                                Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
                                                Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
                                                Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                                                strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
                                                Session["ArrivalDateBackup"] = arrivaldate;
                                                Session["ShiftBackup"] = shifttime;
                                            }
                                            else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
                                            {
                                                Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                                                strStatus = Session["AvaliableTicket"] + "#";
                                                Session["ArrivalDateBackup"] = arrivaldate;
                                                Session["ShiftBackup"] = shifttime;
                                            }
                                            else
                                            {
                                                Session.Remove("AvaliableTicket");
                                                Session.Remove("VFeeTigerProject");
                                                Session.Remove("VFeeSurcharge");
                                                Session.Remove("TotaVechileFees");
                                                strStatus = "0#";
                                            }
                                        }
                                        else
                                        {
                                            Session.Remove("AvaliableTicket");
                                            Session.Remove("VFeeTigerProject");
                                            Session.Remove("VFeeSurcharge");
                                            Session.Remove("TotaVechileFees");
                                            strStatus = "0#!";//16/01/2021
                                        }
                                    }
                                    else
                                    {
                                        if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
                                        {
                                            Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                                            Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
                                            Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
                                            Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                                            strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
                                            Session["ArrivalDateBackup"] = arrivaldate;
                                            Session["ShiftBackup"] = shifttime;
                                        }
                                        else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
                                        {
                                            Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                                            strStatus = Session["AvaliableTicket"] + "#";
                                            Session["ArrivalDateBackup"] = arrivaldate;
                                            Session["ShiftBackup"] = shifttime;
                                        }
                                        else
                                        {
                                            Session.Remove("AvaliableTicket");
                                            Session.Remove("VFeeTigerProject");
                                            Session.Remove("VFeeSurcharge");
                                            Session.Remove("TotaVechileFees");
                                            strStatus = "0#";
                                        }
                                    }





                                }
                                else
                                {
                                    Session.Remove("AvaliableTicket");
                                    Session.Remove("VFeeTigerProject");
                                    Session.Remove("VFeeSurcharge");
                                    Session.Remove("TotaVechileFees");
                                    strStatus = "0#";
                                }

                                if (placeID == 2 && dtTicketdetails.Rows[0][0].ToString() == "0")
                                {
                                    if (pagetypeSession == "2")
                                    {
                                        //here we check for waiting availibility
                                        dtTicketdetails = await bkt.CheckTicketWaitingAvailability();

                                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                                        Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
                                        Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
                                        Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                                        strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"] + "#" + dtTicketdetails.Rows[0][3].ToString();
                                        Session["ArrivalDateBackup"] = arrivaldate;
                                        Session["ShiftBackup"] = shifttime;
                                    }
                                    else
                                    {
                                        Session.Remove("AvaliableTicket");
                                        Session.Remove("VFeeTigerProject");
                                        Session.Remove("VFeeSurcharge");
                                        Session.Remove("TotaVechileFees");
                                        strStatus = "0#";
                                    }
                                }
                            }
                            else
                            {
                                Session.Remove("AvaliableTicket");
                                Session.Remove("VFeeTigerProject");
                                Session.Remove("VFeeSurcharge");
                                Session.Remove("TotaVechileFees");
                                strStatus = "0#!";
                            }
                            //}
                            //else
                            //{
                            //    Session.Remove("AvaliableTicket");
                            //    Session.Remove("VFeeTigerProject");
                            //    Session.Remove("VFeeSurcharge");
                            //    Session.Remove("TotaVechileFees");
                            //    strStatus = "0#!";  ///shh
                            //}
                        }
                        else
                        {
                            Session.Remove("AvaliableTicket");
                            Session.Remove("VFeeTigerProject");
                            Session.Remove("VFeeSurcharge");
                            Session.Remove("TotaVechileFees");
                            strStatus = "0#!";
                        }
                    }
                    else
                    {
                        Session.Remove("AvaliableTicket");
                        Session.Remove("VFeeTigerProject");
                        Session.Remove("VFeeSurcharge");
                        Session.Remove("TotaVechileFees");
                        strStatus = "0#!";
                    }
                }
                else
                {
                    Session.Remove("AvaliableTicket");
                    Session.Remove("VFeeTigerProject");
                    Session.Remove("VFeeSurcharge");
                    Session.Remove("TotaVechileFees");
                    strStatus = "0#E";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(strStatus);
        }

        //public async Task<JsonResult> CheckTicketAvailability(int placeID, string arrivaldate, string shifttime, int Zone, int vehicleID, string captcha, string authtk)
        //{
        //    //       string captchaResponse = HttpContext.Request.Form["g-recaptcha"];
        //    // string captchaResponse1 = HttpContext.Request.Form["g-Recaptcha-Response"];
        //    // string captchaResponse2 = HttpContext.Request.Form["g-Recaptcha-Response"];
        //    Session["CheckAvailTime"] = DateTime.Now;
        //    Session["AvaliableTicket"] = null;
        //    if (Session["CurrentBookingOrAdvanceBooking"] != null)
        //    {
        //        Session["PlaceIdBackup"] = placeID;
        //        Session["ZoneIdBackup"] = Zone;
        //        Session["ShiftIdBackup"] = shifttime;
        //        Session["VehicleIdBackup"] = vehicleID;
        //        Session["IsCurrentOrAdvanceBackup"] = Session["CurrentBookingOrAdvanceBooking"];
        //    }
        //    else
        //    {
        //        Session["PlaceIdBackup"] = null;
        //        Session["ZoneIdBackup"] = null;
        //        Session["ShiftIdBackup"] = null;
        //        Session["VehicleIdBackup"] = null;
        //        Session["IsCurrentOrAdvanceBackup"] = null;
        //    }

        //    DateTime StartSession = Convert.ToDateTime(Session["CaptchaLoadTimeSession"]);
        //    DateTime EndSession = Convert.ToDateTime(DateTime.Now.ToString());
        //    var diffInSeconds = (EndSession - StartSession).TotalSeconds;

        //    string strStatus = string.Empty;

        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    DataTable dtTicketdetails = new DataTable();
        //    string encString = string.Empty;
        //    string decString = string.Empty;
        //    int byPassCondition = 1; // this variable is create on 06-06-2022 by Mukesh as Per
        //                             // Instructions from forest department to remove the captcha.
        //    try
        //    {
        //        string SessionCaptcha = System.Web.HttpContext.Current.Session["Captcha"].ToString();
        //        if (byPassCondition == 1 || (String.Equals(captcha, SessionCaptcha) && diffInSeconds > 7))
        //        {
        //            if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null && Session["VehicleIdCurrentAdvanceSession"] != null)
        //            {
        //                if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == shifttime.ToString() && Session["VehicleIdCurrentAdvanceSession"].ToString() == vehicleID.ToString())
        //                {
        //                    // Check 1 second difference between place/ arrivaldate / vehicle changes done by shaan 15 / 01 / 2021

        //                    DateTime StartSessionA = Convert.ToDateTime(Session["PlaceIdCurrentAdvanceTimeSession"]);
        //                    DateTime EndSessionB = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
        //                    var diffInSecondsbetweenPlace_ArrivalDate = (EndSessionB - StartSessionA).TotalSeconds;
        //                    DateTime StartSessionC = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
        //                    DateTime EndSessionD = Convert.ToDateTime(Session["VehicleIdCurrentAdvanceTimeSession"]);
        //                    var diffInSecondsbetweenArrivalDate_Vehicle = (EndSessionD - StartSessionC).TotalSeconds;
        //                    //end shaan 15/01/2021
        //                    //if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
        //                    //{

        //                    BookOnTicket bkt = new BookOnTicket();
        //                    bkt.PlaceId = placeID;
        //                    bkt.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
        //                    bkt.ShiftTime = shifttime;
        //                    bkt.ZoneId = Zone;
        //                    bkt.vehicleID = vehicleID;
        //                    Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
        //                    //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false")
        //                    //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
        //                    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //                    {
        //                        bkt.KioskUserId = "0";

        //                    }
        //                    else
        //                    {
        //                        bkt.KioskUserId = Session["KioskUserId"].ToString();
        //                    }

        //                    string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
        //                    bool IsValidRequestForCurrentOrAdvanced = false;
        //                    bool IsStringFill = true;
        //                    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //                    {
        //                        if (placeID == 53 || placeID == 57)
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;

        //                            }
        //                        }

        //                        if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                        if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                        Session["CheckAvailabilityTimeSessionStart"] = null;
        //                        Session["CheckAvailabilityTimeSessionEnd"] = null;
        //                        Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
        //                        Session["NationlitySelectionInvalidSession"] = null;
        //                        Session["strRows"] = null;
        //                        Session["strRows_Time"] = null;
        //                        Session["ArrivalDateBackup"] = null;
        //                        Session["ShiftBackup"] = null;
        //                    }
        //                    else
        //                    {
        //                        // changes by shaan 21-02-2020
        //                        // Below Three line added on 11-04-2022 
        //                        Session["CheckAvailabilityTimeSessionStart"] = null;
        //                        Session["CheckAvailabilityTimeSessionEnd"] = null;
        //                        Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
        //                        // Above Three line added on 11-04-2022 
        //                        if (placeID == 2)
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;

        //                            }
        //                            if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                            if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                            Session["CheckAvailabilityTimeSessionStart"] = null;
        //                            Session["CheckAvailabilityTimeSessionEnd"] = null;
        //                            Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
        //                            Session["NationlitySelectionInvalidSession"] = null;
        //                            Session["strRows"] = null;
        //                            Session["strRows_Time"] = null;
        //                            Session["ArrivalDateBackup"] = arrivaldate;
        //                            Session["ShiftBackup"] = shifttime;
        //                        }
        //                        // end
        //                    }

        //                    if (placeID == 2 || placeID == 68)
        //                    {
        //                        string CurrentToken = Session["CurrentToken"].ToString();
        //                        string CurrentTokenLast = CurrentToken.Split('-').Last();
        //                        if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("V"))
        //                        {
        //                            Session["CurrentToken"] = null;
        //                            encString = GenerateEncToken();
        //                            decString = "CT-" + GetDecToken(encString);
        //                            Session["CurrentToken"] = decString;
        //                            IsStringFill = true;
        //                        }
        //                        else
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                    }
        //                    if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1" && placeID != 53 && placeID != 57)
        //                    {
        //                        if (Session["ServerTime"] != null)
        //                        {
        //                            if (Convert.ToDateTime(Session["ServerTime"].ToString()) > Convert.ToDateTime("07:00 AM") && Convert.ToDateTime(Session["ServerTime"].ToString()) < Convert.ToDateTime("10:00 AM"))
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                    }


        //                    if (IsStringFill)
        //                    {
        //                        //dtTicketdetails = bkt.CheckTicketAvailability();
        //                        dtTicketdetails = await bkt.CheckTicketAvailabilityWityPalaceOfWheel();

        //                        if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
        //                        {
        //                            //if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                            //{
        //                            //    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                            //    Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                            //    Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                            //    Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                            //    strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                            //}
        //                            //else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                            //{
        //                            //    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                            //    strStatus = Session["AvaliableTicket"] + "#";
        //                            //}
        //                            //else
        //                            //{
        //                            //    Session.Remove("AvaliableTicket");
        //                            //    Session.Remove("VFeeTigerProject");
        //                            //    Session.Remove("VFeeSurcharge");
        //                            //    Session.Remove("TotaVechileFees");
        //                            //    strStatus = "0#";
        //                            //}

        //                            if (placeID == 2)
        //                            {
        //                                if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
        //                                {
        //                                    if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                                    {
        //                                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                        Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                                        Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                                        Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                                        strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                                        Session["ArrivalDateBackup"] = arrivaldate;
        //                                        Session["ShiftBackup"] = shifttime;
        //                                    }
        //                                    else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                                    {
        //                                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                        strStatus = Session["AvaliableTicket"] + "#";
        //                                        Session["ArrivalDateBackup"] = arrivaldate;
        //                                        Session["ShiftBackup"] = shifttime;
        //                                    }
        //                                    else
        //                                    {
        //                                        Session.Remove("AvaliableTicket");
        //                                        Session.Remove("VFeeTigerProject");
        //                                        Session.Remove("VFeeSurcharge");
        //                                        Session.Remove("TotaVechileFees");
        //                                        strStatus = "0#";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Session.Remove("AvaliableTicket");
        //                                    Session.Remove("VFeeTigerProject");
        //                                    Session.Remove("VFeeSurcharge");
        //                                    Session.Remove("TotaVechileFees");
        //                                    strStatus = "0#!";//16/01/2021
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                                {
        //                                    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                    Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                                    Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                                    Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                                    strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                                    Session["ArrivalDateBackup"] = arrivaldate;
        //                                    Session["ShiftBackup"] = shifttime;
        //                                }
        //                                else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                                {
        //                                    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                    strStatus = Session["AvaliableTicket"] + "#";
        //                                    Session["ArrivalDateBackup"] = arrivaldate;
        //                                    Session["ShiftBackup"] = shifttime;
        //                                }
        //                                else
        //                                {
        //                                    Session.Remove("AvaliableTicket");
        //                                    Session.Remove("VFeeTigerProject");
        //                                    Session.Remove("VFeeSurcharge");
        //                                    Session.Remove("TotaVechileFees");
        //                                    strStatus = "0#";
        //                                }
        //                            }





        //                        }
        //                        else
        //                        {
        //                            Session.Remove("AvaliableTicket");
        //                            Session.Remove("VFeeTigerProject");
        //                            Session.Remove("VFeeSurcharge");
        //                            Session.Remove("TotaVechileFees");
        //                            strStatus = "0#";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Session.Remove("AvaliableTicket");
        //                        Session.Remove("VFeeTigerProject");
        //                        Session.Remove("VFeeSurcharge");
        //                        Session.Remove("TotaVechileFees");
        //                        strStatus = "0#!";
        //                    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    Session.Remove("AvaliableTicket");
        //                    //    Session.Remove("VFeeTigerProject");
        //                    //    Session.Remove("VFeeSurcharge");
        //                    //    Session.Remove("TotaVechileFees");
        //                    //    strStatus = "0#!";  ///shh
        //                    //}
        //                }
        //                else
        //                {
        //                    Session.Remove("AvaliableTicket");
        //                    Session.Remove("VFeeTigerProject");
        //                    Session.Remove("VFeeSurcharge");
        //                    Session.Remove("TotaVechileFees");
        //                    strStatus = "0#!";
        //                }
        //            }
        //            else
        //            {
        //                Session.Remove("AvaliableTicket");
        //                Session.Remove("VFeeTigerProject");
        //                Session.Remove("VFeeSurcharge");
        //                Session.Remove("TotaVechileFees");
        //                strStatus = "0#!";
        //            }
        //        }
        //        else
        //        {
        //            Session.Remove("AvaliableTicket");
        //            Session.Remove("VFeeTigerProject");
        //            Session.Remove("VFeeSurcharge");
        //            Session.Remove("TotaVechileFees");
        //            strStatus = "0#E";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }

        //    return Json(strStatus);
        //}

        //public async Task<JsonResult> CheckTicketAvailability(int placeID, string arrivaldate, string shifttime, int Zone, int vehicleID, string captcha, string authtk)
        //{
        //    Session["CheckAvailTime"] = DateTime.Now;
        //    // string captchaResponse = HttpContext.Request.Form["g-recaptcha"];
        //    // string captchaResponse1 = HttpContext.Request.Form["g-Recaptcha-Response"];
        //    // string captchaResponse2 = HttpContext.Request.Form["g-Recaptcha-Response"];

        //    DateTime StartSession = Convert.ToDateTime(Session["CaptchaLoadTimeSession"]);
        //    DateTime EndSession = Convert.ToDateTime(DateTime.Now.ToString());
        //    var diffInSeconds = (EndSession - StartSession).TotalSeconds;

        //    string strStatus = string.Empty;

        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    DataTable dtTicketdetails = new DataTable();
        //    string encString = string.Empty;
        //    string decString = string.Empty;
        //    try
        //    {
        //        string SessionCaptcha = System.Web.HttpContext.Current.Session["Captcha"].ToString();
        //        if (String.Equals(captcha, SessionCaptcha) && diffInSeconds > 7)
        //        {
        //            if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null && Session["VehicleIdCurrentAdvanceSession"] != null)
        //            {
        //                if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == shifttime.ToString() && Session["VehicleIdCurrentAdvanceSession"].ToString() == vehicleID.ToString())
        //                {
        //                    // Check 1 second difference between place/ arrivaldate / vehicle changes done by shaan 15 / 01 / 2021

        //                    DateTime StartSessionA = Convert.ToDateTime(Session["PlaceIdCurrentAdvanceTimeSession"]);
        //                    DateTime EndSessionB = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
        //                    var diffInSecondsbetweenPlace_ArrivalDate = (EndSessionB - StartSessionA).TotalSeconds;
        //                    DateTime StartSessionC = Convert.ToDateTime(Session["ArrivalDateCurrentAdvanceTimeSession"]);
        //                    DateTime EndSessionD = Convert.ToDateTime(Session["VehicleIdCurrentAdvanceTimeSession"]);
        //                    var diffInSecondsbetweenArrivalDate_Vehicle = (EndSessionD - StartSessionC).TotalSeconds;
        //                    //end shaan 15/01/2021
        //                    //if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
        //                    //{

        //                    BookOnTicket bkt = new BookOnTicket();
        //                    bkt.PlaceId = placeID;
        //                    bkt.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
        //                    bkt.ShiftTime = shifttime;
        //                    bkt.ZoneId = Zone;
        //                    bkt.vehicleID = vehicleID;
        //                    Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
        //                    //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false")
        //                    //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
        //                    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //                    {
        //                        bkt.KioskUserId = "0";

        //                    }
        //                    else
        //                    {
        //                        bkt.KioskUserId = Session["KioskUserId"].ToString();
        //                    }

        //                    string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
        //                    bool IsValidRequestForCurrentOrAdvanced = false;
        //                    bool IsStringFill = true;
        //                    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //                    {
        //                        if (placeID == 53 || placeID == 57)
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;

        //                            }
        //                        }

        //                        if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                        if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                        Session["CheckAvailabilityTimeSessionStart"] = null;
        //                        Session["CheckAvailabilityTimeSessionEnd"] = null;
        //                        Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
        //                        Session["NationlitySelectionInvalidSession"] = null;
        //                        Session["strRows"] = null;
        //                        Session["strRows_Time"] = null;
        //                    }
        //                    else
        //                    {
        //                        // changes by shaan 21-02-2020
        //                        if (placeID == 2)
        //                        {
        //                            if (arrivaldate == DateTime.Now.ToString("dd/MM/yyyy") || arrivaldate == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //                            {
        //                                IsValidRequestForCurrentOrAdvanced = true;

        //                            }
        //                            if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                            if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                            Session["CheckAvailabilityTimeSessionStart"] = null;
        //                            Session["CheckAvailabilityTimeSessionEnd"] = null;
        //                            Session["CheckAvailabilityTimeSessionStart"] = DateTime.Now.ToString();
        //                            Session["NationlitySelectionInvalidSession"] = null;
        //                            Session["strRows"] = null;
        //                            Session["strRows_Time"] = null;
        //                        }
        //                        // end
        //                    }

        //                    if (placeID == 2 || placeID == 68)
        //                    {
        //                        string CurrentToken = Session["CurrentToken"].ToString();
        //                        string CurrentTokenLast = CurrentToken.Split('-').Last();
        //                        if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("V"))
        //                        {
        //                            Session["CurrentToken"] = null;
        //                            encString = GenerateEncToken();
        //                            decString = "CT-" + GetDecToken(encString);
        //                            Session["CurrentToken"] = decString;
        //                            IsStringFill = true;
        //                        }
        //                        else
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                    }
        //                    if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1")
        //                    {
        //                        if (Session["ServerTime"] != null)
        //                        {
        //                            if (Convert.ToDateTime(Session["ServerTime"].ToString()) > Convert.ToDateTime("07:00 AM") && Convert.ToDateTime(Session["ServerTime"].ToString()) < Convert.ToDateTime("10:00 AM"))
        //                            {
        //                                IsStringFill = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            IsStringFill = false;
        //                        }
        //                    }

        //                    if (IsStringFill)
        //                    {
        //                        //dtTicketdetails = bkt.CheckTicketAvailability();
        //                        dtTicketdetails =  bkt.CheckTicketAvailabilityWityPalaceOfWheel();
        //                        if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
        //                        {
        //                            //if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                            //{
        //                            //    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                            //    Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                            //    Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                            //    Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                            //    strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                            //}
        //                            //else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                            //{
        //                            //    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                            //    strStatus = Session["AvaliableTicket"] + "#";
        //                            //}
        //                            //else
        //                            //{
        //                            //    Session.Remove("AvaliableTicket");
        //                            //    Session.Remove("VFeeTigerProject");
        //                            //    Session.Remove("VFeeSurcharge");
        //                            //    Session.Remove("TotaVechileFees");
        //                            //    strStatus = "0#";
        //                            //}

        //                            if (placeID == 2)
        //                            {
        //                                if (diffInSecondsbetweenPlace_ArrivalDate >= 2 && diffInSecondsbetweenArrivalDate_Vehicle >= 2)
        //                                {
        //                                    if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                                    {
        //                                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                        Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                                        Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                                        Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                                        strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                                    }
        //                                    else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                                    {
        //                                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                        strStatus = Session["AvaliableTicket"] + "#";
        //                                    }
        //                                    else
        //                                    {
        //                                        Session.Remove("AvaliableTicket");
        //                                        Session.Remove("VFeeTigerProject");
        //                                        Session.Remove("VFeeSurcharge");
        //                                        Session.Remove("TotaVechileFees");
        //                                        strStatus = "0#";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Session.Remove("AvaliableTicket");
        //                                    Session.Remove("VFeeTigerProject");
        //                                    Session.Remove("VFeeSurcharge");
        //                                    Session.Remove("TotaVechileFees");
        //                                    strStatus = "0#!";//16/01/2021
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
        //                                {
        //                                    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                    Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
        //                                    Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
        //                                    Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
        //                                    strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"];
        //                                }
        //                                else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
        //                                {
        //                                    Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
        //                                    strStatus = Session["AvaliableTicket"] + "#";
        //                                }
        //                                else
        //                                {
        //                                    Session.Remove("AvaliableTicket");
        //                                    Session.Remove("VFeeTigerProject");
        //                                    Session.Remove("VFeeSurcharge");
        //                                    Session.Remove("TotaVechileFees");
        //                                    strStatus = "0#";
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Session.Remove("AvaliableTicket");
        //                            Session.Remove("VFeeTigerProject");
        //                            Session.Remove("VFeeSurcharge");
        //                            Session.Remove("TotaVechileFees");
        //                            strStatus = "0#";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Session.Remove("AvaliableTicket");
        //                        Session.Remove("VFeeTigerProject");
        //                        Session.Remove("VFeeSurcharge");
        //                        Session.Remove("TotaVechileFees");
        //                        strStatus = "0#!";
        //                    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    Session.Remove("AvaliableTicket");
        //                    //    Session.Remove("VFeeTigerProject");
        //                    //    Session.Remove("VFeeSurcharge");
        //                    //    Session.Remove("TotaVechileFees");
        //                    //    strStatus = "0#!";  ///shh
        //                    //}
        //                }
        //                else
        //                {
        //                    Session.Remove("AvaliableTicket");
        //                    Session.Remove("VFeeTigerProject");
        //                    Session.Remove("VFeeSurcharge");
        //                    Session.Remove("TotaVechileFees");
        //                    strStatus = "0#!";
        //                }
        //            }
        //            else
        //            {
        //                Session.Remove("AvaliableTicket");
        //                Session.Remove("VFeeTigerProject");
        //                Session.Remove("VFeeSurcharge");
        //                Session.Remove("TotaVechileFees");
        //                strStatus = "0#!";
        //            }
        //        }
        //        else
        //        {
        //            Session.Remove("AvaliableTicket");
        //            Session.Remove("VFeeTigerProject");
        //            Session.Remove("VFeeSurcharge");
        //            Session.Remove("TotaVechileFees");
        //            strStatus = "0#E";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }

        //    return Json(strStatus);
        //}

        //captcha added by shaan 15/01/2021

        public JsonResult ShowCaptcha()
        {
            //set session null on captcha refresh
            Session["AvaliableTicket"] = null;
            Session["CheckAvailTime"] = null;
            Session["PlaceIdBackup"] = null;
            Session["ZoneIdBackup"] = null;
            Session["ShiftIdBackup"] = null;
            Session["VehicleIdBackup"] = null;
            Session["IsCurrentOrAdvanceBackup"] = null;
            //set session null on captcha refresh
            //var data = "";
            CaptchaImg obj = new CaptchaImg();
            string captchaString = "";
            string filename = Server.MapPath("~/Content/TempData/") + System.Guid.NewGuid().ToString("N");
            var data = obj.GetJsonCaptcha(filename, 8, out captchaString);
            System.Web.HttpContext.Current.Session["Captcha"] = captchaString;
            var request = System.Web.HttpContext.Current.Request;
            var response = System.Web.HttpContext.Current.Response;
            var newContext = new HttpContext(request, response);
            newContext.Response.ContentType = "image/jpeg";
            Session["CaptchaLoadTimeSession"] = DateTime.Now.ToString();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="placeID"></param>
        /// <param name="Zone"></param>
        /// <returns></returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult vehicleByCategory(int vehicleCatID, Int64 placeID, int Zone, string authtk)
        {            
            BookOnTicket bkt = new BookOnTicket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            object vehiclelist = "";
            string encString = string.Empty;
            string decString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                bkt.PlaceId = placeID;
                bkt.ZoneId = Zone;
                // cst.ShiftType = Shift;
                dt = bkt.Select_vehicle(Convert.ToInt64(vehicleCatID));
                ViewBag.vname = dt;
                foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                {
                    vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                }

                //changes done by shaan 05/11/2020
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    Session["ArrivalDateCurrentAdvanceSession"] = null;
                    Session["ShiftIdCurrentAdvanceSession"] = null;
                    Session["VehicleIdCurrentAdvanceSession"] = null;
                    Session["AvailableTicket"] = null;
                }
                else
                {
                    // changes by shaan 21-02-2020
                    if (placeID == 2 || placeID==68)
                    {
                        Session["ArrivalDateCurrentAdvanceSession"] = null;
                        Session["ShiftIdCurrentAdvanceSession"] = null;
                        Session["VehicleIdCurrentAdvanceSession"] = null;
                        Session["AvailableTicket"] = null;
                    }
                    // end
                }

                if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null)
                {
                    if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString())
                    {
                        Session["ZoneIdCurrentAdvanceSession"] = Zone;
                    }

                }
                //end

                if (placeID == 2 || placeID == 68)
                {
                    string CurrentToken = Session["CurrentToken"].ToString();
                    string CurrentTokenLast = CurrentToken.Split('-').Last();
                    if (CurrentTokenLast.Trim() == GetDecToken(authtk) && CurrentToken.Contains("P") && Session["ZoneCount"] == null)
                    {
                        Session["CurrentToken"] = null;
                        encString = GenerateEncToken();
                        decString = "Z-" + GetDecToken(encString);
                        Session["CurrentToken"] = decString;
                        Session["ZoneCount"] = 1;
                        vehiclelist = new SelectList(vehicle, "Value", "Text");
                    }
                    else if (Session["ZoneCount"] != null)
                    {
                        if (Convert.ToInt32(Session["ZoneCount"]) == 1)
                        {
                            Session["CurrentToken"] = null;
                            encString = GenerateEncToken();
                            decString = "Z-" + GetDecToken(encString);
                            Session["CurrentToken"] = decString;
                            Session["ZoneCount"] = 1;
                            vehiclelist = new SelectList(vehicle, "Value", "Text");
                        }
                        //vehiclelist = null;
                    }
                    else
                    {
                        vehiclelist = null;
                    }
                }
                else
                {
                    vehiclelist = new SelectList(vehicle, "Value", "Text");
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { obj1 = vehiclelist, obj2 = encString });
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult vehicleByCategoryNew(int vehicleCatID, Int64 placeID, int Zone, int shiftId)
        {
            BookOnTicket bkt = new BookOnTicket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            object vehiclelist = "";
            string encString = string.Empty;
            string decString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                bkt.PlaceId = placeID;
                bkt.ZoneId = Zone;
                // cst.ShiftType = Shift;
                dt = bkt.Select_vehicle(Convert.ToInt64(vehicleCatID));
                ViewBag.vname = dt;
                if (shiftId != 0)
                {
                    foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                    {
                        vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                    }
                    vehiclelist = new SelectList(vehicle, "Value", "Text");
                }
                else
                {
                    vehiclelist = null;
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { obj1 = vehiclelist, obj2 = "" });
        }

        /// <summary>
        /// function to get all types of fees for ticket booking on the basis of given param values
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="districtID"></param>
        /// <param name="nationality"></param>
        /// <param name="memberType"></param>
        /// <returns>Json result for fees</returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult SelectFee(Int64 placeID, string nationality, string memberType, int vehicleID, string rowNo, string name, string gender)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();

            try
            {
                string encString = string.Empty;
                string decString = string.Empty;
                if (placeID == 2 || placeID == 68)
                {
                    if (!string.IsNullOrEmpty(name) && gender != "0")
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            if (Convert.ToInt32(rowNo) == i)
                            {
                                if (Session["CurrentToken"].ToString().Contains("CT"))
                                {
                                    Session["CurrentToken"] = null;
                                    encString = GenerateEncToken();
                                    decString = "R" + (i + 1) + "-" + GetDecToken(encString);
                                    Session["CurrentToken"] = decString;
                                }
                                else
                                {
                                    if (Session["CurrentToken"].ToString().Contains("R" + (i)))
                                    {
                                        Session["CurrentToken"] = null;
                                        encString = GenerateEncToken();
                                        decString = "R" + (i + 1) + "-" + GetDecToken(encString);
                                        Session["CurrentToken"] = decString;
                                    }
                                }
                            }
                        }
                    }
                }

                //changes by shaan 16-12-2020
                if (Session["NationalitySelectCount"] == null)
                {
                    Session["NationalitySelectCount"] = "0";
                }
                if (Session["NationalitySelectCount"].ToString() == "0")
                {
                    Session["NationalitySelectCount"] = "1";
                    Session["FirstNationalitySelectTimeSession"] = DateTime.Now.ToString();
                }
                // end changes 16-12-2020

                //changes by shaan 21-12-2020
                if (Session["NationlitySelectionInvalidSession"] == null)
                {
                    Session["NationlitySelectionInvalidSession"] = "0";
                }
                if (Session["NationlitySelectionInvalidSession"].ToString() != "1")
                {
                    string strRow = (Session["strRows"] != null ? Session["strRows"].ToString() : "");
                    string strRowTime = (Session["strRows_Time"] != null ? Session["strRows_Time"].ToString() : "");
                    if (strRow.Contains(rowNo) == false)
                    {
                        if (strRowTime == "")
                        {
                            Session["strRows_Time"] = rowNo + "_" + DateTime.Now.ToString();
                        }
                        else
                        {
                            string[] spl = strRowTime.Split('_');
                            string cRow = spl[0];
                            string cTime = spl[1];
                            //calculate the time diffrence here
                            string utime = DateTime.Now.ToString();
                            DateTime StartSession = Convert.ToDateTime(cTime);
                            DateTime EndSession = Convert.ToDateTime(utime);
                            var diffInSeconds = (EndSession - StartSession).TotalSeconds;
                            if (diffInSeconds <= 3)
                            {
                                Session["NationlitySelectionInvalidSession"] = "1";
                            }
                            //calculate the time diffrence here
                            Session["strRows_Time"] = rowNo + "_" + utime;
                        }

                        Session["strRows"] = strRow + "," + rowNo;

                    }
                }
                // end changes 21-12-2020
                DataTable dt = new DataTable();
                BookOnTicket cst = new BookOnTicket();
                cst.PlaceId = placeID;
                cst.MemberNationality = nationality;
                cst.MemberType = memberType;
                cst.vehicleID = vehicleID;
                dt = cst.SelectMemberFees();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Session["AutomatedScript"] = Convert.ToInt32(dr["IsAutomatedScriptRandomNo"]) == 0 ? "1" : Convert.ToString(dr["AutomatedScriptRandomNo"]);
                        fees.Add(new BookOnTicket()
                        {
                            MemberFees_TigerProject = Convert.ToDecimal(dr["MFees_TigerProject"].ToString()),
                            MemberFees_Surcharge = Convert.ToDecimal(dr["MFees_Surcharge"].ToString()),
                            TRDF = Convert.ToDecimal(dr["TRDF"].ToString()),
                            CameraFees_TigerProject = Convert.ToDecimal(dr["CFees_TigerProject"].ToString()),
                            CameraFees_Surcharge = Convert.ToDecimal(dr["CFees_Surcharge"].ToString()),

                            TotalPerMemberFees = (Convert.ToDecimal(dr["MFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["MFees_Surcharge"].ToString()) + Convert.ToDecimal(dr["TRDF"].ToString()) + Convert.ToDecimal(dr["Vehicle_TRDF"]) + Convert.ToDecimal(dr["GuidFee_TRDF"])),

                            TotalPerMemberCameraFees = (Convert.ToDecimal(dr["CFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["CFees_Surcharge"].ToString())),

                            BoardingVehicleFee = Convert.ToDecimal(dr["BoardingVehicleFee"]),
                            BoardingVehicleFeeGSTPercentage = Convert.ToDecimal(dr["BoardingVehicleFeeGSTPercentage"]),
                            BoardingVehicleFeeGSTAmount = Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]),


                            BoardingGuideFee = Convert.ToDecimal(dr["BoardingGuideFee"]),
                            BoardingGuideFeeGSTPercentage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]),
                            BoardingGuideFeeGSTAmount = Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),

                            TotalBoardingFee = Convert.ToDecimal(dr["BoardingVehicleFee"]) + Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]) + Convert.ToDecimal(dr["BoardingGuideFee"]) + Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),


                            GSTMessage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]) == 0 ? "" : Convert.ToString(dr["BoardingGuideFeeGSTPercentage"]) + " % GST  applicable on guide fees And " + Convert.ToString(dr["BoardingVehicleFeeGSTPercentage"]) + "% applicable on vehicle rent",

                            Vehicle_TRDF = Convert.ToDecimal(dr["Vehicle_TRDF"]),

                            GuidFee_TRDF = Convert.ToDecimal(dr["GuidFee_TRDF"]),
                            AutomatedScript = Convert.ToString(Session["AutomatedScript"])

                        });
                    }
                    int FillingTotal = fees.Count() + Convert.ToInt32(Session["MemberFillingCount"]);
                    Session["MemberFillingCount"] = FillingTotal;

                }
                else
                {
                    fees.Add(new BookOnTicket()
                    {
                        MemberFees_TigerProject = Convert.ToDecimal(0),
                        MemberFees_Surcharge = Convert.ToDecimal(0),
                        TRDF = Convert.ToDecimal(0),
                        CameraFees_TigerProject = Convert.ToDecimal(0),
                        CameraFees_Surcharge = Convert.ToDecimal(0),
                        TotalPerMemberFees = Convert.ToDecimal(0),
                        TotalPerMemberCameraFees = Convert.ToDecimal(0),
                        AutomatedScript = null
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(fees);
        }

        /// <summary>
        /// Function to get accomodation charges on the basis of room type
        /// </summary>
        /// <param name="AccomoID"></param>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <returns> json result return Accomodation charges.</returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult SelectAccomoFee(Int64 AccomoID, Int64 placeID, string arrivaldate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> Accomofees = new List<BookOnTicket>();
            try
            {
                DataTable dt = new DataTable();
                BookOnTicket cst = new BookOnTicket();
                cst.AccomoID = AccomoID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null); ;
                cst.PlaceId = placeID;
                dt = cst.Select_Accomo_Fees_Availability();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Accomofees.Add(new BookOnTicket()
                        {
                            RoomAvailability = Convert.ToInt32(dr["AvailableRoom"].ToString()),
                            RoomCharge = Convert.ToDecimal(dr["RatePerRoom"].ToString()),
                        });
                    }
                }
                else
                {
                    Accomofees.Add(new BookOnTicket()
                    {
                        RoomAvailability = Convert.ToInt32(0),
                        RoomCharge = Convert.ToDecimal(0),
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(Accomofees);
        }

        public JsonResult GetOTP()
        {

            int iSSendOTP = 0;
            Session["RandomOTP"] = null;
            string otp = Globals.Util.GetRandomNumber();
            Session["RandomOTP"] = otp;
            var user = Session["SSODetail"] as UserProfile;
            if (!string.IsNullOrEmpty(Session["RandomOTP"].ToString()))
            {
                iSSendOTP = 1;

                string Message = "Dear applicant,Your wildlife ticket booking OTP is " + otp + " .OTP will expire in 10 minutes.Onexpiry of time, Please regenerate the OTP.";

                string html = "<html><body><p>Dear applicant,Your wildlife ticket booking OTP is " + otp + " .OTP will expire in 10 minutes.On expiry of time, Please regenerate the OTP.</p></body></html>";

                SMS_EMail_Services sMS_EMail_Services = new SMS_EMail_Services();
                DateTime StartDT = DateTime.Now;
                SMS_EMail_Services.sendSingleSMS(user.MobileNumber, Message);
                DateTime CurrentTimer = DateTime.Now;
                //DateTime CurrentTimer = Convert.ToDateTime("10:00:30 AM");
                DateTime t1 = Convert.ToDateTime("10:00:00 AM");
                DateTime t2 = Convert.ToDateTime("10:00:30 AM");
                if (CurrentTimer >= t1 && CurrentTimer <= t2)
                {
                    iSSendOTP = 0;
                    Session.Clear();
                    Session.Abandon();

                }
                if (iSSendOTP == 1)
                {
                    sMS_EMail_Services.sendEMail("Verify OTP for wildlife booking", html, user.EmailId, "");
                    SMS_EMail_Services.sendSingleSMS(user.MobileNumber, Message);
                    sMS_EMail_Services.sendEMail("Verify OTP for wildlife booking", html, user.EmailId, "");
                }
                //SMS_EMail_Services.sendSingleSMS("9314558220", Message);
                //sMS_EMail_Services.sendEMail("Verify OTP for wildlife booking", html, "sanjaysingh.jadon@gmail.com", "");


                return Json(iSSendOTP, JsonRequestBehavior.AllowGet);
            }
            return Json(iSSendOTP, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Function to save ticket details to database with all the values
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="Command"></param>
        /// <param name="form"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        /// 

        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> FinalSubmit(List<MemberInfo> lstMemberInfo, BookOnTicket cs, string Command, FormCollection form, string Message)
        //{
        //    string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
        //    new Common().ErrorLog("FinalSubmit From WEB Zone :" + cs.ZoneId + ",placid" + cs.PlaceId, "FinalSubmit_DOA:" + cs.DateOfArrival + ",shift:" + cs.ShiftTime, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //    //To Stop Crosssing of Advance or Current booking Mukesh Kumar Jangid on 24/11/2021
        //    bool isKioskUser = true;
        //    if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //    {
        //        if (cs.PlaceId != 53 && cs.PlaceId != 57 && Convert.ToInt64(form["ddl_place"].ToString()) != 53 && Convert.ToInt64(form["ddl_place"].ToString()) != 57)
        //            isKioskUser = false;
        //    }

        //    if (Session["ArrivalDateBackup"] != null && Session["ShiftBackup"] != null)
        //    {
        //        if (Session["ArrivalDateBackup"].ToString() != cs.ArrivalDate.ToString("dd/MM/yyyy"))
        //        {
        //            TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //            string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //            string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //            DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //            Session["ArrivalDateBackup"] = null;
        //            Session["ShiftBackup"] = null;
        //            new Common().ErrorLog("FinalSubmit From WEB", "Arrival Date Not Matched", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //            return RedirectToAction("InvalidRequest");
        //        }
        //    }
        //    if (Session["CrossVarification"] != null)
        //    {
        //        if (Session["CrossVarification"].ToString().Equals(Session["LblPlaceName"].ToString()) != true && Session["LblPlaceName"].ToString().ToLower() == "current")
        //        {
        //            TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //            string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //            string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //            DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //            Session["CrossVarification"] = null;
        //            Session["LblPlaceName"] = null;
        //            new Common().ErrorLog("FinalSubmit From WEB", "Current Session Not Matched", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //            return RedirectToAction("InvalidRequest");
        //            //return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //        }
        //    }
        //    ////// Check Wait Time From Client Side Added By Mukesh Jangid On 20/11/2021
        //    ////if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1 && Session["WTCl"] != null)
        //    ////{
        //    ////    var waittime = (int)Session["WTCl"];

        //    ////    var diffInSeconds1 = (DateTime.Now - Convert.ToDateTime(Session["WTTime"].ToString())).TotalSeconds;
        //    ////    if (waittime > 0)
        //    ////    {
        //    ////        waittime = waittime / 1000;
        //    ////        if (diffInSeconds1 - waittime < 0)
        //    ////        {
        //    ////            TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!!!";
        //    ////            string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //    ////            string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //    ////            DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //    ////            new Common().ErrorLog("FinalSubmit From WEB", "Waiting time not stayed", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //    ////            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //    ////        }
        //    ////    }
        //    ////}
        //    bool ErrorMessageFlag = true;

        //    if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") || cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //    {
        //        if (pagetypeSession == "2" && Convert.ToInt64(form["ddl_place"].ToString()) == 2)
        //        {
        //            new Common().ErrorLog("FinalSubmit From WEB", "Ranthamboer booking not allowed in Advance on current booking like", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //            return RedirectToAction("InvalidRequest");
        //        }
        //        if (isKioskUser == false || true) // this ( || true ) change made on 11-04-2022
        //        {
        //            string CheckavailabiltyStartTime = Session["CheckAvailabilityTimeSessionStart"].ToString();
        //            string FirstNationalitySelectTime = Session["FirstNationalitySelectTimeSession"].ToString();
        //            if (!string.IsNullOrEmpty(CheckavailabiltyStartTime) && !string.IsNullOrEmpty(FirstNationalitySelectTime))
        //            {
        //                DateTime StartSession1 = Convert.ToDateTime(CheckavailabiltyStartTime);
        //                DateTime EndSession1 = Convert.ToDateTime(FirstNationalitySelectTime);
        //                var diffInSeconds1 = (EndSession1 - StartSession1).TotalSeconds;
        //                if (diffInSeconds1 <= 4)
        //                {
        //                    new Common().ErrorLog("FinalSubmit From WEB", "Nationality row time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //                    return RedirectToAction("InvalidRequest");
        //                }
        //            }
        //        }

        //    }
        //    if (Session["AvaliableTicket"] == null)
        //    {
        //        new Common().ErrorLog("FinalSubmit From WEB", "Check availability not done", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //        return RedirectToAction("InvalidRequest");
        //    }
        //    if (Convert.ToUInt32(Session["AvaliableTicket"]) == 0)
        //    {
        //        new Common().ErrorLog("FinalSubmit From WEB", "Check availability not done", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //        return RedirectToAction("InvalidRequest");
        //    }
        //    if (Session["NationlitySelectionInvalidSession"].ToString() == "1" && isKioskUser == false)
        //    {
        //        new Common().ErrorLog("FinalSubmit From WEB", "Nationality Time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //        return RedirectToAction("InvalidRequest");
        //    }

        //    string MemberFillSession = "";
        //    DateTime StartSession;
        //    DateTime EndSession;
        //    double diffInSeconds = 0.0;
        //    UInt32 ectualSeconds = 0;
        //    UInt32 cnt = 0;
        //    bool isIndianExists = false;
        //    if (isKioskUser == false || true) // this ( || true ) change made on 11-04-2022
        //    {
        //        foreach (var item in lstMemberInfo)  //----
        //        {
        //            if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
        //            {
        //                cnt++;
        //                if (item.MemberNationality == "1")
        //                    isIndianExists = true;
        //            }
        //        }
        //        MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
        //        Session["CheckAvailabilityTimeSessionEnd"] = DateTime.Now.ToString();
        //        StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
        //        EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
        //        diffInSeconds = (EndSession - StartSession).TotalSeconds;
        //        //ectualSeconds = Convert.ToUInt32(MemberFillSession) * 8;
        //        ectualSeconds = Convert.ToUInt32(cnt) * 8;
        //        if (ectualSeconds > diffInSeconds || diffInSeconds > 660)
        //        {
        //            new Common().ErrorLog("FinalSubmit From WEB", "Member Detail total Time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
        //            return RedirectToAction("InvalidRequest");
        //        }
        //    }

        //    if (isIndianExists == true)
        //    {

        //        long UserId = Convert.ToInt64(Session["UserID"].ToString());
        //        DataTable vdt = await cs.ValidateSSOMobile(UserId);
        //        if (vdt.Rows.Count > 0)
        //        {
        //            if (Convert.ToInt16(vdt.Rows[0]["intStatus"]) == 0)
        //            {
        //                TempData["RowCheck"] = vdt.Rows[0]["Msg"];
        //                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //            }
        //        }
        //        else
        //        {
        //            TempData["RowCheck"] = "Kindly Update Your Valid Mobile Number on SSO Portal for booking !";
        //            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //        }
        //    }
        //    if (Session["PlaceIdBackup"] == null || Session["ZoneIdBackup"] == null || Session["ShiftIdBackup"] == null || Session["IsCurrentOrAdvanceBackup"] == null || Session["VehicleIdBackup"] == null)
        //    {
        //        if (Session["PlaceIdBackup"] == null)
        //            new Common().ErrorLog("FinalSubmit From WEB", "Place id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //        if (Session["ZoneIdBackup"] == null)
        //            new Common().ErrorLog("FinalSubmit From WEB", "Zone id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //        if (Session["ShiftIdBackup"] == null)
        //            new Common().ErrorLog("FinalSubmit From WEB", "Shift id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //        if (Session["IsCurrentOrAdvanceBackup"] == null)
        //            new Common().ErrorLog("FinalSubmit From WEB", "Current or Advance Session not found", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //        if (Session["VehicleIdBackup"] == null)
        //            new Common().ErrorLog("FinalSubmit From WEB", "Vehicle id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));


        //        return RedirectToAction("InvalidRequest");
        //    }
        //    else
        //    {
        //        if (Convert.ToInt64(form["ddl_place"].ToString()) != Convert.ToInt64(Session["PlaceIdBackup"]) || Convert.ToInt64(form["ddl_Zone"].ToString()) != Convert.ToInt64(Session["ZoneIdBackup"]) || Convert.ToInt16(form["ddl_Shift"].ToString()) != Convert.ToInt16(Session["ShiftIdBackup"].ToString()) || Session["IsCurrentOrAdvanceBackup"].ToString() != Session["CurrentBookingOrAdvanceBooking"].ToString() || Convert.ToInt32(form["ddl_vehicle"].ToString()) != Convert.ToInt32(Session["VehicleIdBackup"].ToString()))
        //        {
        //            if (Convert.ToInt64(form["ddl_place"].ToString()) != Convert.ToInt64(Session["PlaceIdBackup"]))
        //                new Common().ErrorLog("FinalSubmit From WEB", "Place id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //            if (Convert.ToInt64(form["ddl_Zone"].ToString()) != Convert.ToInt64(Session["ZoneIdBackup"]))
        //                new Common().ErrorLog("FinalSubmit From WEB", "Zone id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //            if (Convert.ToInt16(form["ddl_Shift"].ToString()) != Convert.ToInt16(Session["ShiftIdBackup"].ToString()))
        //                new Common().ErrorLog("FinalSubmit From WEB", "Shift id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //            if (Session["IsCurrentOrAdvanceBackup"].ToString() != Session["CurrentBookingOrAdvanceBooking"].ToString())
        //                new Common().ErrorLog("FinalSubmit From WEB", "Current Session Or Advance Session trying to crossing each other (third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

        //            if (Convert.ToInt32(form["ddl_vehicle"].ToString()) != Convert.ToInt32(Session["VehicleIdBackup"].ToString()))
        //                new Common().ErrorLog("FinalSubmit From WEB", "Vehile id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));


        //            return RedirectToAction("InvalidRequest");
        //        }
        //    }

        //    // above code To Stop Crosssing of Advance or Current booking Mukesh Kumar Jangid on 24/11/2021


        //    // Above Section of Code to Check Wait Time From Client Side Added By Mukesh Jangid On 20/11/2021

        //    #region Check Captcha Validation by Rajveer

        //    if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
        //    {
        //        ErrorMessageFlag = false;
        //        TempData["ErrorMessage"] = "Invalid captcha";

        //        string result = string.Empty;
        //        string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //        if (!string.IsNullOrEmpty(ip))
        //        {
        //            string[] ipRange = ip.Split(',');
        //            int le = ipRange.Length - 1;
        //            result = ipRange[0];
        //        }
        //        else
        //        {
        //            result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        }
        //        await cs.InvalidCaptchaLog("InvalidCaptcha", Convert.ToString(Session["SSOID"]), result);
        //        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //    }
        //    #endregion

        //    else if (ErrorMessageFlag && cs.hdn_IAgreement == "1")
        //    //if (true)
        //    {
        //        //Changes by shaan 02/11/2020
        //        //string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
        //        pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
        //        string AvaliableTicketSession = Session["AvaliableTicket"] as string;
        //        //string MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
        //        MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
        //        bool IsValidRequestForCurrentOrAdvanced = false;
        //        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") || true) // this ( || true ) change made on 11-04-2022
        //        {
        //            if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
        //            {
        //                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null) //--
        //                {
        //                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
        //                }
        //                if (cs.PlaceId == 53 || cs.PlaceId == 57)
        //                {
        //                    if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
        //                    {
        //                        IsValidRequestForCurrentOrAdvanced = true;
        //                    }
        //                }
        //                else
        //                {
        //                    if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") || cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
        //                    {
        //                        IsValidRequestForCurrentOrAdvanced = true;
        //                    }
        //                }

        //                if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }
        //                if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }
        //                if (!string.IsNullOrEmpty(MemberFillSession))
        //                {

        //                    Session["CheckAvailabilityTimeSessionEnd"] = DateTime.Now.ToString();
        //                    //DateTime StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
        //                    //DateTime EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
        //                    StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
        //                    EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
        //                    //var diffInSeconds = (EndSession - StartSession).TotalSeconds;
        //                    //var ectualSeconds = Convert.ToInt32(MemberFillSession) * 7;
        //                    diffInSeconds = (EndSession - StartSession).TotalSeconds;
        //                    ectualSeconds = Convert.ToUInt32(MemberFillSession) * 7;
        //                    if (ectualSeconds > diffInSeconds || diffInSeconds > 660)
        //                    {
        //                        return RedirectToAction("InvalidRequest");
        //                    }
        //                }
        //                else
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }

        //                string CheckavailabiltyStartTime = Session["CheckAvailabilityTimeSessionStart"].ToString();
        //                string FirstNationalitySelectTime = Session["FirstNationalitySelectTimeSession"].ToString();
        //                if (!string.IsNullOrEmpty(CheckavailabiltyStartTime) && !string.IsNullOrEmpty(FirstNationalitySelectTime))
        //                {
        //                    //DateTime StartSession = Convert.ToDateTime(CheckavailabiltyStartTime);
        //                    //DateTime EndSession = Convert.ToDateTime(FirstNationalitySelectTime);
        //                    StartSession = Convert.ToDateTime(CheckavailabiltyStartTime);
        //                    EndSession = Convert.ToDateTime(FirstNationalitySelectTime);
        //                    //var diffInSeconds = (EndSession - StartSession).TotalSeconds;
        //                    diffInSeconds = (EndSession - StartSession).TotalSeconds;
        //                    if (diffInSeconds <= 3)
        //                    {
        //                        return RedirectToAction("InvalidRequest");
        //                    }
        //                }

        //                if (Session["NationlitySelectionInvalidSession"].ToString() == "1")
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }
        //            }  //---

        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
        //            {
        //                if (Convert.ToInt64(form["ddl_place"].ToString()) == 2 || Convert.ToInt64(form["ddl_place"].ToString()) == 68)
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }
        //            }  //---

        //        }
        //        //end chnages
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //        int rownumber = 0;
        //        int rowcount = 0;
        //        decimal finalAmount = 0;
        //        try
        //        {

        //            foreach (var item in lstMemberInfo)  //----
        //            {
        //                if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
        //                {
        //                    rownumber = Convert.ToInt16(item.MemberSLNo);
        //                    rowcount++;
        //                }
        //            }  //---
        //            if (rownumber != rowcount)
        //            {
        //                TempData["RowCheck"] = "Enter member details continiously";
        //                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //            }

        //            if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
        //            {
        //                var duplicatememberIds = lstMemberInfo.GroupBy(xy => new { xy.MemberIdType, xy.MemberIdNo }).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
        //                if (duplicatememberIds.Count() > 1)
        //                {
        //                    return RedirectToAction("InvalidRequest");
        //                }

        //                if (Convert.ToInt64(form["ddl_place"].ToString()) == 2 || Convert.ToInt64(form["ddl_place"].ToString()) == 68)
        //                {
        //                    if (Session["CurrentToken"] != null)
        //                    {
        //                        if (!Session["CurrentToken"].ToString().Contains("R" + rowcount.ToString()))
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return RedirectToAction("InvalidRequest");
        //                    }
        //                }
        //            }  //---


        //            #region Added by shaan to check arrival date is not equal to sunday ---29-06-2021
        //            //Added by shaan to check arrival date is not equal to sunday 29-06-2021
        //            //DateTime date = Convert.ToDateTime(cs.ArrivalDate);
        //            //DayOfWeek day = date.DayOfWeek;
        //            //string dayToday = day.ToString();
        //            //if (day == DayOfWeek.Sunday)
        //            //{
        //            //    return RedirectToAction("InvalidRequest");
        //            //}
        //            //string SelectedVehicle = form["hdnVType"].ToString();
        //            //if (cs.PlaceId == 68 && SelectedVehicle.ToLower() == "gypsy" && rowcount > 4)
        //            //{
        //            //    return RedirectToAction("InvalidRequest");
        //            //}
        //            //else if (cs.PlaceId == 68 && SelectedVehicle.ToLower() == "canter" && rowcount > 10)
        //            //{
        //            //    return RedirectToAction("InvalidRequest");
        //            //}

        //            //end 29-06-2021
        //            #endregion
        //            if (Convert.ToString(form["AutomatedScript"]) == null || Convert.ToString(form["AutomatedScript"]) != Convert.ToString(Session["AutomatedScript"]))
        //            {
        //                TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //                string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //                string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //                DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //            }
        //            var diffInSeconds1 = (DateTime.Now - Convert.ToDateTime(Session["CheckAvailTime"].ToString())).TotalSeconds;

        //            int diffMinuts = (diffInSeconds1 > 0 ? (int)diffInSeconds1 / 60 : 0);
        //            int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["CheckAvailTime"].ToString());
        //            int byPassCondition = 1; // this variable is create on 06-06-2022 by Mukesh as Per
        //                                     // Instructions from forest department to remove the captcha.
        //            if (Command == "Submit" && (AvaliableTicketSession != null && AvaliableTicketSession != "") && (diffMinuts < timeout || byPassCondition == 1))
        //            {
        //                #region MemberInfo
        //                DataTable dtMemberInfo = new DataTable();
        //                dtMemberInfo = MemberInformation(lstMemberInfo);
        //                if (dtMemberInfo.Rows.Count == 0)
        //                {
        //                    TempData["RowCheck"] = "Enter member details";
        //                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                }
        //                else
        //                {
        //                    if (dtMemberInfo.Rows.Count > 0)
        //                    {
        //                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
        //                        {
        //                            finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
        //                        }
        //                    }
        //                }

        //                #endregion

        //                #region Submission

        //                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
        //                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
        //                {
        //                    cs.KioskUserId = "0";

        //                }
        //                else
        //                {
        //                    cs.KioskUserId = Session["KioskUserId"].ToString();
        //                }
        //                cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
        //                Session["RequestId"] = RequestId();
        //                cs.RequestId = Session["RequestId"].ToString();
        //                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
        //                {
        //                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
        //                }
        //                else
        //                {
        //                    cs.PlaceId = 0;
        //                }
        //                if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
        //                {
        //                    cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
        //                }
        //                else
        //                {
        //                    cs.ZoneId = 0;
        //                }
        //                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
        //                {
        //                    cs.ShiftTime = form["ddl_Shift"].ToString();
        //                }
        //                else
        //                {
        //                    cs.ShiftTime = "";
        //                }
        //                cs.TotalMember = Convert.ToInt32(rowcount);

        //                if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
        //                {

        //                    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

        //                    #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
        //                    //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
        //                    //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
        //                    //{
        //                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
        //                    //}
        //                    //else
        //                    //{
        //                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
        //                    //}

        //                    #endregion

        //                    #region Check Current Shift In Current Booking Restrict Inspect element Data Value Hard coded
        //                    if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1 && cs.KioskUserId == "0")
        //                    {
        //                        if (Session["CheckShiftwiseValue"] != null)
        //                        {
        //                            List<BookOnTicket> CheckShift = (List<BookOnTicket>)Session["CheckShiftwiseValue"];
        //                            if (CheckShift != null && CheckShift.FirstOrDefault().DateOfArrival == cs.ArrivalDate.ToString("dd/MM/yyyy"))
        //                            {
        //                                if (CheckShift.FirstOrDefault().isMorning == "True" && cs.ShiftTime == "1")
        //                                {

        //                                }
        //                                else if (CheckShift.FirstOrDefault().isEvening == "True" && cs.ShiftTime == "2")
        //                                {

        //                                }
        //                                else
        //                                {
        //                                    TempData["datevalidation"] = "This shift not available on this date.";
        //                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                                }
        //                            }

        //                        }

        //                    }

        //                    #endregion

        //                    DataTable DTCheckBooking = new DataTable();
        //                    #region Restrict Months
        //                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

        //                    if (DTCheckBooking.Rows.Count > 0)
        //                    {
        //                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
        //                        {
        //                            TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }

        //                    }
        //                    #endregion

        //                    #region Get Open Days by rajveer


        //                    DataTable GetDaysDataTable = new DataTable();
        //                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
        //                    long AddDaysVal = 0;
        //                    if (GetDaysDataTable.Rows.Count > 0)
        //                    {
        //                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
        //                    }
        //                    #endregion
        //                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
        //                    if (cs.ArrivalDate > expiryDate)
        //                    {
        //                        TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }

        //                    //Kiosk User Restrictation
        //                    if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
        //                    {
        //                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
        //                        TempData["datevalidation"] = "Arrival date should not be more than Today";
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }
        //                }
        //                if (TempData["DurationTo"] != null)
        //                {

        //                    DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
        //                    if (cs.ArrivalDate > MaxDate)
        //                    {
        //                        TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }
        //                }
        //                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
        //                {
        //                    cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
        //                    Session["VehicleId"] = cs.vehicleID;
        //                }
        //                else
        //                {
        //                    cs.vehicleID = 0;
        //                }
        //                if (Session["VFeeTigerProject"] != null)
        //                {

        //                    cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
        //                }
        //                else
        //                {
        //                    cs.VehicleFees_TigerProject = 0;
        //                }
        //                if (Session["VFeeSurcharge"] != null)
        //                {

        //                    cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
        //                }
        //                else
        //                {
        //                    cs.VehicleFees_Surcharge = 0;
        //                }
        //                if (Session["TotaVechileFees"] != null)
        //                {

        //                    cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
        //                }
        //                else
        //                {
        //                    cs.VehicleFees_Total = 0;
        //                }
        //                if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
        //                {
        //                    cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
        //                }
        //                else
        //                { cs.AccomoID = 0; }
        //                if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
        //                {
        //                    cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
        //                }
        //                else
        //                { cs.TotalRoom = 0; }
        //                if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
        //                {
        //                    cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
        //                }
        //                else
        //                { cs.RoomCharge = 0; }

        //                string result = string.Empty;
        //                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //                if (!string.IsNullOrEmpty(ip))
        //                {
        //                    string[] ipRange = ip.Split(',');
        //                    int le = ipRange.Length - 1;
        //                    result = ipRange[0];
        //                }
        //                else
        //                {
        //                    result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //                }
        //                cs.IPAddress = result;

        //                //Changes by shaan 03/11/2020
        //                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") || true) // this ( || true ) change made on 11-04-2022
        //                {
        //                    if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
        //                    {
        //                        if (Session["PlaceIdCurrentAdvanceSession"] != null)
        //                        {
        //                            int[] places = { 2, 53, 57 };
        //                            string pageType = Session["CurrentBookingOrAdvanceBooking"].ToString();
        //                            int placeIdSession = Convert.ToInt32(Session["PlaceIdCurrentAdvanceSession"]);
        //                            if (pageType == "1" && placeIdSession != cs.PlaceId)
        //                            {
        //                                return RedirectToAction("InvalidRequest");
        //                            }
        //                            if (pageType == "2" && !places.Contains(placeIdSession) == false && placeIdSession != cs.PlaceId)
        //                            {
        //                                return RedirectToAction("InvalidRequest");
        //                            }


        //                        }
        //                        else
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }


        //                        if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null && Session["VehicleIdCurrentAdvanceSession"] != null && AvaliableTicketSession != null)
        //                        {
        //                            if (Session["PlaceIdCurrentAdvanceSession"].ToString() == cs.PlaceId.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == cs.ZoneId.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == cs.ShiftTime.ToString() && Session["VehicleIdCurrentAdvanceSession"].ToString() == cs.vehicleID.ToString())
        //                            {

        //                            }
        //                            else
        //                            {
        //                                return RedirectToAction("InvalidRequest");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }

        //                        //end chnages
        //                        //Changes by shaan 02/11/2020
        //                        if (string.IsNullOrEmpty(AvaliableTicketSession))
        //                        {
        //                            return RedirectToAction("InvalidRequest");

        //                        }
        //                        if (cs.TotalMember > Convert.ToUInt32(AvaliableTicketSession))
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }
        //                        if (string.IsNullOrEmpty(MemberFillSession))
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }
        //                        if (cs.TotalMember > Convert.ToInt32(MemberFillSession))
        //                        {
        //                            return RedirectToAction("InvalidRequest");
        //                        }
        //                    } //--
        //                }
        //                //end changes




        //                DataTable dts = new DataTable();

        //                DataTable dtcheckTicket = new DataTable();
        //                string strcheckTicket = string.Empty;
        //                //dtcheckTicket = cs.CheckTicketAvailability();
        //                dtcheckTicket = await cs.CheckTicketAvailabilityWityPalaceOfWheel();

        //                Session["BookingUserDetails"] = cs;
        //                strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
        //                if (Convert.ToInt32(strcheckTicket) - cs.TotalMember >= 0 && cs.TotalMember > 0)
        //                {
        //                    if (form["hdnVType"].ToString() == "Gypsy" && cs.TotalMember > 6)
        //                    {
        //                        TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //                        string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //                        string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //                        DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }
        //                    else if (form["hdnVType"].ToString() == "Canter" && cs.TotalMember > 20)
        //                    {
        //                        TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //                        string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
        //                        string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
        //                        DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }

        //                    //shaan chnage 06/11/2020
        //                    Session["ZoneIdCurrentAdvanceSession"] = null;
        //                    Session["ArrivalDateCurrentAdvanceSession"] = null;
        //                    Session["ShiftIdCurrentAdvanceSession"] = null;
        //                    Session["VehicleIdCurrentAdvanceSession"] = null;
        //                    //end changes

        //                    //if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1") ////Change done by Amit on 26-10-2020 for OTP change for Kiosk User Sariska
        //                    if ((Session["CurrentBookingOrAdvanceBooking"].ToString() == "1") && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false"))
        //                    {
        //                        DataTable dtOTPISShow = cs.IsOTPShow();
        //                        string IsShowOTP = dtOTPISShow.Rows[0][0].ToString();
        //                        if (IsShowOTP == "1")
        //                        {
        //                            if (!string.IsNullOrEmpty(HttpContext.Session["RandomOTP"].ToString()))
        //                            {
        //                                string userOtp = Request.Form["txt_UserOtp"].ToString();
        //                                string rndOtp = Session["RandomOTP"].ToString();
        //                                if (rndOtp == userOtp)
        //                                {
        //                                    //New code 25-02-2021
        //                                    //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
        //                                    //bool flg = true;

        //                                    //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
        //                                    //{
        //                                    //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
        //                                    //    {

        //                                    //        ///Execute here
        //                                    //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                                    //        dtMemberInfo.AcceptChanges();
        //                                    //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
        //                                    //        flg = false;
        //                                    //        GloblaUserModel.EnteredUserList.RemoveAt(0);
        //                                    //    }
        //                                    //}
        //                                    //New code 25-02-2021 End

        //                                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                                    dtMemberInfo.AcceptChanges();
        //                                    dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
        //                                }
        //                                else
        //                                {
        //                                    TempData["RowCheck"] = "Invalid OTP";
        //                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                TempData["RowCheck"] = "OTP session time out!";
        //                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //New code 25-02-2021
        //                            //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
        //                            //bool flg = true;

        //                            //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
        //                            //{
        //                            //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
        //                            //    {
        //                            //        ///Execute here
        //                            //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                            //        dtMemberInfo.AcceptChanges();
        //                            //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
        //                            //        flg = false;
        //                            //        GloblaUserModel.EnteredUserList.RemoveAt(0);
        //                            //    }
        //                            //}
        //                            //New code 25-02-2021 End

        //                            dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                            dtMemberInfo.AcceptChanges();
        //                            dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //New code 25-02-2021
        //                        //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
        //                        //bool flg = true;

        //                        //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
        //                        //{
        //                        //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
        //                        //    {
        //                        //        ///Execute here
        //                        //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                        //        dtMemberInfo.AcceptChanges();
        //                        //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
        //                        //        flg = false;
        //                        //        GloblaUserModel.EnteredUserList.RemoveAt(0);
        //                        //    }
        //                        //}
        //                        //New code 25-02-2021 End

        //                        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
        //                        dtMemberInfo.AcceptChanges();
        //                        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);




        //                    }
        //                }         //--
        //                else
        //                {
        //                    TempData["RowCheck"] = "Ticket not avaliable";
        //                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                }
        //                //if (dts.Rows.Count > 0)
        //                //{
        //                //    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
        //                //    Session["totalprice"] = finalAmnt;
        //                //    if (Session["totalprice"].ToString() == "0")
        //                //    {
        //                //        //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
        //                //        TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
        //                //        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
        //                //    TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
        //                //    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                //}
        //                string TicketId = "";
        //                if (dts.Rows.Count > 0)
        //                {
        //                    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());


        //                    Session["RequestId"] = Convert.ToString(dts.Rows[0]["Transaction_Id"]);////SET New request ID in Session by Amit On 01-03-2021
        //                    cs.RequestId = Convert.ToString(Session["RequestId"]);
        //                    ////END Change on 01-03-2021


        //                    string StatusType = dts.Rows[0]["StatusType"].ToString();
        //                    TicketId = dts.Rows[0]["TicketId"].ToString();
        //                    Session["totalprice"] = finalAmnt;
        //                    if (Session["totalprice"].ToString() == "0")
        //                    {
        //                        if (StatusType == "001")
        //                        {
        //                            TempData["datevalidation"] = "Invalid request due unavailability in Database!";
        //                            //TempData["datevalidation"] = "At this moment Ticket booking requests received are more than available tickets, please try again!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                        else if (StatusType == "002")
        //                        {
        //                            TempData["datevalidation"] = "You have already booked tickets for the combination of the same date, same place,same Zone,same Shift, and same IP!";
        //                            //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                        else if (StatusType == "003")
        //                        {
        //                            TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later!!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                        else if (StatusType == "004")
        //                        {
        //                            TempData["datevalidation"] = "Ticket not available.";
        //                            // TempData["datevalidation"] = "The booking requests  received are more than the available inventory at this moment, please try again!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                        else if (StatusType == "005")
        //                        {
        //                            TempData["datevalidation"] = "Your IP has been blocked, Please contact to administrator!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                        else
        //                        {
        //                            TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later.";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
        //                    //TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
        //                    //return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");

        //                    TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later!";
        //                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                }


        //                #region Add Kiosk User by Rajveer
        //                EducationTours edu = new EducationTours();
        //                edu.Location = Convert.ToInt64(cs.PlaceId);
        //                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

        //                if (dt.Rows.Count > 0)
        //                {
        //                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
        //                    {
        //                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
        //                    }
        //                }

        //                // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
        //                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
        //                {
        //                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
        //                    KioskPaymentDetails _obj = new KioskPaymentDetails();
        //                    _obj.ModuleId = 1;
        //                    _obj.ServiceTypeId = 1;
        //                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
        //                    _obj.SubPermissionId = 1;



        //                    //_obj.RequestedId = Convert.ToString(cs.RequestId);  ///Commented by Amit on 01-03-2021 for get New RequestId
        //                    _obj.RequestedId = Convert.ToString(dts.Rows[0]["Transaction_Id"]); ///New RequestId Added by Amit on 01-03-2021


        //                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);
        //                    if (dtKiosk.Rows.Count > 0)
        //                    {
        //                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
        //                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
        //                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
        //                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
        //                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
        //                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
        //                        ViewBag.ViewModel = dts.AsEnumerable();
        //                        return PartialView("KioskPaymentDetailWildlife", _obj);
        //                    }
        //                }
        //                else
        //                {
        //                    //ViewData.Model = dts.AsEnumerable();
        //                    //Session["BookingDatamodal"] = dts.AsEnumerable();
        //                    //return View("OnlineTicketPayment");
        //                    if (TicketId.Trim() != "" && TicketId.Length > 0 && TicketId != null)
        //                    {
        //                        if (Convert.ToInt64(TicketId) > 0)
        //                        {
        //                            ViewData.Model = dts.AsEnumerable();
        //                            Session["BookingDatamodal"] = dts.AsEnumerable();
        //                            Session["CrossVarification"] = null;
        //                            Session["LblPlaceName"] = null;
        //                            return View("OnlineTicketPayment");
        //                        }
        //                        else
        //                        {
        //                            TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later !!!!";
        //                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                        }

        //                    }
        //                    else
        //                    {
        //                        TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later !!!";
        //                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        //                    }
        //                }
        //                #endregion


        //                // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
        //                #endregion
        //            }
        //            else
        //            {
        //                TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
        //                new Common().ErrorLog("Command=" + Command + ",AvaliableTicketSession=" + AvaliableTicketSession + ",diffMinuts=" + diffMinuts + ",timeout=" + timeout + "", actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["RowCheck"] = ex.Message;
        //            new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //        }

        //    }
        //    return RedirectToAction("BookOnlineTicket");
        //}
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FinalSubmit(List<MemberInfo> lstMemberInfo, BookOnTicket cs, string Command, FormCollection form, string Message)
        {
            string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
            new Common().ErrorLog("FinalSubmit From WEB Zone :" + cs.ZoneId + ",placid" + cs.PlaceId, "FinalSubmit_DOA:" + cs.DateOfArrival + ",shift:" + cs.ShiftTime, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

            //To Stop Crosssing of Advance or Current booking Mukesh Kumar Jangid on 24/11/2021
            bool isKioskUser = true;
            if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
            {
                if (cs.PlaceId != 53 && cs.PlaceId != 57 && Convert.ToInt64(form["ddl_place"].ToString()) != 53 && Convert.ToInt64(form["ddl_place"].ToString()) != 57)
                    isKioskUser = false;
            }

            if (Session["ArrivalDateBackup"] != null && Session["ShiftBackup"] != null)
            {
                if (Session["ArrivalDateBackup"].ToString() != cs.ArrivalDate.ToString("dd/MM/yyyy"))
                {
                    TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                    string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
                    string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
                    DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
                    Session["ArrivalDateBackup"] = null;
                    Session["ShiftBackup"] = null;
                    new Common().ErrorLog("FinalSubmit From WEB", "Arrival Date Not Matched", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                    return RedirectToAction("InvalidRequest");
                }
            }
            if (Session["CrossVarification"] != null)
            {
                if (Session["CrossVarification"].ToString().Equals(Session["LblPlaceName"].ToString()) != true && Session["LblPlaceName"].ToString().ToLower() == "current")
                {
                    TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                    string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
                    string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
                    DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
                    Session["CrossVarification"] = null;
                    Session["LblPlaceName"] = null;
                    new Common().ErrorLog("FinalSubmit From WEB", "Current Session Not Matched", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                    return RedirectToAction("InvalidRequest");
                    //return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                }
            }
            ////// Check Wait Time From Client Side Added By Mukesh Jangid On 20/11/2021
            ////if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1 && Session["WTCl"] != null)
            ////{
            ////    var waittime = (int)Session["WTCl"];

            ////    var diffInSeconds1 = (DateTime.Now - Convert.ToDateTime(Session["WTTime"].ToString())).TotalSeconds;
            ////    if (waittime > 0)
            ////    {
            ////        waittime = waittime / 1000;
            ////        if (diffInSeconds1 - waittime < 0)
            ////        {
            ////            TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!!!";
            ////            string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
            ////            string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
            ////            DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
            ////            new Common().ErrorLog("FinalSubmit From WEB", "Waiting time not stayed", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            ////            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
            ////        }
            ////    }
            ////}
            bool ErrorMessageFlag = true;

            if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") || cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
            {
                if (pagetypeSession == "2" && Convert.ToInt64(form["ddl_place"].ToString()) == 2)
                {
                    new Common().ErrorLog("FinalSubmit From WEB", "Ranthamboer booking not allowed in Advance on current booking like", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                    return RedirectToAction("InvalidRequest");
                }
                if (isKioskUser == false || true) // this ( || true ) change made on 11-04-2022
                {
                    string CheckavailabiltyStartTime = Session["CheckAvailabilityTimeSessionStart"].ToString();
                    string FirstNationalitySelectTime = Session["FirstNationalitySelectTimeSession"].ToString();
                    if (!string.IsNullOrEmpty(CheckavailabiltyStartTime) && !string.IsNullOrEmpty(FirstNationalitySelectTime))
                    {
                        DateTime StartSession1 = Convert.ToDateTime(CheckavailabiltyStartTime);
                        DateTime EndSession1 = Convert.ToDateTime(FirstNationalitySelectTime);
                        var diffInSeconds1 = (EndSession1 - StartSession1).TotalSeconds;
                        if (diffInSeconds1 <= 4)
                        {
                            new Common().ErrorLog("FinalSubmit From WEB", "Nationality row time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                            return RedirectToAction("InvalidRequest");
                        }
                    }
                }

            }
            string dayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(cs.ArrivalDate.ToString("dd/MM/yyyy")).DayOfWeek);
            DateTime fdate = DateTime.Parse("30/06/2023");
            DateTime tdate = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            if (dayName == "Wednesday" && tdate > fdate)
            {
                new Common().ErrorLog("FinalSubmit From WEB", "Prohibited day Date entered by script", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                return RedirectToAction("BookingWarningMessage");
            }
            if (Session["AvaliableTicket"] == null)
            {
                new Common().ErrorLog("FinalSubmit From WEB", "Check availability not done", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                return RedirectToAction("InvalidRequest");
            }
            if (Convert.ToUInt32(Session["AvaliableTicket"]) == 0)
            {
                new Common().ErrorLog("FinalSubmit From WEB", "Check availability not done", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                return RedirectToAction("InvalidRequest");
            }
            if (Session["NationlitySelectionInvalidSession"].ToString() == "1" && isKioskUser == false)
            {
                new Common().ErrorLog("FinalSubmit From WEB", "Nationality Time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                return RedirectToAction("InvalidRequest");
            }

            string MemberFillSession = "";
            DateTime StartSession;
            DateTime EndSession;
            double diffInSeconds = 0.0;
            UInt32 ectualSeconds = 0;
            UInt32 cnt = 0;
            bool isIndianExists = false;
            if (isKioskUser == false || true) // this ( || true ) change made on 11-04-2022
            {
                foreach (var item in lstMemberInfo)  //----
                {
                    if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                    {
                        cnt++;
                        if (item.MemberNationality == "1")
                            isIndianExists = true;
                    }
                }
                MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
                Session["CheckAvailabilityTimeSessionEnd"] = DateTime.Now.ToString();
                StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
                EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
                diffInSeconds = (EndSession - StartSession).TotalSeconds;
                //ectualSeconds = Convert.ToUInt32(MemberFillSession) * 8;
                ectualSeconds = Convert.ToUInt32(cnt) * 8;
                if (ectualSeconds > diffInSeconds || diffInSeconds > 660)
                {
                    new Common().ErrorLog("FinalSubmit From WEB", "Member Detail total Time not waited", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                    return RedirectToAction("InvalidRequest");
                }
            }
            long UserId = Convert.ToInt64(Session["UserID"].ToString());
            if (isIndianExists == true)
            {

              
                DataTable vdt = await cs.ValidateSSOMobile(UserId);
                if (vdt.Rows.Count > 0)
                {
                    if (Convert.ToInt16(vdt.Rows[0]["intStatus"]) == 0)
                    {
                        TempData["RowCheck"] = vdt.Rows[0]["Msg"];
                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                    }
                }
                else
                {
                    TempData["RowCheck"] = "Kindly Update Your Valid Mobile Number on SSO Portal for booking !";
                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                }
            }
            if (UserId > 0)
            {
                string res1 = string.Empty;
                string ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipaddress))
                {
                    string[] ipRanges = ipaddress.Split(',');
                    int len1 = ipRanges.Length - 1;
                    ipaddress = ipRanges[0];
                }
                else
                {
                    ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                DataTable vdt2 = await cs.ValidateAllowedBookings(UserId, ipaddress, form["ArrivalDate"].ToString(),Convert.ToInt32(form["ddl_place"].ToString()), Convert.ToInt16(form["ddl_Shift"].ToString()));
                if (vdt2.Rows.Count > 0)
                { 
                    if (Convert.ToInt16(vdt2.Rows[0][0].ToString()) == 1)
                    {
                        TempData["RowCheck"] = vdt2.Rows[0][1].ToString();
                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                    }
                }
            }
           
                

            if (Session["PlaceIdBackup"] == null || Session["ZoneIdBackup"] == null || Session["ShiftIdBackup"] == null || Session["IsCurrentOrAdvanceBackup"] == null || Session["VehicleIdBackup"] == null)
            {
                if (Session["PlaceIdBackup"] == null)
                    new Common().ErrorLog("FinalSubmit From WEB", "Place id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                if (Session["ZoneIdBackup"] == null)
                    new Common().ErrorLog("FinalSubmit From WEB", "Zone id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                if (Session["ShiftIdBackup"] == null)
                    new Common().ErrorLog("FinalSubmit From WEB", "Shift id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                if (Session["IsCurrentOrAdvanceBackup"] == null)
                    new Common().ErrorLog("FinalSubmit From WEB", "Current or Advance Session not found", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                if (Session["VehicleIdBackup"] == null)
                    new Common().ErrorLog("FinalSubmit From WEB", "Vehicle id not supplied", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));


                return RedirectToAction("InvalidRequest");
            }
            else
            {
                if (Convert.ToInt64(form["ddl_place"].ToString()) != Convert.ToInt64(Session["PlaceIdBackup"]) || Convert.ToInt64(form["ddl_Zone"].ToString()) != Convert.ToInt64(Session["ZoneIdBackup"]) || Convert.ToInt16(form["ddl_Shift"].ToString()) != Convert.ToInt16(Session["ShiftIdBackup"].ToString()) || Session["IsCurrentOrAdvanceBackup"].ToString() != Session["CurrentBookingOrAdvanceBooking"].ToString() || Convert.ToInt32(form["ddl_vehicle"].ToString()) != Convert.ToInt32(Session["VehicleIdBackup"].ToString()))
                {
                    if (Convert.ToInt64(form["ddl_place"].ToString()) != Convert.ToInt64(Session["PlaceIdBackup"]))
                        new Common().ErrorLog("FinalSubmit From WEB", "Place id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                    if (Convert.ToInt64(form["ddl_Zone"].ToString()) != Convert.ToInt64(Session["ZoneIdBackup"]))
                        new Common().ErrorLog("FinalSubmit From WEB", "Zone id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                    if (Convert.ToInt16(form["ddl_Shift"].ToString()) != Convert.ToInt16(Session["ShiftIdBackup"].ToString()))
                        new Common().ErrorLog("FinalSubmit From WEB", "Shift id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                    if (Session["IsCurrentOrAdvanceBackup"].ToString() != Session["CurrentBookingOrAdvanceBooking"].ToString())
                        new Common().ErrorLog("FinalSubmit From WEB", "Current Session Or Advance Session trying to crossing each other (third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));

                    if (Convert.ToInt32(form["ddl_vehicle"].ToString()) != Convert.ToInt32(Session["VehicleIdBackup"].ToString()))
                        new Common().ErrorLog("FinalSubmit From WEB", "Vehile id crossing mis-matched(third party tool or did the revese engineering)", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));


                    return RedirectToAction("InvalidRequest");
                }
            }

            // above code To Stop Crosssing of Advance or Current booking Mukesh Kumar Jangid on 24/11/2021


            // Above Section of Code to Check Wait Time From Client Side Added By Mukesh Jangid On 20/11/2021

            #region Check Captcha Validation by Rajveer

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";

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
                await cs.InvalidCaptchaLog("InvalidCaptcha", Convert.ToString(Session["SSOID"]), result);
                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
            }
            #endregion

            else if (ErrorMessageFlag && cs.hdn_IAgreement == "1")
            //if (true)
            {
                //Changes by shaan 02/11/2020
                //string pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
                pagetypeSession = Session["CurrentBookingOrAdvanceBooking"] as string;
                string AvaliableTicketSession = Session["AvaliableTicket"] as string;
                //string MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
                MemberFillSession = Convert.ToString(Session["MemberFillingCount"]);
                bool IsValidRequestForCurrentOrAdvanced = false;
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") || true) // this ( || true ) change made on 11-04-2022
                {
                    if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
                    {
                        if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null) //--
                        {
                            cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                        }
                        if (cs.PlaceId == 53 || cs.PlaceId == 57)
                        {
                            if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                            {
                                IsValidRequestForCurrentOrAdvanced = true;
                            }
                        }
                        else
                        {
                            if (cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") || cs.ArrivalDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"))
                            {
                                IsValidRequestForCurrentOrAdvanced = true;
                            }
                        }

                        if (pagetypeSession == "1" && IsValidRequestForCurrentOrAdvanced == false)
                        {
                            return RedirectToAction("InvalidRequest");
                        }
                        if (pagetypeSession == "2" && IsValidRequestForCurrentOrAdvanced == true)
                        {
                            return RedirectToAction("InvalidRequest");
                        }
                        if (!string.IsNullOrEmpty(MemberFillSession))
                        {

                            Session["CheckAvailabilityTimeSessionEnd"] = DateTime.Now.ToString();
                            //DateTime StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
                            //DateTime EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
                            StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
                            EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
                            //var diffInSeconds = (EndSession - StartSession).TotalSeconds;
                            //var ectualSeconds = Convert.ToInt32(MemberFillSession) * 7;
                            diffInSeconds = (EndSession - StartSession).TotalSeconds;
                            ectualSeconds = Convert.ToUInt32(MemberFillSession) * 7;
                            if (ectualSeconds > diffInSeconds || diffInSeconds > 660)
                            {
                                return RedirectToAction("InvalidRequest");
                            }
                        }
                        else
                        {
                            return RedirectToAction("InvalidRequest");
                        }

                        string CheckavailabiltyStartTime = Session["CheckAvailabilityTimeSessionStart"].ToString();
                        string FirstNationalitySelectTime = Session["FirstNationalitySelectTimeSession"].ToString();
                        if (!string.IsNullOrEmpty(CheckavailabiltyStartTime) && !string.IsNullOrEmpty(FirstNationalitySelectTime))
                        {
                            //DateTime StartSession = Convert.ToDateTime(CheckavailabiltyStartTime);
                            //DateTime EndSession = Convert.ToDateTime(FirstNationalitySelectTime);
                            StartSession = Convert.ToDateTime(CheckavailabiltyStartTime);
                            EndSession = Convert.ToDateTime(FirstNationalitySelectTime);
                            //var diffInSeconds = (EndSession - StartSession).TotalSeconds;
                            diffInSeconds = (EndSession - StartSession).TotalSeconds;
                            if (diffInSeconds <= 3)
                            {
                                return RedirectToAction("InvalidRequest");
                            }
                        }

                        if (Session["NationlitySelectionInvalidSession"].ToString() == "1")
                        {
                            return RedirectToAction("InvalidRequest");
                        }
                    }  //---

                }
                else
                {
                    if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
                    {
                        if (Convert.ToInt64(form["ddl_place"].ToString()) == 2 || Convert.ToInt64(form["ddl_place"].ToString()) == 68)
                        {
                            return RedirectToAction("InvalidRequest");
                        }
                    }  //---

                }
                //end chnages
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                int rownumber = 0;
                int rowcount = 0;
                decimal finalAmount = 0;
                try
                {

                    foreach (var item in lstMemberInfo)  //----
                    {
                        if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                        {
                            rownumber = Convert.ToInt16(item.MemberSLNo);
                            rowcount++;
                        }
                    }  //---
                    if (rownumber != rowcount)
                    {
                        TempData["RowCheck"] = "Enter member details continiously";
                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                    }

                    if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
                    {
                        var duplicatememberIds = lstMemberInfo.GroupBy(xy => new { xy.MemberIdType, xy.MemberIdNo }).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                        if (duplicatememberIds.Count() > 1)
                        {
                            return RedirectToAction("InvalidRequest");
                        }

                        if (Convert.ToInt64(form["ddl_place"].ToString()) == 2 || Convert.ToInt64(form["ddl_place"].ToString()) == 68)
                        {
                            if (Session["CurrentToken"] != null)
                            {
                                if (!Session["CurrentToken"].ToString().Contains("R" + rowcount.ToString()))
                                {
                                    return RedirectToAction("InvalidRequest");
                                }
                            }
                            else
                            {
                                return RedirectToAction("InvalidRequest");
                            }
                        }
                    }  //---


                    #region Added by shaan to check arrival date is not equal to sunday ---29-06-2021
                    //Added by shaan to check arrival date is not equal to sunday 29-06-2021
                    //DateTime date = Convert.ToDateTime(cs.ArrivalDate);
                    //DayOfWeek day = date.DayOfWeek;
                    //string dayToday = day.ToString();
                    //if (day == DayOfWeek.Sunday)
                    //{
                    //    return RedirectToAction("InvalidRequest");
                    //}
                    //string SelectedVehicle = form["hdnVType"].ToString();
                    //if (cs.PlaceId == 68 && SelectedVehicle.ToLower() == "gypsy" && rowcount > 4)
                    //{
                    //    return RedirectToAction("InvalidRequest");
                    //}
                    //else if (cs.PlaceId == 68 && SelectedVehicle.ToLower() == "canter" && rowcount > 10)
                    //{
                    //    return RedirectToAction("InvalidRequest");
                    //}

                    //end 29-06-2021
                    #endregion
                    if (Convert.ToString(form["AutomatedScript"]) == null || Convert.ToString(form["AutomatedScript"]) != Convert.ToString(Session["AutomatedScript"]))
                    {
                        TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                        string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
                        string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
                        DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                    }
                    var diffInSeconds1 = (DateTime.Now - Convert.ToDateTime(Session["CheckAvailTime"].ToString())).TotalSeconds;

                    int diffMinuts = (diffInSeconds1 > 0 ? (int)diffInSeconds1 / 60 : 0);
                    int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["CheckAvailTime"].ToString());

                    if (Command == "Submit" && (AvaliableTicketSession != null && AvaliableTicketSession != "") && diffMinuts < timeout)
                    {
                        #region MemberInfo
                        DataTable dtMemberInfo = new DataTable();
                        dtMemberInfo = MemberInformation(lstMemberInfo);
                        if (dtMemberInfo.Rows.Count == 0)
                        {
                            TempData["RowCheck"] = "Enter member details";
                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        }
                        else
                        {
                            if (dtMemberInfo.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                                {
                                    finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                                }
                            }
                        }

                        #endregion

                        #region Submission

                        Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        {
                            cs.KioskUserId = "0";

                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();
                        }
                        cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                        Session["RequestId"] = RequestId();
                        cs.RequestId = Session["RequestId"].ToString();
                        if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                        {
                            cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                        }
                        else
                        {
                            cs.PlaceId = 0;
                        }
                        if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                        {
                            cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                        }
                        else
                        {
                            cs.ZoneId = 0;
                        }
                        if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                        {
                            cs.ShiftTime = form["ddl_Shift"].ToString();
                        }
                        else
                        {
                            cs.ShiftTime = "";
                        }
                        cs.TotalMember = Convert.ToInt32(rowcount);

                        if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                        {

                            cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                            #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                            //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                            //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                            //{
                            //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                            //}
                            //else
                            //{
                            //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                            //}

                            #endregion

                            #region Check Current Shift In Current Booking Restrict Inspect element Data Value Hard coded
                            if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 1 && cs.KioskUserId == "0")
                            {
                                if (Session["CheckShiftwiseValue"] != null)
                                {
                                    List<BookOnTicket> CheckShift = (List<BookOnTicket>)Session["CheckShiftwiseValue"];
                                    if (CheckShift != null && CheckShift.FirstOrDefault().DateOfArrival == cs.ArrivalDate.ToString("dd/MM/yyyy"))
                                    {
                                        if (CheckShift.FirstOrDefault().isMorning == "True" && cs.ShiftTime == "1")
                                        {

                                        }
                                        else if (CheckShift.FirstOrDefault().isEvening == "True" && cs.ShiftTime == "2")
                                        {

                                        }
                                        else
                                        {
                                            TempData["datevalidation"] = "This shift not available on this date.";
                                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                        }
                                    }

                                }

                            }

                            #endregion

                            DataTable DTCheckBooking = new DataTable();
                            #region Restrict Months
                            DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                            if (DTCheckBooking.Rows.Count > 0)
                            {
                                if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                                {
                                    TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }

                            }
                            #endregion

                            #region Get Open Days by rajveer


                            DataTable GetDaysDataTable = new DataTable();
                            GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                            long AddDaysVal = 0;
                            if (GetDaysDataTable.Rows.Count > 0)
                            {
                                AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                            }
                            #endregion
                            DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                            if (cs.ArrivalDate > expiryDate)
                            {
                                TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }

                            //Kiosk User Restrictation
                            if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                            {
                                //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                                TempData["datevalidation"] = "Arrival date should not be more than Today";
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }
                        }
                        if (TempData["DurationTo"] != null)
                        {

                            DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                            if (cs.ArrivalDate > MaxDate)
                            {
                                TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }
                        }
                        if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                        {
                            cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                            Session["VehicleId"] = cs.vehicleID;
                        }
                        else
                        {
                            cs.vehicleID = 0;
                        }
                        if (Session["VFeeTigerProject"] != null)
                        {

                            cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_TigerProject = 0;
                        }
                        if (Session["VFeeSurcharge"] != null)
                        {

                            cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_Surcharge = 0;
                        }
                        if (Session["TotaVechileFees"] != null)
                        {

                            cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_Total = 0;
                        }
                        if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                        {
                            cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                        }
                        else
                        { cs.AccomoID = 0; }
                        if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                        {
                            cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                        }
                        else
                        { cs.TotalRoom = 0; }
                        if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                        {
                            cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                        }
                        else
                        { cs.RoomCharge = 0; }

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
                        cs.IPAddress = result;

                        //Changes by shaan 03/11/2020
                        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") || true) // this ( || true ) change made on 11-04-2022
                        {
                            if (!string.IsNullOrEmpty(form["txt_cpt"].ToString()))  //---
                            {
                                if (Session["PlaceIdCurrentAdvanceSession"] != null)
                                {
                                    int[] places = { 2, 53, 57 };
                                    string pageType = Session["CurrentBookingOrAdvanceBooking"].ToString();
                                    int placeIdSession = Convert.ToInt32(Session["PlaceIdCurrentAdvanceSession"]);
                                    if (pageType == "1" && placeIdSession != cs.PlaceId)
                                    {
                                        return RedirectToAction("InvalidRequest");
                                    }
                                    if (pageType == "2" && !places.Contains(placeIdSession) == false && placeIdSession != cs.PlaceId)
                                    {
                                        return RedirectToAction("InvalidRequest");
                                    }


                                }
                                else
                                {
                                    return RedirectToAction("InvalidRequest");
                                }


                                if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null && Session["ArrivalDateCurrentAdvanceSession"] != null && Session["ShiftIdCurrentAdvanceSession"] != null && Session["VehicleIdCurrentAdvanceSession"] != null && AvaliableTicketSession != null)
                                {
                                    if (Session["PlaceIdCurrentAdvanceSession"].ToString() == cs.PlaceId.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == cs.ZoneId.ToString() && Session["ShiftIdCurrentAdvanceSession"].ToString() == cs.ShiftTime.ToString() && Session["VehicleIdCurrentAdvanceSession"].ToString() == cs.vehicleID.ToString())
                                    {

                                    }
                                    else
                                    {
                                        return RedirectToAction("InvalidRequest");
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("InvalidRequest");
                                }

                                //end chnages
                                //Changes by shaan 02/11/2020
                                if (string.IsNullOrEmpty(AvaliableTicketSession))
                                {
                                    return RedirectToAction("InvalidRequest");

                                }
                                if (cs.TotalMember > Convert.ToUInt32(AvaliableTicketSession))
                                {
                                    return RedirectToAction("InvalidRequest");
                                }
                                if (string.IsNullOrEmpty(MemberFillSession))
                                {
                                    return RedirectToAction("InvalidRequest");
                                }
                                if (cs.TotalMember > Convert.ToInt32(MemberFillSession))
                                {
                                    return RedirectToAction("InvalidRequest");
                                }
                            } //--
                        }
                        //end changes




                        DataTable dts = new DataTable();

                        DataTable dtcheckTicket = new DataTable();
                        DataTable dtcheckWaitingTicket = new DataTable();
                        string strcheckTicket = string.Empty;
                        //dtcheckTicket = cs.CheckTicketAvailability();
                        dtcheckTicket = await cs.CheckTicketAvailabilityWityPalaceOfWheel();

                        Session["BookingUserDetails"] = cs;
                        strcheckTicket = dtcheckTicket.Rows[0][0].ToString();

                        bool isBookingAvailable = false;

                        if (Convert.ToInt32(strcheckTicket) - cs.TotalMember >= 0 && cs.TotalMember > 0)
                        {
                            isBookingAvailable = true;
                        }
                        else if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"]) == 2)
                        {
                            dtcheckWaitingTicket = await cs.CheckTicketWaitingAvailability();
                            if (Convert.ToInt32(dtcheckWaitingTicket.Rows[0][0].ToString()) - cs.TotalMember >= 0 && cs.TotalMember > 0)
                            {
                                isBookingAvailable = true;
                            }
                        }

                        if (isBookingAvailable == true)
                        {
                            if (form["hdnVType"].ToString() == "Gypsy" && cs.TotalMember > 6)
                            {
                                TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                                string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
                                string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
                                DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }
                            else if (form["hdnVType"].ToString() == "Canter" && cs.TotalMember > 20)
                            {
                                TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                                string lstmemberlistlog = Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberInfo);
                                string onlinebookinglog = Newtonsoft.Json.JsonConvert.SerializeObject(cs);
                                DataTable rs = cs.Save_OnlineBookingAutomatedScript(Convert.ToInt64(Session["UserID"]), onlinebookinglog, lstmemberlistlog);
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }

                            //shaan chnage 06/11/2020
                            Session["ZoneIdCurrentAdvanceSession"] = null;
                            Session["ArrivalDateCurrentAdvanceSession"] = null;
                            Session["ShiftIdCurrentAdvanceSession"] = null;
                            Session["VehicleIdCurrentAdvanceSession"] = null;
                            //end changes

                            //if (Session["CurrentBookingOrAdvanceBooking"].ToString() == "1") ////Change done by Amit on 26-10-2020 for OTP change for Kiosk User Sariska
                            if ((Session["CurrentBookingOrAdvanceBooking"].ToString() == "1") && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false"))
                            {
                                DataTable dtOTPISShow = cs.IsOTPShow();
                                string IsShowOTP = dtOTPISShow.Rows[0][0].ToString();
                                if (IsShowOTP == "1")
                                {
                                    if (!string.IsNullOrEmpty(HttpContext.Session["RandomOTP"].ToString()))
                                    {
                                        string userOtp = Request.Form["txt_UserOtp"].ToString();
                                        string rndOtp = Session["RandomOTP"].ToString();
                                        if (rndOtp == userOtp)
                                        {
                                            //New code 25-02-2021
                                            //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
                                            //bool flg = true;

                                            //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
                                            //{
                                            //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
                                            //    {

                                            //        ///Execute here
                                            //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                            //        dtMemberInfo.AcceptChanges();
                                            //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                                            //        flg = false;
                                            //        GloblaUserModel.EnteredUserList.RemoveAt(0);
                                            //    }
                                            //}
                                            //New code 25-02-2021 End

                                            dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                            dtMemberInfo.AcceptChanges();
                                            dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                                        }
                                        else
                                        {
                                            TempData["RowCheck"] = "Invalid OTP";
                                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                        }
                                    }
                                    else
                                    {
                                        TempData["RowCheck"] = "OTP session time out!";
                                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                    }
                                }
                                else
                                {
                                    //New code 25-02-2021
                                    //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
                                    //bool flg = true;

                                    //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
                                    //{
                                    //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
                                    //    {
                                    //        ///Execute here
                                    //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                    //        dtMemberInfo.AcceptChanges();
                                    //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                                    //        flg = false;
                                    //        GloblaUserModel.EnteredUserList.RemoveAt(0);
                                    //    }
                                    //}
                                    //New code 25-02-2021 End

                                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                    dtMemberInfo.AcceptChanges();
                                    dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                                }
                            }
                            else
                            {
                                //New code 25-02-2021
                                //GloblaUserModel.EnteredUserList.Add(Session["UserID"].ToString());
                                //bool flg = true;

                                //while (GloblaUserModel.EnteredUserList.Count > 0 && flg == true)
                                //{
                                //    if (GloblaUserModel.EnteredUserList[0] == Session["UserID"].ToString())
                                //    {
                                //        ///Execute here
                                //        dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                //        dtMemberInfo.AcceptChanges();
                                //        dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                                //        flg = false;
                                //        GloblaUserModel.EnteredUserList.RemoveAt(0);
                                //    }
                                //}
                                //New code 25-02-2021 End

                                dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                                dtMemberInfo.AcceptChanges();
                                dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);




                            }
                        }         //--
                        else
                        {
                            TempData["RowCheck"] = "Permit not avaliable";
                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        }
                        //if (dts.Rows.Count > 0)
                        //{
                        //    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                        //    Session["totalprice"] = finalAmnt;
                        //    if (Session["totalprice"].ToString() == "0")
                        //    {
                        //        //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        //        TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
                        //        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        //    }
                        //}
                        //else
                        //{
                        //    //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        //    TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
                        //    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        //}
                        string TicketId = "";
                        if (dts.Rows.Count > 0)
                        {
                            decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());


                            Session["RequestId"] = Convert.ToString(dts.Rows[0]["Transaction_Id"]);////SET New request ID in Session by Amit On 01-03-2021
                            cs.RequestId = Convert.ToString(Session["RequestId"]);
                            ////END Change on 01-03-2021


                            string StatusType = dts.Rows[0]["StatusType"].ToString();
                            TicketId = dts.Rows[0]["TicketId"].ToString();
                            Session["totalprice"] = finalAmnt;
                            if (Session["totalprice"].ToString() == "0")
                            {
                                if (StatusType == "001")
                                {
                                    TempData["datevalidation"] = "Invalid request due unavailability in Database!";
                                    //TempData["datevalidation"] = "At this moment Ticket booking requests received are more than available tickets, please try again!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                                else if (StatusType == "002")
                                {
                                    TempData["datevalidation"] = "You have already booked tickets for the combination of the same date, same place,same Zone,same Shift, and same IP!";
                                    //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                                else if (StatusType == "003")
                                {
                                    TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later!!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                                else if (StatusType == "004")
                                {
                                    TempData["datevalidation"] = "Ticket not available.";
                                    // TempData["datevalidation"] = "The booking requests  received are more than the available inventory at this moment, please try again!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                                else if (StatusType == "005")
                                {
                                    TempData["datevalidation"] = "Your IP has been blocked, Please contact to administrator!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                                else
                                {
                                    TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later.";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }
                            }
                        }
                        else
                        {
                            //TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                            //TempData["datevalidation"] = "Due to the heavy user transaction load server taking too much time to respond, please try again later!";
                            //return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");

                            TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later!";
                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        }


                        #region Add Kiosk User by Rajveer
                        EducationTours edu = new EducationTours();
                        edu.Location = Convert.ToInt64(cs.PlaceId);
                        DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                            {
                                Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                            }
                        }

                        // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                        if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                        {
                            //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                            KioskPaymentDetails _obj = new KioskPaymentDetails();
                            _obj.ModuleId = 1;
                            _obj.ServiceTypeId = 1;
                            _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                            _obj.SubPermissionId = 1;



                            //_obj.RequestedId = Convert.ToString(cs.RequestId);  ///Commented by Amit on 01-03-2021 for get New RequestId
                            _obj.RequestedId = Convert.ToString(dts.Rows[0]["Transaction_Id"]); ///New RequestId Added by Amit on 01-03-2021


                            DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                            if (dtKiosk.Rows.Count > 0)
                            {
                                _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                ViewBag.ViewModel = dts.AsEnumerable();
                                return PartialView("KioskPaymentDetailWildlife", _obj);
                            }
                        }
                        else
                        {
                            //ViewData.Model = dts.AsEnumerable();
                            //Session["BookingDatamodal"] = dts.AsEnumerable();
                            //return View("OnlineTicketPayment");
                            if (TicketId.Trim() != "" && TicketId.Length > 0 && TicketId != null)
                            {
                                if (Convert.ToInt64(TicketId) > 0)
                                {
                                    ViewData.Model = dts.AsEnumerable();
                                    Session["BookingDatamodal"] = dts.AsEnumerable();
                                    Session["CrossVarification"] = null;
                                    Session["LblPlaceName"] = null;
                                    return View("OnlineTicketPayment");
                                }
                                else
                                {
                                    TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later !!!!";
                                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                                }

                            }
                            else
                            {
                                TempData["datevalidation"] = "Due to the heavy user transaction load server is taking too much time to respond, please try again later !!!";
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }
                        }
                        #endregion


                        // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                        #endregion
                    }
                    else
                    {
                        TempData["RowCheck"] = "Not Valid, Data begin pushed through a third party tool, Please try again!";
                        new Common().ErrorLog("Command=" + Command + ",AvaliableTicketSession=" + AvaliableTicketSession + ",diffMinuts=" + diffMinuts + ",timeout=" + timeout + "", actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                    }
                }
                catch (Exception ex)
                {
                    TempData["RowCheck"] = ex.Message;
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }

            }
            return RedirectToAction("BookOnlineTicket");
        }

        /// <summary>
        /// function to return member information in datatable
        /// </summary>
        /// <param name="lstMemberInfo"></param>
        /// <returns></returns>
        public DataTable MemberInformation(List<MemberInfo> lstMemberInfo)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(String));
                objDt2.Columns.Add("FeeTigerProject", typeof(String));
                objDt2.Columns.Add("FeeSurcharge", typeof(String));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(String));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(String));
                objDt2.Columns.Add("TRDF", typeof(String));

                objDt2.Columns.Add("TotalFeePerMember", typeof(String));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(String));
                objDt2.Columns.Add("TotalCamera", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(String));
                objDt2.Columns.Add("BoardingGuideFee", typeof(String));
                objDt2.Columns.Add("TotalBoardingFee", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(String));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(String));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(String));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(String));
                objDt2.Columns.Add("Isactive", typeof(int));
                objDt2.Columns.Add("PNR_NO", typeof(String));
                objDt2.Columns.Add("Seat_NO", typeof(String));
                objDt2.Columns.Add("Room_No", typeof(String));
                //Added by shaan 30-03-2021
                objDt2.Columns.Add("Fees_TigerProjectHalfDayFullDayCharge", typeof(String));
                objDt2.Columns.Add("Fee_SurchargeHalfDayFullDayCharge", typeof(String));
                //end
                

                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                    {

                        dr["Name"] = item.MemberName;
                        dr["Gender"] = item.MemberGender;
                        dr["Nationality"] = item.MemberNationality;
                        dr["IDType"] = item.MemberIdType;
                        dr["IDNo"] = item.MemberIdNo;
                        dr["MemberType"] = 2;
                        dr["FeeTigerProject"] = item.MemberFees_TigerProject;
                        dr["FeeSurcharge"] = item.MemberFees_Surcharge;
                        dr["CameraFeeTigerProject"] = item.CameraFees_TigerProject;
                        dr["CameraFeeSurcharge"] = item.CameraFees_Surcharge;
                        dr["TRDF"] = item.TRDF;

                        dr["BoardingVehicleFee"] = item.BoardingVehicleFee;
                        dr["BoardingGuideFee"] = item.BoardingGuideFee;

                        dr["BoardingVehicleFeeGSTPercentage"] = item.BoardingVehicleFeeGSTPercentage;
                        dr["BoardingGuideFeeGSTPercentage"] = item.BoardingGuideFeeGSTPercentage;
                        dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(item.BoardingVehicleFeeGSTAmount);
                        dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        item.TotalBoardingFee = Convert.ToDecimal(item.BoardingVehicleFee) + Math.Ceiling(item.BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(item.BoardingGuideFee) + Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        dr["TotalFeePerMember"] = item.TotalPerMemberFees;
                        dr["TotalFeePerCamera"] = item.TotalPerMemberCameraFees;
                        dr["TotalCamera"] = item.MemberTotalCamera;
                        dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) + (Convert.ToDecimal(item.TotalPerMemberCameraFees)) + (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        dr["Vehicle_TRDF"] = item.Vehicle_TRDF;
                        dr["GuidFee_TRDF"] = item.GuidFee_TRDF;
                        dr["isactive"] = 1;
                        dr["PNR_NO"] = string.IsNullOrEmpty(item.PNR_NO) ? "" : item.PNR_NO;
                        dr["Seat_NO"] = string.IsNullOrEmpty(item.Seat_NO) ? "" : item.Seat_NO;
                        dr["Room_No"] = string.IsNullOrEmpty(item.Room_No) ? "" : item.Room_No;
                        //Added by shaan 30-03-2021
                        dr["Fees_TigerProjectHalfDayFullDayCharge"] = "0.00";
                        dr["Fee_SurchargeHalfDayFullDayCharge"] = "0.00";
                        //End
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
        /// <summary>
        /// For Departmental User payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeptKioskUserPay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    // Added by Vandana Gupta for Departmental Kiosk Payment
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();
                    DataSet DS = _obj.GetRequestIDDetails(Session["RequestId"].ToString());
                    DataTable DTReqDetails = DS.Tables[0];

                    _obj.RequestedIdEn = Encryption.encrypt(Session["RequestId"].ToString());
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 2;
                    _obj.PermissionId = 1;
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Session["RequestId"].ToString();
                    //===== ADDED BY ARVIND
                    _obj.PlaceID = DTReqDetails.Rows[0]["PlaceID"].ToString();
                    _obj.PlaceName = DTReqDetails.Rows[0]["PlaceName"].ToString();
                    _obj.DateOfArrival = DTReqDetails.Rows[0]["DateOfArrival"].ToString();
                    _obj.ZoneID = DTReqDetails.Rows[0]["ZoneID"].ToString();
                    _obj.ZoneName = DTReqDetails.Rows[0]["ZoneName"].ToString();
                    _obj.VehicleID = DTReqDetails.Rows[0]["VehicleID"].ToString();
                    _obj.VehicleName = DTReqDetails.Rows[0]["VehicleName"].ToString();
                    _obj.ShiftTime = DTReqDetails.Rows[0]["ShiftTime"].ToString();
                    _obj.ShiftName = DTReqDetails.Rows[0]["ShiftName"].ToString();

                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["KioskUserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["totalprice"].ToString());
                    _obj.PaidOn = Convert.ToString(DateTime.Now);

                    if (DS != null && DS.Tables[1].Rows.Count > 0)
                    {
                        string stringVehicle = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        ViewBag.VehicleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(stringVehicle);
                    }

                    return PartialView("PaymentByDepartmentalKioskUser", _obj);
                }
                else
                {
                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }


        [HttpPost]
        public ActionResult DeptKioskUserPayVIPSeats()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    // Added by Vandana Gupta for Departmental Kiosk Payment
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();

                    DataTable DTReqDetails = _obj.GetRequestIDDetailsForVIPSeats(Session["RequestId"].ToString());

                    _obj.RequestedIdEn = Encryption.encrypt(Session["RequestId"].ToString());
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 2;
                    _obj.PermissionId = 1;
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Session["RequestId"].ToString();
                    //===== ADDED BY ARVIND
                    _obj.PlaceID = DTReqDetails.Rows[0]["PlaceID"].ToString();
                    _obj.PlaceName = DTReqDetails.Rows[0]["PlaceName"].ToString();
                    _obj.DateOfArrival = DTReqDetails.Rows[0]["DateOfArrival"].ToString();
                    _obj.ZoneID = DTReqDetails.Rows[0]["ZoneID"].ToString();
                    _obj.ZoneName = DTReqDetails.Rows[0]["ZoneName"].ToString();
                    _obj.VehicleID = DTReqDetails.Rows[0]["VehicleID"].ToString();
                    _obj.VehicleName = DTReqDetails.Rows[0]["VehicleName"].ToString();
                    _obj.ShiftTime = DTReqDetails.Rows[0]["ShiftTime"].ToString();
                    _obj.ShiftName = DTReqDetails.Rows[0]["ShiftName"].ToString();

                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["KioskUserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["totalprice"].ToString());
                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                    return PartialView("PaymentByDepartmentalKioskUserVIP", _obj);
                }
                else
                {
                    return RedirectToAction("wildlifeBooking", "BookOnlineTicket");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        //Note: Code Updated with Ref. to bug ID 77
        /// <summary>
        /// Function to pay ticket amount
        /// </summary>
       
        [HttpPost]
        public async Task<ActionResult> Pay()
        {
            bool ErrorMessageFlag = true;
            //#region Check Captcha Validation by Rajveer
            //if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "False" && Convert.ToString(Session["IsKioskUser"]) == "False" && Convert.ToString(this.IsCaptchaValid("Validate your captcha")) == "False")
            //{
            //    ErrorMessageFlag = false;
            //    TempData["ErrorMessage"] = "Invalid captcha";
            //}
            //#endregion 
            //else 
            if (ErrorMessageFlag)
            //if (true)
            {
                int PlaceId = 0;
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                try
                {
                    // get different heads amount from DB
                    BookOnTicket OBJ = new BookOnTicket();
                    DataTable dtPlace = new DataTable();
                    DataTable dt = new DataTable();
                    int AvailableSeats = -1;
                    int isCurrent = -1;
                    if(Session["CurrentBookingOrAdvanceBooking"]!=null)
                        isCurrent = Convert.ToInt16(Session["CurrentBookingOrAdvanceBooking"].ToString());

                    dtPlace= await OBJ.GetPlaceId(Convert.ToString(Session["RequestId"]));
                    PlaceId =Convert.ToInt32( dtPlace.Rows[0]["PlaceId"].ToString());
                    if ((isCurrent == 1 || isCurrent == 2) && (PlaceId == 2 || PlaceId == 53 || PlaceId == 57 || PlaceId == 68))
                    {
                        dt = await OBJ.CheckAvailability_On_BeforeAndAfterPay(UserID, Convert.ToString(Session["RequestId"]), true);

                        AvailableSeats = Convert.ToInt32(dt.Rows[0]["SeatAvailableCount"].ToString());
                        string customMsg = dt.Rows[0]["Msg"].ToString();
                        PlaceId = (int)dt.Rows[0]["PlaceID"];
                        //if (AvailableSeats == -1)
                        if (AvailableSeats < 0)
                        {
                            TempData["datevalidation"] = "Permit  not available!";
                            return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                        }
                        else if ((isCurrent == 1 || isCurrent == 2) && (PlaceId == 2 || PlaceId == 53 || PlaceId == 57 || PlaceId == 68))
                        {

                            string requStatus = await OBJ.CheckRequestStatus(UserID, Convert.ToString(Session["RequestId"]));
                            if (requStatus == "N")
                            {
                                TempData["datevalidation"] = "Permit  not available!";
                                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            }
                            else { 
                            //else if (requStatus == "F") //MODIFIED ON 29-10-2022
                            //{
                            //    TempData["datevalidation"] = "Request No : " + Session["RequestId"] + " Transaction Failed";
                            //    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                            //}
                            //else if (requStatus == "R") //MODIFIED ON 29-10-2022
                            //{

                                DataSet DS = new DataSet();
                                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("WildLifeTickets", Convert.ToString(Session["RequestId"]));
                                string REVENUEHEAD = "";
                                int IsAmtInsertForPatialRefund = 0;
                                if (PlaceId == 2)
                                {
                                    decimal TotalTicketAmount = 0;
                                    decimal RemaingReturningFund = OBJ.Get_ReturningAmountBalance();

                                    if (RemaingReturningFund > 0)
                                    {
                                        if (RemaingReturningFund > 0)
                                        {


                                            TotalTicketAmount = Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["TigerProject"].ToString()) + Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Surcharge"].ToString())));// + Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()));

                                            if (RemaingReturningFund - TotalTicketAmount >= 0)
                                            {
                                                TotalTicketAmount = TotalTicketAmount + Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()));
                                                string toBeRevenueHeadEntryFeeCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]); //+ "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]);
                                                string toBeRevenueHeadEcoDevelopmentSurchargeCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]);// + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]);
                                                string toBeRevenueHeadFOUNDATIONCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]);// + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"]);
                                                string toBeHeadVehicleRentAndGuidFees = Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]);// + "-" + Convert.ToString(Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"])));
                                                IsAmtInsertForPatialRefund = OBJ.SaveReturningAmount(Convert.ToString(Session["RequestId"]), toBeRevenueHeadEntryFeeCode, toBeRevenueHeadEcoDevelopmentSurchargeCode, toBeRevenueHeadFOUNDATIONCode, toBeHeadVehicleRentAndGuidFees, Convert.ToDecimal(DS.Tables[0].Rows[0]["TigerProject"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["Surcharge"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"].ToString()));
                                                if (IsAmtInsertForPatialRefund == 1)
                                                {
                                                    REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-0|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(0) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(TotalTicketAmount) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"])));
                                                }
                                            }
                                        }
                                    }
                                }
                                if (IsAmtInsertForPatialRefund == 0)
                                    REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"] + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"]))));
                                    // string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"] + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"]));
                                // string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();

                                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrlForLocal"].ToString();

                                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();
                                string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["RequestId"]),
                                Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                                Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                                Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                                ReturnUrl + "BookOnlineTicket/Payment",
                                ReturnUrl + "BookOnlineTicket/Payment",
                                Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                                Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());


                                //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + "Step Pay : Request Post To Emitra ========== " });

                                Response.Write(forms);

                            } //MODIFIED ON 29-10-2022
                        }
                    }                    
                    else
                    {
                        DataSet DS = new DataSet();
                        DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("WildLifeTickets", Convert.ToString(Session["RequestId"]));
                        string REVENUEHEAD = "";
                        int IsAmtInsertForPatialRefund = 0;
                        if (PlaceId == 63)
                        {
                            decimal TotalTicketAmount = 0;
                            decimal RemaingReturningFund = OBJ.Get_ReturningAmountBalance();

                            if (RemaingReturningFund > 0)
                            {
                                if (RemaingReturningFund > 0)
                                {

                                    TotalTicketAmount = Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["TigerProject"].ToString()) + Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Surcharge"].ToString())));// + Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()));

                                    if (RemaingReturningFund - TotalTicketAmount >= 0)
                                    {
                                        TotalTicketAmount= TotalTicketAmount+ Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()));
                                        string toBeRevenueHeadEntryFeeCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]); //+ "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]);
                                        string toBeRevenueHeadEcoDevelopmentSurchargeCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]);// + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]);
                                        string toBeRevenueHeadFOUNDATIONCode = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]);// + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"]);
                                        string toBeHeadVehicleRentAndGuidFees = Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]);// + "-" + Convert.ToString(Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"])));
                                        IsAmtInsertForPatialRefund = OBJ.SaveReturningAmount(Convert.ToString(Session["RequestId"]), toBeRevenueHeadEntryFeeCode, toBeRevenueHeadEcoDevelopmentSurchargeCode, toBeRevenueHeadFOUNDATIONCode, toBeHeadVehicleRentAndGuidFees, Convert.ToDecimal(DS.Tables[0].Rows[0]["TigerProject"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["Surcharge"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["Foundation"].ToString()), Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"].ToString()));
                                        if (IsAmtInsertForPatialRefund == 1)
                                        {
                                            REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-0|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(0) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(TotalTicketAmount) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(Math.Ceiling(Convert.ToDecimal(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"])));
                                        }
                                    }
                                }
                            }
                        }
                        if (IsAmtInsertForPatialRefund == 0)
                             REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"] + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"]));
                        // string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();

                        string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrlForLocal"].ToString();

                        EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();
                        string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["RequestId"]),
                      Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                      Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                      Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                      ReturnUrl + "BookOnlineTicket/Payment",
                      ReturnUrl + "BookOnlineTicket/Payment",
                      Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                      Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());

                        //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + "Step Pay : Request Post To Emitra ========== " });

                        Response.Write(forms);
                        
                    }


                    //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + "Step Pay : Get Heads amount from DB ========== " });


                    //string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["RequestId"]),
                    //    Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                    //    Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                    //    Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                    //    ReturnUrl + "BookOnlineTicket/Payment", ReturnUrl + "BookOnlineTicket/Payment",
                    //    Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                    //    Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }


            //ViewData.Model = dts.AsEnumerable();
            ViewData.Model = Session["BookingDatamodal"];
            return View("OnlineTicketPayment");
        }
        /// <summary>
        /// function to display response from emitra
        /// </summary>
        /// <returns></returns>
        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int fmdssStatus = 0;string resultMsg = "";
            DataTable resDt = new DataTable();
            if (Session["RequestId"] != null)
            {
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

                    //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23"); //Live
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    // string DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";

                    // string DecryptedData = "{'MERCHANTCODE':'FOREST0117', 'REQTIMESTAMP':'20190614131735000' ,'PRN':'636961150514796677' ,'AMOUNT':'1994.00' ,'PAIDAMOUNT':'2000.00', 'SERVICEID':'2239' ,'TRANSACTIONID':'190208589073', 'RECEIPTNO':'19199639988' ,'EMITRATIMESTAMP':'20190614131843682', 'STATUS':'SUCCESS', 'PAYMENTMODE':'BILLDESK(RPP)', 'PAYMENTMODEBID':'22889757' ,'PAYMENTMODETIMESTAMP':'20190614131741000' ,'RESPONSECODE':'200', 'RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.', 'UDF1':'BHUPENDRA SHARMA' ,'UDF2':'udf2', 'CHECKSUM':'c1b14b96b845327065053366e11abd39'}";

                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["RequestId"].ToString(), "WildLifeTicketBooking", DecryptedData);

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion

                    //****************************** for test only

                    //ObjPGResponse.STATUS = "SUCCESS";
                    //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                    //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                    //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                    //****************************** for test only;

                    string steps = string.Empty;
                    #region Response Status
                    // if (ObjPGResponse.STATUS == "FAILED") Arvind Sir
                    if (ObjPGResponse.STATUS != "SUCCESS") //Rajveer
                    {
                        //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 0: Enter Success " });
                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;
                        // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                        //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
                        Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        {
                            cs.KioskUserId = "0";
                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();

                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                        //if (System.Threading.Monitor.TryEnter(emitraLock, 200000))
                        //{

                        //  System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 0: Enter Success " });
                        try
                        {
                            cs.Trn_Status_Code = 0;
                            fmdssStatus = 0;
                            resultMsg = "";
                            resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                            if (resDt.Rows.Count > 0)
                            {

                                fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                                resultMsg = resDt.Rows[0][3].ToString();
                            }
                                // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 1: After DB " });
                            }
                        catch (Exception ex)
                        {
                            //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 2: catch in Thread " + ex.Message });
                        }
                        finally
                        {
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 3: finally execute " });
                            // System.Threading.Monitor.Exit(emitraLock);
                        }
                        // }

                        //lock (emitraLock)
                        //{
                        //    cs.Trn_Status_Code = 0;
                        //    fmdssStatus = cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                        //}

                        //*****************************************************************

                        if (fmdssStatus == 1)
                        {

                            dtrow["TRANSACTION STATUS"] = "SUCCESS";                            
                            int status = cs.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), dtrow["TRANSACTION STATUS"].ToString(), true);
                            SendSMSEmailForSuccessTransaction();
                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";

                        }
                        dt.Rows.Add(dtrow);

                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {
                        // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 0: Enter Success " });
                        DataRow dtrow = dt.NewRow();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = ObjPGResponse.PRN;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {

                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 1: Enter Success " });
                            //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************

                            //if (System.Threading.Monitor.TryEnter(emitraLock, 200000))
                            //{
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 2: Enter in Threading " });
                            try
                            {
                                // if (Convert.ToString(Session["RequestId"]).Equals(ObjPGResponse.PRN) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                                if (Convert.ToString(Session["RequestId"]).Equals(ObjPGResponse.PRN) && (Session["totalprice"] != null && Convert.ToDecimal(ObjPGResponse.AMOUNT) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                                {
                                    int status = cs.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), dtrow["TRANSACTION STATUS"].ToString(),true);
                                    // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 3: check RequestId and amount" });
                                    cs.Trn_Status_Code = 1;
                                    status1 = 1;
                                    fmdssStatus = 0;
                                    resultMsg = "";
                                    resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                                    if (resDt.Rows.Count > 0)
                                    {

                                        fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                                        resultMsg = resDt.Rows[0][3].ToString();
                                    }
                                }
                                else // Added to save mismatch in payment
                                {
                                    // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 4: not match RequestId and amount" });
                                    cs.Trn_Status_Code = 0;
                                    status1 = 0;
                                    fmdssStatus = 0;
                                    resultMsg = "";
                                    resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                                    if (resDt.Rows.Count > 0)
                                    {
                                        fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                                        resultMsg = resDt.Rows[0][3].ToString();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                resultMsg = ex.Message.ToString() + " , " + ex.StackTrace.ToString();
                                // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 5: catch in Thread " + ex.Message });
                            }

                            finally
                            {
                                // System.Threading.Monitor.Exit(emitraLock);
                                // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 6: finally execute " });
                            }
                            //}

                            //lock (emitraLock)
                            //{
                            //    if (Convert.ToString(Session["RequestId"]).Equals(finalreqid) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(finalamount)))
                            //    {
                            //        cs.Trn_Status_Code = 1;
                            //        status1 = 1;
                            //        fmdssStatus = cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                            //    }
                            //    else // Added to save mismatch in payment
                            //    {
                            //        cs.Trn_Status_Code = 0;
                            //        status1 = 0;
                            //        fmdssStatus = cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                            //    }
                            // }
                            //******************************************************

                        }
                        if (fmdssStatus == 1)
                        {

                            dtrow["TRANSACTION STATUS"] = "SUCCESS";
                            int status = cs.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), dtrow["TRANSACTION STATUS"].ToString(), true);
                            SendSMSEmailForSuccessTransaction();
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 7: SUCCESS : SendSMSEmailForSuccessTransaction " });
                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";
                            int status = cs.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), dtrow["TRANSACTION STATUS"].ToString(), false);
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 8: FAILED : not SendSMSEmailForSuccessTransaction " });
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion
                    List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();

                    ViewBag.TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();
                    ViewBag.ResultMsg = resultMsg;
                    if (ViewBag.TicketStatus == "SUCCESS")
                    {
                        DataTable DTdetails = cs.Get_BookedTicketDetails(Session["RequestId"].ToString(), "ONLINRBOOKING");
                        int status = cs.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), ViewBag.TicketStatus, true);
                        foreach (DataRow dr in DTdetails.Rows)
                        {
                            List.Add(
                                   new CS_BoardingDetails()
                                   {
                                       PrintID = Convert.ToString(dr["PrintID"]),
                                       RequestID = Convert.ToString(dr["RequestID"]),
                                       PlaceName = Convert.ToString(dr["PlaceName"]),
                                       Vehicle = Convert.ToString(dr["Vehicle"]),
                                       TotalMembers = Convert.ToString(dr["TotalMembers"]),
                                       DateofBooking = Convert.ToString(dr["DateofBooking"]),
                                       DateofVisit = Convert.ToString(dr["DateofVisit"]),
                                       AmountTobePaid = Convert.ToString(dr["AmountTobePaid"]),
                                       BoardingPointName = Convert.ToString(dr["BoardingPointName"]),
                                   });

                        }

                        if (DTdetails.Rows.Count > 0)
                        {
                            ViewBag.PrintID = Convert.ToString(DTdetails.Rows[0]["PrintID"]);
                        }
                        else
                        {
                            ViewBag.PrintID = "";
                        }

                    }

                    ViewData["TicketSummary"] = List;

                    ViewData.Model = dt.AsEnumerable();
                    if (resultMsg != "Success")
					{
						BookOnTicket cs2 = new BookOnTicket();
						DataTable DTdetails = cs2.UpdateFailedTicketStatus(Session["RequestId"].ToString());
                        int status = cs2.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), resultMsg, false);
                    }
                }
                catch (Exception ex)
                {
                    if (resultMsg != "Success")
                    {
                        BookOnTicket cs2 = new BookOnTicket();
                        int status = cs2.UpdatePartialAddFundEmitraStatus(Session["RequestId"].ToString(), resultMsg, false);
                        DataTable DTdetails = cs2.UpdateFailedTicketStatus(Session["RequestId"].ToString());
                    }
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                    //  System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 8: Final catch " + ex.Message });
                }
                return View("TransactionStatus");
            }
            return View();
        }

        public void SendSMSEmailForSuccessTransaction()
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataSet DT = new DataSet();
            DT = objSMSandEMAILtemplate.GetUserDetailsWildlife(Session["RequestId"].ToString(), "GETUSERDETAILSFORSENDSMSANDEMAILforWildLife");
            if (DT.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(DT.Tables[0].Rows[0]["EmailId"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Tables[0].Rows[0]["NAME"]), Convert.ToString(DT.Tables[0].Rows[0]["PlaceName"]), Convert.ToString(DT.Tables[0].Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["WildLifeTicketEmailTemplate"].ToString()));

                    objSMS_EMail_Services.sendEMail("WildLife Ticket Booking for RequestID : " + Convert.ToString(DT.Tables[0].Rows[0]["RequestID"]), body, Convert.ToString(DT.Tables[0].Rows[0]["EmailId"]), ConfigurationManager.AppSettings["WildLifeTicketEmail_CC"].ToString());

                    body = string.Empty;

                }

                if (Convert.ToString(DT.Tables[0].Rows[0]["Mobile"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Tables[0].Rows[0]["NAME"]), Convert.ToString(DT.Tables[0].Rows[0]["PlaceName"]), Convert.ToString(DT.Tables[0].Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["WildLifeTicketSMSTemplate"].ToString()));

                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Tables[0].Rows[0]["Mobile"]), body);

                    if (DT.Tables[1].Rows.Count > 0)
                    {
                        if (DT.Tables[1].Columns.Contains("MobileNo"))
                        {
                            foreach (DataRow dr in DT.Tables[1].Rows)
                            {
                                SMS_EMail_Services.sendSingleSMS(Convert.ToString(dr["MobileNo"]), body);
                            }
                        }
                    }

                    body = string.Empty;
                }

            }

            #endregion

        }
       
        /// <summary>
        /// Function to print ticket with all necessory details on the basis of ticket ID
        /// </summary>
        /// <param name="ticketid"></param>
        /// <returns>Json result with ticket details</returns>
        [HttpPost, ValidateHeaderAntiForgeryToken]
        public string PrintTicket(Int64 ticketid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                BookOnTicket cs = new BookOnTicket();
                cs.TicketID = ticketid;

                ds = cs.Select_TicketData_For_Print();

                DT1 = ds.Tables[0];
                DT2 = ds.Tables[1];
                DT3 = ds.Tables[2];
                sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><img src='../images/risl-logo-small.png' alt='RISL' ></div><div class='col-xs-12 col-sm-4 centr'>Department of Forest, <br>Goverment of<br> Rajasthan</span></div><div class='col-xs-12 col-sm-4' style='padding: 0'> <span class='pull-right pdate'><img src='../images/e-mitra_logo.png' alt='E-Mitra' > </div>  <div class='divider'></div></div>");
                if (ds != null)
                {

                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr> <td col-lg-12 colspan='5' style='text-align:center'>BOOKING CONFIRMATION SLIP</td></tr>");
                    sb.Append("<tr><td col-lg-3><b>Reserve : </b></td><td col-lg-3>" + DT1.Rows[0]["PlaceName"] + "</td><td col-lg-3><b>Booking No:</b> </td><td col-lg-3>" + DT1.Rows[0]["RequestID"] + " </td></tr>");

                    sb.Append("<tr><td col-lg-3><b>Date/Time of Booking : </b></td><td col-lg-3>" + DT1.Rows[0]["EnteredOn"] + "</td><td col-lg-3><b>Date of Visit : </b> </td><td col-lg-3>" + DT1.Rows[0]["DateOfArrival"] + " </td></tr>");
                    sb.Append("<tr><td col-lg-3><b>Booked Seats : </b></td><td col-lg-3 colspan='3'>" + DT1.Rows[0]["NoofTicket"] + "</td> </tr>");
                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3><b>S.No</b></td><td col-lg-3><b>Visitor Name</b></td><td col-lg-3><b>Category</b></td><td col-lg-3><b>Id Proof & Id Details</b></td><td col-lg-3><b>Camera</b></td><td col-lg-3><b>Shift</b></td><td col-lg-3><b>Vehicle</b></td><td col-lg-3><b>Amount(Rs.)</b></td></tr>");

                    for (int i = 0; i < DT2.Rows.Count; i++)
                    {
                        int j = i + 1;
                        sb.Append("<tr><td col-lg-3>" + j + "</td><td col-lg-3>" + DT2.Rows[i]["Name"] + "</td><td col-lg-3>" + DT2.Rows[i]["Nationality"] + "</td><td col-lg-3>" + DT2.Rows[i]["IDProof"] + "</td><td col-lg-3>" + DT2.Rows[i]["NoOfCamera"] + "</td><td col-lg-3>" + DT2.Rows[i]["Shift"] + "</td><td col-lg-3>" + DT2.Rows[i]["VName"] + "</td><td col-lg-3>" + DT2.Rows[i]["Amount"] + "</td></tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3 ><b>Total Amount Incl. of Service Charges: Rs.</b></td><td col-lg-3 colspan='5'>" + Convert.ToString(DT1.Rows[0]["AmountTobePaid"]) + "</td> </tr>");
                    sb.Append("<tr><td col-lg-3 ><b>Boarding Point : </b></td><td col-lg-3 colspan='5'>" + DT1.Rows[0]["Boarding_Point"] + "</td> </tr>");
                    sb.Append("<tr><td col-lg-3><b>Contact Person : </b></td><td col-lg-3>" + DT1.Rows[0]["contactperson"] + "</td><td col-lg-3><b>Phone No : </b></td><td col-lg-3>" + DT1.Rows[0]["PhoneNo"] + "</td><td col-lg-3><b>Address : </b></td><td col-lg-3>" + DT1.Rows[0]["Address"] + "</td> </tr>");
                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3  colspan='2' >Terms and conditions for Visitors :</td> </tr>");



                    ///Start Static Code

                    //sb.Append("<tr><td col-lg-3 >1.</td><td col-lg-3>The visitor must reach to the forest booking center to collect the boarding pass,at least 45 minutes prior to the departure time. </td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >2.</td><td col-lg-3>In case, same Id proof will not be produced at the time of seeking entrance in the park then the ticket shall be deemed cancelled or fake.</td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >3.</td><td col-lg-3>Aforesaid total charges include tourist entry fee,Reserve development fee,Vehicle entry fee,Reserve development fee and online Booking charges. </td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >4.</td><td col-lg-3>Please take two copies of this voucher and bring with your self at the time of boarding. </td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >5.</td><td col-lg-3>Every visitor has to pay vehicle rent and Guide fee at the time of collecting boarding pass additionally.(If applicable) </td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >6.</td><td col-lg-3>Seats remaining vacant due to non-turn up of any visitors can filled by the park management at the booking window. </td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >7.</td><td col-lg-3>Boarding pass of Morning shift shall be collected from the booking office: " + DT1.Rows[0]["PlaceName"].ToString() + " during 5:00 PM to 8:00 PM of previous evening and for evening shift during 10:00 AM to 01:00 PM.</td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >8.</td><td col-lg-3>In order to avoid fake bookings, a restriction of booking maximum 6 seats in sinlge transaction has been made.</td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >9.</td><td col-lg-3>In case of group booking, park authority will try to give preference in a single vehicle based on availibility at the time of entry.</td> </tr>");
                    //sb.Append("<tr><td col-lg-3 >10.</td><td col-lg-3>Entry time of park: </td> </tr>");


                    ////Get T&C by TicketId
                    DataTable dtTC = new DataTable();
                    dtTC = cs.Select_TermandConditionbyTicketId(ticketid);
                    int index = 1;
                    for (int i = 0; i < dtTC.Rows.Count; i++)
                    {
                        string sTC = Convert.ToString(dtTC.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());
                        sb.Append("<tr><td col-lg-3 >" + index + "</td><td col-lg-3>" + sTC + "</td> </tr>");
                        index += 1;
                    }

                    sb.Append("<tr><td col-lg-3  colspan='2' >");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr> <td col-lg-3>Period</td><td col-lg-3>Morning Trip</td><td col-lg-3> Afternoon Trip </td>");
                    for (int k = 0; k < DT3.Rows.Count; k++)
                    {
                        sb.Append("<tr> <td col-lg-3>" + DT3.Rows[k]["Period"] + "</td><td col-lg-3>" + DT3.Rows[k]["MorningTrip"] + "</td><td col-lg-3>" + DT3.Rows[k]["AfterNoonTrip"] + "</td>");
                    }
                    sb.Append("</td> </tr>");
                    sb.Append("</table>");
                    sb.Append("</table>");

                    ///End Static Code

                    string filepath = htmlToPdfDownloadTickets.WildlifeDownloadPdf(ds, dtTC);


                    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(filepath)))
                    {
                        string FilePath = Server.MapPath(filepath);
                        WebClient User = new WebClient();
                        Byte[] FileBuffer = User.DownloadData(FilePath);
                        if (FileBuffer != null)
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", FileBuffer.Length.ToString());
                            Response.BinaryWrite(FileBuffer);
                            Response.End();
                        }
                    }

                }


                sb.Append("</div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString();
        }
        [HttpPost]
        public JsonResult UserCovidOptionalBooking(CovidBooking oCovid)
        {
            BookOnTicket cs = new BookOnTicket();
            DataSet DT = new DataSet();
            string ticketid= Encryption.decrypt(oCovid.TicketId);
            DataTable dtf = cs.Select_BookedTicketStatus(Convert.ToInt64(ticketid));
            int IsValidOperattion = 0;
            var isValidRequest = new BookOnTicket();
            DataTable dtChek = new DataTable();
            if (!string.IsNullOrEmpty(oCovid.TicketId))
            {
                isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.TicketID == Convert.ToInt64(Encryption.decrypt(oCovid.TicketId))).FirstOrDefault();

            }
            else
            {
                isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.RequestedId == oCovid.RequestID).FirstOrDefault();

            }
            if (isValidRequest != null)
            {
                if (isValidRequest.COVIDStatus == 1 && isValidRequest.TicketMemberBordingStatus == 0)
                {

                    IsValidOperattion = 1;
                    DT = cs.Save_UserCovidOptionalBooking(oCovid);

                    if (string.IsNullOrEmpty(oCovid.ApprovedVisitDate) && !string.IsNullOrEmpty(oCovid.FirstDate))
                    {
                        if (Convert.ToInt32(DT.Tables[1].Rows[0]["isDFOApproved"]) == 0)
                        {
                            MailCovidData MailCovidData = new MailCovidData();
                            //MailCovidData.ToMail="sanjaysingh.jadon@gmail.com";
                            MailCovidData.ToMail = "arindam.ifs.forest@rajasthan.gov.in";
                            MailCovidData.MailSubject = "Citizen Covid Booking Request";
                            MailCovidData.RequestID = DT.Tables[0].Rows[0]["RequestID"].ToString();
                            MailCovidData.PreviousVisitDate = DT.Tables[0].Rows[0]["PreviousVisitDate"].ToString();
                            MailCovidData.PreviousShift = DT.Tables[0].Rows[0]["PreviousShift"].ToString();
                            MailCovidData.PreviousZone = DT.Tables[0].Rows[0]["PreviousZone"].ToString();
                            MailCovidData.NoOFMembers = DT.Tables[0].Rows[0]["NoOFMembers"].ToString();
                            MailCovidData.FirstDate = DT.Tables[0].Rows[0]["FirstDate"].ToString();
                            MailCovidData.SecondDate = DT.Tables[0].Rows[0]["SecondDate"].ToString();
                            MailCovidData.ThirdDate = DT.Tables[0].Rows[0]["ThirdDate"].ToString();
                            MailCovidData.RequestedShift = DT.Tables[0].Rows[0]["RequestedShift"].ToString();
                            MailCovidData.RequestedPlaceName = DT.Tables[0].Rows[0]["RequestedPlaceName"].ToString();

                            string Message = "COVID-19 Special Booking" + Environment.NewLine + " " + Environment.NewLine + "Request Id :" + MailCovidData.RequestID + " " + Environment.NewLine + "Place Name : " + MailCovidData.RequestedPlaceName + " " + Environment.NewLine + "Previous Zone : " + MailCovidData.PreviousZone + " " + Environment.NewLine + "Previous Shift : " + MailCovidData.PreviousShift + " " + Environment.NewLine + "Previous Visit Date : " + MailCovidData.PreviousVisitDate + " " + Environment.NewLine + "Total Members : " + MailCovidData.NoOFMembers + " " + Environment.NewLine + "First Optional Date : " + MailCovidData.FirstDate + " " + Environment.NewLine + "Second Optional Date : " + MailCovidData.SecondDate + " " + Environment.NewLine + "Third Optional Date : " + MailCovidData.ThirdDate + " " + Environment.NewLine + "Requested Shift : " + MailCovidData.RequestedShift;

                            string html = "<html><body><h3>COVID-19 Special Booking</h3><p>Sir,</p><p>In the reference of the visit during lockdown period, guest has been submitted choices of visit date as follows:</p>" +
                                "<h4>a. Previous Ticket Details:<h4>" +
                                "<table cellpadding='2px' border='1px'>" +
                                "<tr><td>Request Id : " + MailCovidData.RequestID + "</td><td>Place Name : " + MailCovidData.RequestedPlaceName + "</td><td>Zone : " + MailCovidData.PreviousZone + "</td></tr>" +
                                "<tr><td>Date Of Visit : " + MailCovidData.PreviousVisitDate + "</td><td>Shift : " + MailCovidData.PreviousShift + "</td><td>Total Visitors : " + MailCovidData.NoOFMembers + "</td></tr>" +
                                "<table>" +
                                "<h4>b. Choices of Visit Date:<h4>" +
                                "<table cellpadding='2px' border='1px'>" +
                                "<tr><td>First Optional Date : " + MailCovidData.FirstDate + "</td><td>Second Optional Date : " + MailCovidData.SecondDate + "</td><td>Third Optional Date : " + MailCovidData.ThirdDate + "</td></tr>" +
                                "<tr><td colsapn='2'>Requested Shift : " + MailCovidData.RequestedShift + "</td></tr>" +
                                "<table>" +
                                "</body></html>";

                            SMS_EMail_Services sMS_EMail_Services = new SMS_EMail_Services();
                            SMS_EMail_Services.sendSingleSMS("9414112921", Message);
                            sMS_EMail_Services.sendEMail(MailCovidData.MailSubject, html, MailCovidData.ToMail, "");

                        }
                        else
                        {
                            IsValidOperattion = 0;
                        }
                    }
                    else
                    {
                        IsValidOperattion = 0;
                    }
                }
                else
                {
                    IsValidOperattion = 0;
                }
            }
            else
            {
                IsValidOperattion = 0;
            }


            return Json(IsValidOperattion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DepartmentCovidOptionalBooking()
        {
            CovidBooking oCovid = new CovidBooking();
            BookOnTicket cs = new BookOnTicket();
            DataSet DT = new DataSet();
            DT = cs.Get_UserCovidOptionalBookingList(oCovid);
            oCovid.lstOptionalCovidBooking = Globals.Util.GetListFromTable<OptionalCovidBooking>(DT.Tables[0]);
            return View(oCovid);
        }

        public ActionResult _DepartmentCovidOptionalBookingPartial(string RequestId)
        {
            CovidBooking oCovid = new CovidBooking();
            oCovid.RequestID = RequestId;
            BookOnTicket cs = new BookOnTicket();
            DataSet DT = new DataSet();
            DT = cs.Get_UserCovidOptionalBookingList(oCovid);
            oCovid.RequestID = RequestId;
            oCovid.TicketId = Encryption.encrypt(DT.Tables[0].Rows[0]["TicketId"].ToString());
            oCovid.ShiftName = DT.Tables[0].Rows[0]["ShiftName"].ToString();
            oCovid.ShiftId = Convert.ToInt32(DT.Tables[0].Rows[0]["ShiftId"].ToString());
            oCovid.FirstDate = DT.Tables[0].Rows[0]["FirstDate"].ToString();
            oCovid.SecondDate = DT.Tables[0].Rows[0]["SecondDate"].ToString();
            oCovid.ThirdDate = DT.Tables[0].Rows[0]["ThirdDate"].ToString();

            DataSet ds = new DataSet();
            cs.TicketID = Convert.ToInt64(DT.Tables[0].Rows[0]["TicketId"]);
            ds = cs.Select_CovidTicketData();
            oCovid.lstMemberDetails = new List<CovidMemberDetails>();
            oCovid.PlaceId = Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceId"]);
            oCovid.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountTobePaid"]);
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                oCovid.VehicleName = Convert.ToString(dr["vName"]);
                CovidMemberDetails memberdetails = new CovidMemberDetails();
                memberdetails.Name = Convert.ToString(dr["Name"]);
                memberdetails.Nationality = Convert.ToString(dr["Nationality"]);
                memberdetails.IdProof = Convert.ToString(dr["IdProof"]);
                memberdetails.NoofCamera = Convert.ToString(dr["NoofCamera"]);
                memberdetails.MemberFees = Convert.ToString(dr["MemberFee"]);
                memberdetails.CameraFees = Convert.ToString(dr["CameraFee"]);
                memberdetails.VehicleFees = Convert.ToString(dr["VehicleFee"]);
                memberdetails.BoardingVehicleFee = Convert.ToString(dr["BoardingVehicleFee"]);
                memberdetails.BoardingGuideFeeGSTAmount = Convert.ToString(dr["BoardingGuideFeeGSTAmount"]);
                memberdetails.BoardingVehicleFeeGstAmount = Convert.ToString(dr["BoardingVehicleFeeGstAmount"]);

                memberdetails.BoardingGuideFee = Convert.ToString(dr["BoardingGuideFee"]);
                memberdetails.Amount = Convert.ToString(dr["Amount"]);

                oCovid.lstMemberDetails.Add(memberdetails);

            }

            return PartialView("_DepartmentCovidOptionalBookingPartial", oCovid);
        }


        [HttpGet]
        public ActionResult CovidBooking(string ticketid)
        {
            CovidBooking oCovid = new CovidBooking();
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            bool flag = false;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                BookOnTicket cs = new BookOnTicket();

                string en = Encryption.encrypt(ticketid);
                cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));

                ds = cs.Select_CovidTicketData();

                List<CovidBooking> lstoCovid = new List<CovidBooking>();


                if (ds.Tables.Count > 0)
                {
                    oCovid.TicketId = ticketid;
                    oCovid.RequestID = Convert.ToString(ds.Tables[0].Rows[0]["RequestID"].ToString());
                    oCovid.PlaceName = Convert.ToString(ds.Tables[0].Rows[0]["PlaceName"]);
                    oCovid.PlaceId = Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"]);
                    oCovid.ShiftName = Convert.ToString(ds.Tables[0].Rows[0]["Shift"]);
                    oCovid.DateofArrival = "";//Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    oCovid.DateofArrival1 = Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    oCovid.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountTobePaid"]);


                    oCovid.lstMemberDetails = new List<CovidMemberDetails>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        oCovid.VehicleName = Convert.ToString(dr["vName"]);
                        CovidMemberDetails memberdetails = new CovidMemberDetails();
                        memberdetails.Name = Convert.ToString(dr["Name"]);
                        memberdetails.Nationality = Convert.ToString(dr["Nationality"]);
                        memberdetails.IdProof = Convert.ToString(dr["IdProof"]);
                        memberdetails.NoofCamera = Convert.ToString(dr["NoofCamera"]);
                        memberdetails.MemberFees = Convert.ToString(dr["MemberFee"]);
                        memberdetails.CameraFees = Convert.ToString(dr["CameraFee"]);
                        memberdetails.VehicleFees = Convert.ToString(dr["VehicleFee"]);
                        memberdetails.BoardingVehicleFee = Convert.ToString(dr["BoardingVehicleFee"]);
                        memberdetails.BoardingGuideFeeGSTAmount = Convert.ToString(dr["BoardingGuideFeeGSTAmount"]);
                        memberdetails.BoardingVehicleFeeGstAmount = Convert.ToString(dr["BoardingVehicleFeeGstAmount"]);

                        memberdetails.BoardingGuideFee = Convert.ToString(dr["BoardingGuideFee"]);
                        memberdetails.Amount = Convert.ToString(dr["Amount"]);

                        oCovid.lstMemberDetails.Add(memberdetails);

                    }

                    ViewBag.RQid = oCovid.RequestID;


                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(oCovid);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalSubmitForCovidOnlineBooking(CovidBooking covidBooking)
        {

            
            BookOnTicket cs = new BookOnTicket();

            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag == true)
            {

                // string ticketid = Encryption.encrypt(covidBooking.TicketId);
                Int64 TicketID = Convert.ToInt64(Encryption.decrypt(covidBooking.TicketId));
                string ReqId = RequestId();

                CovidBooking oBooking = new CovidBooking();
                oBooking.RequestID = ReqId;
                oBooking.DateofArrival = covidBooking.DateofArrival;
                oBooking.TicketId = TicketID.ToString();
                oBooking.ShiftName = covidBooking.ShiftName;

                ////check Available seats
                DataTable oDt = oBooking.CheckAvailableSeats(oBooking);
                DataTable dtf = cs.Select_BookedTicketStatus(TicketID);
                var isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.TicketID == TicketID).FirstOrDefault();
                if (isValidRequest != null)
                {
                    if (isValidRequest.COVIDStatus == 1 && isValidRequest.TicketMemberBordingStatus == 0)
                    {
                        DataTable dtChek = cs.Select_BookedTicketDFOStatusWise(TicketID);
                        if (Convert.ToInt32(dtChek.Rows[0]["isDFOApproved"]) == 0)
                        {
                            if (Convert.ToInt16(oDt.Rows[0]["AvailableSeats"]) > 0)
                            {
                                DataTable dt = oBooking.SaveCovidBooking(oBooking);
                                ReqId = Convert.ToString(dt.Rows[0]["RequestID"]);
                                return RedirectToAction("CovidSuccess", "BookOnlineTicket", new { rid = ReqId });
                            }
                            else
                            {
                                return RedirectToAction("Covidfail", "BookOnlineTicket", new { msg = "Seats not available" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("Covidfail", "BookOnlineTicket", new { msg = "RequestId is not valid for covid" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Covidfail", "BookOnlineTicket", new { msg = "RequestId is not valid for covid" });
                    }
                }
                else
                {
                    return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");

                }
            }
            return RedirectToAction("CovidBooking", new { ticketid = covidBooking.TicketId });
            //return RedirectToAction("BookOnlineTickets");
            //}
            //return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
        }
        public bool CheckCovidTicketDateAvailabilityDuration(CovidBooking oBook)
        {
            bool IsvalidDate = false;
            string[] dateArr = oBook.DateofArrival.Split('/');
            string[] MonthArr = { "07", "08", "09" };
            bool isValid = MonthArr.Contains(dateArr[1]);
            if (!isValid)
            {
                DateTime dateOfArrival = Convert.ToDateTime(oBook.DateofArrival);
                if (dateOfArrival > Convert.ToDateTime(DateTime.Now.Date.AddDays(1).ToString("dd/MM/yyyy")) && dateOfArrival < Convert.ToDateTime("01/07/2022"))
                {
                    IsvalidDate = true;
                }
                else
                {
                    IsvalidDate = false;
                }

            }
            else
            {
                IsvalidDate = false;
            }
            return IsvalidDate;
        }

        public string CheckCovidTicket(CovidBooking oBook)
        {
            if (Convert.ToDateTime(oBook.DateofArrival) >= Convert.ToDateTime("01/10/2020"))
            {
                Int64 TicketID = Convert.ToInt64(Encryption.decrypt(oBook.TicketId));

                CovidBooking oBooking = new CovidBooking();
                oBooking.DateofArrival = oBook.DateofArrival;
                oBooking.TicketId = TicketID.ToString();
                oBooking.ShiftName = oBook.ShiftName;
                oBooking.PlaceName = oBook.PlaceName;


                DataTable oDt = oBooking.CheckAvailableSeats(oBooking);
                if (Convert.ToInt16(oDt.Rows[0]["AvailableSeats"]) > 0)
                    return Convert.ToString(oDt.Rows[0]["AvailableSeats"]);
                else
                    return "0";
            }
            return "0";

        }
        public string CheckCovidTicketNew(CovidBooking oBook)
        {
            if (Convert.ToDateTime(oBook.DateofArrival) >= Convert.ToDateTime("01/10/2020"))
            {
                Int64 TicketID = Convert.ToInt64(Encryption.decrypt(oBook.TicketId));

                CovidBooking oBooking = new CovidBooking();
                oBooking.DateofArrival = oBook.DateofArrival;
                oBooking.TicketId = TicketID.ToString();
                oBooking.ShiftName = oBook.ShiftName;
                oBooking.PlaceName = oBook.PlaceName;


                DataTable oDt = oBooking.CheckAvailableSeatsForCovid(oBooking);
                if (Convert.ToInt16(oDt.Rows[0]["AvailableSeats"]) > 0)
                    return Convert.ToString(oDt.Rows[0]["AvailableSeats"]);
                else
                    return "0";
            }
            return "0";

        }


        public string CheckCovidHDFDTicket(CovidBooking oBook)
        {
            if (Convert.ToDateTime(oBook.DateofArrival) >= Convert.ToDateTime("01/10/2020"))
            {
                Int64 TicketID = Convert.ToInt64(Encryption.decrypt(oBook.TicketId));

                CovidBooking oBooking = new CovidBooking();
                oBooking.DateofArrival = oBook.DateofArrival;
                oBooking.TicketId = TicketID.ToString();
                oBooking.ShiftName = oBook.ShiftName;
                oBooking.PlaceName = oBook.PlaceName;


                DataTable oDt = oBooking.CheckAvailableSeatsForHDFD(oBooking);
                if (Convert.ToInt16(oDt.Rows[0]["AvailableSeats"]) > 0)
                    return Convert.ToString(oDt.Rows[0]["AvailableSeats"]);
                else
                    return "0";
            }
            return "0";

        }

        public ActionResult Covidfail(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }

        public ActionResult CovidSuccess(string rid)
        {
            ViewBag.RequestId = rid;
            return View();
        }

        [HttpGet]
        public ActionResult CovidBookingHDFD(string ticketid)
        {
            CovidBooking oCovid = new CovidBooking();
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            bool flag = false;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                BookOnTicket cs = new BookOnTicket();

                string en = Encryption.encrypt(ticketid);
                cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));

                ds = cs.Select_CovidTicketData();

                List<CovidBooking> lstoCovid = new List<CovidBooking>();


                if (ds.Tables.Count > 0)
                {
                    oCovid.TicketId = ticketid;
                    oCovid.RequestID = Convert.ToString(ds.Tables[0].Rows[0]["RequestID"].ToString());
                    oCovid.PlaceName = Convert.ToString(ds.Tables[0].Rows[0]["PlaceName"]);
                    oCovid.PlaceId = Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"]);
                    oCovid.ShiftName = Convert.ToString(ds.Tables[0].Rows[0]["Shift"]);
                    oCovid.DateofArrival = "";//Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    oCovid.DateofArrival1 = Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    oCovid.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountTobePaid"]);


                    oCovid.lstMemberDetails = new List<CovidMemberDetails>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        oCovid.VehicleName = Convert.ToString(dr["vName"]);
                        CovidMemberDetails memberdetails = new CovidMemberDetails();
                        memberdetails.Name = Convert.ToString(dr["Name"]);
                        memberdetails.Nationality = Convert.ToString(dr["Nationality"]);
                        memberdetails.IdProof = Convert.ToString(dr["IdProof"]);
                        memberdetails.NoofCamera = Convert.ToString(dr["NoofCamera"]);
                        memberdetails.MemberFees = Convert.ToString(dr["MemberFee"]);
                        memberdetails.CameraFees = Convert.ToString(dr["CameraFee"]);
                        memberdetails.VehicleFees = Convert.ToString(dr["VehicleFee"]);
                        memberdetails.BoardingVehicleFee = Convert.ToString(dr["BoardingVehicleFee"]);
                        memberdetails.BoardingGuideFeeGSTAmount = Convert.ToString(dr["BoardingGuideFeeGSTAmount"]);
                        memberdetails.BoardingVehicleFeeGstAmount = Convert.ToString(dr["BoardingVehicleFeeGstAmount"]);

                        memberdetails.BoardingGuideFee = Convert.ToString(dr["BoardingGuideFee"]);
                        memberdetails.Amount = Convert.ToString(dr["Amount"]);

                        oCovid.lstMemberDetails.Add(memberdetails);

                    }

                    ViewBag.RQid = oCovid.RequestID;


                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(oCovid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalSubmitForCovidOnlineBookingHDFD(CovidBooking covidBooking)
        {


            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag == true)
            {

                // string ticketid = Encryption.encrypt(covidBooking.TicketId);
                Int64 TicketID = Convert.ToInt64(Encryption.decrypt(covidBooking.TicketId));
                string ReqId =  RequestId();
                BookOnTicket cs = new BookOnTicket();
                CovidBooking oBooking = new CovidBooking();
                oBooking.RequestID = ReqId;
                oBooking.DateofArrival = covidBooking.DateofArrival;
                oBooking.TicketId = TicketID.ToString();
                oBooking.ShiftName = covidBooking.ShiftName;

                ////check Available seats
                DataTable oDt = oBooking.CheckAvailableSeats(oBooking);

                DataTable dtf = cs.Select_BookedTicketStatus(TicketID);
                var isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.TicketID == TicketID).FirstOrDefault();
                if (isValidRequest != null)
                {
                    if (isValidRequest.COVIDStatus == 1 && isValidRequest.TicketMemberBordingStatus == 0)
                    {
                        if (Convert.ToInt16(oDt.Rows[0]["AvailableSeats"]) > 0)
                        {
                            DataTable dt = oBooking.SaveCovidBooking(oBooking);
                            ReqId = Convert.ToString(dt.Rows[0]["RequestID"]);
                            return RedirectToAction("CovidSuccess", "BookOnlineTicket", new { rid = ReqId });
                        }
                        else
                        {
                            return RedirectToAction("Covidfail", "BookOnlineTicket", new { msg = "Seats not available" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Covidfail", "BookOnlineTicket", new { msg = "RequestId is not valid for covid" });
                    }
                }
                else
                {
                    return RedirectToAction("CovidBookingHDFD", "BookOnlineTicket");
                }
            }
            return RedirectToAction("CovidBookingHDFD", new { ticketid = covidBooking.TicketId });
            //return RedirectToAction("BookOnlineTickets");

        }


        [HttpGet]
        public FileResult PrintWildLifeTicket(string ticketid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            bool flag = false;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                BookOnTicket cs = new BookOnTicket();

                string en = Encryption.encrypt(ticketid);
                cs.TicketID = Convert.ToInt64(Encryption.decrypt(ticketid));

                //#region "Delay Download Ticket for one hour"
                //DataTable oDelayDataTable = cs.Select_DelayStatus(cs.TicketID);
                //if (Convert.ToString(oDelayDataTable.Rows[0]["Status"]) == "True")
                //    flag = true;
                //else
                //    flag = false;

                //#endregion


                //if (flag == true)
                //{
               
                ReSchedule reSchedule = new ReSchedule();
                DataTable dt = reSchedule.CheckIsReScheduledTicketForPrint(cs.TicketID, "HD/FD ReSchedule");
                bool isReSchedule = Convert.ToBoolean(dt.Rows[0]["ReScheduleExists"]);
                if (isReSchedule == false)
                {
                    ds = cs.Select_TicketData_For_Print();
                }                    
                else
                {
                    ds = reSchedule.Select_ReScheduledTicket_ToPrint(cs.TicketID);
                }
                DataTable dtTC = new DataTable();
                dtTC = cs.Select_TermandConditionbyTicketId(Convert.ToInt64(Encryption.decrypt(ticketid)));
                string filepath = "";
                if (isReSchedule == false)
                {
                    filepath = htmlToPdfDownloadTickets.WildlifeDownloadPdf(ds, dtTC);
                }
                else
                {
                    filepath = reSchedule.WildlifeDownloadReScheduledTicketPdf(ds, dtTC);
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
                //}

                //else
                //{
                //    ////redirect to delay page
                //    Response.Redirect("~/DelayTicket.html");

                //    //RedirectToAction("DelayTicket", "SessionExpire");
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpGet]
        public ActionResult RefundRequestMemberWise(string ticketid)
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

            TempData["ListCurrentBookingMemberDetails"] = List;
            return View(cs);
        }

        [HttpPost]
        public ActionResult RefundRequestMemberWise(BookOnTicket cs)
        {
            DataTable DT = new DataTable();

            DT = cs.SubmitFor_BookTicket_ForRefundProcessMemberWise(cs);
            if (DT.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                #endregion


                TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }

            return RedirectToAction("BookOnlineTicket");


        }

        [HttpGet]
        public ActionResult RefundRequest(string ticketid)
        {
            BookOnTicket cs = new BookOnTicket();
            //#region "Delay Download Ticket for one hour"
            //bool flag = false;
            //DataTable oDelayDataTable = cs.Select_DelayStatus(cs.TicketID);
            //if (Convert.ToString(oDelayDataTable.Rows[0]["Status"]) == "True")
            //    flag = true;
            //else
            //    flag = false;
            //#endregion
            //if (flag == true)
            //{
            DataTable DT = new DataTable();

            DT = cs.Get_BookTicket_ForRefundProcess(Encryption.decrypt(ticketid));
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
            DataTable DT = new DataTable();

            DT = cs.SubmitFor_BookTicket_ForRefundProcess(cs);
            if (DT.Rows.Count > 0)
            {
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                #region Email and SMS

                objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                #endregion


                TempData["RowCheck"] = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.";
            }

            return RedirectToAction("BookOnlineTicket");


        }

        [HttpGet]
        public ActionResult Refund(string ticketid)
        {
            BookOnTicket cs = new BookOnTicket();
            DataTable DT = new DataTable();

            DT = cs.Get_BookTicket_ForRefund(Encryption.decrypt(ticketid));
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
            return View(cs);


        }
        [HttpPost]
        public JsonResult Refund(BookOnTicket cs)
        {
            DataTable dtf = cs.Select_BookedTicketStatus(cs.TicketID, cs.RequestId);
            var isValidRequest = new BookOnTicket();
            DataTable DT = new DataTable();
            string msg = string.Empty;
            isValidRequest = Globals.Util.GetListFromTable<BookOnTicket>(dtf).Where(x => x.RequestedId == cs.RequestId).FirstOrDefault();
            if (isValidRequest != null)
            {
                if (isValidRequest.COVIDStatus == 1 && isValidRequest.TicketMemberBordingStatus == 0 && isValidRequest.RefundStatus == 0)
                {
                    DT = cs.SubmitFor_BookTicket_ForRefund(cs);
                    if (DT.Rows.Count > 0)
                    {
                        SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                        #region Email and SMS

                        objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
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


        #region Wildlife Booking VIP and Emergency
        public ActionResult BookOnlineTickets()
        {

            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).AddHours(-10).ToString("yyyy/MM/dd");//Change date 10 AM
            ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
            var MemberList = new List<BookOnTicket>();
            try
            {


                Session.Remove("AvaliableTicket");
                Session.Remove("VFeeTigerProject");
                Session.Remove("VFeeSurcharge");
                Session.Remove("TotaVechileFees");
                Session.Remove("totalprice");
                Session.Remove("RequestId");
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtPlace = new DataTable();
                for (int i = 0; i < 20; i++)
                {
                    MemberList.Add(new BookOnTicket());
                }
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicketForVIPSeats("GENRALBOOKING", 5);

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),

                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString())

                    });
                }
                ViewData["ticketlist"] = ticketList;
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                ////New change for emitra Kiosk 06-09-2018
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
                    //added by rajveer bcz kiosk user not immplemet in wildlife only dept user working so kiosk and dept usr working is same
                    Session["IsDepartmentalKioskUser"] = true;

                    #region KioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion

                }

                else
                {
                    #region Place
                    dtPlace = Bok.Select_Place(UserID);
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                //******************************************************************************


                #region OnlineBookingPopUp Developed by Rajveer

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "OnlineBooking";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;

                }
                #endregion

                // Session["IsDepartmentalKioskUser"] = "True";


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(MemberList);
        }

        public JsonResult BindShiftByPlaceZoneForVIPSeats(int placeID, int Zone)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();
            try
            {
                BookOnTicket bkt = new BookOnTicket();
                DataTable dtShift = new DataTable();

                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    // === set default date and shift as per database 
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;
                    DataTable DT_BoardingDuration = new DataTable();
                    DT_BoardingDuration = obj.GetBoardingDurationForVIPSeats(Convert.ToString(placeID), Convert.ToString(Zone), "GetBoardingDurationForVIPSeatsWithZone");

                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                    {
                        // =========== EVENING_SHIFT
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "2";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "False",
                                isEvening = "True",
                                isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                            });
                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                    {
                        // =========== MORNING_SHIFT                       
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "1";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "True",
                                isEvening = "False",
                                isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                            });
                        }
                    }

                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"]) == true)
                    {
                        // =========== MORNING_SHIFT                       
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingHalfDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingHalfDayEveningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            if (fees.Count > 0 && (fees.FirstOrDefault().isMorning == "True" || fees.FirstOrDefault().isEvening == "True"))
                            {
                                fees.FirstOrDefault().isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"]));
                            }
                            else
                            {
                                SHIFT_TYPE = "4";
                                fees.Add(new BookOnTicket()
                                {
                                    isMorning = "False",
                                    isEvening = "False",
                                    isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                    isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                                });
                            }
                        }
                    }
                    // === set default date and shift as per database  
                    #endregion

                }
                else
                {
                    #region Citizen User
                    bkt.PlaceId = Convert.ToInt64(placeID);
                    bkt.ZoneId = Convert.ToInt64(Zone);
                    dtShift = bkt.Select_Shift_By_PlacesZones();
                    if (dtShift.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtShift.Rows)
                        {
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = dr["isMorning"].ToString(),
                                isEvening = dr["isEvening"].ToString(),
                                isFullDay = dr["isFullDay"].ToString(),

                            });
                        }
                    }
                    else
                    {
                        fees.Add(new BookOnTicket()
                        {
                            isMorning = "",
                            isEvening = "",
                            isFullDay = "",
                        });
                    }
                    #endregion

                }
                DataTable dta = new DataTable();
                bkt.PlaceId = Convert.ToInt64(placeID);
                dta = bkt.GetAccomodationType();
                ViewBag.Accomo = dta;
                foreach (System.Data.DataRow dr in ViewBag.Accomo.Rows)
                {
                    Accomodation.Add(new SelectListItem { Text = @dr["RoomType"].ToString(), Value = @dr["AccommodationID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = fees, list2 = Accomodation, list3 = DateTime.Now.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet });
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckTicketAvailabilityForVIPSeats(int placeID, string arrivaldate, string shifttime, int Zone, int vehicleID)
        {
            string strStatus = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtTicketdetails = new DataTable();
            try
            {
                BookOnTicket bkt = new BookOnTicket();
                bkt.PlaceId = placeID;
                bkt.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                bkt.ShiftTime = shifttime;
                bkt.ZoneId = Zone;
                bkt.vehicleID = vehicleID;
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false")
                //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    bkt.KioskUserId = "0";

                }
                else
                {
                    bkt.KioskUserId = Session["KioskUserId"].ToString();
                }

                dtTicketdetails = bkt.CheckTicketAvailabilityForVIP();
                if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
                {
                    if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
                    {
                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                        Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
                        Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
                        Session["VExtraFee"] = dtTicketdetails.Rows[0][3].ToString();
                        Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                        strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"] + "#" + Session["VExtraFee"] + "#" + Convert.ToInt32(dtTicketdetails.Rows[0][4]);
                    }
                    else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
                    {
                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                        strStatus = Session["AvaliableTicket"] + "#";
                    }
                    else
                    {
                        Session.Remove("AvaliableTicket");
                        Session.Remove("VFeeTigerProject");
                        Session.Remove("VFeeSurcharge");
                        Session.Remove("TotaVechileFees");
                        Session.Remove("VExtraFee");
                        strStatus = "0#";
                    }
                }
                else
                {
                    Session.Remove("AvaliableTicket");
                    Session.Remove("VFeeTigerProject");
                    Session.Remove("VFeeSurcharge");
                    Session.Remove("TotaVechileFees");
                    Session.Remove("VExtraFee");
                    strStatus = "0#";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(strStatus);
        }
        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="placeID"></param>
        /// <param name="Zone"></param>
        /// <returns></returns>
        /// 

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult vehicleByCategoryForVIPSeat(int vehicleCatID, Int64 placeID, int Zone, string ShiftType)
        {
            BookOnTicket bkt = new BookOnTicket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                bkt.PlaceId = placeID;
                bkt.ZoneId = Zone;
                // cst.ShiftType = Shift;
                dt = bkt.Select_vehicleForVIP(Convert.ToInt64(vehicleCatID), Convert.ToInt64(ShiftType));
                ViewBag.vname = dt;
                foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                {
                    vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new SelectList(vehicle, "Value", "Text"));
        }


        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult SelectFeeForVIPSeats(Int64 placeID, string nationality, string memberType, int vehicleID, int ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();
            try
            {
                DataTable dt = new DataTable();
                BookOnTicket cst = new BookOnTicket();
                cst.PlaceId = placeID;
                cst.MemberNationality = nationality;
                cst.MemberType = memberType;
                cst.vehicleID = vehicleID;
                dt = cst.SelectMemberFeesForVIPSeats(ShiftType);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new BookOnTicket()
                        {
                            MemberFees_TigerProject = Convert.ToDecimal(dr["MFees_TigerProject"].ToString()),
                            MemberFees_Surcharge = Convert.ToDecimal(dr["MFees_Surcharge"].ToString()),
                            TRDF = Convert.ToDecimal(dr["TRDF"].ToString()),
                            CameraFees_TigerProject = Convert.ToDecimal(dr["CFees_TigerProject"].ToString()),
                            CameraFees_Surcharge = Convert.ToDecimal(dr["CFees_Surcharge"].ToString()),

                            TotalPerMemberFees = (Convert.ToDecimal(dr["MFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["MFees_Surcharge"].ToString()) + Convert.ToDecimal(dr["TRDF"].ToString()) + Convert.ToDecimal(dr["Vehicle_TRDF"]) + Convert.ToDecimal(dr["GuidFee_TRDF"])),

                            TotalPerMemberCameraFees = (Convert.ToDecimal(dr["CFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["CFees_Surcharge"].ToString())),

                            BoardingVehicleFee = Convert.ToDecimal(dr["BoardingVehicleFee"]),
                            BoardingVehicleFeeGSTPercentage = Convert.ToDecimal(dr["BoardingVehicleFeeGSTPercentage"]),
                            BoardingVehicleFeeGSTAmount = Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]),


                            BoardingGuideFee = Convert.ToDecimal(dr["BoardingGuideFee"]),
                            BoardingGuideFeeGSTPercentage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]),
                            BoardingGuideFeeGSTAmount = Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),

                            TotalBoardingFee = Convert.ToDecimal(dr["BoardingVehicleFee"]) + Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]) + Convert.ToDecimal(dr["BoardingGuideFee"]) + Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),


                            GSTMessage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]) == 0 ? "" : Convert.ToString(dr["BoardingGuideFeeGSTPercentage"]) + " % GST  applicable on guide fees And " + Convert.ToString(dr["BoardingVehicleFeeGSTPercentage"]) + "% applicable on vehicle rent",

                            Vehicle_TRDF = Convert.ToDecimal(dr["Vehicle_TRDF"]),

                            GuidFee_TRDF = Convert.ToDecimal(dr["GuidFee_TRDF"]),

                        });
                    }
                }
                else
                {
                    fees.Add(new BookOnTicket()
                    {
                        MemberFees_TigerProject = Convert.ToDecimal(0),
                        MemberFees_Surcharge = Convert.ToDecimal(0),
                        TRDF = Convert.ToDecimal(0),
                        CameraFees_TigerProject = Convert.ToDecimal(0),
                        CameraFees_Surcharge = Convert.ToDecimal(0),
                        TotalPerMemberFees = Convert.ToDecimal(0),
                        TotalPerMemberCameraFees = Convert.ToDecimal(0),
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(fees);
        }



        [ValidateAntiForgeryToken]
        public ActionResult FinalSubmitForVIPSeats(List<MemberInfo> lstMemberInfo, List<MemberDetailViewModel> lstMembers, BookOnTicket cs, string Command, FormCollection form, string Message)
        {

            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag)
            {


                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                int rownumber = 0;
                int rowcount = 0;
                decimal finalAmount = 0;
                try
                {

                    foreach (var item in lstMemberInfo)
                    {
                        if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                        {
                            rownumber = Convert.ToInt16(item.MemberSLNo);
                            rowcount++;
                        }
                    }
                    if (rownumber != rowcount)
                    {
                        TempData["RowCheck"] = "Enter member details continiously";
                        return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                    }
                    if (Command == "Submit")
                    {
                        #region MemberInfo
                        DataTable dtMemberInfo = new DataTable();
                        long placeId = 0;
                        int vehicleId = 0;
                        int shifttype = 0;
                        if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                        {
                            placeId = Convert.ToInt64(form["ddl_place"].ToString());
                        }
                        if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                        {
                            shifttype = Convert.ToInt32(form["ddl_Shift"].ToString());
                        }
                        if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                        {
                            vehicleId = Convert.ToInt32(form["ddl_vehicle"].ToString());
                        }

                        dtMemberInfo = MemberInformations(lstMembers, placeId, vehicleId, shifttype); //MemberInformation(lstMemberInfo);
                        if (dtMemberInfo.Rows.Count == 0)
                        {
                            TempData["RowCheck"] = "Enter member details";
                            return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                        }
                        else
                        {
                            if (dtMemberInfo.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                                {
                                    finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                                }

                                if (Session["VExtraFee"] != null && Convert.ToDecimal(Session["VExtraFee"]) > 0)
                                {
                                    finalAmount += Convert.ToDecimal(Session["VExtraFee"]);
                                }
                            }
                        }

                        #endregion

                        #region Submission

                        //Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                        Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                        //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" || Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" )
                        //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" ))
                        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        {
                            cs.KioskUserId = "0";

                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();
                        }
                        //  cs.SsoToken = Request.Cookies["RAJSSO"].Value;
                        cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                        //System.Threading.Thread.Sleep(10000);    
                        //cs.RequestId = DateTime.Now.Ticks.ToString();
                        //Session["RequestId"] = cs.RequestId;
                        //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                        Session["RequestId"] = RequestId();
                        cs.RequestId = Session["RequestId"].ToString();
                        //***************************************************************************************
                        if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                        {
                            cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                        }
                        else
                        {
                            cs.PlaceId = 0;
                        }
                        if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                        {
                            cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                        }
                        else
                        {
                            cs.ZoneId = 0;
                        }
                        if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                        {
                            cs.ShiftTime = form["ddl_Shift"].ToString();
                        }
                        else
                        {
                            cs.ShiftTime = "";
                        }
                        cs.TotalMember = lstMembers.Where(x => x.LeaderName != "").Sum(x => x.TotalPersons); //Convert.ToInt32(rowcount);

                        if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                        {

                            cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                            #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                            //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                            //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                            //{
                            //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                            //}
                            //else
                            //{
                            //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                            //}

                            #endregion

                            DataTable DTCheckBooking = new DataTable();
                            #region Restrict Months
                            DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                            if (DTCheckBooking.Rows.Count > 0)
                            {
                                if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                                {
                                    TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                                    return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                                }

                            }
                            #endregion

                            #region Get Open Days by rajveer


                            DataTable GetDaysDataTable = new DataTable();
                            GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                            long AddDaysVal = 0;
                            if (GetDaysDataTable.Rows.Count > 0)
                            {
                                AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                            }
                            #endregion
                            //DateTime expiryDate = DateTime.Today.AddDays(89);
                            DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                            if (cs.ArrivalDate > expiryDate)
                            {
                                //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                                TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                                return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                            }

                            //Kiosk User Restrictation
                            if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                            {
                                //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                                TempData["datevalidation"] = "Arrival date should not be more than Today";
                                return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                            }
                        }
                        if (TempData["DurationTo"] != null)
                        {

                            DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                            if (cs.ArrivalDate > MaxDate)
                            {
                                TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                                return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                            }
                        }
                        if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                        {
                            cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                            Session["VehicleId"] = cs.vehicleID;
                        }
                        else
                        {
                            cs.vehicleID = 0;
                        }
                        if (Session["VFeeTigerProject"] != null)
                        {

                            cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_TigerProject = 0;
                        }
                        if (Session["VFeeSurcharge"] != null)
                        {

                            cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_Surcharge = 0;
                        }
                        if (Session["TotaVechileFees"] != null)
                        {

                            cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                        }
                        else
                        {
                            cs.VehicleFees_Total = 0;
                        }
                        if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                        {
                            cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                        }
                        else
                        { cs.AccomoID = 0; }
                        if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                        {
                            cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                        }
                        else
                        { cs.TotalRoom = 0; }
                        if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                        {
                            cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                        }
                        else
                        { cs.RoomCharge = 0; }

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
                        cs.IPAddress = result;
                        DataTable dts = new DataTable();

                        DataTable dtcheckTicket = new DataTable();
                        string strcheckTicket = string.Empty;
                        dtcheckTicket = cs.CheckTicketAvailabilityForVIP();
                        strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                        if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                        {
                            dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                            dtMemberInfo.AcceptChanges();
                            int indiancount = lstMembers.Where(x => x.NationalityId == "1").FirstOrDefault().TotalPersons;
                            int foreignercount = lstMembers.Where(x => x.NationalityId == "2").FirstOrDefault().TotalPersons;
                            int indianstudentcount = lstMembers.Where(x => x.NationalityId == "3").FirstOrDefault().TotalPersons;
                            dts = cs.Submit_TicketDetailsForVIPSeats(dtMemberInfo, finalAmount, indiancount, foreignercount, indianstudentcount);

                        }
                        else
                        {
                            TempData["RowCheck"] = "Ticket not avaliable";
                            return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                        }
                        if (dts.Rows.Count > 0)
                        {
                            decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                            Session["totalprice"] = finalAmnt;
                            if (Session["totalprice"].ToString() == "0")
                            {
                                TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                                return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                            }
                        }
                        else
                        {
                            TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                            return RedirectToAction("BookOnlineTickets", "BookOnlineTicket");
                        }


                        #region Add Kiosk User by Rajveer
                        EducationTours edu = new EducationTours();
                        edu.Location = Convert.ToInt64(cs.PlaceId);
                        DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                            {
                                Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                            }
                        }

                        // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                        if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                        {
                            //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                            KioskPaymentDetails _obj = new KioskPaymentDetails();
                            _obj.ModuleId = 1;
                            _obj.ServiceTypeId = 1;
                            _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                            _obj.SubPermissionId = 1;
                            _obj.RequestedId = Convert.ToString(cs.RequestId);
                            DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                            if (dtKiosk.Rows.Count > 0)
                            {
                                _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                                _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                                _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                                _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                                _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                                _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                                ViewBag.ViewModel = dts.AsEnumerable();
                                return PartialView("KioskPaymentDetailWildlife", _obj);
                            }
                        }
                        else
                        {
                            ViewData.Model = dts.AsEnumerable();
                            Session["PaymentData"] = dts.AsEnumerable();
                            return View("OnlineTicketPaymentKiosk");
                        }
                        #endregion


                        // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                //return View("OnlineTicketPayment");//Comment By Rajveer Kiosk User
            }
            return RedirectToAction("BookOnlineTickets");
        }


        public ActionResult _MemberDetail(int vehicleCatID, Int64 placeID, int ShiftType)
        {
            List<MemberDetailViewModel> lstMembers = new List<MemberDetailViewModel>();
            List<BookOnTicket> lstTickets = CalculateFees(placeID, "2", vehicleCatID, ShiftType);
            #region Add Member details
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Indian",
                        NationalityId = "1",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Passport", Value = "1" },
                        new SelectListItem { Text = "Aadhar", Value = "2" },
                        new SelectListItem { Text = "Driving Licence", Value = "3" },
                        new SelectListItem { Text = "Voter ID", Value = "4" },
                        new SelectListItem { Text = "PAN Card", Value = "5" },
                        new SelectListItem { Text = "Office ID", Value = "6" }


                     };
                    ViewBag.IndianId = ObjList;
                }
                if (i == 1)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Foreigner",
                        NationalityId = "2",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Passport", Value = "1" },
                    };
                    ViewBag.ForeignerId = ObjList;
                }
                if (i == 2)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Student",
                        NationalityId = "3",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Student ID", Value = "7" },
                    };
                    ViewBag.StudentId = ObjList;
                }

            }

            return PartialView(lstMembers);

            #endregion
        }

        public List<BookOnTicket> CalculateFees(Int64 placeID, string memberType, int vehicleID, int ShiftType)
        {
            List<BookOnTicket> fees = new List<BookOnTicket>();
            DataSet ds = new DataSet();
            BookOnTicket cst = new BookOnTicket();
            cst.PlaceId = placeID;
            cst.MemberType = memberType;
            cst.vehicleID = vehicleID;
            ds = cst.SelectFeesForVIPSeats(ShiftType);
            for (int i = 0; i < 3; i++)
            {
                if (ds.Tables[i].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[i].Rows)
                    {
                        string nationality = "";
                        if (i == 0)
                            nationality = "1";
                        else if (i == 1)
                            nationality = "2";
                        else if (i == 2)
                            nationality = "3";
                        fees.Add(new BookOnTicket()
                        {

                            MemberFees_TigerProject = Convert.ToDecimal(dr["MFees_TigerProject"].ToString()),
                            MemberFees_Surcharge = Convert.ToDecimal(dr["MFees_Surcharge"].ToString()),
                            TRDF = Convert.ToDecimal(dr["TRDF"].ToString()),
                            CameraFees_TigerProject = Convert.ToDecimal(dr["CFees_TigerProject"].ToString()),
                            CameraFees_Surcharge = Convert.ToDecimal(dr["CFees_Surcharge"].ToString()),

                            TotalPerMemberFees = (Convert.ToDecimal(dr["MFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["MFees_Surcharge"].ToString()) + Convert.ToDecimal(dr["TRDF"].ToString()) + Convert.ToDecimal(dr["Vehicle_TRDF"]) + Convert.ToDecimal(dr["GuidFee_TRDF"])),

                            TotalPerMemberCameraFees = (Convert.ToDecimal(dr["CFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["CFees_Surcharge"].ToString())),

                            BoardingVehicleFee = Convert.ToDecimal(dr["BoardingVehicleFee"]),
                            BoardingVehicleFeeGSTPercentage = Convert.ToDecimal(dr["BoardingVehicleFeeGSTPercentage"]),
                            BoardingVehicleFeeGSTAmount = Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]),


                            BoardingGuideFee = Convert.ToDecimal(dr["BoardingGuideFee"]),
                            BoardingGuideFeeGSTPercentage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]),
                            BoardingGuideFeeGSTAmount = Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),

                            TotalBoardingFee = Convert.ToDecimal(dr["BoardingVehicleFee"]) + Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]) + Convert.ToDecimal(dr["BoardingGuideFee"]) + Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),


                            GSTMessage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]) == 0 ? "" : Convert.ToString(dr["BoardingGuideFeeGSTPercentage"]) + " % GST  applicable on guide fees And " + Convert.ToString(dr["BoardingVehicleFeeGSTPercentage"]) + "% applicable on vehicle rent",

                            Vehicle_TRDF = Convert.ToDecimal(dr["Vehicle_TRDF"]),

                            GuidFee_TRDF = Convert.ToDecimal(dr["GuidFee_TRDF"]),

                            MemberNationality = nationality
                        });
                    }
                }
                else
                {
                    fees.Add(new BookOnTicket()
                    {
                        MemberFees_TigerProject = Convert.ToDecimal(0),
                        MemberFees_Surcharge = Convert.ToDecimal(0),
                        TRDF = Convert.ToDecimal(0),
                        CameraFees_TigerProject = Convert.ToDecimal(0),
                        CameraFees_Surcharge = Convert.ToDecimal(0),
                        TotalPerMemberFees = Convert.ToDecimal(0),
                        TotalPerMemberCameraFees = Convert.ToDecimal(0),
                    });
                }
            }

            return fees;
        }



        #region MemberDetail By Suraj Singh
        public DataTable MemberInformations(List<MemberDetailViewModel> lstMemberInfo, Int64 placeID, int vehicleID, int ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");

            List<BookOnTicket> lstTicketInfo = CalculateFees(placeID, "2", vehicleID, ShiftType);
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(String));
                objDt2.Columns.Add("FeeTigerProject", typeof(String));
                objDt2.Columns.Add("FeeSurcharge", typeof(String));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(String));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(String));
                objDt2.Columns.Add("TRDF", typeof(String));

                objDt2.Columns.Add("TotalFeePerMember", typeof(String));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(String));
                objDt2.Columns.Add("TotalCamera", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(String));
                objDt2.Columns.Add("BoardingGuideFee", typeof(String));
                objDt2.Columns.Add("TotalBoardingFee", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(String));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(String));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(String));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(String));
                objDt2.Columns.Add("PNR_NO", typeof(String));
                objDt2.Columns.Add("Seat_NO", typeof(String));
                objDt2.Columns.Add("Room_No", typeof(String));


                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();

                    if (item.LeaderName != string.Empty && item.LeaderName != null && item.IdType != "0" && item.IdNo != string.Empty && item.IdNo != null && item.NationalityId != "0")
                    {

                        dr["Name"] = item.LeaderName;
                        dr["Gender"] = "1";
                        dr["Nationality"] = item.NationalityId;
                        dr["IDType"] = item.IdType;
                        dr["IDNo"] = item.IdNo;
                        dr["MemberType"] = 2;
                        dr["FeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().MemberFees_TigerProject * item.TotalPersons;
                        dr["FeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().MemberFees_Surcharge * item.TotalPersons;
                        dr["CameraFeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().CameraFees_TigerProject * item.TotalPersons;
                        dr["CameraFeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().CameraFees_Surcharge * item.TotalPersons;
                        dr["TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TRDF * item.TotalPersons;

                        dr["BoardingVehicleFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFee * item.TotalPersons;
                        dr["BoardingGuideFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFee * item.TotalPersons;

                        dr["BoardingVehicleFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTPercentage * item.TotalPersons;
                        dr["BoardingGuideFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTPercentage * item.TotalPersons;

                        dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount * item.TotalPersons);
                        dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTAmount * item.TotalPersons);

                        //lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTAmount);

                        dr["TotalFeePerMember"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberFees * item.TotalPersons;
                        decimal CemaraFess = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees;
                        dr["TotalFeePerCamera"] = CemaraFess * item.NoOfVideoCamera;
                        // calculate camera fees
                        //decimal percamerafees = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees;
                        int totalCamera = item.NoOfVideoCamera;
                        dr["TotalCamera"] = totalCamera;

                        // END

                        dr["TotalBoardingFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee * item.TotalPersons;
                        // calculate total fees
                        decimal totalAmount = (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberFees * item.TotalPersons) + (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees * totalCamera)) + (Math.Round(Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee))));

                        dr["FinalAmountTobePaid"] = totalAmount;
                        // END
                        dr["Vehicle_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().Vehicle_TRDF * item.TotalPersons;
                        dr["GuidFee_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().GuidFee_TRDF * item.TotalPersons;
                        dr["PNR_NO"] = string.IsNullOrEmpty(item.PNR_NO) ? "" : item.PNR_NO;
                        dr["Seat_NO"] = string.IsNullOrEmpty(item.Seat_NO) ? "" : item.Seat_NO;
                        dr["Room_No"] = string.IsNullOrEmpty(item.Room_No) ? "" : item.Room_No;
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
        #endregion

        #endregion


        #region Wildlife Booking VIP and Emergency

        public ActionResult WildLifeTicketHistoryForVIP(string ActionValues, int row = 5)
        {

            ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var MemberList = new List<BookOnTicket>();
            try
            {
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicketForVIPSeats(ActionValues, row);

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),
                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString()),
                        COVIDStatus = Convert.ToInt32(dr["COVIDStatus"]),
                        TicketMemberBordingStatus = Convert.ToInt32(dr["TicketMemberBordingStatus"]),
                        isDFOApproved = Convert.ToInt32(dr["isDFOApproved"]),
                        RefundStatus = Convert.ToInt32(dr["RefundStatus"]),
                        PlaceId = Convert.ToInt32(dr["PlaceId"]),
                        RequestedId = "" + dr["RequestedId"],
                        OldRequestID = "" + (dr["OldRequestID"]==null?"":dr["OldRequestID"].ToString())

                    });
                }

                ViewData["ticketlist"] = ticketList;
                DateTime mileStoneFromDate = DateTime.ParseExact("10/01/2022", "MM/dd/yyyy", null);
                ViewBag.mileStoneFromDate = mileStoneFromDate;

                DateTime today = DateTime.Today; // As DateTime
                string s_today = today.ToString("MM/dd/yyyy"); // As String
                DateTime cDate = DateTime.ParseExact(s_today, "MM/dd/yyyy", null);
                ViewBag.CurrentDate = cDate;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }
        public ActionResult WildlifeBooking(string PlaceName = "")
        {
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).AddHours(-10).ToString("yyyy/MM/dd");//Change date 10 AM
            ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
            var MemberList = new List<BookOnTicket>();
            try
            {
                //string RTDC = Encryption.encrypt("RTDC");
                //string POW = Encryption.encrypt("POW");

                #region Lable Name Deside
                if (!string.IsNullOrEmpty(PlaceName))
                {
                    Session["PlaceLbl"] = Encryption.decrypt(Convert.ToString(PlaceName));
                }

                #endregion


                Session.Remove("AvaliableTicket");
                Session.Remove("VFeeTigerProject");
                Session.Remove("VFeeSurcharge");
                Session.Remove("TotaVechileFees");
                Session.Remove("totalprice");
                Session.Remove("RequestId");

                //////////hari krishan
                Session["CheckAvailabilityTimeSessionStartTatkal"] = null;
                Session["CheckAvailabilityTimeSessionEndTatkal"] = null;
                Session["ArrivalDateBackupTatkal"] = null;
                Session["InvalidRequestTatkal"] = null;
                //////////hari krishan

                BookOnTicket Bok = new BookOnTicket();
                DataTable dtPlace = new DataTable();
                //for (int i = 0; i < 20; i++)
                //{
                //    MemberList.Add(new BookOnTicket());
                //}
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicketForVIPSeats("GENRALBOOKING", 5);

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString()),
                        COVIDStatus = Convert.ToInt32(dr["COVIDStatus"]),
                        RefundStatus = Convert.ToInt32(dr["RefundStatus"]),
                        TicketMemberBordingStatus = Convert.ToInt32(dr["TicketMemberBordingStatus"]),
                        isDFOApproved = Convert.ToInt32(dr["isDFOApproved"]),
                        PlaceId = Convert.ToInt32(dr["PlaceId"])



                    });
                }
                ViewData["ticketlist"] = ticketList;
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                ////New change for emitra Kiosk 06-09-2018
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
                    //added by rajveer bcz kiosk user not immplemet in wildlife only dept user working so kiosk and dept usr working is same
                    Session["IsDepartmentalKioskUser"] = true;

                    #region KioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion

                }

                else
                {
                    #region Place
                    if (Encryption.decrypt(Convert.ToString(PlaceName)).ToLower().Trim() == "pow")
                    {
                        dtPlace = Bok.Select_PlaceForVIPOnlineBooking(8);
                    }
                    else if (Encryption.decrypt(Convert.ToString(PlaceName)).ToLower().Trim() == "rtdc")
                    {
                        dtPlace = Bok.Select_PlaceForVIPOnlineBooking(7);
                    }
                    else if (Encryption.decrypt(Convert.ToString(PlaceName)).ToLower().Trim() == "current")
                    {
                        dtPlace = Bok.Select_PlaceForVIPOnlineBooking(6);
                    }
                    else
                    {
                        dtPlace = Bok.Select_PlaceForVIPOnlineBooking(3);
                    }
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                //******************************************************************************


                #region OnlineBookingPopUp Developed by Rajveer

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "OnlineBooking";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;

                }
                #endregion

                // Session["IsDepartmentalKioskUser"] = "True";


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckSafariAccomoAvailabilityVIPOnlineBooking(int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> lstZone = new List<SelectListItem>();
            List<SelectListItem> lstQuotaSeats = new List<SelectListItem>();
            BookOnTicket bot1 = null;
            int isquotaOnoroff = 0;
            string NEWDATEOPEN = string.Empty;
            string OpenBookingDays = "0";
            string CurrentBookingDays = "0";
            try
            {
                BookOnTicket bot = new BookOnTicket();
                DataSet dsSafariAccomodation = new DataSet();
                bot.PlaceId = Convert.ToInt64(PlaceID);
                dsSafariAccomodation = bot.chkSafariAccomoForVIPOnlineBooking(PlaceID);
                if (dsSafariAccomodation.Tables.Count > 0)
                {
                    bot1 = new BookOnTicket();
                    if (dsSafariAccomodation.Tables[0].Rows.Count > 0)
                    {
                        bot1.isSafari = dsSafariAccomodation.Tables[0].Rows[0]["IsSafari"].ToString();
                        bot1.isAccomo = dsSafariAccomodation.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {
                        bot1.isSafari = "";
                        bot1.isAccomo = "";
                    }
                    if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }

                    #region check quota seats on or off
                    var quotaSetting = objWildlifebooking.GetOnlineBookingSetting(PlaceID);
                    isquotaOnoroff = quotaSetting.IsQuotaSeatsOnandOff;
                    var quotaSeats = objWildlifebooking.GetQuotaSeats(PlaceID, 0);
                    if (quotaSeats.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in quotaSeats.Rows)
                        {
                            lstQuotaSeats.Add(new SelectListItem { Text = @dr["QuotaSeats"].ToString(), Value = @dr["Name"].ToString() });
                        }
                    }
                    #endregion


                    if (dsSafariAccomodation.Tables[2].Rows.Count > 0)
                    {
                        bot1.DurationFrom = dsSafariAccomodation.Tables[2].Rows[0]["DurationFromDate"].ToString();
                        bot1.DurationTo = dsSafariAccomodation.Tables[2].Rows[0]["DurationToDate"].ToString();
                        TempData["DurationTo"] = bot1.DurationTo;
                    }
                    else
                    {

                        bot1.DurationFrom = "NF";
                        bot1.DurationTo = "NF";
                        TempData["DurationTo"] = "";
                    }
                    Session["NEWDATEOPEN"] = "0";
                    if (dsSafariAccomodation.Tables[3].Rows.Count > 0)
                    {
                        NEWDATEOPEN = Convert.ToString(dsSafariAccomodation.Tables[3].Rows[0]["NEWDATEOPEN"]);
                        ViewBag.NEWDATEOPEN = NEWDATEOPEN;

                        #region open Date 10 AM every day
                        Session["NEWDATEOPEN"] = null;
                        Session["NEWDATEOPEN"] = NEWDATEOPEN;
                        #endregion

                        //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
                        ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
                    }
                    if (dsSafariAccomodation.Tables[4].Rows.Count > 0)
                    {
                        OpenBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["OpenBookingDuration"].ToString();
                        CurrentBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["CurrentDateOpen"].ToString();
                    }
                }
                else
                {
                    bot1 = new BookOnTicket();
                    bot1.isSafari = "";
                    bot1.isAccomo = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = bot1.isSafari, list2 = bot1.isAccomo, list3 = lstZone, list4 = bot1.DurationFrom, list5 = bot1.DurationTo, NEWDATEOPEN = NEWDATEOPEN, list6 = OpenBookingDays, list7 = CurrentBookingDays, list8 = lstQuotaSeats, list9 = isquotaOnoroff }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult BindShiftByPlaceZoneOnlineBooking(int placeID, int Zone, string ArrivalDate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();
            try
            {
                // changes done by shaan 05/11/2020
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    Session["ShiftIdCurrentAdvanceSession"] = null;
                    Session["VehicleIdCurrentAdvanceSession"] = null;
                    Session["AvaliableTicket"] = null;
                }
                else
                {
                    // changes by shaan 21-02-2020
                    if (placeID == 2)
                    {
                        Session["ShiftIdCurrentAdvanceSession"] = null;
                        Session["VehicleIdCurrentAdvanceSession"] = null;
                        Session["AvaliableTicket"] = null;
                    }
                    // end 
                }
                // changes done by shaan 05/11/2020
                if (Session["CurrentBookingOrAdvanceBooking"] != null && Session["PlaceIdCurrentAdvanceSession"] != null && Session["ZoneIdCurrentAdvanceSession"] != null)
                {
                    if (Session["PlaceIdCurrentAdvanceSession"].ToString() == placeID.ToString() && Session["ZoneIdCurrentAdvanceSession"].ToString() == Zone.ToString())
                    {
                        Session["ArrivalDateCurrentAdvanceSession"] = ArrivalDate;
                    }

                }
                //end

                BookOnTicket bkt = new BookOnTicket();
                DataTable dtShift = new DataTable();

                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    // === set default date and shift as per database 
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;
                    DataTable DT_BoardingDuration = new DataTable();
                    DT_BoardingDuration = obj.GetBoardingDurationForVIPSeats(Convert.ToString(placeID), Convert.ToString(Zone), "GetBoardingDurationForVIPSeatsWithZone");

                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                    {
                        // =========== EVENING_SHIFT
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingEveningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "2";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "False",
                                isEvening = "True",
                                isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                            });
                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                    {
                        // =========== MORNING_SHIFT                       
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            SHIFT_TYPE = "1";
                            fees.Add(new BookOnTicket()
                            {
                                isMorning = "True",
                                isEvening = "False",
                                isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                            });
                        }
                    }

                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"]) == true)
                    {
                        // =========== MORNING_SHIFT                       
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingHalfDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingHalfDayEveningTimeTo"]))
                        {
                            DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                            if (fees.Count > 0 && (fees.FirstOrDefault().isMorning == "True" || fees.FirstOrDefault().isEvening == "True"))
                            {
                                fees.FirstOrDefault().isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"]));
                            }
                            else
                            {
                                SHIFT_TYPE = "4";
                                fees.Add(new BookOnTicket()
                                {
                                    isMorning = "False",
                                    isEvening = "False",
                                    isFullDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["isFullDayShift"])),
                                    isHalfDay = Convert.ToString(Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsHalfDayShift"])),
                                });
                            }
                        }
                    }
                    // === set default date and shift as per database  
                    #endregion
                    Session["ArrivalDateCurrentAdvanceSession"] = DateTime.Now.ToString("dd/MM/yyyy");
                    Session["ShiftIdCurrentAdvanceSession"] = SHIFT_TYPE;
                }
                else
                {
                    #region Citizen User
                    bkt.PlaceId = Convert.ToInt64(placeID);
                    bkt.ZoneId = Convert.ToInt64(Zone);
                    bkt.DateOfArrival = ArrivalDate;
                    dtShift = bkt.Select_Shift_By_PlacesZonesOnlineBooking();
                    if (dtShift.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtShift.Rows)
                        {
                            fees.Add(new BookOnTicket()
                            {
                                DateOfArrival = bkt.DateOfArrival,
                                isMorning = dr["isMorning"].ToString(),
                                isEvening = dr["isEvening"].ToString(),
                                isFullDay = dr["isFullDay"].ToString(),
                                isHalfDay = dr["isHalfDay"].ToString(),
                            });
                        }
                    }
                    else
                    {
                        fees.Add(new BookOnTicket()
                        {
                            DateOfArrival = bkt.DateOfArrival,
                            isMorning = "",
                            isEvening = "",
                            isFullDay = "",
                            isHalfDay = "",
                        });
                    }
                    #endregion

                    #region Check Current Shift In Current Booking Restrict Inspect element Data Value Hard coded
                    Session["CheckShiftwiseValueTatkal"] = null;
                    Session["CheckShiftwiseValueTatkal"] = fees;

                    #endregion

                }


                DataTable dta = new DataTable();
                bkt.PlaceId = Convert.ToInt64(placeID);
                dta = bkt.GetAccomodationType();
                ViewBag.Accomo = dta;
                foreach (System.Data.DataRow dr in ViewBag.Accomo.Rows)
                {
                    Accomodation.Add(new SelectListItem { Text = @dr["RoomType"].ToString(), Value = @dr["AccommodationID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = fees, list2 = Accomodation, list3 = DateTime.Now.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet });
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckTicketAvailabilityForVIPSeatsOnlineBooking(int placeID, string arrivaldate, string shifttime, int Zone, int vehicleID)
        {
            string strStatus = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int ISSeat_NO = 0;
            int ISPNR_NO = 0;
            int ISRoom_No = 0;
            DataTable dtTicketdetails = new DataTable();
            List<SelectListItem> lstQuotaSeats = new List<SelectListItem>();
            int isquotaOnoroff = 0;
            bool checkfordeapartuser = false;
            //////////hari krishan
            Session["ArrivalDateBackupTatkal"] = arrivaldate;
            Session["InvalidRequestTatkal"] = 1;

            if (arrivaldate.Equals(DateTime.Now.AddDays(6).Date.ToShortDateString()))
            {
                DateTime startTime = DateTime.Now;
                int hour = startTime.Hour;
                if (hour < 10)
                {
                    Session["InvalidRequestTatkal"] = 0;
                }
            }
            Session["CheckAvailabilityTimeSessionStartTatkal"] = DateTime.Now.ToString();
            //////////hari krishan
            try
            {
                BookOnTicket bkt = new BookOnTicket();
                bkt.PlaceId = placeID;
                bkt.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                bkt.ShiftTime = shifttime;
                bkt.ZoneId = Zone;
                bkt.vehicleID = vehicleID;
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false")
                //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    bkt.KioskUserId = "0";

                }
                else
                {
                    bkt.KioskUserId = Session["KioskUserId"].ToString();
                }


                bool IsOpen = objWildlifebooking.IsOpenForDepartmentUser(placeID); // Check Online booking open for department user
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True" && IsOpen)
                {
                    checkfordeapartuser = true;
                }
                string seatpereqpt = "0";
                string CalculationSeatWiseOrVehicleWise = "S";
                dtTicketdetails = bkt.CheckTicketAvailabilityForOnlineBooking();
                if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
                {
                    if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
                    {
                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                        Session["VFeeTigerProject"] = dtTicketdetails.Rows[0][1].ToString();
                        Session["VFeeSurcharge"] = dtTicketdetails.Rows[0][2].ToString();
                        Session["VExtraFee"] = dtTicketdetails.Rows[0][3].ToString();
                        Session["TotaVechileFees"] = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                        seatpereqpt = Convert.ToString(dtTicketdetails.Rows[0]["SeatPerEqpt"]);
                        CalculationSeatWiseOrVehicleWise = Convert.ToString(dtTicketdetails.Rows[0]["CalculationSeatWiseOrVehicleWise"]);
                        strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"] + "#" + Session["VExtraFee"] + "#" + Convert.ToInt32(dtTicketdetails.Rows[0][4]);
                    }
                    else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
                    {
                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                        strStatus = Session["AvaliableTicket"] + "#";
                    }
                    else
                    {
                        Session.Remove("AvaliableTicket");
                        Session.Remove("VFeeTigerProject");
                        Session.Remove("VFeeSurcharge");
                        Session.Remove("TotaVechileFees");
                        Session.Remove("VExtraFee");
                        strStatus = "0#";
                    }

                    ISSeat_NO = Convert.ToInt16(dtTicketdetails.Rows[0]["ISSeat_NO"]);
                    ISPNR_NO = Convert.ToInt16(dtTicketdetails.Rows[0]["ISPNR_NO"]);
                    ISRoom_No = Convert.ToInt16(dtTicketdetails.Rows[0]["ISRoom_No"]);
                }
                else
                {
                    Session.Remove("AvaliableTicket");
                    Session.Remove("VFeeTigerProject");
                    Session.Remove("VFeeSurcharge");
                    Session.Remove("TotaVechileFees");
                    Session.Remove("VExtraFee");
                    strStatus = "0#";
                }
                strStatus += "#" + checkfordeapartuser + "#" + seatpereqpt + "#" + CalculationSeatWiseOrVehicleWise;

                #region check quota seats on or off



                var quotaSetting = objWildlifebooking.GetOnlineBookingSetting(placeID);
                isquotaOnoroff = quotaSetting.IsQuotaSeatsOnandOff;
                var quotaSeats = objWildlifebooking.GetQuotaSeats(placeID, vehicleID);
                if (quotaSeats.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in quotaSeats.Rows)
                    {
                        lstQuotaSeats.Add(new SelectListItem { Text = @dr["QuotaSeats"].ToString(), Value = @dr["Name"].ToString() });
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { strStatus = strStatus, lstQuotaSeats = lstQuotaSeats, isquotaOnoroff = isquotaOnoroff, ISSeat_NO = ISSeat_NO, ISPNR_NO = ISPNR_NO, ISRoom_No = ISRoom_No }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult vehicleByCategoryForVIPSeatOnlineBooking(int vehicleCatID, Int64 placeID, int Zone, string ShiftType)
        {
            BookOnTicket bkt = new BookOnTicket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                bkt.PlaceId = placeID;
                bkt.ZoneId = Zone;
                // cst.ShiftType = Shift;
                dt = bkt.Select_vehicleForOnlineBooking(Convert.ToInt64(vehicleCatID), Convert.ToInt64(ShiftType));
                ViewBag.vname = dt;
                foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                {
                    vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "vehicleByCategoryForVIPSeat" + "_" + "OnlineBooking", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return Json(new SelectList(vehicle, "Value", "Text"));
        }

        [ValidateAntiForgeryToken]
        public ActionResult FinalSubmitForOnlineBooking(List<MemberInfo> lstMemberInfo, List<MemberDetailViewModel> lstMembers, BookOnTicket cs, string Command, FormCollection form, string Message, List<HttpPostedFileBase> fileUpload)
        {

            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag && cs.hdn_IAgreement == "1")
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                try
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());


                    bool IsOpen = objWildlifebooking.IsOpenForDepartmentUser(Convert.ToInt32(cs.PlaceId)); // Check Online booking open for department user
                    if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True" && IsOpen)
                    {
                        return DepartmentUserSubmit(lstMembers, Command, form, cs, fileUpload);
                    }
                    else
                    {

                        return OnlineUserSubmit(lstMemberInfo, Command, form, cs, fileUpload);
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                //return View("OnlineTicketPayment");//Comment By Rajveer Kiosk User
            }
            return RedirectToAction("WildlifeBooking");
        }
        public ActionResult _MemberDetailOnlineBooking(int vehicleCatID, Int64 placeID, int ShiftType)
        {
            List<MemberDetailViewModel> lstMembers = new List<MemberDetailViewModel>();
            int seatsPerEqpt = 0;
            List<BookOnTicket> lstTickets = objWildlifebooking.CalculateFeesOnlineBooking(placeID, "2", vehicleCatID, ShiftType, out seatsPerEqpt);
            #region Add Member details
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Indian",
                        NationalityId = "1",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "1").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Passport", Value = "1" },
                        new SelectListItem { Text = "Aadhar", Value = "2" },
                        new SelectListItem { Text = "Driving Licence", Value = "3" },
                        new SelectListItem { Text = "Voter ID", Value = "4" },
                        new SelectListItem { Text = "PAN Card", Value = "5" },
                        new SelectListItem { Text = "Office ID", Value = "6" }


                     };
                    ViewBag.IndianId = ObjList;
                }
                if (i == 1)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Foreigner",
                        NationalityId = "2",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "2").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Passport", Value = "1" },
                    };
                    ViewBag.ForeignerId = ObjList;
                }
                if (i == 2)
                {
                    lstMembers.Add(new MemberDetailViewModel
                    {
                        Nationality = "Student",
                        NationalityId = "3",
                        FeesPerMember = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalPerMemberFees,
                        CameraFees = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalPerMemberCameraFees,
                        GuideFees = lstTickets.Where(x => x.MemberNationality == "3").FirstOrDefault().TotalBoardingFee,
                    });
                    List<SelectListItem> ObjList = new List<SelectListItem>()
                    {
                        new SelectListItem { Text = "Student ID", Value = "7" },
                    };
                    ViewBag.StudentId = ObjList;
                }

            }

            return PartialView(lstMembers);

            #endregion
        }
        public ActionResult OnlineUserSubmit(List<MemberInfo> lstMemberInfo, string Command, FormCollection form, BookOnTicket cs, List<HttpPostedFileBase> fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int rownumber = 0;
            int rowcount = 0;
            decimal finalAmount = 0;
            foreach (var item in lstMemberInfo)
            {
                if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                {
                    rownumber = Convert.ToInt16(item.MemberSLNo);
                    rowcount++;
                }
            }
            if (rownumber != rowcount)
            {
                TempData["RowCheck"] = "Enter member details continiously";
                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
            }
            if (Command == "Submit")
            {
                #region MemberInfo
                long placeId = 0;
                int vehicleId = 0;
                int shifttype = 0;
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    placeId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    shifttype = Convert.ToInt32(form["ddl_Shift"].ToString());
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    vehicleId = Convert.ToInt32(form["ddl_vehicle"].ToString());
                }
                // Check that booking will vehicle wise or seat wise
                DataTable dtMemberInfo = new DataTable();

                string bookingmethod = objWildlifebooking.CheckBookingSeatWiseOrVehicleWise(Convert.ToInt32(cs.PlaceId));
                int TotalMembers = 0;
                List<MemberInfo> lst = lstMemberInfo.Where(x => !string.IsNullOrEmpty(x.MemberName)).ToList();

                TotalMembers = lst.Count();
                if (bookingmethod.ToLower() == "v")
                {
                    lstMemberInfo = objWildlifebooking.ReGenrateMemberInfoVechileWiseForOnline(lstMemberInfo);
                    dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBookingForeignWise(lstMemberInfo, placeId, vehicleId, shifttype);
                }
                else
                {
                    dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBookingIndianAndForeign(lstMemberInfo, placeId, vehicleId, shifttype);
                }

               
                if (dtMemberInfo.Rows.Count == 0)
                {
                    TempData["RowCheck"] = "Enter member details";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }
                else
                {
                    if (dtMemberInfo.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                        {
                            finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                           
                        }
                    }
                }

                
                //hari krishan
                DateTime StartSession;
                DateTime EndSession;
                double diffInSeconds = 0.0;
                UInt32 ectualSeconds = 0;

                  

                Session["CheckAvailabilityTimeSessionEndTatkal"] = DateTime.Now.ToString();
                //DateTime StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStart"]);
                //DateTime EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEnd"]);
                StartSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionStartTatkal"]);
                EndSession = Convert.ToDateTime(Session["CheckAvailabilityTimeSessionEndTatkal"]);
                //var diffInSeconds = (EndSession - StartSession).TotalSeconds;
                //var ectualSeconds = Convert.ToInt32(MemberFillSession) * 7;
                diffInSeconds = (EndSession - StartSession).TotalSeconds;
                ectualSeconds = Convert.ToUInt32(TotalMembers) * 8;
                //if (ectualSeconds > diffInSeconds || diffInSeconds > 660)
                if ((ectualSeconds > diffInSeconds || diffInSeconds > 660) || Convert.ToInt32(Session["InvalidRequestTatkal"]) == 0 || Convert.ToInt32(Session["AvaliableTicket"]) < TotalMembers || form["ArrivalDate"].ToString() != Session["ArrivalDateBackupTatkal"].ToString())
                {
                    return RedirectToAction("InvalidRequest");
                }
                //hari krishan
                #endregion

                #region Submission

                //Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" || Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" )
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" ))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    cs.KioskUserId = "0";

                }
                else
                {
                    cs.KioskUserId = Session["KioskUserId"].ToString();
                }
                //  cs.SsoToken = Request.Cookies["RAJSSO"].Value;
                cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                //System.Threading.Thread.Sleep(10000);    
                //cs.RequestId = DateTime.Now.Ticks.ToString();
                //Session["RequestId"] = cs.RequestId;
                //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                Session["RequestId"] = RequestId();
                cs.RequestId = Session["RequestId"].ToString();
                //***************************************************************************************
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                else
                {
                    cs.PlaceId = 0;
                }
                if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                {
                    cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                }
                else
                {
                    cs.ZoneId = 0;
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    cs.ShiftTime = form["ddl_Shift"].ToString();
                }
                else
                {
                    cs.ShiftTime = "";
                }
                cs.TotalMember = Convert.ToInt32(dtMemberInfo.Rows.Count);

                if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                {

                    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                    #region Check Current Shift In Current Booking Restrict Inspect element Data Value Hard coded
                    if (Session["CheckShiftwiseValueTatkal"] != null && cs.KioskUserId == "0")
                    {
                        List<BookOnTicket> CheckShift = (List<BookOnTicket>)Session["CheckShiftwiseValueTatkal"];
                        if (CheckShift != null && CheckShift.FirstOrDefault().DateOfArrival == cs.ArrivalDate.ToString("dd/MM/yyyy"))
                        {
                            if (CheckShift.FirstOrDefault().isMorning == "True" && cs.ShiftTime == "1")
                            {

                            }
                            else if (CheckShift.FirstOrDefault().isEvening == "True" && cs.ShiftTime == "2")
                            {

                            }
                            else if (CheckShift.FirstOrDefault().isHalfDay == "True" && cs.ShiftTime == "4")
                            {

                            }
                            else if (CheckShift.FirstOrDefault().isFullDay == "True" && cs.ShiftTime == "3")
                            {

                            }
                            else
                            {
                                TempData["datevalidation"] = "This shift not available on this date.";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }

                    }

                    #endregion

                    #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                    //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                    //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                    //}
                    //else
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                    //}

                    #endregion

                    DataTable DTCheckBooking = new DataTable();
                    #region Restrict Months
                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                    if (DTCheckBooking.Rows.Count > 0)
                    {
                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                        {
                            TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                            return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                        }

                    }
                    #endregion

                    #region Get Open Days by rajveer


                    DataTable GetDaysDataTable = new DataTable();
                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                    long AddDaysVal = 0;
                    if (GetDaysDataTable.Rows.Count > 0)
                    {
                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                    }
                    #endregion
                    //DateTime expiryDate = DateTime.Today.AddDays(89);
                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                    if (cs.ArrivalDate > expiryDate)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }

                    //Kiosk User Restrictation
                    if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than Today";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }
                }
                if (TempData["DurationTo"] != null)
                {

                    DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                    if (cs.ArrivalDate > MaxDate)
                    {
                        TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                        return RedirectToAction("BookOnlineTicket", "BookOnlineTicket");
                    }
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                    Session["VehicleId"] = cs.vehicleID;
                }
                else
                {
                    cs.vehicleID = 0;
                }
                if (Session["VFeeTigerProject"] != null)
                {

                    cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                }
                else
                {
                    cs.VehicleFees_TigerProject = 0;
                }
                if (Session["VFeeSurcharge"] != null)
                {

                    cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                }
                else
                {
                    cs.VehicleFees_Surcharge = 0;
                }
                if (Session["TotaVechileFees"] != null)
                {

                    cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                }
                else
                {
                    cs.VehicleFees_Total = 0;
                }
                if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                {
                    cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                }
                else
                { cs.AccomoID = 0; }
                if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                {
                    cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                }
                else
                { cs.TotalRoom = 0; }
                if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                {
                    cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                }
                else
                { cs.RoomCharge = 0; }

                #region CheckBooking Quota
                string quotaName = string.Empty;
                int cwlwquota = 0;
                int ccfquota = 0;
                int inchargequota = 0;
                var setting = objWildlifebooking.GetOnlineBookingSetting(Convert.ToInt32(placeId));
                if (!string.IsNullOrEmpty(Convert.ToString(form["ddlQuota"])))
                {
                    quotaName = Convert.ToString(form["ddlQuota"].ToString());
                    if (quotaName == "CWLW")
                        cwlwquota = cs.TotalMember;
                    else if (quotaName == "CCF")
                        ccfquota = cs.TotalMember;
                    else if (quotaName == "Incharge")
                        inchargequota = cs.TotalMember;

                    DataTable CheckQuotaSeatsAvalibality = objWildlifebooking.CheckQuotaSeatsAvalibality(Convert.ToInt32(placeId), cs.ArrivalDate, cs.vehicleID);
                    if (CheckQuotaSeatsAvalibality.Rows.Count > 0 && setting.IsQuotaSeatsOnandOff == 1)
                    {
                        int cwlwremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCWLWSeats"]);
                        int ccfremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCCFSeats"]);
                        int inchargeremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainningInchargeSeats"]);
                        if (quotaName == "CWLW")
                        {
                            if (cwlwquota > cwlwremainingseats)
                            {
                                TempData["CheckQuota"] = cwlwremainingseats + " seats available in cwlw quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "CCF")
                        {
                            if (ccfquota > ccfremainingseats)
                            {
                                TempData["CheckQuota"] = ccfremainingseats + " seats available in ccf quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "Incharge")
                        {
                            if (inchargequota > inchargeremainingseats)
                            {
                                TempData["CheckQuota"] = inchargeremainingseats + " seats available in incharge quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }


                    }
                }

                #endregion


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
                cs.IPAddress = result;
                DataTable dts = new DataTable();

                DataTable dtcheckTicket = new DataTable();
                string strcheckTicket = string.Empty;
                dtcheckTicket = cs.CheckTicketAvailabilityForOnlineBooking();
                strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                {
                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                    dtMemberInfo.AcceptChanges();
                    dts = cs.Submit_TicketDetailsForOnlineBooking(dtMemberInfo, finalAmount);
                }
                else
                {
                    TempData["RowCheck"] = "Ticket not avaliable";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }
                if (dts.Rows.Count > 0)
                {
                    if (setting.IsQuotaSeatsOnandOff == 1)
                    {
                        objWildlifebooking.SaveOnlineQuotaSeats(cs.RequestId, Convert.ToInt32(cs.PlaceId), cwlwquota, ccfquota, inchargequota, cs.ArrivalDate, DateTime.Now, Convert.ToInt32(cs.EnteredBy), cs.vehicleID);
                    }
                    #region Save File
                    string FileFullName = string.Empty;
                    string FilePath = "~/OnlineBookingVIPDocuments/";
                    string path;
                    List<DocumentList> docModel = new List<DocumentList>();
                    if (fileUpload != null)
                    {
                        int i = 0;
                        foreach (var itm in fileUpload)
                        {
                            if (itm != null)
                            {
                                DocumentList view = new DocumentList();
                                FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                view.FileName = itm.FileName;
                                view.FilePath = Path.Combine(FilePath, FileFullName);
                                view.FileType = 1;//Means DSR1 
                                Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                                docModel.Add(view);
                                i++;
                            }
                        }
                        if (docModel.Count > 0)
                        {
                            int c = objWildlifebooking.SaveOnlineFileUploder("SAVE", cs.RequestId, Convert.ToInt32(cs.PlaceId), Convert.ToInt64(UserID), 1, docModel);
                        }
                    }
                    #endregion

                    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    if (Session["totalprice"].ToString() == "0")
                    {
                        TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }
                }
                else
                {
                    TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }


                #region Add Kiosk User by Rajveer
                EducationTours edu = new EducationTours();
                edu.Location = Convert.ToInt64(cs.PlaceId);
                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                    {
                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                    }
                }

                // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 1;
                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Convert.ToString(cs.RequestId);
                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                    if (dtKiosk.Rows.Count > 0)
                    {
                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                        ViewBag.ViewModel = dts.AsEnumerable();
                        return PartialView("KioskPaymentDetailWildlife", _obj);
                    }
                }
                else
                {
                    ViewData.Model = dts.AsEnumerable();
                    Session["BookingDatamodal"] = dts.AsEnumerable();
                    return View("OnlineTicketPaymentKiosk");
                }
                #endregion


                // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                #endregion
            }
            return RedirectToAction("WildlifeBooking");
        }

        public ActionResult DepartmentUserSubmit(List<MemberDetailViewModel> lstMembers, string Command, FormCollection form, BookOnTicket cs, List<HttpPostedFileBase> fileUpload)
        {
            decimal finalAmount = 0;
            if (Command == "Submit")
            {
                #region MemberInfo
                DataTable dtMemberInfo = new DataTable();
                long placeId = 0;
                int vehicleId = 0;
                int shifttype = 0;
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    placeId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    shifttype = Convert.ToInt32(form["ddl_Shift"].ToString());
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    vehicleId = Convert.ToInt32(form["ddl_vehicle"].ToString());
                }

                List<MemberInfo> lstMemberInfo = objWildlifebooking.ReGenrateMemberInfoVechileWiseForDepartment(lstMembers, placeId, vehicleId, shifttype);

                dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBooking(lstMemberInfo, placeId, vehicleId, shifttype);

                //dtMemberInfo = objWildlifebooking.MemberInformationsOnlineBooking(lstMembers, placeId, vehicleId, shifttype); //MemberInformation(lstMemberInfo);
                if (dtMemberInfo.Rows.Count == 0)
                {
                    TempData["RowCheck"] = "Enter member details";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }
                else
                {
                    if (dtMemberInfo.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                        {
                            finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                        }

                        if (Session["VExtraFee"] != null && Convert.ToDecimal(Session["VExtraFee"]) > 0)
                        {
                            finalAmount += Convert.ToDecimal(Session["VExtraFee"]);
                        }
                    }
                }
                #endregion
                #region Submission

                //Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" || Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" )
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" ))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    cs.KioskUserId = "0";

                }
                else
                {
                    cs.KioskUserId = Session["KioskUserId"].ToString();
                }
                //  cs.SsoToken = Request.Cookies["RAJSSO"].Value;
                cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                //System.Threading.Thread.Sleep(10000);    
                //cs.RequestId = DateTime.Now.Ticks.ToString();
                //Session["RequestId"] = cs.RequestId;
                //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                Session["RequestId"] = RequestId();
                cs.RequestId = Session["RequestId"].ToString();
                //***************************************************************************************
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                else
                {
                    cs.PlaceId = 0;
                }
                if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                {
                    cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                }
                else
                {
                    cs.ZoneId = 0;
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    cs.ShiftTime = form["ddl_Shift"].ToString();
                }
                else
                {
                    cs.ShiftTime = "";
                }
                cs.TotalMember = dtMemberInfo.Rows.Count; //lstMembers.Where(x => x.LeaderName != "").Sum(x => x.TotalPersons); //Convert.ToInt32(rowcount);

                if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                {

                    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                    #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                    //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                    //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                    //}
                    //else
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                    //}

                    #endregion

                    DataTable DTCheckBooking = new DataTable();
                    #region Restrict Months
                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                    if (DTCheckBooking.Rows.Count > 0)
                    {
                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                        {
                            TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                            return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                        }

                    }
                    #endregion

                    #region Get Open Days by rajveer


                    DataTable GetDaysDataTable = new DataTable();
                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                    long AddDaysVal = 0;
                    if (GetDaysDataTable.Rows.Count > 0)
                    {
                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                    }
                    #endregion
                    //DateTime expiryDate = DateTime.Today.AddDays(89);
                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                    if (cs.ArrivalDate > expiryDate)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }

                    //Kiosk User Restrictation
                    if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than Today";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }
                }
                if (TempData["DurationTo"] != null)
                {

                    DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                    if (cs.ArrivalDate > MaxDate)
                    {
                        TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                    Session["VehicleId"] = cs.vehicleID;
                }
                else
                {
                    cs.vehicleID = 0;
                }
                if (Session["VFeeTigerProject"] != null)
                {

                    cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                }
                else
                {
                    cs.VehicleFees_TigerProject = 0;
                }
                if (Session["VFeeSurcharge"] != null)
                {

                    cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                }
                else
                {
                    cs.VehicleFees_Surcharge = 0;
                }
                if (Session["TotaVechileFees"] != null)
                {

                    cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                }
                else
                {
                    cs.VehicleFees_Total = 0;
                }
                if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                {
                    cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                }
                else
                { cs.AccomoID = 0; }
                if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                {
                    cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                }
                else
                { cs.TotalRoom = 0; }
                if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                {
                    cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                }
                else
                { cs.RoomCharge = 0; }

                #region CheckBooking Quota
                string quotaName = string.Empty;
                int cwlwquota = 0;
                int ccfquota = 0;
                int inchargequota = 0;
                var setting = objWildlifebooking.GetOnlineBookingSetting(Convert.ToInt32(placeId));
                if (!string.IsNullOrEmpty(Convert.ToString(form["ddlQuota"])))
                {
                    quotaName = Convert.ToString(form["ddlQuota"].ToString());
                    if (quotaName == "CWLW")
                        cwlwquota = cs.TotalMember;
                    else if (quotaName == "CCF")
                        ccfquota = cs.TotalMember;
                    else if (quotaName == "Incharge")
                        inchargequota = cs.TotalMember;

                    DataTable CheckQuotaSeatsAvalibality = objWildlifebooking.CheckQuotaSeatsAvalibality(Convert.ToInt32(placeId), cs.ArrivalDate, cs.vehicleID);

                    if (CheckQuotaSeatsAvalibality.Rows.Count > 0 && setting.IsQuotaSeatsOnandOff == 1)
                    {
                        int cwlwremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCWLWSeats"]);
                        int ccfremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCCFSeats"]);
                        int inchargeremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainningInchargeSeats"]);
                        if (quotaName == "CWLW")
                        {
                            if (cwlwquota > cwlwremainingseats)
                            {
                                TempData["CheckQuota"] = cwlwremainingseats + " seats available in cwlw quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "CCF")
                        {
                            if (ccfquota > ccfremainingseats)
                            {
                                TempData["CheckQuota"] = ccfremainingseats + " seats available in ccf quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "Incharge")
                        {
                            if (inchargequota > inchargeremainingseats)
                            {
                                TempData["CheckQuota"] = inchargeremainingseats + " seats available in incharge quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }


                    }
                }

                #endregion

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
                cs.IPAddress = result;
                DataTable dts = new DataTable();

                DataTable dtcheckTicket = new DataTable();
                string strcheckTicket = string.Empty;
                dtcheckTicket = cs.CheckTicketAvailabilityForOnlineBooking();
                strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                {
                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                    dtMemberInfo.AcceptChanges();
                    int indiancount = lstMembers.Where(x => x.NationalityId == "1").FirstOrDefault().TotalPersons;
                    int foreignercount = lstMembers.Where(x => x.NationalityId == "2").FirstOrDefault().TotalPersons;
                    int indianstudentcount = lstMembers.Where(x => x.NationalityId == "3").FirstOrDefault().TotalPersons;
                    dts = cs.Submit_TicketDetailsForOnlineBooking(dtMemberInfo, finalAmount, indiancount, foreignercount, indianstudentcount);

                }
                else
                {
                    TempData["RowCheck"] = "Ticket not avaliable";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }
                if (dts.Rows.Count > 0)
                {
                    if (setting.IsQuotaSeatsOnandOff == 1)
                    {
                        objWildlifebooking.SaveOnlineQuotaSeats(cs.RequestId, Convert.ToInt32(cs.PlaceId), cwlwquota, ccfquota, inchargequota, cs.ArrivalDate, DateTime.Now, Convert.ToInt32(cs.EnteredBy), cs.vehicleID);
                    }
                    #region Save File
                    string FileFullName = string.Empty;
                    string FilePath = "~/OnlineBookingVIPDocuments/";
                    string path;
                    List<DocumentList> docModel = new List<DocumentList>();
                    if (fileUpload != null)
                    {
                        int i = 0;
                        foreach (var itm in fileUpload)
                        {
                            if (itm != null)
                            {
                                DocumentList view = new DocumentList();
                                FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                view.FileName = itm.FileName;
                                view.FilePath = Path.Combine(FilePath, FileFullName);
                                view.FileType = 1;//Means DSR1 
                                Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                                docModel.Add(view);
                                i++;
                            }
                        }
                        if (docModel.Count > 0)
                        {
                            int c = objWildlifebooking.SaveOnlineFileUploder("SAVE", cs.RequestId, Convert.ToInt32(cs.PlaceId), Convert.ToInt64(Session["UserID"]), 1, docModel);
                        }
                    }
                    #endregion

                    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    if (Session["totalprice"].ToString() == "0")
                    {
                        TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                    }
                }
                else
                {
                    TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                    return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                }


                #region Add Kiosk User by Rajveer
                EducationTours edu = new EducationTours();
                edu.Location = Convert.ToInt64(cs.PlaceId);
                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                    {
                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                    }
                }

                // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 1;
                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Convert.ToString(cs.RequestId);
                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                    if (dtKiosk.Rows.Count > 0)
                    {
                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                        ViewBag.ViewModel = dts.AsEnumerable();
                        return PartialView("KioskPaymentDetailWildlife", _obj);
                    }
                }
                else
                {
                    ViewData.Model = dts.AsEnumerable();
                    return View("OnlineTicketPaymentKiosk");
                }
                #endregion


                // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                #endregion
            }
            return RedirectToAction("WildlifeBooking");
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult SelectFeeOnlineBooking(Int64 placeID, string nationality, string memberType, int vehicleID, int ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<BookOnTicket> fees = new List<BookOnTicket>();
            try
            {
                DataTable dt = new DataTable();
                BookOnTicket cst = new BookOnTicket();
                cst.PlaceId = placeID;
                cst.MemberNationality = nationality;
                cst.MemberType = memberType;
                cst.vehicleID = vehicleID;
                dt = cst.SelectMemberFeesOnlineBooking(ShiftType);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new BookOnTicket()
                        {
                            MemberFees_TigerProject = Convert.ToDecimal(dr["MFees_TigerProject"].ToString()),
                            MemberFees_Surcharge = Convert.ToDecimal(dr["MFees_Surcharge"].ToString()),
                            TRDF = Convert.ToDecimal(dr["TRDF"].ToString()),
                            CameraFees_TigerProject = Convert.ToDecimal(dr["CFees_TigerProject"].ToString()),
                            CameraFees_Surcharge = Convert.ToDecimal(dr["CFees_Surcharge"].ToString()),

                            TotalPerMemberFees = (Convert.ToDecimal(dr["MFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["MFees_Surcharge"].ToString()) + Convert.ToDecimal(dr["TRDF"].ToString()) + Convert.ToDecimal(dr["Vehicle_TRDF"]) + Convert.ToDecimal(dr["GuidFee_TRDF"])),

                            TotalPerMemberCameraFees = (Convert.ToDecimal(dr["CFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["CFees_Surcharge"].ToString())),

                            BoardingVehicleFee = Convert.ToDecimal(dr["BoardingVehicleFee"]),
                            BoardingVehicleFeeGSTPercentage = Convert.ToDecimal(dr["BoardingVehicleFeeGSTPercentage"]),
                            BoardingVehicleFeeGSTAmount = Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]),


                            BoardingGuideFee = Convert.ToDecimal(dr["BoardingGuideFee"]),
                            BoardingGuideFeeGSTPercentage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]),
                            BoardingGuideFeeGSTAmount = Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),

                            TotalBoardingFee = Convert.ToDecimal(dr["BoardingVehicleFee"]) + Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]) + Convert.ToDecimal(dr["BoardingGuideFee"]) + Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),


                            GSTMessage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]) == 0 ? "" : Convert.ToString(dr["BoardingGuideFeeGSTPercentage"]) + " % GST  applicable on guide fees And " + Convert.ToString(dr["BoardingVehicleFeeGSTPercentage"]) + "% applicable on vehicle rent",

                            Vehicle_TRDF = Convert.ToDecimal(dr["Vehicle_TRDF"]),

                            GuidFee_TRDF = Convert.ToDecimal(dr["GuidFee_TRDF"]),
                            //Added by shaan 30-03-2021
                            Fees_TigerProjectHalfDayFullDayCharge = string.IsNullOrEmpty(dr["Fees_TigerProjectHalfDayFullDayCharge"].ToString()) ? 0 : Convert.ToDecimal(dr["Fees_TigerProjectHalfDayFullDayCharge"]),
                            Fee_SurchargeHalfDayFullDayCharge = string.IsNullOrEmpty(dr["Fee_SurchargeHalfDayFullDayCharge"].ToString()) ? 0 : Convert.ToDecimal(dr["Fee_SurchargeHalfDayFullDayCharge"])
                            //end

                        });
                    }
                }
                else
                {
                    fees.Add(new BookOnTicket()
                    {
                        MemberFees_TigerProject = Convert.ToDecimal(0),
                        MemberFees_Surcharge = Convert.ToDecimal(0),
                        TRDF = Convert.ToDecimal(0),
                        CameraFees_TigerProject = Convert.ToDecimal(0),
                        CameraFees_Surcharge = Convert.ToDecimal(0),
                        TotalPerMemberFees = Convert.ToDecimal(0),
                        TotalPerMemberCameraFees = Convert.ToDecimal(0),
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(fees);
        }
        public PartialViewResult MemberInfoOnlineBooking(int rowcount, int Is_PNR_NO, int Is_Seat_NO, int Is_Room_No)
        {
           
            var MemberList = new List<BookOnTicket>();
            for (int i = 0; i < rowcount; i++)
            {
                BookOnTicket model = new BookOnTicket();
                model.Is_PNR_NO = Is_PNR_NO;
                model.Is_Seat_NO = Is_Seat_NO;
                model.Is_Room_No = Is_Room_No;
                MemberList.Add(model);
            }
            return PartialView(MemberList);
        }
        public JsonResult AutoCompleteVehicleName(string prefix, string vehicleType)
        {

            List<AutoCompleteData> GName = new List<AutoCompleteData>();
            DataTable DT1 = new DataTable();
            DT1 = objWildlifebooking.GetVehicleList(vehicleType, prefix);
            for (int i = 0; i < DT1.Rows.Count; i++)
            {
                string text = Convert.ToString(DT1.Rows[i]["VehicleName"]);
                string value = Convert.ToString(DT1.Rows[i]["VehicleNo"]);
                GName.Add(new AutoCompleteData
                {
                    Text = text,
                    Value = value
                });
            }
            return Json(GName, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AutoCompleteGuideName(string prefix, string vehicleType)
        {

            List<AutoCompleteData> GName = new List<AutoCompleteData>();
            DataTable DT1 = new DataTable();
            DT1 = objWildlifebooking.GetGuideList(vehicleType, prefix);
            for (int i = 0; i < DT1.Rows.Count; i++)
            {
                string text = Convert.ToString(DT1.Rows[i]["GuideName"]);
                string value = Convert.ToString(DT1.Rows[i]["PersonName"]);
                GName.Add(new AutoCompleteData
                {
                    Text = text,
                    Value = value
                });
            }
            return Json(GName, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HalfDay FullDay Booking
        public ActionResult WildlifeBookings()
        {
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).AddHours(-10).ToString("yyyy/MM/dd");//Change date 10 AM
            ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
            var MemberList = new List<BookOnTicket>();
            try
            {
                Session.Remove("AvaliableTicket");
                Session.Remove("VFeeTigerProject");
                Session.Remove("VFeeSurcharge");
                Session.Remove("TotaVechileFees");
                Session.Remove("totalprice");
                Session.Remove("RequestId");
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtPlace = new DataTable();
                //for (int i = 0; i < 20; i++)
                //{
                //    MemberList.Add(new BookOnTicket());
                //}
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicketForVIPSeats("HALFDAYFULLDAYBOOKING", 5);

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),
                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString())

                    });
                }
                ViewData["ticketlist"] = ticketList;
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                ////New change for emitra Kiosk 06-09-2018
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
                    //added by rajveer bcz kiosk user not immplemet in wildlife only dept user working so kiosk and dept usr working is same
                    Session["IsDepartmentalKioskUser"] = true;

                    #region KioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion

                }

                else
                {
                    #region Place
                    dtPlace = Bok.Select_PlaceForVIPOnlineBooking(4);
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                //******************************************************************************


                #region OnlineBookingPopUp Developed by Rajveer

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "OnlineBooking";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;

                }
                #endregion

                // Session["IsDepartmentalKioskUser"] = "True";


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult FinalSubmitForOnlineBookingHalfday(List<MemberInfo> lstMemberInfo, List<MemberDetailViewModel> lstMembers, BookOnTicket cs, string Command, FormCollection form, string Message, List<HttpPostedFileBase> fileUpload)
        {

            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            if (1 == 1)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                try
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                    bool IsOpen = objWildlifebooking.IsOpenForDepartmentUser(Convert.ToInt32(cs.PlaceId)); // Check Online booking open for department user
                    if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True" && IsOpen)
                    {
                        return DepartmentUserSubmit(lstMembers, Command, form, cs, fileUpload);
                    }
                    else
                    {

                        return OnlineUserSubmitHalfDay(lstMemberInfo, Command, form, cs, fileUpload);
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                //return View("OnlineTicketPayment");//Comment By Rajveer Kiosk User
            }
            return RedirectToAction("WildlifeBookings");
        }


        public ActionResult OnlineUserSubmitHalfDay(List<MemberInfo> lstMemberInfo, string Command, FormCollection form, BookOnTicket cs, List<HttpPostedFileBase> fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int rownumber = 0;
            int rowcount = 0;
            decimal finalAmount = 0;
            foreach (var item in lstMemberInfo)
            {
                if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                {
                    rownumber = Convert.ToInt16(item.MemberSLNo);
                    rowcount++;
                }
            }
            if (rownumber != rowcount)
            {
                TempData["RowCheck"] = "Enter member details continiously";
                return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
            }
            if (Command == "Submit")
            {
                #region MemberInfo
                //DataTable dtMemberInfo = new DataTable();
                //dtMemberInfo = MemberInformation(lstMemberInfo);

                // Check that booking will vehicle wise or seat wise
                string bookingmethod = objWildlifebooking.CheckBookingSeatWiseOrVehicleWise(Convert.ToInt32(cs.PlaceId));
                if (bookingmethod.ToLower() == "v")
                {
                    lstMemberInfo = objWildlifebooking.ReGenrateMemberInfoVechileWiseForOnline(lstMemberInfo);
                }
                DataTable dtMemberInfo = new DataTable();


                long placeId = 0;
                int vehicleId = 0;
                int shifttype = 0;
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    placeId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    shifttype = Convert.ToInt32(form["ddl_Shift"].ToString());
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    vehicleId = Convert.ToInt32(form["ddl_vehicle"].ToString());
                }
                dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBooking(lstMemberInfo, placeId, vehicleId, shifttype);

                if (dtMemberInfo.Rows.Count == 0)
                {
                    TempData["RowCheck"] = "Enter member details";
                    return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                }
                else
                {
                    if (dtMemberInfo.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                        {
                            finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                        }
                      
                    }
                }

                #endregion

                #region Submission

                //Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" || Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" )
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" ))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    cs.KioskUserId = "0";

                }
                else
                {
                    cs.KioskUserId = Session["KioskUserId"].ToString();
                }
                //  cs.SsoToken = Request.Cookies["RAJSSO"].Value;
                cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                //System.Threading.Thread.Sleep(10000);    
                //cs.RequestId = DateTime.Now.Ticks.ToString();
                //Session["RequestId"] = cs.RequestId;
                //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                Session["RequestId"] = RequestId();
                cs.RequestId = Session["RequestId"].ToString();
                //***************************************************************************************

                if (!string.IsNullOrEmpty(Convert.ToString(form["CitizenRemarks"])))
                {
                    cs.CitizenRemarksVal = Convert.ToString(form["CitizenRemarks"]);
                }
                else
                {
                    cs.CitizenRemarksVal = "";
                }

                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                else
                {
                    cs.PlaceId = 0;
                }
                if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                {
                    cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                }
                else
                {
                    cs.ZoneId = 0;
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    cs.ShiftTime = form["ddl_Shift"].ToString();
                }
                else
                {
                    cs.ShiftTime = "";
                }
                cs.TotalMember = Convert.ToInt32(dtMemberInfo.Rows.Count);

                if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                {

                    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                    #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                    //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                    //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                    //}
                    //else
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                    //}

                    #endregion

                    DataTable DTCheckBooking = new DataTable();
                    #region Restrict Months
                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                    if (DTCheckBooking.Rows.Count > 0)
                    {
                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                        {
                            TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                            return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                        }

                    }
                    #endregion

                    #region Get Open Days by rajveer


                    DataTable GetDaysDataTable = new DataTable();
                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                    long AddDaysVal = 0;
                    if (GetDaysDataTable.Rows.Count > 0)
                    {
                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                    }
                    #endregion
                    //DateTime expiryDate = DateTime.Today.AddDays(89);
                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                    if (cs.ArrivalDate > expiryDate)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                        return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                    }

                    //Kiosk User Restrictation
                    if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than Today";
                        return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                    }
                }
                if (TempData["DurationTo"] != null)
                {

                    DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                    if (cs.ArrivalDate > MaxDate)
                    {
                        TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                        return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                    }
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                    Session["VehicleId"] = cs.vehicleID;
                }
                else
                {
                    cs.vehicleID = 0;
                }
                if (Session["VFeeTigerProject"] != null)
                {

                    cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                }
                else
                {
                    cs.VehicleFees_TigerProject = 0;
                }
                if (Session["VFeeSurcharge"] != null)
                {

                    cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                }
                else
                {
                    cs.VehicleFees_Surcharge = 0;
                }
                if (Session["TotaVechileFees"] != null)
                {

                    cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                }
                else
                {
                    cs.VehicleFees_Total = 0;
                }
                if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                {
                    cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                }
                else
                { cs.AccomoID = 0; }
                if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                {
                    cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                }
                else
                { cs.TotalRoom = 0; }
                if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                {
                    cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                }
                else
                { cs.RoomCharge = 0; }

                #region CheckBooking Quota
                string quotaName = string.Empty;
                int cwlwquota = 0;
                int ccfquota = 0;
                int inchargequota = 0;
                var setting = objWildlifebooking.GetOnlineBookingSetting(Convert.ToInt32(placeId));
                if (!string.IsNullOrEmpty(Convert.ToString(form["ddlQuota"])))
                {
                    quotaName = Convert.ToString(form["ddlQuota"].ToString());
                    if (quotaName == "CWLW")
                        cwlwquota = cs.TotalMember;
                    else if (quotaName == "CCF")
                        ccfquota = cs.TotalMember;
                    else if (quotaName == "Incharge")
                        inchargequota = cs.TotalMember;

                    DataTable CheckQuotaSeatsAvalibality = objWildlifebooking.CheckQuotaSeatsAvalibality(Convert.ToInt32(placeId), cs.ArrivalDate, cs.vehicleID);
                    if (CheckQuotaSeatsAvalibality.Rows.Count > 0 && setting.IsQuotaSeatsOnandOff == 1)
                    {
                        int cwlwremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCWLWSeats"]);
                        int ccfremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCCFSeats"]);
                        int inchargeremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainningInchargeSeats"]);
                        if (quotaName == "CWLW")
                        {
                            if (cwlwquota > cwlwremainingseats)
                            {
                                TempData["CheckQuota"] = cwlwremainingseats + " seats available in cwlw quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "CCF")
                        {
                            if (ccfquota > ccfremainingseats)
                            {
                                TempData["CheckQuota"] = ccfremainingseats + " seats available in ccf quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "Incharge")
                        {
                            if (inchargequota > inchargeremainingseats)
                            {
                                TempData["CheckQuota"] = inchargeremainingseats + " seats available in incharge quota";
                                return RedirectToAction("WildlifeBooking", "BookOnlineTicket");
                            }
                        }


                    }
                }

                #endregion


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
                cs.IPAddress = result;
                DataTable dts = new DataTable();

                DataTable dtcheckTicket = new DataTable();
                string strcheckTicket = string.Empty;
                dtcheckTicket = cs.CheckTicketAvailabilityForOnlineBooking();
                strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                {
                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                    dtMemberInfo.AcceptChanges();
                    dts = cs.Submit_TicketDetailsForOnlineBooking(dtMemberInfo, finalAmount);
                    ViewData["CitizenRemarks"] = cs.CitizenRemarksVal;

                }
                else
                {
                    TempData["RowCheck"] = "Ticket not avaliable";
                    return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                }
                if (dts.Rows.Count > 0)
                {
                    if (setting.IsQuotaSeatsOnandOff == 1)
                    {
                        objWildlifebooking.SaveOnlineQuotaSeats(cs.RequestId, Convert.ToInt32(cs.PlaceId), cwlwquota, ccfquota, inchargequota, cs.ArrivalDate, DateTime.Now, Convert.ToInt32(UserID), cs.vehicleID);
                    }

                    #region Save File
                    string FileFullName = string.Empty;
                    string FilePath = "~/OnlineBookingVIPDocuments/";
                    string path;
                    List<DocumentList> docModel = new List<DocumentList>();
                    if (fileUpload != null)
                    {
                        int i = 0;
                        foreach (var itm in fileUpload)
                        {
                            if (itm != null)
                            {
                                DocumentList view = new DocumentList();
                                FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                view.FileName = itm.FileName;
                                view.FilePath = Path.Combine(FilePath, FileFullName);
                                view.FileType = 1;//Means DSR1 
                                Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                                docModel.Add(view);
                                i++;
                            }
                        }
                        if (docModel.Count > 0)
                        {
                            int c = objWildlifebooking.SaveOnlineFileUploder("SAVE", cs.RequestId, Convert.ToInt32(cs.PlaceId), Convert.ToInt64(UserID), 1, docModel);
                        }
                    }
                    #endregion

                    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    if (Session["totalprice"].ToString() == "0")
                    {
                        TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                    }
                }
                else
                {
                    TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                    return RedirectToAction("WildlifeBookings", "BookOnlineTicket");
                }


                #region Add Kiosk User by Rajveer
                EducationTours edu = new EducationTours();
                edu.Location = Convert.ToInt64(cs.PlaceId);
                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                    {
                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                    }
                }

                // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 1;
                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Convert.ToString(cs.RequestId);
                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                    if (dtKiosk.Rows.Count > 0)
                    {
                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                        ViewBag.ViewModel = dts.AsEnumerable();
                        return PartialView("KioskPaymentDetailWildlife", _obj);
                    }
                }
                else
                {
                    ViewData.Model = dts.AsEnumerable();
                    return View("OnlineTicketPaymentHalfdayFullday");
                }
                #endregion


                // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                #endregion
            }
            return RedirectToAction("WildlifeBookings");
        }

        public ActionResult SubmitRemarkHalfDayFullDay(string RequestID = "", string CitizenRemarks = "", string AdminRemarks = "", string AdminVisitdate = "", int ISApprovalOrReject = 0)
        {
            try
            {
                BookOnTicket bot = new BookOnTicket();
                DataTable DT = new DataTable();
                DT = bot.SubmitReviewApprovalRemark("Save", 1, CitizenRemarks, RequestID, AdminRemarks, AdminVisitdate, Convert.ToInt64(Session["UserID"]));
                if (DT.Rows.Count > 0)
                {
                    SMSandEMAILtemplate obj = new SMSandEMAILtemplate();
                    DataTable dtUserInfo = obj.GetUserDetails(RequestID, "GETUSERDETAILSFORSENDSMSANDEMAILforHalfDayFullDayApproval");
                    obj.SendMailComman("ALL", "Half/Full Day Citizen Visit Permit", RequestID, Convert.ToString(dtUserInfo.Rows[0]["NAME"]), Convert.ToString(dtUserInfo.Rows[0]["EmailId"]), Convert.ToString(dtUserInfo.Rows[0]["Mobile"]), string.Empty, string.Empty);
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + Convert.ToString(DT.Rows[0]["msg"]) + " </div>";

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitRemarkHalfDayFullDay" + "_" + "BookOnlineTicket", ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("WildlifeBookings");

        }


        public ActionResult OnlineBookingApproveReject()
        {
            ViewBag.ReturnMsg = TempData["msg"];
            ViewBag.IsError = TempData["isError"];
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SaveOnlineBookingApproveReject> lstSaveOnlineBookingApproveReject = new List<SaveOnlineBookingApproveReject>();
            try
            {
                SaveOnlineBookingApproveReject Bok = new SaveOnlineBookingApproveReject();
                DataTable dtf = new DataTable();
                dtf = Bok.GetDataOnlineBookingApproveReject();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstSaveOnlineBookingApproveReject.Add(new SaveOnlineBookingApproveReject()
                    {
                        SNO = count,
                        RequestID = Convert.ToString(dr["RequestID"].ToString()),
                        CitizenVisitDate = Convert.ToString(dr["CitizenVisitDate"].ToString()),
                        PlaceID = Convert.ToInt32(dr["PlaceID"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        ZoneID = Convert.ToInt32(dr["ZoneID"].ToString()),
                        ZoneName = Convert.ToString(dr["ZoneName"].ToString()),
                        ShiftTime = Convert.ToInt32(dr["ShiftTime"].ToString()),
                        ShiftTimeName = Convert.ToString(dr["ShiftTimeName"].ToString()),
                        DateOfArrival = Convert.ToString(dr["DateOfArrival"].ToString()),
                    });
                    count++;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("OnlineBookingApproveReject", lstSaveOnlineBookingApproveReject);
        }

        public ActionResult GetDetailsWithRequestID(string RequestID, int IsCitizenOrAdmin)
        {
            TempData["RequestID"] = RequestID;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SaveOnlineBookingApproveReject> lstSaveOnlineBookingApproveReject = new List<SaveOnlineBookingApproveReject>();
            try
            {
                SaveOnlineBookingApproveReject Bok = new SaveOnlineBookingApproveReject();
                DataSet dtf = new DataSet();
                dtf = Bok.GetDataOnlineBookingApproveReject(RequestID, UserID, IsCitizenOrAdmin);
                int count = 1;
                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    lstSaveOnlineBookingApproveReject.Add(new SaveOnlineBookingApproveReject()
                    {
                        SNO = count,
                        RequestID = Convert.ToString(dr["RequestID"].ToString()),
                        CitizenRemarks = Convert.ToString(dr["CitizenRemarks"].ToString()),
                        AdminRemarks = Convert.ToString(dr["AdminRemarks"].ToString()),
                        CitizenVisitDate = Convert.ToString(dr["CitizenVisitDate"].ToString()),
                        CreatedDate = Convert.ToString(dr["CreatedDate"].ToString()),
                        CreatedBy = Convert.ToString(dr["CreatedBy"].ToString()),
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        PlaceID = Convert.ToInt32(dr["PlaceID"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        ZoneID = Convert.ToInt32(dr["ZoneID"].ToString()),
                        ZoneName = Convert.ToString(dr["ZoneName"].ToString()),
                        ShiftTime = Convert.ToInt32(dr["ShiftTime"].ToString()),
                        ShiftTimeName = Convert.ToString(dr["ShiftTimeName"].ToString()),
                        DateOfArrival = Convert.ToString(dr["DateOfArrival"].ToString()),
                        TotalMembers = Convert.ToInt32(dr["TotalMembers"].ToString()),
                        TotalFees = Convert.ToDecimal(dr["TotalFees"].ToString()),
                        AmountTobePaid = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        AmountWithServiceCharges = Convert.ToDecimal(dr["AmountWithServiceCharges"].ToString()),
                        EmitraTransactionId = Convert.ToString(dr["EmitraTransactionId"].ToString()),
                        Reserve_Status = Convert.ToString(dr["Reserve_Status"].ToString()),
                        Name = Convert.ToString(dr["Name"].ToString()),
                        Gender = Convert.ToString(dr["Gender"].ToString()),
                        IDType = Convert.ToString(dr["IDType"].ToString()),
                        IDNo = Convert.ToString(dr["IDNo"].ToString()),
                        Nationality = Convert.ToString(dr["Nationality"].ToString()),
                        MemberType = Convert.ToString(dr["MemberType"].ToString()),
                        Status = Convert.ToInt32(dr["ApplicationStatus"].ToString())
                    });
                    count++;
                }


                #region Get Upload files

                IDictionary<string, string> dict = new Dictionary<string, string>();
                if (dtf.Tables[1] != null && dtf.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in dtf.Tables[1].Rows)
                    {
                        dict.Add(Convert.ToString(dr["FileName"]), Convert.ToString(dr["FilePath"].ToString()));
                    }

                }
                ViewBag.FileUploader = dict;
                #endregion

                #region Get Review Approve Button Show or not
                int showHideReviewApproveBtn = 0;
                if (dtf.Tables[2] != null && dtf.Tables[2].Rows.Count > 0)
                {
                    showHideReviewApproveBtn = Convert.ToInt32(dtf.Tables[2].Rows[0]["SaveButtonOpenClose"]);
                }
                ViewBag.showHideReviewApproveBtn = showHideReviewApproveBtn;
                #endregion

                #region Get total Vehicle And Booked Seats
                int totalVehicle = 0;
                int ApprovedSeats = 0;
                int RemainingSeats = 0;
                if (dtf.Tables[3] != null && dtf.Tables[3].Rows.Count > 0)
                {
                    totalVehicle = Convert.ToInt32(dtf.Tables[3].Rows[0]["totalVehicle"]);
                    ApprovedSeats = Convert.ToInt32(dtf.Tables[3].Rows[0]["ApprovedSeats"]);
                    RemainingSeats = Convert.ToInt32(dtf.Tables[3].Rows[0]["RemainingSeats"]);
                }
                ViewBag.totalVehicle = totalVehicle;
                ViewBag.ApprovedSeats = ApprovedSeats;
                ViewBag.RemainingSeats = RemainingSeats;
                #endregion

                #region Get Application Log
                List<OnlineBookingApproveRejectLogModel> LogModel = new List<OnlineBookingApproveRejectLogModel>();
                OnlineBookingApproveRejectLogModel model = null;
                if (dtf.Tables[4] != null && dtf.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dr in dtf.Tables[4].Rows)
                    {
                        model = new OnlineBookingApproveRejectLogModel();
                        model.AdminRemarks = Convert.ToString(dr["AdminRemarks"]);
                        model.USerName = Convert.ToString(dr["Name"]);
                        model.StatusName = Convert.ToString(dr["StatusDesc"]);
                        LogModel.Add(model);
                    }

                }
                ViewBag.LogModel = LogModel;
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return PartialView("_partialGetDetailsWithRequestID", lstSaveOnlineBookingApproveReject);
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public ActionResult GetDetailsWithRequestID(SaveOnlineBookingApproveReject obj, string Command)
        {
            string RequestID = Convert.ToString(TempData["RequestID"]);
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int ApproveRejectStatus = 0;
            if (Command == "Approve")
            {
                ApproveRejectStatus = 2;
            }
            else if (Command == "Reject")
            {
                ApproveRejectStatus = 3;
            }
            else if (Command == "Forwarded")
            {
                ApproveRejectStatus = 10;
            }
            else
            {
                ApproveRejectStatus = 1;
            }
            try
            {
                int Status = 0;
                string msg = "";
                SaveOnlineBookingApproveReject Bok = new SaveOnlineBookingApproveReject();
                DataTable dtf = new DataTable();
                dtf = Bok.SaveDataOnlineBookingApproveReject(RequestID, ApproveRejectStatus, obj.DateOfArrival, obj.AdminRemarks, UserID);
                foreach (DataRow dr in dtf.Rows)
                {
                    Status = Convert.ToInt32(dr["Status"].ToString()) == 1 ? 0 : Convert.ToInt32(dr["Status"].ToString());
                    msg = Convert.ToString(dr["msg"].ToString());
                }
                if (Status == 0)
                {
                    SMSandEMAILtemplate objSendMail = new SMSandEMAILtemplate();
                    DataTable dtUserInfo = objSendMail.GetUserDetails(RequestID, "GETUSERDETAILSFORSENDSMSANDEMAILforHalfDayFullDayApproved");
                    objSendMail.SendMailComman("ALL", "Half/Full Day Request Approved", RequestID, Convert.ToString(dtUserInfo.Rows[0]["NAME"]), Convert.ToString(dtUserInfo.Rows[0]["EmailId"]), Convert.ToString(dtUserInfo.Rows[0]["Mobile"]), Command, string.Empty);
                }

                TempData["msg"] = msg;
                TempData["isError"] = Convert.ToBoolean(Status);
                // TempData.Remove("RequestID");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("OnlineBookingApproveReject", "BookOnlineTicket");
        }


        public void PayHalfDayFullDay(string RequestID, string TotalAmount)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                // get different heads amount from DB
                BookOnTicket OBJ = new BookOnTicket();
                DataSet DS = new DataSet();

                if (!string.IsNullOrEmpty(RequestID) && !string.IsNullOrEmpty(TotalAmount))
                {
                    RequestID = Encryption.decrypt(Convert.ToString(RequestID));
                    TotalAmount = Encryption.decrypt(Convert.ToString(TotalAmount));
                    Session["RequestId"] = RequestID;
                    Session["totalprice"] = TotalAmount;

                }

                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("WildLifeTickets", Convert.ToString(Session["RequestId"]));

                string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"] + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"]));

                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + "Step Pay : Get Heads amount from DB ========== " });


                string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["RequestId"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                    ReturnUrl + "BookOnlineTicket/PaymentForVIPBooking", ReturnUrl + "BookOnlineTicket/PaymentForVIPBooking",
                    Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());

                //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + "Step Pay : Request Post To Emitra ========== " });

                Response.Write(forms);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        public ActionResult PaymentForVIPBooking()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int fmdssStatus = 0;
            if (Session["RequestId"] != null)
            {
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

                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23"); //Live
                    //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    // string DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Session["RequestId"].ToString(), "WildLifeTicketBooking", DecryptedData);

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion

                    //****************************** for test only

                    //ObjPGResponse.STATUS = "SUCCESS";
                    //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    //ObjPGResponse.PRN = Convert.ToString(Session["RequestId"]);
                    //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                    //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                    //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                    //****************************** for test only;

                    string steps = string.Empty;
                    #region Response Status
                    // if (ObjPGResponse.STATUS == "FAILED") Arvind Sir
                    if (ObjPGResponse.STATUS != "SUCCESS") //Rajveer
                    {
                        //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 0: Enter Success " });
                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;
                        // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                        //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        //if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null))
                        Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                        if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                        {
                            cs.KioskUserId = "0";
                        }
                        else
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();

                        }

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                        //if (System.Threading.Monitor.TryEnter(emitraLock, 200000))
                        //{

                        //  System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 0: Enter Success " });
                        try
                        {
                            cs.Trn_Status_Code = 0;
                            fmdssStatus = cs.UpdateTransactionStatusForVIP("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 1: After DB " });
                        }
                        catch (Exception ex)
                        {
                            //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 2: catch in Thread " + ex.Message });
                        }
                        finally
                        {
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 3: finally execute " });
                            // System.Threading.Monitor.Exit(emitraLock);
                        }
                        // }

                        //lock (emitraLock)
                        //{
                        //    cs.Trn_Status_Code = 0;
                        //    fmdssStatus = cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                        //}

                        //*****************************************************************

                        if (fmdssStatus == 1)
                        {

                            dtrow["TRANSACTION STATUS"] = "SUCCESS";


                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";

                        }
                        dt.Rows.Add(dtrow);

                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {
                        // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 0: Enter Success " });
                        DataRow dtrow = dt.NewRow();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = ObjPGResponse.PRN;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {

                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 1: Enter Success " });
                            //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************

                            //if (System.Threading.Monitor.TryEnter(emitraLock, 200000))
                            //{
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 2: Enter in Threading " });
                            try
                            {
                                if (Convert.ToString(Session["RequestId"]).Equals(ObjPGResponse.PRN) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                                {
                                    // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 3: check RequestId and amount" });
                                    cs.Trn_Status_Code = 1;
                                    status1 = 1;
                                    fmdssStatus = cs.UpdateTransactionStatusForVIP("3", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                                }
                                else // Added to save mismatch in payment
                                {
                                    // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 4: not match RequestId and amount" });
                                    cs.Trn_Status_Code = 0;
                                    status1 = 0;
                                    fmdssStatus = cs.UpdateTransactionStatusForVIP("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                                }
                            }
                            catch (Exception ex)
                            {
                                // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 5: catch in Thread " + ex.Message });
                            }

                            finally
                            {
                                // System.Threading.Monitor.Exit(emitraLock);
                                // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 6: finally execute " });
                            }
                            //}



                        }
                        if (fmdssStatus == 1)
                        {

                            dtrow["TRANSACTION STATUS"] = "SUCCESS";
                            SendSMSEmailForSuccessTransaction();
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 7: SUCCESS : SendSMSEmailForSuccessTransaction " });
                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";
                            // System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 8: FAILED : not SendSMSEmailForSuccessTransaction " });
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion
                    List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();

                    ViewBag.TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();

                    if (ViewBag.TicketStatus == "SUCCESS")
                    {
                        DataTable DTdetails = cs.Get_BookedTicketDetails(Session["RequestId"].ToString(), "VIPBOOKING");

                        foreach (DataRow dr in DTdetails.Rows)
                        {
                            List.Add(
                                   new CS_BoardingDetails()
                                   {
                                       PrintID = Convert.ToString(dr["PrintID"]),
                                       RequestID = Convert.ToString(dr["RequestID"]),
                                       PlaceName = Convert.ToString(dr["PlaceName"]),
                                       Vehicle = Convert.ToString(dr["Vehicle"]),
                                       TotalMembers = Convert.ToString(dr["TotalMembers"]),
                                       DateofBooking = Convert.ToString(dr["DateofBooking"]),
                                       DateofVisit = Convert.ToString(dr["DateofVisit"]),
                                       AmountTobePaid = Convert.ToString(dr["AmountTobePaid"]),
                                       BoardingPointName = Convert.ToString(dr["BoardingPointName"]),
                                   });

                        }

                        if (DTdetails.Rows.Count > 0)
                        {
                            ViewBag.PrintID = Convert.ToString(DTdetails.Rows[0]["PrintID"]);
                        }
                        else
                        {
                            ViewBag.PrintID = "";
                        }

                    }

                    ViewData["TicketSummary"] = List;

                    ViewData.Model = dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                    //  System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { Convert.ToString(Session["RequestId"]) + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 8: Final catch " + ex.Message });
                }
                return View("TransactionStatus");
            }
            return View();
        }

        #endregion
        public ActionResult WildLifeRejectAppoveReport()
        {
            WildLifeRejectAppoveReport model = new WildLifeRejectAppoveReport();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Zone cst = new Zone();
            DataTable dtPlace = new DataTable();
            List<SelectListItem> Places = new List<SelectListItem>();
            dtPlace = cst.SelectPlaces();
            foreach (System.Data.DataRow dr in dtPlace.Rows)
            {
                Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }
            ViewBag.Place = Places;
            WildLifeRejectApprovedReportRepo repo = new WildLifeRejectApprovedReportRepo();
            try
            {
                DataSet DS = new DataSet();
                DS = repo.GetWildLifeRejectApprovedReportData(model);
                if (DS != null)
                {
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.Pending = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }

                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.Failed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }

                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.Approved = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }
                    if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[3]);
                        model.Success = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public ActionResult WildLifeRejectAppoveReport(WildLifeRejectAppoveReport model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Zone cst = new Zone();
            DataTable dtPlace = new DataTable();
            List<SelectListItem> Places = new List<SelectListItem>();
            dtPlace = cst.SelectPlaces();
            foreach (System.Data.DataRow dr in dtPlace.Rows)
            {
                Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }
            ViewBag.Place = Places;
            WildLifeRejectApprovedReportRepo repo = new WildLifeRejectApprovedReportRepo();
            try
            {
                DataSet DS = new DataSet();
                DS = repo.GetWildLifeRejectApprovedReportData(model);
                if (DS != null)
                {
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.Pending = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }

                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.Failed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }

                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.Approved = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }
                    if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[3]);
                        model.Success = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetWildLifeRejectApprovedReportList>>(str);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);
        }


        #region HalfDay Full Day Booking in Wildlife
        public ActionResult WildlifeBookingFD(string CT = "")
        {
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
            //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).AddHours(-10).ToString("yyyy/MM/dd");//Change date 10 AM
            ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
            var MemberList = new List<BookOnTicket>();
            try
            {

                #region Check this is current or Advance
                if (!string.IsNullOrEmpty(CT))
                {
                    string PlaceLblName = Encryption.decrypt(CT);
                    Session["PlaceLbl"] = PlaceLblName;
                    Session["CurrentBookingOrAdvanceBookingFDHD"] = PlaceLblName.ToLower() == "current" ? "1" : "2";
                }
                #endregion

                Session.Remove("AvaliableTicket");
                Session.Remove("VFeeTigerProject");
                Session.Remove("VFeeSurcharge");
                Session.Remove("TotaVechileFees");
                Session.Remove("totalprice");
                Session.Remove("RequestId");
                BookOnTicket Bok = new BookOnTicket();
                DataTable dtPlace = new DataTable();
                //for (int i = 0; i < 20; i++)
                //{
                //    MemberList.Add(new BookOnTicket());
                //}
                DataTable dtf = new DataTable();
                dtf = Bok.Select_BookedTicketForVIPSeats("HALFDAYFULLDAYBOOKING", 5);

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnTicket()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        CancleTicketStatus = Convert.ToInt32(dr["CancleTicketStatus"].ToString()),
                        CancleTicketStatusName = Convert.ToString(dr["CancleTicketStatusName"].ToString()),
                        COVIDStatus = Convert.ToInt32(dr["COVIDStatus"]),
                        TicketMemberBordingStatus = Convert.ToInt32(dr["TicketMemberBordingStatus"]),
                        isDFOApproved = Convert.ToInt32(dr["isDFOApproved"]),
                        RefundStatus = Convert.ToInt32(dr["RefundStatus"]),
                        PlaceId = Convert.ToInt32(dr["PlaceId"]),
                        RequestedId = "" + dr["RequestedId"]

                    });
                }
                ViewData["ticketlist"] = ticketList;
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    #region DeptKioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(Session["SSOid"].ToString(), "GETLOADDATAFORVIPHalfDayFullDay");

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                ////New change for emitra Kiosk 06-09-2018
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
                    //added by rajveer bcz kiosk user not immplemet in wildlife only dept user working so kiosk and dept usr working is same
                    Session["IsDepartmentalKioskUser"] = true;

                    #region KioskUser
                    BookOnTicket bkIsDept = new BookOnTicket();
                    DataTable dtkiosk = new DataTable();
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
                    dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                    if (dtkiosk.Rows.Count > 0)
                    {
                        if (dtkiosk.Rows[0][0].ToString() == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }
                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();
                    Ds = obj.BindDptKioskPLACESVIPSeats(Session["SSOid"].ToString());

                    foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion

                }

                else
                {
                    #region Place
                    dtPlace = Bok.Select_PlaceForVIPOnlineBooking(5);
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }
                //******************************************************************************


                #region OnlineBookingPopUp Developed by Rajveer

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "OnlineBooking";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;

                }
                #endregion

                string exDate = "";
                int IsLeapYear = 0;
                List<string> ExcludeDateList = new List<string>();
                string currentYear = DateTime.Now.Year.ToString();
                int cYear = Convert.ToInt32(currentYear);
                for (int y = cYear; y <= cYear + 1; y++)
                {
                    //for (int i = 7; i < 10; i++)
                    //{

                    //    for (int j = 1; j <= (i == 9 ? 30 : 31); j++)
                    //    {
                    //        exDate = (j < 10 ? "0" + j : "" + j) + "/" + "0" + i + "/" + y.ToString();
                    //        ExcludeDateList.Add(exDate);
                    //    }
                    //}
                    IsLeapYear = y % 4;
                    for (int i = 1; i <= 12; i++)
                    {
                        int toDays = (i == 4 || i == 6 || i == 9 || i == 11 ? 30 : (IsLeapYear == 0 && i == 2 ? 29 : (IsLeapYear > 0 && i == 2 ? 28 : 31)));
                        for (int j = 1; j <= toDays; j++)
                        {
                            if (i < 7 || i > 9)
                            {

                                exDate = (j < 10 ? "0" + j : "" + j) + "/" + (i < 10 ? "0" : "") + i + "/" + y.ToString();

                                DateTime fdate = DateTime.Parse("30/06/2023");
                                DateTime tdate = DateTime.Parse(exDate);

                                if (tdate > fdate)
                                {
                                    string dayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(exDate).DayOfWeek);
                                    if (dayName == "Wednesday")
                                        ExcludeDateList.Add(exDate);
                                }
                            }
                            else
                            {
                                exDate = (j < 10 ? "0" + j : "" + j) + "/" + (i < 10 ? "0" : "") + i + "/" + y.ToString();
                                ExcludeDateList.Add(exDate);
                            }

                        }
                    }
                }

                String excludeList = Newtonsoft.Json.JsonConvert.SerializeObject(ExcludeDateList);

                ViewBag.ExcludeDateList = ExcludeDateList;
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                DateTime mileStoneFromDate = DateTime.ParseExact("10/01/2022", "MM/dd/yyyy", null);
                ViewBag.mileStoneFromDate = mileStoneFromDate;

               
                DateTime today = DateTime.Today; // As DateTime
                string s_today = today.ToString("MM/dd/yyyy"); // As String
                DateTime cDate = DateTime.ParseExact(s_today, "MM/dd/yyyy", null);
                ViewBag.CurrentDate = cDate;
                // Session["IsDepartmentalKioskUser"] = "True";


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            ViewBag.TransMsg = "0";
            if (TempData["RefundSubmissionMsg"] != null)
            {
                ViewBag.TransMsg = TempData["RefundSubmissionMsg"].ToString();
                TempData["RefundSubmissionMsg"] = null;
            }
            if (TempData["RescheduleMsg"] != null)
            {
                ViewBag.TransMsg = TempData["RescheduleMsg"].ToString();
                TempData["RescheduleMsg"] = null;
            }
           
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult FinalSubmitForOnlineBookingFD(List<MemberInfo> lstMemberInfo, List<MemberDetailViewModel> lstMembers, BookOnTicket cs, string Command, FormCollection form, string Message, List<HttpPostedFileBase> fileUpload)
        {

            bool ErrorMessageFlag = true;
            //----Mukesh Jangid Add this below code on 21-03-2022----
            string dtArrival = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).ToShortDateString();
            string dtNow = DateTime.ParseExact(DateTime.Now.AddDays(1).ToShortDateString(), "dd/MM/yyyy", null).ToShortDateString();
            var currTime = DateTime.Now;
            String cTime = currTime.ToString("HH:mm");

            string[] cTimSpl = cTime.Split(':');
            int hh = Convert.ToInt32(cTimSpl[0]); int mm = Convert.ToInt32(cTimSpl[1]);

            DateTime dtArvl = DateTime.ParseExact(dtArrival.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtNw = DateTime.ParseExact(dtNow.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int result = DateTime.Compare(dtArvl, dtNw);

            if (Convert.ToInt16(Session["CurrentBookingOrAdvanceBookingFDHD"]) == 2)
            {
                if (result <= 0)
                {
                    return RedirectToAction("InvalidRequest");
                }
            }
            else
            {
                //if (dtArrival.Trim() == dtNow.Trim())
                if (result <= 0)
                {
                    if (hh < 10)
                    {
                        return RedirectToAction("InvalidRequest");
                    }
                }
            }
            //----Mukesh Jangid Add this above code on 21-03-2022----
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && !this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag && cs.hdn_IAgreement == "1")  //Added by shaan 16-03-2021
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                try
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                    bool IsOpen = objWildlifebooking.IsOpenForDepartmentUser(Convert.ToInt32(cs.PlaceId)); // Check Online booking open for department user
                    if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True" && IsOpen)
                    {
                        return DepartmentUserSubmit(lstMembers, Command, form, cs, fileUpload);
                    }
                    else
                    {

                        return OnlineUserSubmitFD(lstMemberInfo, Command, form, cs, fileUpload);
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                //return View("OnlineTicketPayment");//Comment By Rajveer Kiosk User
            }
            return RedirectToAction("FinalSubmitForOnlineBookingFD");
        }


        public ActionResult OnlineUserSubmitFD(List<MemberInfo> lstMemberInfo, string Command, FormCollection form, BookOnTicket cs, List<HttpPostedFileBase> fileUpload)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int rownumber = 0;
            int rowcount = 0;
            decimal finalAmount = 0;
            foreach (var item in lstMemberInfo)
            {
                if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                {
                    rownumber = Convert.ToInt16(item.MemberSLNo);
                    rowcount++;
                }
            }
            if (rownumber != rowcount)
            {
                TempData["RowCheck"] = "Enter member details continiously";
                return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
            }
            if (Command == "Submit")
            {
                #region MemberInfo
                //DataTable dtMemberInfo = new DataTable();
                //dtMemberInfo = MemberInformation(lstMemberInfo);

                // Check that booking will vehicle wise or seat wise
                string bookingmethod = objWildlifebooking.CheckBookingSeatWiseOrVehicleWise(Convert.ToInt32(cs.PlaceId));
                if (bookingmethod.ToLower() == "v")
                {
                    lstMemberInfo = objWildlifebooking.ReGenrateMemberInfoVechileWiseForOnline(lstMemberInfo);
                }
                DataTable dtMemberInfo = new DataTable();


                long placeId = 0;
                int vehicleId = 0;
                int shifttype = 0;
                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    placeId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    shifttype = Convert.ToInt32(form["ddl_Shift"].ToString());
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    vehicleId = Convert.ToInt32(form["ddl_vehicle"].ToString());
                }
                dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBooking(lstMemberInfo, placeId, vehicleId, shifttype);

                if (dtMemberInfo.Rows.Count == 0)
                {
                    TempData["RowCheck"] = "Enter member details";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                else
                {
                    if (dtMemberInfo.Rows.Count > 0)
                    {
                        decimal FinalAmountTobePaid = 0, Fees_TigerProjectHalfDayFullDayCharge = 0, Fee_SurchargeHalfDayFullDayCharge = 0;
                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                        {
                            FinalAmountTobePaid += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                            Fees_TigerProjectHalfDayFullDayCharge += Convert.ToDecimal(dtMemberInfo.Rows[k]["Fees_TigerProjectHalfDayFullDayCharge"].ToString());
                            Fee_SurchargeHalfDayFullDayCharge += Convert.ToDecimal(dtMemberInfo.Rows[k]["Fee_SurchargeHalfDayFullDayCharge"].ToString());
                            //finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString()) + Convert.ToDecimal(dtMemberInfo.Rows[k]["Fees_TigerProjectHalfDayFullDayCharge"].ToString()) + Convert.ToDecimal(dtMemberInfo.Rows[k]["Fee_SurchargeHalfDayFullDayCharge"].ToString());
                            //finalAmount +=Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString()) + Convert.ToDecimal(dtMemberInfo.Rows[k]["Fees_TigerProjectHalfDayFullDayCharge"].ToString()) + Convert.ToDecimal(dtMemberInfo.Rows[k]["Fee_SurchargeHalfDayFullDayCharge"].ToString());
                        }
                       // finalAmount = Math.Ceiling(FinalAmountTobePaid+ Fees_TigerProjectHalfDayFullDayCharge + Fee_SurchargeHalfDayFullDayCharge);
                        finalAmount = Math.Ceiling(FinalAmountTobePaid) + Math.Ceiling(Fees_TigerProjectHalfDayFullDayCharge) + Math.Ceiling(Fee_SurchargeHalfDayFullDayCharge);
                    }
                }

                #endregion

                #region Submission

                //Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                Session["KioskUserId"] = (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false") ? null : Session["KioskUserId"].ToString();
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" || Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" )
                //if ((Session["KioskUserId"] == null || Session["KioskUserId"] == "" ))
                if ((Session["KioskUserId"] == "" || Session["KioskUserId"] == null) && (Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" && Convert.ToString(Session["IsDepartmentalKioskUser"]).ToLower() == "false"))
                {
                    cs.KioskUserId = "0";

                }
                else
                {
                    cs.KioskUserId = Session["KioskUserId"].ToString();
                }
                //  cs.SsoToken = Request.Cookies["RAJSSO"].Value;
                cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                //System.Threading.Thread.Sleep(10000);    
                //cs.RequestId = DateTime.Now.Ticks.ToString();
                //Session["RequestId"] = cs.RequestId;
                //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
                Session["RequestId"] = RequestId();
                cs.RequestId = Session["RequestId"].ToString();
                //***************************************************************************************



                if (!string.IsNullOrEmpty(Convert.ToString(form["CitizenRemarks"])))
                {
                    cs.CitizenRemarksVal = Convert.ToString(form["CitizenRemarks"]);
                }
                else
                {
                    cs.CitizenRemarksVal = "";
                }

                if (form["ddl_place"].ToString() != "" && form["ddl_place"].ToString() != null)
                {
                    cs.PlaceId = Convert.ToInt64(form["ddl_place"].ToString());
                }
                else
                {
                    cs.PlaceId = 0;
                }
                if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                {
                    cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                }
                else
                {
                    cs.ZoneId = 0;
                }
                if (form["ddl_Shift"].ToString() != "" && form["ddl_Shift"].ToString() != null)
                {
                    cs.ShiftTime = form["ddl_Shift"].ToString();
                }
                else
                {
                    cs.ShiftTime = "";
                }
                cs.TotalMember = Convert.ToInt32(dtMemberInfo.Rows.Count);

                if (form["ArrivalDate"].ToString() != null && form["ArrivalDate"].ToString() != "")
                {

                    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);

                    #region New date check 10 AM Every date if last open booking date(means 365 days) time 8 AM then Current date -1 day booking is open
                    //DateTime Opendate = Convert.ToDateTime(Session["NEWDATEOPEN"]);
                    //if (Convert.ToString(Session["NEWDATEOPEN"]) != null && Convert.ToString(Session["NEWDATEOPEN"]) != "0" && Opendate.ToString("dd/MM/yyyy") == Convert.ToDateTime(form["ArrivalDate"]).ToString("dd/MM/yyyy") && DateTime.Now.Hour < 10)
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                    //}
                    //else
                    //{
                    //    cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                    //}

                    #endregion

                    DataTable DTCheckBooking = new DataTable();
                    #region Restrict Months
                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmit(cs.PlaceId, cs.ArrivalDate);

                    if (DTCheckBooking.Rows.Count > 0)
                    {
                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                        {
                            TempData["datevalidation"] = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                            return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                        }

                    }
                    #endregion

                    #region Get Open Days by rajveer


                    DataTable GetDaysDataTable = new DataTable();
                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBooking(cs.PlaceId);
                    long AddDaysVal = 0;
                    if (GetDaysDataTable.Rows.Count > 0)
                    {
                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                    }
                    #endregion
                    //DateTime expiryDate = DateTime.Today.AddDays(89);
                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                    if (cs.ArrivalDate > expiryDate)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than " + AddDaysVal + " days";
                        return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                    }

                    //Kiosk User Restrictation
                    if (cs.ArrivalDate != DateTime.Now.Date && Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {
                        //TempData["datevalidation"] = "Arrival date should not be more than 90 days";
                        TempData["datevalidation"] = "Arrival date should not be more than Today";
                        return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                    }
                }

                #region Check Last 5 Day Booking In FD and FULL Day
                cs.OldRequestID = Convert.ToString(form["OldRequestID"]);
                cs.OldRequestIDSecound = Convert.ToString(form["OldRequestIDSecound"]);
                cs.txt_OldIDProof = Convert.ToString(form["OldIDProof"]);


                if (string.IsNullOrEmpty(cs.OldRequestID))
                {
                    TempData["datevalidation"] = "Please enter a vaild previous Request ID!!!";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                if (string.IsNullOrEmpty(cs.OldRequestIDSecound))
                {
                    TempData["datevalidation"] = "Please enter a vaild previous Request ID!!!";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                else if (string.IsNullOrEmpty(cs.txt_OldIDProof))
                {
                    TempData["datevalidation"] = "Please enter a vaild ID Proof as mention in previous Request ID !!!";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                else if (lstMemberInfo.Count(s => s.MemberIdNo.Trim().ToLower() == cs.txt_OldIDProof.Trim().ToLower()) == 0)
                {
                    TempData["datevalidation"] = "Please enter the same ID Proof submited in the previous booking ids as mentioned above.";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                else if (cs.OldRequestIDSecound.Trim().ToLower() == cs.OldRequestID.Trim().ToLower())
                {
                    TempData["datevalidation"] = "The previous request ids should not be same !";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                else
                {
                    DataTable DTCheckLastBookingDuration = new DataTable();
                    DTCheckLastBookingDuration = cs.CheckOldRequestIdFD(cs.PlaceId, cs.OldRequestID, cs.OldRequestIDSecound, Convert.ToInt64(Session["UserID"]), Convert.ToString(Session["IsDepartmentalKioskUser"]), form["ArrivalDate"].ToString(), cs.txt_OldIDProof);
                    if (DTCheckLastBookingDuration != null && DTCheckLastBookingDuration.Rows.Count > 0 && Convert.ToString(DTCheckLastBookingDuration.Rows[0]["Status"]) == "0")
                    {
                        TempData["datevalidation"] = Convert.ToString(DTCheckLastBookingDuration.Rows[0]["msg"]);
                        return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                    }
                }
                #endregion

                if (TempData["DurationTo"] != null)
                {

                    DateTime MaxDate = DateTime.ParseExact(TempData["DurationTo"].ToString(), "dd/MM/yyyy", null);
                    if (cs.ArrivalDate > MaxDate)
                    {
                        TempData["datevalidation"] = "Arrival date should not be more than " + MaxDate;
                        return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                    }
                }
                if (form["ddl_vehicle"].ToString() != "" && form["ddl_vehicle"].ToString() != null)
                {
                    cs.vehicleID = Convert.ToInt32(form["ddl_vehicle"].ToString());
                    Session["VehicleId"] = cs.vehicleID;
                }
                else
                {
                    cs.vehicleID = 0;
                }
                if (Session["VFeeTigerProject"] != null)
                {

                    cs.VehicleFees_TigerProject = Convert.ToDecimal(Session["VFeeTigerProject"].ToString());
                }
                else
                {
                    cs.VehicleFees_TigerProject = 0;
                }
                if (Session["VFeeSurcharge"] != null)
                {

                    cs.VehicleFees_Surcharge = Convert.ToDecimal(Session["VFeeSurcharge"].ToString());
                }
                else
                {
                    cs.VehicleFees_Surcharge = 0;
                }
                if (Session["TotaVechileFees"] != null)
                {

                    cs.VehicleFees_Total = Convert.ToDecimal(Session["TotaVechileFees"].ToString());
                }
                else
                {
                    cs.VehicleFees_Total = 0;
                }
                if (form["ddl_Accomo"].ToString() != "" && form["ddl_Accomo"].ToString() != null)
                {
                    cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                }
                else
                { cs.AccomoID = 0; }
                if (form["TotalRoom"].ToString() != "" && form["TotalRoom"].ToString() != null)
                {
                    cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                }
                else
                { cs.TotalRoom = 0; }
                if (form["RoomCharge"].ToString() != "" && form["RoomCharge"].ToString() != null)
                {
                    cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                }
                else
                { cs.RoomCharge = 0; }

                #region CheckBooking Quota
                string quotaName = string.Empty;
                int cwlwquota = 0;
                int ccfquota = 0;
                int inchargequota = 0;
                var setting = objWildlifebooking.GetOnlineBookingSetting(Convert.ToInt32(placeId));
                if (!string.IsNullOrEmpty(Convert.ToString(form["ddlQuota"])))
                {
                    quotaName = Convert.ToString(form["ddlQuota"].ToString());
                    if (quotaName == "CWLW")
                        cwlwquota = cs.TotalMember;
                    else if (quotaName == "CCF")
                        ccfquota = cs.TotalMember;
                    else if (quotaName == "Incharge")
                        inchargequota = cs.TotalMember;

                    DataTable CheckQuotaSeatsAvalibality = objWildlifebooking.CheckQuotaSeatsAvalibality(Convert.ToInt32(placeId), cs.ArrivalDate, cs.vehicleID);
                    if (CheckQuotaSeatsAvalibality.Rows.Count > 0 && setting.IsQuotaSeatsOnandOff == 1)
                    {
                        int cwlwremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCWLWSeats"]);
                        int ccfremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainingCCFSeats"]);
                        int inchargeremainingseats = Convert.ToInt32(CheckQuotaSeatsAvalibality.Rows[0]["RemainningInchargeSeats"]);
                        if (quotaName == "CWLW")
                        {
                            if (cwlwquota > cwlwremainingseats)
                            {
                                TempData["CheckQuota"] = cwlwremainingseats + " seats available in cwlw quota";
                                return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "CCF")
                        {
                            if (ccfquota > ccfremainingseats)
                            {
                                TempData["CheckQuota"] = ccfremainingseats + " seats available in ccf quota";
                                return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                            }
                        }
                        else if (quotaName == "Incharge")
                        {
                            if (inchargequota > inchargeremainingseats)
                            {
                                TempData["CheckQuota"] = inchargeremainingseats + " seats available in incharge quota";
                                return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                            }
                        }


                    }
                }

                #endregion


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
                cs.IPAddress = result;
                DataTable dts = new DataTable();

                DataTable dtcheckTicket = new DataTable();
                string strcheckTicket = string.Empty;
                dtcheckTicket = cs.CheckTicketAvailabilityForOnlineBooking();
                strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                {
                    dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                    dtMemberInfo.AcceptChanges();
                    dts = cs.Submit_TicketDetailsForOnlineBooking(dtMemberInfo, finalAmount);
                    ViewData["CitizenRemarks"] = cs.CitizenRemarksVal;

                }
                else
                {
                    TempData["RowCheck"] = "Ticket not avaliable";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }
                if (dts.Rows.Count > 0)
                {
                    if (setting.IsQuotaSeatsOnandOff == 1)
                    {
                        objWildlifebooking.SaveOnlineQuotaSeats(cs.RequestId, Convert.ToInt32(cs.PlaceId), cwlwquota, ccfquota, inchargequota, cs.ArrivalDate, DateTime.Now, Convert.ToInt32(UserID), cs.vehicleID);
                    }

                    #region Save File
                    string FileFullName = string.Empty;
                    string FilePath = "~/OnlineBookingVIPDocuments/";
                    string path;
                    List<DocumentList> docModel = new List<DocumentList>();
                    if (fileUpload != null)
                    {
                        int i = 0;
                        foreach (var itm in fileUpload)
                        {
                            if (itm != null)
                            {
                                DocumentList view = new DocumentList();
                                FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
                                path = Path.Combine(FilePath, FileFullName);
                                view.FileName = itm.FileName;
                                view.FilePath = Path.Combine(FilePath, FileFullName);
                                view.FileType = 1;//Means DSR1 
                                Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                                docModel.Add(view);
                                i++;
                            }
                        }
                        if (docModel.Count > 0)
                        {
                            int c = objWildlifebooking.SaveOnlineFileUploder("SAVE", cs.RequestId, Convert.ToInt32(cs.PlaceId), Convert.ToInt64(UserID), 1, docModel);
                        }
                    }
                    #endregion

                    decimal finalAmnt = Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    if (Session["totalprice"].ToString() == "0")
                    {
                        TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                        return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                    }
                }
                else
                {
                    TempData["datevalidation"] = "Booking quota exhausted for today session!!!";
                    return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket");
                }


                #region Add Kiosk User by Rajveer
                EducationTours edu = new EducationTours();
                edu.Location = Convert.ToInt64(cs.PlaceId);
                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                    {
                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                    }
                }

                // Session["KioskUserId"] = Convert.ToString(Session["IsKioskUser"]).ToLower() == "false" ? null : Session["KioskUserId"].ToString();
                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 1;
                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Convert.ToString(cs.RequestId);
                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);
                    if (dtKiosk.Rows.Count > 0)
                    {
                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                        ViewBag.ViewModel = dts.AsEnumerable();
                        return PartialView("KioskPaymentDetailWildlife", _obj);
                    }
                }
                else
                {
                    ViewData.Model = dts.AsEnumerable();
                    return View("OnlineTicketPaymentKiosk");
                }
                #endregion


                // ViewData.Model = dts.AsEnumerable();//Comment By Rajveer Kiosk User
                #endregion
            }
            return RedirectToAction("WildlifeBookingFD");
        }

        [HttpPost, ValidateHeaderAntiForgeryToken]
        public JsonResult CheckSafariAccomoAvailabilityVIPOnlineBookingHDFD(int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> lstZone = new List<SelectListItem>();
            List<SelectListItem> lstQuotaSeats = new List<SelectListItem>();
            BookOnTicket bot1 = null;
            int isquotaOnoroff = 0;
            string NEWDATEOPEN = string.Empty;
            string OpenBookingDays = "0";
            string CurrentBookingDays = "0";
            try
            {
                BookOnTicket bot = new BookOnTicket();
                DataSet dsSafariAccomodation = new DataSet();
                bot.PlaceId = Convert.ToInt64(PlaceID);
                dsSafariAccomodation = bot.chkSafariAccomoForVIPOnlineBookingFDFD(PlaceID);
                if (dsSafariAccomodation.Tables.Count > 0)
                {
                    bot1 = new BookOnTicket();
                    if (dsSafariAccomodation.Tables[0].Rows.Count > 0)
                    {
                        bot1.isSafari = dsSafariAccomodation.Tables[0].Rows[0]["IsSafari"].ToString();
                        bot1.isAccomo = dsSafariAccomodation.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {
                        bot1.isSafari = "";
                        bot1.isAccomo = "";
                    }
                    if (dsSafariAccomodation.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in dsSafariAccomodation.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }

                    #region check quota seats on or off
                    var quotaSetting = objWildlifebooking.GetOnlineBookingSetting(PlaceID);
                    isquotaOnoroff = quotaSetting.IsQuotaSeatsOnandOff;
                    var quotaSeats = objWildlifebooking.GetQuotaSeats(PlaceID, 0);
                    if (quotaSeats.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in quotaSeats.Rows)
                        {
                            lstQuotaSeats.Add(new SelectListItem { Text = @dr["QuotaSeats"].ToString(), Value = @dr["Name"].ToString() });
                        }
                    }
                    #endregion


                    if (dsSafariAccomodation.Tables[2].Rows.Count > 0)
                    {
                        bot1.DurationFrom = dsSafariAccomodation.Tables[2].Rows[0]["DurationFromDate"].ToString();
                        bot1.DurationTo = dsSafariAccomodation.Tables[2].Rows[0]["DurationToDate"].ToString();
                        TempData["DurationTo"] = bot1.DurationTo;
                    }
                    else
                    {

                        bot1.DurationFrom = "NF";
                        bot1.DurationTo = "NF";
                        TempData["DurationTo"] = "";
                    }
                    Session["NEWDATEOPEN"] = "0";
                    if (dsSafariAccomodation.Tables[3].Rows.Count > 0)
                    {
                        NEWDATEOPEN = Convert.ToString(dsSafariAccomodation.Tables[3].Rows[0]["NEWDATEOPEN"]);
                        ViewBag.NEWDATEOPEN = NEWDATEOPEN;

                        #region open Date 10 AM every day
                        Session["NEWDATEOPEN"] = null;
                        Session["NEWDATEOPEN"] = NEWDATEOPEN;
                        #endregion

                        //ViewBag.currentDate = DateTime.Now.Date.AddDays(1).ToString("yyyy/MM/dd");--Change Date 00.00
                        ViewBag.currentDate = DateTime.Now.Date.AddDays(2).AddHours(-10).ToString("MMM dd, yyyy");//Change date 10 AM
                    }
                    if (dsSafariAccomodation.Tables[4].Rows.Count > 0)
                    {
                        OpenBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["OpenBookingDuration"].ToString();
                        CurrentBookingDays = dsSafariAccomodation.Tables[4].Rows[0]["CurrentDateOpen"].ToString();
                    }
                }
                else
                {
                    bot1 = new BookOnTicket();
                    bot1.isSafari = "";
                    bot1.isAccomo = "";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = bot1.isSafari, list2 = bot1.isAccomo, list3 = lstZone, list4 = bot1.DurationFrom, list5 = bot1.DurationTo, NEWDATEOPEN = NEWDATEOPEN, list6 = OpenBookingDays, list7 = CurrentBookingDays, list8 = lstQuotaSeats, list9 = isquotaOnoroff }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CheckPreviousOnlineBooking(int placeID, string ArrivalDate, string OldRequestID, string OldRequestIDSecound, string OldIdProof)
        {
            string strStatus = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            BookOnTicket cs = new BookOnTicket();
            CheckPreviousBookingModel modelc = new CheckPreviousBookingModel();
            try
            {
                #region Check Last 5 Day Booking In FD and FULL Day

                if (string.IsNullOrEmpty(OldRequestID))
                {
                    modelc.msg = "Please enter a vaild previous Request ID!!!";
                    modelc.Status = 0;
                }
                if (string.IsNullOrEmpty(OldRequestIDSecound))
                {
                    modelc.msg = "Please enter a vaild previous Request ID!!!";
                    modelc.Status = 0;
                }
                else if (string.IsNullOrEmpty(OldIdProof))
                {
                    modelc.msg = "Please enter a vaild ID Proof as mention in previous Request ID !!!";
                    modelc.Status = 0;
                }
                else if (OldRequestIDSecound.Trim().ToLower() == OldRequestID.Trim().ToLower())
                {
                    modelc.msg = "The previous request ids should not be same !";
                    modelc.Status = 0;
                }
                else
                {
                    DataTable DTCheckLastBookingDuration = new DataTable();
                    DTCheckLastBookingDuration = cs.CheckOldRequestIdFD(placeID, OldRequestID, OldRequestIDSecound, Convert.ToInt64(Session["UserID"]), Convert.ToString(Session["IsDepartmentalKioskUser"]), ArrivalDate, OldIdProof);
                    if (DTCheckLastBookingDuration != null && DTCheckLastBookingDuration.Rows.Count > 0)
                    {
                        List<CheckPreviousBookingModel> list = new List<CheckPreviousBookingModel>();
                        string str = JsonConvert.SerializeObject(DTCheckLastBookingDuration);
                        modelc = JsonConvert.DeserializeObject<List<CheckPreviousBookingModel>>(str).FirstOrDefault();
                    }
                    else
                    {
                        modelc.msg = "Some Error occured!";
                        modelc.Status = 0;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(modelc);
        }
        #endregion


        public JsonResult IsOTPValidationApply()
        {
            BookOnTicket cs = new BookOnTicket();
            //DataTable dt = cs.IsOTPShow();
            //string IsShowOTP = dt.Rows[0][0].ToString();
            string IsShowOTP = "0";
            return Json(IsShowOTP, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult BookingStartCounter()
        {
            #region Booking on 10:00 AM login    //changes done on 14-10-2020 by shaan
            BookOnTicket Bok = new BookOnTicket();
            DataTable dtServerTime = Bok.GetServerTimeForCurrentBooking();
            Session["ServerTime"] = dtServerTime.Rows[0]["OnlyTime"].ToString();
            DateTime CurrentTime = Convert.ToDateTime(Session["ServerTime"].ToString());
            DateTime t2 = Convert.ToDateTime("10:00:00 AM");
            if (CurrentTime >= t2)
            {
                return RedirectToAction("BookOnlineTicket", "BookOnlineTicket", new { CT = "Q3VycmVudA==" });
            }

            #endregion
            return View();
        }

        public ActionResult ReScheduleBookingHDFD(string ticketid)
        {
            ReSchedule reSchedule = new ReSchedule();
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            //DataTable DT2 = new DataTable();
            //DataTable DT3 = new DataTable();
            //bool flag = false;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                DataSet ds = new DataSet();
                ReSchedule cs = new ReSchedule();

                string en = Encryption.encrypt(ticketid);
                Convert.ToInt64(Encryption.decrypt(ticketid));

                ds = cs.Select_TicketData(Convert.ToInt64(Encryption.decrypt(ticketid)));

                List<ReSchedule> lstoCovid = new List<ReSchedule>();


                if (ds.Tables.Count > 0)
                {
                    reSchedule.TicketId = ticketid;
                    reSchedule.RequestID = Convert.ToString(ds.Tables[0].Rows[0]["RequestID"].ToString());
                    reSchedule.PlaceName = Convert.ToString(ds.Tables[0].Rows[0]["PlaceName"]);
                    reSchedule.PlaceId = Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"]);
                    reSchedule.ShiftName = Convert.ToString(ds.Tables[0].Rows[0]["Shift"]);
                    reSchedule.DateofArrival = "";//Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    reSchedule.DateofArrival1 = Convert.ToString(ds.Tables[0].Rows[0]["DateofArrival"]);
                    reSchedule.TotalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["AmountTobePaid"]);


                    reSchedule.lstMemberDetails = new List<ReScheduleMemberDetails>();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        reSchedule.VehicleName = Convert.ToString(dr["vName"]);
                        ReScheduleMemberDetails memberdetails = new ReScheduleMemberDetails();
                        memberdetails.Name = Convert.ToString(dr["Name"]);
                        memberdetails.Nationality = Convert.ToString(dr["Nationality"]);
                        memberdetails.IdProof = Convert.ToString(dr["IdProof"]);
                        memberdetails.NoofCamera = Convert.ToString(dr["NoofCamera"]);
                        memberdetails.MemberFees = Convert.ToString(dr["MemberFee"]);
                        memberdetails.CameraFees = Convert.ToString(dr["CameraFee"]);
                        memberdetails.VehicleFees = Convert.ToString(dr["VehicleFee"]);
                        memberdetails.BoardingVehicleFee = Convert.ToString(dr["BoardingVehicleFee"]);
                        memberdetails.BoardingGuideFeeGSTAmount = Convert.ToString(dr["BoardingGuideFeeGSTAmount"]);
                        memberdetails.BoardingVehicleFeeGstAmount = Convert.ToString(dr["BoardingVehicleFeeGstAmount"]);

                        memberdetails.BoardingGuideFee = Convert.ToString(dr["BoardingGuideFee"]);
                        memberdetails.Amount = Convert.ToString(dr["Amount"]);

                        reSchedule.lstMemberDetails.Add(memberdetails);


                    }

                    ViewBag.RQid = reSchedule.RequestID;
                    DT1 = cs.GetZoneList(2); //2 for ranthambor normal booking
                    List<SelectListItem> zoneList = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in DT1.Rows)
                    {
                        zoneList.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }
                    ViewBag.ZoneList = zoneList;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(reSchedule);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReScheduleBookingHDFD(ReSchedule rescheduleBooking)
        {
            //Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            //BookOnTicket Bok = new BookOnTicket();
            //DataTable DT = Bok.IsValidUser(UserID);
            //if (Convert.ToInt16(DT.Rows[0][0]) == 0)
            //{
            //	Response.Redirect("~/UnAuthorizedUser.html", true);
            //}
            int liveUat= Convert.ToInt16( ConfigurationManager.AppSettings["IsLiveOrUAT"].ToString());
            bool IsLiveOrUAT =(liveUat==0? false:true);
            if (ModelState.IsValid == true)
            {
                DataTable DT = new DataTable();
                ReSchedule cs = new ReSchedule();
                rescheduleBooking.RequestID = RequestId();
                DT = cs.SaveReScheduleBooking(rescheduleBooking, IsLiveOrUAT);
                string RequestIdList = DT.Rows[0]["RequestIdList"].ToString();

                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                //objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenReScheduleRequest", Convert.ToString(DT.Rows[0]["RequestIdList"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");

                TempData["RescheduleMsg"]= "Your visit has been rescheduled for Ranthambore National Park successfully and new permit Id is : "+ RequestIdList;
                return RedirectToAction("WildlifeBookingFD", "BookOnlineTicket", new { CT = "QWR2YW5jZQ ==" });
            }

            return View();
        }

        public ActionResult InvalidRequest()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }
        public ActionResult BookingWarningMessage()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }
        #region Report added by shaan 29-01-2021
        public ActionResult WildLifeBookingList()
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
        public JsonResult GetWildLifeTicketList(string DateType, string FromDate, string ToDate, string Place, string TypeOfBooking, string Status)
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

                dataSet = BOT.WildLifebookedTicketList(WBFM);


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
        #endregion

        //[HttpPost]
        //public JsonResult ReVerifyPayment(string requestedId)
        //{
        //    BookOnTicket cs = new BookOnTicket();
        //    DataTable dt = cs.ReVeriyPayment(requestedId);
                  
        //    var data = new { ReturnMsg = dt.Rows[0][0].ToString() };
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}
