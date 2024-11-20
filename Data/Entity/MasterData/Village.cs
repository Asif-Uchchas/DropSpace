using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Data.Entity.MasterData
{
    public class Village : Base
    {
        public int unionWardId { get; set; }
        public UnionWard unionWard { get; set; }

        public int? thanaId { get; set; }
        public Thana thana { get; set; }

        public int? districtsId { get; set; }
        public District districts { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        public string? villageCode { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? villageName { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? villageNameBn { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        public string? shortName { get; set; }
        [Column(TypeName = "NVARCHAR(10)")]
        public string? isActive { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? latitude { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? longitude { get; set; }



    }
}
