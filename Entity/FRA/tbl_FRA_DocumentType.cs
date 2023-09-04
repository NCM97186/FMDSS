using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_FRA_DocumentType
    {
        [Key]
        public int DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; } 
        public int DocumentLevel { get; set; }
        public bool ActiveStatus { get; set; }
    }
}