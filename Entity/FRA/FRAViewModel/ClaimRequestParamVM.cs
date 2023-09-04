using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class ClaimRequestParamVM
    {
        public int ClaimTypeID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DistrictID { get; set; }
        public string BlockID { get; set; }
        public string GPID { get; set; }
    }

    public class ClaimRequestSubParamVM
    {
        public string ActionCode { get; set; }
        public int ClaimTypeID { get; set; }
        public int DistrictID { get; set; }
        public string BlockID { get; set; }
        public string GPID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}