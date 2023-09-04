using FMDSS.Entity.FRAViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class SurveyDetailsVM
    {
        public ClaimRequestDetailsVM ClaimRequestDetailsVM { get; set; }
        public tbl_FRA_SurveyDetails SurveyDetails { get; set; } 
    }
}