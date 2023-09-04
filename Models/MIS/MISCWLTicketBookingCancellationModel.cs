#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web; 
using System.ComponentModel.DataAnnotations;
#endregion

namespace FMDSS.Models.MIS
{
    public class MISCWLTicketBookingCancellationModel
    {
        #region Properties

        [Required]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        
        public int ID { get; set; }
        [Display(Name = "Request ID")]
        public string RequestID { get; set; }
        [Display(Name = "Emitra Transaction ID")]
        public string EmitraTransactionID { get; set; }
        [Display(Name = "Emitra Status")]
        public string EmitraStatus { get; set; }
        [Display(Name = "Emitra Status Name")]
        public string EmitraStatusName { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }
        [Display(Name = "IFSC Code")]
        public string IFSCCode { get; set; }
        [Display(Name = "Account Type")] 
        public string AccountType { get; set; }
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }
        [Display(Name = "Entered On")]
        public DateTime EnteredOn { get; set; }
        [Display(Name = "Entered By")]
        public Int64 EnteredBy { get; set; }
        [Display(Name = "User Name")] 
        public string UserName { get; set; }
        [Display(Name = "SSO Id")]
        public string SSOId { get; set; }
        [Display(Name = "Ticket Amount")]
        public decimal TicketAmount { get; set; }
        [Display(Name = "Service Charge")]
        public decimal ServiceCharge { get; set; }
        [Display(Name = "Refund Amount")]
        public decimal RefundAmount { get; set; }

        #endregion Properties 
    }

    public class EmitraStatus
    {
        public int IDNo { get; set; }
        public string Name { get; set; }
    }

    public class CitizenWildLifeCancellationViewModel
    {
        public MISCWLTicketBookingCancellationModel CWLTicketBookingCancellationModel { get; set; }
        public List<MISCWLTicketBookingCancellationModel> TicketBookingCancellationList { get; set; }
        public List<EmitraStatus> EmitraStatusList { get; set; } 
    }
    
    public class MISCWLTicketBookingCancellationRepository:DAL
    {
        #region [Public Methods]
        #region [GetCitizenCancellationDetails]
        public DataSet GetCitizenCancellationDetails(string fromDate, string toDate, string requestID, string emitraStatus)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("MIS_GetCitizenCancellationDetails", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", "GetWildLifeTicketbookCancelDetails");
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                cmd.Parameters.AddWithValue("@RequestID", requestID);
                cmd.Parameters.AddWithValue("@EmitraStatus", emitraStatus);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(ds);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                Conn.Close();
            }

            return ds;
             

        }
        #endregion
        #region GetCitizenCancellationDetailsByID
        public DataSet GetCitizenCancellationDetailsByID(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("MIS_GetCitizenCancellationDetails", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", "GetWildLifeTicketbookCancelDetailsById");
                cmd.Parameters.AddWithValue("@Citizen_WildLifeTicketbookingCancellationId", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(ds);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                Conn.Close();
            }

            return ds;


        }
        #endregion
        #endregion
    }

}