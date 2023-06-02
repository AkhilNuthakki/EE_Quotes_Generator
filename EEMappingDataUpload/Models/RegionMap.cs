using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EEMappingDataUpload.Models
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
