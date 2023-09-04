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
    public class ADWorkDescription : DAL
    {
        public int Index { get; set; }
        public int WorkDescriptionId { get; set; }

        public string OperationType { get; set; }

        public string WorkDescription { get; set; }
      
        public bool IsactiveView { get; set; }
        public int isActive { get; set; }



        public IList<SelectListItem> GetAD_WorkDescription()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_WorkDescription", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_AD_WorkDescription");

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _result.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[i]["WorkDescriptionId"]), Text = Convert.ToString(dt.Rows[i]["WorkDescription"]) });
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

        public DataTable Select_AD_WorkDescriptions()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_WorkDescription", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllAD_WorkDescription");
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

        public DataTable Select_AD_WorkDescription(int WorkDescriptionId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_WorkDescription", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneAD_WorkDescription");
                cmd.Parameters.AddWithValue("@WorkDescriptionId ", WorkDescriptionId);
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


        public DataTable AddUpdateAD_WorkDescription(ADWorkDescription oWorkDescription)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_WorkDescription", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateAD_WorkDescription");
                cmd.Parameters.AddWithValue("@WorkDescriptionId ", oWorkDescription.WorkDescriptionId);
                cmd.Parameters.AddWithValue("@WorkDescription", oWorkDescription.WorkDescription);
                cmd.Parameters.AddWithValue("@isActive", oWorkDescription.isActive);
               
                
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