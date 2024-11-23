using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Data.Entity.MasterData.PublicMapping
{
    public class Country : Base
    {
        [Column(TypeName = "NVARCHAR(20)")]
        public string? countryCode { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? countryName { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? countryNameBn { get; set; }
        [Column(TypeName = "NVARCHAR(20)")]
        public string? shortName { get; set; }
        [Column(TypeName = "NVARCHAR(10)")]
        public string? isActive { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? latitude { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? longitude { get; set; }

    }
}
