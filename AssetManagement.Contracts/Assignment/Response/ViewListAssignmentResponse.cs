using AssetManagement.Domain.Enums.Assignment;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class ViewListAssignmentResponse : IEquatable<ViewListAssignmentResponse>
    {
        public int Id { get; set; }
        public int? NoNumber { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public State State { get; set; }

        public bool Equals(ViewListAssignmentResponse? other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Id == other.Id;
        }
    }
}
