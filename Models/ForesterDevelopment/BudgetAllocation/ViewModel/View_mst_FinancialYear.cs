using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_mst_FinancialYear:BaseModelSerializable
    {
        public long ID { get; set; }

        public string FinancialYear { get; set; }

        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}