using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.E_SignIntegration
{
    #region GetParticular Transation Status
    public class clsGetTransationStatus
    {
        public string fileName { get; set; }
        public string outCode { get; set; }
    }

    public class respDetails
    {
        public string respTimestamp { get; set; }
        public string respStatusDescription { get; set; }
        public string respStatus { get; set; }
        public string respBid { get; set; }

    }

    public class tackDetails
    {
        public string tackStatusDescription { get; set; }
        public string tackTimestamp { get; set; }
        public string tackBid { get; set; }
        public string tackStatus { get; set; }

    }

    public class ackDetails
    {
        public string ackStatus { get; set; }
        public string ackTimestamp { get; set; }
        public string ackStatusDescription { get; set; }

    }

    public class transactionsDetails
    {
        public transactionsDetails()
        {
            ackDetails = new ackDetails();
            tackDetails = new tackDetails();
            respDetails = new respDetails();
        }
        public string disbursementRefNo { get; set; }
        public string amount { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryName { get; set; }

        public string ifsc { get; set; }
        public string finalStatus { get; set; }
        public string remarks1 { get; set; }
        public string remarks2 { get; set; }

        public string dueDate { get; set; }

        public ackDetails ackDetails { get; set; }
        public tackDetails tackDetails { get; set; }
        public respDetails respDetails { get; set; }
    }

    public class clsGetTransationStatusResponse
    {
        public clsGetTransationStatusResponse()
        {
            transactions = new List<transactionsDetails>();
        }

        public string status { get; set; }
        public string message { get; set; }

        public List<transactionsDetails> transactions { get; set; }
    }

    #endregion

    #region Get All Transation Status

    public class clsAllTransationStatusRequest
    {
        public string fileName { get; set; }
        public string allRecords { get; set; }
    }

    public class TransationStatusDetails
    {
        public string DISBURSEMENTREFNO { get; set; }
        public string OUTCODE { get; set; }
        public string AMOUNT { get; set; }
        public string IFSC { get; set; }
        public string BENEFICIARYACCOUNTNUMBER { get; set; }
        public string DUEDATE { get; set; }
        public string ACKSTATUS { get; set; }
        public string ACKSTATUSDESCRIPTION { get; set; }
        public string TACKSTATUS { get; set; }
        public string TACKSTATUSDESCRIPTION { get; set; }

        public string TACKBID { get; set; }
        public string RESPSTATUS { get; set; }
        public string RESPSTATUSDESCRIPTION { get; set; }
        public string RESPBID { get; set; }
        public string ISNEFT { get; set; }

        public string CREATEDAT { get; set; }
        public string FINAL_STATUS { get; set; }
        public string FINAL_REMARK { get; set; }
    }
    public class clsAllTransationStatusResponse
    {
        public clsAllTransationStatusResponse()
        {
            data = new List<TransationStatusDetails>();
        }
        public string status { get; set; }
        public string message { get; set; }

        public List<TransationStatusDetails> data { get; set; }
    }

    #endregion
}