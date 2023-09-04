using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_Forest_WildLifeCircles
    {
           [Key]
            public int ROWID { get; set; }

            public string REG_CODE { get; set; }

            public string CIRCLE_CODE { get; set; }

            public string CIRCLE_NAME { get; set; }

            public string CIRCLE_HNAME { get; set; }

            public float AREA_SQKM { get; set; }

            public bool ISWILDLIFECIRCLE { get; set; }

            public int isBOTH { get; set; }

    }
}