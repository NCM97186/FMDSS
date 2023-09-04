using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;


namespace FMDSS.Models.Master
{
  
     public class Districts:DAL
    {
        public int Index { get; set; }
        public long ROWID { get; set; }

        public string STATE_CODE { get; set; }

        public string DIST_CODE { get; set; }

        public string DIST_NAME { get; set; }

        public bool ISOrganisingCamp { get; set; }
        public bool IsActive { get; set; }
        public string CurrentAction { get; set; }
        public Int16 STATUS { get; set; }
         
        public DataTable Select_District()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UPDATE_Districts", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SELECTALLDistricts");
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

        public void UpdateDistrict(Districts oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UPDATE_Districts", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", CurrentAction);
                cmd.Parameters.AddWithValue("@ROWID", oPlace.ROWID);
                cmd.Parameters.AddWithValue("@STATUS", oPlace.STATUS);   
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                
               
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