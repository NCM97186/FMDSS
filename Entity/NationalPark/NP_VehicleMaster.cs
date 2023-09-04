using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class NP_VehicleMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte GovtSpecifiedOrPrivate { get; set; }
        public int SeatAlloted { get; set; }
        public byte FeesApplicable { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }
    }
}