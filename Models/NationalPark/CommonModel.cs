using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class CommonModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}