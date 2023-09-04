using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_Division
    {

        public long DivisionID { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
