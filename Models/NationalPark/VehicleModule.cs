using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class VehicleModule
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Enter Vehicle Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Select Vehicle Type")]
        public VehicleType GovtSpecifiedOrPrivate { get; set; }

        [Required(ErrorMessage = "Enter number of seats")]
        [Range(1, 99, ErrorMessage = "Please enter numeric value")] 
        public int SeatAlloted { get; set; }

        [Required(ErrorMessage = "Select fees applicable")]
        public FeesApplicable FeesApplicable { get; set; }

        public bool IsActive { get; set; }
    }
}