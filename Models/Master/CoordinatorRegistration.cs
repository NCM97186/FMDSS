using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
     public class CoordinatorRegistration:DAL
     {
        public Int64 ID { get; set; }
        public int Index { get; set; }
        public int DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public string Name { get; set; }
        public string SSOID { get; set; }    
        public string Address { get; set; }
        public string Pincode { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public Int64 EnteredBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string OperationType { get; set; }


        public DataTable Select_CoordinatorRegistrationS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_CoordinatorRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectAllCoordinatorRegistration");
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

        public DataTable Select_CoordinatorRegistration(Int64 ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_CoordinatorRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneCoordinatorRegistration");
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

        public DataTable GETDistrictsForCoordinatorRegistration()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_CoordinatorRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAllDistrictsForCoordinatorRegistration");
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

        public DataTable AddUpdateCoordinatorRegistration(CoordinatorRegistration oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_CoordinatorRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateCoordinatorRegistration");

                cmd.Parameters.AddWithValue("@ID", oPlace.ID);                
                cmd.Parameters.AddWithValue("@DIST_CODE", oPlace.DIST_CODE);
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@SSOID", oPlace.SSOID);
                cmd.Parameters.AddWithValue("@Address", oPlace.Address);
                cmd.Parameters.AddWithValue("@Pincode", oPlace.Pincode);
                cmd.Parameters.AddWithValue("@isActive", oPlace.Isactive);
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

    }
}