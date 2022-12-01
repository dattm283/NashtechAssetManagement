using AssetManagement.Domain.Enums.AppUser;

namespace AssetManagement.Contracts.User.Response
{
    public class ViewDetailUser_UserResponse
    {
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
    }
}
