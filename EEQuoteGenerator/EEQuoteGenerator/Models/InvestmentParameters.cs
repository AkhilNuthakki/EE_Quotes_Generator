using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EEQuoteGenerator.Models
{
    [Table("Quote_Investment_Parameters")]
    public class InvestmentParameters
    {
        [Key, ForeignKey("QuoteId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuoteId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Zone { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "Panel Make")]
        public string PanelMake { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "Roof Mount Type")]
        public string RoofMountType { get; set; }

        [Required]
        [Display(Name = "Panel Watts")]
        public int PanelWatts { get; set; }

        [Required]
        [Display(Name = "No of Panels")]
        public int NoOfPanels { get; set; }

        [Required]
        public int Orientation { get; set; }

        [Required]
        public int Inclination { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "System Size")]
        public double SystemSize { get; set; }

        [Required]
        [Display(Name = "System Cost")]
        public int SystemCost { get; set; }

        [Required]
        public int AnnualGeneration { get; set; }

        [Required]
        [Display(Name = "Shade Factor")]
        public int ShadeFactor { get; set; }

        [Required]
        [Display(Name = "Percentage of Electricity Exported")]
        public int PercetageElectricityExported { get; set; }

        [Required]
        [Display(Name = "Assumed Export")]
        public int AssumedExport { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,4)")]
        [Display(Name = "Export Tariff")]
        public double ExportTariff { get; set; }

        [Required]
        [Display(Name = "Assumed Occupancy Type")]
        [Column(TypeName = "nvarchar(30)")]
        public string AssumedOccupancyType { get; set; }

        [Required]
        [Display(Name = "Assumed Electric Consumpt.")]
        public int AssumedAnnualElectricityConsumption { get; set; }

        [Required]
        [Display(Name = "Expected Solar PV Consumpt.")]
        public int ExpectedSolarPVConsumption { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,4)")]
        [Display(Name = "Current Electricity Cost")]
        public double CurrentElectricityCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,4)")]
        [Display(Name = "Green Levy Taxes Cost")]
        public double GreenLevyTaxesCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,4)")]
        [Display(Name = "Total Electricity Cost")]
        public double TotalCurrentElectricityCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Retail Price Inflation (RPI)")]
        public double RPI { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Annual Fuel Price Inflation")]
        public double AnnualFuelPriceInflation { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Borrowing Cost")]
        public double BorrowingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Percentage Financed")]
        public double PercentageFinanced { get; set; }

        [Required]
        [Display(Name = "Loan Period (Years)")]
        public int LoanPeriodYears { get; set; }

        [Required]
        [Display(Name = "Annual Maintenance Package")]
        public int AnnualMaintenancPackage { get; set; }

        [Required]
        [Display(Name = "Annual Insurance")]
        public int AnnualInsurance { get; set; }

        [Required]
        [Display(Name = "Annual Monitoring Only Package")]
        public int AnnualMonitoringOnlyPackage { get; set; }

        public virtual Quote quote { get; set; }


        public static List<SelectListItem> GetRoofMountList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
             {
                 new SelectListItem{ Value="In-Roof",Text="In-Roof"},
                 new SelectListItem{ Value="On-Roof",Text="On-Roof"},
             };

            return list;
        }


        public static List<SelectListItem> GetOccupancyTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
             {
                 new SelectListItem{ Value="Home all day",Text="Home all day"},
                 new SelectListItem{ Value="Home half day",Text="Home half day"},
                 new SelectListItem{ Value="Out all day",Text="Out all day"}
             };

            return list;
        }
    }
}
