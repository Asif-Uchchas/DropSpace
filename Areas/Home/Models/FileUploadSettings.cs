using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.Data.Entity.MasterData.PublicMapping;

namespace DropSpace.Areas.Home.Models
{
    public class FileUploadSettings
    {
        public IEnumerable<FileTypeModel>? fileTypes { get; set; }
        public IEnumerable<FileLimitsModel>? fileLimits { get; set; }
    }


    public class FileTypeModel
    {
        public int fileTypeId { get; set; }
        public string fileTypeName { get; set; } //like mp4, mp3, mkv
        public bool isActive { get; set; }
    }


    public class FileLimitsModel
    {
        public int fileTypeId { get; set; }
        public int hourFileNo { get; set; } //number of files that can be uploaded in an hour
        public int hourFileSize { get; set; } //all file sizes are in megabytes
        public int dayFileNo { get; set; } //number of files that can be uploaded in a day
        public int dayFileSize { get; set; }
        public int totalFileNo { get; set; } //number of files that can be uploaded
        public int totalFileSize { get; set; }
        public int alertFileSize { get; set; }
        public int archiveFileSize { get; set; }
        public int archivingMonth { get; set; }
        public bool isActive { get; set; } = true;
    }

    


}
