using DropSpace.Data.Entity.MasterData.PublicMapping;
using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Data.Entity.MasterData.PoliceMapping
{
    public class PoliceThana:Base
    {
        public int? rangeMetroId { get; set; }
        public RangeMetro rangeMetro { get; set; }

        public int? divisionDistrictId { get; set; }
        public DivisionDistrict divisionDistrict { get; set; }

        public int? zoneCircleId { get; set; }
        public ZoneCircle zoneCircle { get; set; }

        public int? upazillaId { get; set; }
        public Thana upazilla { get; set; }

        [Column(TypeName = "NVARCHAR(350)")]
        public string? policeThanaName { get; set; }
        [Column(TypeName = "NVARCHAR(350)")]
        public string? policeThanaNameBn { get; set; }
        [Column(TypeName = "NVARCHAR(10)")]
        public string? isActive { get; set; }
        [Column(TypeName = "NVARCHAR(10)")]
        public string? isReportable { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? latitude { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? longitude { get; set; }

        [ForeignKey("policeThana")]
        public int? policeThanaId { get; set; }
        public virtual PoliceThana? policeThana { get; set; }
        public string? address { get; set; }
        public int? fariType { get; set; } // 1=Fari, 2=Camp,3=Investigation Center,4=Office
        public int? isChild { get; set; }
        [NotMapped]
        public string? stringId { get; set; }


    }
}
