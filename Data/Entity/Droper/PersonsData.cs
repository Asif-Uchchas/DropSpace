using DropSpace.Data.Entity.MasterData.PublicMapping;

namespace DropSpace.Data.Entity.Droper
{
    public class PersonsData:Base
    {
        public string? name { get; set; }
        public string? mobile { get; set; }
        public int? unionId { get; set; }
        public UnionWard? union { get; set; }
        public int? villageId { get; set; }
        public Village? village { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
    }
}
