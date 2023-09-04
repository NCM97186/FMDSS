using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class ImportLink : DAL
    {

        public long ImportID { get; set; }
        public int Index { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string TextString { get; set; }
        [Required]
        


        public int IsActive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }



        public DataTable Select_ImportLinks()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ImportLink_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllImportLink");
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

        public DataTable Select_ImportLink(int ImportID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ImportLink_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneImportLink");
                cmd.Parameters.AddWithValue("@ImportID", ImportID);
                cmd.Parameters.AddWithValue("@Link", Link);
                cmd.Parameters.AddWithValue("@TextString", TextString);
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



        public DataTable ADDUpdateImportLink(ImportLink oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ImportLink_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateImportLink");
                cmd.Parameters.AddWithValue("@ImportID", oPlace.ImportID);
                cmd.Parameters.AddWithValue("@Link", oPlace.Link);
                cmd.Parameters.AddWithValue("@TextString", oPlace.TextString);
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

        

        
    }
}