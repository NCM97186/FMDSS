using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class OfficeMapping : DAL
    {
        public int ID { get; set; }
        public string OffcLevel { get; set; }
        public string ForestBoundaries { get; set; }
        public string OfficeID { get; set; }

        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }

        public string USERID { get; set; }
        public string SSOID{ get; set; }
        public string Designation { get; set; }
        public string Designation_Name { get; set; }
        public string REPORTINGTO { get; set; }


        public List<UnMappedOfficeDetails> UnMappedOfficeLIST { get; set; }

        public List<MappedOfficeDetails> MappedOfficeLIST { get; set; }

        public DataSet GetMapUnmapDesignationsForPA(string OfficeID)
        {
            try
            { 
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetMapUnmapOfficeForUser");
                cmd.Parameters.AddWithValue("@OfficeID", OfficeID);
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

        public string MappingForPA(Int64 USERIDs, string ForestOffices, bool STATUS, Int64 LOGINSSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "MapOfficeForUser");
                cmd.Parameters.AddWithValue("@USERID", USERIDs);
                cmd.Parameters.AddWithValue("@OfficeID", ForestOffices);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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

        public DataSet GetREPORTINGTO(string USERID)
        {
            try
            { 
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetREPORTINGTO");
                cmd.Parameters.AddWithValue("@USERID", USERID);
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

        public string SubmitREPORTINGTO(OfficeMapping model,string LOGINSSOID)
        {
            try
            { 
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "MapOfficeForUser");
                cmd.Parameters.AddWithValue("@USERID", model.USERID);
                cmd.Parameters.AddWithValue("@OfficeID", model.OfficeID);
                cmd.Parameters.AddWithValue("@REPORTINGTO", model.REPORTINGTO);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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

    public class UnMappedOfficeDetails
    {
        public int Index { get; set; }
        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }
    
    }

    public class MappedOfficeDetails
    {
        public int Index { get; set; }
        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

    }
}