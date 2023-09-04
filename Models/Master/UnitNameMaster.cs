using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class UnitNameMaster : DAL
    {
        public Int64 UnitId { get; set; }
        public int Index { get; set; }
        public string ShortName { get; set; }
        public string UnitName { get; set; }
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
      
        public string OperationType { get; set; }

        public DataTable Select_UnitNameMasters()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UnitNameMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllUnitNameMaster");
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

        public DataTable Select_UnitNameMaster(int UnitId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UnitNameMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneUnitNameMaster");
                cmd.Parameters.AddWithValue("@UnitId", UnitId);
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

        public DataTable AddUpdateUnitNameMaster(UnitNameMaster oUnitNameMaster)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UnitNameMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateUnitNameMaster");
                cmd.Parameters.AddWithValue("@UnitId", oUnitNameMaster.UnitId);
                cmd.Parameters.AddWithValue("@ShortName", oUnitNameMaster.ShortName);
                cmd.Parameters.AddWithValue("@UnitName", oUnitNameMaster.UnitName);
                cmd.Parameters.AddWithValue("@isActive", oUnitNameMaster.isActive);
                
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