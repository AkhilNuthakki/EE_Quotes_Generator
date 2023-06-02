using System;
using System.Collections.Generic;
using System.Linq;

using EEQuoteGenerator.Models;
using System.Threading.Tasks;

namespace EEQuoteGenerator.Services
{
    public class CalculateCostModelService
    {
        public const int YEARS_OF_COSTMODEL = 20;
        public const double DEPRECATION = 0.005;
      
        public static Quote CalculateandUpdateCostSummaryandProjections(Quote Quote)
        {
            try
            {
                InvestmentParameters quote_inputs = Quote.InvestmentParam;
                //calculation of some input paramters
                Quote.InvestmentParam.TotalCurrentElectricityCost = Math.Round(quote_inputs.CurrentElectricityCost + quote_inputs.GreenLevyTaxesCost, 4);
                Quote.InvestmentParam.SystemSize = (quote_inputs.PanelWatts * ((double)quote_inputs.NoOfPanels / 1000));


                QuoteCostSummary cost_summary = new QuoteCostSummary();
                List<CostProjection> projections = new List<CostProjection>();
                List<double> cashflowList = new List<double>();
                double OutputKWhSum = 0;

                //setting quote id variable
                cost_summary.QuoteId = Quote.QuoteId;
                // setting financed or unfinaced flag
                double TotalIntrestAmount = 0;
                if (quote_inputs.PercentageFinanced > 0){
                    cost_summary.IsFinanced = true;
                }else{
                    cost_summary.IsFinanced = false;
                }

                //calculation of each and every variable of cost model and adding to the costprojections list
                for (int i = 0; i < YEARS_OF_COSTMODEL; i++)
                {
                    CostProjection costmodel = new CostProjection();
                    costmodel.Year = i+1;
                    if (i == 0)
                    {
                        costmodel.CurrentCostPerkWp = quote_inputs.TotalCurrentElectricityCost;
                        costmodel.OutputkWh = Math.Round(quote_inputs.SystemSize * quote_inputs.AnnualGeneration);
                        costmodel.CapitalOutlay = quote_inputs.SystemCost;
                        costmodel.InsuranceCost = quote_inputs.AnnualInsurance;
                        costmodel.MaintenanceCost = quote_inputs.AnnualMaintenancPackage;
                        costmodel.MonitoringOnlyCost = quote_inputs.AnnualMonitoringOnlyPackage;

                        // Financed Model Calculations
                        if (cost_summary.IsFinanced)
                        {
                            costmodel.LoanRepaymentAmount = i > quote_inputs.LoanPeriodYears ? 0 : Math.Round(((costmodel.CapitalOutlay * quote_inputs.PercentageFinanced) / 100) / quote_inputs.LoanPeriodYears);
                            costmodel.BorrowedAmount = Math.Round(((costmodel.CapitalOutlay * quote_inputs.PercentageFinanced) / 100) - costmodel.LoanRepaymentAmount);
                            costmodel.InterestAmount = costmodel.BorrowedAmount > 0 ? Math.Round((((costmodel.CapitalOutlay * quote_inputs.PercentageFinanced) / 100) * quote_inputs.BorrowingCost) / 100) : 0;
                        }
                       
                        // 

                        costmodel.TotalAnnualCost = Math.Round(costmodel.InsuranceCost + costmodel.MaintenanceCost + costmodel.MonitoringOnlyCost + costmodel.LoanRepaymentAmount + costmodel.InterestAmount);
                        costmodel.RevenueFromSaving = Math.Round((costmodel.OutputkWh - (costmodel.OutputkWh * quote_inputs.PercetageElectricityExported) / 100) * costmodel.CurrentCostPerkWp);
                        costmodel.RevenueFromExport = Math.Round((costmodel.OutputkWh * quote_inputs.AssumedExport * quote_inputs.ExportTariff) / 100);
                        costmodel.TotalAnnualRevenue = costmodel.RevenueFromExport + costmodel.RevenueFromSaving;
                        costmodel.CashFlow = costmodel.TotalAnnualRevenue - costmodel.TotalAnnualCost;
                        costmodel.FinalBalance = Math.Round(costmodel.CashFlow - (quote_inputs.SystemCost - ((costmodel.CapitalOutlay * quote_inputs.PercentageFinanced) / 100)));

                    }
                    else
                    {
                        costmodel.CurrentCostPerkWp = Math.Round(projections.ElementAt(i - 1).CurrentCostPerkWp + ((quote_inputs.AnnualFuelPriceInflation / 100) * projections.ElementAt(i - 1).CurrentCostPerkWp), 3);
                        costmodel.OutputkWh = Math.Round(quote_inputs.SystemSize * quote_inputs.AnnualGeneration * Math.Pow(1 - DEPRECATION, i));
                        costmodel.CapitalOutlay = quote_inputs.SystemCost;
                        costmodel.InsuranceCost = Math.Round(projections.ElementAt(i - 1).InsuranceCost + (projections.ElementAt(i - 1).InsuranceCost * (quote_inputs.RPI / 100)));
                        costmodel.MaintenanceCost = Math.Round(projections.ElementAt(i - 1).MaintenanceCost + (projections.ElementAt(i - 1).MaintenanceCost * (quote_inputs.RPI / 100)));
                        costmodel.MonitoringOnlyCost = Math.Round(projections.ElementAt(i - 1).MonitoringOnlyCost + (projections.ElementAt(i - 1).MonitoringOnlyCost * (quote_inputs.RPI / 100)));

                        // Financed Model Calculations
                        if (cost_summary.IsFinanced)
                        {
                            costmodel.LoanRepaymentAmount = i+1 > quote_inputs.LoanPeriodYears ? 0 : Math.Round(((costmodel.CapitalOutlay * quote_inputs.PercentageFinanced) / 100) / quote_inputs.LoanPeriodYears);
                            costmodel.BorrowedAmount = (projections.ElementAt(i - 1).BorrowedAmount - costmodel.LoanRepaymentAmount) <= 0 ? 0 : projections.ElementAt(i - 1).BorrowedAmount - costmodel.LoanRepaymentAmount;
                            costmodel.InterestAmount = projections.ElementAt(i - 1).BorrowedAmount > 0 ? Math.Round((projections.ElementAt(i - 1).BorrowedAmount * quote_inputs.BorrowingCost) / 100) : 0;
                        }
                        // 

                        costmodel.TotalAnnualCost = costmodel.InsuranceCost + costmodel.MaintenanceCost + costmodel.MonitoringOnlyCost + costmodel.LoanRepaymentAmount + costmodel.InterestAmount;
                        costmodel.RevenueFromSaving = Math.Round((costmodel.OutputkWh - (costmodel.OutputkWh * quote_inputs.PercetageElectricityExported) / 100) * costmodel.CurrentCostPerkWp);
                        costmodel.RevenueFromExport = Math.Round((costmodel.OutputkWh * quote_inputs.AssumedExport * quote_inputs.ExportTariff) / 100);
                        costmodel.TotalAnnualRevenue = costmodel.RevenueFromExport + costmodel.RevenueFromSaving;
                        costmodel.CashFlow = costmodel.TotalAnnualRevenue - costmodel.TotalAnnualCost;
                        costmodel.FinalBalance = projections.ElementAt(i - 1).FinalBalance + costmodel.CashFlow;


                    }

                    //necessary calculations for quote cost summary
                    if (costmodel.FinalBalance > 0 && cost_summary.PayBackYears == 0)
                    {
                        cost_summary.PayBackYears = i;
                    }
                    if (cost_summary.IsFinanced)
                    {
                        TotalIntrestAmount += costmodel.InterestAmount;
                    }
                    cashflowList.Add(costmodel.CashFlow);
                    OutputKWhSum += costmodel.OutputkWh;

                    costmodel.QuoteId = Quote.QuoteId;
                    projections.Add(costmodel);
                }


                //setting Estimated 20 Year profit variable
                cost_summary.EST20YearProfit = projections.ElementAt(projections.Count - 1).FinalBalance;
                //setting Assumed Annula Electricity Generated variable
                cost_summary.AssumedAnnualElectricityGenerated = projections.ElementAt(0).OutputkWh;
                //setting IRR variable after calculation
                double[] cashflowArray = cashflowList.ToArray();
                cost_summary.IRR = 30.0;
                //setting capital Outlay 
                cost_summary.CapitalOutlay = quote_inputs.SystemCost + (int)TotalIntrestAmount;
                //setting Yeild variable after calculation
                cost_summary.Yield = Math.Round(projections.ElementAt(0).TotalAnnualRevenue / cost_summary.CapitalOutlay, 2) * 100;
                // setting CO2 Savings for 20 years variable
                cost_summary.CO2SavingsYear20 = (int)(OutputKWhSum * 0.541) / 1000;
                // setting CO2 Savings Per Year variable
                cost_summary.CO2SavingsPerYear = cost_summary.CO2SavingsYear20 / 20;

                // setting costprojections and cost summary variable
                Quote.CostProjections = projections;
                Quote.CostSummary = cost_summary;

                return Quote;
            }
            catch
            {
                return Quote;
            }
            
        }
    }
}
