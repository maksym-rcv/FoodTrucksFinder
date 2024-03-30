namespace FoodTrucksFinder.Models
{
    public class FoodTrucksViewModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int AmountOfResults { get; set; }
        public string PreferredFood { get; set; }
        public List<FoodTruck>? FoodTrucks { get; set; }
    }
}
