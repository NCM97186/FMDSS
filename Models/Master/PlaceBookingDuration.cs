using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System;

namespace FMDSS.Models.Master
{   
   public class PlaceBookingDuration:DAL
    {
        public int ID{ get; set; }
        public int Index { get; set; }
        public long PlaceID { get; set; }
        public long ZoneID { get; set; }
        public string OperationType { get; set; }
        public string PlaceName { get; set; }
        public string ZoneName { get; set; }
        public string BookingTypeName { get; set; }
        public string DurationFromDate { get; set; }
        public string DurationToDate { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public string TicketDurationFromDate { get; set; }
        public string TicketDurationToDate { get; set; }




       
       public DataTable Select_PlaceBookingDurations()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_PlaceBookingDuration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaceBookingDuration");
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
       public DataTable Select_PlaceBookingDuration(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_PlaceBookingDuration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOnePlaceBookingDuration");
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
       public DataTable AddUpdatePlaceBookingDuration(PlaceBookingDuration oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_PlaceBookingDuration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlaceBookingDuration");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@ZoneID", oPlace.ZoneID);
                cmd.Parameters.AddWithValue("@BookingTypeName", oPlace.BookingTypeName);
                cmd.Parameters.AddWithValue("@DurationFromDate", oPlace.DurationFromDate);
                cmd.Parameters.AddWithValue("@DurationToDate", oPlace.DurationToDate);
                cmd.Parameters.AddWithValue("@TicketDurationToDate", oPlace.TicketDurationToDate);
                cmd.Parameters.AddWithValue("@TicketDurationFromDate", oPlace.TicketDurationFromDate);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
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

       public DataTable GETPlace()
       {
           try
           {
               DALConn();
               SqlCommand cmd = new SqlCommand("Sp_PlaceBookingDuration", Conn);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", "SelectPlaceLIST");
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

       }
       public DataTable GETZone(string PlaceID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_PlaceBookingDuration", Conn);
                cmd.Parameters.AddWithValue("@Action", "SelectZoneLIST");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
               
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

    }

}