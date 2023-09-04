using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using FMDSS.Models;

namespace FMDSS.Entity
{
    [Serializable]
    public class tbl_FRA_ClaimRequestDocument:BaseModelSerializable
    {
        [Key]
        public long ClaimRequestDocumentID { get; set; }
        public Nullable<long> WorkFlowDetailsID { get; set; }
        public string DocumentName { get; set; } 
        public int DocumentTypeID { get; set; }
        public string DocumentPath { get; set; }
        [NotMapped]
        public String DocumentTypeName { get; set; }
        public Nullable<bool> IsESign { get; set; }
        public Nullable<bool> ActiveStatus { get; set; } 
        [NotMapped]
        public string TempID { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
        [NotMapped]
        public int DocumentLevel { get; set; } 
    }
}
