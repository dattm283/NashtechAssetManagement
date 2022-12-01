using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Request
{
    public class UpdateAssignmentRequest
    {
        public string AssignToAppUserStaffCode { get; set; }

        public string AssetCode { get; set; }

        public DateTime AssignedDate { get; set; }

        public string Note { get; set; }
    }
}
