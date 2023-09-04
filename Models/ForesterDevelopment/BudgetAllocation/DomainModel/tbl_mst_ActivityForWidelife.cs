using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_ActivityForWidelife
    {
        [Key]
        public int ID { get; set; }      
        public string Activity_Name { get; set; }

        public string ActivityDescription { get; set; }

        public string ActivityGroup { get; set; }

        public string ActivityCategory { get; set; }

        public string ReferenceNo { get; set; }

        public string ReferenceDoc { get; set; }

        public int IsActive { get; set; }

        public long CreatedBy { get; set; }

        public string CreatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public string UpdatedOn { get; set; }


    }
}