using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class ContactUs : DAL
    {

        public long ContactUsId { get; set; }
        public int Index { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]

        public string TextString { get; set; }

        public int IsActive { get; set; }
        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }



        public DataTable Select_ContactUs()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ContactUs_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllContact");
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

        public DataTable Select_Contact(int ContactUsId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ContactUs_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneContact");
                cmd.Parameters.AddWithValue("@ContactUsId", ContactUsId);
                cmd.Parameters.AddWithValue("@Heading", Heading);
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



        public DataTable AddUpdateContactUs(ContactUs oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ContactUs_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateContact");
               
                cmd.Parameters.AddWithValue("@Heading", oPlace.Heading);
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