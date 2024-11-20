using DropSpace.Areas.Auth.Models;
using DropSpace.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DropSpace.ERPService.AuthService.Interfaces
{
    public interface IUserInfoes
    {

        Task<string> UpdateAspNetUser(ApplicationUser user);
        Task<ApplicationUser> GetAspnetuserByUser(string userName);
        Task<ApplicationUser> GetUserInfoByUser(string userName);
        Task<IEnumerable<ApplicationUser>> GetUserInfoListByUser(string userName);
        Task<ApplicationUser> GetUserBasicInfoes(string userName);
        
        Task<IEnumerable<string>> GetRoleListByUserId(string Id);
        Task<bool> DeleteUserRoleListByUserId(string Id);
        Task<bool> DeleteRoleById(string Id);
       
        Task<bool> DeleteUserById(string id);
        Task<ApplicationUser> UpdateAspNetUser(string userId);
       
        Task<string> GetRoleNameByUserId(string Id);
        Task<string> GetRoleNameByUserIdNew(string Id);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByUserName1(string userName);




    }
}
