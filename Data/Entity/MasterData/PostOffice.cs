using System.ComponentModel.DataAnnotations;

namespace DropSpace.Data.Entity.MasterData
{
    public class PostOffice : Base
    {
        public int thanaId { get; set; }
        public Thana? thana { get; set; }

        [MaxLength(10)]
        public string? postalCode { get; set; }
        [MaxLength(100)]
        public string? postalName { get; set; }
        [MaxLength(20)]
        public string? postalShortName { get; set; }
        [MaxLength(100)]
        public string? postalNameBn { get; set; }

        
    }
}
