using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_BudgetAllocation_Circle
    {
        [Key]
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public int FY_ID { get; set; }
        [My.Data.Annotations.Precision(18, 5)]
        public decimal AllocatedAmount { get; set; }

        [My.Data.Annotations.Precision(18, 5)]
        public decimal TotalAmount { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string Division { get; set; }
        public long SchemeID { get; set; }
        public long ActivityID { get; set; }
        public long SubActivityID { get; set; }
        public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public bool isActive { get; set; }
        public long BudgetHeadAllocationID { get; set; }
        public string ISCircleDivision { get; set; }

        public string SiteName { get; set; }

        public int IsRecurring { get; set; }

        public string SanctuaryCode { get; set; }

        public string Unit { get; set; }
        public decimal NumberPerUnit { get; set; }
        public decimal RatePerUnit { get; set; }

        public string IsCoreOrBuffer { get; set; }
    }

    public class tbl_BudgetAllocation_CircleLevels
    {
        [Key]
        public long ID { get; set; }
        public long tbl_BudgetAllocation_CircleID { get; set; }

        public long tbl_BudgetAllocation_ParentID { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }

        public int Status { get; set; }

        public Decimal AllocatedAmountCircleLevel { get; set; }

        public Decimal AllocatedAmountDistribute { get; set; }

        public Decimal ExtraAllocatedAmount { get; set; }


    }


   
}