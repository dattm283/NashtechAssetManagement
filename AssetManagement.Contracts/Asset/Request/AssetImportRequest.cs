using Microsoft.AspNetCore.Http;

namespace AssetManagement.Contracts.Asset.Request
{
    public class AssetImportRequest
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}
