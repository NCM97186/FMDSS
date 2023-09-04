using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_mst_Forest_Ranges
    {
        [Key]
        public int ROWID { get; set; }

        public string REG_CODE { get; set; }

        public string CIRCLE_CODE { get; set; }

        public string DIV_CODE { get; set; }

        public string RANGE_CODE { get; set; }

        public string RANGE_NAME { get; set; }

        public string RANGE_HNAME { get; set; }

      //  public float AREA_SQKM { get; set; }
    }
}