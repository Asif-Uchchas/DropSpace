
using DropSpace.Data.Entity.LogInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DropSpace.ERPServices.AuthService.Interfaces
{
    public interface IDbChangeService
    {
        Task<int> SaveUserLogHistory(UserLogHistory userLogHistory);

        Task<IEnumerable<UserLogHistory>> GetAllUserLogHistory();

        Task<IEnumerable<UserLogHistory>> GetUserLogHistoryByUser(string userName);
    }
}
