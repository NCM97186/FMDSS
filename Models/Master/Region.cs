using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class Region:DAL
    {
        public int Index { get; set; }
        public long ROWID { get; set; }

        public string OperationType { get; set; } 
           
        public string REG_CODE { get; set; }

        public string REG_NAME { get; set; }

       public string REG_HNAME { get; set; }

        public decimal AREA_SQKM { get; set; }
       public void regionauto()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_reset_autoregion", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteScalar();                
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
     
        public DataTable Select_Regions()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_AllRegion", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlace");
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

        public DataTable Select_Region(int ROWID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_AllRegion", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOnePlace");
                cmd.Parameters.AddWithValue("@ROWID", ROWID);
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

    
        public DataTable AddUpdateRegion(Region oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_AllRegion", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlace");
                cmd.Parameters.AddWithValue("@ROWID", oPlace.ROWID);
                cmd.Parameters.AddWithValue("@REG_CODE", oPlace.REG_CODE);
                cmd.Parameters.AddWithValue("@REG_NAME", oPlace.REG_NAME);
                cmd.Parameters.AddWithValue("@REG_HNAME", oPlace.REG_HNAME);
                cmd.Parameters.AddWithValue("@AREA_SQKM", oPlace.AREA_SQKM);
                //cmd.Parameters.AddWithValue("@TicketAllocatedPerShift", oPlace.TicketAllocatedPerShift);
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

       /* public Int64 DeleteRegion()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Place", Conn);
                cmd.Parameters.AddWithValue("@ROWID", ROWID);
               // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }*/
    
    }
   

}