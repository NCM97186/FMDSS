using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models
{
    public class cls_ReviewerApprover
    {
        
        public string PermissionId { get; set; }
        public string SubPermissionId { get; set; }
        public string OfficeLevelId { get; set; }

         

    }
}