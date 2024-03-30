using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace FoodTrucksFinder.Models
{
    public class CsvFoodTruckRepository : IFoodTruckRepository
    {
        private readonly string _csvFilePath;

        public CsvFoodTruckRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public IQueryable<FoodTruck> FoodTrucks
        {
            get
            {
                if (!File.Exists(_csvFilePath))
                {
                    throw new FileNotFoundException($"CSV file '{_csvFilePath}' was not found.");
                }

                return LoadFoodTrucksFromCsv().AsQueryable();
            }
        }

        private List<FoodTruck> LoadFoodTrucksFromCsv()
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                var records = csv.GetRecords<FoodTruck>().ToList();
                return records;
            }
        }
    }
}
