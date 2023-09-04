using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace FMDSS.Models
{
    public class RequestDetails
    {
        public string DivName { get; set; }
        public string DistName { get; set; }
        public string BlockName { get; set; }
        public string Gpname { get; set; }
        public string VillageName { get; set; }
    }
    public class NewCitizenDetails
    {
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }
    public class AnalyticData
    {
        public string TotalRequest { get; set; }
        public string Approved { get; set; }
        public string Pending { get; set; }
        public string Reject { get; set; }
    }
    public class CitizenDetails
    {
        public string RequestID { get; set; }
        public string ssoId { get; set; }
        public string KML_Path { get; set; }
        public string Revenue_Map_Path { get; set; }
        public DateTime DurationFrom { get; set; }
        public DateTime DurationTo { get; set; }
        public string Amount { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string Final_Amount { get; set; }
    }
    public class KPIs
    {
        public string value { get; set; }
        public string unitCode { get; set; }
    }
    public class CitizenModel : DAL
    {
        public DataTable GetCitizenData(string spName, string Duration, string DATETIME_FROM, string DATETIME_TO, string CIRCLE_CODE, string DIVISON_CODE, string RANGE_CODE, string ServiceCategory, string ServiceName)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(spName, Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("RU-ru");
                cmd.Parameters.AddWithValue("@DURATION", Duration);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATETIME_FROM);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATETIME_TO);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", CIRCLE_CODE);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", DIVISON_CODE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", RANGE_CODE);
                cmd.Parameters.AddWithValue("@ServiceCategory", ServiceCategory);
                cmd.Parameters.AddWithValue("@ServiceName", ServiceName);
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
        public List<AnalyticData> GetAnalyticData(NewCitizenDetails citizenDetails)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spCitizenReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("RU-ru");
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(citizenDetails.fromdate, cul));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(citizenDetails.todate, cul));
                cmd.Parameters.AddWithValue("@Type", "CountRequest");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                List<AnalyticData> oCitizenList = new List<AnalyticData>();
                foreach (DataRow dr in dt.Rows)
                {
                    AnalyticData CD = new AnalyticData();
                    CD.TotalRequest = Convert.ToString(dr["TOTAL"]);
                    CD.Approved = Convert.ToString(dr["Approved"]);
                    CD.Pending = Convert.ToString(dr["Pending"]);
                    CD.Reject = Convert.ToString(dr["Reject"]);
                    oCitizenList.Add(CD);
                }
                return oCitizenList;
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
        public List<CitizenDetails> GetCitizenRequestList(NewCitizenDetails citizenDetails)
        {
            List<CitizenDetails> oCitizenList = new List<CitizenDetails>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spCitizenReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("RU-ru");
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(citizenDetails.fromdate, cul));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(citizenDetails.todate, cul));
                cmd.Parameters.AddWithValue("@Type", "AdminFixedPermissionsCount");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    CitizenDetails CD = new CitizenDetails();
                    CD.RequestID = Convert.ToString(dr["RequestedID"]);
                    CD.ssoId = Convert.ToString(dr["SSOID"]);
                    CD.KML_Path = Convert.ToString(dr["KML_Path"]);
                    CD.Revenue_Map_Path = Convert.ToString(dr["Revenue_Map_Path"]);
                    CD.DurationFrom = Convert.ToDateTime(dr["DurationFrom"]);
                    CD.DurationTo = Convert.ToDateTime(dr["DurationTo"]);
                    CD.Amount = Convert.ToString(dr["Amount"]);
                    CD.Tax = Convert.ToString(dr["Tax"]);
                    CD.Final_Amount = Convert.ToString(dr["Final_Amount"]);
                    oCitizenList.Add(CD);
                }
            }
            catch
            {
            }
            finally
            {
                Conn.Close();
            }
            return oCitizenList;
        }
        public List<CitizenDetails> GetCitizenRequestList()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spCitizenReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@FromDate", DateTime.Now.AddDays(-30));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Type", "AdminFixedPermissionsCount");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                List<CitizenDetails> oCitizenList = new List<CitizenDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    CitizenDetails CD = new CitizenDetails();
                    CD.RequestID = Convert.ToString(dr["RequestedID"]);
                    CD.ssoId = Convert.ToString(dr["SSOID"]);
                    CD.KML_Path = Convert.ToString(dr["KML_Path"]);
                    CD.Revenue_Map_Path = Convert.ToString(dr["Revenue_Map_Path"]);
                    CD.DurationFrom = Convert.ToDateTime(dr["DurationFrom"]);
                    CD.DurationTo = Convert.ToDateTime(dr["DurationTo"]);
                    CD.Amount = Convert.ToString(dr["Amount"]);
                    CD.Tax = Convert.ToString(dr["Tax"]);
                    CD.Final_Amount = Convert.ToString(dr["Final_Amount"]);
                    oCitizenList.Add(CD);
                }
                return oCitizenList;
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
        public List<RequestDetails> GetRequestDetails(string id)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spCitizenReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RequestId", id);
                cmd.Parameters.AddWithValue("@Type", "FixedPermissionDetails");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                List<RequestDetails> oRequestList = new List<RequestDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    RequestDetails CD = new RequestDetails();
                    CD.DivName = Convert.ToString(dr["DIVNAME"]);
                    CD.DistName = Convert.ToString(dr["DISTNAME"]);
                    CD.BlockName = Convert.ToString(dr["BLOCKNAME"]);
                    CD.Gpname = Convert.ToString(dr["GPNAME"]);
                    CD.VillageName = Convert.ToString(dr["VILLNAME"]);
                    oRequestList.Add(CD);
                }
                return oRequestList;
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
        public DataTable GetCircle()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Circle");
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
        public DataTable GetDivision(string CircleCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Division");
                cmd.Parameters.AddWithValue("@PARAMETER1", CircleCode);
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
        public DataTable GetRange(string div_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "RANGE");
                cmd.Parameters.AddWithValue("@PARAMETER1", div_Code);
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
        public DataTable GetBLOCK(string div_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "BLOCK");
                cmd.Parameters.AddWithValue("@PARAMETER1", div_Code);
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
        public DataTable GetPermissionStatus()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "PermissionStatus");
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


        public DataTable GetPermissionStatusForResearch()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "PermissionStatusForResearch");
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
        public DataTable GetModule()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetModule");
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
        public DataTable GetFIXEDPermissionSubCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "FIXEDPermissionSubCategory");
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
        //========= DROPDOWN FILL FOR DEVELOPMENT 
        public DataTable GetProgramS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Program");
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
        public DataTable GetScheme(string Parameter1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Scheme");
                cmd.Parameters.AddWithValue("@PARAMETER1", Parameter1);
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
        public DataTable GetProject(string Parameter1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Project");
                cmd.Parameters.AddWithValue("@PARAMETER1", Parameter1);
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
        public DataTable GetModel(string Parameter1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Project");
                cmd.Parameters.AddWithValue("@PARAMETER1", Parameter1);
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
        public DataTable GetActivity(string Parameter1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Activity");
                cmd.Parameters.AddWithValue("@PARAMETER1", Parameter1);
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
        public DataTable FetchKPIsData(string Parameter1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetKPIsData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ActionType", Parameter1);
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
        public DataTable GetCurrentCircleDivision(string NOCRequestId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GetCurrentCircleDivRange", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "NOC_Current_Cir_Div");
                cmd.Parameters.AddWithValue("@NOCRequestId", NOCRequestId);
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
        public DataTable GetCurrentNocOtherDetail(string NOCRequestId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_NOC_Procedures", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetNocOtherDetail");
                cmd.Parameters.AddWithValue("@NOCRequestId", NOCRequestId);
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
        public DataTable TransferNocDivision(string NOCRequestId, string tDiv_Code,long UserId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_NOC_Procedures", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "NOC_Transfer");
                cmd.Parameters.AddWithValue("@Div_Code", tDiv_Code);
                cmd.Parameters.AddWithValue("@NOCRequestId", NOCRequestId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
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
        public DataTable GetHQLevelUser(long UserId,string NOCRequestId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_NOC_Procedures", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetHQLevelUser");
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@NOCRequestId", NOCRequestId);
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
}
