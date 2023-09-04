using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class Tbl_Encroacher_Details
    {
        [Key]
        public long ID { get; set; }               
        public string EN_Code { get; set; }     
        public string Encroacher_Name { get; set; }
        public string Encroacher_FatherName { get; set; }
        public string Encroacher_Address { get; set; }

        public string AadharOrBhamasha { get; set; }
        public string Encroacher_Aadhar { get; set; }
        public string Encroacher_Bhamashah { get; set; }


      //  public DateTime DOE { get; set; }

    }

   
}