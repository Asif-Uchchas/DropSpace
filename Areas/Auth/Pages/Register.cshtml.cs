using DropSpace.Areas.Auth.Models;
using DropSpace.Contracts;
using DropSpace.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DropSpace.Areas.Auth.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginModel> _logger;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager
            ,IUnitOfWork unitOfWork,  ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
    
            _logger = logger;
            //this.authViewModel = authViewModel;
        }
        [BindProperty]
        public List<RoleNameViewModel>? roleNames { get; set; }
        //public IEnumerable<SpecialBranchUnit>? specialBranchUnits { get; set; }
        //public IEnumerable<Rank>? ranks { get; set; }
        public AuthViewModel? authViewModel { get; set; }


        public async Task OnGetAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                List<RoleNameViewModel> lstRole = new List<RoleNameViewModel>();
                foreach (var data in roles)
                {
                    RoleNameViewModel rolesModel = new RoleNameViewModel
                    {
                        roleId = data.Id,
                        roleName = data.Name
                    };
                    lstRole.Add(rolesModel);
                }
                roleNames = lstRole;
                //ranks = await _unitOfWork.Ranks.FindAll();
                //specialBranchUnits = await _unitOfWork.SpecialBranchUnits.FindAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
