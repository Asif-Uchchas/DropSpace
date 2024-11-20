using System.ComponentModel.DataAnnotations.Schema;

namespace DropSpace.Areas.Auth.Models
{
    public class AspNetUsersViewModel
    {
        public string aspnetId { get; set; }
        public string UserName { get; set; }
        // public string mobileNumberPersonal { get; set; }
        public string UserId { get; set; }

        public string Email { get; set; }

        public int? UserTypeId { get; set; }
        public int? companyId { get; set; }

        public string EmpCode { get; set; }

        public decimal? FinancialValue { get; set; }

        public int? isActive { get; set; }
        public int? thanaId { get; set; }
        public string thanaName { get; set; }

        public string EmpName { get; set; }
        public int EmployeeId { get; set; }

        public string DivisionName { get; set; }

        public string DesignationName { get; set; }

        public string userTypeName { get; set; }

        public string companyName { get; set; }
        public string departmentName { get; set; }
        public string empType { get; set; }
        public DateTime? joiningDate { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public int? status { get; set; }
        public int? photoId { get; set; }

        public int? projectId { get; set; }
        public int? proId { get; set; }
        public int? departmentId { get; set; }
        public string roleId { get; set; }

        public string Id { get; set; }
        public int? specialBranchUnitId { get; set; }

        public int? projId { get; set; }
        public string projectName { get; set; }
        public string imageUrl { get; set; }
        [NotMapped]
        public List<string> lstRole { get; set; }
        [NotMapped]
        public string lastRole { get; set; }
    }
}
