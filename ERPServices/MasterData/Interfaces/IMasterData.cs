using DropSpace.Data.Entity.MasterData.PublicMapping;

namespace DropSpace.ERPServices.MasterData.Interfaces
{
    public interface IMasterData
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<IEnumerable<Division>> GetDivisionsByCountryId(int CntId);
        Task<IEnumerable<District>> GetAllDistrict();
        Task<IEnumerable<District>> GetDistrictsByDivisonId(int DivisionId);
        Task<IEnumerable<Thana>> GetThanasByDistrictId(int DistrictId);
        Task<IEnumerable<Thana>> GetActiveThanasByDistrictId(int DistrictId);
        Task<IEnumerable<UnionWard>> GetUnionWardsByThanaId(int thanaId);
        Task<IEnumerable<UnionWard>> GetActiveUnionWardsByThanaId(int thanaId);
        Task<IEnumerable<Village>> GetAllVillageByUnionId(int id);
        Task<IEnumerable<Village>> GetAllActiveVillageByUnionId(int id);
    }
}
