using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EEQuoteGenerator.Models
{
    [Table("User_Details")]
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required (ErrorMessage ="User Email is required")]
        [Column(TypeName = "nvarchar(250)")]
        public string UserEmail { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Role is required")]
        public string UserRole { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string UserId { get; set; }

        
        public static List<SelectListItem> GetRolesList()
        {
            List<SelectListItem> Rolelist = new List<SelectListItem>()
             {
                 new SelectListItem{ Value="Project Assistant",Text="Project Assistant"},
                 new SelectListItem{ Value="Project Manager",Text="Project Manager"},
                 new SelectListItem{ Value="Commercial Director",Text="Commercial Director"}
             };

            return Rolelist;
        }


    }

}
