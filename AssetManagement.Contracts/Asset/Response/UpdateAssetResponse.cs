using AssetManagement.Domain.Enums.Asset;

namespace AssetManagement.Contracts.Asset.Response
{
    public class UpdateAssetResponse
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
