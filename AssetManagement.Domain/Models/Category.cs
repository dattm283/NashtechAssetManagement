using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Domain.Models
{
    #nullable disable
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Category Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Category Prefix")]
        public string Prefix { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
