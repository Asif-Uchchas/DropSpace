namespace DropSpace.Data.Entity.MasterData
{
    public class FileType:Base
    {
        public string fileTypeName { get; set; }
        public bool isActive { get; set; } = true;
    }
}
