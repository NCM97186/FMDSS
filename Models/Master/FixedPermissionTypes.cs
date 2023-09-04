using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class FixedPermissionTypes : DAL
    {
        public int Index { get; set; }
        public int P_ID { get; set; }

        public string OperationType { get; set; }

        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int EmitraServiceId { get; set; }
        public int Discount { get; set; }
        public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }





        public DataTable Select_FixedPermissionTypess()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FixedPermissionTypes", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllFixedPermissionTypes");
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

        public DataTable Select_FixedPermissionTypes(int P_ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FixedPermissionTypes", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneFixedPermissionTypes");
                cmd.Parameters.AddWithValue("@P_ID", P_ID);
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

        public DataTable AddUpdateFixedPermissionTypes(FixedPermissionTypes oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FixedPermissionTypes", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateFixedPermissionTypes");
                cmd.Parameters.AddWithValue("@P_ID", oPlace.P_ID);
               
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@Status", oPlace.Status);
                cmd.Parameters.AddWithValue("@Description", oPlace.Description);
                cmd.Parameters.AddWithValue("@EmitraServiceId", oPlace.EmitraServiceId);
                cmd.Parameters.AddWithValue("@Discount", oPlace.Discount);
                cmd.Parameters.AddWithValue("@Amount", oPlace.Amount);
                cmd.Parameters.AddWithValue("@Tax", oPlace.Tax);
                          
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
              
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