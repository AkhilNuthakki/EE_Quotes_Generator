using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEQuoteGenerator.Models
{
    [Table("Quote_Cost_Summary_Details")]
    public class QuoteCostSummary
    {
        [Key, ForeignKey("QuoteId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuoteId { get; set; }

        [Required]
        public int CapitalOutlay { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double EST20YearProfit { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double AssumedAnnualElectricityGenerated { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public double Yield { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public double IRR { get; set; }

        [Required]
        public int CO2SavingsYear20 { get; set; }

        [Required]
        public int CO2SavingsPerYear { get; set; }

        [Required]
        public int PayBackYears { get; set; }

        [Required]
        public bool IsFinanced { get; set; }

        public virtual Quote quote { get; set; }

    }
}
