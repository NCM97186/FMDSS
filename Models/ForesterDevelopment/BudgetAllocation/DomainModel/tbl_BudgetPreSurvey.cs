using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_BudgetPreSurvey
    {
        [Key]
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }
        public long SubBudgetHeadID { get; set; }
        public int FY_ID { get; set; }
        public decimal TotalAmount { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DivisionCode { get; set; }
        public long SchemeID { get; set; }
        public long ActivityID { get; set; }
        public long SubActivityID { get; set; }
        public string  CreatedOn { get; set; }
        public long CreatedBy { get; set; }

        public string UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public int isActive { get; set; }
        public string ISCircleDivision { get; set; }

        public string SiteName { get; set; }

        public string IsRecurring { get; set; }

        public string RangeCode { get; set; }

        public string Unit { get; set; }
        public decimal NumberPerUnit { get; set; }
        public decimal RatePerUnit { get; set; }

        public string UploadDocuments { get; set; }
    }


    public class tbl_BudgetPreSurveyFilesUpload
    {
        [Key]
        public int ID { get; set; }
        public long BudgetPreSurveyId { get; set; }
        public string FilesName { get; set; }
        public int Status { get; set; }
        public string CreatedDate { get; set; }
        public long Createdby { get; set; }
    }

}