using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEQuoteGenerator.Models
{
    [Table("Customer_Details")]
    public class Customer
    {
        [Key, ForeignKey("QuoteId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuoteId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Email ")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Postcode")]
        public string PostCode { get; set; }

        public virtual Quote quote { get; set; }
    }
}
