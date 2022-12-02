using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Enums.Asset
{
    public enum State
    {
        Available,
        NotAvailable,
        WaitingForRecycling,
        Recycled,
        Assigned
    }
}
