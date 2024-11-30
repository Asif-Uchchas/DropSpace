using DropSpace.Areas.Home.Models;
using DropSpace.Data.Entity.Droper;

namespace DropSpace.ERPServices.PersonData.Interfaces
{
    public interface IPersonData
    {
        Task<int> AddPersonsDataAsync(PersonsData personsData);
        Task AddUploadedFilesAsync(IEnumerable<UploadedFiles> uploadedFiles);
        Task<List<PersonDataWithFilesDto>> GetPersonDataWithFilesByMobileAsync(string mobile, string otp);
        Task<bool> IsTheSamePerson(string mobile, string otp);
        Task<Dictionary<int, int>> GetHourlyDataCountAsync(DateTime date);
        Task<Dictionary<DateTime, int>> GetDailyDataCountAsync(DateTime startDate, DateTime endDate);
        Task<List<PersonDataWithFilesDto>> GetPersonDataWithFilesAsync(DateTime date, int? hour = null);
        Task<bool> CheckShortUrl(string url);
        Task<UploadedFiles> GetUrlFromShortUrl(string url);
    }
}
