using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.MIS
{
    [Serializable]
    public class MISCommonModel : DAL
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Place { get; set; }
        public string SHIFT_TYPE { get; set; }
        public string BOOKING_TYPE { get; set; }
        public string PLACE_NAME { get; set; }
        public string SHIFTName { get; set; }
        public string DATETYPE { get; set; }

       

        public string ReportType { get; set; }
        public string Modules { get; set; }
        public string SSOID { get; set; }

        public string TRNS_Status { get; set; }

        public string IP_ADDRESS { get; set; }

        public string ModeOfBooking { get; set; }

        //Added by Nitin 08/11/2017
        public List<MISTicketTransactionDetails> MISTicketTransactionDetails { get; set; }
        public string DownloadExcel { get; set; }
        public string TimeDifference { get; set; }
        //Added by Nitin 08/11/2017

        public string TicketId { get; set; }
        public string RemarkStatus { get; set; }
        public string Remark { get; set; }

		public string RequestId { get; set; }
		public string RefundStatus { get; set; }
		public string PaymentReferenceId { get; set; }
		public string PaymentMode { get; set; }
		public string DateOfRefund { get; set; }


		public DataTable GetBoardingIssueStatusData(string FromDate, string ToDate, string Place, string SHIFT_TYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_BoardingIssueStatusReports", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public DataTable GETALLSSOIDsByPlaceDateShift(string DateOfBooking, string Place, string SHIFT_TYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_DayCloserReport", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "GETALLSSOIDsByPlaceDateShift");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DateOfBooking);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public DataTable GETALLIPsBySSOID(string DateOfBooking, string Place, string SHIFT_TYPE, string SSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_DayCloserReport", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "GETALLIPsBySSOID");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DateOfBooking);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public DataTable GetCurrentBookingDayColserDetails(string ACTION, string FromDate, string ToDate, string Place, string SHIFT_TYPE, string SSOID, string IP_ADDRESS)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_DayCloserReport", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);

                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@IP_ADDRESS", IP_ADDRESS);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        #region "For Citizen Inventory"
        public DataTable GetCitizenCurrentBookingRemainingSeatsDetails(string ACTION, string DateOfArrival, string Place, string SHIFT_TYPE, long USERID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ForCitizenGetRemainingSeatReportForWildlifeBooking", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@PlaceId", Place);
                cmd.Parameters.AddWithValue("@ShiftID", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DateOfArrival", DateOfArrival);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        #endregion

        public DataTable GetCurrentBookingRemainingSeatsDetails(string ACTION, string DateOfArrival, string Place, string SHIFT_TYPE, long USERID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetRemainingSeatReportForWildlifeBooking", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@PlaceId", Place);
                cmd.Parameters.AddWithValue("@ShiftID", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DateOfArrival", DateOfArrival);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

		public DataTable saveRefundedData(MISCommonModel model)
		{
			try
			{
				DALConn();
				DataTable dt = new DataTable();
				SqlCommand cmd = new SqlCommand("MIS_ListTicketRefundDetails", Conn);
				cmd.Parameters.AddWithValue("@ACTION", "SaveRefundedInformation");
				cmd.Parameters.AddWithValue("@RequestId",Convert.ToString(Encryption.decrypt(model.RequestId)));
				cmd.Parameters.AddWithValue("@RefundStatus", model.RefundStatus);
				cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
				cmd.Parameters.AddWithValue("@PaymentReferenceId", model.PaymentReferenceId);
				cmd.Parameters.AddWithValue("@DateOfRefund", model.DateOfRefund);
				cmd.Parameters.AddWithValue("@Remark", model.Remark);
				cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.SelectCommand.CommandTimeout = 500000000;

				da.Fill(dt);

				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Conn.Close();
			}
		}

        public DataTable saveRefundedDataJhalana(MISCommonModel model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("TB_BookTicket_NP_RefundProcess", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "SaveRefundedInformation");
                cmd.Parameters.AddWithValue("@RequestId", Convert.ToString(Encryption.decrypt(model.RequestId)));
                cmd.Parameters.AddWithValue("@RefundStatus", model.RefundStatus);
                cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                cmd.Parameters.AddWithValue("@PaymentReferenceId", model.PaymentReferenceId);
                cmd.Parameters.AddWithValue("@DateOfRefund", model.DateOfRefund);
                cmd.Parameters.AddWithValue("@Remark", model.Remark);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetCurrentBookingZoneDetails(string ACTION, string DateOfArrival, string Place, string SHIFT_TYPE, long USERID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetZoneWiseReportForWildlifeBooking", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@PlaceId", Place);
                cmd.Parameters.AddWithValue("@ShiftID", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DateOfArrival", DateOfArrival);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetTicketBookingSummaryRequestIDWise(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string BOOKING_TYPE, string DATETYPE, string TRNS_Status, string ModeOfBooking)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingSummaryRequestID", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", "BoardingPass");
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.Parameters.AddWithValue("@TRNS_Status", TRNS_Status);
                cmd.Parameters.AddWithValue("@BOOKINGMODE", ModeOfBooking);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetBookingData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string BOOKING_TYPE, string DATETYPE, string TRNS_Status, string BOOKINGMODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", "BoardingPass");
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.Parameters.AddWithValue("@TRNS_Status", TRNS_Status);
                cmd.Parameters.AddWithValue("@BOOKINGMODE", BOOKINGMODE);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd); da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetBookingTransactionData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string BOOKING_TYPE, string DATETYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingIDWiseDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", "BoardingPass");
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetNegativeBookingTransactionData(string FromDate, string ToDate, string Place, string SHIFT_TYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_NegativeBookingsIDWiseDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }



        public DataTable SixPlusTransactionDetailsData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string DATETYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBooking6PlusDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable TicketCancellationDetailsData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string DATETYPE)
        {
            try
            {
                DALConn();
                
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ListTicketCancellationDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO",ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

		public DataTable TicketRefundDetailsData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string DATETYPE)
		{
			try
			{
				DALConn();

				DataTable dt = new DataTable();
				SqlCommand cmd = new SqlCommand("MIS_ListTicketRefundDetails", Conn);
				cmd.Parameters.AddWithValue("@Action", "ListOfTicketRefundDetails");
				cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
				cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
				cmd.Parameters.AddWithValue("@PlaceID", Place);
				cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
				cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.SelectCommand.CommandTimeout = 500000000;
				da.Fill(dt);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Conn.Close();
			}
		}

        public DataTable TicketRefundDetailsDataJhalana(string FromDate, string ToDate, string Place)
        {
            try
            {
                DALConn();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("TB_BookTicket_NP_RefundProcess", Conn);
                cmd.Parameters.AddWithValue("@Action", "ListOfTicketRefundDetails");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                //cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                //cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable TicketCancellationDetailsDataResend(string Action, int IsPartialOrFullCanelation, string TicketID, string RemarkStatus, string Remark, long USERID)
        {
            try
            {
                DALConn();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GETTicketBookingUserDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@IsPartialOrFullCanelation", IsPartialOrFullCanelation);
                cmd.Parameters.AddWithValue("@TicketID", TicketID);
                cmd.Parameters.AddWithValue("@RemarksStatus", RemarkStatus);
                cmd.Parameters.AddWithValue("@Remarks", Remark);
                cmd.Parameters.AddWithValue("@UserID", USERID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

		public DataTable TicketRefundUserDetailsDataResend(string Action, int IsPartialOrFullCanelation, string TicketID, string RemarkStatus, string Remark, long USERID)
		{
			try
			{
				DALConn();

				DataTable dt = new DataTable();
				SqlCommand cmd = new SqlCommand("SP_GETTicketBookingUserDetails", Conn);
				cmd.Parameters.AddWithValue("@Action", Action);
				cmd.Parameters.AddWithValue("@IsPartialOrFullCanelation", IsPartialOrFullCanelation);
				cmd.Parameters.AddWithValue("@TicketID", TicketID);
				cmd.Parameters.AddWithValue("@RemarksStatus", RemarkStatus);
				cmd.Parameters.AddWithValue("@Remarks", Remark);
				cmd.Parameters.AddWithValue("@UserID", USERID);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.SelectCommand.CommandTimeout = 500000000;
				da.Fill(dt);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				Conn.Close();
			}
		}



		public DataTable TicketPartialCancellationDetailsData(string FromDate, string ToDate, string Place)
        {
            try
            {
                DALConn();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_OnlineBookingPartialCancelation", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public DataTable GetHeadWiseDepositDetailsData(string FromDate, string ToDate, string Place, string SHIFT_TYPE, string Action, string ModeOfBooking, string DATETYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_HeadWiseDepositDetailAndSummary", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@BOOKINGMODE", ModeOfBooking);
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetLogData(string ACTION, string DATETIME_FROM, string DATETIME_TO, string Moduleid, string SSOid)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_UserLogAndException", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATETIME_FROM);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATETIME_TO);
                cmd.Parameters.AddWithValue("@Moduleid", Moduleid);
                cmd.Parameters.AddWithValue("@SSOid", SSOid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable OnlineInformationBooking(string FromDate, string ToDate, long PlaceId, string Action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetTicketbookingInfoMemberWise", Conn);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable HalfDayFullDayDetail(string DATETYPE, string FromDate, string ToDate)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GETHalfDayFullDayDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@Action", "GetHalfDayFullDayDetails");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        #region  Added by Sunny For Vehicle Report

        public int Zone { get; set; }
        public int VehicleType { get; set; }
        public string IndianForeigner { get; set; }
        public int PlaceID { get; set; }
        public int ShiftID { get; set; }
        public long Index { get; set; }

        public string DateOfArrival { get; set; }
        public string ZoneName { get; set; }

        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public string IndianCount { get; set; }
        public string NonIndianCount { get; set; }
        public string IndianCameraCount { get; set; }
        public string NonIndianCameraCount { get; set; }
        public DataTable OnlineVehicleWiseReport(string FromDate, string ToDate, int Place, int Zone, int SHIFT_TYPE, int VehicleType, string IndianForeigner)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_OnlineVehicleWiseReport", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@Zone", Zone);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                cmd.Parameters.AddWithValue("@IndianForeigner", IndianForeigner);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        #endregion
    }

    public class MISExceptionUserLog
    {



        public int Index { get; set; }
        public string ErrorDate { get; set; }
        public string eErrorDate { get; set; }
        public string ErrorTime { get; set; }
        public string Module { get; set; }
        public string FunctionName { get; set; }
        public string ErrorMsg { get; set; }

        //=============================
        public string ClientIPAddress { get; set; }
        public string ssoid { get; set; }
        public string ModuleName { get; set; }
        public string ServiceTypeDesc { get; set; }
        public string SubPermissionDesc { get; set; }
        public string ActivityDate { get; set; }
        public string ActivityStartTime { get; set; }
        public string ActivityEndTime { get; set; }

        public string ActivityDuration { get; set; }




    }



    public class MISTicketTransactionDetails
    {

        public string Heads { get; set; }
        public string Fees { get; set; }


        public Int64 Index { get; set; }

        public Int64 TicketID { get; set; }
        public string BookingID { get; set; }
       // public string DateOfBooking { get; set; }
       // public string DateOfVisit { get; set; }
        public string Ssoid { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public string PlaceName { get; set; }
        // public string ZoneName { get; set; }
        public string ShiftTime { get; set; }
        public string NoOfForeignerMembers { get; set; }
        public string NoOfIndianMembers { get; set; }
       
       // public string TotalMembers { get; set; }

        public string ActualTicketDifference { get; set; }        
        public string TotalNoOfCamera { get; set; }
        public string VehicleName { get; set; }

       // public decimal  AmountTobePaid { get; set; }
        public string TransactionStatus { get; set; }
        public string EmitraTransactionID { get; set; }

        public decimal RefundAmount { get; set; }

        public string ResendStatus { get; set; }
        public string DateofCancelation { get; set; }
        public string Manual_Status { get; set; }
        public string Manual_Remarks { get; set; }


        // Head Wise Deposit Details 
        public string HEADS { get; set; }
        public string TOTAL_MEMBER { get; set; }
        public string INCOME_FROM_TOURISM_IN_RTR { get; set; }
        public string INCOME_FROM_ECO_DEV_SURCHARGE_IN_RTR { get; set; }
        public string TRDF { get; set; }
        public string TOTAL { get; set; }

        // TABLE ListHeadWiseDepositDetail

        public string RequestID { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfVisit { get; set; }
        public string ZoneName { get; set; }
        public string ShiftType { get; set; }
        public string Name { get; set; }
        public string Indian { get; set; }
        public string NonIndian { get; set; }
        public string TotalMembers { get; set; }

        public string CameraForIndian { get; set; }
        public string CameraForNonIndian { get; set; }
        public string IncomeFromTourismIndianMemberEntryFee { get; set; }
        public string IncomeFromTourismNonIndianMemberEntryFee { get; set; }
        public string IncomeFromTourismGypsyEntryFee { get; set; }
        public string IncomeFromTourismCanterEntryFee { get; set; }
        public string IncomeFromTourismIndianCameraEntryFee { get; set; }
        public string IncomeFromTourismNonIndianCameraEntryFee { get; set; }
        public string IncomeFromTourismIndianHDFDCharge { get; set; }
        public string IncomeFromTourismNonIndianHDFDCharge { get; set; }
        public string TOTALIncomeFromTourism { get; set; }
        public string IncomeFromECODEVIndianMemberEntryFee { get; set; }
        public string IncomeFromECODEVNonIndianMemberEntryFee { get; set; }
        public string IncomeFromECODEVGypsyEntryFee { get; set; }
        public string IncomeFromECODEVCanterEntryFee { get; set; }
        public string IncomeFromECODEVIndianCameraEntryFee { get; set; }
        public string IncomeFromECODEVNonIndianCameraEntryFee { get; set; }
        public string IncomeFromEcoDevIndianHDFdCharge { get; set; }
        public string IncomeFromEcoDevNonIndianHDFdCharge { get; set; }
        public string TOTALIncomeFromECODEV { get; set; }
        public string FoundationIndianMemberEntryFee { get; set; }
        public string FoundationNonIndianMemberEntryFee { get; set; }

        public string FoundationForVehicleEntryFee { get; set; }
        public string FoundationForGuidFee { get; set; }
        
        public string TOTALFoundation { get; set; }

        // added by arvind k sharma
        							

        public string VehicleRentFees { get; set; }
        public string VehicleRentFeesGSTPercentage { get; set; }
        public string VehicleRentFeesGSTAmount { get; set; }
        public string TOTALVehicleRentFees { get; set; }


        public string GuideFees { get; set; }
        public string GuideFeesGSTPercentage { get; set; }
        public string GuideFeesGSTAmount { get; set; }
        public string TOTALGuideFees { get; set; }


        // added by arvind k sharma

        public decimal AmountTobePaid { get; set; }
        public string TotalFeeHeadwise { get; set; }

        public string EmitraCharges { get; set; }
        public string TaxOnEmitraCharges { get; set; }
        public string EMitraTotalCharges { get; set; }
        public string AmountWithServiceCharges { get; set; }
        public string TotalPayment { get; set; }

        public string AMOUNT_STATUS { get; set; }
        public string AMOUNT_DIFFERENCE { get; set; }
        public string ModeOfBooking { get; set; }
		public string Refund_Status { get; set; }
		public string Payment_Reference_Id { get; set; }
		public string Payment_Mode { get; set; }
		public string Date_Of_Refund { get; set; }
		public string Remark { get; set; }



	}


    public class ViewTicketDT1
    {
        public string RequestID { get; set; }
        public string PlaceName { get; set; }
        public string EnteredOn { get; set; }
        public string DateOfArrival { get; set; }
        public string NoofTicket { get; set; }
        public decimal finalAmnt { get; set; }
        public string Boarding_Point { get; set; }
        public string contactperson { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
       

        public IEnumerable<ViewTicketDT2> ViewTicket2 { get; set; }
        public IEnumerable<ViewTicketDT3> ViewTicket3 { get; set; }
    }

    public class ViewTicketRemaningDT1
    {
        public string PlaceName { get; set; }
        public string ZoneName { get; set; }
        public string VehicleName { get; set; }
        public long TotalSeats { get; set; }
        public long seatsForCitizen { get; set; }

        public long ExtraseatsForCitizen { get; set; }

        public long TotalBookedSeat { get; set; }
        public long CurrentBookingSeat { get; set; }

        public long ExtraCurrentBookingSeat { get; set; }
        public long RemainingSeat { get; set; }
        public long UpComming { get; set; } ///Change by AMit for Citizen Report
        public long UnderProcess { get; set; }///Change by AMit for Citizen Report

        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public long NoOfMember { get; set; }

        public string RequestID { get; set; }

        public string ShiftTime { get; set; }

        public int Index { get; set; }

        public long CitizenbookedSeats { get; set; }
        public long CurrentbookedSeats { get; set; }
        public long CitizenRemainingSeat { get; set; }
        public long CurrentRemainingSeat { get; set; }
    }

    public class ViewTicketDT2
    {
        public int index { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string IDProof { get; set; }
        public string NoOfCamera { get; set; }
        public string Shift { get; set; }
        public string VName { get; set; }
    }

    public class ViewTicketDT3
    {
        public string Period { get; set; }
        public string MorningTrip { get; set; }
        public string AfterNoonTrip { get; set; }

      
    }

    public class SocialAuditorVehiclewiseReport :DAL
    {
        public int index { get; set; }
        public string FromDate { get; set; }
       
        public string ToDate { get; set; }
        public string Place { get; set; }        
        public string ZoneName { get; set; }
        public string ShiftType { get; set; }
        public string MCanter { get; set; }
        public string MGypsy { get; set; }

        public string ECanter { get; set; }
        public string EGypsy { get; set; }


        public List<SelectListItem> ListPlace { get; set; }

        public DataTable GetListData(SocialAuditorVehiclewiseReport Data)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_WildLife_SocialAuditor", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetList");
                cmd.Parameters.AddWithValue("@DATETIME_FROM", Data.FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", Data.ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Data.Place);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetPlace()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_WildLife_SocialAuditor", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlace");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


    }
    public class OnlineBookingReleaseTicketSchedularModel
    {
        public string RequestId { get; set; }
        public string TicketId { get; set; }
        public string Trn_Status { get; set; }
        public string TicketBookingEnterOn { get; set; }
        public string CreatedDate { get; set; }

    }


    public class OnlineBookingReleaseTicketSchedularRepo : DAL
    {
        public DataTable GetListData(long userID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GetOnlineBookingUpdateStatusEachMint", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                dt = new DataTable();
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Funcation Name" + "_" + "GetListData in OnlineBookingReleaseTicketSchedularRepo", 0, DateTime.Now, userID);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }

    public class HalfDayFullDayDetailModel
    {
        public string SerialNumber { get; set; }
        public string HalfDayFullDayBookingPlace { get; set; }
        public string HalfDayFullDayRequestID { get; set; }
        public string FullDayFullDayVisitDate { get; set; }
        public string HalfDayFullDayEnterOn { get; set; }
        public string FirstRefPlaceName { get; set; }
        public string FirstRefRequestID { get; set; }
        public string RefVisitDate { get; set; }
        public string FirstEnterOn { get; set; }
        public string SecoundRefPlaceName { get; set; }
        public string SecoudReqID { get; set; }
        public string RefVisitDate2 { get; set; }
        public string SecEnterOn { get; set; }
        public string OldIdProof { get; set; }
        public string ShiftType { get; set; }
        public string AmountTobePaid { get; set; }
        public string Ssoid { get; set; }
    }

    public class OnlineTicketBookingSSOModel
    {
        public string ID { get; set; }
        public string TicketID { get; set; }
        public string RequestIDWithMember { get; set; }
        public string RequestID { get; set; }
        public string EmitraTransactionID { get; set; }
        public string Place { get; set; }
        public string ZoneName { get; set; }
        public string SSOID { get; set; }
        public string TransactionStatus { get; set; }
        public string UserName { get; set; }
        public string UserDetails { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfVisit { get; set; }
        public string VehicleName { get; set; }

    }

    public class OnlineTicketBookingSSOListModel
    {
        public OnlineTicketBookingSSOListModel()
        {
            List = new List<OnlineTicketBookingSSOModel>();
        }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Place { get; set; }
        public string SSOID { get; set; }
        public string RequestID { get; set; }
        public string EmitraTransactionID { get; set; }
        public string ReconsilationResponse { get; set; }
        public List<OnlineTicketBookingSSOModel> List { get; set; }
    }

    public class OnlineTicketBoolingSSORepo : DAL
    {
        public DataTable GetTicketBookingBySSO(OnlineTicketBookingSSOListModel model, string Action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingDetailsBySSO", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", Convert.ToString(model.FromDate));
                cmd.Parameters.AddWithValue("@DATETIME_TO", Convert.ToString(model.ToDate));
                cmd.Parameters.AddWithValue("@PlaceID", Convert.ToString(model.Place));
                cmd.Parameters.AddWithValue("@SSOId", Convert.ToString(model.SSOID));
                cmd.Parameters.AddWithValue("@RequestID", Convert.ToString(model.RequestID));
                cmd.Parameters.AddWithValue("@EmitratransationID", Convert.ToString(model.EmitraTransactionID));

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd); da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
    }



    public class EventLogDetailModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }

        public string SerialNumber { get; set; }
        public string DatabaseName { get; set; }
        public string EventType { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string SqlCommand { get; set; }
        public string EventDate { get; set; }
        public string LoginName { get; set; }



    }



    public class sqlEventType : DAL
    {
        public DataTable GetEventLogs(EventLogDetailModel model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spChangeLog", Conn);
                cmd.Parameters.AddWithValue("@Action", "SELECTLog");
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToString(model.FromDate));
                cmd.Parameters.AddWithValue("@Todate", Convert.ToString(model.ToDate));
                cmd.Parameters.AddWithValue("@EventType", Convert.ToString(model.EventType));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd); da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


    }


    public class MISNurseryDetails : DAL
    {
       
        public string NurseryName { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string Distribution { get; set; }
        public string QtyForPlantation { get; set; }
        public string TotalStock { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public DataSet MIS_NurseryDetails(string Circle, string Division, string Range)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "MISGetDetailsNursery");
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", Circle);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", Division);
                cmd.Parameters.AddWithValue("@RANGE_CODE", Range);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
    }

}