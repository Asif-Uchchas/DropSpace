using DropSpace.Data.Entity.MasterData;

namespace DropSpace.Data.Entity.Droper
{
    public class UploadedFiles:Base
    {
        public int? personsDataId { get; set; }
        public PersonsData? personsData { get; set; }
        public int? crimeTypeId { get; set; }
        public CrimeInfo? crimeType { get; set; }
        public string? attachmentUrl { get; set; }
        public string? shortUrl { get; set; }
        public string? newFileName { get; set; }
        public string? oldFileName { get; set; }
        public decimal? fileSize { get; set; }
    }
}
