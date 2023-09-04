using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class cls_SeatInventorySetting : DAL
    {
        public int SeatInventorySetting { get; set; }
       public string 	PlaceId {get;set;}
       public string PlaceName { get; set; }
        public int	ShiftId {get;set;}
        public string ShiftName { get; set; }
        public int	ZoneId {get;set;}
        public string ZoneName { get; set; }
        public string	VehicleName {get;set;}
        public bool	isAdvance {get;set;}
        public bool	isCurrent {get;set;}
        public bool isActive { get; set; }

        public List<cls_SeatInventorySetting> List { get; set; }
        public cls_SeatInventorySetting Model { get; set; }
        public DataTable Select_SeatInventorySetting()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSeatInventorySetting", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectAll");
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

        public DataSet GetSelect_SeatInventorySetting(long id)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("spSeatInventorySetting", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SELECTONE");
                cmd.Parameters.AddWithValue("@SeatInventorySetting", id);
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
                SqlCommand cmd = new SqlCommand("spSeatInventorySetting", Conn);
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

        public DataTable SelectZone()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSeatInventorySetting", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetZone");
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


        public string SaveSeatInventorySetting(cls_SeatInventorySetting modellist)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSeatInventorySetting", Conn);
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@PlaceId", modellist.PlaceId);
                cmd.Parameters.AddWithValue("@ShiftId", modellist.ShiftId);
                cmd.Parameters.AddWithValue("@ZoneId", modellist.ZoneId);
                cmd.Parameters.AddWithValue("@VehicleName",  modellist.VehicleName);
                cmd.Parameters.AddWithValue("@isAdvance",  modellist.isAdvance);
                cmd.Parameters.AddWithValue("@isCurrent",  modellist.isCurrent);
                cmd.Parameters.AddWithValue("@isActive",  modellist.isActive);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToString(dt.Rows[0]["MSG"]);

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