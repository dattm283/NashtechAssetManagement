using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Request
{
    public class UpdateAssignmentRequest
    {
        public Guid? AssignedTo { get; set; }

        public int? AssetId { get; set; }

        public DateTime AssignedDate { get; set; }

        public string Note { get; set; }
    }
}
