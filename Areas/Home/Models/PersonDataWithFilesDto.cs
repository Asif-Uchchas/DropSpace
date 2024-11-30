using DropSpace.Data.Entity.MasterData;

namespace DropSpace.Areas.Home.Models
{
    public class PersonDataWithFilesDto
    {
        public int Id { get; set; }
        public string? mobileMsk { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public int? UnionId { get; set; }
        public string? UnionName { get; set; }
        public int? VillageId { get; set; }
        public string? VillageName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? otp { get; set; }
        public string? otpMsk { get; set; }
        public DateTime? createdAt { get; set; }
        public List<UploadedFileDto>? UploadedFiles { get; set; }
    }

    public class UploadedFileDto
    {
        public int Id { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? crimeTypeId { get; set; }
        public CrimeInfo? crimeType { get; set; }
        public string? shortUrl { get; set; }
        public string? newFileName { get; set; }
        public string? oldFileName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? uploadDatetime { get; set; }
    }
}
