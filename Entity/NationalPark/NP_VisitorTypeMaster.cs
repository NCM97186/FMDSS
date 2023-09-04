using System.ComponentModel.DataAnnotations;

namespace FMDSS.Entity
{
    public class NP_VisitorTypeMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime AddDate { get; set; }
        public long UserId { get; set; }
    }
}