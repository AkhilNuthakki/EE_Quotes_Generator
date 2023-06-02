using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EEQuoteGenerator.Models;
using EEQuoteGenerator.Services;
using EEQuoteGenerator.Filters;
using System.Text.RegularExpressions;
using System.IO;

namespace EEQuoteGenerator.Controllers
{
    public class GenerateQuoteController : Controller
    {
        private readonly EEDbContext _context;

        public GenerateQuoteController(EEDbContext context)
        {
            _context = context;
        }

        //GET: GenerateQuote/QuoteInputs
        [SessionExpireFilter]
        public IActionResult QuoteInputs()
        {
            ViewData["RoofMountList"] = EEQuoteGenerator.Models.InvestmentParameters.GetRoofMountList();
            ViewData["OccupancyTypeList"] = EEQuoteGenerator.Models.InvestmentParameters.GetOccupancyTypeList();
            return View("QuoteInputs");
        }

        //GET: GenerateQuote/EditQuoteInputs/id
        [SessionExpireFilter]
        public async Task<IActionResult> EditQuoteInputs(int Id)
        {
            ViewData["RoofMountList"] = EEQuoteGenerator.Models.InvestmentParameters.GetRoofMountList();
            ViewData["OccupancyTypeList"] = EEQuoteGenerator.Models.InvestmentParameters.GetOccupancyTypeList();
            if (Id == 0)
            {
                return View("QuoteInputs");
            }
            var quote = await _context.Quotes
                .Include(quote => quote.Customer)
                .Include(quote => quote.InvestmentParam)
                .Include(quote => quote.CostSummary)
                .Include(quote => quote.CostProjections)
                .Include(quote => quote.Components)
                .Where(quote => quote.QuoteId == Id)
                .FirstOrDefaultAsync();
            return View("QuoteInputs", quote);
            
        }

        [SessionExpireFilter]
        public async Task<IActionResult> DownloadQuote(int Id){

            string UserId = HttpContext.Session.GetString("UserId");
            var User = await _context.Users.FirstOrDefaultAsync(user => user.UserId == UserId);
            if (Id == 0)
            {
                return NotFound();
            }
            var Quote = await _context.Quotes
                .Include(quote => quote.Customer)
                .Include(quote => quote.InvestmentParam)
                .Include(quote => quote.CostSummary)
                .Include(quote => quote.CostProjections)
                .Include(quote => quote.Components)
                .Where(quote => quote.QuoteId == Id)
                .FirstOrDefaultAsync();

            string DestinationFile = GenerateQuoteService.CreateQuoteDocument(Quote, User, ".docx");
            var memory = new MemoryStream();
            using (var stream = new FileStream(DestinationFile, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            GenerateQuoteService.DeleteQuote(DestinationFile);

            return File(memory, "application/vnd.ms-word", Path.GetFileName(DestinationFile));

            
        }

        [SessionExpireFilter]
        public async Task<IActionResult> EmailQuote(int Id)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            if (Id == 0)
            {
                return NotFound();
            }
            var Quote = await _context.Quotes
                .Include(quote => quote.Customer)
                .Include(quote => quote.InvestmentParam)
                .Include(quote => quote.CostSummary)
                .Include(quote => quote.CostProjections)
                .Include(quote => quote.Components)
                .Where(quote => quote.QuoteId == Id)
                .FirstOrDefaultAsync();

            var User = await _context.Users.FirstOrDefaultAsync(user => user.UserId == UserId);

            string DestinationFile = GenerateQuoteService.CreateQuoteDocument(Quote, User, "pdf");
            if (DestinationFile == null)
            {
                return Json(new { message = "FAILED" });
            }

            string TCs = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Consumer TCs.pdf";

            List<string> Attachments = new List<string>();
            Attachments.Add(DestinationFile);
            Attachments.Add(TCs);

            Boolean isEmailSent = await EmailSenderService.SendEmailAsync(Quote.Customer.EmailId, "Environmental Energy LTD - Quote Reference " + Quote.QuoteReference.Replace("_", "/"), Attachments, Quote, User);

            GenerateQuoteService.DeleteQuote(DestinationFile);

            if (isEmailSent)
            {
                return Json(new { message = "SUCCESS" });
            }
            else
            {
                return Json(new { message = "FAILED" });
            }

        }

        [SessionExpireFilter]
        public async Task<IActionResult> QuoteDetails(int Id)
        {
            if (Id == 0)
            {
                return NotFound();
            }
            var quote = await _context.Quotes
                .Include(quote => quote.Customer)
                .Include(quote => quote.InvestmentParam)
                .Include(quote => quote.CostSummary)
                .Include(quote => quote.CostProjections)
                .Include(quote => quote.Components)
                .Where(quote => quote.QuoteId == Id)
                .FirstOrDefaultAsync();

            return View(quote);
        }

        // POST: GenerateQuote/CalculateCostModel
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public async Task<IActionResult> CalculateCostModel(Quote Quote)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            ViewData["RoofMountList"] = EEQuoteGenerator.Models.InvestmentParameters.GetRoofMountList();
            ViewData["OccupancyTypeList"] = EEQuoteGenerator.Models.InvestmentParameters.GetOccupancyTypeList();

            string Zone = null;

            Quote.Customer.PostCode = Quote.Customer.PostCode.ToUpper();
            Zone = GetZonefromPostCode(Quote.Customer.PostCode);

            if(Zone == null)
            {
                ViewBag.ErrorMessage = "Couldn't able to find mapping Zone for the given postcode. Please contact System Administrator.";
                return View("QuoteInputs");
            }

            var IrradianceMap = await _context.IrradianceDatasets.FirstOrDefaultAsync(i => i.Zone == Zone && i.Inclination == Quote.InvestmentParam.Inclination && i.Orientation == Quote.InvestmentParam.Orientation);

            if (IrradianceMap == null)
            {
                ViewBag.ErrorMessage = "Couldn't able to find Irradinace Value for the given postcode, Orientation and Inclination. Please contact System Administrator.";
                return View("QuoteInputs");
            }

            Quote.InvestmentParam.AnnualGeneration = IrradianceMap.AnnualGenValue - (IrradianceMap.AnnualGenValue * Quote.InvestmentParam.ShadeFactor)/100;
            Quote.InvestmentParam.Zone = Zone;

            Quote.MasterProjectType = "S" + (Quote.Components.Batteries ? "-B" : "") + (Quote.Components.EvChargers ? "-EV" : "");
            Quote.QuoteGeneratedDate = DateTime.Now;
            Quote.QuotedGeneratedBy = UserId;

            Quote = CalculateCostModelService.CalculateandUpdateCostSummaryandProjections(Quote);
           
            try
            {
                if (Quote.QuoteId != 0)
                {
                    Quote = UpdateAllPrimaryKeysOfEntites(Quote);
                    _context.Quotes.Update(Quote);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var quote = _context.Quotes.Add(Quote);
                    await _context.SaveChangesAsync();
                    Quote.QuoteId = quote.Entity.QuoteId;
                }
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("QuoteInputs",Quote);
            }

            return View(Quote);
        }

        private Quote UpdateAllPrimaryKeysOfEntites(Quote Quote)
        {
            Quote.Customer.QuoteId = Quote.QuoteId;
            Quote.InvestmentParam.QuoteId = Quote.QuoteId;
            Quote.Components.QuoteId = Quote.QuoteId;
            foreach (CostProjection p in Quote.CostProjections)
            {
                p.QuoteId = Quote.QuoteId;
            }

            return Quote;
        }

        private bool InvestmentInputParametersExists(int id)
        {
            return _context.QuoteInvestmentParameters.Any(e => e.QuoteId == id);
        }

        private bool QuoteExists(int id)
        {
            return _context.Quotes.Any(e => e.QuoteId == id);
        }

        public string GetZonefromPostCode(string postcode)
        {
            String Zone = null;
            Regex CharCodeRGX = new Regex("[A-Z]{1,2}");
            Regex NumCodeRGX = new Regex("[0-9]{1,2}");
            string PCCharCode = "";
            int PCNumCode = 0;
            MatchCollection CharCodeMatches = CharCodeRGX.Matches(postcode);
            if (CharCodeMatches.Count > 0)
            {
                PCCharCode = CharCodeMatches.ElementAt(0).Value;
            }
            else
            {
                return null;
            }
            MatchCollection NumCodeMatches = NumCodeRGX.Matches(postcode);
            if (NumCodeMatches.Count > 0)
            {
                PCNumCode = Int32.Parse(NumCodeMatches.ElementAt(0).Value);
            }

            var RegionZoneMap = _context.RegionMappings.Where(r => r.Postcode.StartsWith(PCCharCode)).ToList();

            if (RegionZoneMap.Count == 0)
            {
                Zone = null;
            }
            else if (RegionZoneMap.Count == 1)
            {
                Zone = RegionZoneMap.ElementAt(0).Region;
            }
            else
            {
                string DefaultZone = null;
                foreach (RegionMap map in RegionZoneMap)
                {
                    Regex rgx = new Regex("[0-9]{1,2}");
                    MatchCollection bounds = rgx.Matches(map.Postcode);
                    if (bounds.Count > 0)
                    {
                        if (bounds.Count == 1)
                        {
                            int value = Int32.Parse(bounds.ElementAt(0).Value);
                            if (PCNumCode == value) { Zone = map.Region; }
                        }
                        else
                        {
                            int LowerBound = Int32.Parse(bounds.ElementAt(0).Value);
                            int UpperBound = Int32.Parse(bounds.ElementAt(1).Value);
                            if (PCNumCode >= LowerBound && PCNumCode <= UpperBound)
                            {
                                Zone = map.Region;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DefaultZone = map.Region;
                    }
                }
                if (Zone == null) { Zone = DefaultZone; }
            }
            return Zone;
        }
    }
}
