using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_AD_District_Maping
    {
        [Key]
        public long ID { get; set; }
        public string DIV_CODE { get; set; }
        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public  string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string RequestedID { get; set; }

        public string Area { get; set; }
        public string GPSLat { get; set; }

        public string GPSLong { get; set; }

        public string GISID { get; set; }
        public string GISOrignalFilePath { get; set; }
        public string GISFilePath { get; set; }
        public string Forest_DivCode { get; set; }
        public string AreaShapeinHactare { get; set; }

    }
}