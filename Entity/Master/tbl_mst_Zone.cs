using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_mst_Zone
    {
        [Key]
        public long ZoneID { get; set; }
        public Nullable<long> PlaceID { get; set; }
        public string ZoneName { get; set; }
        public Nullable<int> TicketAllocatedPerShift { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public string ShiftType { get; set; }
        public Nullable<bool> isMorning { get; set; }
        public Nullable<bool> isEvening { get; set; }
        public Nullable<bool> isFullDay { get; set; }
        public Nullable<bool> isDptKiosk { get; set; }
        public Nullable<bool> isCitizen { get; set; }
    }
}