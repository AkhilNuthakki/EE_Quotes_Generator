using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EEQuoteGenerator.Models
{
    [Table("Postcode_Region_Mapping")]
    public class RegionMap
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar(30)")]
        public string Postcode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Region { get; set; }
    }
}
