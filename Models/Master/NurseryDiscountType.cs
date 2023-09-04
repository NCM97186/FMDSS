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
    public class NurseryDiscountType : DAL
    {
        public int Index { get; set; }
        public int ID { get; set; }

        public string OperationType { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Value")]
        public double Value { get; set; }

        [Required]
        [Display(Name = "VID")]
        public int VID { get; set; }
        [Required]
        [Display(Name = "ValueType_Name")]
        public string ValueType_Name { get; set; }
      
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }



        public IList<SelectListItem> Get_NurseryDiscountType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_NurseryDiscountType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_NurseryDiscountType");

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _result.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[i]["ID"]), Text = Convert.ToString(dt.Rows[i]["Name"]) });
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

        public DataTable Select_NurseryDiscountTypes()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_NurseryDiscountType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAll_NurseryDiscountType");
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

        public DataTable Select_NurseryDiscountType(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_NurseryDiscountType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOne_NurseryDiscountType");
                cmd.Parameters.AddWithValue("@ID ", ID);
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


        public DataTable AddUpdate_NurseryDiscountType(NurseryDiscountType oNDT)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_NurseryDiscountType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdate_NurseryDiscountType");
                cmd.Parameters.AddWithValue("@ID", oNDT.ID);
                cmd.Parameters.AddWithValue("@Name", oNDT.Name);
                cmd.Parameters.AddWithValue("@Value", oNDT.Value);
                cmd.Parameters.AddWithValue("@VID", oNDT.VID);
                cmd.Parameters.AddWithValue("@IsActive", oNDT.IsActive);
               
                
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

        public DataTable SelectAllValueTypeName()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_NurseryDiscountType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllValueTypeName");
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