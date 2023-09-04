using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class NP_HeadRequiredForEmitraService
    {
        [Key]
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int HeadId { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }
    }
}