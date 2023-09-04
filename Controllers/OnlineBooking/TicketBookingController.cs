//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : File contains calling functions from UI
//  Date Created : 13-06-2015
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  :rajkumar
//  Modified On  :29-08-2016
//  Reviewed By  : Anshul Bansal
//  Reviewed On  : Amar Swain 
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


namespace FMDSS.Controllers.OnlineBooking
{
    [MyAuthorization]
    public class TicketBookingController : BaseController
    {
        //
        // GET: /TicketBooking/ 
        //Note: Code Updated with Ref. to bug ID 77
        //Note: Code Updated with Ref. to bug ID 86,106,107,108,110,116,117,118 in .cshtml file
        List<SelectListItem> Accomodation = new List<SelectListItem>();
        List<SelectListItem> vehicleCategory = new List<SelectListItem>();
        List<CS_Ticket> ticketList = new List<CS_Ticket>();
        List<CS_BoardingDetails> ListBoarding = new List<CS_BoardingDetails>();
        List<SelectListItem> lstZone = new List<SelectListItem>();
        List<SelectListItem> lstPlace = new List<SelectListItem>();
        DataTable DT_DptUser;
        DataSet Ds = new DataSet();
        int ModuleID = 1;
        /// <summary>
        /// Renders UI for Online Ticket Booking
        /// </summary>
        /// <returns>View for  Online Ticke and bind all booked ticket</returns>
        /// 



        [HttpGet]

        public ActionResult BoardingPass()
        {
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
                if (TempData["Details"] != null)
                {
                    ListBoarding = (List<CS_BoardingDetails>)(TempData["Details"]);
                    ViewData["ListBoarding"] = ListBoarding;
                    Ds = obj.BindDptKioskPLACES(SSOID);
                    foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                    {
                        lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }
                    ViewBag.LstddlZone = lstZone;


                }
                else
                {

                    ViewData["ListBoarding"] = ListBoarding;
                }
                obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }

        [NonAction]

        public void BoardingPassLoad()
        {
            List<SelectListItem> lstShiftType = new List<SelectListItem>();

            lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
            lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });
            lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });

            ViewBag.LstddlShift = lstShiftType;




        }

        public ActionResult BoardingPass(CS_BoardingDetails BoardingDetails)
        {
            ListBoarding.Clear();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                BoardingPassLoad();
                CS_BoardingDetails obj = new CS_BoardingDetails();
                DataTable DT;
                string BOOKING_TYPE = "BoardingPass";
                string Date = string.Empty;
                Date = DateTime.Now.ToString("dd-MM-yyyy");

                Date = "10/10/2016";

              //  DT = obj.GetCurrentDateBooking(Date, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE);
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

                            GuidName = Convert.ToString(dr["GuidName"]),
                            VehicleNumber = Convert.ToString(dr["VehicleNumber"]),

                        });
                    count += 1;
                }
                string SSOID = Session["SSOid"].ToString();
                Ds = obj.BindDptKioskPLACES(SSOID);

                //ViewBag.PlaceID = DT.Rows[0][0].ToString();
                //ViewBag.PlaceName = DT.Rows[0][1].ToString();


                foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                {
                    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }

                ViewBag.LstddlZone = lstZone;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            // TempData["Details"] = ListBoarding;
            ViewData["ListBoarding"] = ListBoarding;
            return View(BoardingDetails);
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
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();



            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);

                DataTable DT;

                string[] Val = id.Split('_');

                BoardingDetails.UPDATEBoardingIssueStatus(Val[0].Split('-')[1].ToString(), Convert.ToString(Session["UserID"]));

                DT = BoardingDetails.BoardingPassForOne(Val[0].Split('-')[1].ToString());

                foreach (DataRow dr in DT.Rows)
                {
                    BoardingDetails.DisplayBookingId = Val[1];
                    BoardingDetails.HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString());
                    BoardingDetails.PlaceName = Convert.ToString(dr["PlaceName"]);
                    BoardingDetails.NameOfVisitor = Convert.ToString(dr["Name"]);
                    BoardingDetails.DateofVisit = Convert.ToString(dr["DateOfVisit"]);
                    BoardingDetails.DateofBooking = Convert.ToString(dr["ReservatindDate"]);
                    BoardingDetails.Shift = Convert.ToString(dr["ShiftTime"]);
                    BoardingDetails.Vehicle = Convert.ToString(dr["VehicleName"]);
                    BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneID = Convert.ToString(dr["ZoneID"]);
                    BoardingDetails.Nationality = Convert.ToString(dr["Nationality"]);
                    BoardingDetails.IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]);
                    BoardingDetails.Camera = Convert.ToString(dr["NoOfCamera"]);
                    BoardingDetails.Amount = Convert.ToString(dr["AmountTobePaid"]);
                    BoardingDetails.BoardingPointName = Convert.ToString(dr["Boarding_Point"]);

                    BoardingDetails.ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]);
                    BoardingDetails.BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]);
                    BoardingDetails.PrintID = Encryption.encrypt(id);

                    BoardingDetails.GuidName = Convert.ToString(dr["GuidName"]);
                    BoardingDetails.VehicleNumber = Convert.ToString(dr["VehicleNumber"]);

                    BoardingDetails.EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]);
                    BoardingDetails.ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]);


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);

        }


        public ActionResult PrintBoardingPass(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();
            try
            {
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                id = Encryption.decrypt(id);


                DataTable DT;

                string[] Val = id.Split('_');

                DT = BoardingDetails.BoardingPassForOne(Val[0].Split('-')[1].ToString());

                foreach (DataRow dr in DT.Rows)
                {
                    BoardingDetails.DisplayBookingId = Val[1];
                    BoardingDetails.HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString());
                    BoardingDetails.PlaceName = Convert.ToString(dr["PlaceName"]);
                    BoardingDetails.NameOfVisitor = Convert.ToString(dr["Name"]);
                    BoardingDetails.DateofVisit = Convert.ToString(dr["DateOfVisit"]);
                    BoardingDetails.DateofBooking = Convert.ToString(dr["ReservatindDate"]);
                    BoardingDetails.Shift = Convert.ToString(dr["ShiftTime"]);
                    BoardingDetails.Vehicle = Convert.ToString(dr["VehicleName"]);
                    BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneID = Convert.ToString(dr["ZoneID"]);
                    BoardingDetails.Nationality = Convert.ToString(dr["Nationality"]);
                    BoardingDetails.IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]);
                    BoardingDetails.Camera = Convert.ToString(dr["NoOfCamera"]);
                    BoardingDetails.Amount = Convert.ToString(dr["AmountTobePaid"]);
                    BoardingDetails.BoardingPointName = Convert.ToString(dr["Boarding_Point"]);

                    BoardingDetails.ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]);
                    BoardingDetails.BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]);
                    BoardingDetails.PrintID = Encryption.encrypt(id);

                    BoardingDetails.GuidName = Convert.ToString(dr["GuidName"]);
                    BoardingDetails.VehicleNumber = Convert.ToString(dr["VehicleNumber"]);

                    BoardingDetails.EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]);
                    BoardingDetails.ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]);


                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(BoardingDetails); ;
        }

        public string BoardingPassPrint(string id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            StringBuilder SB = new StringBuilder();
            try
            {

                id = Encryption.decrypt(id);

                CS_BoardingDetails BoardingDetails = new CS_BoardingDetails();
                DT_DptUser = BoardingDetails.GetDptUserEmail(UserID);
                DataTable DT;

                string[] Val = id.Split('_');

                DT = BoardingDetails.BoardingPassForOne(Val[0].Split('-')[1].ToString());

                foreach (DataRow dr in DT.Rows)
                {
                    BoardingDetails.DisplayBookingId = Val[1];
                    BoardingDetails.HDNBookingId = Convert.ToString(dr["HDNRequestID"].ToString());
                    BoardingDetails.PlaceName = Convert.ToString(dr["PlaceName"]);
                    BoardingDetails.NameOfVisitor = Convert.ToString(dr["Name"]);
                    BoardingDetails.DateofVisit = Convert.ToString(dr["DateOfVisit"]);
                    BoardingDetails.DateofBooking = Convert.ToString(dr["ReservatindDate"]);
                    BoardingDetails.Shift = Convert.ToString(dr["ShiftTime"]);
                    BoardingDetails.Vehicle = Convert.ToString(dr["VehicleName"]);
                    BoardingDetails.ZoneAtTheTimeOfBooking = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneAtTheTimeOfBoarding = Convert.ToString(dr["ZoneName"]);
                    BoardingDetails.ZoneID = Convert.ToString(dr["ZoneID"]);
                    BoardingDetails.Nationality = Convert.ToString(dr["Nationality"]);
                    BoardingDetails.IdproofIdDetails = Convert.ToString(dr["IdproofIdDetails"]);
                    BoardingDetails.Camera = Convert.ToString(dr["NoOfCamera"]);
                    BoardingDetails.Amount = Convert.ToString(dr["AmountTobePaid"]);
                    BoardingDetails.BoardingPointName = Convert.ToString(dr["Boarding_Point"]);

                    BoardingDetails.ZoneUpdateStatus = Convert.ToInt32(dr["ZoneUpdateStatus"]);
                    BoardingDetails.BoardingIssueStatus = Convert.ToInt32(dr["BoardingIssueStatus"]);

                    BoardingDetails.GuidName = Convert.ToString(dr["GuidName"]);
                    BoardingDetails.VehicleNumber = Convert.ToString(dr["VehicleNumber"]);

                    BoardingDetails.EmailIDDeptUser = Convert.ToString(DT_DptUser.Rows[0]["EmailId"]);
                    BoardingDetails.ContactNoDeptUser = Convert.ToString(DT_DptUser.Rows[0]["Mobile"]);



                }


                SB.Append("<div class='wrapper'><section class='print-invoice'><div class='row border-divider'><div class='col-xs-12 col-sm-4'><img src='../../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class='' /></div><div class='col-xs-12 col-sm-4 centr'><span class='pdate'><h3>Boarding pass</h3></span></div>");
                SB.Append("<div class='col-xs-12 col-sm-4 sj-logo'></div><div class='divider'></div></div><div class='panel panel-default'><div class='panel-body'><div id='tbl_unbold' class='table-responsive'><table class='table table-bordered'><thead><tr>");

                SB.Append("<th>" + BoardingDetails.PlaceName + "</th><th>Booking No. " + BoardingDetails.DisplayBookingId + "</th></tr></thead></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Reservation Date</th><th>Date of Visit</th><th>Shift</th><th>Route Name</th><th>" + BoardingDetails.Vehicle + "</th><th>Guide Name</th></tr></thead>");

                SB.Append("<tbody><tr><td>" + BoardingDetails.DateofBooking + "</td><td>" + BoardingDetails.DateofVisit + "</td><td>" + BoardingDetails.Shift + "</td>");
                SB.Append("<td>" + BoardingDetails.ZoneAtTheTimeOfBooking + "</td><td>" + BoardingDetails.VehicleNumber + "</td><td>" + BoardingDetails.GuidName + "</td></tr></tbody></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Visitor Name </th><th>Nationality</th><th>Id proof & Id Details</th><th>Camera</th><th>Amount</th></tr></thead>");

                SB.Append("<tbody><tr><td>" + BoardingDetails.NameOfVisitor + "</td><td>" + BoardingDetails.Nationality + " </td><td>" + BoardingDetails.IdproofIdDetails + "  </td>");
                SB.Append("<td>" + BoardingDetails.Camera + "</td><td>" + BoardingDetails.Amount + " </td></tr></tbody></table></div>");

                SB.Append("<label><h4>Boarding Point Name:</h4></label><div class='divider'></div>");
                SB.Append("<p>" + BoardingDetails.BoardingPointName + " <br /><br />");
                SB.Append("Terms & Conditions :-<br />* Every Visitor has to pay Vehicle Rent and Guide Fee at time of collecting boarding pass additionally.<br />* The visitor must reach the boarding point at least 15 minutes prior to the departure time.<br />* Any violation of rules will be punishable under wildlife protection Act 1972</p>");
                SB.Append("<label><h4>Abide by the Rules of the National Park:</h4></label><div class='divider'></div>");
                SB.Append("<p>DO's:-1). Enter the park with a valid ticket. 2). Take official guide with you inside the park. 3) maximum speed limit is 20 km/hr.  4). Always carry drinking water. 5). Maintain silence and discipline during excursions. 6). Allow the animals to have the right of way. 7). Wear colors which match with Nature. 8). Please carry maps/Guide Book for your reference 9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass.<br />DONT'S:-1). don't get down, unless told by the guide. 2). Don't carry arms,explosives or intoxicants inside the park. 3). Don't blow horn. 4). Don't litter with cans,bottles, plastic bags etc. 5). Don't try to feed the animals. 6). Don't smoke or lit fire. 7). Don't tease or chase the animals. 8). Don't leave plastic/Polybags.</p>");
                SB.Append("<div class='print-bg-tkt'><div class='centr'>Designed and maintained by Forest Department.<br />Please contact " + BoardingDetails.EmailIDDeptUser + ", Contact No. " + BoardingDetails.ContactNoDeptUser + " for any kind of query.</div></div></div></div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return SB.ToString();
        }


        public JsonResult GuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber)
        {
            CS_BoardingDetails obj = new CS_BoardingDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            try
            {
                string ids = Encryption.decrypt(ID);

                string[] Val = ids.Split('_');
                string UpdatedBy = Convert.ToString(Session["UserID"]);
                DataTable DT;
                DT = obj.UpdateGuidNameAndVehicleNumber(Val[0].Split('-')[1].ToString(), GuidName, VehicleNumber, Val[1]);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            // return RedirectToAction("BoardingPass", "TicketBooking");
            return Json(new
            {
                redirectUrl = Url.Action("PrintBoardingPass", "TicketBooking"),
                isRedirect = true
            });

        }







        [HttpGet]

        public ActionResult IssueBoardingList()
        {
            ListBoarding.Clear();

            ViewData["ListIssueBoarding"] = ListBoarding;


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            CS_BoardingDetails obj = new CS_BoardingDetails();

            string SSOID = Session["SSOid"].ToString();
            try
            {
                Ds = obj.BindDptKioskPLACES(SSOID);
                BoardingPassLoad();
                if (TempData["Details"] != null)
                {
                    ListBoarding = (List<CS_BoardingDetails>)(TempData["Details"]);
                    ViewData["ListIssueBoarding"] = ListBoarding;
                    Ds = obj.BindDptKioskPLACES(SSOID);
                    foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                    {
                        lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }
                    ViewBag.LstddlZone = lstZone;


                }
                else
                {

                    ViewData["ListIssueBoarding"] = ListBoarding;
                }
                obj.PlaceId = Ds.Tables[0].Rows[0][0].ToString();
                obj.PlaceName = Ds.Tables[0].Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }


            return View(obj);
        }
        public ActionResult IssueBoardingList(CS_BoardingDetails BoardingDetails)
        {


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

                Date = "10/10/2016";

                DT = obj.GetCurrentDateBooking(Date, BoardingDetails.PlaceId, BoardingDetails.Shift, BOOKING_TYPE);
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
                        });
                    count += 1;
                }
                string SSOID = Session["SSOid"].ToString();
                // Ds = obj.BindDptKioskPLACES(SSOID);


                //foreach (System.Data.DataRow dr in Ds.Tables[1].Rows)
                //{
                //    lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                //}

                //  TempData["Details"] = ListBoarding;
                ViewData["ListIssueBoarding"] = ListBoarding;
                ViewBag.LstddlZone = lstZone;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(BoardingDetails);
        }



        public ActionResult BoardingPassListPrint(string id)
        {
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
                DS = BoardingDetails.GetIssueBoardingListPrint(Val[0], Val[1], Val[2], Val[3], Val[4]);

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
                ViewData["ListIssueBoardingPrint"] = ListBoarding;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(BoardingDetails);
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
                DS = BoardingDetails.GetIssueBoardingListPrint(Val[0], Val[1], Val[2], Val[3], Val[4]);


                SB.Append("<div class='wrapper'><section class='print-invoice'><div class='row border-divider'><div class='col-xs-12 col-sm-4'><img src='../../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class='' /></div><div class='col-xs-12 col-sm-4 centr'><span class='pdate'><h3>Boarding List</h3></span></div>");
                SB.Append("<div class='col-xs-12 col-sm-4 sj-logo'></div><div class='divider'></div></div><div class='panel panel-default'><div class='panel-body'><div id='tbl_unbold' class='table-responsive'><table class='table table-bordered'><thead><tr>");

                SB.Append("<th>" + Convert.ToString(DS.Tables[0].Rows[0]["PlaceName"]) + "</th></tr></thead></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th>Date of Visit</th><th>Zone</th><th>Shift</th><th>Vehicle</th><th>Vehicle Number</th><th>Number Of Visitors</th><th >Guide Name</th></tr></thead>");

                SB.Append("<tbody><tr><td>" + Convert.ToString(DS.Tables[0].Rows[0]["DateOfVisit"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["ZoneNameBooking"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["ShiftTime"]) + "</td>");
                SB.Append("<td>" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleName"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleNumber"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["VisitorCount"]) + "</td><td>" + Convert.ToString(DS.Tables[0].Rows[0]["GuidName"]) + "</td></tr></tbody></table></div>");

                SB.Append("<div id='tbl_unbold' class='table-responsive'><table class='table table-striped table-bordered table-hover'>");
                SB.Append("<thead><tr><th style='width:5%;' >SR. NO.</th><th style='width:25%;' >Booking Id</th><th style='width:25%;'>Name Of Visitor</th><th style='width:25%;'>Idproof / IdDetails</th><th style='width:10%;' >Camera</th><th style='width:10%;' >Verify Visitor</th></tr></thead>");


                int count = 1;
                SB.AppendLine("<tbody><tr>");
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    SB.Append("<tr><td style='font-size:12px;'>" + count + "</td><td style='font-size:12px;'>" + Convert.ToString(dr["BoardingID"]) + " </td><td style='font-size:12px;'>" + Convert.ToString(dr["NameOfVisitor"]) + "  </td>");
                    SB.Append("<td style='font-size:12px;'>" + Convert.ToString(dr["IdproofIdDetails"]) + "</td><td style='font-size:12px;'>" + Convert.ToString(dr["NoOfCamera"]) + " </td><td></td></tr>");
                    count += 1;
                }
                SB.Append("</tbody></table></div>");
                SB.Append("<label><h4>Boarding Point :</h4></label> " + DS.Tables[0].Rows[0]["BoardingPointName"] + "");
                SB.Append("<div class='divider'></div><p></p></div></div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return SB.ToString();
        }







        public ActionResult BookTicket()
        {

            // book ticket between 10.00 to 23.59

            //DateTime CurrentDT = DateTime.Now;
            // DateTime DT = DateTime.Now.Date;
            //string OpenDT = OpenDT +"10:10:00";

            //if (CurrentDT < EndDate)


            //"09-08-2016 20:17:41";

            //if (CurrentDT < EndDate)


            //========================================



            CS_Ticket cst = new CS_Ticket();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                List<CS_Ticket> feesDetails = new List<CS_Ticket>();
                DataTable dtf = new DataTable();

                cst.UserID = Convert.ToInt64(Session["UserId"].ToString());
                dtf = cst.Select_BookedTicket();

                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new CS_Ticket()
                    {
                        //RowID = Int64.Parse(dr["Rowid"].ToString()),
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        TransactionId = dr["EmitraTransactionID"].ToString(),
                        Date = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString())
                    });
                }

                DataTable dt = new DataTable();
                dt = cst.GetVehicleType();
                ViewBag.Vehiclecat = dt;
                foreach (System.Data.DataRow dr in ViewBag.Vehiclecat.Rows)
                {
                    vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
                }
                ViewBag.Vehiclecat = vehicleCategory;

                DataTable dtPlace = new DataTable();
                if (Session["IsDepartmentalKioskUser"].ToString() == "True")
                {

                    dtPlace = cst.Select_PlaceDeptUser();
                }
                else
                {

                    dtPlace = cst.Select_Place();
                }
                foreach (System.Data.DataRow dr in dtPlace.Rows)
                {
                    lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }
                ViewBag.Place = lstPlace;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(ticketList);
        }

        /// <summary>
        /// Function to bind district on the basis of wildlife category
        /// </summary>
        /// <param name="Category"></param>
        /// <returns>Json result for District</returns>
        public JsonResult DistrictbyCategory(string Category)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> District = new List<SelectListItem>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
                DataTable dtd = new DataTable();
                cst.Category = Category;
                dtd = cst.Get_CategorywiseDistrict();
                ViewBag.district = dtd;
                foreach (System.Data.DataRow dr in ViewBag.district.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = District;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(District, "Value", "Text"));
        }

        /// <summary>
        /// function to bind places on the basic of districts
        /// </summary>
        /// <param name="districtID"></param>
        /// <returns>Json resultfor places</returns>
        [HttpPost]
        public JsonResult ZoneByPlace(int PlaceId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> Zone = new List<SelectListItem>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
                DataTable dt = new DataTable();
                cst.PlaceID = PlaceId;
                cst.ZoneId = 0;
                dt = cst.Select_Zone_ByPlace();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ViewBag.fname = dt;
                        foreach (DataRow dr in dt.Rows)
                        {
                            Zone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }
                    else
                    {
                        Zone.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    }
                }
                else
                {
                    Zone.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Zone, "Value", "Text"));


        }




        //Note: Code Updated with Ref. to bug ID 155,171
        /// <summary>
        /// Check avaliablility of seats
        /// </summary>
        /// <param name="PlaceID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VistDate(int PlaceID, int Zone)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            CS_Ticket cst1 = null;
            try
            {
                CS_Ticket cst = new CS_Ticket();

                DataSet ds = new DataSet();
                cst.PlaceID = Convert.ToInt64(PlaceID);
                cst.ZoneId = Convert.ToInt64(Zone);
                ds = cst.GetVisitDate();
                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cst1 = new CS_Ticket();
                        cst1.DurationFrom = ds.Tables[0].Rows[0]["DurationFromDate"].ToString();
                        cst1.DurationTo = ds.Tables[0].Rows[0]["DurationToDate"].ToString();
                    }
                    else
                    {
                        cst1 = new CS_Ticket();
                        cst1.DurationFrom = "NF";
                        cst1.DurationTo = "NF";
                    }
                }
                else
                {
                    cst1 = new CS_Ticket();
                    cst1.DurationFrom = "NF";
                    cst1.DurationTo = "NF";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = cst1.DurationFrom, list2 = cst1.DurationTo }, JsonRequestBehavior.AllowGet);


        }


        //Note: Code Updated with Ref. to bug ID 155,171
        /// <summary>
        /// Check avaliablility of seats
        /// </summary>
        /// <param name="PlaceID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckSafariAccomoAvailability(int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            CS_Ticket cst1 = null;
            List<CS_Ticket> safariAccomo = new List<CS_Ticket>();
            List<SelectListItem> lstZone = new List<SelectListItem>();
            try
            {
                CS_Ticket cst = new CS_Ticket();

                DataSet ds = new DataSet();
                cst.PlaceID = Convert.ToInt64(PlaceID);
                ds = cst.chkSafariAccomo();
                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cst1 = new CS_Ticket();

                        cst1.isSafari = ds.Tables[0].Rows[0]["IsSafari"].ToString();
                        cst1.isAccomo = ds.Tables[0].Rows[0]["IsAccommodation"].ToString();
                    }
                    else
                    {

                        cst1.isSafari = "";
                        cst1.isAccomo = "";
                    }
                }
                else
                {
                    cst1.isSafari = "";
                    cst1.isAccomo = "";

                }
                if (ds.Tables[1] != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in ds.Tables[1].Rows)
                        {
                            lstZone.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["zoneID"].ToString() });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { list1 = cst1.isSafari, list2 = cst1.isAccomo, list3 = lstZone }, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// function to check ticket availability on the basis of given param values
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="shifttime"></param>
        /// <returns> json result return available ticket </returns>
        [HttpPost]
        public JsonResult CheckTicketAvailability(int placeID, string arrivaldate, string shifttime, int Zone)
        {
            string st = string.Empty;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                CS_Ticket cst = new CS_Ticket();
                cst.PlaceID = placeID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                cst.ShiftTime = shifttime;
                cst.ZoneId = Zone;
                st = cst.CheckTicketAvailability();
                ViewBag.fname = st;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(st);

        }
        //Note: Code Updated with Ref. to bug ID 150
        /// <summary>
        /// function to bind Shift time on given param values
        /// </summary>
        /// <param name="districtID"></param>
        /// <param name="placeID"></param>
        /// <returns> Return Shift</returns>
        [HttpPost]
        public JsonResult BindShiftByDistrictPlace(int placeID, int Zone)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<TicketBooking> fees = new List<TicketBooking>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
                DataTable dt = new DataTable();
                // cst.DistrictID = Convert.ToString(districtID);
                cst.PlaceID = Convert.ToInt64(placeID);
                cst.ZoneId = Convert.ToInt64(Zone);
                dt = cst.Select_Shift_ByDistrict_Places();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new TicketBooking()
                        {
                            isMorning = dr["isMorning"].ToString(),
                            isEvening = dr["isEvening"].ToString(),
                            isFullDay = dr["isFullDay"].ToString(),
                        });
                    }
                }
                else
                {

                    fees.Add(new TicketBooking()
                    {
                        isMorning = "",
                        isEvening = "",
                        isFullDay = "",
                    });

                }

                DataTable dta = new DataTable();
                cst.PlaceID = Convert.ToInt64(placeID);
                dta = cst.GetAccomodationType();
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
            return Json(new { list1 = fees, list2 = Accomodation, JsonRequestBehavior.AllowGet });


        }

        /// <summary>
        /// function to get all types of fees for ticket booking on the basis of given param values
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="districtID"></param>
        /// <param name="nationality"></param>
        /// <param name="memberType"></param>
        /// <returns>Json result for fees</returns>
        [HttpPost]
        public JsonResult SelectFee(Int64 placeID, string districtID, string nationality, string memberType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<CS_Ticket> fees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.PlaceID = placeID;
                cst.DistrictID = districtID;
                cst.Nationality = nationality;
                cst.MemberType = memberType;
                dt = cst.SelectMemberFees();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new CS_Ticket()
                        {
                            FeePerMember = Convert.ToDecimal(dr["MemberFee"].ToString()),
                            CameraFee = Convert.ToDecimal(dr["CameraFees"].ToString()),
                        });
                    }
                }
                else
                {
                    fees.Add(new CS_Ticket()
                    {
                        FeePerMember = Convert.ToDecimal(0),
                        CameraFee = Convert.ToDecimal(0),
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
        /// Function to create xml file of all memeber details for whom ticket is booking
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Return xml row for members details</returns>
        [HttpPost]
        public JsonResult memberData(CS_Ticket member)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (member != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["MemberInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("CrewMember");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["MemberInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    }

                    XmlElement Crew_TYPE = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement Name = xmldoc.CreateElement("Name");
                    XmlElement Gender = xmldoc.CreateElement("Gender");
                    XmlElement IDType = xmldoc.CreateElement("IDType");
                    XmlElement IDNo = xmldoc.CreateElement("IDNo");
                    XmlElement Nationality = xmldoc.CreateElement("Nationality");
                    XmlElement MemberType = xmldoc.CreateElement("MemberType");
                    XmlElement FeePerMember = xmldoc.CreateElement("FeePerMember");
                    XmlElement TotalCamera = xmldoc.CreateElement("TotalCamera");
                    XmlElement CameraFee = xmldoc.CreateElement("CameraFee");
                    XmlElement UploadId = xmldoc.CreateElement("UploadId");


                    Name.InnerText = member.Name;
                    Gender.InnerText = member.Gender;
                    IDType.InnerText = member.IDType;
                    IDNo.InnerText = member.IDNo;
                    Nationality.InnerText = member.Nationality;
                    MemberType.InnerText = member.MemberType;
                    FeePerMember.InnerText = Convert.ToString(member.FeePerMember);
                    TotalCamera.InnerText = Convert.ToString(member.TotalCamera);
                    CameraFee.InnerText = Convert.ToString(member.CameraFee);
                    UploadId.InnerText = member.UploadId;


                    Crew_TYPE.AppendChild(Name);
                    Crew_TYPE.AppendChild(Gender);
                    Crew_TYPE.AppendChild(IDType);
                    Crew_TYPE.AppendChild(IDNo);
                    Crew_TYPE.AppendChild(Nationality);
                    Crew_TYPE.AppendChild(MemberType);
                    Crew_TYPE.AppendChild(FeePerMember);
                    Crew_TYPE.AppendChild(TotalCamera);
                    Crew_TYPE.AppendChild(CameraFee);
                    Crew_TYPE.AppendChild(UploadId);


                    xmldoc.DocumentElement.AppendChild(Crew_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(member, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// session check for sessio
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SessionMemberData()
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            CS_Ticket member = new CS_Ticket();
            List<CS_Ticket> lstMember = new List<CS_Ticket>();
            DataSet dsm = new DataSet();
            if (Session["MemberInfo"] != null)
            {
                dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                foreach (System.Data.DataRow dr in dsm.Tables[0].Rows)
                {
                    lstMember.Add(new CS_Ticket
                    {
                        Name = dr["Name"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        IDType = dr["IDType"].ToString(),
                        IDNo = dr["IDNo"].ToString(),
                        Nationality = dr["Nationality"].ToString(),
                        MemberType = dr["MemberType"].ToString(),
                        FeePerMember = Convert.ToInt64(dr["FeePerMember"].ToString()),
                        TotalCamera = Convert.ToInt32(dr["TotalCamera"].ToString()),
                        CameraFee = Convert.ToInt64(dr["CameraFee"].ToString()),
                        UploadId = dr["UploadId"].ToString(),
                    });
                }
            }
            else
            {
                lstMember = null;
            }
            return Json(lstMember, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete members data entry
        /// </summary>
        /// <param name="member"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deleteMemberData(CS_Ticket member, string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Session["MemberInfo"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["IDNo"].ToString() == Id)
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                        Session["MemberInfo"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(member, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to select vehicle by there category e.g vehicle, boat, electravan
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="placeID"></param>
        /// <returns>Json result as vehicle list</returns>
        [HttpPost]
        public JsonResult vehicleByCategory(int vehicleCatID, Int64 placeID, int Zone)
        {
            CS_Ticket cst = new CS_Ticket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                cst.PlaceID = placeID;
                cst.ZoneId = Zone;
                dt = cst.Select_vehicle(Convert.ToInt64(vehicleCatID));
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

        /// <summary>
        /// Function return vehicle fees on the basis of give param values
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="vehicleID"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="placeID"></param>
        /// <returns>Json result for vehicle fees</returns>
        [HttpPost]
        public JsonResult SelectVehicleFee(Int64 vehicleCatID, Int64 vehicleID, string arrivaldate, Int64 placeID, Int64 Zone, int shift)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<CS_Ticket> vehiclefees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.VehicleCatID = vehicleCatID;
                cst.VehicleID = vehicleID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null); ;
                cst.PlaceID = placeID;
                cst.ZoneId = Zone;
                cst.ShiftType = shift;
                dt = cst.Select_vehicle_Fees_Seat();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        vehiclefees.Add(new CS_Ticket()
                        {
                            AvailableSeat = Convert.ToInt32(dr["AvailableSeat"].ToString()),
                            FeePerVehicle = Convert.ToDecimal(dr["TotalFee"].ToString()),
                        });
                    }
                }
                else
                {
                    vehiclefees.Add(new CS_Ticket()
                    {
                        AvailableSeat = Convert.ToInt32(0),
                        FeePerVehicle = Convert.ToDecimal(0),
                    });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(vehiclefees);

        }

        /// <summary>
        /// Function to get accomodation charges on the basis of room type
        /// </summary>
        /// <param name="AccomoID"></param>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <returns> json result return Accomodation charges.</returns>
        [HttpPost]
        public JsonResult SelectAccomoFee(Int64 AccomoID, Int64 placeID, string arrivaldate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<CS_Ticket> vehiclefees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.AccomoID = AccomoID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null); ;
                cst.PlaceID = placeID;
                dt = cst.Select_Accomo_Fees_Availability();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        vehiclefees.Add(new CS_Ticket()
                        {
                            RoomAvailability = Convert.ToInt32(dr["AvailableRoom"].ToString()),
                            RoomCharge = Convert.ToDecimal(dr["RatePerRoom"].ToString()),
                        });
                    }
                }
                else
                {
                    vehiclefees.Add(new CS_Ticket()
                    {
                        AvailableSeat = Convert.ToInt32(0),
                        FeePerVehicle = Convert.ToDecimal(0),
                    });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(vehiclefees);

        }

        /// <summary>
        /// Function to create xml file for safari ticket details
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult safariData(CS_Ticket member)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (member != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["safariInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("SafariVehicleInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["safariInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    }

                    XmlElement vehicleInfo = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement vahicleCatID = xmldoc.CreateElement("vahicleCatID");
                    XmlElement vahicleCat = xmldoc.CreateElement("vahicleCat");
                    XmlElement vID = xmldoc.CreateElement("vID");
                    XmlElement vName = xmldoc.CreateElement("vName");
                    XmlElement SeatForBooking = xmldoc.CreateElement("SeatForBooking");
                    XmlElement FeePerVehicle = xmldoc.CreateElement("FeePerVehicle");
                    XmlElement Totalfee = xmldoc.CreateElement("Totalfee");

                    vahicleCatID.InnerText = Convert.ToString(member.VehicleCatID);
                    vahicleCat.InnerText = member.VehicleCategory;
                    vID.InnerText = Convert.ToString(member.VehicleID);
                    vName.InnerText = member.VehicleName;
                    SeatForBooking.InnerText = Convert.ToString(member.SeatForBooking);
                    FeePerVehicle.InnerText = Convert.ToString(member.FeePerVehicle);
                    Totalfee.InnerText = Convert.ToString(member.VehicleFeeTotal);

                    vehicleInfo.AppendChild(vahicleCatID);
                    vehicleInfo.AppendChild(vahicleCat);
                    vehicleInfo.AppendChild(vID);
                    vehicleInfo.AppendChild(vName);
                    vehicleInfo.AppendChild(SeatForBooking);
                    vehicleInfo.AppendChild(FeePerVehicle);
                    vehicleInfo.AppendChild(Totalfee);

                    xmldoc.DocumentElement.AppendChild(vehicleInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
            }

            return Json(member, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deletesafariData(CS_Ticket member, string Id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Session["safariInfo"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i + 1 == Convert.ToInt32(Id))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                        Session["safariInfo"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(member, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to save ticket details to database with all the values
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="Command"></param>
        /// <param name="form"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public ActionResult SubmitticketDetails(CS_Ticket cs, string Command, FormCollection form, string Message)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Command == "Submit")
                {
                    if (Session["UserId"] != null)
                    {
                        if (Session["KioskUserId"] != null)
                        {
                            cs.KioskUserId = Session["KioskUserId"].ToString();
                        }
                        else
                        {
                            cs.KioskUserId = "0";
                        }
                        cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                        cs.RequestID = DateTime.Now.Ticks.ToString();
                        Session["RequestId"] = cs.RequestID;
                        // cs.Category = form["Category"].ToString();
                        // cs.DistrictID = form["ddl_Districts"].ToString();
                        cs.PlaceID = Convert.ToInt64(form["ddl_place"].ToString());
                        if (form["ddl_Zone"].ToString() != "" && form["ddl_Zone"].ToString() != null)
                        {
                            cs.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                        }
                        else
                        {
                            cs.ZoneId = 0;
                        }
                        if (form["hdn_vehicleID"].ToString() != "")
                        {
                            cs.VehicleID = Convert.ToInt64(form["hdn_vehicleID"].ToString());
                        }
                        else
                        {
                            TempData["maxdate"] = "Please select vehicle.";
                            return RedirectToAction("BookTicket", "TicketBooking");
                        }
                        cs.ShiftTime = form["ddl_Shift"].ToString();
                        cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                        if (form["TotalMember"].ToString() != "")
                        {
                            cs.TotalMember = Convert.ToInt32(form["TotalMember"].ToString());
                        }
                        else
                        {
                            cs.TotalMember = 0;
                        }
                        if (form["ddl_Accomo"].ToString() != "")
                        {
                            cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                        }
                        else
                        { cs.AccomoID = 0; }

                        if (form["TotalRoom"].ToString() != "")
                        {
                            cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                        }
                        else
                        { cs.TotalRoom = 0; }
                        if (form["RoomCharge"].ToString() != "")
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

                        DateTime dt = DateTime.Now;
                        dt = dt.AddDays(89);
                        //dt = DateTime.ParseExact(dt.AddDays("90").ToString(), "dd/MM/yyyy", null);
                        if (cs.ArrivalDate > dt)
                        {
                            TempData["maxdate"] = "Date should not be more than 90 days from today date";
                            return RedirectToAction("BookTicket", "TicketBooking");
                        }
                        DataSet dsm = new DataSet();
                        if (Session["MemberInfo"] != null)
                        {
                            dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                            if (dsm.Tables.Count > 0)
                            {
                                if (dsm.Tables[0].Rows.Count > 0)
                                {
                                    if (dsm.Tables[0].Rows.Count > 6)
                                    {
                                        if (Session["MemberInfo"] != null)
                                        {
                                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                                            {
                                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                                Session["MemberInfo"] = null;
                                            }
                                        }
                                        TempData["maxdate"] = "Member should not be more than 6";
                                        return RedirectToAction("BookTicket", "TicketBooking");
                                    }

                                }

                            }

                        }
                        else
                        {
                            dsm.Clear();
                            DataTable objDt2 = new DataTable("Table");
                            objDt2.Columns.Add("Name", typeof(String));
                            objDt2.Columns.Add("Gender", typeof(String));
                            objDt2.Columns.Add("IDType", typeof(String));
                            objDt2.Columns.Add("IDNo", typeof(String));
                            objDt2.Columns.Add("Nationality", typeof(String));
                            objDt2.Columns.Add("MemberType", typeof(String));
                            objDt2.Columns.Add("FeePerMember", typeof(String));
                            objDt2.Columns.Add("TotalCamera", typeof(String));
                            objDt2.Columns.Add("CameraFee", typeof(String));
                            objDt2.Columns.Add("UploadId", typeof(String));
                            objDt2.AcceptChanges();
                            dsm.Tables.Add(objDt2);
                            objDt2.Clear();
                            objDt2.AcceptChanges();
                        }
                        DataSet dsv = new DataSet();
                        if (Session["safariInfo"] != null)
                        {

                            dsv.ReadXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));

                        }
                        else
                        {
                            dsv.Clear();
                            DataTable objDt2 = new DataTable("Table");
                            objDt2.Columns.Add("vahicleCatID", typeof(String));
                            objDt2.Columns.Add("vahicleCat", typeof(String));
                            objDt2.Columns.Add("vID", typeof(String));
                            objDt2.Columns.Add("vName", typeof(String));
                            objDt2.Columns.Add("SeatForBooking", typeof(String));
                            objDt2.Columns.Add("FeePerVehicle", typeof(String));
                            objDt2.Columns.Add("Totalfee", typeof(String));
                            objDt2.AcceptChanges();
                            dsv.Tables.Add(objDt2);
                            objDt2.AcceptChanges();
                        }
                        DataTable dts = new DataTable();
                        if (dsm.Tables[0].Rows.Count > 0)
                        {

                            CS_Ticket cst = new CS_Ticket();
                            string st = string.Empty;
                            cst.PlaceID = Convert.ToInt64(form["ddl_place"].ToString());
                            cst.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                            cst.ShiftTime = form["ddl_Shift"].ToString();
                            cst.ZoneId = Convert.ToInt64(form["ddl_Zone"].ToString());
                            st = cst.CheckTicketAvailability();

                            if (Convert.ToInt32(st) >= Convert.ToInt32(form["TotalMember"].ToString()))
                            {
                                dts = cs.Submit_TicketDetails(dsm.Tables[0], dsv.Tables[0]);

                            }
                            else
                            {
                                return RedirectToAction("BookTicket", "TicketBooking");
                            }


                            //dts = cs.Submit_TicketDetails(dsm.Tables[0], dsv.Tables[0]);
                        }
                        if (Session["MemberInfo"] != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                Session["MemberInfo"] = null;
                            }
                        }
                        if (Session["safariInfo"] != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                Session["safariInfo"] = null;
                            }
                        }
                        if (dts.Rows.Count > 0)
                        {
                            decimal finalAmnt = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(dts.Rows[0]["TotalAmount"].ToString()));
                            Session["totalprice"] = finalAmnt;
                        }

                        ViewData.Model = dts.AsEnumerable();
                        return View("TicketPayment");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("Login", "Login");
        }
        //Note: Code Updated with Ref. to bug ID 77
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
                //EM33172142@5488


                Payment pay = new Payment();
                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "TicketBooking/Payment", Session["User"].ToString(), "", "");
                Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            if (Session["RequestId"] != null)
            {
                try
                {
                    //TicketBooking cs = new TicketBooking();
                    int status1 = 0;
                    CS_Ticket cs = new CS_Ticket();
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
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion


                    string response = Request.QueryString["trnParams"].ToString();
                    //string response = "yXBdEkS6dSGTCohZ8VmsDuZkk9apDX2AJkACzuSmD6gc601RZn4UOpK823hjVERWp0SxTl3EiVeqJ/PBwCCcRrOTU1CJrl1VLe5W4zZXUq6aEkCERKPRhxQu9jGYORuckUzNnE1 l6flcwuTPVXDh6Ra6hFUfj43OQ48bwDba8v2RnhgJasajFfnNThsUelcDjbrJTR CT3yeIOdMM/z5Q9xZc3bGMqkZg4u0n3hoePCMH7rFETTQIspUsQsr9OL3Cf25W 9pz43p6skE8VWuKyW1HdTdKbHurgJkfurNjnsnOzOHDWAWfEQG1KRXhSr4cQGsegHV0zuuG/5q1t3gwaWq177ViRxU69x sJTSx9rGNNnN523ingz4R6DkPNaj7rpAAqzDHQQnByajyAWCLeHpEnurFI9 TOUk04tFHoR/JTzB92DGwfQZQ7PBZ0mrqyNfb yzVo2lor3vzwbdaAV90Iuk/QbxZJ4yoU5lYw=";

                    //response = "yXBdEkS6dSGTCohZ8VmsDq41eAzocGYsxapscFxnZAC6CVC5Epazm35vzBQybDx85235abr9p6y5TS0bPGNHFoVaRNYz9KLTt2AczL1+aTxFYCbS9irCPOVdPh6wDfeLyU5BTf6gyKp/fPZfS2UUAF4tEgsRbNCGjeBm1Q4DmkSu7C2W4CTo5HY6v6bh0fnCsc5hnqQd9dHkP4SEl++4wWJzFJcCdvJE2ENkvfsHA/xXSFuApzSQa4ZlxeBGofD66mu9trYJOYCJia/fTaBe7Wk+QBWwgXGYXmF7XaDJi6Wf3rv13lDlOG8+nWkSErFFXduFRdMhOmOOi173m5UnOGuB6+hWvsHTK0VvI95Le15bo7/LZvCO8AKhfsvdBjHU5HyxP3RwGAXQIkM0/VJMqoN9NoRzb0hdeCt+FloDgqvLvDc4gsTNn6bvVbJMJfnWFSNkBqhaF3687u4XCYoasUEpqt4vnbDhgeZHcQLYivvyxJBXnRxY9g==";
                    string ResponseResult = pay.ProcesTranscationresponce(response);

                    //string status = Request.QueryString["STATUS"].ToString();
                    //  <RESPONSE 
                    //  RCPT_NO='1699099911226398' 
                    //  TRN_TIME='2016-08-10 10:46:05.0' 
                    //  REQUEST_ID='636064226721636615' 
                    //  AMOUNT='244.0' 
                    //  OTHER_PARAM_1='SHOYAB ANSARI' 
                    //  OTHER_PARAM_2='null' 
                    //  OTHER_PARAM_3='null' 
                    //  STATUS='SUCCESS' 
                    //  BANK_NAME='HDFC' 
                    //  BANK_ACC_NO='NO ACCOUNT FOUND' 
                    //  BANK_BID_NO='162233833329-000000004859915'
                    // ></RESPONSE>|5446A4A066B8111459676F4C2F223A57894850CD45CD607110E6AA7C34E49A57

                    #region Response decryption
                    string str1, str2;
                    str1 = ResponseResult.Replace("<RESPONSE ", "");
                    str2 = str1.Replace("></RESPONSE>", " ");
                    string[] Responsearr = str2.Split(new string[] { "' " }, StringSplitOptions.RemoveEmptyEntries);
                    string checkFail = "STATUS='FAILED";
                    string checkSucess = "STATUS='SUCCESS";
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
                    string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 1);
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
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 1);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 1);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 1);
                        cs.TransactionId = "0";
                        cs.RequestID = finalreqid;
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 1);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 1);

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";//transtime[1];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            cs.Trn_Status_Code = 0;
                            cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (finalstatus1 == "SUCCESS")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[2].Split('=');
                        string[] reqamt = Responsearr[3].Split('=');
                        string[] username = Responsearr[4].Split('=');
                        string[] status = Responsearr[7].Split('=');
                        string[] bank = Responsearr[8].Split('=');
                        string[] bankbidno = Responsearr[10].Split('=');

                        DataRow dtrow = dt.NewRow();
                        string rowstatus = status[1].ToString();
                        int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 1);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 1);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 1);
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 1);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 1);
                        string rawbank = bank[1].ToString();
                        int banklen = Convert.ToInt32(rawbank.Length);
                        string finalbank = rawbank.ToString().Substring(1, banklen - 1);
                        //string rawbankbid = bankbidno[1].ToString();
                        //int bankidlen = Convert.ToInt32(rawbankbid.Length);
                        //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                        cs.RequestID = finalreqid;
                        cs.TransactionId = finalemitraid;
                        dtrow["TRANSACTION TIME"] = transtime[1]; //+ Responsearr[2];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;
                        dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            if (Convert.ToString(Session["RequestId"]).Equals(finalreqid) && (Session["totalprice"] != null && Convert.ToDecimal(Session["totalprice"]) == Convert.ToDecimal(finalamount)))
                            {
                                cs.Trn_Status_Code = 1;
                                status1 = 1;
                                cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                            }
                            else // Added to save mismatch in payment
                            {
                                cs.Trn_Status_Code = 0;
                                status1 = 0;
                                cs.UpdateTransactionStatus("3", Convert.ToDouble(finalamount));
                            }
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                    #region old trans code

                    //string response = Request.QueryString["trnParams"].ToString();
                    ////string response = "yXBdEkS6dSGTCohZ8VmsDuZkk9apDX2AJkACzuSmD6gc601RZn4UOpK823hjVERWp0SxTl3EiVeqJ/PBwCCcRrOTU1CJrl1VLe5W4zZXUq6aEkCERKPRhxQu9jGYORuckUzNnE1 l6flcwuTPVXDh6Ra6hFUfj43OQ48bwDba8v2RnhgJasajFfnNThsUelcDjbrJTR CT3yeIOdMM/z5Q9xZc3bGMqkZg4u0n3hoePCMH7rFETTQIspUsQsr9OL3Cf25W 9pz43p6skE8VWuKyW1HdTdKbHurgJkfurNjnsnOzOHDWAWfEQG1KRXhSr4cQGsegHV0zuuG/5q1t3gwaWq177ViRxU69x sJTSx9rGNNnN523ingz4R6DkPNaj7rpAAqzDHQQnByajyAWCLeHpEnurFI9 TOUk04tFHoR/JTzB92DGwfQZQ7PBZ0mrqyNfb yzVo2lor3vzwbdaAV90Iuk/QbxZJ4yoU5lYw=";
                    //string ResponseResult = pay.ProcesTranscationresponce(response);
                    ////string status = Request.QueryString["STATUS"].ToString();
                    //#region Response decryption
                    //string str1, str2;
                    //str1 = ResponseResult.Replace("<RESPONSE ", "");
                    //str2 = str1.Replace("></RESPONSE>", "");
                    //string[] Responsearr = str2.Split(' ');
                    //string checkFail = "STATUS='FAILED'";
                    //string checkSucess = "STATUS='SUCCESS'";
                    //string rowstatus1 = "";
                    //foreach (var item in Responsearr)
                    //{
                    //    if (item.Equals(checkFail))
                    //    {
                    //        string[] status2 = item.Split('=');
                    //        rowstatus1 = status2[1].ToString();
                    //    }
                    //    if (item.Equals(checkSucess))
                    //    {
                    //        string[] status2 = item.Split('=');
                    //        rowstatus1 = status2[1].ToString();
                    //    }
                    //}
                    //int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                    //string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 2);
                    //#endregion
                    //#region Response Status
                    //if (finalstatus1 == "FAILED")
                    //{
                    //    string[] emitratransid = Responsearr[0].Split('=');
                    //    string[] transtime = Responsearr[1].Split('=');
                    //    string[] reqid = Responsearr[2].Split('=');
                    //    string[] reqamt = Responsearr[3].Split('=');
                    //    string[] username = Responsearr[4].Split('=');
                    //    string[] status = Responsearr[7].Split('=');


                    //    DataRow dtrow = dt.NewRow();
                    //    string rowstatus = status[1].ToString();
                    //    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    //    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    //    string rowreqid = reqid[1].ToString();
                    //    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    //    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    //    string rawemitra = emitratransid[1].ToString();
                    //    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    //    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    //    cs.TransactionId = "0";
                    //    cs.RequestID = finalreqid;
                    //    string rawtransamount = reqamt[1].ToString();
                    //    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    //    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    //    string rawusername = username[1].ToString();
                    //    int usernamelen = Convert.ToInt32(rawusername.Length);
                    //    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);

                    //    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    //    dtrow["REQUEST ID"] = finalreqid;
                    //    dtrow["EMITRA TRANSACTION ID"] = "";
                    //    dtrow["TRANSACTION TIME"] = "";//transtime[1];
                    //    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    //    dtrow["USER NAME"] = finalUserName;

                    //    if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                    //    {
                    //        cs.Trn_Status_Code = 0;
                    //    }
                    //    dt.Rows.Add(dtrow);
                    //}
                    //else if (finalstatus1 == "SUCCESS")
                    //{
                    //    string[] emitratransid = Responsearr[0].Split('=');
                    //    string[] transtime = Responsearr[1].Split('=');
                    //    string[] reqid = Responsearr[3].Split('=');
                    //    string[] reqamt = Responsearr[4].Split('=');
                    //    string[] username = Responsearr[5].Split('=');
                    //    string[] status = Responsearr[8].Split('=');
                    //    string[] bank = Responsearr[9].Split('=');
                    //    string[] bankbidno = Responsearr[13].Split('=');

                    //    DataRow dtrow = dt.NewRow();
                    //    string rowstatus = status[1].ToString();
                    //    int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                    //    string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                    //    string rowreqid = reqid[1].ToString();
                    //    int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                    //    string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                    //    string rawemitra = emitratransid[1].ToString();
                    //    int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                    //    string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                    //    cs.TransactionId = finalreqid;
                    //    string rawtransamount = reqamt[1].ToString();
                    //    int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                    //    string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                    //    string rawusername = username[1].ToString();
                    //    int usernamelen = Convert.ToInt32(rawusername.Length);
                    //    string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
                    //    string rawbank = bank[1].ToString();
                    //    int banklen = Convert.ToInt32(rawbank.Length);
                    //    string finalbank = rawbank.ToString().Substring(1, banklen - 2);
                    //    //string rawbankbid = bankbidno[1].ToString();
                    //    //int bankidlen = Convert.ToInt32(rawbankbid.Length);
                    //    //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
                    //    dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                    //    dtrow["REQUEST ID"] = finalreqid;
                    //    dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                    //    cs.TransactionId = finalemitraid;
                    //    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    //    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    //    dtrow["USER NAME"] = finalUserName;
                    //    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    //    //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                    //    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    //    {
                    //        cs.Trn_Status_Code = 1;
                    //        status1 = 1;
                    //    }
                    //    dt.Rows.Add(dtrow);
                    //}
                    //#endregion
                    #endregion



                    ViewData.Model = dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                return View("TransactionStatus");
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
                CS_Ticket cs = new CS_Ticket();
                cs.TicketID = ticketid;
                ds = cs.Select_TicketData_For_Print();


                DT1 = ds.Tables[0];
                DT2 = ds.Tables[1];
                DT3 = ds.Tables[2];
                //sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><div class='col-xs-12 col-sm-4 centr'><img src='../images/risl-logo-small.png' alt='RISL' >   </div>		<div class='col-xs-12 col-sm-4'> <span class='pull-right pdate' align= 'center'>Department of Forest, <br>Goverment of<br> Rajasthan</span> </div><img src='../images/e-mitra_logo.png' alt='E-Mitra' >  </div>  <div class='divider'></div></div>");
                sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><img src='../../images/risl-logo-small.png' alt='RISL' ></div><div class='col-xs-12 col-sm-4 centr'>Department of Forest, <br>Government of<br> Rajasthan</span></div><div class='col-xs-12 col-sm-4' style='padding: 0'> <span class='pull-right pdate'><img src='../../images/e-mitra_logo.png' alt='E-Mitra' > </div>  <div class='divider'></div></div>");

                if (ds != null)
                {

                    // decimal finalAmnt = Common.CalculateFinalFeeForOnlineTicketing(Convert.ToDecimal(DT1.Rows[0]["AmountWithServiceCharges"].ToString()));
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr> <td col-lg-12 colspan='5' style='text-align:center'>BOOKING CONFIRMATION SLIP</td></tr>");
                    sb.Append("<tr><td col-lg-3><b>Reserve : </b></td><td col-lg-3>" + DT1.Rows[0]["PlaceName"] + "</td><td col-lg-3><b>Booking No:</b> </td><td col-lg-3>" + DT1.Rows[0]["RequestID"] + " </td></tr>");

                    sb.Append("<tr><td col-lg-3><b>Date/Time of Booking : </b></td><td col-lg-3>" + DT1.Rows[0]["EnteredOn"] + "</td><td col-lg-3><b>Date of Visit : </b> </td><td col-lg-3>" + DT1.Rows[0]["DateOfArrival"] + " </td></tr>");
                    //sb.Append("<tr><td col-lg-3><b>Reserve : </b></td><td col-lg-3>" + DT1.Rows[0]["PlaceName"] + "</td><td col-lg-3><b>Booking No:</b> </td><td col-lg-3>" + DT1.Rows[0]["RequestID"] + " </td></tr>");


                    sb.Append("<tr><td col-lg-3><b>Booked Seats : </b></td><td col-lg-3 colspan='3'>" + DT1.Rows[0]["NoofTicket"] + "</td> </tr>");

                    //sb.Append("<tr><td col-lg-3 ><b>Date of Visit : </b></td><td col-lg-3 colspan='3'>" + DT1.Rows[0]["DateOfArrival"] + "</td> </tr>");
                    //sb.Append("<tr><td col-lg-3><b>Booked Seats : </b></td><td col-lg-3>" + DT1.Rows[0]["NoofTicket"] + "</td><td col-lg-3><b>Total Amount Incl. of Service Charges: Rs. </b> </td><td col-lg-3>" + finalAmnt + " </td></tr>");


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
                    sb.Append("<tr><td col-lg-3 ><b>Total Amount Incl. of Service Charges: Rs.</b></td><td col-lg-3 colspan='5'>" + Convert.ToString(DT1.Rows[0]["AmountWithServiceCharges"]) + "</td> </tr>");
                    sb.Append("<tr><td col-lg-3 ><b>Boarding Point : </b></td><td col-lg-3 colspan='5'>" + DT1.Rows[0]["Boarding_Point"] + "</td> </tr>");
                    sb.Append("<tr><td col-lg-3><b>Contact Person : </b></td><td col-lg-3>" + DT1.Rows[0]["contactperson"] + "</td><td col-lg-3><b>Phone No : </b></td><td col-lg-3>" + DT1.Rows[0]["PhoneNo"] + "</td><td col-lg-3><b>Address : </b></td><td col-lg-3>" + DT1.Rows[0]["Address"] + "</td> </tr>");
                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3  colspan='2' >Terms and conditions for Visitors :</td> </tr>");
                    sb.Append("<tr><td col-lg-3 >1.</td><td col-lg-3>The visitor must reach to the forest booking center to collect the boarding pass,at least 45 minutes prior to the departure time. </td> </tr>");
                    sb.Append("<tr><td col-lg-3 >2.</td><td col-lg-3>In case, same Id proof will not be produced at the time of seeking entrance in the park then the ticket shall be deemed cancelled or fake.</td> </tr>");
                    sb.Append("<tr><td col-lg-3 >3.</td><td col-lg-3>Aforesaid total charges include tourist entry fee,Reserve development fee,Vehicle entry fee,Reserve development fee and online Booking charges. </td> </tr>");
                    sb.Append("<tr><td col-lg-3 >4.</td><td col-lg-3>Please take two copies of this voucher and bring with your self at the time of boarding. </td> </tr>");
                    sb.Append("<tr><td col-lg-3 >5.</td><td col-lg-3>Every visitor has to pay vehicle rent and Guide fee at the time of collecting boarding pass additionally.(If applicable) </td> </tr>");
                    sb.Append("<tr><td col-lg-3 >6.</td><td col-lg-3>Seats remaining vacant due to non-turn up of any visitors can filled by the park management at the booking window. </td> </tr>");
                    sb.Append("<tr><td col-lg-3 >7.</td><td col-lg-3>Boarding pass of Morning shift shall be collected from the booking office: " + DT1.Rows[0]["PlaceName"].ToString() + " during 5:00 PM to 8:00 PM of previous evening and for evening shift during 10:00 AM to 01:00 PM.</td> </tr>");
                    sb.Append("<tr><td col-lg-3 >8.</td><td col-lg-3>In order to avoid fake bookings, a restriction of booking maximum 6 seats in sinlge transaction has been made.</td> </tr>");
                    sb.Append("<tr><td col-lg-3 >9.</td><td col-lg-3>In case of group booking, park authority will try to give preference in a single vehicle based on availibility at the time of entry.</td> </tr>");
                    sb.Append("<tr><td col-lg-3 >10.</td><td col-lg-3>Entry time of park: </td> </tr>");
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
                }
                sb.Append("</div></section></div>");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString();
        }



    }
}
