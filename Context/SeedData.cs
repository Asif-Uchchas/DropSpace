using Microsoft.AspNetCore.Identity;
using DropSpace.Data.Entity;

namespace DropSpace.Context
{
    public static class SeedData
    {
        public static void Seed(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var users = userManager.GetUsersInRoleAsync("Employee").Result;

            if (userManager.FindByNameAsync("suza").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "imran",
                    Email = "imran@opus-bd.com",
                    createdAt = DateTime.Now,
                    createdBy = "system"
                };
                var result = userManager.CreateAsync(user, "123456@Phq").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Admin"
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new ApplicationRole
                {
                    Name = "Employee"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
