using Microsoft.AspNetCore.Identity;
using DropSpace.Data.Entity;
using DropSpace.Data.Entity.MasterData;

namespace DropSpace.Context
{
    public static class SeedData
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, DropSpaceDbContext context)
        {
            await SeedUserTypeAsync(context);
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager, context);
        }
        private static async Task SeedUserTypeAsync(DropSpaceDbContext context)
        {
            var userTypes = new[]
            {
                new UserType { userTypeName = "Public", isActive = true },
                new UserType { userTypeName = "Police", isActive = true },
            };

            foreach (var userType in userTypes)
            {
                // Check if the user type already exists
                if (!context.userTypes.Any(ut => ut.userTypeName == userType.userTypeName))
                {
                    await context.userTypes.AddAsync(userType);
                }
            }

            await context.SaveChangesAsync();
        }
        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new ApplicationRole { Name = "Admin" };
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Public"))
            {
                var role = new ApplicationRole { Name = "Public" };
                await roleManager.CreateAsync(role);
            }
        }
        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, DropSpaceDbContext context)
        {

            if (await userManager.FindByNameAsync("opususer") == null)
            {
                var userTypeId = context.userTypes.Where(ut => ut.userTypeName == "Public").FirstOrDefault().Id;
                var user = new ApplicationUser
                {
                    UserName = "opususer",
                    Email = "info@opus-bd.com",
                    userType = userTypeId,
                    createdAt = DateTime.Now,
                    createdBy = "system"
                };
                var result = await userManager.CreateAsync(user, "123456@Phq");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Public");
                }
            }

            if (await userManager.FindByNameAsync("opusadmin") == null)
            {
                var userTypeId = context.userTypes.Where(ut => ut.userTypeName == "Police").FirstOrDefault().Id;
                var user = new ApplicationUser
                {
                    UserName = "opusadmin",
                    Email = "info@opus-bd.com",
                    userType = userTypeId,
                    createdAt = DateTime.Now,
                    createdBy = "system"
                };
                var result = await userManager.CreateAsync(user, "123456@Phq");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

    }
}
