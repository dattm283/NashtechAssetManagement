using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AssetManagement.Domain.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public string StaffCode { get; set; }

        [MaxLength(50)]
        public AssetManagement.Domain.Enums.AppUser.UserGender Gender { get; set; }

        [MaxLength(50)]
        public AssetManagement.Domain.Enums.AppUser.AppUserLocation Location { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsLoginFirstTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        [InverseProperty(nameof(Assignment.AssignedToAppUser))]
        public virtual ICollection<Assignment> AssignedToAssignments { get; set; }

        [InverseProperty(nameof(Assignment.AssignedByAppUser))]
        public virtual ICollection<Assignment> AssignedByAssignments { get; set; }

    }
}
