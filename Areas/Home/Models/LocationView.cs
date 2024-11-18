using Microsoft.AspNetCore.Mvc.Rendering;

namespace DropSpace.Areas.Home.Models
{
    public class LocationView
    {
        
    }

    public class LocationRequest
    {
        public string? SelectedDivision { get; set; }
        public string? SelectedDistrict { get; set; }
        public string? SelectedThana { get; set; }
        public string? SelectedUnion { get; set; }
    }

    // Models/LocationResponse.cs
    public class LocationResponse
    {
        public List<SelectListItem>? Divisions { get; set; }
        public List<SelectListItem>? Districts { get; set; }
        public List<SelectListItem>? Thanas { get; set; }
        public List<SelectListItem>? Unions { get; set; }
    }
}
