using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DropSpace.Data.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public int? isActive { get; set; }

        public DateTime? createdAt { get; set; }
        [MaxLength(120)]
        public string? createdBy { get; set; }

        [MaxLength(20)]
        public string? bpNo { get; set; }
        [MaxLength(10)]
        public string? otpCode { get; set; }
    }
}
