using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
   
    public class ZooEqptFee : DAL
    {

        public Int64 FeeId{ get; set; }
        public int Index { get; set; }
        public Int64 PlaceID{ get; set; }
       
       
        public string PlaceName { get; set; }
        public int ZooVehicleID { get; set; }
        public string VehicleName { get; set; }


        public int NumberofVehicle { get; set; }
      
        public decimal TotalFee { get; set; }
        public decimal FeePerVehicle { get; set; }
       
        public int IsActive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public DataTable Select_ZooEqptFees()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooEqptFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZooEqptFee");
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

        public DataTable Select_ZooEqptFee(int FeeId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooEqptFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooEqptFee");
                cmd.Parameters.AddWithValue("@FeeId", FeeId);
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

        public DataTable AddUpdateZooEqptFee(ZooEqptFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooEqptFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooEqptFee");
                cmd.Parameters.AddWithValue("@FeeId", oPlace.FeeId);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@ZooVehicleID", oPlace.ZooVehicleID);
                cmd.Parameters.AddWithValue("@FeePerVehicle", oPlace.FeePerVehicle);
                cmd.Parameters.AddWithValue("@NumberofVehicle", oPlace.NumberofVehicle);
                cmd.Parameters.AddWithValue("@TotalFee", oPlace.TotalFee);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
                

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

        public DataTable VehicalEqpt()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooEqptFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllVehicalEqptName");

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
                SqlCommand cmd = new SqlCommand("Sp_ZooEqptFee", Conn);
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