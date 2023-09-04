using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class cls_AppSetting:DAL
    {
        public int AppSettingID { get; set; }
        public int cINDEX { get; set; }
        public string SettingType { get; set; }
        public string AppKey { get; set; }
        public string AppValue { get; set; }
        public string Description { get; set; }
        public bool ActiveStatus { get; set; }

        public List<cls_AppSetting> List { get; set; }
        public cls_AppSetting Model { get; set; }
        public DataTable Select_AppSettingList()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spAppSetting", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SELECTALL");
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

        public DataSet Select_AppDetails(long id)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("spAppSetting", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectONE");
                cmd.Parameters.AddWithValue("@AppSettingID", id);
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


        public DataTable AddUpdate_AppDetails(cls_AppSetting model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spAppSetting", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "INSERTUpdate");
                cmd.Parameters.AddWithValue("@AppSettingID ", model.AppSettingID);
                cmd.Parameters.AddWithValue("@SettingType", model.SettingType);
                cmd.Parameters.AddWithValue("@AppKey", model.AppKey);
                cmd.Parameters.AddWithValue("@AppValue", model.AppValue);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@ActiveStatus", model.ActiveStatus);
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