using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
  
    public class ZooHeadMaster : DAL
    {
        public Int32 HeadId{ get; set; }
        public int Index { get; set; }
        public string HeadName { get; set; }
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }

        public DataTable Select_ZooHeadMasters()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZooHeadMaster");
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

        public DataTable Select_ZooHeadMaster(int HeadID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooHeadMaster");
                cmd.Parameters.AddWithValue("@HeadID", HeadID);
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

        public DataTable AddUpdateZooHeadMaster(ZooHeadMaster oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeadMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooHeadMaster");
                cmd.Parameters.AddWithValue("@HeadId", oPlace.HeadId);
                cmd.Parameters.AddWithValue("@HeadName", oPlace.HeadName);
                cmd.Parameters.AddWithValue("@isActive", oPlace.isActive);
                
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

      

        //public bool Check_DuplicateRecord()
        //{
        //    try
        //    {
        //        DALConn();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("Sp_ZooHeadMaster", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "CheckDuplicateEquipment");
        //        cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
        //        cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //}

    }
}