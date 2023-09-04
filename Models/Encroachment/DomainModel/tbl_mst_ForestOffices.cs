using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_mst_ForestOffices
    {
       
        public int ROWID { get; set; }
        public string DIV_CODE { get; set; }

        [Key]
        public string Office_ID { get; set; }
        public string OfficeName { get; set; }
    }
}