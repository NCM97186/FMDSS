//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : File contains calling functions from UI
//  Date Created : 13-08-2016
//  History      :
//  Version      : 1.0
//  Author       : Arvind Kumar Sharma
//  Modified By  :
//  Modified On  :
//  Reviewed By  : Amit
//  Reviewed On  : 
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.CitizenService.PermissionService;
using System.Xml;
using System.Configuration;
using System.Text;
using FMDSS.Filters;
using FMDSS.Models;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.Common;
using System.Web.UI.WebControls;
using System.Collections;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using System.IO;
using System.Web.UI;
using FMDSS.Models.BookOnlineTicket;

namespace FMDSS.Controllers.Master
{
    public class BoardingMasterController : BaseController
    {

        List<SelectListItem> Accomodation = new List<SelectListItem>();
        List<SelectListItem> vehicleCategory = new List<SelectListItem>();
        List<CS_Ticket> ticketList = new List<CS_Ticket>();
        List<CS_BoardingDetails> ListBoarding = new List<CS_BoardingDetails>();
        List<SelectListItem> lstZone = new List<SelectListItem>();
        List<SelectListItem> lstPlace = new List<SelectListItem>();
        List<SelectListItem> lstVehicle = new List<SelectListItem>();

        List<SelectListItem> LstGandV = new List<SelectListItem>();

        List<AssignGuideAndVehicleName> ListAssignGuideAndVehicleName = new List<AssignGuideAndVehicleName>();



        DataTable DT_DptUser;
        DataTable DT_BoardingDuration;
        DataSet Ds = new DataSet();
        int ModuleID = 1;
        /// <summary>
        /// Renders UI for Online Ticket Booking
        /// </summary>
        /// <returns>View for  Online Ticke and bind all booked ticket</returns>
        /// 


        [NonAction]
        public void BoardingPassLoad()
        {
            List<SelectListItem> lstShiftType = new List<SelectListItem>();

            lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
            lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });
            // lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });

            ViewBag.LstddlShift = lstShiftType;

        }

        [NonAction]
        public string getnewcolor(Int16 Number)
        {

            string[] Colors = new string[60];


            Colors[0] = "#D6D2D1";
            Colors[1] = "#F6F1F0";
            Colors[2] = "#D6D2D1";
            Colors[3] = "#F6F1F0";
            Colors[4] = "#D6D2D1";
            Colors[5] = "#F6F1F0";
            Colors[6] = "#D6D2D1";
            Colors[7] = "#F6F1F0";
            Colors[8] = "#D6D2D1";
            Colors[9] = "#F6F1F0";
            Colors[10] = "#D6D2D1";
            Colors[10] = "#F6F1F0";
            Colors[11] = "#D6D2D1";
            Colors[12] = "#F6F1F0";
            Colors[13] = "#D6D2D1";
            Colors[14] = "#F6F1F0";
            Colors[15] = "#D6D2D1";
            Colors[16] = "#F6F1F0";
            Colors[17] = "#D6D2D1";
            Colors[18] = "#F6F1F0";
            Colors[19] = "#D6D2D1";
            Colors[20] = "#F6F1F0";

            Colors[21] = "#D6D2D1";
            Colors[22] = "#F6F1F0";
            Colors[23] = "#D6D2D1";
            Colors[24] = "#F6F1F0";
            Colors[25] = "#D6D2D1";
            Colors[26] = "#F6F1F0";
            Colors[27] = "#D6D2D1";
            Colors[28] = "#F6F1F0";
            Colors[29] = "#D6D2D1";
            Colors[30] = "#F6F1F0";

            Colors[31] = "#D6D2D1";
            Colors[32] = "#F6F1F0";
            Colors[33] = "#D6D2D1";
            Colors[34] = "#F6F1F0";
            Colors[35] = "#D6D2D1";
            Colors[36] = "#F6F1F0";
            Colors[37] = "#D6D2D1";
            Colors[38] = "#F6F1F0";
            Colors[39] = "#D6D2D1";
            Colors[40] = "#F6F1F0";

            Colors[41] = "#D6D2D1";
            Colors[42] = "#F6F1F0";
            Colors[43] = "#D6D2D1";
            Colors[44] = "#F6F1F0";
            Colors[45] = "#D6D2D1";
            Colors[46] = "#F6F1F0";
            Colors[47] = "#D6D2D1";
            Colors[48] = "#F6F1F0";
            Colors[49] = "#D6D2D1";
            Colors[50] = "#F6F1F0";

            Colors[51] = "#D6D2D1";
            Colors[52] = "#F6F1F0";
            Colors[53] = "#D6D2D1";
            Colors[54] = "#F6F1F0";
            Colors[55] = "#D6D2D1";
            Colors[56] = "#F6F1F0";
            Colors[57] = "#D6D2D1";
            Colors[58] = "#F6F1F0";
            Colors[59] = "#D6D2D1";




            return Colors[Number].ToString();
        }

        [HttpGet]
        public ActionResult AssignGuideAndVehicleForDateOfVisite()
        {

            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            AssignGuideAndVehicleName obj = new AssignGuideAndVehicleName();

            string SSOID = Session["SSOid"].ToString();
            try
            {
                Ds = obj.BindDptKioskPLACES(SSOID);
                BoardingPassLoad();

                ViewData["ListBoarding"] = ListAssignGuideAndVehicleName;
                Ds = obj.BindDptKioskPLACES(SSOID);

                obj.PlaceID = Ds.Tables[0].Rows[0][0].ToString();
                obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();


                // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)

                // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                DateTime DateTimes = DateTime.Now;
                int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                string DATEOFVISITE = string.Empty;
                string SHIFT_TYPE = string.Empty;

                // =========== EVENING_SHIFT
                if (CurrentTime > 9 && CurrentTime < 14)
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "2";
                }


                // =========== MORNING_SHIFT
                if (CurrentTime > 15)
                {
                    DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                }
                else if (CurrentTime > -1 && CurrentTime < 8)
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                }

                obj.DateOfVisit = DATEOFVISITE;
                obj.ShiftType = SHIFT_TYPE;


                DT_DptUser = obj.GetVehicleByZoneAndDateOfVisit(Convert.ToInt32(obj.PlaceID), obj.DateOfVisit);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["Name"].ToString() });
                }

                ViewBag.LstddlVehicle = lstVehicle;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }

        public ActionResult AssignGuideAndVehicleForDateOfVisite(AssignGuideAndVehicleName AssignGVname)
        {

            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                AssignGuideAndVehicleName obj = new AssignGuideAndVehicleName();
                DataTable DT;
                string BOOKING_TYPE = "BoardingPass";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");

                DT = obj.GetAssignGuideAndVehicleName(AssignGVname.DateOfVisit, AssignGVname.PlaceID, AssignGVname.ShiftType, BOOKING_TYPE, AssignGVname.VehicalEqptName);
                int count = 1;

                foreach (DataRow dr in DT.Rows)
                {
                    ListAssignGuideAndVehicleName.Add(
                        new AssignGuideAndVehicleName()
                        {
                            Index = count,
                            GVID = Convert.ToString(dr["GVID"].ToString()),
                            PlaceID = Convert.ToString(dr["PlaceID"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            ShiftTypeName = Convert.ToString(dr["ShiftTypeName"]),
                            ShiftType = AssignGVname.ShiftType,
                            VehicalEqptName = Convert.ToString(dr["VehicalEqptName"]),
                            GuideName = Convert.ToString(dr["GuideName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                            IsactiveForMapping = Convert.ToBoolean(dr["IsactiveForMapping"]),

                        });
                    count += 1;
                }
                string SSOID = Session["SSOid"].ToString();

                DT_DptUser = AssignGVname.GetVehicleByZoneAndDateOfVisit(Convert.ToInt32(AssignGVname.PlaceID), AssignGVname.DateOfVisit);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["Name"].ToString() });
                }

                ViewBag.LstddlVehicle = lstVehicle;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            ViewData["ListBoarding"] = ListAssignGuideAndVehicleName;
            return View(AssignGVname);
        }
        [HttpPost]
        public JsonResult InsertCurrentAvailableGuideAndVehicle(string GVID, string PlaceID, string ShiftType, string DateOfVisit, string VehicalEqptName, string GuideName, string VehicleNumber, string IsactiveForMapping)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            AssignGuideAndVehicleName BoardingDetails = new AssignGuideAndVehicleName();
            string STATUS = string.Empty;
            try
            {
                DT_DptUser = BoardingDetails.INSERTGuideAndVehicleForAssign(GVID, PlaceID, ShiftType, DateOfVisit, VehicalEqptName, GuideName, VehicleNumber, IsactiveForMapping);
                STATUS = DT_DptUser.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new { STATUS }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ValidationAndMappingGVforTickets(string IDGV, string RequestID, string PlaceID, string ZoneID, string DateOfVisit, string VehicleID, string ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            AssignGuideAndVehicleName BoardingDetails = new AssignGuideAndVehicleName();
            string STATUS = string.Empty;
            try
            {
                DT_DptUser = BoardingDetails.ValidationAndMappingGVforTickets("ValidationAndMappingGVforTickets", IDGV, RequestID, PlaceID, ZoneID, DateOfVisit, VehicleID, ShiftType);
                if (DT_DptUser.Rows.Count > 0)
                {
                    STATUS = "TRUE";
                }
                else
                {
                    STATUS = "FALSE";

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }



            return Json(new { STATUS }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult INSERTMappingGVforTickets(string IDGV, string RequestID, string PlaceID, string ZoneID, string DateOfVisit, string VehicleID, string ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            AssignGuideAndVehicleName BoardingDetails = new AssignGuideAndVehicleName();
            string STATUS = string.Empty;
            try
            {
                DT_DptUser = BoardingDetails.ValidationAndMappingGVforTickets("INSERTMappingGVforTickets", IDGV, RequestID, PlaceID, ZoneID, DateOfVisit, VehicleID, ShiftType);



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }



            return Json(new { STATUS }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult MappingofTicketswithGuideNameAndVehicleNumber()
        {

            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {
                Ds = obj.BindDptKioskPLACES(SSOID);
                BoardingPassLoad();

                ViewData["ListBoarding"] = ListBoarding;
                Ds = obj.BindDptKioskPLACES(SSOID);
                foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
                ViewBag.LstddlZone = lstZone;

                ViewBag.LstddlVehicle = lstVehicle;
                ViewBag.LstddlGandV = LstGandV;
                obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();


                // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)

                // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                DateTime DateTimes = DateTime.Now;
                int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                string DATEOFVISITE = string.Empty;
                string SHIFT_TYPE = string.Empty;

                // =========== EVENING_SHIFT
                if (CurrentTime > 9 && CurrentTime < 14)
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "2";
                }


                // =========== MORNING_SHIFT
                if (CurrentTime > 15)
                {
                    DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                }
                else if (CurrentTime > -1 && CurrentTime < 8)
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                }

                obj.DateofVisit = DATEOFVISITE;
                obj.Shift = SHIFT_TYPE;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }
        [HttpPost]
        public ActionResult MappingofTicketswithGuideNameAndVehicleNumber(CS_BoardingDetails BoardingDetails)
        {
            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataSet DT;
                string BOOKING_TYPE = "MappingTicketswithGuideAndVehicle";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");

                Date = "10/10/2016";

                DT = obj.GetMappingTicketswithGuideAndVehicle(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);
                int count = 1;
                Int16 ColorNumber = 0;
                foreach (DataRow dr in DT.Tables[0].Rows)
                {
                    if (count == 1)
                    {
                        ViewBag.Finlacolor = getnewcolor(ColorNumber);
                        ViewBag.RID = Convert.ToString(dr["RequestID"].ToString());
                    }
                    else
                    {
                        if (ViewBag.RID == Convert.ToString(dr["RequestID"].ToString()))
                        {
                        }
                        else
                        {
                            ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                            ViewBag.Finlacolor = getnewcolor(ColorNumber);
                            ViewBag.RID = Convert.ToString(dr["RequestID"].ToString());
                        }
                    }

                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            Colors = ViewBag.Finlacolor,
                            PlaceName = Convert.ToString(dr["PlaceName"]),

                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]),
                            ZoneID = Convert.ToString(dr["ZoneID"]),
                            AmountTobePaid = Convert.ToString(dr["TotalMembers"]), // use AmountTobePaid property as TotalMembers
                            GuidName = Convert.ToString(dr["MappingGVID"]),



                        });
                    count += 1;
                }

                string SSOID = Session["SSOid"].ToString();
                Ds = obj.BindDptKioskPLACES(SSOID);

                foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
                ViewBag.LstddlZone = lstZone;

                DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID));

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                foreach (System.Data.DataRow dr in DT.Tables[1].Rows)
                {
                    LstGandV.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }


                ViewBag.LstddlGandV = LstGandV;


                ViewBag.LstddlVehicle = lstVehicle;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            // TempData["Details"] = ListBoarding;
            ViewData["ListBoarding"] = ListBoarding;
            return View(BoardingDetails);
        }

        [HttpGet]
        public ActionResult BoardingPass(string KioskBoarding = "", string KioskBoardingDirect="false")
        {

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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
            }





            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {

                BoardingPassLoad();


                Ds = obj.BindDptKioskPLACES(SSOID);

                ViewBag.LstddlZone = lstZone;

                ViewBag.LstddlVehicle = lstVehicle;
                string tempPlaceId = string.Empty;
                if (Ds.Tables.Count > 0)
                {

                    if (Ds.Tables[0].Rows.Count > 1)
                    {

                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {

                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }

                        tempPlaceId = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                        obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();

                        foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                        }

                        tempPlaceId = obj.PlaceId;
                    }

                    // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
                    // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                    // === set default date and shift as per database 

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;

                    DT_BoardingDuration = obj.GetBoardingDuration(tempPlaceId);
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "2";
                                obj.DisplayShiftName = "Evening";
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            // =========== MORNING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                            else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                        }
                    }

                    obj.DateofVisit = DATEOFVISITE;
                    obj.Shift = SHIFT_TYPE;

                    // === set default date and shift as per database


                    if (KioskBoarding != string.Empty)
                    {
                        KioskBoarding = Encryption.decrypt(KioskBoarding);
                        DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(KioskBoarding.ToString());


                        if (dtKioskBoarding.Rows.Count > 0)
                        {

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                            obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                            obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                            obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();


                            DT_DptUser = obj.GetZoneByPlace(dtKioskBoarding.Rows[0]["PlaceID"].ToString());

                            lstZone.Clear();
                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                            }




                            DT_DptUser = obj.GetVehicleByZoneAndPlace(Convert.ToInt32(dtKioskBoarding.Rows[0]["PlaceID"].ToString()), Convert.ToInt32(dtKioskBoarding.Rows[0]["ZoneID"].ToString()));

                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                            }

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();

                            string BOOKING_TYPE = "RequestIDWiseBoardingPass";

                            #region If kiosk  User Direct Print Boarding Pass Developed by Rajveer.this parameter pass KioskBoardingDirect=true from KioskTransactionStatusWildlife views
                            if (!string.IsNullOrEmpty(KioskBoardingDirect) && KioskBoardingDirect.ToLower()=="true")
                            {
                                obj.RequestID = KioskBoarding.ToString();
                            }
                            #endregion

                            COMMONGETDATA(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);
                        }

                    }
                    else
                    {
                        ViewData["ListBoarding"] = ListBoarding;


                        ViewBag.LstddlPlace = lstPlace;
                        ViewBag.LstddlZone = lstZone;
                        ViewBag.LstddlVehicle = lstVehicle;

                    }

                }
                else
                {
                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;
                }
                ViewBag.GuideSummary = "";
                ViewBag.KioskBoardingDirect = KioskBoardingDirect;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }


        public void COMMONGETDATA(string DateofVisit, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string Zone, string VehicleTypeID, string RequestID)
        {
            DataTable DT;

            CS_BoardingDetails obj = new CS_BoardingDetails();

            DT = obj.GetCurrentDateBooking(DateofVisit, PlaceID, SHIFT_TYPE, BOOKING_TYPE, Zone, VehicleTypeID, RequestID);
            int count = 1;
            Int16 ColorNumber = 0;
            foreach (DataRow dr in DT.Rows)
            {
                Random random = new Random();
                Color randomColor = new Color();

                if (count == 1)
                {
                    ViewBag.Finlacolor = getnewcolor(ColorNumber);
                    //ViewBag.RID = Convert.ToString(dr["RequestID"].ToString());
                }
                else
                {
                    //if (ViewBag.RID == Convert.ToString(dr["RequestID"].ToString()))
                    //{

                    //}
                    //else
                    {
                        ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                        ViewBag.Finlacolor = getnewcolor(ColorNumber);
                    //    ViewBag.RID = Convert.ToString(dr["RequestID"].ToString());
                    }
                }

                #region "GetVehicleNumber"
                    
                #endregion


                ListBoarding.Add(


                    new CS_BoardingDetails()
                    {

                        Index = count,
                        DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                        VehicleName= Convert.ToString(dr["NEWVehicleName"].ToString()) ,
                        // HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                      //  RequestID = Convert.ToString(dr["RequestID"].ToString()),
                        Colors = ViewBag.Finlacolor,
                        PlaceName = Convert.ToString(dr["PlaceName"]),
                        //  NameOfVisitor = Convert.ToString(dr["Name"]),
                        DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                        DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                        Shift = Convert.ToString(dr["ShiftTime"]),
                        Vehicle = Convert.ToString(dr["VehicleName"]),
                        ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                        ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                        ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                        ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                        BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),
                        IsDepartmentalKioskUser = Convert.ToInt16(dr["IsDepartmentalKioskUser"].ToString() == string.Empty ? 0 : dr["IsDepartmentalKioskUser"]),
                        GuidName = Convert.ToString(dr["GuidName"]),
                        VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                        ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),
                        IsNoShowOpenOrNot = DT.Columns.Contains("IsNoShowOpenOrNot")? Convert.ToString( dr["IsNoShowOpenOrNot"]):"S",
                        


                    });
                count += 1;
            }

            var distinctNotes = ListBoarding.Select(x => x.GuidName).Distinct();
            StringBuilder SBListBoardingSummary = new StringBuilder();

            SBListBoardingSummary.Append("[ Total Visitor : " + Convert.ToString(ListBoarding.Count()) + " ]  / ");


            foreach (var UGuideName in distinctNotes)
            {
                if (UGuideName != "")
                {
                    SBListBoardingSummary.Append(UGuideName + " : " + Convert.ToString(ListBoarding.Count(item => item.GuidName == UGuideName)) + " / ");
                }

            }


            ViewBag.GuideSummary = SBListBoardingSummary.ToString();
            ViewData["ListBoarding"] = ListBoarding;
        }

        private string GetSystemGeneratedVehicleNumber(string NewVehicleName, string RequestID)
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string VehicleName = obj.GetSystemGeneratedNumber(RequestID);
            return (VehicleName == "0"? NewVehicleName: VehicleName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BoardingPass(CS_BoardingDetails BoardingDetails)
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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

                if (!TimeBound(BoardingDetails.PlaceId))
                {
                    TempData["ValidIPAddress"] = "Boarding Pass Generation time period has expired";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");

                }


            }

            ListBoarding.Clear();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "RequestIDWiseBoardingPass";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");




                string SSOID = Session["SSOid"].ToString();
                Ds = obj.BindDptKioskPLACES(SSOID);
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 1)
                    {
                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {
                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                    }

                    COMMONGETDATA(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);

                    DT_DptUser = BoardingDetails.GetZoneByPlace(BoardingDetails.PlaceId);

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }




                    DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID));

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;

                }
                else
                {


                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(BoardingDetails);
        }



        public JsonResult ZoneByPlace(string PlaceId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();

            try
            {
                DT_DptUser = BoardingDetails.GetZoneByPlace(PlaceId);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(lstZone, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetVehicleByZoneAndPlace(int PlaceId, int ZoneID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();

            try
            {
                DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(PlaceId, ZoneID);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UpdateBoardingZoneS(string ZoneID, string RecordID)
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {

                string UpdatedBy = Convert.ToString(Session["UserID"]);
                DataTable DT;
                DT = obj.UpdateBoardingZone(ZoneID, RecordID, UpdatedBy);

                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                            HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            NameOfVisitor = Convert.ToString(dr["Name"]),
                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                            ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                            ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                            ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                            BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),

                        });
                    count += 1;
                }

                TempData["Details"] = ListBoarding;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            // return RedirectToAction("BoardingPass", "TicketBooking");
            return Json(new
            {
                redirectUrl = Url.Action("BoardingPass", "TicketBooking"),
                isRedirect = true
            });

        }

        public ActionResult IssueBoardingPass(string id)
        {
            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();



            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);

                DataTable DT;
                DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 18));


                // BoardingDetails.UPDATEBoardingIssueStatus(Val[0].Split('-')[1].ToString(), Convert.ToString(Session["UserID"]));







                foreach (DataRow dr in DT.Rows)
                {
                    BoardingDetails.DisplayBookingId = Convert.ToString(dr["BoardingID"].ToString().Substring(0, 18));

                    BoardingDetails.PlaceName = Convert.ToString(dr["PlaceName"]);
                    BoardingDetails.NameOfVisitor = Convert.ToString(dr["NameOfVisitor"]);
                    BoardingDetails.DateofVisit = Convert.ToString(dr["DateOfVisit"]);
                    BoardingDetails.DateofBooking = Convert.ToString(dr["ReservatindDate"]);
                    BoardingDetails.Shift = Convert.ToString(dr["ShiftTime"]);
                    BoardingDetails.Vehicle = Convert.ToString(dr["VehicleName"]);
                    BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]);

                    BoardingDetails.Nationality = Convert.ToString(dr["Nationality"]);
                    BoardingDetails.IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]);
                    BoardingDetails.Camera = Convert.ToString(dr["NoOfCamera"]);
                    BoardingDetails.Amount = Convert.ToString(dr["AmountTobePaid"]);
                    BoardingDetails.BoardingPointName = Convert.ToString(dr["Boarding_Point"]);

                    BoardingDetails.PrintID = Encryption.encrypt(id);

                    BoardingDetails.GuidName = Convert.ToString(dr["GuidName"]);
                    BoardingDetails.VehicleNumber = Convert.ToString(dr["VehicleNumber"]);

                    BoardingDetails.EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]);
                    BoardingDetails.ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]);
                    BoardingDetails.ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]);

                }

                BoardingDetails.TotalMembers = Convert.ToString(DT.Rows.Count);

                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(


                      new CS_BoardingDetails()
                      {

                          Index = count,
                          DisplayBookingId = Convert.ToString(dr["BoardingID"].ToString()),
                          PlaceName = Convert.ToString(dr["PlaceName"]),
                          NameOfVisitor = Convert.ToString(dr["NameOfVisitor"]),
                          DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                          DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                          Shift = Convert.ToString(dr["ShiftTime"]),
                          Vehicle = Convert.ToString(dr["VehicleName"]),
                          ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]),
                          ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]),

                          Nationality = Convert.ToString(dr["Nationality"]),
                          IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]),
                          Camera = Convert.ToString(dr["NoOfCamera"]),
                          Amount = Convert.ToString(dr["AmountTobePaid"]),
                          BoardingPointName = Convert.ToString(dr["Boarding_Point"]),


                          PrintID = Encryption.encrypt(id),

                          GuidName = Convert.ToString(dr["GuidName"]),
                          VehicleNumber = Convert.ToString(dr["VehicleNumber"]),

                          EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]),
                          ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]),


                      });

                    count = count + 1;
                }

                ViewData["ListBoardingS"] = ListBoarding;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);

        }

        [HttpGet]
        public FileResult PrintBoardingPass(string id)
        {
            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();



            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);
                
                DataTable DT;
                if(id.Contains("C19-"))
                {
                    DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 22));
                }
                else
                {
                    DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 18)); //commented by shanawaz on 24 - 07 - 2020
                }
                
                

                string filepath = htmlToPdfDownloadTickets.WildlifeBoardingPassDownloadPdf(DT, DT_DptUser);

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


        public ActionResult PrintBoardingPassOld(string id)
        {

            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();



            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);

                DataTable DT;



                // BoardingDetails.UPDATEBoardingIssueStatus(Val[0].Split('-')[1].ToString(), Convert.ToString(Session["UserID"]));

                DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 18));

                foreach (DataRow dr in DT.Rows)
                {
                    BoardingDetails.DisplayBookingId = Convert.ToString(dr["BoardingID"].ToString().Substring(0, 18));

                    BoardingDetails.PlaceName = Convert.ToString(dr["PlaceName"]);
                    BoardingDetails.NameOfVisitor = Convert.ToString(dr["NameOfVisitor"]);
                    BoardingDetails.DateofVisit = Convert.ToString(dr["DateOfVisit"]);
                    BoardingDetails.DateofBooking = Convert.ToString(dr["ReservatindDate"]);
                    BoardingDetails.Shift = Convert.ToString(dr["ShiftTime"]);
                    BoardingDetails.Vehicle = Convert.ToString(dr["VehicleName"]);
                    BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]);

                    BoardingDetails.Nationality = Convert.ToString(dr["Nationality"]);
                    BoardingDetails.IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]);
                    BoardingDetails.Camera = Convert.ToString(dr["NoOfCamera"]);
                    BoardingDetails.Amount = Convert.ToString(dr["AmountTobePaid"]);
                    BoardingDetails.BoardingPointName = Convert.ToString(dr["Boarding_Point"]);



                    BoardingDetails.PrintID = Encryption.encrypt(id);

                    BoardingDetails.GuidName = Convert.ToString(dr["GuidName"]);
                    BoardingDetails.VehicleNumber = Convert.ToString(dr["VehicleNumber"]);

                    BoardingDetails.EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]);
                    BoardingDetails.ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]);
                    BoardingDetails.ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]);

                }
                BoardingDetails.TotalMembers = Convert.ToString(DT.Rows.Count);

                int count = 1;

                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(


                      new CS_BoardingDetails()
                      {


                          Index = count,
                          DisplayBookingId = Convert.ToString(dr["BoardingID"].ToString()),
                          PlaceName = Convert.ToString(dr["PlaceName"]),
                          NameOfVisitor = Convert.ToString(dr["NameOfVisitor"]),
                          DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                          DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                          Shift = Convert.ToString(dr["ShiftTime"]),
                          Vehicle = Convert.ToString(dr["VehicleName"]),
                          ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]),
                          ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]),

                          Nationality = Convert.ToString(dr["Nationality"]),
                          IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]),
                          Camera = Convert.ToString(dr["NoOfCamera"]),
                          Amount = Convert.ToString(dr["AmountTobePaid"]),
                          BoardingPointName = Convert.ToString(dr["Boarding_Point"]),


                          PrintID = Encryption.encrypt(id),

                          GuidName = Convert.ToString(dr["GuidName"]),
                          VehicleNumber = Convert.ToString(dr["VehicleNumber"]),

                          EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]),
                          ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]),


                      });
                    count = count + 1;

                }

                ViewData["ListBoardingS"] = ListBoarding;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);

        }

        public string BoardingPassPrint(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            StringBuilder SB = new StringBuilder();
            try
            {
                CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();

                id = Encryption.decrypt(id);

                DataTable DT;
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);


                // BoardingDetails.UPDATEBoardingIssueStatus(Val[0].Split('-')[1].ToString(), Convert.ToString(Session["UserID"]));

                DT = BoardingDetails.BoardingPassForOne(id.Substring(0, 18));



                SB.Append("<div class='wrapper'><section class='print-invoice'><div class='row border-divider'><div class='col-xs-12 col-sm-4'><img src='../../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class='' /></div><div class='col-xs-12 col-sm-4 centr'><span class='pdate'><h3>Boarding pass</h3></span></div>");
                SB.Append("<div class='col-xs-12 col-sm-4 sj-logo'></div><div class='divider'></div></div><div class='panel panel-default'><div class='panel-body'><div id='tbl_unbold' class='table-responsive'><table class='table table-bordered'><thead><tr>");

                SB.Append("<th>Boarding Pass : " + DT.Rows[0]["PlaceName"].ToString() + "</th><th style='text-align:right; padding-right:20px;' > Booking ID : " + DT.Rows[0]["BoardingID"].ToString().Substring(0, 18) + "</th></tr></thead></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Visit Date</th><th>Reservation Date</th><th>Shift</th><th>Route Name</th><th>Total Members</th><th>" + Convert.ToString(DT.Rows[0]["VehicleName"]) + "</th><th>Guide name </th><th>Ticket Amount</th><th>Mode Of Booking</th></tr></thead>");

                SB.Append("<tbody><tr><td>" + DT.Rows[0]["DateofVisit"].ToString() + "</td><td>" + DT.Rows[0]["ReservatindDate"].ToString() + "</td><td>" + DT.Rows[0]["ShiftTime"].ToString() + "</td>");
                SB.Append("<td>" + DT.Rows[0]["ZoneName"].ToString() + "</td> <td>" + DT.Rows.Count + " </td><td>" + Convert.ToString(DT.Rows[0]["VehicleNumber"]) + " </td><td>" + Convert.ToString(DT.Rows[0]["GuidName"]) + " </td>  <td>" + DT.Rows[0]["AmountTobePaid"].ToString() + " </td><td>" + DT.Rows[0]["ModeOfBooking"].ToString() + " </td></tr></tbody></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Sr. No.</th><th>Visitor Name </th><th>Nationality</th><th>Id proof / Id Details</th><th>Camera</th></tr></thead>");
                SB.Append("<tbody>");

                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    SB.Append("<tr><td>" + count + "</td>   <td>" + Convert.ToString(dr["NameOfVisitor"]) + "</td><td>" + Convert.ToString(dr["Nationality"]) + " </td><td>" + Convert.ToString(dr["IdproofIdDetails"]) + "  </td>");
                    SB.Append("<td>" + Convert.ToString(dr["NoOfCamera"]) + "</td></tr>");
                    count = count + 1;
                }

                SB.Append("</tbody></table></div>");


                SB.Append("<label><h4>Boarding Point :</h4></label><div class='divider'></div>");
                SB.Append("<p>" + DT.Rows[0]["Boarding_Point"].ToString() + " <br /><br />");
                SB.Append("Terms & Conditions :-<br />* Every Visitor has to pay Vehicle Rent and Guide Fee at time of collecting boarding pass additionally.<br />* The visitor must reach the boarding point at least 15 minutes prior to the departure time.<br />* Any violation of rules will be punishable under wildlife protection Act 1972</p>");
                SB.Append("<label><h4>Abide by the Rules of the National Park:</h4></label><div class='divider'></div>");
                SB.Append("<p>DO's:-1). Enter the park with a valid ticket. 2). Take official guide with you inside the park. 3) maximum speed limit is 20 km/hr.  4). Always carry drinking water. 5). Maintain silence and discipline during excursions. 6). Allow the animals to have the right of way. 7). Wear colors which match with Nature. 8). Please carry maps/Guide Book for your reference 9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass.<br />DONT'S:-1). don't get down, unless told by the guide. 2). Don't carry arms,explosives or intoxicants inside the park. 3). Don't blow horn. 4). Don't litter with cans,bottles, plastic bags etc. 5). Don't try to feed the animals. 6). Don't smoke or lit fire. 7). Don't tease or chase the animals. 8). Don't leave plastic/Polybags.</p>");
                SB.Append("<div class='print-bg-tkt'><div class='centr'>Designed and maintained by Forest Department.<br />Please contact " + DT_DptUser.Rows[0]["EmailId"].ToString() + ", Contact No. " + DT_DptUser.Rows[0]["Mobile"].ToString() + " for any kind of query.</div></div></div></div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return SB.ToString();
        }

        // Begin Arvind Change for Boarding pass log 10/11/2016

        //public JsonResult GuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, int GuideId, int VehicleID2, string IsCurrentBooking = "")
        public JsonResult GuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, string IsCurrentBooking = "")
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            int isBlocked = 0;
            var requesteId = Encryption.decrypt(ID);
            try
            {

                string UpdatedBy = Convert.ToString(Session["UserID"]);
                DataTable DT;DataTable dataTable;
                
                dataTable = obj.IsBlockedRequest(requesteId);
                 isBlocked =(int)dataTable.Rows[0]["Blocked"];
                if (isBlocked == 0)
                {
                    if (requesteId.Contains("C19-"))
                    {
                        DT = obj.UpdateGuidNameAndVehicleNumber(Encryption.decrypt(ID), GuidName.ToUpper(), VehicleNumber.ToUpper(), UserID.ToString());
                    }
                    else
                    {
                        DT = obj.UpdateGuidNameAndVehicleNumber(Encryption.decrypt(ID).Substring(0, 18), GuidName.ToUpper(), VehicleNumber.ToUpper(),  UserID.ToString());  //comment by shaan 24 - 07 - 2020
                    }
                }
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            // return RedirectToAction("BoardingPass", "TicketBooking");
            if (isBlocked == 0)
            {
                if (IsCurrentBooking == "")
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("PrintBoardingPass", "TicketBooking"),
                        isRedirect = true
                    });
                }
                else
                {
                    string STATUS = string.Empty; STATUS = "IsCurrentBooking";

                    return Json(new { STATUS }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
            
                var data = new { STATUS = "Blocked", Msg= "This Request id :" + requesteId + "is blocked,kindly contact to system administrator.!!!" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        // end  Arvind Change for Boarding pass log 10/11/2016
        public JsonResult ValidationForGuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime)
        //public JsonResult ValidationForGuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, int GuideId, int VehicleID2)
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            string STATUS = string.Empty;
            string rows = string.Empty;

            try
            {



                string UpdatedBy = Convert.ToString(Session["UserID"]);
                DataTable DT = new DataTable();
                DataSet DS = new DataSet();
                var requesteId = Encryption.decrypt(ID);
                if(requesteId.Contains("C19-"))
                {
                    DS = obj.ValidationForGuidNameAndVehicleNumber(GuidName.ToUpper(), VehicleNumber.ToUpper(), Place, ZoneID, DateOfArrival, VehicleID, ShiftTime, Encryption.decrypt(ID), UpdatedBy);
                } else
                {
                    DS = obj.ValidationForGuidNameAndVehicleNumber(GuidName.ToUpper(), VehicleNumber.ToUpper(), Place, ZoneID, DateOfArrival, VehicleID, ShiftTime, Encryption.decrypt(ID).Substring(0, 18), UpdatedBy);
                }
                if (DS.Tables[0].Rows.Count > 0)
                {
                    STATUS = "TRUE";
                }
                else
                {
                    STATUS = "FALSE";

                    if (DS.Tables[1].Rows.Count > 0)
                    {
                        rows = Convert.ToString(DS.Tables[1].Rows[0][0]);
                    }


                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(new { STATUS = STATUS, Remaining = rows }, JsonRequestBehavior.AllowGet);

        }

        // Begin Arvind Change for Boarding pass log 10/11/2016
        public JsonResult UpdateForNotArrived(string ID)
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            string STATUS = string.Empty;
            try
            {
                string ids = Encryption.decrypt(ID);

                string[] Val = ids.Split('_');
                string UpdatedBy = Convert.ToString(Session["UserID"]);
                DataTable DT;
                DT = obj.UpdateForNotArrived(Val[0].Split('-')[0].ToString(), Val[0].Split('-')[1].ToString(), UserID.ToString());
                if (DT.Rows.Count > 0)
                {
                    STATUS = "TRUE";
                }
                else
                {
                    STATUS = "FALSE";

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }



            return Json(new { STATUS }, JsonRequestBehavior.AllowGet);

        }
        // end  Arvind Change for Boarding pass log 10/11/2016


        public bool TimeBound(string Places)
        {


            // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
            // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

            CS_BoardingDetails objq = new CS_BoardingDetails();

            DateTime DateTimes = DateTime.Now;
            int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
            string DATEOFVISITE = string.Empty;
            string SHIFT_TYPE = string.Empty;
            bool status = false;
            DT_BoardingDuration = objq.GetBoardingDuration(Places);

            if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
            {
                // =========== EVENING_SHIFT
                if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "2";
                    status = true;
                }
            }
            if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
            {
                // =========== MORNING_SHIFT
                if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                    objq.DisplayShiftName = "Morning";
                    status = true;
                }
                else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                    objq.DisplayShiftName = "Morning";
                    status = true;

                }
            }
            else
            {
                status = false;
            }

            return status;
        }

        [HttpGet]

        public ActionResult IssueBoardingList()
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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
            }

            ListBoarding.Clear();

            ViewData["ListIssueBoarding"] = ListBoarding;


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {
                BoardingPassLoad();

                ViewData["ListBoarding"] = ListBoarding;
                Ds = obj.BindDptKioskPLACES(SSOID);



                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 1)
                    {

                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {

                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }

                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                        obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();

                        foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                        }



                    }

                    // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
                    // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                    // === set default date and shift as per database 

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;

                    DT_BoardingDuration = obj.GetBoardingDuration(obj.PlaceId);
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "2";
                                obj.DisplayShiftName = "Evening";
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            // =========== MORNING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                            else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                        }

                        obj.DateofVisit = DATEOFVISITE;
                        obj.Shift = SHIFT_TYPE;

                        ViewBag.LstddlZone = lstZone;
                        ViewBag.LstddlVehicle = lstVehicle;
                        ViewBag.LstddlPlace = lstPlace;


                    }

                    obj.DateofVisit = DATEOFVISITE;
                    obj.Shift = SHIFT_TYPE;

                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewBag.LstddlPlace = lstPlace;


                }



                // === set default date and shift as per database

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }

        public ActionResult IssueBoardingList(CS_BoardingDetails BoardingDetails)
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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

                if (!TimeBound(BoardingDetails.PlaceId))
                {

                    TempData["ValidIPAddress"] = "Boarding Pass Generation time period has expired";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");
                }


            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "BoardingPassList";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");



                DT = obj.GetCurrentDateBooking(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);
                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            GuidName = Convert.ToString(dr["GuidName"]),
                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            ZoneID = Convert.ToString(dr["ZoneIDBooking"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                            VisitorCount = Convert.ToString(dr["VisitorCount"]),
                            PlaceId = Convert.ToString(dr["PlaceID"]),
                            ShiftID = Convert.ToString(dr["ShiftID"]),
                            RequestID = Convert.ToString(dr["RequestID"]),
                        });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                Ds = obj.BindDptKioskPLACES(SSOID);

                if (Ds.Tables[0].Rows.Count > 1)
                {

                    ViewBag.MultiPlaceStatus = true;
                    foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                    {

                        lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                    }
                    ViewBag.LstddlPlace = lstPlace;
                }
                else if (Ds.Tables[0].Rows.Count == 1)
                {
                    ViewBag.MultiPlaceStatus = false;
                }



                DT_DptUser = BoardingDetails.GetZoneByPlace(BoardingDetails.PlaceId);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }

                ViewBag.LstddlZone = lstZone;


                DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID));

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.LstddlVehicle = lstVehicle;




                ViewData["ListIssueBoarding"] = ListBoarding;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);
        }


        

        public ActionResult BoardingPassListPrint(string id)
        {
            if (!Convert.ToBoolean(Session["IsDepartmentalKioskUser"]))
            {
                Response.Redirect("~/KioskDashboard/KioskDashboard", false);
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();
            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                string IDs = string.Empty;
                IDs = id;
                id = Encryption.decrypt(id);

                DataSet DS = new DataSet();

                string[] Val = id.Split('_');
                //"10/10/2016_ 2_ 1_ BoardingPassListPrint_ RJ14 51M 5561"
                DS = BoardingDetails.GetIssueBoardingListPrint(Val[0], Val[1], Val[2], Val[3], Val[4], Val[5], Val[6]);



                BoardingDetails.HDNBookingId = IDs;
                BoardingDetails.GuidName = Convert.ToString(DS.Tables[0].Rows[0]["GuidName"]);
                BoardingDetails.DateofVisit = Convert.ToString(DS.Tables[0].Rows[0]["DateOfVisit"]);
                BoardingDetails.PlaceName = Convert.ToString(DS.Tables[0].Rows[0]["PlaceName"]);
                BoardingDetails.Shift = Convert.ToString(DS.Tables[0].Rows[0]["ShiftTime"]);
                BoardingDetails.ZoneID = Convert.ToString(DS.Tables[0].Rows[0]["ZoneIDBooking"]);
                BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(DS.Tables[0].Rows[0]["ZoneNameBooking"]);
                BoardingDetails.Vehicle = Convert.ToString(DS.Tables[0].Rows[0]["VehicleName"]);
                BoardingDetails.VehicleNumber = Convert.ToString(DS.Tables[0].Rows[0]["VehicleNumber"]);
                BoardingDetails.VisitorCount = Convert.ToString(DS.Tables[0].Rows[0]["VisitorCount"]);
                BoardingDetails.BoardingPointName = Convert.ToString(DS.Tables[0].Rows[0]["BoardingPointName"]);

                int count = 1;
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            DisplayBookingId = Convert.ToString(dr["BoardingID"]),
                            NameOfVisitor = Convert.ToString(dr["NameOfVisitor"]),
                            IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]),
                            Camera = Convert.ToString(dr["NoOfCamera"]),

                        });
                    count += 1;
                }



                string filepath = htmlToPdfDownloadTickets.WildlifeBoardingPassListDownloadPdf(BoardingDetails, ListBoarding);

                if (System.IO.File.Exists(filepath))
                {

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
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return null;
        }


        public string BoardingListPrint(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            StringBuilder SB = new StringBuilder();
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();
            try
            {
                id = Encryption.decrypt(id);
                DataSet DS = new DataSet();
                string[] Val = id.Split('_');
                DS = BoardingDetails.GetIssueBoardingListPrint(Val[0], Val[1], Val[2], Val[3], Val[4], Val[5], Val[6]);


                SB.Append("<div class='wrapper'><section class='print-invoice'><div class='row border-divider'><div class='col-xs-12 col-sm-4'><img src='../../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class='' /></div><div class='col-xs-12 col-sm-4 centr'><span class='pdate'><h3>Boarding List</h3></span></div>");
                SB.Append("<div class='col-xs-12 col-sm-4 sj-logo'></div><div class='divider'></div></div><div class='panel panel-default'><div class='panel-body'><div id='tbl_unbold' class='table-responsive'><table class='table table-bordered'><thead><tr>");

                SB.Append("<th>" + Convert.ToString(DS.Tables[0].Rows[0]["PlaceName"]) + "</th><th style=' text-align:right; font-weight:normal;' >  [ <label>Booking Point :</label>" + DS.Tables[0].Rows[0]["BoardingPointName"] + " ]</th></tr></thead></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Date of Visit</th><th>Zone</th><th>Shift</th><th>Vehicle</th><th>Vehicle Number</th><th>Number Of Visitors</th><th >Guide Name</th></tr></thead>");

                SB.Append("<tbody><tr><td>" + Convert.ToString(DS.Tables[0].Rows[0]["DateOfVisit"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["ZoneNameBooking"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["ShiftTime"]) + "</td>");
                SB.Append("<td>" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleName"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleNumber"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["VisitorCount"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["GuidName"]) + "</td></tr></tbody></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th style='width:5%;' >S.NO.</th><th style='width:25%;' >Booking Id</th><th style='width:25%;'>Name Of Visitor</th><th style='width:25%;'>Idproof / IdDetails</th><th style='width:10%;' >Camera</th><th style='width:10%;' >Verify Visitor</th></tr></thead>");


                int count = 1;
                SB.AppendLine("<tbody><tr>");
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    SB.Append("<tr><td style='font-size:12px;'>" + count + "</td><td style='font-size:12px;'>" + Convert.ToString(dr["BoardingID"]) + " </td><td style='font-size:12px;'>" + Convert.ToString(dr["NameOfVisitor"]) + "  </td>");
                    SB.Append("<td style='font-size:12px;'>" + Convert.ToString(dr["IdproofIdDetails"]) + "</td><td style='font-size:12px;'>" + Convert.ToString(dr["NoOfCamera"]) + " </td><td></td></tr>");
                    count += 1;
                }
                SB.Append("</tbody></table></div>");

                SB.Append("</div></div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return SB.ToString();
        }

        public ActionResult GetNotArrivedTicketDetails(string Ticket)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            try
            {
                Ticket = Encryption.decrypt(Ticket);
                Ticket = Ticket.ToString().Substring(0, 18);
                DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(Ticket.ToString());

                if (dtKioskBoarding.Rows.Count > 0)
                {

                    obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                    obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                    obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                    obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();

                    obj.DateofVisit = dtKioskBoarding.Rows[0]["DateOfArrival"].ToString();
                    obj.Shift = dtKioskBoarding.Rows[0]["ShiftTime"].ToString();
                    obj.RequestID = Ticket.ToString();

                    string BOOKING_TYPE = "BoardingPass";
                    DataTable DT = new DataTable();
                    DT = obj.GetCurrentDateBooking(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);
                    ViewBag.ReqID = Encryption.encrypt(Ticket);

                    int count = 1;
                    Int16 ColorNumber = 0;
                    foreach (DataRow dr in DT.Rows)
                    {
                        Random random = new Random();
                        Color randomColor = new Color();

                        if (count == 1)
                        {
                            ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                            ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                        }
                        else
                        {
                            if (ViewBag.RID1 == Convert.ToString(dr["DisplayRequestID"].ToString()))
                            {

                            }
                            else
                            {
                                ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                                ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                                ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                            }
                        }

                        ListBoarding.Add(


                            new CS_BoardingDetails()
                            {

                                Index = count,
                                DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                                HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                                RequestID = Convert.ToString(dr["RequestID"].ToString()),
                                Colors = ViewBag.Finlacolor1,
                                PlaceName = Convert.ToString(dr["PlaceName"]),
                                NameOfVisitor = Convert.ToString(dr["Name"]),
                                DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                                DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                                Shift = Convert.ToString(dr["ShiftTime"]),
                                Vehicle = Convert.ToString(dr["VehicleName"]),
                                ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                                ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                                ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                                ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                                BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),
                                IsDepartmentalKioskUser = Convert.ToInt16(dr["IsDepartmentalKioskUser"].ToString() == string.Empty ? 0 : dr["IsDepartmentalKioskUser"]),
                                GuidName = Convert.ToString(dr["GuidName"]),
                                VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                                IsNotArrival = Convert.ToBoolean(dr["NotArrivedOnBoardingTimeStatus"]),
                            });
                        count += 1;
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_NotArrivedTicketDetails", ListBoarding);

        }


        [HttpPost]
        public ActionResult UpdateNotArrived(FormCollection FC)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);




            string ss = string.Empty;
            try
            {

                ss = Encryption.encrypt(FC["ReqID"].ToString().Substring(0, 18));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return RedirectToAction("BoardingPass", "BoardingMaster", new { KioskBoarding = ss.ToString() });

        }

        public ActionResult GetViewTicketDetails(string Ticket)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            try
            {
                Ticket = Encryption.decrypt(Ticket);
                Ticket = Ticket.ToString().Substring(0, 18);
                DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(Ticket.ToString());

                if (dtKioskBoarding.Rows.Count > 0)
                {

                    obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                    obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                    obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                    obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();

                    obj.DateofVisit = dtKioskBoarding.Rows[0]["DateOfArrival"].ToString();
                    obj.Shift = dtKioskBoarding.Rows[0]["ShiftTime"].ToString();
                    obj.RequestID = Ticket.ToString();

                    string BOOKING_TYPE = "BoardingPass";
                    DataTable DT = new DataTable();
                    DT = obj.GetCurrentDateBooking(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);


                    int count = 1;
                    Int16 ColorNumber = 0;
                    foreach (DataRow dr in DT.Rows)
                    {
                        Random random = new Random();
                        Color randomColor = new Color();

                        if (count == 1)
                        {
                            ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                            ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                        }
                        else
                        {
                            if (ViewBag.RID1 == Convert.ToString(dr["DisplayRequestID"].ToString()))
                            {

                            }
                            else
                            {
                                ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                                ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                                ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                            }
                        }

                        ListBoarding.Add(


                            new CS_BoardingDetails()
                            {

                                Index = count,
                                DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                                HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                                RequestID = Convert.ToString(dr["RequestID"].ToString()),
                                Colors = ViewBag.Finlacolor1,
                                PlaceName = Convert.ToString(dr["PlaceName"]),
                                NameOfVisitor = Convert.ToString(dr["Name"]),
                                DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                                DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                                Shift = Convert.ToString(dr["ShiftTime"]),
                                Vehicle = Convert.ToString(dr["VehicleName"]),
                                ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                                ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                                ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                                ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                                BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),
                                IsDepartmentalKioskUser = Convert.ToInt16(dr["IsDepartmentalKioskUser"].ToString() == string.Empty ? 0 : dr["IsDepartmentalKioskUser"]),
                                GuidName = Convert.ToString(dr["GuidName"]),
                                VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                                IsNotArrival = Convert.ToBoolean(dr["NotArrivedOnBoardingTimeStatus"]),
                            });
                        count += 1;
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_ViewTicketDetails", ListBoarding);

        }

        [HttpPost]
        public JsonResult GetAutoCompleteCity(string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, string RequestId)
        {
            try
            {
                #region City

                List<RequestList> result = new List<RequestList>();
                CS_BoardingDetails obj = new CS_BoardingDetails();

                DataTable dsRequestId = obj.GetAutoCompleteRequestId(Place, ZoneID, DateOfArrival, VehicleID, ShiftTime, RequestId);

                foreach (DataRow row in dsRequestId.Rows)
                {
                    RequestList list = new RequestList();
                    list.BookingId = row["RequestID"].ToString();
                    result.Add(list);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region "Online Boarding Pass for Citizen"
        [HttpGet]
        public ActionResult BoardingPassOnline(string KioskBoarding = "", string KioskBoardingDirect = "false")
        {

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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
            }

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {

                BoardingPassLoad();


                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");

                ViewBag.LstddlZone = lstZone;

                ViewBag.LstddlVehicle = lstVehicle;
                string tempPlaceId = string.Empty;
                if (Ds.Tables.Count > 0)
                {

                    if (Ds.Tables[0].Rows.Count > 1)
                    {

                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {

                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }

                        tempPlaceId = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                        obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();

                        foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                        }

                        tempPlaceId = obj.PlaceId;
                    }

                    // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
                    // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                    // === set default date and shift as per database 

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;

                    DT_BoardingDuration = obj.GetBoardingDuration(tempPlaceId);
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "2";
                                obj.DisplayShiftName = "Evening";
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            // =========== MORNING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                            else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                        }
                    }

                    obj.DateofVisit = DATEOFVISITE;
                    // obj.Shift = SHIFT_TYPE;

                    // === set default date and shift as per database


                    if (KioskBoarding != string.Empty)
                    {
                        KioskBoarding = Encryption.decrypt(KioskBoarding);
                        DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(KioskBoarding.ToString());


                        if (dtKioskBoarding.Rows.Count > 0)
                        {

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                            obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                            obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                            obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();


                            DT_DptUser = obj.GetZoneByPlace(dtKioskBoarding.Rows[0]["PlaceID"].ToString());

                            lstZone.Clear();
                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                            }




                            DT_DptUser = obj.GetVehicleByZoneAndPlace(Convert.ToInt32(dtKioskBoarding.Rows[0]["PlaceID"].ToString()), Convert.ToInt32(dtKioskBoarding.Rows[0]["ZoneID"].ToString()));

                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                            }

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();

                            string BOOKING_TYPE = "RequestIDWiseBoardingPassForVIPSeats";

                            #region If kiosk  User Direct Print Boarding Pass Developed by Rajveer.this parameter pass KioskBoardingDirect=true from KioskTransactionStatusWildlife views
                            if (!string.IsNullOrEmpty(KioskBoardingDirect) && KioskBoardingDirect.ToLower() == "true")
                            {
                                obj.RequestID = KioskBoarding.ToString();
                            }
                            #endregion

                            COMMONGETDATA(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);
                        }

                    }
                    else
                    {
                        ViewData["ListBoarding"] = ListBoarding;


                        ViewBag.LstddlPlace = lstPlace;
                        ViewBag.LstddlZone = lstZone;
                        ViewBag.LstddlVehicle = lstVehicle;

                    }

                }
                else
                {
                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;
                }
                ViewBag.GuideSummary = "";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BoardingPassOnline(CS_BoardingDetails BoardingDetails)
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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

                if (!TimeBound(BoardingDetails.PlaceId))
                {
                    TempData["ValidIPAddress"] = "Boarding Pass Generation time period has expired";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");

                }


            }

            ListBoarding.Clear();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "RequestIDWiseBoardingPassForVIPSeats";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");




                string SSOID = Session["SSOid"].ToString();
                // Ds = obj.BindDptKioskPLACESVIPSeats(SSOID);
                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 1)
                    {
                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {
                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                    }

                    COMMONGETDATA(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);

                    DT_DptUser = BoardingDetails.GetZoneByPlace(BoardingDetails.PlaceId);

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }




                    DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID));

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;

                }
                else
                {


                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(BoardingDetails);
        }




        #endregion



        #region Boarding Pass For VIP Seats
        [HttpGet]
        public ActionResult BoardingPassForVIPSeats(string KioskBoarding = "", string KioskBoardingDirect = "false")
        {

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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
            } 

            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {

                BoardingPassLoad();


                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");

                ViewBag.LstddlZone = lstZone;

                ViewBag.LstddlVehicle = lstVehicle;
                string tempPlaceId = string.Empty;
                if (Ds.Tables.Count > 0)
                {

                    if (Ds.Tables[0].Rows.Count > 1)
                    {

                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {

                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }

                        tempPlaceId = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                        obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();

                        foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                        }

                        tempPlaceId = obj.PlaceId;
                    }

                    // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
                    // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                    // === set default date and shift as per database 

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;

                    DT_BoardingDuration = obj.GetBoardingDuration(tempPlaceId);
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "2";
                                obj.DisplayShiftName = "Evening";
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            // =========== MORNING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                            else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                        }
                    }

                    obj.DateofVisit = DATEOFVISITE;
                   // obj.Shift = SHIFT_TYPE;

                    // === set default date and shift as per database


                    if (KioskBoarding != string.Empty)
                    {
                        KioskBoarding = Encryption.decrypt(KioskBoarding);
                        DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(KioskBoarding.ToString());


                        if (dtKioskBoarding.Rows.Count > 0)
                        {

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                            obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                            obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                            obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();


                            DT_DptUser = obj.GetZoneByPlace(dtKioskBoarding.Rows[0]["PlaceID"].ToString());

                            lstZone.Clear();
                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                            }




                            DT_DptUser = obj.GetVehicleByZoneAndPlace(Convert.ToInt32(dtKioskBoarding.Rows[0]["PlaceID"].ToString()), Convert.ToInt32(dtKioskBoarding.Rows[0]["ZoneID"].ToString()));

                            foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                            {
                                lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                            }

                            obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();

                            string BOOKING_TYPE = "RequestIDWiseBoardingPassForVIPSeats";

                            #region If kiosk  User Direct Print Boarding Pass Developed by Rajveer.this parameter pass KioskBoardingDirect=true from KioskTransactionStatusWildlife views
                            if (!string.IsNullOrEmpty(KioskBoardingDirect) && KioskBoardingDirect.ToLower() == "true")
                            {
                                obj.RequestID = KioskBoarding.ToString();
                            }
                            #endregion

                            COMMONGETDATA(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);
                        }

                    }
                    else
                    {
                        ViewData["ListBoarding"] = ListBoarding;


                        ViewBag.LstddlPlace = lstPlace;
                        ViewBag.LstddlZone = lstZone;
                        ViewBag.LstddlVehicle = lstVehicle;

                    }

                }
                else
                {
                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;
                }
                ViewBag.GuideSummary = "";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BoardingPassForVIPSeats(CS_BoardingDetails BoardingDetails)
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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

                if (!TimeBound(BoardingDetails.PlaceId))
                {
                    TempData["ValidIPAddress"] = "Boarding Pass Generation time period has expired";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");

                }


            }

            ListBoarding.Clear();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "RequestIDWiseBoardingPassForVIPSeats";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");




                string SSOID = Session["SSOid"].ToString();
               // Ds = obj.BindDptKioskPLACESVIPSeats(SSOID);
                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 1)
                    {
                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {
                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                    }

                    COMMONGETDATA(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);

                    DT_DptUser = BoardingDetails.GetZoneByPlace(BoardingDetails.PlaceId);

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }




                    DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlace(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID));

                    foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;

                }
                else
                {


                    ViewBag.GuideSummary = "";
                    ViewBag.LstddlPlace = lstPlace;
                    ViewBag.MultiPlaceStatus = false;
                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewData["ListBoarding"] = ListBoarding;


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(BoardingDetails);
        }

        public JsonResult ZoneByPlaceForVIPSeat(string PlaceId, string shiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();

            try
            {
                DT_DptUser = BoardingDetails.GetZoneByPlaceForVIPSeat(PlaceId, shiftType);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(lstZone, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetVehicleByZoneAndPlaceForVIPSeats(int PlaceId, int ZoneID,int ShiftType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();

            try
            {
                DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlaceForVIPSeats(PlaceId, ZoneID, ShiftType);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetViewTicketDetailsForVIPSeats(string Ticket)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            try
            {
                Ticket = Encryption.decrypt(Ticket);
                if(Ticket.Contains("C19-"))
                {
                    Ticket = Ticket.Substring(0,22).ToString();
                }
                else
                {
                    Ticket = Ticket.ToString().Substring(0, 18);
                }
               
                DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(Ticket.ToString());

                if (dtKioskBoarding.Rows.Count > 0)
                {

                    obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                    obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                    obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                    obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();

                    obj.DateofVisit = dtKioskBoarding.Rows[0]["DateOfArrival"].ToString();
                    obj.Shift = dtKioskBoarding.Rows[0]["ShiftTime"].ToString();
                    obj.RequestID = Ticket.ToString();

                    string BOOKING_TYPE = "BoardingPassForVIPSeats";
                    DataTable DT = new DataTable();
                    DT = obj.GetCurrentDateBooking(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);


                    int count = 1;
                    Int16 ColorNumber = 0;
                    foreach (DataRow dr in DT.Rows)
                    {
                        Random random = new Random();
                        Color randomColor = new Color();

                        if (count == 1)
                        {
                            ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                            ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                        }
                        else
                        {
                            if (ViewBag.RID1 == Convert.ToString(dr["DisplayRequestID"].ToString()))
                            {

                            }
                            else
                            {
                                ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                                ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                                ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                            }
                        }

                        ListBoarding.Add(


                            new CS_BoardingDetails()
                            {

                                Index = count,
                                DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                                HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                                RequestID = Convert.ToString(dr["RequestID"].ToString()),
                                Colors = ViewBag.Finlacolor1,
                                PlaceName = Convert.ToString(dr["PlaceName"]),
                                NameOfVisitor = Convert.ToString(dr["Name"]),
                                DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                                DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                                Shift = Convert.ToString(dr["ShiftTime"]),
                                Vehicle = Convert.ToString(dr["VehicleName"]),
                                ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                                ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                                ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                                ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                                BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),
                                IsDepartmentalKioskUser = Convert.ToInt16(dr["IsDepartmentalKioskUser"].ToString() == string.Empty ? 0 : dr["IsDepartmentalKioskUser"]),
                                GuidName = Convert.ToString(dr["GuidName"]),
                                VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                                IsNotArrival = Convert.ToBoolean(dr["NotArrivedOnBoardingTimeStatus"]),
                            });
                        count += 1;
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_ViewTicketDetails", ListBoarding);

        }

        public ActionResult GetViewTicketDetailsSeats(string VehicleNumber)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            try
            {
                VehicleNumber = Encryption.decrypt(VehicleNumber);
               // VehicleNumber = Ticket.ToString().Substring(0, 18);
                DataTable dtKioskBoarding = obj.GetLoadDataIfKioskUser(VehicleNumber.ToString());

                if (dtKioskBoarding.Rows.Count > 0)
                {

                    obj.PlaceId = dtKioskBoarding.Rows[0]["PlaceID"].ToString();
                    obj.PlaceName = dtKioskBoarding.Rows[0]["PlaceName"].ToString();

                    obj.ZoneID = dtKioskBoarding.Rows[0]["ZoneID"].ToString();

                    obj.Vehicle = dtKioskBoarding.Rows[0]["VehicleID"].ToString();

                    obj.DateofVisit = dtKioskBoarding.Rows[0]["DateOfArrival"].ToString();
                    obj.Shift = dtKioskBoarding.Rows[0]["ShiftTime"].ToString();
                    obj.RequestID = VehicleNumber.ToString();

                    string BOOKING_TYPE = "BoardingPassForVIPSeats";
                    DataTable DT = new DataTable();
                    DT = obj.GetCurrentDateBooking(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);


                    int count = 1;
                    Int16 ColorNumber = 0;
                    foreach (DataRow dr in DT.Rows)
                    {
                        Random random = new Random();
                        Color randomColor = new Color();

                        if (count == 1)
                        {
                            ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                            ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                        }
                        else
                        {
                            if (ViewBag.RID1 == Convert.ToString(dr["DisplayRequestID"].ToString()))
                            {

                            }
                            else
                            {
                                ColorNumber = Convert.ToInt16(ColorNumber + Convert.ToInt16(1));

                                ViewBag.Finlacolor1 = getnewcolor(ColorNumber);
                                ViewBag.RID1 = Convert.ToString(dr["DisplayRequestID"].ToString());
                            }
                        }

                        ListBoarding.Add(


                            new CS_BoardingDetails()
                            {

                                Index = count,
                                DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                                HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString()),
                                RequestID = Convert.ToString(dr["RequestID"].ToString()),
                                Colors = ViewBag.Finlacolor1,
                                PlaceName = Convert.ToString(dr["PlaceName"]),
                                NameOfVisitor = Convert.ToString(dr["Name"]),
                                DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                                DateofBooking = Convert.ToString(dr["ReservatindDate"]),
                                Shift = Convert.ToString(dr["ShiftTime"]),
                                Vehicle = Convert.ToString(dr["VehicleName"]),
                                ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                                ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneNameBoarding"]),
                                ZoneID = Convert.ToString(dr["ZoneIDBoarding"]),
                                ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]),
                                BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]),
                                IsDepartmentalKioskUser = Convert.ToInt16(dr["IsDepartmentalKioskUser"].ToString() == string.Empty ? 0 : dr["IsDepartmentalKioskUser"]),
                                GuidName = Convert.ToString(dr["GuidName"]),
                                VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                                IsNotArrival = Convert.ToBoolean(dr["NotArrivedOnBoardingTimeStatus"]),
                            });
                        count += 1;
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_ViewTicketDetails", ListBoarding);

        }



        [HttpGet]

        public ActionResult IssueBoardingListForVIP()
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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
            }

            ListBoarding.Clear();

            ViewData["ListIssueBoarding"] = ListBoarding;


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {
                BoardingPassLoad();
                ViewBag.MultiPlaceStatus = false;
                ViewData["ListBoarding"] = ListBoarding;
                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");



                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 1)
                    {

                        ViewBag.MultiPlaceStatus = true;
                        foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                        {

                            lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                        }
                        ViewBag.MultiPlaceStatus = true;
                       // obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                    }
                    else if (Ds.Tables[0].Rows.Count == 1)
                    {
                        ViewBag.MultiPlaceStatus = false;
                        obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                        obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();

                        foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                        }



                    }

                    // Morning Shift  : previous day evening 16:00 (04 PM) to next day morning 06:00 (04 PM)
                    // Evening Shift  : current day afternoon 10:00 (10 AM) to current day afternoon 15:00 (03 PM)

                    // === set default date and shift as per database 

                    DateTime DateTimes = DateTime.Now;
                    int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));
                    string DATEOFVISITE = string.Empty;
                    string SHIFT_TYPE = string.Empty;

                    DT_BoardingDuration = obj.GetBoardingDurationForVIPSeats(obj.PlaceId, string.Empty, "GetBoardingDurationForVIPSeats");
                    if (DT_BoardingDuration.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                        {
                            // =========== EVENING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "2";
                                obj.DisplayShiftName = "Evening";
                            }
                        }
                        if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                        {
                            // =========== MORNING_SHIFT
                            if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                            else if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                            {
                                DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                                SHIFT_TYPE = "1";
                                obj.DisplayShiftName = "Morning";
                            }
                        }

                        obj.DateofVisit = DATEOFVISITE;
                        obj.Shift = SHIFT_TYPE;

                        ViewBag.LstddlZone = lstZone;
                        ViewBag.LstddlVehicle = lstVehicle;
                        ViewBag.LstddlPlace = lstPlace;


                    }

                    obj.DateofVisit = DATEOFVISITE;
                    obj.Shift = SHIFT_TYPE;

                    ViewBag.LstddlZone = lstZone;
                    ViewBag.LstddlVehicle = lstVehicle;
                    ViewBag.LstddlPlace = lstPlace;


                }



                // === set default date and shift as per database

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }

        [HttpPost]

        public ActionResult IssueBoardingListForVIP(CS_BoardingDetails BoardingDetails)
        {
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
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

                if (!TimeBound(BoardingDetails.PlaceId))
                {

                    TempData["ValidIPAddress"] = "Boarding Pass Generation time period has expired";
                    return RedirectToAction("KioskDashboard", "KioskDashboard");
                }


            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "BoardingPassList";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");



                DT = obj.GetCurrentDateBooking(BoardingDetails.DateofVisit, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE, BoardingDetails.ZoneID, BoardingDetails.Vehicle, BoardingDetails.RequestID);
                int count = 1;
                foreach (DataRow dr in DT.Rows)
                {
                    ListBoarding.Add(
                        new CS_BoardingDetails()
                        {
                            Index = count,
                            GuidName = Convert.ToString(dr["GuidName"]),
                            DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            Shift = Convert.ToString(dr["ShiftTime"]),
                            ZoneID = Convert.ToString(dr["ZoneIDBooking"]),
                            ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneNameBooking"]),
                            Vehicle = Convert.ToString(dr["VehicleName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),
                            VisitorCount = Convert.ToString(dr["VisitorCount"]),
                            PlaceId = Convert.ToString(dr["PlaceID"]),
                            ShiftID = Convert.ToString(dr["ShiftID"]),
                            RequestID = Convert.ToString(dr["RequestID"]),
                        });
                    count += 1;
                }


                string SSOID = Session["SSOid"].ToString();
                Ds = obj.BindDptKioskPLACESVIPSeatsForPlaceWise(SSOID, "GETLOADDATAFORVIPBoardingPass");

                if (Ds.Tables[0].Rows.Count > 1)
                {

                    ViewBag.MultiPlaceStatus = true;
                    foreach (System.Data.DataRow dr2 in Ds.Tables[0].Rows)
                    {

                        lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
                    }
                    ViewBag.LstddlPlace = lstPlace;
                }
                else if (Ds.Tables[0].Rows.Count == 1)
                {
                    ViewBag.MultiPlaceStatus = false;
                }



                DT_DptUser = BoardingDetails.GetZoneByPlace(BoardingDetails.PlaceId);

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }

                ViewBag.LstddlZone = lstZone;


                DT_DptUser = BoardingDetails.GetVehicleByZoneAndPlaceForVIPSeats(Convert.ToInt32(BoardingDetails.PlaceId), Convert.ToInt32(BoardingDetails.ZoneID), Convert.ToInt32(BoardingDetails.Shift));

                foreach (System.Data.DataRow dr in DT_DptUser.Rows)
                {
                    lstVehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.LstddlVehicle = lstVehicle;

                ViewData["ListIssueBoarding"] = ListBoarding;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);
        }


        #endregion

        #region "Holiday Seat Master"
        [HttpGet]
        public ActionResult holidaySeats(string value)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (!string.IsNullOrEmpty(value))
                TempData["Message"] = value; //(value == "Success" ? "Record Saved successfully" : "There is some issue is action");

            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            cls_HolidayDetails model = new cls_HolidayDetails();
            try
            {
                DataTable dt = model.SelectAllRules();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cls_HolidayDetails>>(str);
                    model.Model = model.List.LastOrDefault();
                }
                List<SelectListItem> list = new List<SelectListItem>() { new SelectListItem { Text = "Active", Value = "true" }, new SelectListItem { Text = "DeActive", Value = "false" } };
                ViewBag.ISactivelst = list;
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(model);
        }

        [HttpGet]
        public ActionResult HolidayMasterDetails(string ID,string PlaceId, string PlaceName)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TempData["PlaceName"] = PlaceName;
            cls_HolidayDetails model = new cls_HolidayDetails();
            try
            {
                DataTable dt = model.SelectHolidayRuleDetails(ID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    model.RuleDetailsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RuleDetails>>(str);

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_PartialHolidayMasterView", model);
        }

        public ActionResult AddHolidayMasterDetails(string ID, string PlaceName)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TempData["PlaceName"] = PlaceName;
            AddRuleDetails model = new AddRuleDetails();
            model.RuleId = Convert.ToInt16(ID);
            return PartialView("_PartialAddHolidayMasterDetails", model);
        }


        public ActionResult AddRequest(AddRuleDetails details)
        {
            AddRuleDetails model = new AddRuleDetails();
            DataTable dt= model.addHolidayDetails(details);
            string RuleId = details.RuleId.ToString();

            if (Convert.ToString(dt.Rows[0]["Status"]) == "1")
            {
                // SMS_EMail_Services.sendSingleSMS("7568246030", "Dear Sir, Kindly Approve Inventory request for RTR. From Date :" + details.FromDate + " and ToDate : " + details.ToDate + "Revert back with your responce to 7065051222. Message Text: FMDSS RTR_OB <YES/NO>");

                cls_HolidayDetails oHoliday = new cls_HolidayDetails();

                oHoliday.SendHolidayEmailSMS((int)SMSTEmplate.HolidaySeat_Request,details.RuleId);

                return RedirectToAction("holidaySeats", "BoardingMaster", new { value = Convert.ToString(dt.Rows[0]["MSG"])});
            }
            else
                return RedirectToAction("holidaySeats", "BoardingMaster", new { value = Convert.ToString(dt.Rows[0]["MSG"]) });
        }


        #endregion

    }
    public class RequestList
    {
        public string BookingId { get; set; }
    }
}
