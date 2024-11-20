using DropSpace.Areas.Auth.Models;
using Microsoft.AspNetCore.Identity;


namespace DropSpace.ERPServices.AuthService.Interfaces
{
    public interface IRoleManager
    {
        Task<List<ApplicationRoleViewModel>> GetAllRolesAsync();
        Task<IdentityResult> CreateAsync(ApplicationRoleViewModel model);
        Task<IdentityResult> DeleteAsync(ApplicationRoleViewModel model);
        Task<IEnumerable<string>> GetRoleIds(string userName);
    }
}
