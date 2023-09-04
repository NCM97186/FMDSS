using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class Tbl_Encroach_Appearance
    {
        [Key]
        public long Id { get; set; }
        public string EN_Code { get; set; }      
        public string Decision_Taken { get; set; }    
        public Nullable<DateTime> Decision_Date { get; set; }     
        public string Decision_Description { get; set; }
        public Nullable<DateTime> Next_Date { get; set; }
        public string Next_Decision_Place { get; set; }

        public byte[] Acf_Decision_Upload { get; set; }
        public string Acf_Decision_UploadFileName { get; set; }

        public long Entered_By { get; set; }
    }
}