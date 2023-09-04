using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_State
    {
        [Key]
        public int StateID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
