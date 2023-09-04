using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FMDSS.Models.Master
{
    public class FilmShootingSecurityDeposit : DAL
    {
        public Int64 FilmShootingSDID { get; set; }
        public int Index { get; set; }


        [Remote("CheckDuplicateForFilmShootingSecurityDeposit", "Master", AdditionalFields = "FilmCategory", ErrorMessage = "Duplicate Record Found")]
        public long PlaceID { get; set; }

        public string OperationType { get; set; }

        public string PlaceName { get; set; }

        [Remote("CheckDuplicateForFilmShootingSecurityDeposit", "Master", AdditionalFields = "PlaceID", ErrorMessage = "Duplicate Record Found")]
        public string FilmCategory { get; set; }
        public decimal Amount { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


       
        public DataTable Select_FilmShootingSecurityDepositS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingSecurityDeposit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllFilmShootingSD");
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

        public DataTable Select_FilmShootingSecurityDeposit(int FilmShootingFeesID)
        {
            try
            { 
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingSecurityDeposit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneFilmShootingSD");
                cmd.Parameters.AddWithValue("@FilmShootingSDID", FilmShootingFeesID);
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

        //public DataTable GETDivision()
        //{
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
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

        public DataTable AddUpdateFilmShootingSecurityDeposit(FilmShootingSecurityDeposit oPlace)
        {
            try
            {
                DALConn(); 
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingSecurityDeposit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateFilmShootingSD");
                cmd.Parameters.AddWithValue("@FilmShootingSDID", oPlace.FilmShootingSDID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
               
                cmd.Parameters.AddWithValue("@FilmCategory", oPlace.FilmCategory);
                cmd.Parameters.AddWithValue("@Amount", oPlace.Amount);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);
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

        //public Int64 DeleteCampFees()
        //{
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("SP_Delete_TicketingPlace", Conn);
        //        cmd.Parameters.AddWithValue("@FeeId", CampFeesID);
        //        // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
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

        public DataTable SelectPlaceCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingSecurityDeposit", Conn);
                cmd.Parameters.AddWithValue("@Action", "SelectPlaceCategory");
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

        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingSecurityDeposit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateFilmShootingSD");
                cmd.Parameters.AddWithValue("@FilmShootingSDID", FilmShootingSDID);                
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@FilmCategory", FilmCategory);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return false;
                else
                    return true;
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