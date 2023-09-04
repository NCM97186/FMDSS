using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_Mst_BudgetHead:BaseModelSerializable
    {
        public long ID { get; set; }

        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Budget Head should be numbers only.")]
        [Required(ErrorMessage = "Enter Budget Head")]
        public string BudgetHead { get; set; }

        public DateTime EnterOn { get; set; }

        public long EnterBy { get; set; }

       [Required(ErrorMessage = "Select Type of Head.")]
        public string TypeOfHead { get; set; }

        public Nullable<bool> HaveSubBudgetHead { get; set; }

    }
}