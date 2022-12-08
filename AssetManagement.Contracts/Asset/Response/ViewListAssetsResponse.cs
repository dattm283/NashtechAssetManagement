using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Asset.Response
{
    public class ViewListAssetsResponse : IEquatable<ViewListAssetsResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssetCode { get; set; }
        public string CategoryName { get; set; }
        public string State { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public string Location { get; set; }
        public Boolean IsEditable { get; set; }

        public bool Equals(ViewListAssetsResponse? other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Id == other.Id;
        }
    }
}
