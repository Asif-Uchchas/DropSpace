namespace DropSpace.Areas.Auth.Models
{
    public class AuthViewModel
    {
        public string? userName { get; set; }
        public List<RoleNameViewModel>? roleNames { get; set; }
        //public List<SpecialBranchUnit>? specialBranchUnits { get; set; }
        //public List<Rank>? ranks { get; set; }
    }

    public class RoleNameViewModel
    {
        public string? roleId { get; set; }
        public string? roleName { get; set; }
    }
}
