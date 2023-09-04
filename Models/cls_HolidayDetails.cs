using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{

    public class AddRuleDetails : DAL
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int RuleId { get; set; }



        public DataTable UPdateunauthorizedLog(string MobileNumber,string Msg)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "unauthorized");
                cmd.Parameters.AddWithValue("@Mobile", MobileNumber);
                cmd.Parameters.AddWithValue("@ApproverMsg", Msg);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable CheckApproverValidate(string MobileNumber)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "CheckApproverDetails");
                cmd.Parameters.AddWithValue("@ApproverMobileNumber", MobileNumber);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable UpdateHolidayDetails(string MobileNumber, string Status,string UpdatedBy, string messagetext)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "UpdateStatus");
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@ApproverMobileNumber", MobileNumber);
                cmd.Parameters.AddWithValue("@ApproverMsg", messagetext);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetUserByMobileNumber(string mobilenumber)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetUserByMobile");
                cmd.Parameters.AddWithValue("@Mobile", mobilenumber);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable addHolidayDetails(AddRuleDetails details)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "INSERTDetails");
                cmd.Parameters.AddWithValue("@FromDate", details.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", details.ToDate);
                cmd.Parameters.AddWithValue("@ID", details.RuleId);
                cmd.CommandType = CommandType.StoredProcedure;
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


    public class RuleDetails
    {
        public int cIndex { get; set; }
        public string HolidayDate { get; set; }
        public string EnterDate { get; set; }
        public string EnterBy { get; set; }
        public string IsActive { get; set; }
        public string ApprovedDate { get; set; }
        public string ApproverName { get; set; }
        public string ApproverMobileNumber { get; set; }
    }

    public class cls_HolidayDetails : DAL
    {
        public int Id { get; set; }
        public int cIndex { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string CounterHolidaySeats { get; set; }
        public string OnlineHolidaySeats { get; set; }
        public string VehicleName { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }

        public List<cls_HolidayDetails> List { get; set; }
        public cls_HolidayDetails Model { get; set; }

        public List<RuleDetails> RuleDetailsList { get; set; }



        public DataTable SelectAllRules()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetHolidayRule");
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable SelectHolidayRuleDetails(string ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetHolidayRuleDetails");
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable SelectPlaceList()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectPlace");
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable SelectRuleList(int RuleId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spHolidaySeat", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetHolidayRuleById");
                cmd.Parameters.AddWithValue("@ID", RuleId);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public string SendHolidayEmailSMS(int data,int RuleId=0)
        {
            #region Email and SMS
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            //string sMessage = string.Empty;

            if((int)SMSTEmplate.HolidaySeat_Request==data)
            {
                DataTable oDt = SelectRuleList(RuleId);
                string[] sMessage = new string[] { Convert.ToString(oDt.Rows[0]["ZoneName"]), Convert.ToString(oDt.Rows[0]["ShiftName"]), Convert.ToString(oDt.Rows[0]["CounterHolidaySeats"]), Convert.ToString(oDt.Rows[0]["OnlineHolidaySeats"]), Convert.ToString(oDt.Rows[0]["VehicleName"]) };
                objSMSandEMAILtemplate.SendMailComman("ALL", "HolidaySeat_Request",Convert.ToString(oDt.Rows[0]["PlaceName"]), "", "", "", "","", sMessage);
            }

            return "";
            #endregion
        }
    }
    enum SMSTEmplate
    {
        HolidaySeat_Request = 1,
        HolidaySeat_response = 2
    }
}