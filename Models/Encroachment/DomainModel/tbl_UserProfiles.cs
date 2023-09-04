using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_UserProfiles
    {
        [Key]
        public long UserID { get; set; }

        public string Ssoid { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public string Mobile { get; set; }

        public string Designation { get; set; }

        public string DOB { get; set; }

        public string Gender { get; set; }

        public string Postal_Address1 { get; set; }

        public string Postal_Code1 { get; set; }

        public string District1 { get; set; }

        public string Postal_Address2 { get; set; }

        public string Postal_Code2 { get; set; }

        public string District2 { get; set; }

        public string City2 { get; set; }

        public string Bhamashah_Id { get; set; }

        public string Aadhar_ID { get; set; }

        public string RoleId { get; set; }

        public bool? IsKioskUser { get; set; }

        public bool? IsAgency { get; set; }

        public string AgencyName { get; set; }

        public string AgencyDistrict { get; set; }

        public string AgencyCity { get; set; }

        public string AgencyAddress { get; set; }

        public string AgencySPOC { get; set; }

        public string AgencyContact { get; set; }

        public int Isactive { get; set; }

        public DateTime? EnteredOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public bool? IsSSO { get; set; }

        public bool? IsBhamashah { get; set; }

        public string KioskId { get; set; }

        public bool? IsDepartmentalKioskUser { get; set; }
    }
}