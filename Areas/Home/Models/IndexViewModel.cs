namespace DropSpace.Areas.Home.Models
{
    public class IndexViewModel
    {
        public string? userName { get; set; }
        public string? MessageType { get; set; }
        public string? Message { get; set; }
        public string? WarningMessage { get; set; }
        public IEnumerable<CrimeTypeViewModel>? type { get; set; }
        public FileUploadSettings? fileLimit { get; set; }
    }
}
