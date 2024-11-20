using Microsoft.AspNetCore.Authorization;
using DropSpace.Context;
using System.Security.Claims;

namespace DropSpace.Helpers
{
    public class CustomAuthorizationPolicy : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        private readonly DropSpaceDbContext _dbContext;

        public CustomAuthorizationPolicy(DropSpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            // Get the user's roles from the database
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roles = _dbContext.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToList();

            // Check if the user has the required role
            if (roles.Contains(requirement.Role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public string? Role { get; }

        public CustomAuthorizationRequirement(string? role)
        {
            Role = role;
        }
    }


}
