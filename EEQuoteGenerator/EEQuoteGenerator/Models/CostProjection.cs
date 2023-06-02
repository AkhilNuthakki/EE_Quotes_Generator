using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEQuoteGenerator.Models
{
    [Table("Cost_Projections")]
    public class CostProjection
    {
        [ForeignKey("QuoteId")]
        public int QuoteId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,3)")]
        public double CurrentCostPerkWp { get; set; }

        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public double OutputkWh { get; set; }

        [Required]
        public int CapitalOutlay { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double InsuranceCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double MaintenanceCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double MonitoringOnlyCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double TotalAnnualCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double RevenueFromSaving { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double RevenueFromExport { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double TotalAnnualRevenue { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double CashFlow { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double BorrowedAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double InterestAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double LoanRepaymentAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public double FinalBalance { get; set; }

        public virtual Quote Quote { get; set; }
    }
}
