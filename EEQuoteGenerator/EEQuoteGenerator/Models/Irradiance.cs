using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EEQuoteGenerator.Models
{
    [Table("Irradiance_Datasets")]
    public class Irradiance
    {
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Zone { get; set; }

        [Required]
        public int Inclination { get; set; }

        [Required]
        public int Orientation { get; set; }

        [Required]
        public int AnnualGenValue { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string ZoneName { get; set; }
    }
}
