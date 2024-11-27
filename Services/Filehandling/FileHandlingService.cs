using DropSpace.Areas.Home.Models;
using DropSpace.Context;
using DropSpace.Data.Entity.MasterData;
using DropSpace.Services.Filehandling.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DropSpace.Services.Filehandling
{
    public class FileHandlingService : IFileHandlingService
    {
        private readonly DropSpaceDbContext _context;

        public FileHandlingService(DropSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<FileUploadSettings> GetFileUploadSettingsAsync()
        {
            // Fetch the file types and limits from the database
            var fileTypes = await _context.fileTypes.Where(ft => ft.isActive).ToListAsync();
            var fileLimits = await _context.fileLimits.Where(fl => fl.isActive).ToListAsync();

            return new FileUploadSettings
            {
                fileTypes = fileTypes.Select(ft => new FileTypeModel
                {
                    fileTypeId = ft.Id,
                    fileTypeName = ft.fileTypeName,
                    isActive = ft.isActive
                }),
                fileLimits = fileLimits.Select(fl => new FileLimitsModel
                {
                    fileTypeId = fl.fileTypeId,
                    hourFileNo = fl.hourFileNo,
                    hourFileSize = fl.hourFileSize,
                    dayFileNo = fl.dayFileNo,
                    dayFileSize = fl.dayFileSize,
                    totalFileNo = fl.totalFileNo,
                    totalFileSize = fl.totalFileSize,
                    alertFileSize = fl.alertFileSize,
                    archiveFileSize = fl.archiveFileSize,
                    archivingMonth = fl.archivingMonth,
                    isActive = fl.isActive
                })
            };
        }
    }
}
