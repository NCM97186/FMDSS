using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel
{
    public class tbl_mst_BudgetHead
    {
        [Key]
        public long ID { get; set; }

        public string BudgetHead { get; set; }

        //public DateTime EnterOn { get; set; }

        public Nullable<long> EnterBy { get; set; }

        public string TypeOfHead { get; set; }

        public Nullable<bool> HaveSubBudgetHead { get; set; }
    }
}