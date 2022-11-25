using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Contracts.Asset.Request
{
    public class CreateAssetRequest
    {
        [Required(ErrorMessage = "This field is required")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        public string Specification { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime InstalledDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int State { get; set; }
    }
}
