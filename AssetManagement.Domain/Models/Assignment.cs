using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public AssetManagement.Domain.Enums.Assignment.State State { get; set; }

        public int? AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [ForeignKey(nameof(AssignedToAppUser))]
        public Guid? AssignedTo { get; set; }

        [ForeignKey(nameof(AssignedByAppUser))]
        public Guid? AssignedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Note { get; set; }

        public virtual AppUser AssignedToAppUser { get; set; }
        public virtual AppUser AssignedByAppUser { get; set; }
    }
}
