using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string SpcialDetails { get; set; }

        [ForeignKey(nameof(Villa))]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
