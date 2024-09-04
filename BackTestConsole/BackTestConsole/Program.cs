using BackTestConsole.DataManagement;
using BackTestConsole.PortfolioManagement;
using DotNetProject.DataManagement;

namespace BackTestConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Vérification du nombre d'arguments
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: BacktestConsole.exe <test-params> <mkt-data> <output-file>");
                return;
            }

            // Extraction des chemins des fichiers à partir des arguments
            string testParamsPath = args[0];
            string testDataPath = args[1];
            string outputFilePath = args[2];

            try
            {
                // Charger les paramètres de test et les données de marché
                var testParameters = LoadData.GetTestParameters(testParamsPath);
                var dataFeedList = LoadData.GetDataFeeds(testDataPath);

                //Initialisation des variables

                PortfolioManager portfolioManager = new PortfolioManager(testParameters);

                portfolioManager.InitializePortfolio(dataFeedList[0]);

                for (int i = 1; i < dataFeedList.Count; i++)
                {
                    if (portfolioManager.BalancingTime(i))
                    {
                        portfolioManager.Update(dataFeedList[i]);
                    }
                }
                WriteData.Write(outputFilePath, portfolioManager.OutputDataList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}