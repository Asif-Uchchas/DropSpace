using DropSpace.Context;
using DropSpace.Data.Entity.MasterData.PublicMapping;
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
            return await _context.countries.ToListAsync();
        }
        public async Task<IEnumerable<Division>> GetDivisionsByCountryId(int CntId)
        {
            return await _context.divisions.Where(X => X.countryId == CntId).ToListAsync();
        }
        public async Task<IEnumerable<District>> GetAllDistrict()
        {
            return await _context.districts.ToListAsync();
        }
        public async Task<IEnumerable<District>> GetDistrictsByDivisonId(int DivisionId)
        {
            return await _context.districts.Where(X => X.divisionId == DivisionId).ToListAsync();
        }
        public async Task<IEnumerable<Thana>> GetThanasByDistrictId(int DistrictId)
        {
            return await _context.thanas.Where(x => x.districtId == DistrictId).ToListAsync();
        }
        public async Task<IEnumerable<Thana>> GetActiveThanasByDistrictId(int DistrictId)
        {
            return await _context.thanas.Where(x => x.districtId == DistrictId && x.isActive != "Inactive" && x.thanaNameBn != ".").ToListAsync();
        }
        public async Task<IEnumerable<UnionWard>> GetUnionWardsByThanaId(int thanaId)
        {
            return await _context.unionWards.Where(X => X.thanaId == thanaId).ToListAsync();
        }

        public async Task<IEnumerable<UnionWard>> GetActiveUnionWardsByThanaId(int thanaId)
        {
            return await _context.unionWards.Include(a => a.thana.district.division).Where(X => X.thanaId == thanaId && X.isActive != "Inactive").ToListAsync();
        }
        public async Task<IEnumerable<Village>> GetAllVillageByUnionId(int id)
        {
            return await _context.villages.Where(x => x.unionWardId == id).OrderByDescending(x => x.Id).ToListAsync();
        }
        public async Task<IEnumerable<Village>> GetAllActiveVillageByUnionId(int id)
        {
            return await _context.villages.Where(x => x.unionWardId == id && x.isActive != "Inactive").OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
