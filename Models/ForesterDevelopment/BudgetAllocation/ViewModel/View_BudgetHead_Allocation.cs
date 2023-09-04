using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_BudgetHead_Allocation:BaseModelSerializable
    {
        public long ID { get; set; }

        public long BudgetHeadID { get; set; }

        public long SubBudgetHeadID { get; set; }

        public int FY_ID { get; set; }

        public decimal TotalAllocatedAmount { get; set; }

        public decimal AllocatedAmount { get; set; }

        public long SchemeID { get; set; }
        public string SchemeName { get; set; }

        public long ActivityID { get; set; }
        public string ActivityName { get; set; }

        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public string rowid { get; set; }
        public decimal AvailableAmount { get; set; }     
        public string BudgetHead { get; set; }
        public string SubBudgetHead { get; set; }

        public string FinancialYear { get; set; }

        public bool isActive { get; set; }

        public string Unit { get; set; }

        public decimal RatePerUnit { get; set; }

        public decimal NumberPerUnit { get; set; }

    }
}