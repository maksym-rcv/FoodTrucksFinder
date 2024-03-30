using FoodTrucksFinder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace FoodTrucksFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFoodTruckRepository _context;

        public HomeController(IFoodTruckRepository context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult FindFoodTrucks(FoodTrucksViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Get food trucks nearest to the given location and based on the preferred food
                model.FoodTrucks = _context.FoodTrucks
                    .Where(ft => ft.FoodItems.ToLower().Contains(model.PreferredFood.ToLower()))
                    .OrderBy(ft => CalculateDistance(ft.Latitude, ft.Longitude, model.Latitude, model.Longitude))
                    .Take(model.AmountOfResults)
                    .ToList();

                return PartialView("_SearchResults", model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message /*$"An error occurred while processing your request. {ex.Message}"*/);
            }
        }

        // Function to calculate distance between two coordinates using Haversine formula
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the Earth in kilometers

            var latDifference = (lat2 - lat1) * (Math.PI / 180);  // latitude difference with convertion from degrees to radians
            var lonDifference = (lon2 - lon1) * (Math.PI / 180);  // longitude difference with convertion from degrees to radians

            var a = Math.Pow(Math.Sin(latDifference / 2), 2) +
                Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                Math.Pow(Math.Sin(lonDifference /2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // Distance in kilometers

            return distance;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}