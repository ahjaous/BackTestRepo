using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackTestConsole.PortfolioManagement;

namespace BackTestConsole.PortfolioManagement
{
    internal class Portfolio
    {

        public Dictionary<string, double> Composition;

        private double Cash { get; set; }

        public DateTime LastRebalancingDate { get; private set; }


        public Portfolio(Dictionary<string, double> initialComposition, DataFeed dataFeed, double initialValue ) 
        { 
            Composition = initialComposition;
            LastRebalancingDate = dataFeed.Date;

            double[] spots = DataConverter.GetSpots(dataFeed);
            double buyAssets = DataConverter.ScalarProduct(Composition.Values.ToArray(), spots);
            Cash = initialValue - buyAssets;
        }  


        public void UpdatePortfolio(Dictionary<string, double> newComposition, DataFeed dataFeed) 
        {

            double[] spots = DataConverter.GetSpots(dataFeed);

            double buyAssets = DataConverter.ScalarProduct(newComposition.Values.ToArray(), spots);
            double sellAssets = DataConverter.ScalarProduct(Composition.Values.ToArray(), spots);

            Cash = Cash * RiskFreeRateProvider.GetRiskFreeRateAccruedValue(LastRebalancingDate, dataFeed.Date) - buyAssets + sellAssets;
            Composition = newComposition;
            LastRebalancingDate = dataFeed.Date;
        }

        public double GetPortfolioValue(DataFeed dataFeed)
        {
            double[] spots = DataConverter.GetSpots(dataFeed);
            double buyAssets = DataConverter.ScalarProduct(Composition.Values.ToArray(), spots);
            return Cash + buyAssets;
        }





    }
}