using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Domain.Enums.Asset;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Contracts.Asset.Request
{
    public class UpdateAssetRequest
    {
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        public string Specification { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public DateTime InstalledDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int State { get; set; }
    }
}