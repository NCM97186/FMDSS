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
     public class Zone : DAL
    {
        public int zoneID { get; set; }
        public int Index { get; set; }
        public int PlaceID { get; set; }
        public string OperationType { get; set; }
        public string PlaceName { get; set; }
        public string ZoneName { get; set; }
        public int TicketAllocatedPerShift { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }  
        public long EnteredBy { get; set; }
        public long UpdatedBy { get; set; }
        public string ShiftType { get; set; }
        public string ShiftTypeName { get; set; }
        public int isMorning { get; set; }
        public int isEvening { get; set; }
        public int isFullDay { get; set; }

        public bool isMorningView { get; set; }
        public bool isEveningView { get; set; }
        public bool isFullDayView { get; set; }

        public int isDptKiosk { get; set; }
        public int isCitizen { get; set; }
 
       
        public DataTable Select_ZoneS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZone");
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

        public DataTable Select_Zone(int zoneID)
        {
            try
            { 
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneAllZone");
                cmd.Parameters.AddWithValue("@ZoneID", zoneID);
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


        public DataTable AddUpdateZone(Zone oPlace)
        {
            try
            {
                DALConn(); 
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZone");
                cmd.Parameters.AddWithValue("@ZoneID", oPlace.zoneID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);

                cmd.Parameters.AddWithValue("@ZoneName", oPlace.ZoneName);
                cmd.Parameters.AddWithValue("@TicketAllocatedPerShift", oPlace.TicketAllocatedPerShift);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);

                cmd.Parameters.AddWithValue("@isMorning", oPlace.isMorning);
                cmd.Parameters.AddWithValue("@isEvening", oPlace.isEvening);
                cmd.Parameters.AddWithValue("@isFullDay", oPlace.isFullDay);

                cmd.Parameters.AddWithValue("@isDptKiosk", oPlace.isDptKiosk);
                cmd.Parameters.AddWithValue("@isCitizen", oPlace.isCitizen);


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

     
        public DataTable SelectPlaces()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlace");
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

        public DataTable SelectJhalanaPlaces()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlaceJhalana");
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

        public DataTable SelectPlacesForCitizenInventoryReport()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlaceForCitizenInventory");
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

        public DataTable SelectOnlineBookingRefundRemarks()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZONE", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetOnlineBookingRemarks");
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