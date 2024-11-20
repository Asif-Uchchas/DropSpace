using DropSpace.Data.Entity;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.PersonData.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DropSpace.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IUserInfoes userInfoes;
        private readonly IPersonData _persondata;
        private readonly IMasterData _masterdata;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MasterDataController( UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserInfoes userInfoes, IPersonData persondata, IWebHostEnvironment webHostEnvironment, IMasterData masterData)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.userInfoes = userInfoes;
            _persondata = persondata;
            _webHostEnvironment = webHostEnvironment;
            _masterdata = masterData;
        }

        [HttpPost]
        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var countries = await _masterdata.GetAllCountries();
            return countries;
        }

        [HttpPost]
        public async Task<IEnumerable<Division>> GetDivisionByCountryId(int cId)
        {

            var districts = await _masterdata.GetDivisionsByCountryId(cId);

            return districts;
        }

        [HttpPost]
        public async Task<IEnumerable<District>> GetDistrictByDivisionId(int dId)
        {

            var districts = await _masterdata.GetDistrictsByDivisonId(dId);

            return districts;
        }
        [HttpPost]
        public async Task<IEnumerable<Thana>> GetThanaByDistrictId(int dId)
        {
            var thanas = await _masterdata.GetThanasByDistrictId(dId);

            return thanas;
        }
        [HttpPost]
        public async Task<IEnumerable<Thana>> GetActiveThanaByDistrictId(int dId)
        {
            var thanas = await _masterdata.GetActiveThanasByDistrictId(dId);

            return thanas;
        }

        [HttpPost]
        public async Task<IEnumerable<UnionWard>> GetUnionWardsByThanaId(int tId)
        {
            var unionWards = await _masterdata.GetUnionWardsByThanaId(tId);

            return unionWards;
        }
        // GET: api/AddressMaster/GetActiveUnionWardsByThanaId
        [HttpPost]
        public async Task<IEnumerable<UnionWard>> GetActiveUnionWardsByThanaId(int tId)
        {
            var unionWards = await _masterdata.GetActiveUnionWardsByThanaId(tId);

            return unionWards;
        }

        // GET: api/AddressMaster/GetAllVillageByUnionId
        [HttpPost]
        public async Task<IEnumerable<Village>> GetAllVillageByUnionId(int uId)
        {
            var villages = await _masterdata.GetAllVillageByUnionId(uId);

            return villages;
        }
        // GET: api/AddressMaster/GetAllVillageByUnionId
        [HttpPost]
        public async Task<IEnumerable<Village>> GetAllActiveVillageByUnionId(int uId)
        {
            var villages = await _masterdata.GetAllActiveVillageByUnionId(uId);

            return villages;
        }
    }
}
