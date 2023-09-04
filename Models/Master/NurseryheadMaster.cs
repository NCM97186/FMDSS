using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class NurseryHeadMaster : DAL
    {
        public Int64 NurseriesHeadID { get; set; }
        public int Index { get; set; }
        public string NurserieHeadName { get; set; }
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }

        public DataTable Select_NurseryHeadMasters()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Nursery_HeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllNurseryHeadMaster");
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

        public DataTable Select_NurseryHeadMaster(int NurseriesHeadID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Nursery_HeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneNurseryHeadMaster");
                cmd.Parameters.AddWithValue("@NurseriesHeadID", NurseriesHeadID);
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

        public DataTable AddUpdateNurseryHeadMaster(NurseryHeadMaster oNurseryHeadMaster)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Nursery_HeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateNurseryHeadMaster");
                cmd.Parameters.AddWithValue("@NurseriesHeadID", oNurseryHeadMaster.NurseriesHeadID);
                cmd.Parameters.AddWithValue("@NurserieHeadName", oNurseryHeadMaster.NurserieHeadName);
                cmd.Parameters.AddWithValue("@isActive", oNurseryHeadMaster.isActive);
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