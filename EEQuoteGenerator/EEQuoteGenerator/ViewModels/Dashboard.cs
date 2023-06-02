using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EEQuoteGenerator.ViewModels
{
    public class Dashboard
    {
        [Display(Name = "Total Quotes")]
        public int TotalQuotesCount { get; set; }

        [Display(Name = "Total Users")]
        public int TotalUsersCount { get; set; }

        [Display(Name = "Your Quotes")]
        public int UserQuotesCount { get; set; }

        public Dictionary<string,int> QuotesCountByUser { get; set; }

        public Dictionary<string, int> QuotesCountByRegion { get; set; }
    }
}
