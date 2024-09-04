using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.TimeHandler;

namespace BackTestConsole.PortfolioManagement
{
    internal class PortfolioManager
    {

        public Portfolio Portfolio;

        public RegularRebalancer Rebalancer;
        BasketTestParameters TestParameters { get; set; }
        public List<OutputData> OutputDataList { get; set; }
        Pricer Pricer;

        public PortfolioManager(BasketTestParameters testParameters)
        {
            Rebalancer = new RegularRebalancer(testParameters.RebalancingOracleDescription);
            TestParameters = testParameters;
            Pricer = new Pricer(testParameters);
            OutputDataList = new List<OutputData>();

        }


        public bool BalancingTime(int index)
        {
            return index % Rebalancer.Period == 0;
        }




        public void InitializePortfolio(DataFeed dataFeed)
        {

            double[] spots = DataConverter.GetSpots(dataFeed);

            var pricingResults = Pricer.Price(dataFeed.Date, spots);

            Dictionary<string, double> initialComposition = DataConverter.GetComposition(dataFeed, pricingResults.Deltas);

            Portfolio = new Portfolio(initialComposition, dataFeed, pricingResults.Price);

            OutputData outputData = new OutputData
            {
                Date = dataFeed.Date,
                Deltas = pricingResults.Deltas,
                Value = pricingResults.Price,
                Price = pricingResults.Price,

            };
            OutputDataList.Add(outputData);
        }

        public void Update(DataFeed dataFeed)
        {
            double[] spots = DataConverter.GetSpots(dataFeed);

            var pricingResults = Pricer.Price(dataFeed.Date, spots);

            Dictionary<string, double> newComposition = DataConverter.GetComposition(dataFeed, pricingResults.Deltas);

            Portfolio.UpdatePortfolio(newComposition, dataFeed);


            OutputData outputData = new OutputData
            {
                Date = dataFeed.Date,
                Deltas = pricingResults.Deltas,
                Value = Portfolio.GetPortfolioValue(dataFeed),
                Price = pricingResults.Price,
                DeltasStdDev = pricingResults.DeltaStdDev,
                PriceStdDev = pricingResults.PriceStdDev,

            };
            OutputDataList.Add(outputData);

        }


    }
}