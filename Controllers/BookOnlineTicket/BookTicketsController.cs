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
namespace FMDSS.Controllers.BookOnlineTicket
{
    public class BookTicketsController : Controller
    {

        int ModuleID = 1;
        List<SelectListItem> lstPlace = new List<SelectListItem>();
        List<SelectListItem> Accomodation = new List<SelectListItem>();
        List<BookOnTicket> ticketList = new List<BookOnTicket>();
        BookOnTicket objBook = new BookOnTicket();
        WildLifeOnlineBooking objWildlifebooking = new WildLifeOnlineBooking();
        //***********************Code for allow single request at a time by rajkumar on 19-Sept-2016*****************
        static object slno = new object();
        static object emitraLock = new object();
        static string RequestId()
        {
            lock (slno)
            {
                string requestid = DateTime.Now.Ticks.ToString();
                return requestid;
            }
        }
        //
        // GET: /BookTickets/

        public ActionResult OnlineBooking(string PlaceName = "")
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
        //
      
    }
}
