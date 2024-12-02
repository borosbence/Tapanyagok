using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tapanyagok.Server.Models
{
    [Table("tapanyagok")]
    public partial class Tapanyag
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int id { get; set; }
        [StringLength(50)]
        public string nev { get; set; } = null!;
        [Precision(10, 1)]
        public decimal energia { get; set; }
        [Precision(10, 1)]
        public decimal feherje { get; set; }
        [Precision(10, 1)]
        public decimal zsir { get; set; }
        [Precision(10, 1)]
        public decimal szenhidrat { get; set; }
    }
}
