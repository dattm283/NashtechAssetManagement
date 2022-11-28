namespace AssetManagement.Contracts.Asset.Response
{
    public class CreateAssetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssetCode { get; set; }
        public string CategoryName { get; set; }
        public string State { get; set; }
    }
}
