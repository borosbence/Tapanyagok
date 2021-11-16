using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Tapanyagok.API.Models
{
    [Table("tapanyag")]
    public partial class Tapanyag
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string nev { get; set; }
        public decimal energia { get; set; }
        public decimal feherje { get; set; }
        public decimal zsir { get; set; }
        public decimal szenhidrat { get; set; }
    }
}
