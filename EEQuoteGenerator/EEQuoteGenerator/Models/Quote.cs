using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EEQuoteGenerator.Models
{
    [Table("Quote_Main_Details")]
    public class Quote
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int QuoteId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string QuoteReference { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Master Project Type")]
        public string MasterProjectType { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Master MPAN")]
        public string MasterMPAN { get; set; }

        [Required]
        public DateTime QuoteGeneratedDate { get; set; }

        [Required]
        [Column(TypeName ="nvarchar(15)")]
        public string QuotedGeneratedBy { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual InvestmentParameters InvestmentParam { get; set; }

        public virtual QuoteCostSummary CostSummary { get; set; }

        public virtual List<CostProjection> CostProjections { get; set; }

        public virtual Components Components { get; set; }


        public static List<SelectListItem> GetProjectTypeList()
        {
            List<SelectListItem> Rolelist = new List<SelectListItem>()
             {
                 new SelectListItem{ Value="S",Text="S"},
                 new SelectListItem{ Value="B",Text="B"},
                 new SelectListItem{ Value="EV",Text="EV"}
             };

            return Rolelist;
        }
    }
}
