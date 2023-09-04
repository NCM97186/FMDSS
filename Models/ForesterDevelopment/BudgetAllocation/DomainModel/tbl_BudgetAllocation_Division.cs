using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_BudgetAllocation_Division
    {
        [Key]
        public long ID { get; set; }

        public long BudgetHeadID { get; set; }

        public long SubBudgetHeadID { get; set; }

        public int FY_ID { get; set; }

        public decimal AllocatedAmount { get; set; }

        public string DIV_CODE { get; set; }

        public long SchemeID { get; set; }

        public long ActivityID { get; set; }

        public long SubActivityID { get; set; }

        public long EnteredBy { get; set; }
        public bool isActive { get; set; }
        public long BudgetHeadAllocationID { get; set; }
        public string ISCircleDivision { get; set; }

    }
}