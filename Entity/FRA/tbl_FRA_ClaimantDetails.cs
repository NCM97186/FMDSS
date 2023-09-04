using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class tbl_FRA_ClaimantDetails
    {
        [Key]
        public long ClaimantDetailsID { get; set; } 
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public string BhamashahID { get; set; }
        public string ClaimantName { get; set; }
        public string FatherName { get; set; }
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        [ForeignKey("ClaimRequestDetailsID")]
        public virtual tbl_FRA_ClaimRequestDetails tbl_FRA_ClaimRequestDetails { get; set; }
    }
}