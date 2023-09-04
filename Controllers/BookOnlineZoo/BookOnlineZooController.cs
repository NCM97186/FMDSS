//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : BookOnlineZooController
//  Description  : File contains calling functions from UI
//  Date Created : 26-09-2016
//  History      :
//  Version      : 1.0
//  Author       : Rajkumar
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.BookOnlineZoo;
using FMDSS.Models.BookOnlineTicket;
using System.IO;
using FMDSS.Models.CitizenService.PermissionService;
using System.Configuration;
using FMDSS.Models;
using System.Text;
using FMDSS.Filters;
using FMDSS.Models.OnlineBooking;
using Newtonsoft.Json;
using FMDSS.Models.CitizenService.PermissionService.EducationService;
using System.Linq;
using System.Globalization;
using RestSharp;
using FMDSS.Repository.Interface;
using FMDSS.Repository;
using FMDSS.Models.CommanModels;

namespace FMDSS.Controllers.BookOnlineZoo
{
    [MyAuthorization]
    public class BookOnlineZooController : BaseController
    {
        //
        // GET: /BookOnlineZoo/
        int ModuleID = 1;
        string actionName = string.Empty;
        string controllerName = string.Empty;
        List<BookOnZoo> ticketList = new List<BookOnZoo>();
        /// <summary>
        /// Function to return view of online zoo
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> lstShiftType = new List<SelectListItem>();
        private ICommonRepository _commonRepository;

        #region [Constructor]
        public BookOnlineZooController()
        {
            _commonRepository = new CommonRepository();
        }
        #endregion

        public ActionResult BookOnlineZoo()
        {
            actionName = Convert.ToString(this.ControllerContext.RouteData.Values["action"]);
            controllerName = Convert.ToString(this.ControllerContext.RouteData.Values["controller"]);
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            BookOnZoo obj = new BookOnZoo();
            List<SelectListItem> lstPlace = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            try
            {
                #region Remove All Requested Ids
                Session.Remove("AvaliableTicket");
                Session.Remove("totalprice");
                Session.Remove("ZooRequestId");

                #endregion

                ViewBag.DeptKioskStatus = "False";
                ViewBag.KioskStatus = "False";
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
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
                        if (Convert.ToString(dtkiosk.Rows[0][0]) == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }

                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();

                    DataTable DT_BoardingDuration;


                    Ds = obj2.BindDptKioskPLACES(Convert.ToString(Session["SSOid"]));

                    CS_BoardingDetails objB = new CS_BoardingDetails();

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                    DT_BoardingDuration = objB.GetBoardingDuration(Convert.ToString(Ds.Tables[0].Rows[0]["PlaceID"]));
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {


                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                        {
                            // =========== FullDayShift
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                            {

                                lstShiftType.Add(new SelectListItem { Text = "FullDay", Value = "3" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            //// =========== MORNING_SHIFT
                            //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            //{
                            //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                            //}
                            //else 
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }

                        }

                        if (lstShiftType.Count == 0)
                        {
                            ViewBag.statusoftime = "False";
                        }
                        else
                        {
                            ViewBag.statusoftime = "True";
                        }


                    }
                    ViewBag.Place = lstPlace;

                    if (Convert.ToString(DateTime.Now.DayOfWeek) == "Tuesday")
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));

                    }
                    else
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }

                    #endregion
                }
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";

                    #region EMITRAKioskUser
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
                        if (Convert.ToString(dtkiosk.Rows[0][0]) == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }

                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();

                    DataTable DT_BoardingDuration;


                    Ds = obj2.BindDptKioskPLACES(Convert.ToString(Session["SSOid"]));

                    CS_BoardingDetails objB = new CS_BoardingDetails();

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                    DT_BoardingDuration = objB.GetBoardingDuration(Convert.ToString(Ds.Tables[0].Rows[0]["PlaceID"]));

                    if (DT_BoardingDuration.Rows.Count > 0)
                    {


                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                        {
                            // =========== FullDayShift
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                            {

                                lstShiftType.Add(new SelectListItem { Text = "FullDay", Value = "3" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            //// =========== MORNING_SHIFT
                            //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            //{
                            //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                            //}
                            //else 
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }

                        }

                        if (lstShiftType.Count == 0)
                        {
                            ViewBag.statusoftime = "False";
                        }
                        else
                        {
                            ViewBag.statusoftime = "True";
                        }

                    }
                    ViewBag.Place = lstPlace;


                    if (Convert.ToString(DateTime.Now.DayOfWeek) == "Tuesday")
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }


                    #endregion
                }
                else
                {
                    ViewBag.KioskStatus = "False";
                    #region CITIZEN
                    dtPlace = obj.Select_Place();
                    foreach (System.Data.DataRow dr in dtPlace.Rows)
                    {
                        lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                    }
                    ViewBag.Place = lstPlace;
                    #endregion
                }

                ViewBag.ShiftTypes = lstShiftType;

                //******************************************************************************

                DataTable dtf = new DataTable();
                dtf = obj.Select_BookedTicket();
                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnZoo()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"]),
                        RequestId = Convert.ToString(dr["RequestedId"]),
                        TotalAmount = Convert.ToDecimal(dr["EmitraAmount"]),
                        EmitraTransactionId = Convert.ToString(dr["EmitraTransactionID"]),
                        DateOfArrival = Convert.ToString(dr["DateOfArrival"]),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"])
                    });
                }
                ViewData["ticketlist"] = ticketList;
                TempData["CurrentDate"] = DateTime.Now.ToString("MM/dd/yyyy");
                return View();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }


        #region Camera And Vehicle Extra Inventory
        public ActionResult BookOnlineZooExtraInventory()
        {
            actionName = Convert.ToString(this.ControllerContext.RouteData.Values["action"]);
            controllerName = Convert.ToString(this.ControllerContext.RouteData.Values["controller"]);
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            BookOnZoo obj = new BookOnZoo();
            List<SelectListItem> lstPlace = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            try
            {
                #region Remove All Requested Ids
                Session.Remove("AvaliableTicket");
                Session.Remove("totalprice");
                Session.Remove("ZooRequestId");

                #endregion

                ViewBag.DeptKioskStatus = "False";
                ViewBag.KioskStatus = "False";
                //*******************For Departmental Kiosk User********************************
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";
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
                        if (Convert.ToString(dtkiosk.Rows[0][0]) == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }

                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();

                    DataTable DT_BoardingDuration;


                    Ds = obj2.BindDptKioskPLACES(Convert.ToString(Session["SSOid"]));

                    CS_BoardingDetails objB = new CS_BoardingDetails();

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                    DT_BoardingDuration = objB.GetBoardingDuration(Convert.ToString(Ds.Tables[0].Rows[0]["PlaceID"]));
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {


                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                        {
                            // =========== FullDayShift
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                            {

                                lstShiftType.Add(new SelectListItem { Text = "FullDay", Value = "3" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            //// =========== MORNING_SHIFT
                            //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            //{
                            //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                            //}
                            //else 
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }

                        }

                        if (lstShiftType.Count == 0)
                        {
                            ViewBag.statusoftime = "False";
                        }
                        else
                        {
                            ViewBag.statusoftime = "True";
                        }


                    }
                    ViewBag.Place = lstPlace;

                    if (Convert.ToString(DateTime.Now.DayOfWeek) == "Tuesday")
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));

                    }
                    else
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }

                    #endregion
                }
                else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {
                    ViewBag.KioskStatus = "True";
                    ViewBag.DeptKioskStatus = "True";

                    #region EMITRAKioskUser
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
                        if (Convert.ToString(dtkiosk.Rows[0][0]) == "0")
                        {
                            TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                            return RedirectToAction("KioskDashboard", "KioskDashboard");
                        }

                    }

                    FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                    DataSet Ds = new DataSet();

                    DataTable DT_BoardingDuration;


                    Ds = obj2.BindDptKioskPLACES(Convert.ToString(Session["SSOid"]));

                    CS_BoardingDetails objB = new CS_BoardingDetails();

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                    DT_BoardingDuration = objB.GetBoardingDuration(Convert.ToString(Ds.Tables[0].Rows[0]["PlaceID"]));

                    if (DT_BoardingDuration.Rows.Count > 0)
                    {


                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                        {
                            // =========== FullDayShift
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                            {

                                lstShiftType.Add(new SelectListItem { Text = "FullDay", Value = "3" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            //// =========== MORNING_SHIFT
                            //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            //{
                            //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                            //}
                            //else 
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });

                                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                                {
                                    lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                                }

                            }

                        }

                        if (lstShiftType.Count == 0)
                        {
                            ViewBag.statusoftime = "False";
                        }
                        else
                        {
                            ViewBag.statusoftime = "True";
                        }

                    }
                    ViewBag.Place = lstPlace;


                    if (Convert.ToString(DateTime.Now.DayOfWeek) == "Tuesday")
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                    }


                    #endregion
                }
                //else
                //{
                //    ViewBag.KioskStatus = "False";
                //    #region CITIZEN
                //    dtPlace = obj.Select_Place();
                //    foreach (System.Data.DataRow dr in dtPlace.Rows)
                //    {
                //        lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                //    }
                //    ViewBag.Place = lstPlace;
                //    #endregion
                //}

                ViewBag.ShiftTypes = lstShiftType;

                //******************************************************************************

                //DataTable dtf = new DataTable();
                //dtf = obj.Select_BookedTicket();
                //foreach (DataRow dr in dtf.Rows)
                //{
                //    ticketList.Add(new BookOnZoo()
                //    {
                //        TicketID = Convert.ToInt64(dr["TicketID"]),
                //        TotalAmount = Convert.ToDecimal(dr["EmitraAmount"]),
                //        EmitraTransactionId = Convert.ToString(dr["EmitraTransactionID"]),
                //        DateOfArrival = Convert.ToString(dr["DateOfArrival"]),
                //        TotalMember = Convert.ToInt32(dr["TotalMembers"])
                //    });
                //}
                ViewData["ticketlist"] = ticketList;
                TempData["CurrentDate"] = DateTime.Now.ToString("MM/dd/yyyy");
                return View();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public ActionResult ShowPartialViewExtraInventory(string PlaceId)
        {
            var lstVehicle = new List<BookOnZoo>();
            BookOnZoo obj = new BookOnZoo();
            List<BookOnZoo> lstMember = new List<BookOnZoo>();
            DataSet dsMemberVehle = new DataSet();
            obj.PlaceOfVisit = PlaceId;
            dsMemberVehle = obj.MemberVehicleDetailsExtraInventory();
            foreach (DataRow dr in dsMemberVehle.Tables[0].Rows)
            {
                lstMember.Add(new BookOnZoo()
                {
                    TypeOfMember = dr["FeeChargedOn"].ToString(),
                    NoOfMember = "",
                    FeePerMember = dr["HeadAmount"].ToString(),
                    //TypeOfCamera="",
                    //NoOfCamera = "",
                    //FeePerCamera = dr["CameraAmount"].ToString(),

                    NoOfStillCamera = "",
                    FeePerStillCamera = dr["StillCameraAmount"].ToString(),

                    NoOfVideoCamera = "",
                    FeePerVideoCamera = dr["VideoCameraAmount"].ToString(),

                    TotalFeesOfMember = ""
                });
            }

            foreach (DataRow dr in dsMemberVehle.Tables[1].Rows)
            {
                lstVehicle.Add(new BookOnZoo()
                {
                    TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                    FeePerVehicle = dr["HeadAmount"].ToString(),
                    NoOfVehicle = "",
                    TotalVehicleFee = ""
                });
            }
            var MemberPartialView = RenderRazorViewToString(this.ControllerContext, "ZooMemberInfoExtraInventory", lstMember);
            var VehiclePartialView = RenderRazorViewToString(this.ControllerContext, "ZooVehicleInfoExtraInventory", lstVehicle);
            var VehicleStatus = "FLASE";
            if (dsMemberVehle.Tables[1].Rows.Count > 0)
            {
                VehicleStatus = "TRUE";
            }

            List<BookOnZoo> LstShiftType = new List<BookOnZoo>();

            foreach (DataRow dr in dsMemberVehle.Tables[2].Rows)
            {
                LstShiftType.Add(new BookOnZoo()
                {
                    ShiftTypeID = dr["ID"].ToString(),
                    ShiftTypeName = dr["Name"].ToString(),
                });
            }
            string Holidays = string.Empty;

            //if (dsMemberVehle.Tables[3].Rows.Count > 0)
            //{

            //    Holidays = Convert.ToString(dsMemberVehle.Tables[3].Rows[0][0]);

            //}



            ViewBag.ShiftTypeIDs = Convert.ToString(dsMemberVehle.Tables[2].Rows[0]["ID"]);

            var json = Json(new { MemberPartialView, VehiclePartialView, VehicleStatus, LstShiftType, Holidays = Holidays });
            return json;
        }

        [HttpPost]
        public ActionResult FinalSubmitExtraInventory(List<MemberInformation> lstMember, List<VehicleInformation> lstVehicle, BookOnZoo bz, HttpPostedFileBase DocumentForTour, HttpPostedFileBase UploadId)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string Document = string.Empty;
            string IdUpload = string.Empty;
            var path = "";
            string FilePath = "~/ZoologicalDocument/";
            int totalmember = 0;


            try
            {
                BookOnZoo boz = new BookOnZoo();
                DateTime date = DateTime.ParseExact(bz.DateOfVisit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DataTable dtHoliday = bz.CheckHolidays(bz.PlaceOfVisit, (int)date.DayOfWeek);

                string ips = string.Empty;
                string ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipaddress))
                {
                    string[] ipRange = ipaddress.Split(',');
                    int le = ipRange.Length - 1;
                    ips = ipRange[0];
                }
                else
                {
                    ips = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                bz.IPAddress = ips;

                #region Check Same IP,Shift Type,Date of Visit , Enter Date,SSOID and Place Developed By Rajveer
                //string message=string.Empty;
                //bool flag = boz.CheckSameIPBooking(bz, ref message);
                //if(flag==false)
                //{
                //    TempData["EmptyChk"] = message;
                //    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                //}
                #endregion

                #region Check Extra Inventory Details 
                DataTable CheckExtraInventory = new DataTable();
                CheckExtraInventory = boz.CheckTicketExtraInventory("Check", Convert.ToInt32(bz.PlaceOfVisit), Convert.ToString(bz.OldRequestID.Trim()), bz.DateOfVisit, Convert.ToInt64(Session["KioskUserId"]));
                if (CheckExtraInventory.Rows.Count > 0)
                {
                    if (Convert.ToString(CheckExtraInventory.Rows[0]["status"]) == "0")
                    {
                        TempData["EmptyChk"] =Convert.ToString(CheckExtraInventory.Rows[0]["msg"]);
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }
                }

                #endregion


                #region Check Avaiable Ticket

                DataTable avaiableticketDatatable = new DataTable();

                DataTable ZooCheckMaxTicketBooking = new DataTable();



                avaiableticketDatatable = boz.CheckTicketAvailability(Convert.ToInt32(bz.PlaceOfVisit), Convert.ToInt32(bz.ShiftTypeID), bz.DateOfVisit);
                if (avaiableticketDatatable.Rows.Count > 0)
                {
                    if (avaiableticketDatatable.Rows[0][0].ToString() == "" && avaiableticketDatatable.Rows[0][0].ToString() == "0")
                    {
                        TempData["EmptyChk"] = "All Seat are full on " + bz.DateOfVisit;
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }
                }
                #endregion

                if (dtHoliday.Rows.Count > 0)
                {
                    TempData["EmptyChk"] = "Selected date of visit is a holiday";
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                }

                DataTable dsMember = MemberInformation(lstMember);
                DataTable dsVehicle = VehicleInformation(lstVehicle);

                if (dsMember.Rows.Count == 0)
                {
                    TempData["EmptyChk"] = "Enter member details";
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                }
                else
                {
                    for (int i = 0; i < dsMember.Rows.Count; i++)
                    {
                        totalmember += Convert.ToInt32(dsMember.Rows[i]["NoOfMember"]);
                    }
                }
                if (totalmember > Convert.ToInt32(Session["AvaliableTicket"]))
                {

                    TempData["EmptyChk"] = "You can book only " + Session["AvaliableTicket"].ToString() + " tickets";
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                }

                if (dsVehicle.Rows.Count == 0 && bz.PrivateVehicle == true)
                {
                    TempData["EmptyChk"] = "Enter vehicle details";
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                }


                ZooCheckMaxTicketBooking = boz.ZooCheckMaxTicketBooking(Convert.ToInt32(bz.PlaceOfVisit), totalmember);
                if (ZooCheckMaxTicketBooking.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(ZooCheckMaxTicketBooking.Rows[0]["Status"]) == false)
                    {
                        TempData["EmptyChk"] = "You Can book maximum " + ZooCheckMaxTicketBooking.Rows[0]["MaxSeats"] + " Members in one ticket";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                    }
                }
                if (string.IsNullOrEmpty(bz.OldRequestID))
                {
                    TempData["EmptyChk"] = "Please Fill Old Request ID!!!!";
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                }


                if (DocumentForTour != null && DocumentForTour.ContentLength > 0)
                {
                    Document = Path.GetFileName(DocumentForTour.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    bz.DocumentForTour = path;
                    DocumentForTour.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    bz.DocumentForTour = "";
                }
                if (UploadId != null && UploadId.ContentLength > 0)
                {
                    IdUpload = Path.GetFileName(DocumentForTour.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + IdUpload;
                    path = Path.Combine(FilePath, FileFullName);
                    bz.UploadId = path;
                    UploadId.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    bz.UploadId = "";
                }
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
                bz.IPAddress = result;
                if (Session["KioskUserId"] == null || Session["KioskUserId"] == "")
                {
                    bz.KioskUserId = "0";
                }
                else
                {
                    bz.KioskUserId = Session["KioskUserId"].ToString();
                }
                DataTable dtsubmit = bz.Submit_ZooDetailsExtraInventory(dsMember, dsVehicle, bz.OldRequestID);
                if (dtsubmit.Rows.Count > 0)
                {
                    decimal finalAmnt = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    Session["ZooRequestId"] = dtsubmit.Rows[0]["RequestID"].ToString();
                }

                EducationTours edu = new EducationTours();
                edu.Location = Convert.ToInt64(bz.PlaceOfVisit);
                DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                    {
                        Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                    }
                }


                if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                {



                    #region OLD
                    //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                    KioskPaymentDetails _obj = new KioskPaymentDetails();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 1;
                    _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = Convert.ToString(dtsubmit.Rows[0]["RequestID"]);
                    DataTable dtKiosk = _obj.FetchKisokValue(_obj);



                    if (dtKiosk.Rows.Count > 0)
                    {
                        _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                        _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                        _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                        _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                        _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                        _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                        return PartialView("KioskPaymentDetailExtraInventory", _obj);
                    }
                    #endregion
                }
                else
                {
                    ViewData.Model = dtsubmit.AsEnumerable();
                    return View("OnlineZooPayment");
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }


        [HttpPost]
        public string PrintTicketExtraInventory(Int64 ticketid)
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
                BookOnZoo cs = new BookOnZoo();
                cs.TicketID = ticketid;
                ds = cs.Select_TicketData_For_PrintExtraInventory();

                #region QR Reader
                string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]), "E-m!tr@2016");
                //string decVal = FMDSS.Models.EncodingDecoding.Decrypt(encData, "E-m!tr@2016");
                string QRCodePath = Utility.GenerateMyQCCode(encData, Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]), "QRCodeReader/OnlineBoardingPassQRCode");
                #endregion

                #region Add Logo
                string logo = Convert.ToString(ds.Tables[6].Rows[0]["LOGOUrl"]);
                if (string.IsNullOrEmpty(logo))
                {
                    logo = string.Empty;
                }

                #endregion

                if (ds.Tables.Count > 0)
                {

                    if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                    {

                        sb.Append("<section class=print-invoice style='font-size:8px;font-family:Verdana;' >");
                        sb.Append("<div class=panel panel-default> <div class=panel-body> <div id=tbl_unbold class=table-responsive style='padding-left:14%;'> ");

                        sb.Append("<table class=table>  <thead><tr style=text-align:center><th style=text-align:left> <img src=" + logo + " style=height:75px;width: 75px /></th> <th style=text-align:center>" + ds.Tables[0].Rows[0]["HeadeText"].ToString() + "</th><th style=text-align:right> <img src=/QRCodeReader/OnlineBoardingPassQRCode/" + ds.Tables[0].Rows[0]["RequestId"].ToString() + ".jpg style=height:75px;width: 75px /></th></tr></thead></table></div>");

                        sb.Append("<div id=tbl_unbold class=table-responsive><table class=table table-striped><thead>");
                        sb.Append("<tr> <th>Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right>Date Of Visit : " + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "</th></tr>");

                        sb.Append("<tr> <th>Shift : " + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right> </th></tr>");

                        if (ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() != "")
                        {
                            sb.Append("<tr><th>Issued in Favor of:" + ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() + "</th><th style=text-align:right> ID Number:" + ds.Tables[0].Rows[0]["Institutional_HeadIdNumber"].ToString() + "</th></tr>");
                        }
                        sb.Append("</thead><tbody></tbody></table>");
                        sb.Append("</div>  " +
                         "                               <div id=tbl_unbold class=table-responsive>  " +
                         "                                   <table class=table table-striped table-bordered table-hover>  " +
                         "                                       <thead>  " +
                         "                                           <tr>  " +
                         "                                               <th></th>  " +
                         "                                               <th>Rate(INR)</th>  " +
                         "                                               <th>Qty</th>  " +
                         "                                               <th>Amount(INR)</th>  " +
                         "                                           </tr>  " +
                         "                                       </thead>  " +
                         "                                       <tbody>  ");

                        //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        //{
                        //    int j = i + 1;
                        //    sb.Append("<tr><td>" + ds.Tables[1].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["FeePerMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["NoOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                        //}
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[2].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["FeePerStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["NoOfStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[3].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["FeePerVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["NoOfVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[4].Rows[i]["VehicleType"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["FeePerVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["NoOfVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["TotalVehicleFees"].ToString() + "</td></tr>");
                        }

                        sb.Append("<tr><td>Emitra Amount</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td><td>-</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td></tr>");

                        sb.Append("</tbody>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                        "                               <!-- /.Table3 -->  " +
                        "     " +
                        "                               <!-- Table4 -->  " +
                        "                               <div id=tbl_unbold class=table-responsive>  " +
                        "                                   <table class=table style=text-align: right>  " +
                        "                                       <thead>  " +
                        "                                           <tr>  " +
                        "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0][0].ToString() + "</th>  " +
                        "                                           </tr>  " +
                        "                                       </thead>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                         "                                   <div id=tbl_unbold class=table-responsive style='padding-left:14%;'>" +
                        "                                       <table class=table>" +
                        "                                           <thead>" +
                        "                                               <tr style=text-align:center><th style=text-align:center>" + ds.Tables[0].Rows[0]["FooterText"].ToString() +

                        "                                               </th></tr>" +
                        "                                           </thead>" +
                        "                                       </table>" +
                        "                                   </div>  " +
                        "                         </div>  " +
                        "                  </div>  " +
                        "          </section>");


                    }
                    else
                    {
                        sb.Append("<section class=print-invoice>");
                        sb.Append("<div class=panel panel-default> <div class=panel-body> <div id=tbl_unbold class=table-responsive> ");

                        sb.Append("<table class=table>  <thead><tr style=text-align:center><th style=text-align:left> <img src=" + logo + " style=height:75px;width: 75px /></th> <th style=text-align:center>" + ds.Tables[0].Rows[0]["HeadeText"].ToString() + "</th><th style=text-align:right> <img src=/QRCodeReader/OnlineBoardingPassQRCode/" + ds.Tables[0].Rows[0]["RequestId"].ToString() + ".jpg style=height:75px;width: 75px /></th></tr></thead></table></div>");

                        sb.Append("<div id=tbl_unbold class=table-responsive><table class=table table-striped><thead>");
                        sb.Append("<tr> <th>Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right>Date Of Visit : " + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "</th></tr>");

                        sb.Append("<tr> <th>Shift : " + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right> </th></tr>");

                        if (ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() != "")
                        {
                            sb.Append("<tr><th>Issued in Favor of:" + ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() + "</th><th style=text-align:right> ID Number:" + ds.Tables[0].Rows[0]["Institutional_HeadIdNumber"].ToString() + "</th></tr>");
                        }
                        sb.Append("</thead><tbody></tbody></table>");
                        sb.Append("</div>  " +
                         "                               <div id=tbl_unbold class=table-responsive>  " +
                         "                                   <table class=table table-striped table-bordered table-hover>  " +
                         "                                       <thead>  " +
                         "                                           <tr>  " +
                         "                                               <th></th>  " +
                         "                                               <th>Rate(INR)</th>  " +
                         "                                               <th>Qty</th>  " +
                         "                                               <th>Amount(INR)</th>  " +
                         "                                           </tr>  " +
                         "                                       </thead>  " +
                         "                                       <tbody>  ");

                        //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        //{
                        //    int j = i + 1;
                        //    sb.Append("<tr><td>" + ds.Tables[1].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["FeePerMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["NoOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                        //}
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[2].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["FeePerStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["NoOfStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[3].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["FeePerVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["NoOfVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[4].Rows[i]["VehicleType"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["FeePerVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["NoOfVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["TotalVehicleFees"].ToString() + "</td></tr>");
                        }

                        sb.Append("<tr><td>Emitra Amount</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td><td>-</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td></tr>");

                        sb.Append("</tbody>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                        "                               <!-- /.Table3 -->  " +
                        "     " +
                        "                               <!-- Table4 -->  " +
                        "                               <div id=tbl_unbold class=table-responsive>  " +
                        "                                   <table class=table style=text-align: right>  " +
                        "                                       <thead>  " +
                        "                                           <tr>  " +
                        "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0][0].ToString() + "</th>  " +
                        "                                           </tr>  " +
                        "                                       </thead>  " +
                        "                                   </table>  " +
                        "                               </div>  ");

                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"].ToString()) == 60)
                        {
                            sb.Append(
                 "                               <div id=tbl_unbold class=table-responsive>  " +
                            "                                   <p><b>Terms & Conditions</b> <ul>" +

    //  "Terms and conditions for Visitors : "+
    "<li> The visitor or his / her representative must reach to the Forest Jungle Safari booking centre to collect the boarding pass, at least 45 minutes prior to the entry time. </li> " +
    "<li> The Id proof of visitor produced at the time of collecting boarding pass should be the Id used while booking online ticket, failing which, the ticket will be deemed fake and liable to be cancelled.The visitor must also carry copy of such Id while visiting the Park. </li>" +
    "<li> The charges deposited during online booking include tourist entry fee, vehicle entry fee, vehicle rent, and online booking charges and applicable taxes. </li>" +
    "<li> In Case of Online booking all the 6 seats are required to be filled or else the costumer has to pay difference amount for going inside the park. </li>" +
    "<li> The visitor must bring two copies of confirmation slip at the time of collecting boarding pass.One copy will be deposited in the office and the other copy will be carried by the visitor. </li>" +
    "<li> Seats remaining vacant due to non - turn up of any visitors would be filled by the park management at the booking window. </li>" +
    "<li> Boarding pass shall be collected from the booking office Jhalana Safari Park during 5.00 PM to 6.00 PM of previous evening for the next day morning safari. </li>" +
    "<li> In case of group booking, park authorities will try to adjust the group together in vehicles subject to space availability at the time of entry.  </li>" +
    "<li> Boarding Pass will be issued at: Jhalana Safari Booking Office, Jaipur </li>" +
    "<li> For cancellations made 30 days or more in advance from the date of safari, fifty percent amount would be refunded through the channel the booking was made. </li>" +
    "<li> In case of any changes in applicable Fees &Tax Rates, the difference amount shall be collected at the time of Boarding / entry of the park </li>" +
                            "                                          </ul> </p></div>  ");

                        }
                        sb.Append(
                       "                               <div class=print-bg-tkt>  " +
                        "                                   <div class=centr>  " + ds.Tables[0].Rows[0]["FooterText"].ToString() +
                            //                                           "Visit Timing </br>"+
                            //                                           "Winter (15 oct to 14 March 9:00 AM to  5:00 PM) </br>"+
                            //                                           "Summer(15 March to 14 Oct 8:30 AM to  5:30 PM) </br>"+
                            //"                                       This ticket is valid for bird section also.<br />  " +
                            //"                                      Do not tease the animals.<br />  " +
                            //"                                       Thanks visit again.  " +
                        "                                   </div>  " +
                        "                               </div>  " +
                        "                   <!-- /.Footer -->     " +
                        "                   " +
                        "                         </div>  " +
                        "                           <!-- /.panel-body -->  " +
                        "                             " +
                        "                  </div>  " +
                        "              <!-- /.panel -->  " +
                        "          " +
                        "          </section>");


                        htmlToPdfDownloadTickets.ZooDownloadPdf(ds);
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString();
        }
        #endregion




        public ActionResult ShowPartialView(string PlaceId)
        {
            var lstVehicle = new List<BookOnZoo>();
            var lstEquepment = new List<EquipmentInformation>();
            BookOnZoo obj = new BookOnZoo();
            List<BookOnZoo> lstMember = new List<BookOnZoo>();
            List<BookOnZoo> LstShiftType = new List<BookOnZoo>();
            DataSet dsMemberVehle = new DataSet();
            obj.PlaceOfVisit = PlaceId;
            dsMemberVehle = obj.MemberVehicleDetails();
            var MemberPartialView = string.Empty;
            var VehiclePartialView = string.Empty;
            var VehicleStatus = string.Empty;

            var EquepmentPartialView = string.Empty;
            var EquepmentStatus = string.Empty;
            string Holidays = string.Empty;
            Session["ZooPlaceId"] = PlaceId;
            if (PlaceId != "70")
            {
                foreach (DataRow dr in dsMemberVehle.Tables[0].Rows)
                {

                    lstMember.Add(new BookOnZoo()
                    {
                        TypeOfMember = dr["FeeChargedOn"].ToString(),
                        NoOfMember = "",
                        FeePerMember = dr["HeadAmount"].ToString(),
                        //TypeOfCamera="",
                        //NoOfCamera = "",
                        //FeePerCamera = dr["CameraAmount"].ToString(),					
                        NoOfStillCamera = "",
                        FeePerStillCamera = (PlaceId != "71" ? dr["StillCameraAmount"].ToString() : ""),

                        NoOfVideoCamera = "",
                        FeePerVideoCamera = (PlaceId != "71" ? dr["VideoCameraAmount"].ToString() : ""),

                        TotalFeesOfMember = ""
                    });

                }

                foreach (DataRow dr in dsMemberVehle.Tables[1].Rows)
                {
                    lstVehicle.Add(new BookOnZoo()
                    {
                        TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                        FeePerVehicle = dr["HeadAmount"].ToString(),
                        NoOfVehicle = "",
                        TotalVehicleFee = ""
                    });
                }
                EquepmentStatus = "FLASE";
                if (dsMemberVehle.Tables[4].Rows.Count > 0 && PlaceId == "71")
                {
                    EquepmentStatus = "TRUE";
                    foreach (DataRow dr in dsMemberVehle.Tables[4].Rows)
                    {
                        lstEquepment.Add(new EquipmentInformation()
                        {
                            EquipmentName = dr["FeeChargedOn"].ToString(),
                            FeePerEquipment = dr["HeadAmount"].ToString(),
                            NoOfEquipment = "",
                            TotalEquipmentFee = "",
                            Description = ""
                        });
                    }

                    EquepmentPartialView = RenderRazorViewToString(this.ControllerContext, "ZooEquepmentInfo", lstEquepment);
                }


                MemberPartialView = RenderRazorViewToString(this.ControllerContext, "ZooMemberInfo", lstMember);
                VehiclePartialView = RenderRazorViewToString(this.ControllerContext, "ZooVehicleInfo", lstVehicle);

                VehicleStatus = "FLASE";

                if (dsMemberVehle.Tables[1].Rows.Count > 0)
                {
                    VehicleStatus = "TRUE";
                }

                foreach (DataRow dr in dsMemberVehle.Tables[2].Rows)
                {
                    LstShiftType.Add(new BookOnZoo()
                    {
                        ShiftTypeID = dr["ID"].ToString(),
                        ShiftTypeName = dr["Name"].ToString(),
                    });
                }
                Holidays = string.Empty;

                //if (dsMemberVehle.Tables[3].Rows.Count > 0)
                //{

                //    Holidays = Convert.ToString(dsMemberVehle.Tables[3].Rows[0][0]);

                //}



                ViewBag.ShiftTypeIDs = Convert.ToString(dsMemberVehle.Tables[2].Rows[0]["ID"]);
            }
            else
            {
                obj.PlaceOfVisit = PlaceId;
                dsMemberVehle = obj.MemberVehicleDetails();
                foreach (DataRow dr in dsMemberVehle.Tables[2].Rows)
                {
                    LstShiftType.Add(new BookOnZoo()
                    {
                        ShiftTypeID = dr["ID"].ToString(),
                        ShiftTypeName = dr["Name"].ToString(),
                    });
                }


                MemberPartialView = RenderRazorViewToStringNew(this.ControllerContext, "ZooMemberInfoForPanduPoul", null);
                VehiclePartialView = RenderRazorViewToStringNew(this.ControllerContext, "ZooVehicleInfoForPanduPoul", null);
                VehicleStatus = "TRUE";
            }

            var json = Json(new { MemberPartialView, VehiclePartialView, VehicleStatus, LstShiftType, Holidays = Holidays, EquepmentPartialView, EquepmentStatus });
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

        public static String RenderRazorViewToStringNew(ControllerContext controllerContext, String viewName, Object model = null)
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
        public ActionResult getVehicleDropDownData(string PlaceId)
        {
            var lstVehicle = new List<BookOnZoo>();
            BookOnZoo obj = new BookOnZoo();
            DataSet dsMemberVehle = new DataSet();
            obj.PlaceOfVisit = PlaceId;
            dsMemberVehle = obj.MemberVehicleDetails();
            foreach (DataRow dr in dsMemberVehle.Tables[1].Rows)
            {
                lstVehicle.Add(new BookOnZoo()
                {
                    TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                    FeePerVehicle = dr["HeadAmount"].ToString(),
                    NoOfVehicle = "",
                    TotalVehicleFee = ""
                });
            }
            return Json(lstVehicle);
        }
        public ActionResult getVehicleCharge(string PlaceId, string Vehicle)
        {
            var lstVehicle = new List<BookOnZoo>();
            BookOnZoo obj = new BookOnZoo();
            DataSet dsMemberVehle = new DataSet();
            obj.PlaceOfVisit = PlaceId;
            dsMemberVehle = obj.MemberVehicleDetails();
            foreach (DataRow dr in dsMemberVehle.Tables[1].Rows)
            {
                lstVehicle.Add(new BookOnZoo()
                {
                    TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                    FeePerVehicle = dr["HeadAmount"].ToString(),
                    NoOfVehicle = "",
                    TotalVehicleFee = ""
                });
            }
            var VehicleCharge = lstVehicle.FirstOrDefault(x => x.TypeOfVehicle == Vehicle).FeePerVehicle;
            return Json(VehicleCharge);
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
        public JsonResult CheckTicketAvailability(int PlaceId, int ShiftType, string VisitDate)
        {
            string strStatus = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable dtTicketdetails = new DataTable();
            try
            {
                BookOnZoo boz = new BookOnZoo();
                if (Session["KioskUserId"] == null || Session["KioskUserId"] == "")
                {
                    boz.KioskUserId = "0";
                }
                else
                {
                    boz.KioskUserId = Session["KioskUserId"].ToString();
                }
                dtTicketdetails = boz.CheckTicketAvailability(PlaceId, ShiftType, VisitDate);
                if (dtTicketdetails.Rows.Count > 0)
                {
                    if (dtTicketdetails.Rows[0][0].ToString() != "")
                    {
                        Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                        strStatus = Session["AvaliableTicket"] + "#";
                    }
                    else
                    {
                        Session.Remove("AvaliableTicket");
                        strStatus = "0#";
                    }
                }
                else
                {
                    Session.Remove("AvaliableTicket");
                    strStatus = "0#";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                Session.Remove("AvaliableTicket");
                strStatus = "0#";

            }
            return Json(strStatus);
        }
        /// <summary>
        /// final submission of form
        /// </summary>
        /// <param name="bz"></param>
        /// <param name="DocumentForTour"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult FinalSubmit(List<MemberInformation> lstMember, List<VehicleInformation> lstVehicle, List<EquipmentInformation> lstEquepment, BookOnZoo bz, HttpPostedFileBase DocumentForTour, HttpPostedFileBase UploadId)
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string Document = string.Empty;
            string IdUpload = string.Empty;
            var path = "";
            string FilePath = "~/ZoologicalDocument/";
            int totalmember = 0;


            try
            {
                BookOnZoo boz = new BookOnZoo();
                if (bz.PlaceOfVisit != "70")
                {
                    DateTime date = DateTime.ParseExact(bz.DateOfVisit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DataTable dtHoliday = bz.CheckHolidays(bz.PlaceOfVisit, (int)date.DayOfWeek);

                    string ips = string.Empty;
                    string ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ipaddress))
                    {
                        string[] ipRange = ipaddress.Split(',');
                        int le = ipRange.Length - 1;
                        ips = ipRange[0];
                    }
                    else
                    {
                        ips = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                    bz.IPAddress = ips;

                    #region Check Same IP,Shift Type,Date of Visit , Enter Date,SSOID and Place Developed By Rajveer
                    //string message=string.Empty;
                    //bool flag = boz.CheckSameIPBooking(bz, ref message);
                    //if(flag==false)
                    //{
                    //    TempData["EmptyChk"] = message;
                    //    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                    //}
                    #endregion


                    #region Check Avaiable Ticket

                    DataTable avaiableticketDatatable = new DataTable();

                    DataTable ZooCheckMaxTicketBooking = new DataTable();



                    avaiableticketDatatable = boz.CheckTicketAvailability(Convert.ToInt32(bz.PlaceOfVisit), Convert.ToInt32(bz.ShiftTypeID), bz.DateOfVisit);
                    if (avaiableticketDatatable.Rows.Count > 0)
                    {
                        if (avaiableticketDatatable.Rows[0][0].ToString() == "" && avaiableticketDatatable.Rows[0][0].ToString() == "0")
                        {
                            TempData["EmptyChk"] = "All Seat are full on " + bz.DateOfVisit;
                            return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                        }
                    }
                    #endregion

                    if (dtHoliday.Rows.Count > 0)
                    {
                        TempData["EmptyChk"] = "Selected date of visit is a holiday";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }

                    DataTable dsMember = MemberInformation(lstMember);
                    DataTable dsVehicle = VehicleInformation(lstVehicle);
                    DataTable dsEquepment = EquepmentInformation(lstEquepment);

                    StringWriter sw = new StringWriter();
                    dsEquepment.WriteXml(sw);
                    string xmlEquepmentList = sw.ToString();

                    if (dsMember.Rows.Count == 0)
                    {
                        TempData["EmptyChk"] = "Enter member details";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                    }
                    else
                    {
                        for (int i = 0; i < dsMember.Rows.Count; i++)
                        {
                            totalmember += Convert.ToInt32(dsMember.Rows[i]["NoOfMember"]);
                        }
                    }
                    if (totalmember > Convert.ToInt32(Session["AvaliableTicket"]))
                    {

                        TempData["EmptyChk"] = "You can book only " + Session["AvaliableTicket"].ToString() + " tickets";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }

                    if (dsVehicle.Rows.Count == 0 && bz.PrivateVehicle == true)
                    {
                        TempData["EmptyChk"] = "Enter vehicle details";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                    }

                    ZooCheckMaxTicketBooking = boz.ZooCheckMaxTicketBooking(Convert.ToInt32(bz.PlaceOfVisit), totalmember);
                    if (ZooCheckMaxTicketBooking.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(ZooCheckMaxTicketBooking.Rows[0]["Status"]) == false)
                        {
                            TempData["EmptyChk"] = "You Can book maximum " + ZooCheckMaxTicketBooking.Rows[0]["MaxSeats"] + " Members in one ticket";
                            return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                        }
                    }



                    if (DocumentForTour != null && DocumentForTour.ContentLength > 0)
                    {
                        Document = Path.GetFileName(DocumentForTour.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + Document;
                        path = Path.Combine(FilePath, FileFullName);
                        bz.DocumentForTour = path;
                        DocumentForTour.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        bz.DocumentForTour = "";
                    }
                    if (UploadId != null && UploadId.ContentLength > 0)
                    {
                        IdUpload = Path.GetFileName(DocumentForTour.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + IdUpload;
                        path = Path.Combine(FilePath, FileFullName);
                        bz.UploadId = path;
                        UploadId.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        bz.UploadId = "";
                    }
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
                    bz.IPAddress = result;
                    if (Session["KioskUserId"] == null || Session["KioskUserId"] == "")
                    {
                        bz.KioskUserId = "0";
                    }
                    else
                    {
                        bz.KioskUserId = Session["KioskUserId"].ToString();
                    }
                    DataTable dtsubmit = bz.Submit_ZooDetails(dsMember, dsVehicle, xmlEquepmentList);
                    if (dtsubmit.Rows.Count > 0)
                    {
                        decimal finalAmnt = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"].ToString());
                        Session["totalprice"] = finalAmnt;
                        Session["ZooRequestId"] = dtsubmit.Rows[0]["RequestID"].ToString();
                    }

                    EducationTours edu = new EducationTours();
                    edu.Location = Convert.ToInt64(bz.PlaceOfVisit);
                    DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                        {
                            Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                        }
                    }


                    if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {



                        #region OLD
                        //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                        KioskPaymentDetails _obj = new KioskPaymentDetails();
                        _obj.ModuleId = 1;
                        _obj.ServiceTypeId = 1;
                        _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                        _obj.SubPermissionId = 1;
                        _obj.RequestedId = Convert.ToString(dtsubmit.Rows[0]["RequestID"]);
                        DataTable dtKiosk = _obj.FetchKisokValue(_obj);



                        if (dtKiosk.Rows.Count > 0)
                        {
                            _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                            _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                            _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                            _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                            _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                            _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                            return PartialView("KioskPaymentDetail", _obj);
                        }
                        #endregion
                    }
                    else
                    {
                        ViewData.Model = dtsubmit.AsEnumerable();
                        return View("OnlineZooPayment");
                    }
                }
                else
                {
                    //PanduPoul Code Here
                    DateTime date = DateTime.ParseExact(bz.DateOfVisit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DataTable dtHoliday = bz.CheckHolidays(bz.PlaceOfVisit, (int)date.DayOfWeek);

                    string ips = string.Empty;
                    string ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ipaddress))
                    {
                        string[] ipRange = ipaddress.Split(',');
                        int le = ipRange.Length - 1;
                        ips = ipRange[0];
                    }
                    else
                    {
                        ips = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                    bz.IPAddress = ips;
                    bz.PrivateVehicle = true;
                    DataTable avaiableticketDatatable = new DataTable();
                    DataTable ZooCheckMaxTicketBooking = new DataTable();


                    avaiableticketDatatable = boz.CheckTicketAvailability(int.Parse(bz.PlaceOfVisit), int.Parse(bz.ShiftTypeID), bz.DateOfVisit);
                    if (avaiableticketDatatable.Rows.Count > 0)
                    {
                        if (avaiableticketDatatable.Rows[0][0].ToString() == "" && avaiableticketDatatable.Rows[0][0].ToString() == "0")
                        {
                            TempData["EmptyChk"] = "All Seat are full on " + bz.DateOfVisit;
                            return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                        }
                    }


                    if (dtHoliday.Rows.Count > 0)
                    {
                        TempData["EmptyChk"] = "Selected date of visit is a holiday";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }

                    if (Convert.ToInt32(bz.NoOfMember) > Convert.ToInt32(Session["AvaliableTicket"]))
                    {

                        TempData["EmptyChk"] = "You can book only " + Session["AvaliableTicket"].ToString() + " tickets";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");

                    }

                    if (bz.VehicleInformation.Count == 0)
                    {
                        TempData["EmptyChk"] = "Enter vehicle details";
                        return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                    }

                    DataTable dtsubmit = bz.Submit_ZooDetailsOFPanduPoul();
                    if (dtsubmit.Rows.Count > 0)
                    {
                        decimal finalAmnt = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"].ToString());
                        Session["totalprice"] = finalAmnt;
                        Session["ZooRequestId"] = dtsubmit.Rows[0]["RequestID"].ToString();
                    }

                    EducationTours edu = new EducationTours();
                    edu.Location = Convert.ToInt64(bz.PlaceOfVisit);
                    DataTable dt = edu.GetEmitraDivCode(edu.Location, "3");

                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["DivCode"]) != "")
                        {
                            Session["EmitraDivCode"] = dt.Rows[0]["DivCode"].ToString();
                        }
                    }


                    if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
                    {



                        #region OLD
                        //researchobj.kioskuserid = Convert.ToString(Session["KioskUserId"]);
                        KioskPaymentDetails _obj = new KioskPaymentDetails();
                        _obj.ModuleId = 1;
                        _obj.ServiceTypeId = 1;
                        _obj.PermissionId = Convert.ToInt32(Session["EmitrServiceId"]);
                        _obj.SubPermissionId = 1;
                        _obj.RequestedId = Convert.ToString(dtsubmit.Rows[0]["RequestID"]);
                        DataTable dtKiosk = _obj.FetchKisokValue(_obj);



                        if (dtKiosk.Rows.Count > 0)
                        {
                            _obj.RevenueHead = Convert.ToString(dtKiosk.Rows[0]["RevenueHead"]);
                            _obj.MerchantCode = Convert.ToString(dtKiosk.Rows[0]["MerchantCode"]);
                            _obj.DepartmantalFees = Convert.ToDecimal(dtKiosk.Rows[0]["DepartmantalFees"]);
                            _obj.KMLCharges = Convert.ToDecimal(dtKiosk.Rows[0]["KMLFees"]);
                            _obj.DataEntryAndDocUploadFees = Convert.ToDecimal(dtKiosk.Rows[0]["DataEntryAndDocUpload"]);
                            _obj.TotalAmount = Convert.ToDecimal(dtKiosk.Rows[0]["TotalAmount"]);
                            return PartialView("KioskPaymentDetail", _obj);
                        }
                        #endregion
                    }
                    else
                    {
                        ViewData.Model = dtsubmit.AsEnumerable();
                        return View("OnlineZooPayment");
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// function to return member information in datatable
        /// </summary>
        /// <param name="lstMemberInfo"></param>
        /// <returns></returns>
        public DataTable MemberInformation(List<MemberInformation> lstMemberInfo)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region MemberInfo
                objDt2.Columns.Add("TypeOfMember", typeof(String));
                objDt2.Columns.Add("NoOfMember", typeof(String));
                objDt2.Columns.Add("FeePerMember", typeof(String));
                objDt2.Columns.Add("NoOfStillCamera", typeof(String));
                objDt2.Columns.Add("FeePerStillCamera", typeof(String));
                objDt2.Columns.Add("NoOfVideoCamera", typeof(String));
                objDt2.Columns.Add("FeePerVideoCamera", typeof(String));

                objDt2.Columns.Add("TotalFees", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0" && item.TotalFeesOfMember != null && item.TotalFeesOfMember != "0" && item.TotalFeesOfMember != string.Empty)
                    {
                        dr["TypeOfMember"] = item.TypeOfMember;
                        dr["NoOfMember"] = item.NoOfMember;
                        dr["FeePerMember"] = item.FeePerMember;
                        dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                        dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                        dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                        dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                        dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                            + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                            + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                    else
                    {
                        //if (item.TypeOfMember == "Child Below Age of 5 Years" && item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0")
                        if ((item.TypeOfMember == "Child Below Age of 5 Years"|| item.TypeOfMember == "Child Below Age of 10 Years" || item.TypeOfMember == "Specially abled") && item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0")
                            {
                            dr["TypeOfMember"] = item.TypeOfMember;
                            dr["NoOfMember"] = item.NoOfMember;
                            dr["FeePerMember"] = item.FeePerMember;
                            dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                            dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                            dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                            dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                            dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                                + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                                + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                            objDt2.Rows.Add(dr);
                            objDt2.AcceptChanges();
                        }
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
        /// Function to return member information in datatable
        /// </summary>
        /// <param name="lstMemberInfo"></param>
        /// <returns></returns>
        public DataTable VehicleInformation(List<VehicleInformation> lstVehicleInfo)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region Vehicle Info
                objDt2.Columns.Add("VehicleType", typeof(String));
                objDt2.Columns.Add("FeePerVehicle", typeof(String));
                objDt2.Columns.Add("NoOfVehicle", typeof(String));
                objDt2.Columns.Add("VehicleNumber", typeof(String));
                objDt2.Columns.Add("TotalVehicleFee", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstVehicleInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfVehicle != "0" && item.NoOfVehicle != null && item.NoOfVehicle != string.Empty && item.TotalVehicleFee != "0" && item.TotalVehicleFee != string.Empty && item.TotalVehicleFee != null)
                    {
                        dr["VehicleType"] = item.TypeOfVehicle;
                        dr["FeePerVehicle"] = item.FeePerVehicle;
                        dr["NoOfVehicle"] = item.NoOfVehicle;
                        dr["VehicleNumber"] = (item.VehicleNumber==null || item.VehicleNumber==""?"NA":item.VehicleNumber);
                        dr["TotalVehicleFee"] = (Convert.ToDecimal(item.NoOfVehicle) * Convert.ToDecimal(item.FeePerVehicle));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_v_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return objDt2;
        }
        public DataTable EquepmentInformation(List<EquipmentInformation> lstEquepmentInfo)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("tbl_EquepmentDetail");
            try
            {
                #region Equepment Info
                objDt2.Columns.Add("EqNo", typeof(int));
                objDt2.Columns.Add("EquipmentName", typeof(String));
                objDt2.Columns.Add("FeePerEquipment", typeof(decimal));
                objDt2.Columns.Add("NoOfEquipment", typeof(int));
                objDt2.Columns.Add("TotalEquipmentFee", typeof(decimal));
                objDt2.Columns.Add("Description", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstEquepmentInfo)
                {

                    DataRow dr = objDt2.NewRow();
                    if (item.EqNo != "0" && item.EquipmentName != null && item.EquipmentName != string.Empty && item.FeePerEquipment != "0" && item.TotalEquipmentFee != string.Empty && item.TotalEquipmentFee != null)
                    {
                        dr["EqNo"] = item.EqNo;
                        dr["EquipmentName"] = item.EquipmentName;
                        dr["FeePerEquipment"] = item.FeePerEquipment;
                        dr["NoOfEquipment"] = Convert.ToInt32(item.NoOfEquipment);
                        dr["TotalEquipmentFee"] = (Convert.ToDecimal(item.NoOfEquipment) * Convert.ToDecimal(item.FeePerEquipment));
                        dr["Description"] = (item.Description == null ? "" : item.Description);
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                if (objDt2.Rows.Count == 0)
                {
                    DataRow dr = objDt2.NewRow();
                    dr["EqNo"] = 1;
                    dr["EquipmentName"] = "";
                    dr["FeePerEquipment"] = 0;
                    dr["NoOfEquipment"] = 0;
                    dr["TotalEquipmentFee"] = 0;
                    dr["Description"] = "";
                    objDt2.Rows.Add(dr);
                    objDt2.AcceptChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_e_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return objDt2;
        }
        /// <summary>
        /// Function to pay ticket amount
        /// </summary>
        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                // get different heads amount from DB
                BookOnTicket OBJ = new BookOnTicket();
                DataSet DS = new DataSet();
                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("ZooTickets", Convert.ToString(Session["ZooRequestId"]));
                // 0 Head for ecodevelopment surcharge - 0406-02-800-01
                // 1 Head for entry fees- 0406-01-800-05
                // 2 Grand Total
                // 3 Office
                string REVENUEHEAD = string.Empty;
                if (DS.Tables.Count == 3 && DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                {
                    REVENUEHEAD = Convert.ToString(DS.Tables[2].Rows[0]["ReveneuHead"]);
                }
                else
                {
                    //                                                      Eco-Development                                                         E-Mitra Commision                                           ZOO TRUST SURCHARGE
                    REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["ZooTrustSurcharge"]); 
                }
                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["ZooRequestId"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                    ReturnUrl + "BookOnlineZoo/Payment", ReturnUrl + "BookOnlineZoo/Payment",
                    Convert.ToString(DS.Tables[0].Rows[1]["HeadAmount"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["HeadAmount"]), REVENUEHEAD, Session["User"].ToString(), "", Convert.ToString(DS.Tables[1].Rows[0]["ComType"]));



                Response.Write(forms);



                // Payment pay = new Payment();
                // string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                //// string ReturnUrl = "http://localhost:17105/";
                // string encrypt = pay.RequestString("EM33172142@5488", Session["ZooRequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "BookOnlineZoo/Payment", Session["User"].ToString(), "", "");
                // Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        //[HttpPost]
        //public void Pay()
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

        //    try
        //    {
        //        if (Session["UserID"] != null)
        //        {
        //            DataTable DT = new DataTable();
        //            DT = FMDSS.Models.CommanModels.PaymentRepository.GETEMITRASERVICECODE("ZOO", Convert.ToString(Session["ZooRequestId"]));

        //            if (DT != null && DT.Rows.Count > 0)
        //            {
        //                PaymentViewModel paymodel = new PaymentViewModel();
        //                paymodel.emitraserviceid = Convert.ToString(DT.Rows[0]["EmitraServiceID"]);
        //                paymodel.PayAmt = 0;
        //                paymodel.requestid = Convert.ToString(Session["ZooRequestId"]);
        //                PaymentResponse resp = FMDSS.Models.CommanModels.PaymentRepository.Pay(BookingType.OnlineCitizenBooking, paymodel);
        //                Response.Write(resp.OnlinePaymentResponse);
        //            }
        //            else
        //            {
        //                throw new Exception("Service id not found with this request ID" + Convert.ToString(Session["ZooRequestId"]));
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
        //    }
        //}


        /// <summary>
        /// function to display response from emitra
        /// </summary>
        /// <returns></returns>
        public ActionResult Payment()
        {
            int fmdssStatus = 0;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (Session["ZooRequestId"] != null)
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

                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
                   // EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    BookOnTicket cs1 = new BookOnTicket();
                    cs1.UpdateEmitraResponse(Session["ZooRequestId"].ToString(), "ZooTicketBooking", DecryptedData);

                    int status1 = 0;
                    BookOnZoo cs = new BookOnZoo();
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
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion

                    //****************************** for test only

                    //ObjPGResponse.STATUS = "SUCCESS";
                    //ObjPGResponse.PRN = Convert.ToString(Session["ZooRequestId"]);
                    //ObjPGResponse.PRN = Convert.ToString(Session["ZooRequestId"]);
                    //ObjPGResponse.AMOUNT = Convert.ToString(Session["totalprice"]);
                    //ObjPGResponse.PAIDAMOUNT = Convert.ToString(Convert.ToDecimal(Session["totalprice"]) + Convert.ToDecimal(5));
                    //ObjPGResponse.TRANSACTIONID = DateTime.Now.Ticks.ToString();

                    ////****************************** for test only

                    #region Response Status
                    //if (ObjPGResponse.STATUS == "FAILED")
                    if (ObjPGResponse.STATUS != "SUCCESS")// this line add by rajveer bcz some time emitra send Failed or Pending
                    {
                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = "";

                        dtrow["USER NAME"] = ObjPGResponse.UDF1;// use as user name

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            cs.Trn_Status_Code = 0;
                            cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {

                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = ObjPGResponse.AMOUNT;
                        cs.RequestId = Session["ZooRequestId"].ToString();


                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            if (Convert.ToString(Session["ZooRequestId"]).Equals(ObjPGResponse.PRN) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                            {
                                cs.Trn_Status_Code = 1;
                                status1 = 1;
                                fmdssStatus = cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));

                                //  SendSMSEmailForSuccessTransaction();
                            }
                            else // Added to save mismatch in payment
                            {
                                cs.Trn_Status_Code = 0;
                                status1 = 0;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                            }

                            if (fmdssStatus == 1)
                            {

                                dtrow["TRANSACTION STATUS"] = "SUCCESS";
                                SendSMSEmailForSuccessTransaction();
                                //  System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 7: SUCCESS : SendSMSEmailForSuccessTransaction " });
                            }
                            else
                            {
                                dtrow["TRANSACTION STATUS"] = "FAILED";
                                //   System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step 8: FAILED : not SendSMSEmailForSuccessTransaction " });
                            }

                        }

                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                    DataTable DTdetails = cs.Get_BookedTicketDetails(Session["ZooRequestId"].ToString());
                    List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();
                    ViewBag.TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();

                    if (ViewBag.TicketStatus == "SUCCESS")
                    {
                        foreach (DataRow dr in DTdetails.Rows)
                        {
                            List.Add(
                                   new CS_BoardingDetails()
                                   {
                                       PrintID = Convert.ToString(dr["ZooBookingId"]),
                                       RequestID = Convert.ToString(dr["RequestId"]),
                                       PlaceName = Convert.ToString(dr["PlaceName"]),
                                       Vehicle = Convert.ToString(dr["VehicleType"]),
                                       TotalMembers = Convert.ToString(dr["NoOfMember"]),
                                       DateofBooking = Convert.ToString(dr["DateofBooking"]),
                                       DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                                       AmountTobePaid = Convert.ToString(dr["TotalAmountToBePaid"]),


                                   });

                        }
                    }

                    ViewData["TicketSummary"] = List;


                    ViewData.Model = dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                return View("ZooTransactionStatus");
            }
            return View();
        }


        /// <summary>
        /// Function to print ticket with all necessory details on the basis of ticket ID
        /// </summary>
        /// <param name="ticketid"></param>
        /// <returns>Json result with ticket details</returns>
        [HttpPost]
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
                BookOnZoo cs = new BookOnZoo();
                cs.TicketID = ticketid;
                ds = cs.Select_TicketData_For_Print();

                #region QR Reader
                string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]), "E-m!tr@2016");
                //string decVal = FMDSS.Models.EncodingDecoding.Decrypt(encData, "E-m!tr@2016");
                string QRCodePath = Utility.GenerateMyQCCode(encData, Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]), "QRCodeReader/OnlineBoardingPassQRCode");
                #endregion

                #region Add Logo
                string logo= Convert.ToString(ds.Tables[6].Rows[0]["LOGOUrl"]);
                if (string.IsNullOrEmpty(logo))
                {
                    logo = string.Empty;
                }

                #endregion

                if (ds.Tables.Count > 0)
                {

                    if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                    {

                        sb.Append("<section class=print-invoice style='font-size:8px;font-family:Verdana;' >");
                        sb.Append("<div class=panel panel-default> <div class=panel-body> <div id=tbl_unbold class=table-responsive style='padding-left:14%;'> ");

                        sb.Append("<table class=table>  <thead><tr style=text-align:center><th style=text-align:left> <img src="+ logo + " style=height:75px;width: 75px /></th> <th style=text-align:center>" + ds.Tables[0].Rows[0]["HeadeText"].ToString() + "</th><th style=text-align:right> <img src=/QRCodeReader/OnlineBoardingPassQRCode/" + ds.Tables[0].Rows[0]["RequestId"].ToString() + ".jpg style=height:75px;width: 75px /></th></tr></thead></table></div>");

                        sb.Append("<div id=tbl_unbold class=table-responsive><table class=table table-striped><thead>");
                        sb.Append("<tr> <th>Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right>Date Of Visit : " + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "</th></tr>");

                        sb.Append("<tr> <th>Shift : " + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                       
                        sb.Append("<th style=text-align:right>Date Of Booking : " + ds.Tables[0].Rows[0]["EnteredOn"].ToString() + " </th></tr>");

                        if (ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() != "")
                        {
                            sb.Append("<tr><th>Issued in Favor of:" + ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() + "</th><th style=text-align:right> ID Number:" + ds.Tables[0].Rows[0]["Institutional_HeadIdNumber"].ToString() + "</th></tr>");
                        }
                        if (ds.Tables.Count>8 )
                        {
                            if (ds.Tables[8] != null)
                            {
                                if (ds.Tables[8].Rows.Count > 0)
                                {
                                    if(ds.Tables[8].Rows[0]["ClientName"].ToString() != "" || ds.Tables[8].Rows[0]["MobileNo"].ToString() != "")
                                    {
                                        sb.Append("<tr> <th>" + (ds.Tables[8].Rows[0]["ClientName"].ToString() != "" ? "Name :" + ds.Tables[8].Rows[0]["ClientName"].ToString() : "") + "</th>");
                                        sb.Append("<th style=text-align:right>" + (ds.Tables[8].Rows[0]["MobileNo"].ToString() != "" ? "Mobile No : " + ds.Tables[8].Rows[0]["MobileNo"].ToString() : "") + "</th></tr>");
                                    }                                    
                                }
                            }
                        }
                       
                        sb.Append("</thead><tbody></tbody></table>");
                        sb.Append("</div>  " +
                         "                               <div id=tbl_unbold class=table-responsive>  " +
                         "                                   <table class=table table-striped table-bordered table-hover>  " +
                         "                                       <thead>  " +
                         "                                           <tr>  " +
                         "                                               <th></th>  " +
                         "                                               <th>Rate(INR)</th>  " +
                         "                                               <th>Qty</th>  " +
                         "                                               <th>Amount(INR)</th>  " +
                         "                                           </tr>  " +
                         "                                       </thead>  " +
                         "                                       <tbody>  ");

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[1].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["FeePerMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["NoOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                        }
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[2].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["FeePerStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["NoOfStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[3].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["FeePerVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["NoOfVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[4].Rows[i]["VehicleType"].ToString() + " (" + ds.Tables[4].Rows[i]["VehicleNumber"].ToString() + ")</td><td>" + ds.Tables[4].Rows[i]["FeePerVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["NoOfVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["TotalVehicleFees"].ToString() + "</td></tr>");
                        }

                        sb.Append("<tr><td>Emitra Amount</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td><td>-</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td></tr>");

                        sb.Append("</tbody>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                        "                               <!-- /.Table3 -->  " +
                        "     " +
                        "                               <!-- Table4 -->  " +
                        "                               <div id=tbl_unbold class=table-responsive>  " +
                        "                                   <table class=table style=text-align: right>  " +
                        "                                       <thead>  " +
                        "                                           <tr>  " +
                        "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0][0].ToString() + "</th>  " +
                        "                                           </tr>  " +
                        "                                       </thead>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                         "                                   <div id=tbl_unbold class=table-responsive style='padding-left:14%;'>" +
                        "                                       <table class=table>" +
                        "                                           <thead>" +
                        "                                               <tr style=text-align:center><th style=text-align:center>" + ds.Tables[0].Rows[0]["FooterText"].ToString() +

                        "                                               </th></tr>" +
                        "                                           </thead>" +
                        "                                       </table>" +
                        "                                   </div>  " +
                        "                         </div>  " +
                        "                  </div>  " +
                        "          </section>");


                    }
                    else
                    {
                        sb.Append("<section class=print-invoice>");
                        sb.Append("<div class=panel panel-default> <div class=panel-body> <div id=tbl_unbold class=table-responsive> ");

                        sb.Append("<table class=table>  <thead><tr style=text-align:center><th style=text-align:left> <img src=" + logo + " style=height:75px;width: 75px /></th> <th style=text-align:center>" + ds.Tables[0].Rows[0]["HeadeText"].ToString() + "</th><th style=text-align:right> <img src=/QRCodeReader/OnlineBoardingPassQRCode/" + ds.Tables[0].Rows[0]["RequestId"].ToString() + ".jpg style=height:75px;width: 75px /></th></tr></thead></table></div>");

                        sb.Append("<div id=tbl_unbold class=table-responsive><table class=table table-striped><thead>");
                        sb.Append("<tr> <th>Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right>Date Of Visit : " + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "</th></tr>");
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"].ToString()) == 70)
                            sb.Append("<tr> <th>" + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                        else
                            sb.Append("<tr> <th>Shift : " + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                        sb.Append("<th style=text-align:right>Date Of Booking : " + ds.Tables[0].Rows[0]["EnteredOn"].ToString() + " </th></tr>");

                        if (ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() != "")
                        {
                            sb.Append("<tr><th>Issued in Favor of:" + ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() + "</th><th style=text-align:right> ID Number:" + ds.Tables[0].Rows[0]["Institutional_HeadIdNumber"].ToString() + "</th></tr>");
                        }
                        if (ds.Tables.Count > 8)
                        {
                            if (ds.Tables[8] != null)
                            {
                                if (ds.Tables[8].Rows.Count > 0)
                                {
                                    if (ds.Tables[8].Rows[0]["ClientName"].ToString() != "" || ds.Tables[8].Rows[0]["MobileNo"].ToString() != "")
                                    {
                                        sb.Append("<tr> <th>" + (ds.Tables[8].Rows[0]["ClientName"].ToString() != "" ? "Name :" + ds.Tables[8].Rows[0]["ClientName"].ToString() : "") + "</th>");
                                        sb.Append("<th style=text-align:right>" + (ds.Tables[8].Rows[0]["MobileNo"].ToString() != "" ? "Mobile No : " + ds.Tables[8].Rows[0]["MobileNo"].ToString() : "") + "</th></tr>");
                                    }
                                }
                            }
                        }
                        sb.Append("</thead><tbody></tbody></table>");
                        sb.Append("</div>  " +
                         "                               <div id=tbl_unbold class=table-responsive>  " +
                         "                                   <table class=table table-striped table-bordered table-hover>  " +
                         "                                       <thead>  " +
                         "                                           <tr>  " +
                         "                                               <th></th>  " +
                         "                                               <th>Rate(INR)</th>  " +
                         "                                               <th>Qty</th>  " +
                         "                                               <th>Amount(INR)</th>  " +
                         "                                           </tr>  " +
                         "                                       </thead>  " +
                         "                                       <tbody>  ");

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[1].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["FeePerMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["NoOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                        }
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[2].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["FeePerStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["NoOfStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[3].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["FeePerVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["NoOfVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                        }

                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            int j = i + 1;
                            sb.Append("<tr><td>" + ds.Tables[4].Rows[i]["VehicleType"].ToString() + " (" + ds.Tables[4].Rows[i]["VehicleNumber"].ToString() + ")</td><td>" + ds.Tables[4].Rows[i]["FeePerVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["NoOfVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["TotalVehicleFees"].ToString() + "</td></tr>");
                        }

                        sb.Append("<tr><td>Emitra Amount</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td><td>-</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td></tr>");

                        sb.Append("</tbody>  " +
                        "                                   </table>  " +
                        "                               </div>  " +
                        "                               <!-- /.Table3 -->  " +
                        "     " +
                        "                               <!-- Table4 -->  " +
                        "                               <div id=tbl_unbold class=table-responsive>  " +
                        "                                   <table class=table style=text-align: right>  " +
                        "                                       <thead>  " +
                        "                                           <tr>  " +
                        "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0][0].ToString() + "</th>  " +
                        "                                           </tr>  " +
                        "                                       </thead>  " +
                        "                                   </table>  " +
                        "                               </div>  ");

                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"].ToString()) == 60)
                        {
                            sb.Append(
                 "                               <div id=tbl_unbold class=table-responsive>  " +
                            "                                   <p><b>Terms & Conditions</b> <ul>" +

    //  "Terms and conditions for Visitors : "+
    "<li> The visitor or his / her representative must reach to the Forest Jungle Safari booking centre to collect the boarding pass, at least 45 minutes prior to the entry time. </li> " +
    "<li> The Id proof of visitor produced at the time of collecting boarding pass should be the Id used while booking online ticket, failing which, the ticket will be deemed fake and liable to be cancelled.The visitor must also carry copy of such Id while visiting the Park. </li>" +
    "<li> The charges deposited during online booking include tourist entry fee, vehicle entry fee, vehicle rent, and online booking charges and applicable taxes. </li>" +
    "<li> In Case of Online booking all the 6 seats are required to be filled or else the costumer has to pay difference amount for going inside the park. </li>" +
    "<li> The visitor must bring two copies of confirmation slip at the time of collecting boarding pass.One copy will be deposited in the office and the other copy will be carried by the visitor. </li>" +
    "<li> Seats remaining vacant due to non - turn up of any visitors would be filled by the park management at the booking window. </li>" +
    "<li> Boarding pass shall be collected from the booking office Jhalana Safari Park during 5.00 PM to 6.00 PM of previous evening for the next day morning safari. </li>" +
    "<li> In case of group booking, park authorities will try to adjust the group together in vehicles subject to space availability at the time of entry.  </li>" +
    "<li> Boarding Pass will be issued at: Jhalana Safari Booking Office, Jaipur </li>" +
    "<li> For cancellations made 30 days or more in advance from the date of safari, fifty percent amount would be refunded through the channel the booking was made. </li>" +
     "<li>The Provided ticket for full day/Half day Shift will be only for a single visit as per the schedule defined by the Department of Forest Rajasthan. </li>" +
    "<li> In case of any changes in applicable Fees &Tax Rates, the difference amount shall be collected at the time of Boarding / entry of the park </li>" +
                            "                                          </ul> </p></div>  ");

                        }
                        else if (ds.Tables[7] != null && ds.Tables[7].Rows.Count>0)
                        {
                            string Div="                               <div id=tbl_unbold class=table-responsive>  " +
                            "                                   <p><b>Terms & Conditions</b> <ul style='list-style: none;'>";

                          sb.Append(Div);
                            for(int k =0;k<ds.Tables[7].Rows.Count;k++)
                            {
                                string str=string.Empty;
                                str = "<li>" + Convert.ToString(ds.Tables[7].Rows[k]["DisplayNo"]) + ". " + Convert.ToString(ds.Tables[7].Rows[k]["TEXT"]) + " </li> ";
                                sb.Append(str);
                            }
                            sb.Append("                                          </ul> </p></div>  ");
                        }
                        sb.Append(
                       "                               <div class=print-bg-tkt>  " +
                        "                                   <div class=centr>  " + ds.Tables[0].Rows[0]["FooterText"].ToString() +
                            //                                           "Visit Timing </br>"+
                            //                                           "Winter (15 oct to 14 March 9:00 AM to  5:00 PM) </br>"+
                            //                                           "Summer(15 March to 14 Oct 8:30 AM to  5:30 PM) </br>"+
                            //"                                       This ticket is valid for bird section also.<br />  " +
                            //"                                      Do not tease the animals.<br />  " +
                            //"                                       Thanks visit again.  " +
                        "                                   </div>  " +
                        "                               </div>  " +
                        "                   <!-- /.Footer -->     " +
                        "                   " +
                        "                         </div>  " +
                        "                           <!-- /.panel-body -->  " +
                        "                             " +
                        "                  </div>  " +
                        "              <!-- /.panel -->  " +
                        "          " +
                        "          </section>");


                        htmlToPdfDownloadTickets.ZooDownloadPdf(ds);
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString();
        }

        ////Create PDF for DMS





        ////End PDF



        ////CreatePdf

        public void SendSMSEmailForSuccessTransaction()
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(Session["ZooRequestId"].ToString(), "GETUSERDETAILSFORSENDSMSANDEMAILforZOO");
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                    objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                    body = string.Empty;

                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                    body = string.Empty;
                }

            }


            #endregion




        }

        /// <summary>
        /// For Departmental User payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeptKioskUserPayZOO()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
                {
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();

                    // DataTable DTReqDetails = _obj.GetZOORequestIDDetails(Session["RequestId"].ToString());

                    _obj.RequestedIdEn = Encryption.encrypt(Session["ZooRequestId"].ToString());
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 2;
                    _obj.PermissionId = 1;
                    _obj.SubPermissionId = 2;
                    _obj.RequestedId = Session["ZooRequestId"].ToString();
                    //===== ADDED BY ARVIND
                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["KioskUserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["totalprice"].ToString());
                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                    return PartialView("PaymentByDepartmentalKioskUserForZoo", _obj);
                }
                else
                {
                    return RedirectToAction("BookOnlineZoo", "BookOnlineZoo");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpGet]
        public ActionResult BindVehicles()
        {
            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            BookOnZoo obj = new BookOnZoo();
            List<SelectListItem> lstPlace = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            try
            {
                obj.RequestId = "";

                return View(obj);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]

        public ActionResult ShowPartialViewUsingTicket(string TicketIDs)
        {
            var lstVehicle = new List<BookOnZoo>();
            BookOnZoo obj = new BookOnZoo();
            List<BookOnZoo> lstMember = new List<BookOnZoo>();
            DataSet dsMemberVehle = new DataSet();
            obj.RequestId = TicketIDs;

            dsMemberVehle = obj.BindVehiclesDetailsUsingTicket();

            foreach (DataRow dr in dsMemberVehle.Tables[0].Rows)
            {
                lstVehicle.Add(new BookOnZoo()
                {
                    TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                    FeePerVehicle = dr["HeadAmount"].ToString(),
                    NoOfVehicle = dr["NoOfVehicle"].ToString(),
                    TotalVehicleFee = dr["TotalVehicleFee"].ToString(),

                });
            }

            var VehiclePartialView = RenderRazorViewToString(this.ControllerContext, "ZooVehicleInfo", lstVehicle);
            var VehicleStatus = "FLASE";
            if (dsMemberVehle.Tables[0].Rows.Count > 0)
            {
                VehicleStatus = "TRUE";
            }

            string TicketStatus = Convert.ToString(dsMemberVehle.Tables[1].Rows[0][0]);

            var json = Json(new { VehiclePartialView, VehicleStatus, TicketStatus });
            return json;
        }

        public ActionResult FinalSubmitBindVehicles(List<VehicleInformation> lstVehicle, BookOnZoo bz)
        {

            actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);



            List<SelectListItem> lstPlace = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            try
            {
                DataTable dsVehicle = VehicleInformation(lstVehicle);

                DataTable dtsubmit = bz.Submit_UpdateTicket(dsVehicle, bz.RequestId);

                if (dtsubmit.Rows.Count > 0)
                {
                    decimal finalAmnt = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"].ToString());
                    Session["totalprice"] = finalAmnt;
                    Session["ZooRequestId"] = dtsubmit.Rows[0]["RequestID"].ToString();

                }
                ViewData.Model = dtsubmit.AsEnumerable();
                return View("OnlineZooPayment");

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        //#region Check Same IP,Shift Type,Date of Visit , Enter Date,SSOID and Place Develeoped By Rajveer
        //public JsonResult CheckSameIPBooking(BookOnZoo modal)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        flag = CheckSameIPBooking(modal);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(flag, JsonRequestBehavior.AllowGet);
        //}

        //#endregion
    }


}
