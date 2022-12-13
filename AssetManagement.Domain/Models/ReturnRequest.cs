using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Models
{
    public class ReturnRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public Guid AssignedBy { get; set; }

        public Guid? AcceptedBy { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public DateTime AssignedDate { get; set; }

        public Enums.ReturnRequest.State State { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(AssignmentId))]
        public virtual Assignment Assignment { get; set; }
        [ForeignKey(nameof(AssignedBy))]
        public virtual AppUser AssignedByUser { get; set; }
        [ForeignKey(nameof(AcceptedBy))]
        public virtual AppUser? AcceptedByUser { get; set; }
    }
}
