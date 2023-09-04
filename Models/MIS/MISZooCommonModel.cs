using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.MIS
{
    public class MISZooCommonModel : DAL
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Place { get; set; }
        public string SHIFT_TYPE { get; set; }
        public string BOOKING_TYPE { get; set; }
        public string PLACE_NAME { get; set; }
        public string SHIFTName { get; set; }
        public string DATETYPE { get; set; }
        /*ADDED BY NITIN JAIN 10/11/2017*/
        public string DownloadExcel { get; set; }
        public string TimeDifference { get; set; }
        public List<MISZooDetailReport> MISZooDetailReport; 
        /*ADDED BY NITIN JAIN 10/11/2017*/

        public string ReportType { get; set; }
        public string Modules { get; set; }
        public string TRNS_Status { get; set; }
        public string ModeOfBooking { get; set; }

        public string SSOIDType { get; set; }
        public string SSOID { get; set; }
       

        public string ZooBookingId { get; set; }
        public string RequestId { get; set; }
        public string BookingType { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfVisit { get; set; }
        public string NameofInstituteandOrganization { get; set; }
        public string TotalMember { get; set; }
        public string TotalCamera { get; set; }
        public string VehicleType { get; set; }
        public string IsPrivateVehical { get; set; }
        public string TotalVehicalFees { get; set; }
        public string TotalMemberFees { get; set; }
        public string TotalCameraFees { get; set; }
        public string TransactionStatus { get; set; }
        public string EmitraTransactionId { get; set; }
        public string TotalAmount { get; set; }
        public string IP_Address { get; set; }

        public int Index { get; set; }
        public int ZooTicketBoardingVerificationStatus { get; set; }
        public string BoardingVerificationDateTime { get; set; }





        public DataTable GetPlaces(Int64 UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ZooCommon", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlaces");
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

        public DataTable GetZooTicketBookingDetails(string FromDate, string ToDate, string Place, string DATETYPE, string TRNS_Status, string ModeOfBooking, string BookingType, string SSOIDType, Int64 USERID, string ShiftType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ZooTicketBookingDetails", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@DATETYPE", DATETYPE);
                cmd.Parameters.AddWithValue("@TRNS_Status", TRNS_Status);
                cmd.Parameters.AddWithValue("@BOOKINGMODE", ModeOfBooking);
                cmd.Parameters.AddWithValue("@BookingType", BookingType);
                cmd.Parameters.AddWithValue("@SSOIDType", SSOIDType);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);


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

        public DataTable GetZooHeadWiseDepositDetailAndSummary(string ReportType, string FromDate, string ToDate, string Place, string ModeOfBooking, string SSOIDType, Int64 USERID, string ShiftType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ZooHeadWiseDepositDetailAndSummary", Conn);
                cmd.Parameters.AddWithValue("@Action", ReportType);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@BOOKINGMODE", ModeOfBooking);
                cmd.Parameters.AddWithValue("@SSOIDType", SSOIDType);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
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

        public DataTable GetZooTicketVerification(string Action, string RequestID, int Place)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetZooTicketVerification", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
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


    }

    public class MISZooDetailReport
    {
        public int Index { get; set; }
        public string RequestId { get; set; }
        public string BookingType { get; set; }
        public string DateOfBooking { get; set; }
        public string DateOfVisit { get; set; }
        public string NameofInstituteandOrganization { get; set; }
        public string TotalMember { get; set; }
        public string TotalCamera { get; set; }
        public string VehicleType { get; set; }
        public string IsPrivateVehical { get; set; }
        public string TotalVehicalFees { get; set; }
        public string TotalMemberFees { get; set; }
        public string TotalCameraFees { get; set; }
        public string TransactionStatus { get; set; }
        public string EmitraTransactionId { get; set; }
        public string TotalAmount { get; set; }
        public string IP_Address { get; set; }
        public string PLACE_NAME { get; set; }
        public string SHIFT_TYPE { get; set; }
        public string ModeOfBooking { get; set; }
        public string TicketStatus { get; set; }
    }
}