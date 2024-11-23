using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Data.Entity.MasterData.PublicMapping
{
    public class UnionWard : Base
    {
        public int thanaId { get; set; }
        public Thana thana { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        public string? unionCode { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? unionName { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? unionNameBn { get; set; }
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
