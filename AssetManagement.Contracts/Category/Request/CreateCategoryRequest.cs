using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Contracts.Category.Request
{
    #nullable disable
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Please enter Category Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Category Prefix")]
        public string Prefix { get; set; }
    }
}
