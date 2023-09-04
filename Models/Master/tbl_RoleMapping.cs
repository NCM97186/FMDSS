using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class tbl_RoleMapping
    {
        [Key]
        public int ID { get; set; }
        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public Int64 UserRoleIDs { get; set; }
        public bool IsActive { get; set; }
    }
}
