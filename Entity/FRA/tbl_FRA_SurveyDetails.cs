using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity
{
    public class tbl_FRA_SurveyDetails : CommonEntity
    {
        public tbl_FRA_SurveyDetails()
        {
            this.KhasraDetailsList = new List<tbl_FRA_KhasraDetails>();
        }
        [Key]
        public long SurveyDetailsID { get; set; }
        public Nullable<long> ClaimRequestDetailsID { get; set; }
        public Nullable<long> WorkFlowDetailsID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string North { get; set; }
        public string South { get; set; }
        public string West { get; set; }
        public string East { get; set; }
        public string GISID { get; set; }
        [NotMapped]
        public int ReportType { get; set; }
        public string ActivityData { get; set; }
        public virtual List<tbl_FRA_KhasraDetails> KhasraDetailsList { get; set; }
    }
}
