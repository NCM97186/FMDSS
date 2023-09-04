using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_FRA_KhasraDetails
    {
        public tbl_FRA_KhasraDetails()
        {
            ActiveStatus = true;
        }
        [Key]
        public long KhasraDetailsID { get; set; }
        public long SurveyDetailsID { get; set; }
        public string CompartmentNumber { get; set; }
        public string KhasraNumber { get; set; }
        public string TotalAreaAgainstKhasra { get; set; }
        public string TotalAreaAgainstOccupiedForestLand { get; set; }
        public string TotalAreaApprovedAgainstOccupiedForestLand { get; set; }
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; }
        public string SpecialRemarks { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
    }
}
