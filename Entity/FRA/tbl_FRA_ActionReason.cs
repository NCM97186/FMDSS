using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMDSS.Entity
{
    public class tbl_FRA_ActionReason
    {
        [Key]
        public int ActionReasonID { get; set; }
        public string ModuleName { get; set; }
        public Nullable<int> ActionID { get; set; }
        public string ActionReason { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
