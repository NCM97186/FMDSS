using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Models.CitizenRefunds
{
    public class CitizenRefunds : DAL
    {       
        public List<CitizenRefundDetails> GetCitizenRefundDetails(string ssoid,int CancellationReason)
        {
            List<CitizenRefundDetails> citizenRefundDetails = new List<CitizenRefundDetails>();
            DataTable dt = new DataTable();

            DALConn();
            SqlParameter[] parameters =
                {
            new SqlParameter("@Action", "GetPartialRefundList"),
            new SqlParameter("@SSO_Id", ssoid),
            new SqlParameter("@CancellationReason", CancellationReason), //7 IS USED FOR Partial Cancelliation           
            };

            Fill(dt, "SP_PartialRefundActions", parameters);

            citizenRefundDetails = Globals.Util.GetListFromTable<CitizenRefundDetails>(dt);
            return citizenRefundDetails;
        }
        public string SaveBankDetails(string ssoid, BankDetails bankDetails,out bool status)
        {          
            DataTable dt = new DataTable();
            status = false;
            DALConn();
            SqlParameter[] parameters =
                {
            new SqlParameter("@Action", "SaveBankDetails"),
            new SqlParameter("@SSO_Id", ssoid),
            new SqlParameter("@TicketID", bankDetails.TicketId),
            new SqlParameter("@AccountNo", bankDetails.AccountNo),
            new SqlParameter("@BankName", bankDetails.BankName),
            new SqlParameter("@BranchName", bankDetails.BranchName),
            new SqlParameter("@IFSCCode", bankDetails.IFSCCode),
            new SqlParameter("@AccountType", bankDetails.AccountType),
            new SqlParameter("@AccountHolderName", bankDetails.AccountHolderName),
            new SqlParameter("@RefundAbleAmt", bankDetails.RefundableAmount),            
            new SqlParameter("@ConfirmRefundByCitizen",( bankDetails.ConfirmRefundByCitizen ==true ? 1:0)),
            };

            Fill(dt, "SP_PartialRefundActions", parameters);
            if (dt.Rows[0][1].ToString() == "1")
                status = true;

            return dt.Rows[0][0].ToString();
        }
        public BankDetails GetBankDetails(string ssoid, long TicketId)
        {
            List<BankDetails> bankDetails = new List<BankDetails>();
            DataTable dt = new DataTable();          
            DALConn();
            SqlParameter[] parameters =
                {
            new SqlParameter("@Action", "GetAppliedBankDetail"),
            new SqlParameter("@SSO_Id", ssoid),
            new SqlParameter("@TicketID", TicketId),            
            };

            Fill(dt, "SP_PartialRefundActions", parameters);
            bankDetails = Globals.Util.GetListFromTable<BankDetails>(dt);
            return bankDetails.ToList().FirstOrDefault();
        }
        
    }
    public class CitizenRefundViews
    {
        public List<CitizenRefundDetails> citizenRefundDetails { get; set; }
        public List<BankDetails> bankDetailList { get; set; }
        public BankDetails bankDetail { get; set; }
    }
    public class CitizenRefundDetails
    {
        public int SNo { get; set; }
        public string RequestID { get; set; }
        public long TICKETID { get; set; }
        public int TotalMembers { get; set; }
        public string DateOfArrival { get; set; }
        public decimal RefundableAmt { get; set; }
        public long AppliedTicketId { get; set; }
        public string BookingDate { get; set; }
        public string PlaceName { get; set; }
        public string AppliedStatus { get; set; }          
    }
    public class BankDetails
    {
        public long ApId { get; set; }
        public string RequestId { get; set; }
        public long TicketId { get; set; }
        public decimal RefundableAmount { get; set; }
        public string AppliedDate { get; set; }
        public long UserId { get; set; }
        [Required(ErrorMessage ="Bank Name Required")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Branch Name Required")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "IFSC CODE Required")]
        public string IFSCCode { get; set; }
        [Required(ErrorMessage = "Account Holder Name Required")]
        public string AccountHolderName { get; set; }
        [Required(ErrorMessage = "Account Type Required")]
        public string AccountType { get; set; }
        [Required(ErrorMessage = "Account Number Required")]
        public string AccountNo { get; set; }        
        public bool ConfirmRefundByCitizen { get; set; }
        
    }
}