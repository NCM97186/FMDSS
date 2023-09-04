using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_FDM_Activity
    {
        [Key]
        public long ID { get; set; }

        public string Activity_Name { get; set; }

        public string Activity_Desc { get; set; }

        public string Sub_Activity_Unit { get; set; }

        public string Activity_Type { get; set; }

        public string IsSubActvity { get; set; }

        public string Activity_RefNo { get; set; }

        public int Activity_Year { get; set; }

        public decimal Activity_TotalCost { get; set; }

        public string BSR_Type { get; set; }

        public decimal Activity_BSR_Material_Cost { get; set; }

        public decimal Activity_BSR_Labour_Cost { get; set; }

        public decimal Activity_BSR_Per_Unit { get; set; }

        public DateTime Activity_StartDate { get; set; }

        public DateTime Activity_EndDate { get; set; }

        public string Activity_DocumentPath { get; set; }

        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public string RepoDocID { get; set; }

        public bool DocStatus { get; set; }

        public string FileName { get; set; }

    }
}