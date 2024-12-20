﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Data.Entity.MasterData.PoliceMapping
{
    public class RangeMetro:Base
    {
        [Column(TypeName = "NVARCHAR(350)")]
        public string? rangeMetroName { get; set; }
        [Column(TypeName = "NVARCHAR(350)")]
        public string? rangeMetroNameBn { get; set; }
        [Column(TypeName = "NVARCHAR(10)")]
        public string? isActive { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? latitude { get; set; }
        [Column(TypeName = "NVARCHAR(120)")]
        public string? longitude { get; set; }

        [NotMapped]
        public string? stringId { get; set; }

    }
}
