using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Encroachment.ViewModel
{
    public class EncroachAppearanceView
    {
        public string EN_Code { get; set; }

        [Required(ErrorMessage = "Select decision")]
        public string Decision_Taken { get; set; }

        [Required(ErrorMessage = "Select date")]        
        public Nullable<DateTime> Decision_Date { get; set; }

        [Required(ErrorMessage = "Enter description")]
        public string Decision_Description { get; set; }

        public string Next_Date { get; set; }

        public string Next_Decision_Place { get; set; }

        public HttpPostedFileBase AcfDecisionFiles { get; set; }
        public byte[] Acf_Decision_Upload { get; set; }

        public string Acf_Decision_UploadFileName { get; set; }
        public long Entered_By { get; set; }
    }
}