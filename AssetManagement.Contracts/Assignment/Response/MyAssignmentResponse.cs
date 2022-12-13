using AssetManagement.Domain.Enums.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class MyAssignmentResponse
    {
        public int Id { get; set; }

        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public string CategoryName { get; set; }

        public DateTime AssignedDate { get; set; }

        public State State { get; set; }
    }
}
