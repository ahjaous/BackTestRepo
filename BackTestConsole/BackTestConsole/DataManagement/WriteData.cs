using PricingLibrary.DataClasses;
using System.Text.Json;

namespace BackTestConsole.DataManagement
{
    static class WriteData
    {
        public static void Write(string outputFilePath, List<OutputData> OutputDataList)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            // Sérialiser la liste en JSON
            string jsonString = JsonSerializer.Serialize(OutputDataList, options);

            // Écrire le JSON dans un fichier

            File.WriteAllText(outputFilePath, jsonString);

            Console.WriteLine($"Les données ont été écrites dans le fichier {outputFilePath}");
        }
    }
}