namespace DropSpace.Areas.Auth.Models
{
    public class ApplicationRoleViewModel
    {
        public string? RoleId { get; set; }
        public string? PreRoleId { get; set; }
        public string?[] roleIdList { get; set; }

        public string? userId { get; set; }

        public string? userName { get; set; }
        public string? EuserName { get; set; }

        public string? RoleName { get; set; }

        public int? moduleId { get; set; }

        public string? description { get; set; }
        public string? designation { get; set; }

        public string? moduleName { get; set; }
        //public IEnumerable<AlphaModule> alphaModules { get; set; }
        public IEnumerable<ApplicationRoleViewModel>? roleViewModels { get; set; }
       
        public int userStatus { get; set; }
    }
}
