using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_mst_Forest_Divisions
    {
        [Key]
        public int ROWID { get; set; }
        public string REG_CODE { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DIV_CODE { get; set; }

        public string DIV_NAME { get; set; }

        public string DIV_HNAME { get; set; }

      //  public double AREA_SQKM { get; set; }

     //   public string ADMIN_DIST_MAPPED { get; set; }

        public bool IsActive { get; set; }

        public int ForBudgetModuleDist { get; set; }
    }
}