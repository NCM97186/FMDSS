using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class wcircles:DAL
    {
        public int Index { get; set; }
        public long ROWID { get; set; }

        public string OperationType { get; set; } 
           
        public string REG_CODE { get; set; }
        public string REG_NAME { get; set; }

        public string CIRCLE_CODE { get; set; }

        public string CIRCLE_NAME { get; set; }

        public string CIRCLE_HNAME { get; set; }

        public decimal AREA_SQKM { get; set; }

        public int ISWILDLIFECIRCLE { get; set; }

        
        /* public void Placeauto()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_reset_autoincrement", Conn);
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
        } */

        public DataTable BindRegion(string region)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_select_regcode", Conn);
                cmd.Parameters.AddWithValue("@REG_CODE", region);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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


        public DataTable Select_Circle()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_select_allcircles", Conn);
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

        public DataTable Select_Place(int ROWID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_select_allcircles", Conn);
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

        /*public DataTable GETDivision()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        }*/

        public DataTable AddUpdatePlace(wcircles oPlace)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_select_allcircles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlace");
                cmd.Parameters.AddWithValue("@ROWID", oPlace.ROWID);
                cmd.Parameters.AddWithValue("@REG_CODE",oPlace.REG_CODE);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", oPlace.CIRCLE_CODE);
                cmd.Parameters.AddWithValue("@CIRCLE_NAME", oPlace.CIRCLE_NAME);
                cmd.Parameters.AddWithValue("@CIRCLE_HNAME", oPlace.CIRCLE_HNAME);
                cmd.Parameters.AddWithValue("@AREA_SQKM", oPlace.AREA_SQKM);
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

     /*   public Int64 DeletePlace()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Place", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
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