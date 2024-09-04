using CsvHelper;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;

using System.Globalization;

using System.Text.Json.Serialization;
using System.Text.Json;


namespace DotNetProject.DataManagement
{
    static class LoadData
    {
        public static BasketTestParameters GetTestParameters(string testParamsPath)
        {
            string jsonFile = File.ReadAllText(testParamsPath);

            // Créer les options de désérialisation
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };

            return JsonSerializer.Deserialize<BasketTestParameters>(jsonFile, options)!;

        }

        public static List<DataFeed> GetDataFeeds(string testDataPath)
        {
            var shareValues = new List<ShareValue>();
            

            using (var reader = new StreamReader(testDataPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read records from the CSV file
                var records = csv.GetRecords<ShareValue>();

                // Convert each record to a ShareValue object
                foreach (var record in records)
                {
                    ShareValue shareValue = new ShareValue();
                    shareValue.Value = record.Value;
                    shareValue.DateOfPrice = record.DateOfPrice;
                    shareValue.Id = record.Id;
                    shareValues.Add(shareValue);

                }
            }
            var DataFeeds = shareValues.GroupBy(d => d.DateOfPrice,
                            t => new { Symb = t.Id.Trim(), Val = t.Value },
                            (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val))).ToList();
            return DataFeeds;
        }
    }
}
