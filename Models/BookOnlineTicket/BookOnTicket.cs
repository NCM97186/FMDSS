//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : File contains calling functions from controller
//  Date Created : 22-08-2016
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
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace FMDSS.Models.BookOnlineTicket
{

    public class CovidBooking : DAL
    {
        public string TicketId { get; set; }
        public string PlaceName { get; set; }
        public int PlaceId { get; set; }
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string ZoneName { get; set; }
        public string ShiftName { get; set; }
        public string DateofArrival { get; set; }

        public string DateofArrival1 { get; set; }

        public string RequestID { get; set; }

        public decimal TotalAmount { get; set; }
        public string FirstDate { get; set; }
        public string SecondDate { get; set; }
        public string ThirdDate { get; set; }
        public string ApprovedVisitDate { get; set; }
        public int ShiftId { get; set; }
        public int isDFOApproved { get; set; }
        public bool isActive { get; set; }
        public string Remark { get; set; }
        public string Enteredon { get; set; }
        public int ZoneID { get; set; }
        public int BookedQuota { get; set; }
        public string QuotaType { get; set; }

        public List<CovidMemberDetails> lstMemberDetails { get; set; }
        public List<OptionalCovidBooking> lstOptionalCovidBooking { get; set; }


        public DataTable CheckAvailableSeats(CovidBooking oBooking)
        {
            DataTable dtCovid = new DataTable();
            try
            {
                
                DALConn();
                SqlCommand cmd = new SqlCommand("spGetCovidTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "CheckTicketAvailablity");
                cmd.Parameters.AddWithValue("@OLDTicketId", oBooking.TicketId);               
                cmd.Parameters.AddWithValue("@ShiftName", oBooking.ShiftName);                
                cmd.Parameters.AddWithValue("@DateofArrival1",Convert.ToDateTime(DateofArrival).ToString("MM/dd/yyyy"));                 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }
        public DataTable CheckAvailableSeatsForCovid(CovidBooking oBooking)
        {
            DataTable dtCovid = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("spGetCovidTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "CheckTicketAvailablityForCovid");
                cmd.Parameters.AddWithValue("@OLDTicketId", oBooking.TicketId);
                cmd.Parameters.AddWithValue("@ShiftName", oBooking.ShiftName);
                cmd.Parameters.AddWithValue("@DateofArrival1", Convert.ToDateTime(DateofArrival).ToString("MM/dd/yyyy"));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }

        public DataTable CheckAvailableSeatsForHDFD(CovidBooking oBooking)
        {
            DataTable dtCovid = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("spGetCovidTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "CheckTicketAvailablityForHDFD");
                cmd.Parameters.AddWithValue("@OLDTicketId", oBooking.TicketId);
                cmd.Parameters.AddWithValue("@ShiftName", oBooking.ShiftName);
                cmd.Parameters.AddWithValue("@DateofArrival1", Convert.ToDateTime(DateofArrival).ToString("MM/dd/yyyy"));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }


        public DataTable SaveCovidBooking(CovidBooking oBooking)
        {
            DataTable dtCovid = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spGetCovidTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "SaveCovidTicketDetail");
                cmd.Parameters.AddWithValue("@OLDTicketId", oBooking.TicketId);
                cmd.Parameters.AddWithValue("@RequestId", oBooking.RequestID);
                cmd.Parameters.AddWithValue("@ShiftName", oBooking.ShiftName);
                cmd.Parameters.AddWithValue("@DateofArrival1",Convert.ToDateTime(oBooking.DateofArrival).ToString("yyyy-MM-dd")); 
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }



    }

    public class CovidMemberDetails
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string IdProof { get; set; }
        public string NoofCamera { get; set; }
        public string MemberFees { get; set; }
        public string CameraFees { get; set; }
        public string VehicleFees { get; set; }
        public string BoardingVehicleFee { get; set; }
        public string BoardingGuideFeeGSTAmount { get; set; }
        public string BoardingVehicleFeeGstAmount { get; set; }
        public string BoardingGuideFee { get; set; }
        public string Amount { get; set; }

    }

    public class MailCovidData
    {
        public string ToMail { get; set; }
        public string MailSubject { get; set; }
        public string RequestID { get; set; }
        public string PreviousVisitDate { get; set; }
        public string PreviousShift { get; set; }
        public string PreviousZone { get; set; }
        public string NoOFMembers { get; set; }
        public string RequestedShift { get; set; }
        public string RequestedPlaceName { get; set; }
        public string FirstDate { get; set; }
        public string SecondDate { get; set; }
        public string ThirdDate { get; set; }
        
    }

    public class OptionalCovidBooking
    {
        public int RowId { get; set; }
        public string TicketId { get; set; }
        //public string PlaceName { get; set; }
        public int PlaceId { get; set; }
        //public string VehicleName { get; set; }
        //public string ZoneName { get; set; }
        //public string ShiftName { get; set; }
        //public string DateofArrival { get; set; }
        public string RequestId { get; set; }
        public string FirstDate { get; set; }
        public string SecondDate { get; set; }
        public string ThirdDate { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int isDFOApproved { get; set; }
        public string ApprovedVisitDate { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Enteredon { get; set; }
    }

    #region added by shaan 29-01-2021
    public class WildLifeBookingFilterModel
    {
        public Int64 UserId { get; set; }
        public string DateType { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Place { get; set; }
        public string TypeOfBooking { get; set; }
        public string Status { get; set; }
        public int StartRow { get; set; }
        public int FetchRowsNext { get; set; }
        public string search { get; set; }
        public List<WildLifeBookingListModel> WildLifeBookingListModel { get; set; }
    }
    public class WildLifeBookingListModel
    {
        public Int64 UserId { get; set; }
        public Int64 RowID { get; set; }
        public string RequestID { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfArrival { get; set; }
        public string TotalMembers { get; set; }
        public string TotalAmount { get; set; }
        public string Place { get; set; }
        public string TypeOfBooking { get; set; }
        public string Status { get; set; }
        public int PlaceID { get; set; }
        public string ShiftTime { get; set; }
        public int COVIDStatus { get; set; }
        public int TicketMemberBordingStatus { get; set; }
        public int isDFOApproved { get; set; }
        public int RefundStatus { get; set; }
        public Int64 TicketID { get; set; }
        public Int64 EmitraTransactionID { get; set; }
        public string CancleTicketStatus { get; set; }
        public string CancleTicketStatusName { get; set; }
        public int CancelStatus { get; set; }

        //--Added by Mukesh Kumar Jangid on 19-09-2022
        public string TotalRefundableAmt { get; set; }
        public string TotalRemainingAmt { get; set; }
        public string ActionInitiatedBy { get; set; }
        public string ActionName { get; set; }
        public string CancelliationReason { get; set; }
        public string TransactionId { get; set; }
        public string CancelliationDate { get; set; }
        public string BookingDayDiff { get; set; }
        public Int64 ID { get; set; }
        public string CStatusDesc { get; set; }

        public string Old_RequestId { get; set; }
        public string OldRequestTitle { get; set; }
        //--Added by Mukesh Kumar Jangid on 19-09-2022
        public string Actions { get; set; }

    }
    #endregion



    [Serializable]// add by rajveer load balanceing
    public class BookOnTicket : DAL
    {

        #region global variable

        public Int64 PlaceId;
        public Int64 ZoneId { get; set; }
        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }
        public string isSafari { get; set; }
        public string isAccomo { get; set; }
        public string isMorning { get; set; }
        public string isEvening { get; set; }
        public string isFullDay { get; set; }
        public string OldRequestID { get; set; }
        public string OldRequestIDSecound { get; set; }
        public string AutomatedScript { get; set; }
        public string RequestedId { get; set; }

        /// <summary> ----Added by Mukesh Kumar Jangid on 22-09-2022
        public string Old_RequestId { get; set; }
        public string OldRequestTitle { get; set; }
        public string Reserve_Status { get; set; } //----Added by Mukesh Kumar Jangid on 01-11-2022
        public string WaitingStatus { get; set; }
        /// </summary>


        public int Is_PNR_NO { get; set; }
        public int Is_Seat_NO { get; set; }
        public int Is_Room_No { get; set; }

        public string PNR_NO { get; set; }
        public string Seat_NO { get; set; }
        public string Room_No { get; set; }

        public string txt_OldIDProof { get; set; }
        public string PlaceName { get; set; }
        public string isHalfDay { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string ShiftTime { get; set; }
        public Int32 vehicleID { get; set; }
        public decimal VehicleFees_TigerProject { get; set; }
        public decimal VehicleFees_Surcharge { get; set; }
        public decimal VehicleFees_Total { get; set; }
        public decimal MemberFees_TigerProject { get; set; }
        public decimal MemberFees_Surcharge { get; set; }
        public decimal TRDF { get; set; }
        public decimal CameraFees_TigerProject { get; set; }
        public decimal CameraFees_Surcharge { get; set; }
        public decimal TotalPerMemberFees { get; set; }
        public decimal TotalPerMemberCameraFees { get; set; }

        //Added by shaan 30-03-2021
        public decimal Fees_TigerProjectHalfDayFullDayCharge { get; set; }
        public decimal Fee_SurchargeHalfDayFullDayCharge { get; set; }
        //END

        // added by arvind sharma 25/07/2017
        // begin
        public decimal BoardingVehicleFee { get; set; }
        public decimal BoardingVehicleFeeGSTPercentage { get; set; }
        public decimal BoardingVehicleFeeGSTAmount { get; set; }

        public decimal BoardingGuideFee { get; set; }
        public decimal BoardingGuideFeeGSTPercentage { get; set; }
        public decimal BoardingGuideFeeGSTAmount { get; set; }

        public decimal TotalBoardingFee { get; set; }
        public string GSTMessage { get; set; }
        public decimal Vehicle_TRDF { get; set; }
        public decimal GuidFee_TRDF { get; set; }

        // end
        // added by arvind sharma 25/07/2017


        public Int64 AccomoID;
        public decimal RoomCharge;

        public int totalRoom;

        public decimal RoomAvailability;
        public string KioskUserId { get; set; }
        public Int64 EnteredBy { get; set; }
        public int IsPartialOrFullCancelation { get; set; }
        public string RequestId { get; set; }
        public int TotalMember { get; set; }
        public int TotalRoom { get; set; }
        public string IPAddress { get; set; }
        public string EmitraTransactionId { get; set; }
        public int Trn_Status_Code { get; set; }
        public Int64 TicketID { get; set; }
        public Int64 CGTickets { get; set; }
        public int CGTCount { get; set; }
        public decimal TotalGSTAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string DateOfArrival { get; set; }
        public string SsoToken { get; set; }
        public int CancleTicketStatus { get; set; }
        public string CancleTicketStatusName { get; set; }
        public string Remark { get; set; }
        public decimal RefundAmount { get; set; }
        public int CancelStatus { get; set; }
        public int COVIDStatus { get; set; }
		public int RefundStatus { get; set; }
		public int TicketMemberBordingStatus { get; set; }
        public int isDFOApproved { get; set; }
        public string hdn_IAgreement { get; set; }

        //---Added By Mukesh Kumar Jangid On 18-09-2022

        public bool IsFullRefund { get; set; }
        public int TypeOfActions { get; set; }
        public int CancellationReason { get; set; }
        public string CancellationRemarks { get; set; }
        public bool IsForcefullyAppliedByAdmin { get; set; }
        //---Added By Mukesh Kumar Jangid On 18-09-2022
        #endregion

        #region MemberInformation
        public long MemberTableID { get; set; }
        public string MemberSLNo { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public string MemberGender { get; set; }
        public string MemberIdType { get; set; }
        public string MemberIdNo { get; set; }
        public string MemberTotalCamera { get; set; }
        public string MemberNationality { get; set; }

        public string CitizenRemarksVal { get; set; }
        public string TotalMemberFees { get; set; }
        public string TotalCameraFees { get; set; }
        public string TotalSafariFees { get; set; }
        public string VehicleRent { get; set; }
        public string GSTonVehicleRent { get; set; }
        public string GuideFee { get; set; }
        public string GSTGuideFee { get; set; }
        public string TicketAmount { get; set; }

        public string EmitraCharges { get; set; }
        public string GrandTotal { get; set; }
        public string SSOID { get; set; }

        // bank details for refund
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType { get; set; }
        public string AccountHolderName { get; set; }
        public bool ConfirmRefundByCitizen { get; set; }

        // bank details for refund

        #endregion
        public DataTable Is_AgentUsers(string ssoid, string IPAddress, int SetDayDiff)
        {
            DataTable dt = new DataTable();

            DALConn();
            SqlParameter[] parameters =
                {
            new SqlParameter("@SSO_Id", ssoid),
            new SqlParameter("@IPAddress", IPAddress),
            new SqlParameter("@SetDayDif", SetDayDiff),
            };

            Fill(dt, "sp_CheckAgentUsers", parameters);

            return dt;
        }
        public string GetRequestID(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@RequestID", RequestID),
            new SqlParameter("@Action", "GetRequest")            
            };

                Fill(dt, "GenerateRequest", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GenerateRequest" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            string RequestId = Convert.ToString(dt.Rows[0]["RequestID"]);
            return RequestId;
        }


		public DataTable IsOTPShow()
		{
			DataTable dt = new DataTable();
			try
			{
				DALConn();
				SqlCommand cmd = new SqlCommand("Sp_ISShowOTP", Conn);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "IsOTPShow", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
			}
			finally
			{
				Conn.Close();
			}
			return dt;
		}

		/// <summary>
		/// check ip address of kiosk user
		/// </summary>
		/// <param name="ipaddress"></param>
		/// <returns></returns>
		public DataTable Chk_DeptKioskIP(string ipaddress)
        {
            DataTable dtPlaces = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_TB_ChkDeptKioskUserIP", Conn);
                cmd.Parameters.AddWithValue("@IP_Address", ipaddress);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }
        /// <summary>
        /// Function return places
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Place(long UserID)
        {
            DataTable dtPlaces = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("TB_GetPlace", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.Parameters.AddWithValue("@DeptUser", Convert.ToString(HttpContext.Current.Session["IsDepartmentalKioskUser"]));
                cmd.Parameters.AddWithValue("@IsCurrentOrAdvance", HttpContext.Current.Session["CurrentBookingOrAdvanceBooking"].ToString());
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(UserID));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }

        public DataTable Select_PlaceAPI(long UserID)
        {
            DataTable dtPlaces = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("TB_GetPlace", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.Parameters.AddWithValue("@DeptUser", "False");
                cmd.Parameters.AddWithValue("@IsCurrentOrAdvance", '1');
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(UserID));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }

        /// <summary>
        ///  Function check for safari and accomodation avaliable
        /// </summary>
        /// <returns></returns>
        public DataSet chkSafariAccomo()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkSafariAccomoAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.Parameters.AddWithValue("@IsCurrentOrAdvance", HttpContext.Current.Session["CurrentBookingOrAdvanceBooking"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomo" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet chkSafariAccomoHDFD()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkSafariAccomoAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.Parameters.AddWithValue("@IsCurrentOrAdvance", HttpContext.Current.Session["CurrentBookingOrAdvanceBookingFDHD"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomo" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }


        public DataTable chkSafariAccomoDaysOpenBooking(long PlaceId)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_chkSafariAccomoDaysOpenBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomoDaysOpenBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataTable chkSafariAccomoDaysOpenBookingAPI(long PlaceId)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_chkSafariAccomoDaysOpenBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                //cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.Parameters.AddWithValue("@DeptUser", 0);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomoDaysOpenBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Function returns shift by place and zone
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Shift_By_PlacesZones()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Get_citizen_Place", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@ArrivalDate", DateOfArrival);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public async Task<DataTable> InvalidCaptchaLog(string Action, string ssoid, string IPAddress)
        {
            DataTable dt = new DataTable();

            //DALConn();
            SqlParameter[] parameters =
                {
            new SqlParameter("@Action", Action),
            new SqlParameter("@SSOId", ssoid),
            new SqlParameter("@IPAddress", IPAddress)
            };

            await Task.Run(() => Fill(dt, "spFinalSubmitLog", parameters));

            return dt;
        }


        public DataTable Select_Shift_By_PlacesZonesOnlineBooking()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Get_citizen_PlaceOnlineBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@ArrivalDate", DateOfArrival);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "TB_Get_citizen_PlaceOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_CheckBookingDurations()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_CheckBookingDurations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Date", DateOfArrival);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable Select_CheckBookingDurationsCheckSubmit(long PlaceId, DateTime DateOfArrival)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_CheckBookingDurations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Date", DateOfArrival);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable Save_OnlineBookingAutomatedScript(long UserID, string RequestID, string RequestLog)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_OnlineBookingAutomatedScript", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SAVEOnlineBookingAutomatedScript");
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@OnlineBookingLog", RequestID);
                cmd.Parameters.AddWithValue("@RequestedLog", RequestLog);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Save_OnlineBookingAutomatedScript" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable CheckOldRequestIdFD(long PlaceId, string OldRequestID, string OldRequestIDSecound, long UserID, string IsDeptUserOrCitizen, string DateOfArrival, string @MemberIDProof)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_CheckLastBookingDurationFullAndHaldDay", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@OldRequestID", OldRequestID);
                cmd.Parameters.AddWithValue("@OldRequestIDSecound", OldRequestIDSecound);
                cmd.Parameters.AddWithValue("@DateOfArrival", DateOfArrival);
                cmd.Parameters.AddWithValue("@MemberIDProof", @MemberIDProof);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@DeptUser", IsDeptUserOrCitizen);
                //cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOldRequestIdFD" + "_" + "Citizen", 1, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_CheckBookingDurationsCheckSubmitAPI(long PlaceId, DateTime DateOfArrival)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_CheckBookingDurations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Date", DateOfArrival);
                //cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.Parameters.AddWithValue("@DeptUser", 0);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function return accomodation type
        /// </summary>
        /// <returns></returns>
        public DataTable GetAccomodationType()
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@placeID", PlaceId)
            };
                Fill(dt, "TB_Citizen_SelectAccomodationType", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetAccomodationType" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }
        /// <summary>
        /// function to check ticket availabilty 
        /// </summary>
        /// <returns></returns>
        public DataTable CheckTicketAvailability()
        {
            // string Str = "";
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@VechileID", vehicleID);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //  Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public async Task<DataTable> CheckTicketAvailabilityWityPalaceOfWheel()
        {
            // string Str = "";

            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");

            DataTable dt = new DataTable("TicketAvaliable");
            
          
            
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkTicketAvailabilityWithPOWPlace", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                // cmd.Parameters.AddWithValue("@DateOfArrival",Convert.ToDateTime(ArrivalDate,cul));
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@VechileID", vehicleID);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                 await Task.Run(()=> da.Fill(dt));  /////Change by shaan on 06-01-2020
              

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailabilityWityPalaceOfWheel" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public async Task<DataTable> CheckTicketWaitingAvailability()
        {
            // string Str = "";

            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");

            DataTable dt = new DataTable("TicketAvaliableWaiting");
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_CheckWaitingLimit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                // cmd.Parameters.AddWithValue("@DateOfArrival",Convert.ToDateTime(ArrivalDate,cul));
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@VechileID", vehicleID);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                await Task.Run(() => da.Fill(dt));  /////Change by shaan on 06-01-2020


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailabilityWityPalaceOfWheel" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable InsertDeviceDetails(string DeviceUniqueID, string DeviceType, long Userid, string AppVersion)
        {
            // string Str = "";
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_InsertMobileDeviceDetails_ReqID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestId);
                cmd.Parameters.AddWithValue("@DeviceUniqueID", DeviceUniqueID);
                cmd.Parameters.AddWithValue("@DeviceType", DeviceType);
                cmd.Parameters.AddWithValue("@UserID", Userid);
                cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                cmd.Parameters.AddWithValue("@AppVersion", AppVersion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //  Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_InsertMobileDeviceDetails_ReqID" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable CheckTicketAvailabilityForVIP()
        {
            // string Str = "";
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkTicketAvailabilityForVIP", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@VechileID", vehicleID);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //  Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "TB_ChkTicketAvailabilityForVIP" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable CheckTicketAvailabilityForOnlineBooking()
        {
            // string Str = "";
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkTicketAvailabilityForOnlineBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@VechileID", vehicleID);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //  Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "TB_ChkTicketAvailabilityForOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// return vechile type
        /// </summary>
        /// <param name="VehicleCatID"></param>
        /// <returns></returns>
        public DataTable Select_vehicle(Int64 VehicleCatID)
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId)
            };
                Fill(dt, "TB_Citizen_Select_vehicle_by_CatID", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_vehicle" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }


        public DataTable Select_vehicleForVIP(Int64 VehicleCatID, long Shifttype)
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId),
             new SqlParameter("@ShiftType", Shifttype)
            };
                Fill(dt, "TB_Citizen_Select_vehicle_by_CatIDForVIPSeats", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_vehicleForVIP" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }

        public DataTable Select_vehicleForOnlineBooking(Int64 VehicleCatID, long Shifttype)
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId),
             new SqlParameter("@ShiftType", Shifttype)
            };
                Fill(dt, "TB_Citizen_Select_vehicle_by_CatIDOnlineBooking", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_vehicleForOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }
        /// <summary>
        /// function for member fees
        /// </summary>
        /// <returns></returns>
        public DataTable SelectMemberFees()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_select_Ticket_Camera_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Nationality", MemberNationality);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectMemberFees" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable SelectMemberFeesForVIPSeats(int ShiftType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_select_Ticket_Camera_FeesForVIPSeats", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Nationality", MemberNationality);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectMemberFeesForVIPSeats" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable SelectMemberFeesOnlineBooking(int ShiftType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_select_Ticket_Camera_FeesForOnlineBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@Nationality", MemberNationality);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectMemberFeesOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// function for accomodation fees
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Accomo_Fees_Availability()
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();

                SqlParameter[] parameters =
            {
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@PlaceID", PlaceId)
            };
                Fill(dt, "TB_Citizen_Accomo_Availability", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Accomo_Fees_Availability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }

        /// <summary>
        /// final submission of form
        /// </summary>
        /// <param name="dtm"></param>
        /// <param name="finalAmount"></param>
        /// <returns></returns>
        public async Task<DataTable> ValidateSSOMobile(long UserId)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ActionName","CheckValidMobileNo"),
                    new SqlParameter("@UserId", UserId)
                };
                await Task.Run(() => Fill_WithoutCommit(dt, "SP_ValidateSSOMobile", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "ValidateSSOMobile" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }

            return dt;
        }
        public async Task<DataTable> ValidateAllowedBookings(long UserId,string IPAddress,string VisitDate,int PlaceId,int ShiftId)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ActionName","AllowedBookings"),
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@IPAddress", IPAddress),
                    new SqlParameter("@VisitDate", VisitDate),
                    new SqlParameter("@PlaceId", PlaceId),
                     new SqlParameter("@ShiftId", ShiftId)

                };
                await Task.Run(() => Fill_WithoutCommit(dt, "SP_ValidateSSOMobile", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "ValidateSSOMobile" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }

            return dt;
        }
        public DataTable IsValidUser(long UserId)
        {
            DataTable dt = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("sp_IsValidUser", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public async Task<DataTable> Submit_TicketDetails(DataTable dtm, decimal finalAmount)
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@RequestID", RequestId),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@ShiftTime", ShiftTime),
            new SqlParameter("@VehicleID", vehicleID),
            new SqlParameter("@TotalMembers", TotalMember),
            new SqlParameter("@VehicleFee_TigerProject_PerMember", VehicleFees_TigerProject),
            new SqlParameter("@VehicleFee_Surcharge_PerMember", VehicleFees_Surcharge),
            new SqlParameter("@VehicleFee_Total_PerMember", VehicleFees_Total),
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@TotalRoom", TotalRoom),
            new SqlParameter("@RoomCharge", RoomCharge),
            new SqlParameter("@MemberDetail", dtm),
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@IPAddress", IPAddress),
            new SqlParameter("@kioskUserId",Convert.ToInt64(KioskUserId)),
            new SqlParameter("@TotalAmount",finalAmount),
            new SqlParameter("@ssoToken",SsoToken),
            new SqlParameter("@IAgreement",Int32.Parse(hdn_IAgreement))
            };
                //Fill(dt, "TB_BookTicketWithPalaceOfWheel", parameters);
                
                //await Task.Run(()=> Fill(dt, "TB_BookTicketWithPalaceOfWheel", parameters));
                 await Task.Run(() => Fill_WithoutCommit(dt, "TB_BookTicketWithPalaceOfWheel", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Submit_TicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }

        public DataTable Submit_TicketDetailsForVIPSeats(DataTable dtm, decimal finalAmount, int indiancount = 0, int foreignercount = 0, int indianstudentcount = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                /*DALConn()*/;
                SqlParameter[] parameters =
            {
            new SqlParameter("@RequestID", RequestId),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@ShiftTime", ShiftTime),
            new SqlParameter("@VehicleID", vehicleID),
            new SqlParameter("@TotalMembers", TotalMember),
            new SqlParameter("@VehicleFee_TigerProject_PerMember", VehicleFees_TigerProject),
            new SqlParameter("@VehicleFee_Surcharge_PerMember", VehicleFees_Surcharge),
            new SqlParameter("@VehicleFee_Total_PerMember", VehicleFees_Total),
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@TotalRoom", TotalRoom),
            new SqlParameter("@RoomCharge", RoomCharge),
            new SqlParameter("@MemberDetail", dtm),
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@IPAddress", IPAddress),
            new SqlParameter("@kioskUserId",Convert.ToInt64(KioskUserId)),
            new SqlParameter("@TotalAmount",finalAmount),
            new SqlParameter("@ssoToken",SsoToken),
            new SqlParameter("@IndianMemberCount",indiancount),
            new SqlParameter("@ForeignerMemberCount",foreignercount),
            new SqlParameter("@IndianStudentMemberCount",indianstudentcount),
            };
                //Fill(dt, "TB_BookTicketForVIPSeats", parameters);
                Fill(dt, "TB_BookTicketFinalSubmit", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Submit_TicketDetailsForVIPSeats" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }

        public DataTable Submit_TicketDetailsForOnlineBooking(DataTable dtm, decimal finalAmount, int indiancount = 0, int foreignercount = 0, int indianstudentcount = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@RequestID", RequestId),
            new SqlParameter("@PlaceID", PlaceId),
            new SqlParameter("@ZoneId", ZoneId),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@ShiftTime", ShiftTime),
            new SqlParameter("@VehicleID", vehicleID),
            new SqlParameter("@TotalMembers", TotalMember),
            new SqlParameter("@VehicleFee_TigerProject_PerMember", VehicleFees_TigerProject),
            new SqlParameter("@VehicleFee_Surcharge_PerMember", VehicleFees_Surcharge),
            new SqlParameter("@VehicleFee_Total_PerMember", VehicleFees_Total),
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@TotalRoom", TotalRoom),
            new SqlParameter("@RoomCharge", RoomCharge),
            new SqlParameter("@MemberDetail", dtm),
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@IPAddress", IPAddress),
            new SqlParameter("@kioskUserId",Convert.ToInt64(KioskUserId)),
            new SqlParameter("@TotalAmount",finalAmount),
            new SqlParameter("@ssoToken",SsoToken),
            new SqlParameter("@IndianMemberCount",indiancount),
            new SqlParameter("@ForeignerMemberCount",foreignercount),
            new SqlParameter("@IndianStudentMemberCount",indianstudentcount),
            new SqlParameter("@OLDRequestID",OldRequestID),
            new SqlParameter("@OLDRequestIDSecound",OldRequestIDSecound),
            new SqlParameter("@OLDIDProof",txt_OldIDProof),
            new SqlParameter("@IAgreement",Int32.Parse(hdn_IAgreement))

            };
                //Fill(dt, "TB_BookTicketForVIPSeats", parameters);
                Fill(dt, "TB_BookTicketFORTatkalHalfDayVIPBooning", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Submit_TicketDetailsForOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }
        /// <summary>
        /// function to update transaction status
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataTable UpdateTransactionStatus(string option, double EmitraAmount = 0, double Amount = 0, string TransactionTime = "")
        {
            DataTable dt = new DataTable();
            Int32 chk = 0;
            try
            {
                //DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {
                new SqlParameter("@RequestedId", RequestId),
                new SqlParameter("@TransactionId",transId),
                new SqlParameter("@TransactionStatus", Trn_Status_Code),
                new SqlParameter("@option", option),
                new SqlParameter("@EmitraAmount", EmitraAmount),
                 new SqlParameter("@Amount", Amount),
                 new SqlParameter("@TransactionTime", TransactionTime),
                   new SqlParameter("@kioskUserId",Convert.ToInt64(KioskUserId))
                };
                //Fill(dt, "TB_Common_UpdateTransactionStatus", parameters);
                //Fill(dt, "TB_Common_UpdateTransactionStatusWIthPalaceOfWheel", parameters);
                Fill_WithoutCommit(dt, "TB_Common_UpdateTransactionStatusWIthPalaceOfWheel", parameters);
                if (dt.Rows.Count > 0)
                {
                    
                    chk = Convert.ToInt32(dt.Rows[0][0].ToString());
                  
                    /////Send SMS and Email to User
                    //if(chk==0)
                    //{
                    //    DataTable oDtTicket=new DataTable();
                    //    SqlParameter[] Param =
                    //     {  
                    //         new SqlParameter("@Action", "GetTicketDetails"),
                    //         new SqlParameter("@RequestedId", RequestId)
                    //     };              
                    //     Fill(oDtTicket,"TB_GetTicketDetails", Param);

                    //    if(oDtTicket.Rows.Count>0)
                    //    {
                    //    SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
                    //    /////if Emitra Trn status is Fail then send Fail Email to Client
                    //    #region
                    //    string UserMailBody = Common.GenerateOnlinkTicketBody(Convert.ToString(oDtTicket.Rows[0]["PlaceName"]),Convert.ToString(oDtTicket.Rows[0]["DateofArrival"]),RequestId);

                    //    _objMailSMS.sendEMail("Transaction has failed of booked ticked :" + RequestId, UserMailBody, "amit17rajput@gmail.com", string.Empty);

                    //    string UserSmsBody = Common.SMSOnlineTicketBody(transId.ToString(), RequestId.ToString());
                    //   SMS_EMail_Services.sendSingleSMS("7568246030", UserSmsBody);
                    //#endregion

                    // }

                    //}
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
            //return chk;
        }

        public int UpdateTransactionStatusForVIP(string option, double EmitraAmount = 0, double Amount = 0, string TransactionTime = "")
        {
            DataTable dt = new DataTable();
            Int32 chk = 0;
            try
            {
                //DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {
                new SqlParameter("@RequestedId", RequestId),
                new SqlParameter("@TransactionId",transId),
                new SqlParameter("@TransactionStatus", Trn_Status_Code),
                new SqlParameter("@option", option),
                new SqlParameter("@EmitraAmount", EmitraAmount),
                 new SqlParameter("@Amount", Amount),
                 new SqlParameter("@TransactionTime", TransactionTime),
                   new SqlParameter("@kioskUserId",Convert.ToInt64(KioskUserId))
                };
                Fill(dt, "TB_Common_UpdateTransactionStatusFORVIP", parameters);
                if (dt.Rows.Count > 0)
                {
                    chk = Convert.ToInt32(dt.Rows[0][0].ToString());

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatusForVIP" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return chk;
        }

        /// <summary>
        /// function to update transaction status
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public int UpdateEmitraResponse(string RequestId, string EmitraResponse)
        {
            Int32 chk = 0;
            try
            {
                //DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {
                new SqlParameter("@RequestId", RequestId),
                new SqlParameter("@EmitraResponse",EmitraResponse),
                new SqlParameter("@Enterby", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()))
                };
                chk = ExecuteNonQuery("TB_InsertEmitraResponse", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return chk;
        }


        public DataSet Save_UserCovidOptionalBooking(CovidBooking oCovid)
        {
            DataSet dt = new DataSet();
            string ActionName = string.Empty;
            if (string.IsNullOrEmpty(oCovid.ApprovedVisitDate) && !string.IsNullOrEmpty(oCovid.FirstDate))
            {
                 ActionName = "SaveOptionalCovidBooking";
            }
            else
            {
                ActionName = "UpdateOptionalCovidBooking";
            }
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", ActionName),
            new SqlParameter("@TicketId", Encryption.decrypt(oCovid.TicketId)),
            new SqlParameter("@RequestId", oCovid.RequestID),
            new SqlParameter("@FirstDate", oCovid.FirstDate),
            new SqlParameter("@SecondDate", oCovid.SecondDate),
            new SqlParameter("@ThirdDate", oCovid.ThirdDate),
            new SqlParameter("@ApprovedVisitDate", oCovid.ApprovedVisitDate),
            new SqlParameter("@ShiftId", oCovid.ShiftId),
			new SqlParameter("@PlaceId",oCovid.PlaceId),
			new SqlParameter("@isDFOApproved", oCovid.isDFOApproved),
            new SqlParameter("@IsActive",oCovid.isActive),
            new SqlParameter("@Remark",oCovid.Remark),
            new SqlParameter("@ZoneID",oCovid.ZoneID),
            new SqlParameter("@BookedQuota",oCovid.BookedQuota),
            new SqlParameter("@QuotaType",oCovid.QuotaType),

			new SqlParameter("@userId",Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()))
            };
                Fill(dt, "spGetCovidTicketDetails", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Save_UserCovidOptionalBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return dt;
        }

        public DataSet Get_UserCovidOptionalBookingList(CovidBooking oCovid)
        {
            DataSet ds = new DataSet();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "GetOptionalCovidBookingList"),
            new SqlParameter("@RequestId",oCovid.RequestID),
            new SqlParameter("@ShiftId",oCovid.ShiftId)
            };
                Fill(ds, "spGetCovidTicketDetails", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_UserCovidOptionalBookingList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }


        /// <summary>
        /// get time
        /// </summary>
        /// <returns></returns>
        public DataTable GetServerTimeForCurrentBooking()
        {
            DataTable dt = new DataTable();
            DALConn();
            SqlCommand cmd = new SqlCommand("SP_GetServerTime", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// print ticket
        /// </summary>
        /// <returns></returns>
        public DataTable Select_BookedTicket()
         {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Citizen_Select_BookedTicket", Conn);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        #region added by shaan 03-02-2021
        public DataSet WildLifebookedTicketList(WildLifeBookingFilterModel Model)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Citizen_Select_BookedTicket_List", Conn);
                cmd.Parameters.AddWithValue("@DateType", Model.DateType);
                cmd.Parameters.AddWithValue("@FromDate", Model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", Model.ToDate);
                cmd.Parameters.AddWithValue("@Place", Model.Place);
                cmd.Parameters.AddWithValue("@TypeOfBooking", Model.TypeOfBooking);
                cmd.Parameters.AddWithValue("@Status", Model.Status);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.Parameters.AddWithValue("@StartRow", Model.StartRow);
                cmd.Parameters.AddWithValue("@FetchRowsNext", Model.FetchRowsNext);
                //  cmd.Parameters.AddWithValue("@UserCDR", UserCDR),
                cmd.Parameters.AddWithValue("@Search", Model.search);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "WildLifebookedTicketList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        #endregion
        #region added by Mukesh 16-09-2022
        public DataSet WildLifebookedTicketListForAdmin(WildLifeBookingFilterModel Model)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Admin_Select_BookedTicket_List", Conn);
                cmd.Parameters.AddWithValue("@DateType", Model.DateType);
                cmd.Parameters.AddWithValue("@FromDate", Model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", Model.ToDate);
                cmd.Parameters.AddWithValue("@Place", Model.Place);
                cmd.Parameters.AddWithValue("@TypeOfBooking", Model.TypeOfBooking);
                cmd.Parameters.AddWithValue("@Status", Model.Status);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.Parameters.AddWithValue("@StartRow", Model.StartRow);
                cmd.Parameters.AddWithValue("@FetchRowsNext", Model.FetchRowsNext);
                //  cmd.Parameters.AddWithValue("@UserCDR", UserCDR),
                cmd.Parameters.AddWithValue("@Search", Model.search);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "WildLifebookedTicketList" + "_" + "Admin", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        #endregion
        public DataTable Select_BookedTicketStatus(long TicketId = 0, string RequestId = "")
            {
                DataTable dt = new DataTable();
                try
                {
                    DALConn();

                    SqlCommand cmd = new SqlCommand("TB_Citizen_Select_BookedTicket_Status", Conn);
                    cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                    cmd.Parameters.AddWithValue("@TicketId", TicketId);
                    cmd.Parameters.AddWithValue("@RequestId", RequestId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                }
                finally
                {
                    Conn.Close();
                }
                return dt;
            }

            public DataTable Select_BookedTicketDFOStatusWise(Int64 TicketID)
            {
                DataTable dt = new DataTable();
                try
                {
                    DALConn();

                    SqlCommand cmd = new SqlCommand("spGetCovidTicketDetails", Conn);
                    cmd.Parameters.AddWithValue("@Action", "CheckCovidTicketDFOStautus");
                    cmd.Parameters.AddWithValue("@TicketID", TicketID);
                    //cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                }
                finally
                {
                    Conn.Close();
                }
                return dt;
            }

        public DataTable GetResendCancelationDetails(string ActionName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GETTicketBookingUserDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetResendCancelationDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_BookedTicketAPI(long UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Citizen_Select_BookedTicket", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Mobile WEB API" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_BookedTicketForVIPSeats(string Action, int RowCount)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("TB_Citizen_Select_BookedTicketForVIPBooking", Conn);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.Parameters.AddWithValue("@RowCount", RowCount);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Get_BookedTicketDetails(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Get_BookedTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookedTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        #region "Delay Ticket for one hour"
        public DataTable Select_DelayStatus(long TicketId)
        {
            DataTable ds = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@TicketID", TicketId)
            };
                Fill(ds, "spDelayforOneHour", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "spDelayforOneHour" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }
        #endregion




        /// <summary>
        /// Ticket print
        /// </summary>
        /// <returns></returns>
        public DataSet Select_CovidTicketData()
        {
            DataSet ds = new DataSet();
            try
            {
                //DALConn();

                SqlParameter[] parameters =
            {
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "TB_Covid_Citizen_SelecTicketDetail", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Covid" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }






        /// <summary>
        /// Ticket print
        /// </summary>
        /// <returns></returns>
        public DataSet Select_TicketData_For_Print()
        {
            DataSet ds = new DataSet();
            try
            {
                //DALConn();

                SqlParameter[] parameters =
            {
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "TB_Citizen_SelecTicketDetail", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }
        public DataTable Get_TicketId(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Get_TicketId", Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_TicketId" + "_" + "Mobile Web Api", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;



        }

        /// <summary>
        /// Get details of ticket if ticket success by arvind kumar sharma 01122016
        /// </summary>
        /// <returns></returns>
        public DataTable Get_BookedTicketDetails(string RequestID, string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Get_BookedTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookedTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable UpdateFailedTicketStatus(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("sp_UpdateFailedTicketStatus", Conn);
                cmd.Parameters.AddWithValue("@RequestId", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateFailedTicketStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Dynamic T&C
        /// </summary>
        /// <param name="ticketid"></param>
        /// <returns></returns>
        public DataTable Select_TermandConditionbyTicketId(long ticketid)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_TermsAndCondition_master", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetTCbyTicketId");
                cmd.Parameters.AddWithValue("@TicketId", ticketid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookedTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;



        }


        /// <summary>
        /// function to update transaction status
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public int UpdateEmitraResponse(string RequestId, string ETransactionType, string EmitraResponse)
        {
            Int32 chk = 0;
            try
            {
                //DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {
                new SqlParameter("@RequestId", RequestId),
                  new SqlParameter("@ETransactionType",ETransactionType),
                new SqlParameter("@EmitraResponse",EmitraResponse),
                new SqlParameter("@Enterby", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()))
                };
                chk = ExecuteNonQuery("TB_InsertEmitraResponse", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return chk;
        }






        /// <summary>
        /// Get Head Wise Amount Of Wild Life Tickets
        /// </summary>
        /// <returns></returns>
        public DataSet Get_HeadWiseAmountOfWildLifeTickets(string Action, string RequestID, char IsInChargeOrCitizen = 'C')
        {
            DataSet dt = new DataSet();
            SqlCommand cmd = null;
            try
            {
                DALConn();

                if (Action == "PurchaseNurseryProduce")
                {
                    cmd = new SqlCommand("SP_HeadWiseAmountOfDifferentOnlineBookingForeNurseryProduce", Conn);
                    cmd.Parameters.AddWithValue("@IsInChargeOrCitizen", IsInChargeOrCitizen);
                }
                else
                {
                    cmd = new SqlCommand("SP_HeadWiseAmountOfDifferentOnlineBooking", Conn);
                }


                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_HeadWiseAmountOfWildLifeTickets" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public decimal Get_ReturningAmountBalance()
        {
            decimal RemaingReturningFund = 0;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();
                cmd = new SqlCommand("SP_ReturningAmountTransactions", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetReturningBalance");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        RemaingReturningFund = Convert.ToDecimal(dt.Rows[0]["RemaingReturningFund"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_ReturningAmountTransactions" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return RemaingReturningFund;
        }
        public int SaveReturningAmount(string requestId, string ToBeRevenueHeadEntryFeeCode, string ToBeRevenueHeadEcoDevelopmentSurchargeCode, string ToBeRevenueHeadFOUNDATIONCode, string ToBeHeadVehicleRentAndGuidFees, decimal TigerProject, decimal Surcharge, decimal Foundation, decimal VehicleRentandGuidFees)
        {
            int insertionStatus = 0;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();
                cmd = new SqlCommand("SP_ReturningAmountTransactions", Conn);
                cmd.Parameters.AddWithValue("@Action", "AddFundAgainstPartialRefund");
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@ToBeRevenueHeadEntryFeeCode", ToBeRevenueHeadEntryFeeCode);
                cmd.Parameters.AddWithValue("@ToBeRevenueHeadEcoDevelopmentSurchargeCode", ToBeRevenueHeadEcoDevelopmentSurchargeCode);
                cmd.Parameters.AddWithValue("@ToBeRevenueHeadFOUNDATIONCode", ToBeRevenueHeadFOUNDATIONCode);
                cmd.Parameters.AddWithValue("@ToBeHeadVehicleRentAndGuidFees", ToBeHeadVehicleRentAndGuidFees);
                cmd.Parameters.AddWithValue("@TigerProject", TigerProject);
                cmd.Parameters.AddWithValue("@Surcharge", Surcharge);
                cmd.Parameters.AddWithValue("@Foundation", Foundation);
                cmd.Parameters.AddWithValue("@VehicleRentandGuidFees", VehicleRentandGuidFees);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        insertionStatus = Convert.ToInt16(dt.Rows[0]["SaveStatus"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_ReturningAmountTransactions" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return insertionStatus;
        }
        public int UpdatePartialAddFundEmitraStatus(string requestId,string EmitraStatus,bool PayStatus)
        {
            int updateStaus = 0;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                DALConn();
                cmd = new SqlCommand("SP_ReturningAmountTransactions", Conn);
                cmd.Parameters.AddWithValue("@Action", "UpdateEmitraStatus");
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@EmitraStatus", EmitraStatus);
                cmd.Parameters.AddWithValue("@PayStatus", (PayStatus==true?1:0));
                                    
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        updateStaus = Convert.ToInt32(dt.Rows[0]["UpdateStatus"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_ReturningAmountTransactions" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return updateStaus;
        }
        public async Task<DataTable> CheckAvailability_On_BeforeAndAfterPay(long UserId, string RequestID, bool IsBeforePay)
        {
            int SeatAvailableCount = -1;
            string customMsg = "";
            DataTable dt = new DataTable();
            try
            {
                // DALConn();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@RequestID", RequestID),
                    new SqlParameter("@IsBeforePay", (IsBeforePay == false ? 0 : 1)),
                };
                await Task.Run(() => Fill_WithoutCommit(dt, "SP_FB_CheckAvailability_On_BeforeAndAfterPay", parameters));
                //int res = cmd.ExecuteNonQuery();
                //SeatAvailableCount = Convert.ToInt32(dt.Rows[0]["SeatAvailableCount"].ToString());
                //customMsg = dt.Rows[0]["Msg"].ToString();
                //SeatAvailableCount = Convert.ToInt32(cmd.Parameters["@SeatAvailableCount"].Value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_FB_CheckAvailability_On_BeforeAndAfterPay" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return dt;
        }
        public async Task<DataTable> GetPlaceId(string RequestID)
        {

            DataTable dt = new DataTable();
            try
            {
                // DALConn();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@RequestedId", RequestID)
                };
                await Task.Run(() => Fill_WithoutCommit(dt, "SP_GetPlaceId", parameters));
                //int res = cmd.ExecuteNonQuery();

                //SeatAvailableCount = Convert.ToInt32(cmd.Parameters["@SeatAvailableCount"].Value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_GetPlaceId" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return dt;
        }
        public async Task<string> CheckRequestStatus(long UserId, string RequestID)
        {
            string customMsg = "";
            DataTable dt = new DataTable();
            try
            {
                // DALConn();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@RequestID", RequestID),
                };
                await Task.Run(() => Fill_WithoutCommit(dt, "SP_CheckRequestStatus", parameters));
                //int res = cmd.ExecuteNonQuery();              
                customMsg = dt.Rows[0]["Msg"].ToString();
                //SeatAvailableCount = Convert.ToInt32(cmd.Parameters["@SeatAvailableCount"].Value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_CheckRequestStatus" , 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return customMsg;
        }

        public DataTable Get_BookTicket_ForRefundProcessMemberWise(string TicketID)
        {
            DataTable ds = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
                new SqlParameter("@Action", "ListMemberWise"),
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "TB_BookTicket_RefundProcess", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookTicket_RefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }
        public DataTable SubmitFor_BookTicket_ForRefundProcessMemberWise(BookOnTicket cs)
        {
            DataTable ds = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "PROCESSTOREFUNDMEMBERWISE"),
            new SqlParameter("@TicketID", cs.RequestId),
             new SqlParameter("@AccountNo", cs.AccountNo),
              new SqlParameter("@BankName", cs.BankName),
               new SqlParameter("@BranchName", cs.BranchName),
                new SqlParameter("@IFSCCode", cs.IFSCCode),
                 new SqlParameter("@AccountType", cs.AccountType),
                  new SqlParameter("@AccountHolderName", cs.AccountHolderName),
                   new SqlParameter("@MemberID", cs.MemberSLNo),
                    new SqlParameter("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"])),

            };
                Fill(ds, "TB_BookTicket_RefundProcess", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitFor_BookTicket_ForRefundProcessMemberWise", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }
        public DataTable Get_BookTicket_ForRefundProcess(string TicketID, int IsFullRefund = 0, string IsCitizen = "")
        {
            // IsFullRefund=0 As per forest policy return 25,50,75,100 return refund
            // IsFullRefund=1 Full Refund
            // IsFullRefund=2 Cancelled Without Refund

            string action = "List";
            if (IsCitizen == "Citizen")
                action = "ListCitizen";

            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@TicketID", TicketID),
                new SqlParameter("@IsFullRefund", IsFullRefund)
            };
                Fill(ds, "TB_BookTicket_RefundProcess", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookTicket_RefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        //public DataTable Get_BookTicket_ForRefundProcess(string TicketID)
        //{
        //    DataTable ds = new DataTable();
        //    try
        //    {
        //        //DALConn();
        //        SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Action", "List"),
        //    new SqlParameter("@TicketID", TicketID)
        //    };
        //        Fill(ds, "TB_BookTicket_RefundProcess", parameters);

        //    }

        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, "Get_BookTicket_RefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

        //    }
        //    //finally
        //    //{
        //    //    Conn.Close();
        //    //}
        //    return ds;
        //}

		public DataTable Get_BookTicket_ForRefund(string TicketID)
		{
			DataTable ds = new DataTable();
			try
			{
				//DALConn();
				SqlParameter[] parameters =
			{
				new SqlParameter("@Action", "TcketDataForRefund"),
			new SqlParameter("@TicketID", TicketID)
			};
				Fill(ds, "TB_BookTicket_RefundProcess", parameters);

			}

			catch (Exception ex)
			{
				new Common().ErrorLog(ex.Message, "Get_BookTicket_RefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

			}
			//finally
			//{
			//	Conn.Close();
			//}
			return ds;
		}
		public DataTable SubmitFor_BookTicket_ForRefundProcess(BookOnTicket cs)
        {
            DataTable ds = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "PROCESSTOREFUND"),
            new SqlParameter("@TicketID", cs.RequestId),
             new SqlParameter("@AccountNo", cs.AccountNo),
              new SqlParameter("@BankName", cs.BankName),
               new SqlParameter("@BranchName", cs.BranchName),
                new SqlParameter("@IFSCCode", cs.IFSCCode),
                 new SqlParameter("@AccountType", cs.AccountType),
                  new SqlParameter("@AccountHolderName", cs.AccountHolderName),

            };
                Fill(ds, "TB_BookTicket_RefundProcess", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookTicket_RefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }


		public DataTable SubmitFor_BookTicket_ForRefund(BookOnTicket cs)
		{
			DataTable ds = new DataTable();
			try
			{
				//DALConn();
				SqlParameter[] parameters =
			{
			new SqlParameter("@Action", "REFUND"),
			new SqlParameter("@TicketID", cs.RequestId),
			 new SqlParameter("@AccountNo", cs.AccountNo),
			  new SqlParameter("@BankName", cs.BankName),
			   new SqlParameter("@BranchName", cs.BranchName),
				new SqlParameter("@IFSCCode", cs.IFSCCode),
				 new SqlParameter("@AccountType", cs.AccountType),
				  new SqlParameter("@AccountHolderName", cs.AccountHolderName),

			};
				Fill(ds, "TB_BookTicket_RefundProcess", parameters);

			}

			catch (Exception ex)
			{
				new Common().ErrorLog(ex.Message, "Get_BookTicket_Refund", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

			}
			//finally
			//{
			//	Conn.Close();
			//}
			return ds;
		}


		#region Chnage BY Suraj Singh
		public DataSet SelectFeesForVIPSeats(int ShiftType)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetFeesForVIPSeats", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectFeesForVIPSeats" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet SelectFeesForOnlineBooking(int ShiftType)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetFeesForVIPSeatsOnlineBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                cmd.Parameters.AddWithValue("@vehicleID", vehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectFeesForVIPSeats" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public bool CheckOpenForDepartmentUser(int placeId)
        {
            bool result = false;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_MaintainMemberDetail_OnlineBooking", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.CommandType = CommandType.StoredProcedure;
                object value = cmd.ExecuteScalar();
                result = Convert.ToBoolean(value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOpenForDepartmentUser" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return result;
        }
        #endregion

        public DataTable SubmitFor_BookTicket_ForRefundProcessResend(BookOnTicket cs, string ActionName)
        {
            DataTable ds = new DataTable();
            try
            {
                //DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", ActionName),
            new SqlParameter("@TicketID", cs.RequestId),
             new SqlParameter("@AccountNo", cs.AccountNo),
              new SqlParameter("@BankName", cs.BankName),
               new SqlParameter("@BranchName", cs.BranchName),
                new SqlParameter("@IFSCCode", cs.IFSCCode),
                 new SqlParameter("@AccountType", cs.AccountType),
                  new SqlParameter("@AccountHolderName", cs.AccountHolderName),
                   new SqlParameter("@MemberID", cs.IsPartialOrFullCancelation),
                  new SqlParameter("@UserID",Convert.ToInt64(HttpContext.Current.Session["UserId"])),
            };
                Fill(ds, "TB_BookTicket_RefundProcess", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitFor_BookTicket_ForRefundProcessResend", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            //finally
            //{
            //    Conn.Close();
            //}
            return ds;
        }

        #region VIP online Wildlfe Booking by Rajveer
        public DataTable Select_PlaceForVIPOnlineBooking(int option)
        {
            DataTable dtPlaces = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("TB_GetPlace", Conn);
                cmd.Parameters.AddWithValue("@Option", option);
                cmd.Parameters.AddWithValue("@DeptUser", Convert.ToString(HttpContext.Current.Session["IsDepartmentalKioskUser"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserID"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_PlaceForVIPOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }

        public DataSet chkSafariAccomoForVIPOnlineBooking(int PlaceIds)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkSafariAccomoAvailabilityVIPOnlineBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceIds);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomoForVIPOnlineBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet chkSafariAccomoForVIPOnlineBookingFDFD(int PlaceIds)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("TB_ChkSafariAccomoAvailabilityVIPOnlineBookingHDFD", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceIds);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.Parameters.AddWithValue("@IsCurrentOrAdvance", HttpContext.Current.Session["CurrentBookingOrAdvanceBookingFDHD"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomoForVIPOnlineBookingFDFD" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable SubmitReviewApprovalRemark(string Action, int IsActive, string CitizenRemarks, string RequestID, string AdminRemarks, string AdminDate, long UserID)
        {
            // string Str = "";
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingReviewApproveRemarks", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@CitizenRemarks", CitizenRemarks);
                cmd.Parameters.AddWithValue("@AdminRemarks", AdminRemarks);
                cmd.Parameters.AddWithValue("@AdminVisitdate", AdminDate);
                cmd.Parameters.AddWithValue("@ApproveRejectStatus", IsActive);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //  Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitReviewApprovalRemark" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        #endregion


        #region Zoo Booking API For Emitra Plus 31-12-2018
        public int UpdateEmitraResponseForApi(string RequestId, string ETransactionType, string EmitraResponse, long UserId)
        {
            Int32 chk = 0;
            try
            {
                //DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {
                new SqlParameter("@RequestId", RequestId),
                  new SqlParameter("@ETransactionType",ETransactionType),
                new SqlParameter("@EmitraResponse",EmitraResponse),
                new SqlParameter("@Enterby",UserId )
                };
                chk = ExecuteNonQuery("TB_InsertEmitraResponse", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, UserId);
            }
            //finally
            //{
            //    Conn.Close();
            //}
            return chk;
        }
        #endregion
    }
    public class MemberInfo
    {
        public string MemberSLNo { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public string MemberGender { get; set; }
        public string MemberIdType { get; set; }
        public string MemberIdNo { get; set; }
        public string MemberTotalCamera { get; set; }
        public string MemberNationality { get; set; }
        public string TotalPerMemberFees { get; set; }
        public string TotalPerMemberCameraFees { get; set; }


        public string Seat_NO { get; set; }
        public string Room_No { get; set; }
        public string PNR_NO { get; set; }

        public decimal MemberFees_TigerProject { get; set; }
        public decimal MemberFees_Surcharge { get; set; }
        public decimal TRDF { get; set; }
        public decimal CameraFees_TigerProject { get; set; }
        public decimal CameraFees_Surcharge { get; set; }
        public decimal FinalAmountTobePaid { get; set; }

        //Added by shaan 30-03-2021
        public decimal Fees_TigerProjectHalfDayFullDayCharge { get; set; }
        public decimal Fee_SurchargeHalfDayFullDayCharge { get; set; }
        //END





        // added by arvind sharma 25/07/2017
        // begin
        public decimal Vehicle_TRDF { get; set; }
        public decimal GuidFee_TRDF { get; set; }
        public decimal BoardingVehicleFee { get; set; }
        public decimal BoardingVehicleFeeGSTPercentage { get; set; }
        public decimal BoardingVehicleFeeGSTAmount { get; set; }

        public decimal BoardingGuideFee { get; set; }
        public decimal BoardingGuideFeeGSTPercentage { get; set; }
        public decimal BoardingGuideFeeGSTAmount { get; set; }

        public decimal TotalBoardingFee { get; set; }



        // end
        // added by arvind sharma 25/07/2017
        public int Isactive { get; set; }



    }

    public class CheckPreviousBookingModel
    {
        public string msg { get; set; }
        public string Name { get; set; }
        public string IDNo { get; set; }
        public string Gender { get; set; }
        public string IDType { get; set; }
        public string MemberType { get; set; }
        public int Status { get; set; }
    }

    public class SsoDetailsToken
    {
        public string sAMAccountName { get; set; }
        public List<string> Roles { get; set; }

    }

    public class OnlineBookingApproveRejectLogModel
    {
        public string AdminRemarks { get; set; }
        public string USerName { get; set; }
        public string StatusName { get; set; }
    }

    public class SaveOnlineBookingApproveReject : DAL
    {
        public int SNO { get; set; }
        public string RequestID { get; set; }

        public string CitizenRemarks { get; set; }

        public string AdminRemarks { get; set; }

        public string CitizenVisitDate { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public long TicketID { get; set; }

        public int PlaceID { get; set; }

        public string PlaceName { get; set; }
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public int ShiftTime { get; set; }
        public string ShiftTimeName { get; set; }
        public string DateOfArrival { get; set; }

        public int TotalMembers { get; set; }

        public decimal TotalFees { get; set; }

        public decimal AmountTobePaid { get; set; }

        public decimal AmountWithServiceCharges { get; set; }

        public string EmitraTransactionId { get; set; }
        public string Reserve_Status { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string IDType { get; set; }

        public string IDNo { get; set; }

        public string Nationality { get; set; }

        public string MemberType { get; set; }

        public int Status { get; set; }
        public string msg { get; set; }

        public DataTable GetDataOnlineBookingApproveReject()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingReviewApproveRemarks", Conn);
                cmd.Parameters.AddWithValue("@Action", "LIST");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetDataOnlineBookingApproveReject" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataSet GetDataOnlineBookingApproveReject(string RequestID, long UserID, int IsCitizenOrAdmin)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingReviewApproveRemarks", Conn);
                cmd.Parameters.AddWithValue("@Action", "DetailsOnRequestID");
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ISCitizen", IsCitizenOrAdmin);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetDataOnlineBookingApproveReject" + "_" + "Citizen", 1, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable SaveDataOnlineBookingApproveReject(string RequestID, int ApproveRejectStatus, string AdminVisitdate, string AdminRemarks, long UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingReviewApproveRemarks", Conn);
                cmd.Parameters.AddWithValue("@Action", "SaveApproveReject");
                cmd.Parameters.AddWithValue("@ApproveRejectStatus", ApproveRejectStatus);
                cmd.Parameters.AddWithValue("@AdminRemarks", AdminRemarks);
                cmd.Parameters.AddWithValue("@AdminVisitdate", AdminVisitdate);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SaveDataOnlineBookingApproveReject" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

       

    }

	


	public class WildLifeRejectAppoveReport
    {
        public List<GetWildLifeRejectApprovedReportList> Pending { get; set; }
        public List<GetWildLifeRejectApprovedReportList> Failed { get; set; }
        public List<GetWildLifeRejectApprovedReportList> Success { get; set; }
        public List<GetWildLifeRejectApprovedReportList> Approved { get; set; }
        //public WildLifeRejectAppoveReport()
        //{
        //    Pending = new List<GetWildLifeRejectApprovedReportList>();
        //    Failed = new List<GetWildLifeRejectApprovedReportList>();
        //    Success = new List<GetWildLifeRejectApprovedReportList>();
        //    Approved = new List<GetWildLifeRejectApprovedReportList>();
        //}
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PlaceID { get; set; }
        public int Shift { get; set; }


    }

    public class GetWildLifeRejectApprovedReportList
    {
        public string ZoneName { get; set; }
        public string PlaceName { get; set; }
        public int PlaceID { get; set; }
        public int ZoneID { get; set; }
        public int ShiftTime { get; set; }
        public string ShiftTimeName { get; set; }
        public string RequestID { get; set; }
        public string CitizenVisitDate { get; set; }
        public string DateOfArrival { get; set; }
    }
    public class AutoCompleteData
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public class WildLifeRejectApprovedReportRepo : DAL
    {
        public DataSet GetWildLifeRejectApprovedReportData(WildLifeRejectAppoveReport model)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("sp_GetWildLifeRejectApprovedReportData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", 1);
                cmd.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(model.FromDate) ? null : model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(model.ToDate) ? null : model.ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", model.PlaceID);
                cmd.Parameters.AddWithValue("@ShiftName", model.Shift);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            //return ds;
        }
    }



}