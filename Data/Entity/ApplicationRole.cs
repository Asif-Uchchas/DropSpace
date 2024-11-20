using Microsoft.AspNetCore.Identity;

namespace DropSpace.Data.Entity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string? roleName) : base(roleName)
        {
        }
    }
}
