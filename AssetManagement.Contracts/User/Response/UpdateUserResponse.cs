#nullable disable
namespace AssetManagement.Contracts.User.Response
{
    public class UpdateUserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string StaffCode { get; set; }
        public Domain.Enums.AppUser.UserGender Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
    }
}
