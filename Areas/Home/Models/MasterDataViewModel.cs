namespace DropSpace.Areas.Home.Models
{
    public class MasterDataViewModel
    {
        public string? cId { get; set; }
        public string? dId { get; set; }
        public string? tId { get; set; }
        public string? uId { get; set; }
        public string? mId { get; set; }
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
    public class CrimeTypeViewModel
    {
        public string? crimeTypeNameBn { get; set; }
        public string? Id { get; set; }
    }
    public class PersonsDataViewModel
    {
        public string? typeId { get; set; }
        public DateTime? dateOf { get; set; }
        public string? timeOf { get; set; }
        public string? crimeName { get; set; }
        public string? locationText { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public string? mobile { get; set; }
        public string? unionId { get; set; }
        public string? villageId { get; set; }
    }
}
