using AssetManagement.Domain.Enums.ReturnRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.ReturnRequest.Response
{
    public class ViewListReturnRequestResponse: IEquatable<ViewListReturnRequestResponse>
    {
        public int Id { get; set; }

        public int? NoNumber { get; set; }

        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public string RequestedBy { get; set; }

        public DateTime AssignedDate { get; set; }
        
        public string AcceptedBy { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public State State { get; set; }

        public bool Equals(ViewListReturnRequestResponse? other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Id == other.Id;
        }
    }
}
