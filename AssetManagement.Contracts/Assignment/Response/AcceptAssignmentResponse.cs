using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Assignment.Response
{
    public class AcceptAssignmentResponse
    {
        public int Id { get; set; }
        public AssetManagement.Domain.Enums.Assignment.State State { get; set; }
        public int? AssetId { get; set; }
        public AssetManagement.Domain.Enums.Asset.State? AssetState { get; set; }

    }
}
