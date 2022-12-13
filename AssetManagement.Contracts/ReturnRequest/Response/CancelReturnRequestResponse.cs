using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.ReturnRequest.Response
{
    public class CancelReturnRequestResponse
    {
        public int Id { get; set; }

        public string RequestedBy { get; set; }

        public string AcceptedBy { get; set; }

        public int AssetId { get; set; }

        public DateTime AssignedDate { get; set; }
    }
}
