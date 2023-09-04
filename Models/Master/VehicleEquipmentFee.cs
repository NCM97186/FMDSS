using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class VehicleEquipmentFee : DAL
    {
        public Int64 VehicleID { get; set; }
        public int Index { get; set; }
        public Int64 CategoryID{ get; set; }

        public string CategoryName { get; set; }
        public string Name { get; set; }
        public decimal Fees { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public string EnteredOn { get; set; }
        public string EnteredBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string OperationType { get; set; }

        public DataTable Select_VehicleEquipmentFee()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicleEquipmentFee");
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

        public DataTable Select_VehicleEquipmentFee(int VehicleID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneVehicleEquipmentFee");
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
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

        public DataTable AddUpdateVehicleEquipmentFee(VehicleEquipmentFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateVehicleEquipmentFee");
                cmd.Parameters.AddWithValue("@VehicleID", oPlace.VehicleID);
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.CategoryID);
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@Fees", oPlace.Fees);
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

        public Int64 DeleteVehicleEquipmentFee()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.Parameters.AddWithValue("@Action", "DeleteVehicleEquipmentFee");
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
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

        public DataTable SelectAllVehicleEquipmentCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicleEquipmentCategory");
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
                SqlCommand cmd = new SqlCommand("Sp_VehicleEquipmentFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateVehicleEquipmentFee");
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@Name", Name);

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