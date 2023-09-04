using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_BudgetAllocationPerposal
    {
        [Key]
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public int FY_ID { get; set; }
        public decimal AllocatedAmount { get; set; }
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

        public string RangeCode { get; set; }
        public long? NakaID { get; set; }
    }

    public class tbl_BudgetPerposalFilesUpload
    {
        [Key]
        public int ID { get; set; }
        public long BudgetPerposalId { get; set; }
        public string FilesName { get; set; }
        public int Status { get; set; }
        public string CreatedDate { get; set; }
        public long Createdby { get; set; }
    }
}