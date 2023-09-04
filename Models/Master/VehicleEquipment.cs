using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
  
    public class VehicleEquipment : DAL
    {
        public Int64 CategoryID{ get; set; }
        public int Index { get; set; }
        public string CategoryName { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public string EnteredOn { get; set; }
        public string EnteredBy { get; set; }
        public string UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
        public string OperationType { get; set; }

        public DataTable Select_VehicleEquipments()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicleEquipment");
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

        public DataTable Select_VehicleEquipment(int CategoryID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneVehicleEquipment");
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
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

        public DataTable AddUpdateVehicleEquipment(VehicleEquipment oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateVehicleEquipment");
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", oPlace.CategoryName);
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

        public Int64 DeleteVehicleEquipment()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.Parameters.AddWithValue("@Action", "DeleteVehicleEquipment");
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);                
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

        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateEquipment");
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
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

    public class VehicleMasterModel
    {
        public VehicleMasterModel()
        {
            List = new List<VehicleMaster>();
            Model = new VehicleMaster();
        }

        public List<VehicleMaster> List { get; set; }
        public VehicleMaster Model { get; set; }
    }
    public class VehicleMaster
    {
        public int Index { get; set; }
        public long ID { get; set; }
        [Required]
        public int PlaceID { get; set; }

        public string Status { get; set; }

        public string PlaceName { get; set; }
        [Required]
        public string VehicleType { get; set; }
        [Required]
        public string VehicleNumber { get; set; }
    }
    public class VehicleMasterRepo : DAL
    {


        public DataTable Select_VehicleList(long UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_VehicleMasterList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "LIST");
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

        public DataSet Select_VehicleDetails(long id,long UserID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_VehicleMasterList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "DETAILS");
                cmd.Parameters.AddWithValue("@ID ", id);
                cmd.Parameters.AddWithValue("@UserId ", UserID);
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


        public DataTable AddUpdate_Vehicle(VehicleMaster model, long UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_VehicleMasterList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@ID ", model.ID);
                cmd.Parameters.AddWithValue("@PlaceID", model.PlaceID);
                cmd.Parameters.AddWithValue("@VehicleType", model.VehicleType);
                cmd.Parameters.AddWithValue("@VehicleNumber", model.VehicleNumber);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@UserId", UserID);


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