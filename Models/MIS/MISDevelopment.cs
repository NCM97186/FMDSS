using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.MIS
{
    public class MISDevelopment : DAL
    {
        public int Index { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string HIERARCHY_CODE { get; set; }
        public string ID { get; set; }
        public string RANGE_NAME { get; set; }
        
        public string WorkOrder_Code { get; set; }
        public string WorkOrder_Name { get; set; }
        public string Placeofwork { get; set; }
        public string IFMC_WorkOrder_Code { get; set; }
        public string ContractAgencyType { get; set; }

        public string WorkOrderType { get; set; }
        public string EnteredOn { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WorkorderStatus { get; set; }
        public string SurveyReportStatus { get; set; }
        public string WorkOrderProgressEntry { get; set; }
        public string PaymentStatus { get; set; }
        public string DetailsType { get; set; }


        public string AmountofWorkorder { get; set; }
        public string PaymentAmount { get; set; }
        public string OfficeLevel { get; set; }


        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }




        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }


        public DataTable BASE_DETAILS(MISDevelopment obj)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MIS_WorkOrderSurvyaAndProgressEntry", Conn);
                cmd.Parameters.AddWithValue("@Action", "BASE_DETAILS");
                cmd.Parameters.AddWithValue("@FromDate", obj.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", obj.ToDate);
                cmd.Parameters.AddWithValue("@Code", obj.HIERARCHY_CODE);
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

        public DataSet GetWorkorderDetails(MISDevelopment obj)
        {
            try
            {

                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MIS_WorkOrderSurvyaAndProgressEntry", Conn);
                cmd.Parameters.AddWithValue("@Action", obj.DetailsType);
                cmd.Parameters.AddWithValue("@WorkOrderID", obj.WorkOrder_Code);
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