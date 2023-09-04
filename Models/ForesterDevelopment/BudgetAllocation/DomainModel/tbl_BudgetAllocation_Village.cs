using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_BudgetAllocation_Village
    {
        [Key]
        public long ID { get; set; }

        public long BudgetHeadID { get; set; }

        public long SubBudgetHeadID { get; set; }

        public int FY_ID { get; set; }

        public long AllocatedAmount { get; set; }

        public string RANGE_CODE { get; set; }

        public string VILL_CODE { get; set; }

        //public long Activity_ID { get; set; }

       // public long SubActivity { get; set; }

        public long SchemeID { get; set; }

        public long ActivityID { get; set; }

        public long SubActivityID { get; set; }
      
        public long EnteredBy { get; set; }

    }
}