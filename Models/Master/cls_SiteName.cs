using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class cls_SiteName:DAL
    {
        public int SiteId { get; set; }
        public int cINDEX { get; set; }
        public string Circle_Code { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string Div_Code { get; set; }
        public string DIV_NAME { get; set; }
        public string Range_Code { get; set; }
        public string RANGE_NAME { get; set; }
        public string SiteName { get; set; }
        public bool isActive { get; set; }

        public List<cls_SiteName> List { get; set; }
        public cls_SiteName Model { get; set; }
        public DataTable Select_SiteList()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSite", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GETSITELIST");
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

        public DataSet Select_SiteDetails(long id)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("spSite", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SELECTONE");
                cmd.Parameters.AddWithValue("@SiteID", id);
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


        public DataTable AddUpdate_Site(cls_SiteName model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSite", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd); 
                cmd.Parameters.AddWithValue("@Action", "INSERTUpdate");
                cmd.Parameters.AddWithValue("@SiteId", model.SiteId);
                cmd.Parameters.AddWithValue("@Range_Code", model.Range_Code);
                cmd.Parameters.AddWithValue("@SiteName", model.SiteName);
                cmd.Parameters.AddWithValue("@isActive", model.isActive);
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

        public DataTable GetDivision(string Circle_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spSite", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetDivision");
                cmd.Parameters.AddWithValue("@Circle_Code", Circle_Code);
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