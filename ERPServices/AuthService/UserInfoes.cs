
using DropSpace.Areas.Auth.Models;
using DropSpace.Context;
using DropSpace.Data.Entity;
using DropSpace.ERPService.AuthService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DropSpace.ERPServices.AuthService
{
    public class UserInfoes : IUserInfoes
    {
        private readonly DropSpaceDbContext _context;

        public UserInfoes(DropSpaceDbContext context)
        {
            _context = context;
        }

       
        public async Task<ApplicationUser> GetAspnetuserByUser(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser> GetUserInfoByUser(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<string>> GetRoleListByUserId(string Id)
        {
            return  await _context.UserRoles.Where(x=>x.UserId==Id).Select(x=>x.RoleId).ToListAsync();
        }
          public async Task<string> GetRoleNameByUserId(string Id)
        {
            var data =  await _context.UserRoles.Where(x=>x.UserId==Id).FirstOrDefaultAsync();
            var roleName = await _context.Roles.Where(x => x.Id == data.RoleId).Select(x=>x.Name).FirstOrDefaultAsync();
            
            return roleName;
        }

        


        public async Task<string> GetRoleNameByUserIdNew(string Id)
        {
            var data = await _context.UserRoles.Where(x => x.UserId == Id).LastOrDefaultAsync();
            var roleName = await _context.Roles.Where(x => x.Id == data.RoleId).Select(x => x.Name).FirstOrDefaultAsync();

            return roleName;
        }


        public async Task<bool> DeleteUserRoleListByUserId(string Id)
        {
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == Id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRoleById(string Id)
        {
            _context.Roles.Remove(_context.Roles.Where(x => x.Id == Id).First());
            return 1 == await _context.SaveChangesAsync();
        }

        
        public async Task<ApplicationUser> GetUserBasicInfoes(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ApplicationUser>> GetUserInfoListByUser(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).ToListAsync();
        }

       
       
       
        public async Task<bool> DeleteUserById(string id)
        {
            _context.Users.Remove(await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync());
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<ApplicationUser> UpdateAspNetUser(string userId)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> UpdateAspNetUser(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByUserName1(string userName)
        {
            List<AspNetUsersViewModel> result = (from U in _context.Users
                                                 
                                                 where U.UserName == userName
                                                 //where m.ApplicationUser.UserName == userName
                                                 select new AspNetUsersViewModel
                                                 {
                                                     aspnetId = U.Id,
                                                     UserName = U.UserName,
                                                     Email = U.Email,
                                                     UserId = U.Id,
                                                     isActive = U.isActive,
                                                 }).ToList();


            var aspnetolelist = _context.UserRoles.ToList();
            var aspnetrolenamelist = _context.Roles.ToList();
            List<AspNetUsersViewModel> aspNetUsersViewModels = new List<AspNetUsersViewModel>();
            foreach (AspNetUsersViewModel data in result)
            {
                var roleId = aspnetolelist.Where(x => x.UserId == data.aspnetId).ToList();
                List<string> role = new List<string>();
                foreach (var UserRole in roleId)
                {
                    string rnam = aspnetrolenamelist.Where(x => x.Id == UserRole.RoleId).Select(x => x.Name).First();
                    role.Add(rnam);
                }
                aspNetUsersViewModels.Add(new AspNetUsersViewModel
                {
                    aspnetId = data.aspnetId,
                    UserName = data.UserName,
                    UserId = data.UserId,
                    isActive = data.isActive,
                });

            }
            return aspNetUsersViewModels;
        }

    }
}
