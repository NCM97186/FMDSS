using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_FDM_SchemeForWidelife
    {
        [Key]
        public int ID { get; set; }

        public string Scheme_Name { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int IsActive { get; set; }

        public long CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }

    }

}