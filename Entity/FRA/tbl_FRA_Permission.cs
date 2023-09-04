using System;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class tbl_FRA_Permission
    {
        [Key]
        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
