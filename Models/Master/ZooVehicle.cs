using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
  
    public class ZooVehicle : DAL
    {

        public int ZooVehicleID { get; set; }
        public int Index { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string VehicleName { get; set; }
        [Required]
        public Int64 CategoryID { get; set; }
        [Required]
       
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }

        public DataTable Select_ZooVehicles()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooVehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicle");
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

        public DataTable Select_ZooVehicle(int ZooVehicleID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooVehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooVehicle");
                cmd.Parameters.AddWithValue("@ZooVehicleID", ZooVehicleID);
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

        public DataTable VehicleType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooVehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllVehicleType");

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


        public DataTable AddUpdateZooVehicle(ZooVehicle oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooVehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooVehicle");
                cmd.Parameters.AddWithValue("@ZooVehicleID", oPlace.ZooVehicleID);
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.CategoryID);
                cmd.Parameters.AddWithValue("@VehicleName", oPlace.VehicleName);
               
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

      

        
    }
}