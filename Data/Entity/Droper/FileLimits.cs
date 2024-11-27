using DropSpace.Data.Entity.MasterData;

namespace DropSpace.Data.Entity.Droper
{
    public class FileLimits:Base
    {
        public int fileTypeId { get; set; }
        public FileType fileType { get; set; }
        public int hourFileNo { get; set; }
        public int hourFileSize { get; set; }
        public int dayFileNo { get; set; }
        public int dayFileSize { get; set; }
        public int totalFileNo { get; set; }
        public int totalFileSize { get; set; }
        public int alertFileSize { get; set; }
        public int archiveFileSize { get; set; }
        public int archivingMonth { get; set; }
        public bool isActive { get; set; } = true;
    }
}
