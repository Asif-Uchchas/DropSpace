namespace DropSpace.Areas.Auth.Models
{
    public class UserInformationModel
    {
        public string? userId { get; set; }
        public string? roleId { get; set; }
        public string? bpNo { get; set; }
        public string? empName { get; set; }
        public string? empNameBn { get; set; }
        public string? unitName { get; set; }
        public string? email { get; set; }
        public string? designationName { get; set; }
        public string? mobile { get; set; }
        public string? imgUrl { get; set; }
        public string? password { get; set; }
        public string? nid { get; set; }
        public int? rankId { get; set; }
        public int? bcsBatchId { get; set; }
        public int? branchId { get; set; }
        public int? sectionId { get; set; }
        public IFormFile? formFile { get; set; }
    }
}
