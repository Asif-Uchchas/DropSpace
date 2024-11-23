namespace DropSpace.Areas.Home.Models
{
    public class MasterDataViewModel
    {
        public string? cId { get; set; }
        public string? dId { get; set; }
        public string? tId { get; set; }
        public string? uId { get; set; }
    }
    public class CountryViewModel
    {
        public string? countryNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class DivisionViewModel
    {
        public string? divisionNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class DistrictViewModel
    {
        public string? districtNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class PoliceStationViewModel
    {
        public string? policeThanaNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class UnionWordViewModel
    {
        public string? unionWordNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class VillageViewModel
    {
        public string? villageNameBn { get; set; }
        public string? Id { get; set; }
    }
}
