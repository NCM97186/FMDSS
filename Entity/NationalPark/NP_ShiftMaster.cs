using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class NP_ShiftMaster
    {
        [Key]
        public int Id { get; set; }         
        public string Name { get; set; }
        public string Duration { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }
    }
}