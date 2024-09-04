using PricingLibrary.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackTestConsole.PortfolioManagement
{
    internal static class DataConverter
    {

        public static double[] GetSpots(DataFeed dataFeed)
        {
            return dataFeed.PriceList.Values.ToArray();
        }

        public static Dictionary<string, double> GetComposition(DataFeed dataFeed, double[] composition)
        {
            return dataFeed.PriceList.Keys.Zip(composition, (key, value) => new { key, value })
                                                       .ToDictionary(item => item.key, item => item.value);
        }

        public static double ScalarProduct(double[] firstArray, double[] secondArray)
        {
            return firstArray.Zip(secondArray, (x, y) => x * y).Sum();
        }
    }
}
