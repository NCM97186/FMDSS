using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.E_SignIntegration
{
    public class clsUploadTextFile
    {
        public string merchantCode { get; set; }
        public string bankCode { get; set; }
        public string fileName { get; set; }
        public string fileContent { get; set; }
        public string remitterAccountNumber { get; set; }
        public string totalTransactionCount { get; set; }
    }

    public class clsUploadTextFileResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}