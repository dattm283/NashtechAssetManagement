namespace AssetManagement.Contracts.User.Response
{
    public class ViewListUser_UserResponse
    {
        public string Id { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Type { get; set; }
        public int ValidAssignments { get; set; }
    }
}
