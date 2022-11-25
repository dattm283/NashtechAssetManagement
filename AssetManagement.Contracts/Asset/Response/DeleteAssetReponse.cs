using AssetManagement.Domain.Enums.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Asset.Response
{
    public class DeleteAssetReponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AssetCode { get; set; }

        public string Specification { get; set; }

        public DateTime InstalledDate { get; set; }

        public State State { get; set; }

        public Boolean IsDeleted { get; set; }
    }
}
