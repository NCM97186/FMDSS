using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class NP_HeadMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmitraHeadCode { get; set; }
        public string EmitraKioskHeadCode { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }
    }
}