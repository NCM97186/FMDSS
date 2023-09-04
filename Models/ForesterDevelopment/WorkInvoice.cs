using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace FMDSS.Models.ForesterDevelopment
{

    public class WorkInvoice:DAL
    {
        public string Div_Code { get; set; }
        public string WorkOrderID { get; set; }
        public string MilestoneName { get; set; }
        public string BillVoucherAmount { get; set; }
        
        public string BillVoucherNo { get; set; }
        public string BillVoucherDate { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public string ProgressStatus { get; set; }
     

        public DataTable BindMileStone(Int64 workorderid)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("select Id as MilestoneId,MilestoneName FROM tbl_FDM_Workorder_Milestone where WorkorderID=" + workorderid, Conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable SelectWorkOrderByDivisionCode(string Div_Code)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_WorkOrderByDivCode", Conn);
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        public DataTable BindActivitydetails(Int64 milestone)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("select act.Activity_Name,sact.Sub_Activity_Name,wp.ProgressStatus,wo.FinancialTarget  from tbl_fdm_MilestoneActivity mil " +
                                                "left join tbl_mst_FDM_Activity act on mil.ActivityId=act.ID "+
                                                "left join tbl_mst_FDM_Sub_Activity sact on mil.subactivityid=sact.ID "+
                                                "left join tbl_FDM_Workorderprogress wp on mil.workorderid=wp.WorkOrderID and mil.activityid = wp.Activity and mil.subactivityid=wp.SubActivity " +
                                                "left join tbl_FDM_Workorder wo on mil.WorkOrderID = wo.ID where mil.milestoneid=" + milestone, Conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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