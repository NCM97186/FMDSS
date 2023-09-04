using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_Budget_Expenditure
    {
        [Key]
        public long Id { get; set; }

        public int FY_ID { get; set; }

        public long SchemeID { get; set; }

        public long ActivityID { get; set; }

        public long SubActivityID { get; set; }

        public long BudgetHeadID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string Division { get; set; }
        public long SubBudgetHeadID { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal ExpenditureTilldate { get; set; }

        public string ExpenditureMonths { get; set; }

        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }
        public bool IsActive { get; set; }
        public string ISCircleDivision { get; set; }

        public string SanctuaryCode { get; set; }
        public string WorkProgressDetails { get; set; }

        public string SiteName { get; set; }
        public long BudgetAllocation_CircleID { get; set; }
        public string SiteNameExpenditure { get; set; }
        public string Remarks { get; set; }

        public string IsCoreOrBuffer { get; set; }


        //////For Mobile App (Budget Development) 28-11-2018

        public string Mobile_Web { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    ////For Mobile App (Budget Developemnt) 28-11-2018

    public class tbl_BudgetExpenditure_Image
    {
        public long ID { get; set; }
        public long BudgetAllocation_CircleID { get; set; }
        public string Image_Path { get; set; }

        public string Expenditure_Month { get; set; }
        public long tbl_BudgetExpenditure_ID { get; set; }

    }

}