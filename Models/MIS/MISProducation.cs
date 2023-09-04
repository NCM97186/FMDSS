using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.MIS
{
    public class MISProducationInventory:DAL
    {
        public int Index { get; set; }
     
        public string RANGE_NAME { get; set; }
        public string HIERARCHY_CODE { get; set; }
        public string RANGE_CODE { get; set; }
        
        public string DEPOT_NAME { get; set; }
        public string PRODUCETYPE { get; set; }


        public string PRODUCTNAME { get; set; }

        public string UNITNAME { get; set; }
        public string LOTCOUNT { get; set; }
        public string PRODUCE_QTY { get; set; }

        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }




        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }


        public DataTable GET_INVENTORY(string CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MIS_FPD_INVENTORY", Conn);
                cmd.Parameters.AddWithValue("@CODE", CODE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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


    public class MISProducationNotice: DAL
    {
        public int Index { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string RANGE_NAME { get; set; }
        public string HIERARCHY_CODE { get; set; }

        public string RANGE_CODE { get; set; }
        public string Notice_Number { get; set; }
        
        public string Quantity { get; set; }


        public string ReservedPrice { get; set; }

        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }
        public string NoticeCreatedBy { get; set; }
        public string NoticeApprovalStatus { get; set; }
        public string NoticePublishStatus { get; set; }


        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }

        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }

        public DataTable GET_Notice(MISProducationNotice obj)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MIS_FPD_NOTICE", Conn);
                //cmd.Parameters.AddWithValue("@CODE", obj.RANGE_CODE);//Old
                cmd.Parameters.AddWithValue("@CODE", obj.Range);
                cmd.Parameters.AddWithValue("@FromDate", obj.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", obj.ToDate);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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


