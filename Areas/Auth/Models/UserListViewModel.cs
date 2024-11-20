using System.Collections.Generic;

namespace DropSpace.Areas.Auth.Models
{
    public class UserListViewModel
    {
        public IEnumerable<AspNetUsersViewModel>? aspNetUsersViewModels { get; set; }
        public IEnumerable<ApplicationRoleViewModel>? userRoles { get; set; }
    }
}
