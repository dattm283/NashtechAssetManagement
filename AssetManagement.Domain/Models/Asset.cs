using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Models
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string AssetCode { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }

        [MaxLength(50)]
        public AssetManagement.Domain.Enums.AppUser.AppUserLocation Location { get; set; }
        public AssetManagement.Domain.Enums.Asset.State State { get; set; }
        public Boolean IsDeleted { get; set; }

        //public virtual List<Assignment> Assignments { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
