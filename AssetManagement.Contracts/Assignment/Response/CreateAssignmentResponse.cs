using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Domain.Enums.Assignment;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class CreateAssignmentResponse
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public State State { get; set; }
    }
}
