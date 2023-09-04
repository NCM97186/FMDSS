using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class GISResponseVM
    { 
        public string shapeArea { get; set; }
        public string shapeLength { get; set; }
        public string villageData { get; set; }
        public string centroidData { get; set; }
        public string gisId { get; set; }
        public string activityData { get; set; }
        public string postbackData { get; set; }
    }

    public class GISVillageData
    {
        public string CENSUS_NM_2011 { get; set; }//Vill_Name
        public string CENSUS_CD_2011 { get; set; }//Vill_Code
        public string Direction { get; set; }
    } 
}