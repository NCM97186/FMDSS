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
    public class BaseProduceType : DAL
    {
        public int Index { get; set; }
        public int ID { get; set; }
      
        [Required]
        [Display(Name = "Product Type")]
        public string ProductTypeID { get; set; }
        public string OperationType { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }
      
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }



        public IList<SelectListItem> Get_BaseProduceType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_baseProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_BaseProduceType");

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

        public DataTable Select_BaseProduceTypes()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_baseProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAll_BaseProduceType");
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

        public DataTable Select_BaseProduceType(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_baseProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOne_BaseProduceType");
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


        public DataTable AddUpdate_BaseProduceType(BaseProduceType oBPT)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_mst_baseProduceType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdate_BaseProduceType");
                cmd.Parameters.AddWithValue("@ID", oBPT.ID);
                cmd.Parameters.AddWithValue("@Name", oBPT.Name);
                cmd.Parameters.AddWithValue("@Price", oBPT.Price);
                cmd.Parameters.AddWithValue("@IsActive", oBPT.IsActive);
                cmd.Parameters.AddWithValue("@ProduceTypeID", oBPT.ProductTypeID);
                
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