using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.DTO
{
    public class VillaNumberDto
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpcialDetails { get; set; }
        public int VillaId { get; set; }
    }
}
