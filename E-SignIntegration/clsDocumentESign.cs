using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.E_SignIntegration
{
    public class inputJson
    {
        public string File { get; set; }
    }
    public class clsDocumentESign
    {
        public clsDocumentESign()
        {
            inputJson = new inputJson();
            filetype = "PDF";
            status = "SelfAttested";
            docname = "abc";
            designation = "DCF";
        }

        public inputJson inputJson { get; set; }
        public string filetype { get; set; }
        public string transactionid { get; set; }
        public string docname { get; set; }
        public string designation { get; set; }
        public string status { get; set; }
        public string llx { get; set; }
        public string lly { get; set; }
        public string positionX { get; set; }
        public string positionY { get; set; }
    }

    public class clsDocumentESignResponce
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Document { get; set; }
    }


    public class clsDocumentESignByEmitra
    {
        public string transactionid { get; set; }
        public string filecontant { get; set; }
    }
    public class clsDocumentESignByEmitraResponse
    {
        public string Status { get; set; }
        public string file { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}