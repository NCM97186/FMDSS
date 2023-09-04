using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class ZooSeatInventory : DAL
    {

        public int ZooSeatInventoryId { get; set; }
        public int Index { get; set; }
        public Int64 PlaceID{ get; set; }
       
       
        public string PlaceName { get; set; }
        public int OnlineZooSeats { get; set; }



        public int OffLineZooSeats { get; set; }
      
       
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public DataTable Select_ZooSeatInventorys()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooSeatInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZooSeatInventory");
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

        public DataTable Select_ZooSeatInventory(int ZooSeatInventoryId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooSeatInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooSeatInventory");
                cmd.Parameters.AddWithValue("@ZooSeatInventoryId", ZooSeatInventoryId);
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

        public DataTable AddUpdateZooSeatInventory(ZooSeatInventory oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooSeatInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooSeatInventory");
                cmd.Parameters.AddWithValue("@ZooSeatInventoryId", oPlace.ZooSeatInventoryId);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@OnlineZooSeats", oPlace.OnlineZooSeats);
                cmd.Parameters.AddWithValue("@OffLineZooSeats", oPlace.OffLineZooSeats);
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




        public DataTable PlaceName1()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooSeatInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllPlaceName");

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



       


    }
}