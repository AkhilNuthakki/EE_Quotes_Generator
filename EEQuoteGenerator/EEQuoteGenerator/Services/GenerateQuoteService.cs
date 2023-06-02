using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using EEQuoteGenerator.Models;

namespace EEQuoteGenerator.Services
{
    public class GenerateQuoteService
    {
        public static string CreateQuoteDocument(Quote Quote, User User, string format)
        {
            //fetch the Quote template document from the resource folfer of the project folfer
            string SourceTemplate = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Quote_Template.docx";

            // Check if the Application Directory exists in the local machine, if not create the directory
            string DestinationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Quote Generation Tool\\Quotes";
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            //copy the template into the application directory with new name (reference number of the quote)
            string DestinationFile = DestinationDirectory + "\\Quote_" + Quote.QuoteReference + ".docx";
            DeleteQuote(DestinationFile);
            System.IO.File.Copy(SourceTemplate, DestinationFile);

            //create Word Application and open the copied quote template using the document of word object
            Word.Application OWord = new Word.Application();
            Word.Document ODocument = OWord.Documents.Open(DestinationFile);
            ODocument.Activate();
            OWord.Visible = false;

            //update header of the document
            UpdateHeader(OWord, Quote);
            //update dynamic fields page by page
            UpdateQuoteDocument_Page1(OWord, Quote, User);
            UpdateQuoteDocument_Page2(OWord, Quote);
            UpdateQuoteDocument_Page3(OWord, Quote);
            UpdateQuoteDocument_Page4_Page5(OWord, Quote);
            UpdateQuoteDocument_Page12(OWord, Quote);

            UpdateROITable_Page22(OWord, Quote);

            //save the document based on the format paramter
            if (format.Equals(".docx"))
            {
                ODocument.Save();
                ODocument.Close();
                OWord.Quit();
            }
            else
            {
                //Word.WdSaveFormat wdSaveFormatPDF = Word.WdSaveFormat.wdFormatPDF;
                //object wdSaveFormat = wdSaveFormatPDF;
                //object SaveAsNewFile = DestinationDirectory + "\\Quote_" + Quote.QuoteReference + ".pdf";

                // save as pdf format 
                string SaveAsNewFile = DestinationDirectory + "\\Quote_" + Quote.QuoteReference + ".pdf";
                DeleteQuote(SaveAsNewFile);
                ODocument.SaveAs2(SaveAsNewFile, Word.WdSaveFormat.wdFormatPDF);

                //close the document and quite the word application
                ODocument.Close();
                OWord.Quit();

                //delete the word document after saving it to pdf and return the new pdf file path
                DeleteQuote(DestinationFile);
                DestinationFile = SaveAsNewFile;

            }

            return DestinationFile;
        }

        public static void DeleteQuote(string FilePath)
        {
            try
            {
                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath);
                }
            }
            catch {}
        }

        private static bool UpdateHeader(Word.Application OWord, Quote Quote)
        {
            try
            {
                Word.Range HeaderRange = OWord.ActiveDocument.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                object toFindText = "X_QUOTE_REFERENCE_X";
                object replaceWithText = Quote.QuoteReference.Replace("_", "/");
                object matchCase = true;
                object matchwholeWord = true;
                object matchwildCards = false;
                object matchSoundLike = false;
                object nmatchAllforms = false;
                object forward = true;
                object format = false;
                object matchKashida = false;
                object matchDiactitics = false;
                object matchAlefHamza = false;
                object matchControl = false;
                object replace = 2;
                object wrap = 1;
                HeaderRange.Find.Execute(ref toFindText, ref matchCase, ref matchwholeWord, ref matchwildCards, ref matchSoundLike, ref nmatchAllforms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida, ref matchDiactitics, ref matchAlefHamza, ref matchControl);

                return true;
            }
            catch 
            {
                return false;
            }
        }

        private static bool UpdateQuoteDocument_Page1(Word.Application OWord, Quote Quote, User User)
        {
            try
            {
                FindAndReplace(OWord, "X_TODAY_DATE_X", DateTime.Today.ToShortDateString(), 1);
                string CustomerFullName = Quote.Customer.FirstName + " " + Quote.Customer.LastName;
                FindAndReplace(OWord, "X_CUSTOMER_NAME_X", CustomerFullName.ToUpper(), 1);
                String QuoteReference = Regex.Replace(Quote.QuoteReference, "_", "/");
                FindAndReplace(OWord, "X_QUOTE_REFERENCE_X", QuoteReference, 2);
                string SiteDetails = Quote.Customer.Address + " " + Quote.Customer.PostCode;
                FindAndReplace(OWord, "X_SITE_DETAILS_X", SiteDetails.ToUpper(), 2);
                FindAndReplace(OWord, "X_SYSTEM_SIZE_X", Quote.InvestmentParam.SystemSize + " kW", 2);
                FindAndReplace(OWord, "X_MPAN_X", Quote.MasterMPAN, 1);
                FindAndReplace(OWord, "X_CUSTOMER_FIRST_NAME_X", Quote.Customer.FirstName, 1);
                string UserFullName = User.FirstName + " " + User.LastName;
                FindAndReplace(OWord, "X_USER_NAME_X", UserFullName, 1);
                FindAndReplace(OWord, "X_USER_ROLE_X", User.UserRole, 1);
                return true;
            }
            catch
            {
                return true;
            }
        }

        private static bool UpdateQuoteDocument_Page2(Word.Application OWord, Quote Quote)
        {
            try
            {
                FindAndReplace(OWord, "X_NO_PANEL_X", Quote.InvestmentParam.NoOfPanels.ToString(), 1);
                FindAndReplace(OWord, "X_PANEL_WATTS_X", Quote.InvestmentParam.PanelWatts.ToString() + " W", 1);
                FindAndReplace(OWord, "X_ROOF_MOUNT_TYPE_X", Quote.InvestmentParam.RoofMountType.ToString(), 1);
                double result = Math.Round(((double)(Quote.InvestmentParam.ExpectedSolarPVConsumption / Quote.CostSummary.AssumedAnnualElectricityGenerated) * 100));
                FindAndReplace(OWord, "X_POWER_USED_X", result.ToString() + "%", 1);

                var CostReturnsMap = new Dictionary<string, string>();
                CostReturnsMap.Add("XC1X", "£" + Quote.InvestmentParam.SystemCost.ToString());
                CostReturnsMap.Add("XC2X", Quote.CostSummary.Yield.ToString() + "%");
                CostReturnsMap.Add("XC3X", "£" + Quote.CostSummary.EST20YearProfit.ToString());
                CostReturnsMap.Add("XC4X", Quote.InvestmentParam.SystemSize.ToString());
                CostReturnsMap.Add("XC5X", Quote.InvestmentParam.NoOfPanels.ToString());
                CostReturnsMap.Add("XC6X", Quote.CostSummary.AssumedAnnualElectricityGenerated.ToString() + " KWH");
                CostReturnsMap.Add("XC7X", Quote.CostSummary.CO2SavingsYear20.ToString());
                CostReturnsMap.Add("XC8X", Quote.CostSummary.CO2SavingsPerYear.ToString());
                CostReturnsMap.Add("XC9X", Quote.CostSummary.PayBackYears.ToString());

                //Update Cost and Returns Table
                UpdateTable(OWord, 1, CostReturnsMap);
                return true;
            }
            catch 
            {
                return false;
            } 
        }

        private static bool UpdateQuoteDocument_Page3(Word.Application OWord, Quote Quote)
        {
            try
            {
                var ComponentsMap = new Dictionary<string, string>();
                ComponentsMap.Add("XC1X", Quote.InvestmentParam.PanelWatts.ToString() + " W Solar PV");
                ComponentsMap.Add("XC2X", Quote.InvestmentParam.NoOfPanels.ToString());
                ComponentsMap.Add("XC3X", Quote.Components.Inverter.ToString());
                ComponentsMap.Add("XC4X", Quote.Components.RoofMountingSystem ? "Yes" : "No");
                ComponentsMap.Add("XC5X", Quote.Components.Optimizers ? "Yes" : "No");
                ComponentsMap.Add("XC6X", Quote.Components.LiftingEquipment ? "Yes" : "No");
                ComponentsMap.Add("XC7X", Quote.Components.WasteRemoval ? "Yes" : "No");
                ComponentsMap.Add("XC8X", Quote.Components.DcAcIsolators ? "Yes" : "No");
                ComponentsMap.Add("XC9X", Quote.Components.DcAcWiring ? "Yes" : "No");
                ComponentsMap.Add("XC10X", Quote.Components.DistributionBoardConnection ? "Yes" : "No");
                ComponentsMap.Add("XC11X", Quote.Components.ElectricalGenerationMeter ? "Yes" : "No");
                ComponentsMap.Add("XC12X", Quote.Components.Batteries ? "Yes" : "No");
                ComponentsMap.Add("XC13X", Quote.Components.EvChargers ? "Yes" : "No");
                ComponentsMap.Add("XC14X", Quote.Components.ImmersionController ? "Yes" : "No");
                ComponentsMap.Add("XC15X", Quote.Components.PegionProofing ? "Yes" : "No");
                ComponentsMap.Add("XC16X", Quote.Components.SystemInstallationWarranties ? "Yes" : "No");
                ComponentsMap.Add("XC17X", Quote.Components.MCSRegistration ? "Yes" : "No");
                ComponentsMap.Add("XC18X", Quote.Components.StructuralReport ? "Yes" : "No");

                //Update Cost and Returns Table
                UpdateTable(OWord, 2, ComponentsMap);

                return true;
            }
            catch
            {
                return false;
            } 
        }

        private static bool UpdateQuoteDocument_Page4_Page5(Word.Application OWord, Quote Quote)
        {
            try
            {
                var PerformanceMap = new Dictionary<string, string>();

                PerformanceMap.Add("XC1X", Quote.InvestmentParam.SystemSize.ToString());
                PerformanceMap.Add("XC2X", Quote.InvestmentParam.Orientation.ToString());
                PerformanceMap.Add("XC3X", Quote.InvestmentParam.Inclination.ToString());
                PerformanceMap.Add("XC4X", Quote.InvestmentParam.Zone.ToString());
                double result = Math.Round(Quote.CostSummary.AssumedAnnualElectricityGenerated / Quote.InvestmentParam.SystemSize, 2);
                PerformanceMap.Add("XC5X", result.ToString());
                PerformanceMap.Add("XC6X", Quote.InvestmentParam.ShadeFactor.ToString() + "%");
                result = Math.Round((Quote.CostSummary.AssumedAnnualElectricityGenerated * (100 - Quote.InvestmentParam.ShadeFactor)) / 100);
                PerformanceMap.Add("XC7X", result.ToString());
                PerformanceMap.Add("XC8X", Quote.InvestmentParam.AssumedOccupancyType.ToString());
                PerformanceMap.Add("XC9X", Quote.InvestmentParam.AssumedAnnualElectricityConsumption.ToString());
                PerformanceMap.Add("XC10X", Quote.CostSummary.AssumedAnnualElectricityGenerated.ToString());
                PerformanceMap.Add("XC11X", Quote.InvestmentParam.ExpectedSolarPVConsumption.ToString());
                result = ((double)(Quote.InvestmentParam.ExpectedSolarPVConsumption / Quote.InvestmentParam.AssumedAnnualElectricityConsumption) * 100);
                PerformanceMap.Add("XC12X", result.ToString() + "%");

                //Update Predicted Performance Calculations MCS Table
                UpdateTable(OWord, 3, PerformanceMap);

                var ReturnsMap = new Dictionary<string, string>();

                ReturnsMap.Add("XC1X", Quote.InvestmentParam.SystemSize.ToString());
                ReturnsMap.Add("XC2X", "£" + Quote.InvestmentParam.SystemCost.ToString());
                ReturnsMap.Add("XC3X", Quote.CostSummary.AssumedAnnualElectricityGenerated.ToString());
                ReturnsMap.Add("XC4X", "£" + Quote.InvestmentParam.CurrentElectricityCost.ToString());
                ReturnsMap.Add("XC5X", Quote.InvestmentParam.RPI.ToString());
                ReturnsMap.Add("XC6X", "£" + Quote.CostProjections.ElementAt(0).CashFlow.ToString());
                result = Math.Round(((Quote.CostSummary.AssumedAnnualElectricityGenerated * Quote.InvestmentParam.AssumedExport) / 100) * Quote.InvestmentParam.ExportTariff, 2);
                ReturnsMap.Add("XC7X", "£" + result.ToString());
                result = result + Quote.CostProjections.ElementAt(0).CashFlow;
                ReturnsMap.Add("XC8X", "£" + result.ToString());

                //Update Predicted Returns Calculations Table
                UpdateTable(OWord, 4, ReturnsMap);

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        private static bool UpdateQuoteDocument_Page12(Word.Application OWord, Quote Quote)
        {
            try
            {
                var ContractPriceMap = new Dictionary<string, string>();
                ContractPriceMap.Add("XC1X", "£" + Quote.InvestmentParam.SystemCost.ToString());
                double OptionalItemsCost = 0;
                ContractPriceMap.Add("XC2X", "£" + OptionalItemsCost.ToString());
                double VAT = 0;
                ContractPriceMap.Add("XC3X", "£" + VAT.ToString());
                double TotalCost = Quote.InvestmentParam.SystemCost + VAT + OptionalItemsCost;
                ContractPriceMap.Add("XC4X", "£" + TotalCost.ToString());
                double DepositPaymentAmount = TotalCost * 0.25;
                ContractPriceMap.Add("XC5X", "£" + DepositPaymentAmount.ToString());
                double FinalPaymentAmount = TotalCost * 0.75;
                ContractPriceMap.Add("XC6X", "£" + FinalPaymentAmount.ToString());

                UpdateInnerTable(OWord, 5, 5, 1, 1, ContractPriceMap);

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        private static bool UpdateROITable_Page22(Word.Application OWord, Quote Quote)
        {
            try
            {
                Word.Table OTable = OWord.ActiveDocument.Tables[9];
                string x = OTable.Cell(1, 1).Range.Text;

                if (!Quote.CostSummary.IsFinanced)
                {
                    //deleting the three columns of finance details
                    OTable.Columns[7].Delete();
                    OTable.Columns[7].Delete();
                    OTable.Columns[7].Delete();
                    OTable.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPercent;
                    OTable.PreferredWidth = 100;
                }

                for (int i = 0; i < Quote.CostProjections.Count; i++)
                {
                    OTable.Cell(i + 2, 1).Range.Text = Quote.CostProjections.ElementAt(i).Year.ToString();
                    OTable.Cell(i + 2, 2).Range.Text = Quote.CostProjections.ElementAt(i).CurrentCostPerkWp.ToString();
                    OTable.Cell(i + 2, 3).Range.Text = Quote.CostProjections.ElementAt(i).OutputkWh.ToString();
                    OTable.Cell(i + 2, 4).Range.Text = "£" + Quote.CostProjections.ElementAt(i).TotalAnnualCost.ToString();
                    OTable.Cell(i + 2, 5).Range.Text = "£" + Quote.CostProjections.ElementAt(i).TotalAnnualRevenue.ToString();
                    OTable.Cell(i + 2, 6).Range.Text = "£" + Quote.CostProjections.ElementAt(i).CashFlow.ToString();

                    if (Quote.CostSummary.IsFinanced)
                    {
                        OTable.Cell(i + 2, 7).Range.Text = "£" + Quote.CostProjections.ElementAt(i).BorrowedAmount.ToString();
                        OTable.Cell(i + 2, 8).Range.Text = "£" + Quote.CostProjections.ElementAt(i).LoanRepaymentAmount.ToString();
                        OTable.Cell(i + 2, 9).Range.Text = "£" + Quote.CostProjections.ElementAt(i).InterestAmount.ToString();
                        OTable.Cell(i + 2, 10).Range.Text = "£" + Quote.CostProjections.ElementAt(i).FinalBalance.ToString();
                    }
                    else
                    {
                        OTable.Cell(i + 2, 7).Range.Text = "£" + Quote.CostProjections.ElementAt(i).FinalBalance.ToString();
                    } 
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void FindAndReplace(Word.Application OWord, object toFindText, object replaceWithText, object replaceValue)
        {
            object matchCase = true;
            object matchwholeWord = true;
            object matchwildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object replace = replaceValue;
            object wrap = 1;

            OWord.Selection.Find.Execute(ref toFindText, ref matchCase, ref matchwholeWord, ref matchwildCards, ref matchSoundLike, ref nmatchAllforms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida, ref matchDiactitics, ref matchAlefHamza, ref matchControl);
        }


        private static bool UpdateTable(Word.Application OWord, int TableIndex, Dictionary<string, string> Values)
        {
            try
            {
                Word.Table OTable = OWord.ActiveDocument.Tables[TableIndex];
                Word.Cell OCell = OTable.Cell(1, 1);

                while (OCell != null)
                {
                    string strCellText = OCell.Range.Text;
                    strCellText = Regex.Replace(strCellText, @"\r", "");
                    strCellText = Regex.Replace(strCellText, @"\a", "");
                    if (Values.ContainsKey(strCellText))
                    {
                        Word.Cell Cell2Update = OCell;
                        Cell2Update.Range.Text = Values.GetValueOrDefault(strCellText);
                    }
                    OCell = OCell.Next;
                }

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        private static bool UpdateInnerTable(Word.Application OWord, int TableIndex, int CellRow, int CellCol, int InnerTableIndex, Dictionary<string, string> Values)
        {
            try
            {
                Word.Table OTable = OWord.ActiveDocument.Tables[TableIndex];
                Word.Cell OCell = OTable.Cell(CellRow, CellCol);
                Word.Table OInnerTable = OCell.Tables[InnerTableIndex];
                Word.Cell OInnerCell = OInnerTable.Cell(1, 1);

                while (OInnerCell != null)
                {
                    string strCellText = OInnerCell.Range.Text;
                    strCellText = Regex.Replace(strCellText, @"\r", "");
                    strCellText = Regex.Replace(strCellText, @"\a", "");
                    if (Values.ContainsKey(strCellText))
                    {
                        Word.Cell Cell2Update = OInnerCell;
                        Cell2Update.Range.Text = Values.GetValueOrDefault(strCellText);
                    }
                    OInnerCell = OInnerCell.Next;
                }

                return true;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
