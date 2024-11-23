using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DropSpace.Data.Entity.LogInfo
{
    public class UserLogHistory : Base
    {
        [MaxLength(250)]
        public string userId { get; set; }
        [MaxLength(250)]
        public DateTime logTime { get; set; }
        public int? status { get; set; }
        [MaxLength(250)]
        public string ipAddress { get; set; }
        [MaxLength(250)]
        public string browserName { get; set; }
        [MaxLength(250)]
        public string pcName { get; set; }
        [NotMapped]
        public string statusName { get; set; }


        public string mac { get; set; }
        public string ipAddress2 { get; set; }
        public string deviceName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string deviceId { get; set; }
    }
}
