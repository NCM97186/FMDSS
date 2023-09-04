using FMDSS.Models;
using FMDSS.Models.BookingRoaster;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.OnlineBooking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.BookingRoster
{
    public class BookingRosterController  : Controller //BaseOnlinebookingRanthmboreController
	{
        //
        // GET: /BookingRoster/

        public ActionResult ChooseGuideVehicle()
        {
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			
			List<SelectListItem> lstPlace = new List<SelectListItem>();
		
			ViewBag.PlaceList = lstPlace;
			return View();
        }
		[HttpGet]
		public JsonResult GetPlaceList(string VisitDate)
		{
			List<SelectListItem> lstPlace = new List<SelectListItem>();
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int UserID = Convert.ToInt32(Session["UserID"]);
			string SSOID = Session["SSOid"].ToString();

			try
			{
				BookingRoasterNew BRnew = new BookingRoasterNew();
				DataTable Ds = new DataTable();
				Ds = BRnew.GetPlaceList(SSOID, VisitDate);
				foreach (System.Data.DataRow dr2 in Ds.Rows)
				{

					lstPlace.Add(new SelectListItem { Text = dr2["PlaceName"].ToString(), Value = dr2["PlaceID"].ToString() });
				}
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
			}

			return Json(lstPlace, JsonRequestBehavior.AllowGet);
		}
		
		public ActionResult CitizenChooseGuideVehicle()
		{

			return View();
		}
		
		[HttpGet]
		public JsonResult GetCitizenVisitDetailList(string VisitDate)
		{
			List<CitizenVisitDetail> visitDetal = new List<CitizenVisitDetail>();
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int UserID = Convert.ToInt32(Session["UserID"]);
			string SSOID = Session["SSOid"].ToString();

			try
			{
				BookingRoasterNew BRnew = new BookingRoasterNew();				
				visitDetal = BRnew.GetCitizenTicketList(SSOID, VisitDate);
				
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
			}
			//return PartialView("_PartialCitizenVisitDetail", visitDetal);
			 return Json(visitDetal, JsonRequestBehavior.AllowGet);
		}

		
	    [HttpGet]
		public JsonResult GetVehicleMaxSeats(int PlaceId,  int ShiftId, int VehicleTypeId,int ZoneId)
		{

			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int UserID = Convert.ToInt32(Session["UserID"]);
			string SSOID = Session["SSOid"].ToString();
			int maxSeats = 0;
			
			try
			{
				
				BookingRoasterNew BRnew = new BookingRoasterNew();
				DataTable Ds = new DataTable();
				Ds = BRnew.GetVehicleMaxSeats(PlaceId, VehicleTypeId, ShiftId, ZoneId);
				foreach (System.Data.DataRow dr2 in Ds.Rows)
				{
					maxSeats = Convert.ToInt32(dr2["MaxSeats"].ToString());
				}

			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
			}
			//return PartialView("_PartialCitizenVisitDetail", visitDetal);
			return Json(maxSeats, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetSavedGuideList(int PlaceId, string VisitDate,int VehicleId,int ShiftId, int VehicleTypeId,int ZoneId,int MemberCount,string RequestId)
		{
			
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int UserID = Convert.ToInt32(Session["UserID"]);
			string SSOID = Session["SSOid"].ToString();
			List<SelectListItem> lstGuide = new List<SelectListItem>();
			try
			{
				BookingRoasterNew BRnew = new BookingRoasterNew();
				DataSet Ds = new DataSet();
                var requesteId = Encryption.decrypt(RequestId);
                Ds = BRnew.GetSavedGuideList(PlaceId,VisitDate, VehicleId, VehicleTypeId, ShiftId, ZoneId, MemberCount, requesteId);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(Ds.Tables[0].Rows[0]["MsgStatus"].ToString()) == 1)
                    {
                        foreach (System.Data.DataRow dr2 in Ds.Tables[1].Rows)
                        {
                            lstGuide.Add(new SelectListItem { Text = dr2["GuideName"].ToString() + "[" + dr2["GuideMobileNo"].ToString() + "]", Value = dr2["GuideId"].ToString() });
                        }
                    }
                }
                var data = new { MsgStatus = Convert.ToInt16(Ds.Tables[0].Rows[0]["MsgStatus"].ToString()),Msg= Ds.Tables[0].Rows[0]["Msg"].ToString(), LstGuide= lstGuide };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
			catch (Exception ex)
			{
                
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                var data = new { MsgStatus = 0, Msg = ex.Message.ToString(), LstGuide = lstGuide };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
			//return PartialView("_PartialCitizenVisitDetail", visitDetal);
			
		}
		[HttpGet]
		public JsonResult GetSavedVehicleList(int PlaceId, string VisitDate, int GuideId, int ShiftId,int VehicleTypeId,int	ZoneId, int MemberCount, string RequestId)
		{

			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int UserID = Convert.ToInt32(Session["UserID"]);
			string SSOID = Session["SSOid"].ToString();
			List<SelectListItem> lstVehicle = new List<SelectListItem>();
			try
			{
				BookingRoasterNew BRnew = new BookingRoasterNew();
                DataSet Ds = new DataSet();
                var requesteId = Encryption.decrypt(RequestId);
                Ds = BRnew.GetSavedVehicleList(PlaceId,VisitDate, GuideId, VehicleTypeId, ShiftId, ZoneId, MemberCount, requesteId);
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(Ds.Tables[0].Rows[0]["MsgStatus"].ToString()) == 1)
                    {
                        foreach (System.Data.DataRow dr2 in Ds.Tables[1].Rows)
                        {
                            lstVehicle.Add(new SelectListItem { Text = dr2["VehicleNumber"].ToString(), Value = dr2["VehicleId"].ToString() });
                        }
                    }
                }
                var data = new { MsgStatus = Convert.ToInt16(Ds.Tables[0].Rows[0]["MsgStatus"].ToString()), Msg = Ds.Tables[0].Rows[0]["Msg"].ToString(), LstVehicle = lstVehicle };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                var data = new { MsgStatus = 0, Msg = ex.Message.ToString(), LstVehicle = lstVehicle };
                return Json(data, JsonRequestBehavior.AllowGet);
            }			
		}

		#region New Boarding Pass Generation System
		[HttpGet]
		public	ActionResult GenerateBoardingPass(string KioskBoarding = "", string KioskBoardingDirect = "false")
		{
			List<SelectListItem> vehicleCategory = new List<SelectListItem>();
			List<CS_Ticket> ticketList = new List<CS_Ticket>();
			List<CS_BoardingDetails> ListBoarding = new List<CS_BoardingDetails>();
			List<SelectListItem> lstZone = new List<SelectListItem>();
			List<SelectListItem> lstPlace = new List<SelectListItem>();
			List<SelectListItem> lstVehicle = new List<SelectListItem>();
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


				DataSet Ds = obj.BindDptKioskPLACES(SSOID);

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

					DataTable DT_BoardingDuration = obj.GetBoardingDuration(tempPlaceId);
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


							DataTable DT_DptUser = obj.GetZoneByPlace(dtKioskBoarding.Rows[0]["PlaceID"].ToString());

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
							if (!string.IsNullOrEmpty(KioskBoardingDirect) && KioskBoardingDirect.ToLower() == "true")
							{
								obj.RequestID = KioskBoarding.ToString();
							}
							#endregion

							//COMMONGETDATA(obj.DateofVisit, obj.PlaceId, obj.Shift, BOOKING_TYPE, obj.ZoneID, obj.Vehicle, obj.RequestID);
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
		[NonAction]
		public void BoardingPassLoad()
		{
			List<SelectListItem> lstShiftType = new List<SelectListItem>();

			lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
			lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });
			// lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });

			ViewBag.LstddlShift = lstShiftType;

		}
		//public void COMMONGETDATA(string DateofVisit, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string Zone, string VehicleTypeID, string RequestID)
		public JsonResult GetCitizenBookingVisitDetail(string DateofVisit, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string Zone, string VehicleTypeID, string RequestID)
		{
			List<CS_BoardingDetails> ListBoarding = new List<CS_BoardingDetails>();
			DataTable DT;

			CS_BoardingDetails obj = new CS_BoardingDetails();
			BOOKING_TYPE = "RequestIDWiseBoardingPass";
			DT = obj.GetCurrentDateBooking(DateofVisit, PlaceID, SHIFT_TYPE, BOOKING_TYPE, Zone, VehicleTypeID, RequestID);

            List<SelectListItem> lstGuide = new List<SelectListItem>();
            List<SelectListItem> lstVehicel = new List<SelectListItem>();
            BookingRoasterNew BRnew = new BookingRoasterNew();
            DataTable Ds = new DataTable();
            DataTable Ds2 = new DataTable();
            string reqId = "";
            Ds = BRnew.GetGuideListAccordingBoardingPass(DateofVisit, reqId, SHIFT_TYPE, PlaceID, VehicleTypeID, Convert.ToInt32(Zone));
            Ds2 = BRnew.GetVehicleListAccordingBoardingPass(DateofVisit, VehicleTypeID, PlaceID, SHIFT_TYPE, reqId, Convert.ToInt32(Zone));

            //string strGuideList = "< option value = '0' > --Select Guide-- </ option >";
            //string strVehicleList = "< option value = '0' > --Select Vehicle-- </ option >";
            foreach (System.Data.DataRow dr2 in Ds.Rows)
            {
                //strGuideList+= "<option value=" + dr2["GuideId"].ToString() + ">" + dr2["GuideName"].ToString() + "</option>";
                lstGuide.Add(new SelectListItem { Text = dr2["GuideName"].ToString() + "[" + dr2["GuideMobileNo"].ToString() + "]", Value = dr2["GuideId"].ToString() });
            }
            foreach (System.Data.DataRow dr2 in Ds2.Rows)
            {
                //strVehicleList+= "<option value=" + dr2["VehicleId"].ToString() + ">" + dr2["VehicleNumber"].ToString() + "</option>";
                lstVehicel.Add(new SelectListItem { Text = dr2["VehicleNumber"].ToString(), Value = dr2["VehicleId"].ToString() });
            }

            int count = 1;
			//Int16 ColorNumber = 0;
			foreach (DataRow dr in DT.Rows)
			{


                #region "GetVehicleNumber"

                #endregion
                reqId = Convert.ToString(dr["RequestID"].ToString());
                ListBoarding.Add(
                new CS_BoardingDetails()
                {


                    Index = count,
                    RequestId2 = Encryption.encrypt(reqId),
                    DisplayBookingId = Convert.ToString(dr["DisplayRequestID"].ToString()),
                    VehicleName = Convert.ToString(dr["NEWVehicleName"].ToString()),
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
                    IsNoShowOpenOrNot = DT.Columns.Contains("IsNoShowOpenOrNot") ? Convert.ToString(dr["IsNoShowOpenOrNot"]) : "S",
                    GuideId = Convert.ToInt16(dr["GuideId"].ToString()),
                    //GuideList= lstGuide,
                   
                    VehicleId = Convert.ToInt16(dr["VehicleId1"].ToString()),
					//VehicleList =lstVehicel,
                    
                    ChoiceGuideAmt = Convert.ToDecimal(dr["ChoiceGuideAmt"]),
                    ChoiceVehicleAmt = Convert.ToDecimal(dr["ChoiceVehicleAmt"]),
                    ChoiceGuideReplaceId = Convert.ToInt32(dr["ChoiceGuideReplaceId"]),
                    ChoiceVehicleReplaceId = Convert.ToInt32(dr["ChoiceVehicleReplaceId"]),
                    
                    // ExtraAmountRevised= Convert.ToDecimal(dr["ExtraAmountRevised"].ToString()),
                    //GuideId = Convert.ToInt32(dr["GuideId"])
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


            //ViewBag.GuideSummary = SBListBoardingSummary.ToString();
            //ViewData["ListBoarding"] = ListBoarding;

            var data = new { visitDetal = ListBoarding, GuideList = lstGuide, VehicleList = lstVehicel };
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(ListBoarding, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidationForGuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, int GuideId, int VehicleID2)
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
                if (requesteId.Contains("C19-"))
                {
                    DS = obj.ValidationForGuidNameAndVehicleNumber(GuidName.ToUpper(), VehicleNumber.ToUpper(), Place, ZoneID, DateOfArrival, VehicleID, ShiftTime, Encryption.decrypt(ID), UpdatedBy);
                }
                else
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

                DT = obj.ValidateVehicleAndGuide(requesteId, DateOfArrival, ShiftTime, GuideId, VehicleID2, Convert.ToInt32(VehicleID)); //VehicleID is Equal to Vehicle Type Id
                if (DT.Rows.Count > 0)
                {
                    rows = DT.Rows[0][0].ToString();
                    STATUS = DT.Rows[0][1].ToString();
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }



            return Json(new { STATUS = STATUS, Remaining = rows }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GuidNameAndVehicleNumberUpdate(string ID, string GuidName, string VehicleNumber, int GuideId, int VehicleID2, string IsCurrentBooking = "")
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
                DataTable DT; DataTable dataTable;

                dataTable = obj.IsBlockedRequest(requesteId);
                isBlocked = (int)dataTable.Rows[0]["Blocked"];
                if (isBlocked == 0)
                {
                    if (requesteId.Contains("C19-"))
                    {
                        DT = obj.UpdateGuidNameAndVehicleNumber(Encryption.decrypt(ID), GuidName.ToUpper(), VehicleNumber.ToUpper(), UserID.ToString(), GuideId, VehicleID2);
                    }
                    else
                    {
                        DT = obj.UpdateGuidNameAndVehicleNumber(Encryption.decrypt(ID).Substring(0, 18), GuidName.ToUpper(), VehicleNumber.ToUpper(), UserID.ToString(), GuideId, VehicleID2);  //comment by shaan 24 - 07 - 2020
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

                var data = new { STATUS = "Blocked", Msg = "This Request id :" + requesteId + "is blocked,kindly contact to system administrator.!!!" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
        #region Replaced Guide and Vehicle Detail
        public JsonResult SaveReplacedVehicleGuide(string RequestId, int VehicleId,int GuideId,int ShiftId,string VisitDate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            string requestId = Encryption.decrypt(RequestId);
            
            try
            {
                BookingRoasterNew BRnew = new BookingRoasterNew();
                DataTable dt = new DataTable();
                dt = BRnew.SaveReplacedVehicleGuide(requestId, GuideId, VehicleId,ShiftId,VisitDate);
                var responseStatus=new  {MsgStatus=Convert.ToInt16(dt.Rows[0]["MsgStatus"].ToString()),Msg= dt.Rows[0]["Msg"].ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                var responseStatus = new { MsgStatus = 0, Msg = ex.Message.ToString() };
                return Json(responseStatus, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion
    }
}
