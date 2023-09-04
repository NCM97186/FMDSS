using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_Villages
    {
        [Key]
        public int ROWID { get; set; }

        public string STATE_CODE { get; set; }

        public string DIV_CODE { get; set; }

        public string DIST_CODE { get; set; }

        public string BLK_CODE { get; set; }

        public string GP_CODE { get; set; }

        public string GP_STATUS { get; set; }

        public string VILL_CODE { get; set; }

        public string VILL_NAME { get; set; }

        public string VILL_HNAME { get; set; }

        public float AREA_SQKM { get; set; }

        public string RANGE_CODE { get; set; }

        public bool IsActive { get; set; }

    }
}