namespace DropSpace.Areas.Home.Models
{
    public class MediaViewModel
    {
        public List<MediaFileViewModel> Files { get; set; } = new();
    }

    public class MediaFileViewModel
    {
        public string Url { get; set; }
        public string FileType { get; set; }
        public string FileName => Path.GetFileName(Url);
    }

}
