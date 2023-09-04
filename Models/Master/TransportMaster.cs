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
    public class TransportMaster : DAL
    {
        public int Index { get; set; }
        public int TransportId { get; set; }

        public string OperationType { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Details")]
        public string Details { get; set; }
        public bool IsactiveView { get; set; }
        public int isActive { get; set; }




        public IList<SelectListItem> GetTransportMaster()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TransportMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_TransportMaster");

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                for(int i=0;i<dt.Rows.Count;i++)
                {
                    _result.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[i]["TransportId"]), Text = Convert.ToString(dt.Rows[i]["Name"]) });
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


        public DataTable Select_TransportMasters()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TransportMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllTransportMaster");
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

        public DataTable Select_TransportMaster(int TransportId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TransportMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneTransportMaster");
                cmd.Parameters.AddWithValue("@TransportId ", TransportId);
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


        public DataTable AddUpdateTransportMaster(TransportMaster oObj)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TransportMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateTransportMaster");
                cmd.Parameters.AddWithValue("@TransportId ", oObj.TransportId);
                cmd.Parameters.AddWithValue("@Name", oObj.Name);
                cmd.Parameters.AddWithValue("@Details", oObj.Details);
                cmd.Parameters.AddWithValue("@isActive", oObj.isActive);
                
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