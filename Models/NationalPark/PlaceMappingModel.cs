using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.NationalPark
{
    public class PlaceMappingModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Select Place")]
        public long PlaceId { get; set; }
        public Nullable<long> ZoneId { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public bool IsActive { get; set; }
        
        public string PlaceName { get; set; }
        public string ZoneName { get; set; }
        public string ShiftName { get; set; }
        public string VehicleName { get; set; }        
    }
}