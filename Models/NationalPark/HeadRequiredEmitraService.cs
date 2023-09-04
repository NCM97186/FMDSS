using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class HeadRequiredEmitraService
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Head Name"), MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Head Emitra Online Booking Code"), MaxLength(10)]
        public string EmitraHeadCode { get; set; }

        [Required(ErrorMessage = "Enter Head Emitra Kisok Booking Code"), MaxLength(10)]
        public string EmitraKioskHeadCode { get; set; }

        public bool IsActive { get; set; }
    }
}