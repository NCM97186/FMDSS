using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.UserManagement.ViewModel
{
    #region [Report]
    #region [OffenceReport]
    public class OneClickAccessLogReportVM
    {
        public long SNo { get; set; }
        public string OldSSOID { get; set; }
        public string NewSSOID { get; set; }
        public string Roles { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; } 
    } 
    #endregion
    #endregion
    
}