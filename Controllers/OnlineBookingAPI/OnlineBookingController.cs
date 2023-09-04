using FMDSS.APIInterface;
using FMDSS.APIModel;
using FMDSS.CustomModels.Models;
using FMDSS.Entity;
using FMDSS.Entity.Mob_BudgetVM;
using FMDSS.Entity.VM;
using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.Master;
using FMDSS.Models.OnlineBooking;
using FMDSS.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace FMDSS.Controllers.OnlineBookingAPI
{
    public class OnlineBookingController : ApiController
    {
        private readonly IOnlineBooking _requestManager;
        BookOnTicket bkt = new BookOnTicket();
        static object slno = new object();
        static string RequestId()
        {
            lock (slno)
            {
                string requestid = DateTime.Now.Ticks.ToString();
                return requestid;
            }
        }
        public OnlineBookingController()
        {
            if (_requestManager == null)
            {
                _requestManager = new FMDSS.APIRepo.OnlineBookingRepo();
            }
        }
        [HttpPost]
        public OnlineBookingZoneWiseModelResponse GetOnlineBookingZoneWiseList([FromBody]OnlineBookingZoneWiseDetailRequestModel model)
        {
            OnlineBookingZoneWiseModelResponse response = new OnlineBookingZoneWiseModelResponse();
            try
            {
                if (model == null)
                {
                    model = new OnlineBookingZoneWiseDetailRequestModel();
                }

                if (model != null && model.Model.UserID > 0)
                    response = _requestManager.GetOnlineBookingZoneDetails(model.Model.UserID, model.Model.PlaceID, model.Model.ShiftId, model.Model.DateOfArrival);
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<OnlineBookingZoneWiseModelResponse>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }




        #region Online Booking Morning Evening Shift in Ranthambore

        public DataTableResponse GetPlaceDetails(long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                response = _requestManager.GetPlaceDetails(UserID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }

        public DataSetResponse chkSafariAccomo(long UserID, long PlaceID,int IsCurrentOrAdvance)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.chkSafariAccomo(UserID, "False", PlaceID, IsCurrentOrAdvance);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }

        public DataTableResponse chkSafariAccomo(long UserID, long PlaceID, long ZoneID, long VehicleCatID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                response = _requestManager.Select_vehicle(UserID, PlaceID, ZoneID, VehicleCatID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }


        #region Online Booking API
        [HttpGet]
        public DataTableResponse GetPlaceDetailsAPI(long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (UserID != null && UserID != 0)
                {
                    response = _requestManager.GetPlaceDetailsAPI(UserID);
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }

            }
            catch (Exception ex)
            {
                response = new DataTableResponse() { Message = ex.Message, Status = ResponseStatus.Failed, Data = null };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse chkSafariAccomoAPI(long UserID, long PlaceID, int IsCurrentOrAdvance)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                if (UserID != 0 && PlaceID != 0)
                {
                    response = _requestManager.chkSafariAccomo(UserID, "False", PlaceID, IsCurrentOrAdvance);
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }

            }
            catch (Exception ex)
            {
                response = new DataSetResponse() { Message = ex.Message, Status = ResponseStatus.Failed, Data = null };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse chkSafariAccomoAPI(long UserID, long PlaceID, long ZoneID, long VehicleCatID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (UserID != null && UserID != 0 && PlaceID != 0 && ZoneID != 0 && VehicleCatID != 0)
                {
                    response = _requestManager.Select_vehicle(UserID, PlaceID, ZoneID, VehicleCatID);
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }

            }
            catch (Exception ex)
            {
                response = new DataTableResponse() { Message = ex.Message, Status = ResponseStatus.Failed, Data = null };
            }
            return response;
        }
        [HttpGet]
        public async Task<DataTableResponse> check_ticketavailability(long placeid, string arrivaldate, string shifttime, int zoneid, int vehicleid)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (placeid != 0 && vehicleid != 0 && zoneid != 0 && !string.IsNullOrEmpty(arrivaldate))
                {
                    response = await _requestManager.chkTicketAvailabiltyandSeatsEqp(placeid, arrivaldate, shifttime, zoneid, vehicleid);
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

         [HttpPost]
        public DataTableResponse BindShiftByPlaceZoneOnlineBooking(int placeID, int Zone, string ArrivalDate, int UserID)
         {
             DataTableResponse response = new DataTableResponse();
            try
            {
                System.Web.HttpContext.Current.Session["IsDepartmentalKioskUser"] = "false";
                if (UserID != 0 && placeID != 0 && Zone != 0 && !string.IsNullOrEmpty(ArrivalDate))
                {
                    response = _requestManager.BindShiftByPlaceZoneOnlineBooking(placeID, Zone, ArrivalDate, UserID);
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
         }

        public DataTableResponse bookticket(SubmitBookingViewModel bookingmodel)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {

                //response = _requestManager.chkTicketAvailabiltyandSeatsEqp(placeid, arrivaldate, shifttime, zoneid, vehicleid);
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ResponseStatus.Failed.ToString(), ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpPost]
        public DataTableResponse bookticketss(SubmitBookingViewModel bookingmodel)
        {
            DataTableResponse response = new DataTableResponse();
            if (bookingmodel != null)
                response.Message = bookingmodel.placeid + " " + bookingmodel.arrivaldate;
            return response;
        }
        [HttpPost]

        #region code for Cancel tiket full and partial
        [HttpGet]
        public DataTableResponse RefundRequestMemberWiseAPI(long ticketid)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                DataTable DT = new DataTable();
                BookOnTicket cs = new BookOnTicket();
                cs.TicketID = ticketid;
                DT = cs.Get_BookTicket_ForRefundProcessMemberWise(Convert.ToString(ticketid));
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = DT };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpPost]
        public DataTableResponse RefundRequestMemberWise(BookOnTicketRequestModel model)
        {
            DataTable DT = new DataTable();
            DataTableResponse response = new DataTableResponse();
            BookOnTicket cs = new BookOnTicket();
            try
            {
                System.Web.HttpContext.Current.Session["UserId"] = model.UserID;
                //var Session = HttpContext.Current.Session;
                //if (Session != null)
                //{
                //    if (Session["UserId"] == null)
                //    {
                //        Session["UserId"] = cs.KioskUserId;
                //    }
                //}
                //HttpContext.Current.Session["UserId"] = model.SSOID;
                if (model != null)
                {
                    string str = JsonConvert.SerializeObject(model);
                    cs = JsonConvert.DeserializeObject<BookOnTicket>(str);
                }

                DT = cs.SubmitFor_BookTicket_ForRefundProcessMemberWise(cs);
                if (DT.Rows.Count > 0)
                {
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    #region Email and SMS

                    objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                    #endregion

                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.", Data = DT };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse RefundRequestAPI(long ticketid)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                DataTable DT = new DataTable();
                BookOnTicket cs = new BookOnTicket();
                cs.TicketID = ticketid;
                DT = cs.Get_BookTicket_ForRefundProcess(Convert.ToString(ticketid));
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = DT };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpPost]
        public DataTableResponse RefundRequestAPI(BookOnTicketRequestModel model)
        {
            DataTable DT = new DataTable();
            DataTableResponse response = new DataTableResponse();
            BookOnTicket cs = new BookOnTicket();
            try
            {
                if (model != null)
                {
                    string str = JsonConvert.SerializeObject(model);
                    cs = JsonConvert.DeserializeObject<BookOnTicket>(str);
                }


                DT = cs.SubmitFor_BookTicket_ForRefundProcess(cs);
                if (DT.Rows.Count > 0)
                {
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    #region Email and SMS

                    objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenRefundRequest", Convert.ToString(DT.Rows[0]["RequestID"]), Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), "");
                    #endregion
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Your request for cancellation of the booking ID has been initiated. Your refundable amount will be transferred in the provided bank account with in due course.", Data = DT };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        #endregion

        [HttpPost]
        public async Task<DataTableResponse> FinalSubmitForOnlineBookingAPI(SubmitBookingViewModel model)
        {
            DataTableResponse response = new DataTableResponse();
            WildLifeOnlineBooking objWildlifebooking = new WildLifeOnlineBooking();
            DataTable dts = new DataTable();
            BookOnTicket cs = new BookOnTicket();
            DataTable dtTicketdetails = new DataTable();
            try
            {


                if (model != null)
                {
                    if (model.placeid == 0)
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please select place." };
                    else if (model.zoneid == 0)
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please select zone." };
                    else if (string.IsNullOrEmpty(model.arrivaldate))
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please enter arrival date." };
                    else if (string.IsNullOrEmpty(model.shifttime))
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please select shift." };
                    else if (model.vehicleid == 0)
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please select vehicle." };
                    else if (model.UserID == 0)
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please select user." };
                    else if (model.memberinfo.Count == 0)
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please enter member details." };

                    //System.Web.HttpContext.Current.Session["IsDepartmentalKioskUser"] = model.UserID;
                    int rowcount = 0;
                    decimal finalAmount = 0;
                    foreach (var item in model.memberinfo)
                    {
                        item.MemberType = "2";
                        item.Isactive = 1;
                        if (string.IsNullOrEmpty(item.MemberName) || string.IsNullOrEmpty(item.MemberGender) || string.IsNullOrEmpty(item.MemberIdType) || string.IsNullOrEmpty(item.MemberIdNo) || string.IsNullOrEmpty(item.MemberNationality))
                        {
                            rowcount++;
                        }
                    }
                    if (rowcount > 0)
                    {
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Please enter member details continiously." };
                    }

                    DataTable dtMemberInfo = new DataTable();
                    dtMemberInfo = objWildlifebooking.MemberInformationWildLifeOnlineBookingIndianAndForeign(model.memberinfo, model.placeid, model.vehicleid, Convert.ToInt32(model.shifttime));


                    if (dtMemberInfo.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtMemberInfo.Rows.Count; k++)
                        {
                            finalAmount += Convert.ToDecimal(dtMemberInfo.Rows[k]["FinalAmountTobePaid"].ToString());
                        }
                    }

                    #region submission
                    cs.KioskUserId = "0";
                    cs.EnteredBy = Convert.ToInt64(model.UserID);
                    cs.RequestId = RequestId();
                    cs.PlaceId = Convert.ToInt64(model.placeid);
                    cs.ZoneId = Convert.ToInt64(model.zoneid);
                    cs.ShiftTime = Convert.ToString(model.shifttime);
                    cs.TotalMember = Convert.ToInt32(dtMemberInfo.Rows.Count);
                    cs.ArrivalDate = DateTime.ParseExact(model.arrivaldate.ToString(), "dd/MM/yyyy", null);
                    cs.vehicleID = Convert.ToInt32(model.vehicleid);
                    DataTable DTCheckBooking = new DataTable();
                    #region Restrict Months
                    DTCheckBooking = cs.Select_CheckBookingDurationsCheckSubmitAPI(cs.PlaceId, cs.ArrivalDate);

                    if (DTCheckBooking.Rows.Count > 0)
                    {
                        if (Convert.ToString(DTCheckBooking.Rows[0]["STATUS"]) == "0")
                        {
                            response.Message = "Date of Visit  must be between " + DTCheckBooking.Rows[0]["TicketDurationFromDate"] + " and " + DTCheckBooking.Rows[0]["TicketDurationToDate"] + " but not booked to July,August and September.";
                            response.Status = ResponseStatus.Failed;
                            return response;
                        }

                    }
                    #endregion

                    dtTicketdetails = cs.CheckTicketAvailabilityForOnlineBooking();
                    if (Globals.Util.isValidDataTable(dtTicketdetails) && dtTicketdetails.Rows.Count > 0)
                    {
                        if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() != "" && dtTicketdetails.Rows[0][2].ToString() != "")
                        {
                            // Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                            cs.VehicleFees_TigerProject = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString());
                            cs.VehicleFees_Surcharge = Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                            // Session["VExtraFee"] = dtTicketdetails.Rows[0][3].ToString();
                            cs.VehicleFees_Total = Convert.ToDecimal(dtTicketdetails.Rows[0][1].ToString()) + Convert.ToDecimal(dtTicketdetails.Rows[0][2].ToString());
                            // seatpereqpt = Convert.ToString(dtTicketdetails.Rows[0]["SeatPerEqpt"]);
                            // strStatus = Session["AvaliableTicket"] + "#" + Session["TotaVechileFees"] + "#" + Session["VExtraFee"] + "#" + Convert.ToInt32(dtTicketdetails.Rows[0][4]);
                        }
                        else if (dtTicketdetails.Rows[0][0].ToString() != "" && dtTicketdetails.Rows[0][1].ToString() == "" && dtTicketdetails.Rows[0][2].ToString() == "")
                        {
                            //Session["AvaliableTicket"] = dtTicketdetails.Rows[0][0].ToString();
                            //strStatus = Session["AvaliableTicket"] + "#";
                        }
                        else
                        {
                            return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Arrival date should not be more than " };
                        }
                    }

                    #region Get Open Days by rajveer
                    DataTable GetDaysDataTable = new DataTable();
                    GetDaysDataTable = cs.chkSafariAccomoDaysOpenBookingAPI(cs.PlaceId);
                    long AddDaysVal = 0;
                    if (GetDaysDataTable.Rows.Count > 0)
                    {
                        AddDaysVal = Convert.ToInt64(GetDaysDataTable.Rows[0][0]);
                    }
                    #endregion
                    DateTime expiryDate = DateTime.Today.AddDays(AddDaysVal);
                    if (cs.ArrivalDate > expiryDate)
                    {
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Arrival date should not be more than " + AddDaysVal + " days" };
                    }

                    cs.IPAddress = model.DeviceUniqueID;
                    DataTable dtcheckTicket = new DataTable();
                    string strcheckTicket = string.Empty;
                    dtcheckTicket = await cs.CheckTicketAvailabilityWityPalaceOfWheel();
                    strcheckTicket = dtcheckTicket.Rows[0][0].ToString();
                    if (Convert.ToInt32(strcheckTicket) >= cs.TotalMember)
                    {
                        DataTable dt_InsertDeviceDetails = new DataTable();
                        dt_InsertDeviceDetails = cs.InsertDeviceDetails(Convert.ToString(model.DeviceUniqueID), Convert.ToString(model.DeviceType), Convert.ToInt64(model.UserID), Convert.ToString(model.AppVersion));

                        if (Convert.ToString(dt_InsertDeviceDetails.Rows[0]["Status"]) == "1")
                        {
                            dtMemberInfo.Columns.Remove("FinalAmountTobePaid");
                            dtMemberInfo.AcceptChanges();
                            dts = await cs.Submit_TicketDetails(dtMemberInfo, finalAmount);
                            response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dts };
                        }
                        else
                        {
                            return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = Convert.ToString(dt_InsertDeviceDetails.Rows[0]["Message"]), Data = dt_InsertDeviceDetails };
                        }
                    }
                    else
                    {
                        return response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Ticket not avaliable" };
                    }
                    #endregion

                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong! Please try again. Model get null." };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

        public DataTableResponse GetBookedTickets(long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (UserID != 0)
                {
                    BookOnTicket Bok = new BookOnTicket();
                    DataTable dtf = new DataTable();
                    dtf = Bok.Select_BookedTicketAPI(UserID);
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dtf };
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

        #endregion

        #region code for payment
        public DataTableResponse PayNow(SubmitBookingViewModel model)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (!string.IsNullOrEmpty(model.RequestId) && model.UserID > 0)
                {

                    // get different heads amount from DB
                    BookOnTicket OBJ = new BookOnTicket();
                    DataSet DS = new DataSet();
                    DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("WildLifeTickets", Convert.ToString(model.RequestId));

                    string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"] + "|" + Convert.ToString(DS.Tables[1].Rows[0]["HeadVehicleRentAndGuidFees"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["VehicleRentandGuidFees"]));
                    //string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["TigerProject"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Surcharge"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Foundation"]);
                    //string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                    //string ReturnUrl = "http://103.203.138.101/FmdssOnlineBooking/api/OnlineBooking/PaymentAPI";
                    string ReturnUrl = Convert.ToString(ConfigurationManager.AppSettings["OnlineBookingPaymentRedirectionResponse"]);
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();

                    string forms = ObjEmitraPayRequest.PayRequest(true, Convert.ToString(model.RequestId),
                    Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                        //ReturnUrl + "BookOnlineTicket/Payment", ReturnUrl + "BookOnlineTicket/Payment",
                        //ReturnUrl + "WildLifePaymentResponse.aspx", ReturnUrl + "WildLifePaymentResponse.aspx",
                    ReturnUrl, ReturnUrl,
                    Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]),
                    REVENUEHEAD,
                    Convert.ToString(model.UserID)
                    );
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = forms };
                }
                else
                {
                    throw new Exception("User is not authenticate");
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;

        }

        [HttpPost]
        public DataTableResponse PaymentAPI(string ENCDATA)
        {
            SubmitBookingViewModel model = new SubmitBookingViewModel();
            int fmdssStatus = 0;
            string resultMsg = "";
            DataTable resDt = new DataTable();
            DataTableResponse response = new DataTableResponse();
            if (!string.IsNullOrEmpty(model.RequestId))
            {
                try
                {

                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA1 = "";

                    //if (Request.Form["MERCHANTCODE"] != null)
                    //    MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                    //if (Request.Form["PRN"] != null)
                    //    PRN = Request.Form["PRN"].ToString();
                    //if (Request.Form["STATUS"] != null)
                    //    STATUS = Request.Form["STATUS"].ToString();
                    //if (Request.Form["ENCDATA"] != null)
                    //    ENCDATA = Request.Form["ENCDATA"].ToString();

                       EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23"); //Live
                   // EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    // string DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    BookOnTicket cs = new BookOnTicket();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    cs.UpdateEmitraResponse(Convert.ToString(model.RequestId), "WildLifeTicketBooking", DecryptedData);

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

                        cs.KioskUserId = Convert.ToString(model.UserID);


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
                        if (fmdssStatus == 1)
                        {
                            dtrow["TRANSACTION STATUS"] = "SUCCESS";
                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";
                        }
                        dt.Rows.Add(dtrow);
                        response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {
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
                            try
                            {
                                if (Convert.ToString(model.RequestId).Equals(ObjPGResponse.PRN))
                                {
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
                            }
                            finally
                            {
                            }
                        }
                        if (fmdssStatus == 1)
                        {
                            dtrow["TRANSACTION STATUS"] = "SUCCESS";
                        }
                        else
                        {
                            dtrow["TRANSACTION STATUS"] = "FAILED";
                        }
                        dt.Rows.Add(dtrow);
                        response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                    }
                    #endregion
                    List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();

                    string TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();

                    if (TicketStatus == "SUCCESS")
                    {
                        DataTable DTdetails = cs.Get_BookedTicketDetails(Convert.ToString(model.RequestId));

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
                        string PrintID = "";
                        if (DTdetails.Rows.Count > 0)
                        {
                            PrintID = Convert.ToString(DTdetails.Rows[0]["PrintID"]);
                        }
                        else
                        {
                            PrintID = "";
                        }
                        response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = DTdetails };
                    }

                }
                catch (Exception ex)
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong!", ErrorDescription = ex.Message };
                }
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse PrintWildLifeTicket(string RequestID)
        {
            StringBuilder sb = new StringBuilder();
            DataTableResponse response = new DataTableResponse();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string filepath = "";
            try
            {
                BookOnTicket cs = new BookOnTicket();
                BookOnTicket cs1 = new BookOnTicket();
                DataTable dt_GetTicket = new DataTable();
                DataSet ds = new DataSet();
                long TicketID = 0;
                dt_GetTicket = cs1.Get_TicketId(RequestID);
                if (dt_GetTicket.Rows.Count > 0)
                {
                    TicketID = Convert.ToInt64(dt_GetTicket.Rows[0]["TicketID"]);
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Ticket ID not found !" };
                }
                cs.TicketID = TicketID;
                ds = cs.Select_TicketData_For_Print();

                DataTable dtTC = new DataTable();
                //dtTC = cs.Select_TermandConditionbyTicketId(Convert.ToInt64(Encryption.decrypt(ticketid)));
                dtTC = cs.Select_TermandConditionbyTicketId(TicketID);
                filepath = htmlToPdfDownloadTickets.WildlifeDownloadPdf(ds, dtTC);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = filepath };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

        #endregion

        #endregion

        [HttpGet]
        public DataTableResponse IsValidRequestID(string RequestId, bool isbarcode)
        {
            DataTableResponse response = new DataTableResponse();
            string msg = string.Empty;

            // EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");
            //string EncyptedData = objEncryptDecrypt.encrypt(RequestId);
            // string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(RequestId).Substring(0, 18), "E-m!tr@2016");


            //string DecryptedData = objEncryptDecrypt.decrypt(EncyptedData);
            //string DecryptedData = objEncryptDecrypt.decrypt(RequestId);
            RequestId = RequestId.Replace(" ", "+");
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            try
            {
                if (isbarcode == true)
                {
                    string DecryptedData = FMDSS.Models.EncodingDecoding.Decrypt(RequestId, "E-m!tr@2016");
                    dt = repo.CheckBookingId(DecryptedData);
                }
                else
                {
                    dt = repo.CheckBookingId(RequestId);
                }

                if (dt.Rows.Count > 0)
                {
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Booking id exists.", Data = dt };
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Booking id not exists." };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse IsValidRequestZooTicketIDByMobileNo(string MobileNo, int IsEnterOrOut, int ZooSectionId,int PlaceId, long UserId)
        {
            DataTableResponse response = new DataTableResponse();
            string msg = string.Empty;
            try
            {
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                DataTable dt = new DataTable();
                if (IsEnterOrOut == 0)
                {
                    dt = repo.CheckZooRequestIdByMobileForEnter(MobileNo, ZooSectionId, PlaceId, UserId);
                }
                else if (IsEnterOrOut == 1)
                {
                    dt = repo.CheckZooRequestIdByMobileForOut(MobileNo, ZooSectionId, PlaceId, UserId);
                }
                if (Convert.ToString(dt.Rows[0]["status"]) != "0")
                {
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = Convert.ToString(dt.Rows[0]["msg"]), Data = dt };
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = Convert.ToString(dt.Rows[0]["msg"]) };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse IsValidRequestZooTicketID(string RequestId, bool isbarcode, int IsEnterOrOut, int ZooSectionId, long UserId)
        {
            DataTableResponse response = new DataTableResponse();
            string msg = string.Empty;

            // EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");
            //string EncyptedData = objEncryptDecrypt.encrypt(RequestId);
            // string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(RequestId).Substring(0, 18), "E-m!tr@2016");


            //string DecryptedData = objEncryptDecrypt.decrypt(EncyptedData);
            //string DecryptedData = objEncryptDecrypt.decrypt(RequestId);
            RequestId = RequestId.Replace(" ", "+");
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            try
            {
                if (isbarcode == true && IsEnterOrOut == 0)
                {
                    string DecryptedData = FMDSS.Models.EncodingDecoding.Decrypt(RequestId, "E-m!tr@2016");
                    dt = repo.CheckZooBookingId(DecryptedData, ZooSectionId, UserId);
                }
                if (isbarcode == true && IsEnterOrOut == 1)
                {
                    string DecryptedData = FMDSS.Models.EncodingDecoding.Decrypt(RequestId, "E-m!tr@2016");
                    dt = repo.CheckZooRequestIdForOut(DecryptedData, ZooSectionId, UserId);
                }
                else if (isbarcode == false && IsEnterOrOut == 0)
                {
                    dt = repo.CheckZooBookingId(RequestId, ZooSectionId, UserId);
                }
                else if (isbarcode == false &&  IsEnterOrOut == 1)
                {
                    dt = repo.CheckZooRequestIdForOut(RequestId, ZooSectionId, UserId);
                }


                if (Convert.ToString(dt.Rows[0]["status"]) != "0")
                {
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = Convert.ToString(dt.Rows[0]["msg"]), Data = dt };
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = Convert.ToString(dt.Rows[0]["msg"]) };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetZooSectionList(int PlaceId)
        {
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            DataTableResponse response = new DataTableResponse();
            try
            {
                dt = repo.GetZooSectionList(PlaceId);
                if (dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Get Successfully Records", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Get Zero Reords", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetZooTicketDetailByRequestId(string RequestId, bool isbarcode, int PlaceId, long UserId)
        {
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataSet dt = new DataSet();
            DataTableResponse response = new DataTableResponse();
            try
            {
               
                if (isbarcode == true )
                {
                     RequestId = FMDSS.Models.EncodingDecoding.Decrypt(RequestId, "E-m!tr@2016");
                }
               
                    dt = repo.GetZooTicketDetailByRequestId(RequestId,PlaceId,UserId);

                if (dt.Tables[0].Rows.Count > 0 && dt.Tables[1].Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Get Successfully Records", Data = dt.Tables[0], Data1 = dt.Tables[1] };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Get Zero Reords", Data = dt.Tables[0] };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetZooTicketDetailByMobileNo(string MobileNo, int PlaceId, long UserId)
        {
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataSet dt = new DataSet();
            DataTableResponse response = new DataTableResponse();
            try
            {
                dt = repo.GetZooTicketDetailByMobileNo(MobileNo, PlaceId, UserId);
                if (dt.Tables[0].Rows.Count > 0 && dt.Tables[1].Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Get Successfully Records", Data = dt.Tables[0], Data1 = dt.Tables[1] };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Get Zero Reords", Data = dt.Tables[0] };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetZooTicketDashboardDetail(int PlaceId,string DateOfVisit, long UserId)
        {
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            DataTableResponse response = new DataTableResponse();
            try
            {
                dt = repo.GetZooTicketDashboardDetail(PlaceId, DateOfVisit, UserId);
                if (dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Get Successfully Records", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Get Zero Reords", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetMobileVersion()
        {
            DataTableResponse response = new DataTableResponse();
            string msg = string.Empty;
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            try
            {
                dt = repo.GetMobileVersion();
                if (dt.Rows.Count > 0)
                {
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Fail" };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetMobileVersion(string ApkName)
        {
            DataTableResponse response = new DataTableResponse();
            string msg = string.Empty;
            OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
            DataTable dt = new DataTable();
            try
            {
                dt = repo.GetMobileVersion(ApkName);
                if (dt.Rows.Count > 0)
                {
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                }
                else
                {
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Fail" };
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
    }
}
