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
    public class ADCategory : DAL
    {
        public int Index { get; set; }
        public int AwardCategoryId { get; set; }

        public string OperationType { get; set; }

        public string CategoryName { get; set; }


        public decimal Amount { get; set; }



        public IList<SelectListItem> GetADCategory()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_Category", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_AD_Category");

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _result.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[i]["AwardCategoryId"]), Text = Convert.ToString(dt.Rows[i]["CategoryName"]) });
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

        public DataTable Select_AD_Categorys()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_Category", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllAD_Category");
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

        public DataTable Select_AD_Category(int AwardCategoryId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_Category", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneAD_Category");
                cmd.Parameters.AddWithValue("@AwardCategoryId ", AwardCategoryId);
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


        public DataTable AddUpdateAD_Category(ADCategory oCategory)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_Category", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateAD_Category");
                cmd.Parameters.AddWithValue("@AwardCategoryId ", oCategory.AwardCategoryId);
                cmd.Parameters.AddWithValue("@CategoryName", oCategory.CategoryName);
                cmd.Parameters.AddWithValue("@Amount", oCategory.Amount);
               
                
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