using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;


namespace FMDSS.Models.Master
{
    public class EmitraKioskLinkDirectOrBypass : DAL
    {

        public int Index { get; set; }
        public int ID { get; set; }

        [Required]
        public string SERVICENAME { get; set; }
        
        [Required]
        [AllowHtml]
        [DisplayName("DIRECT LINK")]
        public string DIRECT_LINK { get; set; }
        public Int16 DIRECT_LINK_IsActive { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("RAJ SEVADWAR LINK")]
        public string RAJSEVADWAR_LINK { get; set; }
        public Int16 RAJSEVADWAR_LINK_IsActive { get; set; }
        [Required]
        [DisplayName("MAX RESPONSE TIME IN SECOND")]
        public Int16 MAX_RESPONSE_TIME_SEC { get; set; }
        public Int16 IsActive { get; set; }

        public bool IsactiveView { get; set; }
        


        public DataTable Select_EmitraKioskLinkDirectOrBypass()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MasterEmitraKioskLinkDirectOrBypass", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllEmitraKioskLinkDirectOrBypass");
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

        public DataTable Select_EmitraKioskLinkDirectOrBypass(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MasterEmitraKioskLinkDirectOrBypass", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneEmitraKioskLinkDirectOrBypass");
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

        public DataTable AddUpdateTicker(EmitraKioskLinkDirectOrBypass oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MasterEmitraKioskLinkDirectOrBypass", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateEmitraKioskLinkDirectOrBypass");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@SERVICENAME", oPlace.SERVICENAME);
                cmd.Parameters.AddWithValue("@DIRECT_LINK", oPlace.DIRECT_LINK);
                cmd.Parameters.AddWithValue("@DIRECT_LINK_IsActive", oPlace.DIRECT_LINK_IsActive);
                cmd.Parameters.AddWithValue("@RAJSEVADWAR_LINK", oPlace.RAJSEVADWAR_LINK);
                cmd.Parameters.AddWithValue("@RAJSEVADWAR_LINK_IsActive", oPlace.RAJSEVADWAR_LINK_IsActive);
                cmd.Parameters.AddWithValue("@MAX_RESPONSE_TIME_SEC", oPlace.MAX_RESPONSE_TIME_SEC);
                
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