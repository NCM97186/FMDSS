using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class ShiftModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Shift Name"), MaxLength(50)]
        public string Name { get; set; }        
        public string Duration { get; set; }
        public bool IsActive { get; set; }        
    }
}