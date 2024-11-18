//using DropSpace.Areas.Home.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace DropSpace.Areas.Home.Controllers
//{
//    public class LocationController : Controller
//    {
//        private readonly ILocationService _locationService;


//        public LocationController(ILocationService locationService)
//        {
//            _locationService = locationService;
//        }


//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> GetLocations([FromBody] LocationRequest request)
//        {
//            try
//            {
//                var response = await _locationService.GetLocationDataAsync(request);
//                return Json(response);
//            }
//            catch (Exception)
//            {
//                // Log error securely without exposing details
//                return Json(new LocationResponse());
//            }
//        }


//    }
//}
