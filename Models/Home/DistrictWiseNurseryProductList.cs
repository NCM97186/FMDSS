using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Home
{
    public class ProductListVM
    {
       
        public string DIST_CODE { get; set; }
        public string NURSERY_CODE { get; set; }
        public int Id { get; set; }
        public bool ErrorMessageFlag { get; set; }
        public List<NurseryInformation> nurseryInformationList { get; set; }
        public List<DistrictWiseNurseryProductList> districtWiseNurseryProductList { get; set; }
    }
    public class NurseryInformation
    {
        public string NurseryName { get; set; }
        public string NURSERY_CODE { get; set; }
        public string NurseryAddress { get; set; }
        public string NurseryImageUrl { get; set; }
        public string NurseryIncharge { get; set; }
        public string Mobile { get; set; }
        public string NURSERY_LANDMARK { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }

    }
    public class ProductTypeInformation
    {
        public Int64 RowID { get; set; }
        public string ProductName { get; set; }
        public string ProductBaseTypeName { get; set; }
        public string Citizen_RemainingQTY { get; set; }
        public string Price { get; set; }
    }
    public class DistrictWiseNurseryProductList
    {
        public Int64 RowID { get; set; }
        public string DistCode { get; set; }
        public string DistName { get; set; }
        public string NurseryCode { get; set; }
        public string NurseryName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Citizen_RemainingQTY { get; set; }
        public int ProductNamesID { get; set; }
       

    }
}