using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRA.FRAViewModel
{
    public class RegistrationVM
    {
        public long SNo { get; set; }
        public long RegistrationID { get; set; }
        public string SSOID { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public string DistrictName { get; set; }
        public string Tehsil { get; set; }
        public string Block { get; set; }
        public string GramPanchayat { get; set; }
        public bool ActiveStatus { get; set; }
    }

    public class UserRegistration
    {
        public long RegistrationID { get; set; }
        [Required]
        [System.Web.Mvc.Remote("ValidateSSOID", "ClaimRequest", ErrorMessage = "Please enter valid SSOID.")]
        public string SSOID { get; set; }
        [Required]
        [Display(Name = "District")]
        public string DistrictID { get; set; }
        [Display(Name = "Tehsil")]
        public string TehsilID { get; set; }
        public string BlockID { get; set; }
        public string GPID { get; set; }
        public string VillageCode { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public bool ActiveStatus { get; set; }
    }

    public class UserAdditionalInfo
    {
        public string SSOID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Bhamashah_Id { get; set; }
        public string Aadhar_ID { get; set; }
        public string Postal_Address1 { get; set; }
        public string Postal_Code1 { get; set; }
        public string DOB { get; set; }
    }
}