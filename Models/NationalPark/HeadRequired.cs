using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.NationalPark
{
    public class HeadRequired
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Head Name"), MaxLength(50)]
        public string Name { get; set; } 

        public bool IsRequired { get; set; }   
    }
}
