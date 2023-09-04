using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_FDM_Scheme
    {
        [Key]
        public long ID { get; set; }

        public string Scheme_Name { get; set; }

        public decimal AreaofRolloutinSQKM { get; set; }

        public string RefNoRelatedDocument { get; set; }

        public long ProgramId { get; set; }

        public string Model_Code { get; set; }

        public string DIST_CODE { get; set; }

        public string RefDocument { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ImplementingAgency { get; set; }

        public string Budget_Head { get; set; }

        public string Administrative_Approval { get; set; }

        public DateTime Administrative_Approval_Date { get; set; }

        public string Administrative_Approval_Document { get; set; }

        public string Financial_Approval { get; set; }

        public DateTime Financial_Approval_Date { get; set; }

        public string Financial_Approval_Document { get; set; }

        public string Keybeneficial { get; set; }

        public string Keyactivity { get; set; }

        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }

        public decimal budget { get; set; }

        public string RefDocument_RepoDocID { get; set; }

        public bool RefDocument_DocStatus { get; set; }

        public string RefDocument_FileName { get; set; }

        public string AADDocument_RepoDocID { get; set; }

        public bool AADDocument_DocStatus { get; set; }

        public string AADDocument_FileName { get; set; }

        public string FADDocument_RepoDocID { get; set; }

        public bool FADDocument_DocStatus { get; set; }

        public string FADDocument_FileName { get; set; }

    }

}