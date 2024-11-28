using DropSpace.Areas.Home.Models;

namespace DropSpace.Services.Filehandling.Interfaces
{
    public interface IFileHandlingService
    {
        Task<FileUploadSettings> GetFileUploadSettingsAsync();
    }
}
