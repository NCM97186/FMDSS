using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class  WaterHoleVendor
    {
         
        public string RegNumber { get; set; }
        public string NameofVendor { get; set; }
        public Int64 WaterHoleVendorDetailsID { get; set; }
        public string District { get; set; }
        public string PinCode { get; set; }
        public string RepresentativeName { get; set; }
        public string MobileNumber { get; set; }
        public string VendorSSOId { get; set; }
        public string PurposeforRegistration { get; set; }
        public string Circle_Code { get; set; }
        public string Division_Code { get; set; }
        public string Range_Code { get; set; }
        public string Village_Code { get; set; }
        public string WaterSource_Code { get; set; }
        public string WaterHoles_Code { get; set; }
        public string InsertedBy { get; set; }
        public bool isActive { get; set; }
        public string UsedFor { get; set; }


    }
}