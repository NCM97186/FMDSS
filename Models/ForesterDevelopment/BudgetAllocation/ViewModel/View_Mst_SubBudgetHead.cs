using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_Mst_SubBudgetHead:BaseModelSerializable
    {
        public long ID { get; set; }


        public string BudgetHead { get; set; }

        [Required(ErrorMessage = "Select Budget Head")]
        public long BudgetHeadID { get; set; }

        [Required(ErrorMessage = "Enter SubBudget Head")]
        //[RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string SubBudgetHead { get; set; }

        public DateTime EnterOn { get; set; }

        public long EnterBy { get; set; }

        public string Descriptions { get; set; }
    }
}