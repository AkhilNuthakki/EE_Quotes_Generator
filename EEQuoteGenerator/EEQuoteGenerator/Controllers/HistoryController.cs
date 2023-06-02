using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EEQuoteGenerator.Models;
using EEQuoteGenerator.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Web;
using EEQuoteGenerator.Filters;

namespace EEQuoteGenerator.Controllers
{
    public class HistoryController : Controller
    {

        private readonly EEDbContext _context;

        public HistoryController(EEDbContext context)
        {
            _context = context;
        }

        [SessionExpireFilter]
        public async Task<IActionResult> Index(string QuoteReference, string CustomerEmail, string CustomerPostcode, int pageNumber)
        {
            var quotes = from q in _context.Quotes select q;
            if (!string.IsNullOrEmpty(QuoteReference) || !string.IsNullOrEmpty(CustomerEmail) || !string.IsNullOrEmpty(CustomerPostcode))
            {
                if(!string.IsNullOrEmpty(QuoteReference))
                {
                    QuoteReference = QuoteReference.Replace("/", "_");
                }
                ViewData["QuoteReference"] = QuoteReference;
                ViewData["CustomerEmail"] = CustomerEmail;
                ViewData["CustomerPostcode"] = CustomerPostcode;
                quotes = _context.Quotes.Where(quote => quote.QuoteReference.Contains(QuoteReference) || quote.Customer.EmailId.Contains(CustomerEmail) || quote.Customer.PostCode.Contains(CustomerPostcode));
            }

            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 10;
            return View(await PaginatedList<Quote>.CreateAsync(quotes.Include(quote => quote.Customer).OrderByDescending(quote => quote.QuoteGeneratedDate).AsNoTracking(), pageNumber, pageSize));
        }

        [SessionExpireFilter]
        public async Task<IActionResult> Dashboard()
        {
            string UserId = HttpContext.Session.GetString("UserId");
            Dashboard db = new Dashboard();
            try
            {
                db.TotalQuotesCount = _context.Quotes.Count();
                db.TotalUsersCount = _context.Users.Count();
                db.UserQuotesCount = _context.Quotes.Where(Q => Q.QuotedGeneratedBy == UserId).Count();

                var QuotesCountByUser = _context.Quotes.AsEnumerable().GroupBy(Q => Q.QuotedGeneratedBy, (user, quotes) => new { Key = user, Count = quotes.Count() }).OrderByDescending(y => y.Count);

                Dictionary<string, int> QuotesCountByUserMap = new Dictionary<string, int>();
                foreach (var UserGroup in QuotesCountByUser)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserGroup.Key);
                    string UserName = user.FirstName + " " + user.LastName;
                    QuotesCountByUserMap.Add(UserName, UserGroup.Count);
                }

                db.QuotesCountByUser = QuotesCountByUserMap;


                var QuotesCountByRegion = _context.QuoteInvestmentParameters.AsEnumerable().GroupBy(Q => Q.Zone, (zone, quotes) => new { Key = zone, Count = quotes.Count() }).OrderByDescending(y => y.Count);

                Dictionary<string, int> QuotesCountByRegionMap = new Dictionary<string, int>();
                foreach (var ZoneGroup in QuotesCountByRegion)
                {
                    var ZoneDetails = await _context.IrradianceDatasets.FirstOrDefaultAsync(I => I.Zone == ZoneGroup.Key);
                    string ZoneName = ZoneDetails.ZoneName;
                    QuotesCountByRegionMap.Add(ZoneName, ZoneGroup.Count);
                }

                db.QuotesCountByRegion = QuotesCountByRegionMap;
            }
            catch
            {

            }
            return View(db);
        }

    }
}
