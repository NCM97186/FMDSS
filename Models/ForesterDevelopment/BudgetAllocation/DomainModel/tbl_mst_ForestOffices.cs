using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_ForestOffices
    {
        [Key]
        public int ROWID { get; set; }

        public string DIV_CODE { get; set; }

        public string Office_ID { get; set; }

        public string OfficeName { get; set; }
    }
}