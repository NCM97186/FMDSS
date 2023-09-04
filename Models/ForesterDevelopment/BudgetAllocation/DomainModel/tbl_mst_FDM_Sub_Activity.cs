using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_FDM_Sub_Activity
    {
          [Key]
        public long ID { get; set; }

        public string Sub_Activity_Name { get; set; }

        public string Sub_Activity_Unit { get; set; }

        public decimal Sub_Activity_RatePerUnit { get; set; }

        public decimal Sub_Activity_TotalCost { get; set; }

        public string Sub_Activity_BSRType { get; set; }

        public decimal Sub_Activity_BSR_Material_Cost { get; set; }

        public decimal Sub_Activity_BSR_Labour_Cost { get; set; }

        public string Sub_Activity_RefNo { get; set; }

        public string Sub_Activity_DocumentPath { get; set; }

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