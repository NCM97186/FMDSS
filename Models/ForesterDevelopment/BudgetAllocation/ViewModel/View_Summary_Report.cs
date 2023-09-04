using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_Summary_Report:BaseModelSerializable
    {
        public string State { get; set; }
        public long FinancialYear { get; set; }
        public decimal AllocatedAmount { get; set; }
        public string Circle_Name { get; set; }
        public string Circle_Code { get; set; }

        public string Division_Name { get; set; }
        public string Division_Code { get; set; }

        public string Range_Name { get; set; }
        public string Range_Code { get; set; }

        public string Village_Name { get; set; }
        public string Village_Code { get; set; }

        public string Activity_Name { get; set; }
        public string Activity_Code { get; set; }

        public string SubActivity_Name { get; set; }
        public string SubActivity_Code { get; set; }

        public string Scheme_Name { get; set; }
        public string Scheme_Code { get; set; }

      
    }
}