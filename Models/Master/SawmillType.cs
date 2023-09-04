using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FMDSS.Models.Master
{
    public class SawmillType : DAL
    {
        public int Index { get; set; }
        public int SID { get; set; }

        public string OperationType { get; set; }

        public string SName { get; set; }
        public string SawmillDefinition { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

         
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }


        public IList<SelectListItem> GetSawmillType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Sawmill_Type", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_SawmillType");

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _result.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[i]["SID"]), Text = Convert.ToString(dt.Rows[i]["SName"]) });
                }



                // return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return _result;
        }

        public DataTable Select_SawmillTypes()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Sawmill_Type", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllSawmillType");
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

        public DataTable Select_SawmillType(int SID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Sawmill_Type", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneSawmillType");
                cmd.Parameters.AddWithValue("@SID ", SID);
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


        public DataTable AddUpdateSawmillType(SawmillType oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Sawmill_Type", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateSawmillType");
                cmd.Parameters.AddWithValue("@SID ", oPlace.SID);
                cmd.Parameters.AddWithValue("@SName", oPlace.SName);
                cmd.Parameters.AddWithValue("@SawmillDefinition", oPlace.SawmillDefinition);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", oPlace.LastUpdatedBy);
                
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