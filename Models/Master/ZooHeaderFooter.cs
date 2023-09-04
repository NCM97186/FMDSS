using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
   
    public class ZooHeaderFooter : DAL
    {

        public Int64 Id{ get; set; }
        public int Index { get; set; }
        public Int64 PlaceID{ get; set; }
       
       
        public string PlaceName { get; set; }

        public string HeadeText { get; set; }
        public string FooterText { get; set; }
             
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public DataTable Select_ZooHeaderFooters()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeaderFooter", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZooHeaderFooter");
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

        public DataTable Select_ZooHeaderFooter(int Id)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeaderFooter", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooHeaderFooter");
                cmd.Parameters.AddWithValue("@Id", Id);
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

        public DataTable AddUpdateZooHeaderFooter(ZooHeaderFooter oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooHeaderFooter", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooHeaderFooter");
                cmd.Parameters.AddWithValue("@Id", oPlace.Id);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@HeadeText", oPlace.HeadeText);
                cmd.Parameters.AddWithValue("@FooterText", oPlace.FooterText);
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
                SqlCommand cmd = new SqlCommand("Sp_ZooHeaderFooter", Conn);
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