using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class ProduceTypes : DAL
    {
        public Int64 ID{ get; set; }
        public int Index { get; set; }
        public string ProduceType { get; set; }
        public string UnitName { get; set; }
        public int IsActive { get; set; }
        public bool IsactiveView { get; set; }
        public string ProduceFor { get; set; }
        
        public string EnteredOn { get; set; }
        public string EnteredBy { get; set; }
        public string UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
        public string OperationType { get; set; }

        public DataTable Select_ProduceTypes()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDM_ProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllProduceType");
              
                
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

        public DataSet Select_ProduceType(int ID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FDM_ProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneProduceType");
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

        public DataTable AddUpdateProduceType(ProduceTypes oProduceType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDM_ProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateProduceType");
                cmd.Parameters.AddWithValue("@ID", oProduceType.ID);
                cmd.Parameters.AddWithValue("@ProduceType", oProduceType.ProduceType);
                cmd.Parameters.AddWithValue("@UnitName", oProduceType.UnitName);
                cmd.Parameters.AddWithValue("@IsActive", oProduceType.IsActive);
                cmd.Parameters.AddWithValue("@EnteredBy", oProduceType.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oProduceType.UpdatedBy);
                cmd.Parameters.AddWithValue("@ProduceFor", oProduceType.ProduceFor);    
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

        //public Int64 DeleteProduceType()
        //{
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_FDM_ProduceType", Conn);
        //        cmd.Parameters.AddWithValue("@Action", "DeleteProduceType");
        //        cmd.Parameters.AddWithValue("@ID", ID);                
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
        //        return chk;
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

        //public bool Check_DuplicateRecord()
        //{
        //    try
        //    {
        //        DALConn();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("Sp_FDM_ProduceType", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", "CheckDuplicateProduceType");
        //        cmd.Parameters.AddWithValue("@ID", ID);
        //        cmd.Parameters.AddWithValue("@ProduceType", ProduceType);
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