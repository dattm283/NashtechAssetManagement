using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class AssignmentDetailResponse
    {
        public string AssetCode { get; set; }

        public string AssetName { get; set; }   

        public string Specification { get; set; }

        public Guid? AssignedTo { get; set; }

        public string AssignToAppUser { get; set; }

        public Guid? AssignedBy { get; set; }

        public string AssignByAppUser { get; set; }

        public DateTime AssignedDate { get; set; }

        public AssetManagement.Domain.Enums.Assignment.State State { get; set; }

        public string StateName { get; set; }

        public string Note { get; set; }
    }
}
