using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEQuoteGenerator.Models
{
    [Table("Component_Details")]
    public class Components
    {

        [Key, ForeignKey("QuoteId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuoteId { get; set; }


        public int Inverter { get; set; }

        [Display(Name = "Roof Mounting System")]
        public bool RoofMountingSystem { get; set; }

        [Display(Name = "Optimizers")]
        public bool Optimizers { get; set; }

        [Display(Name = "Lifting Equipemnt")]
        public bool LiftingEquipment { get; set; }

        [Display(Name = "Waste Removal")]
        public bool WasteRemoval { get; set; }

        [Display(Name = "DC & AC Insolators")]
        public bool DcAcIsolators { get; set; }

        [Display(Name = "DC & AC Wiring")]
        public bool DcAcWiring { get; set; }

        [Display(Name = "Dist. Board Connection")]
        public bool DistributionBoardConnection { get; set; }

        [Display(Name = "Eletrical Generation Meter")]
        public bool ElectricalGenerationMeter { get; set; }

        [Display(Name = "Batteries")]
        public bool Batteries { get; set; }

        [Display(Name = "EV Chargers")]
        public bool EvChargers { get; set; }

        [Display(Name = "Immersion Controller")]
        public bool ImmersionController { get; set; }

        [Display(Name = "Pegion Proofing")]
        public bool PegionProofing { get; set; }

        [Display(Name = "Installation Warranties")]
        public bool SystemInstallationWarranties { get; set; }

        [Display(Name = "MCS Registration")]
        public bool MCSRegistration { get; set; }

        [Display(Name = "Structural Report")]
        public bool StructuralReport { get; set; }

        public virtual Quote quote { get; set; }
    }
}
