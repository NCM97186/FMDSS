using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class CommonEntity
    {
        public CommonEntity()
        {
            ActiveStatus = true;
        }
        public Nullable<long> AddedBy { get; set; } 
        public Nullable<DateTime> AddedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<DateTime> DeletedOn { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        [NotMapped]
        public String EnteredOn { get; set; }
    }

    public class ResponseMsg
    {
        public bool IsError { get; set; }
        public string ReturnMsg { get; set; }
        public string ReturnIDs { get; set; }
    }
}