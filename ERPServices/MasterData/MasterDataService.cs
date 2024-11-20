using DropSpace.Context;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPServices.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DropSpace.ERPServices.MasterData
{
    public class MasterDataService:IMasterData
    {
        private readonly DropSpaceDbContext _context;

        public MasterDataService(DropSpaceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var data = await _context.Countries.ToListAsync();
            return data;
        }
        public async Task<IEnumerable<Division>> GetDivisionsByCountryId(int CntId)
        {
            return await _context.Divisions.Where(X => X.countryId == CntId).ToListAsync();
        }
        public async Task<IEnumerable<District>> GetDistrictsByDivisonId(int DivisionId)
        {
            return await _context.Districts.Where(X => X.divisionId == DivisionId).ToListAsync();
        }
        public async Task<IEnumerable<Thana>> GetThanasByDistrictId(int DistrictId)
        {
            return await _context.Thanas.Where(x => x.districtId == DistrictId).ToListAsync();
        }
        public async Task<IEnumerable<Thana>> GetActiveThanasByDistrictId(int DistrictId)
        {
            return await _context.Thanas.Where(x => x.districtId == DistrictId && x.isActive != "Inactive" && x.thanaNameBn != ".").ToListAsync();
        }
        public async Task<IEnumerable<UnionWard>> GetUnionWardsByThanaId(int thanaId)
        {

            var data = await _context.UnionWards.Where(X => X.thanaId == thanaId).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<UnionWard>> GetActiveUnionWardsByThanaId(int thanaId)
        {

            var data = await _context.UnionWards.Include(a => a.thana.district.division).Where(X => X.thanaId == thanaId && X.isActive != "Inactive").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<Village>> GetAllVillageByUnionId(int id)
        {
            return await _context.Villages.Where(x => x.unionWardId == id).OrderByDescending(x => x.Id).ToListAsync();
        }
        public async Task<IEnumerable<Village>> GetAllActiveVillageByUnionId(int id)
        {
            return await _context.Villages.Where(x => x.unionWardId == id && x.isActive != "Inactive").OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
