namespace FoodTrucksFinder.Models
{
    public interface IFoodTruckRepository
    {
        IQueryable<FoodTruck> FoodTrucks { get; }
    }
}
