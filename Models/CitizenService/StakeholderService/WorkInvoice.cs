using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
 


namespace FMDSS.Models.StakeholderService
{

    public class WorkInvoice : DAL
    {
        public string Div_Code { get; set; }
        public string WorkOrderID { get; set; }
        public string MilestoneName { get; set; }
        public string MilestonePaymentPercent { get; set; }
        public string MilestoneID { get; set; }
       
        public string BillVoucherAmount { get; set; }
        public string BillVoucherNo { get; set; }
        public string BillVoucherDate { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public string ProgressStatus { get; set; }
        public string WorkOrder_Desc { get; set; }
        public string UserID { get; set; }
        /// <summary>
        /// Function to bind workorder
        /// </summary>
        /// <returns></returns>
        public DataSet BindWorkOrder()
        {
            DataSet dsWorkOrder = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","1"),                   
                new SqlParameter("@SSOId",HttpContext.Current.Session["SSOid"]),    
                new SqlParameter("@WorkOrderId",Convert.ToInt64("0")), 
                new SqlParameter("@MilestoneId",Convert.ToInt64("0")), 
                };
                Fill(dsWorkOrder, "SP_Citizen_WorkOrder", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindWorkOrder" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsWorkOrder;
        }
        /// <summary>
        /// Function to bind milestone
        /// </summary>
        /// <param name="workorderid"></param>
        /// <returns></returns>
        public DataSet BindMileStone(Int64 workorderid)
        {
            DataSet dsMileStone = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","2"),                   
                new SqlParameter("@SSOId",Convert.ToInt64("0")),    
                new SqlParameter("@WorkOrderId",workorderid), 
                new SqlParameter("@MilestoneId",Convert.ToInt64("0")), 
                };
                Fill(dsMileStone, "SP_Citizen_WorkOrder", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindMileStone" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
        
            }
            finally
            {
                Conn.Close();
            }
            return dsMileStone;
        }


        /// <summary>
        /// function to bind activity,sub activity and status
        /// </summary>
        /// <param name="workorderid"></param>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public DataSet BindActivitydetails(Int64 workorderid, Int64 milestone)
        {
            DataSet dsActivity = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","3"),                   
                new SqlParameter("@SSOId",Convert.ToInt64("0")),    
                new SqlParameter("@WorkOrderId",workorderid), 
                new SqlParameter("@MilestoneId",milestone), 
                };
                Fill(dsActivity, "SP_Citizen_WorkOrder", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindActivitydetails" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsActivity;
        }

        public int SaveInvoiceDetails(double BillAmount, string BillDate, Int64 milestone, Int64 UserID)
        {
            int i = 0;
            try
            {
                DALConn();
                DataSet dsActivity = new DataSet();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","4"),                   
     
                new SqlParameter("@MilestoneId",milestone), 
                new SqlParameter("@BillAmount",BillAmount),    
                new SqlParameter("@userID",UserID),  
                new SqlParameter("@BillDate",DateTime.ParseExact(BillDate,"dd/MM/yyyy",null)), 
                };
                  i = ExecuteNonQuery("SP_Citizen_WorkOrder", parameters);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveInvoiceDetails" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
    
            }
            finally
            {
                Conn.Close();
            }
            return i;
        }


        /// <summary>
        /// Function to bind milestone
        /// </summary>
        /// <param name="SSOID"></param>
        /// <returns></returns>
        public DataSet GetInvoiceList(string SSOID)
        {
            DataSet dsInvoice = new DataSet();
            try
            {
                DALConn();
             
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","5"),                   
                new SqlParameter("@SSOId",SSOID),    
  
                };
                Fill(dsInvoice, "SP_Citizen_WorkOrder", parameters);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetInvoiceList" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
    
          
            }
            finally
            {
                Conn.Close();
            }
            return dsInvoice;
        }
        public DataSet GetPDFdetail(Int64 MilestoneId)
        {
            DataSet dsInvoice = new DataSet();
            try
            {
                DALConn();
               
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","6"),                   
                new SqlParameter("@MilestoneId",MilestoneId),    
  
                };
                Fill(dsInvoice, "SP_Citizen_WorkOrder", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetPDFdetail" + "_" + "ForestDevelopment", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
    
            }
            finally
            {
                Conn.Close();
            }
            return dsInvoice;
        }


       
    }
}