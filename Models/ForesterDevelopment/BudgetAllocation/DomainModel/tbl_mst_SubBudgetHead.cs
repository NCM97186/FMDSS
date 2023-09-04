using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_SubBudgetHead
    {
        [Key]
        public long ID { get; set; }

        public long BudgetHeadID { get; set; }

        public string SubBudgetHead { get; set; }
       
        public long EnterBy { get; set; }

        public string Descriptions { get; set; }

    }
}