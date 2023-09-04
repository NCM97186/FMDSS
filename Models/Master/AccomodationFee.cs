using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    
    public class AccomodationFee : DAL
    {

        public Int64 AccommodationID { get; set; }
        public int Index { get; set; }
        public Int64 PlaceID { get; set; }
        public string PlaceName { get; set; }
        public string RoomType { get; set; }
        public int TotalRooms { get; set; }
        public decimal RatePerRoom { get; set; }
        public int IsActive { get; set; }
        public bool IsactiveView { get; set; }
        public string UpdatedBy { get; set; }
        public string EnteredBy { get; set; }
        public string OperationType { get; set; }
        public DataTable Select_AccomodationFees()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllAccomodationFee");
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

        public DataTable Select_AccomodationFee(int AccommodationID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneAccomodationFee");
                cmd.Parameters.AddWithValue("@AccommodationID", AccommodationID);
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

        public DataTable AddUpdateAccomodationFee(AccomodationFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateAccomodationFee");
                cmd.Parameters.AddWithValue("@AccommodationID", oPlace.AccommodationID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@RoomType", oPlace.RoomType);
                cmd.Parameters.AddWithValue("@TotalRooms", oPlace.TotalRooms);
                cmd.Parameters.AddWithValue("@RatePerRoom", oPlace.RatePerRoom);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.IsActive);
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

        public Int64 DeleteAccomodationFee()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.Parameters.AddWithValue("@Action", "DeleteAccomodationFee");
                cmd.Parameters.AddWithValue("@AccommodationID", AccommodationID);
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
        }

       
        public DataTable SelectAllPlaces()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaces");
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



        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AccomodationFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateAccomodationFee");
                cmd.Parameters.AddWithValue("@AccommodationID", AccommodationID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@RoomType", RoomType);
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