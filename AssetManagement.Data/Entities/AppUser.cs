using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        [MaxLength(50)]
        public string Gender { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsLoginFirstTime { get; set; }

        [ForeignKey("AppRole")]
        public Guid RoleId { get; set; }
        public AppRole AppRole { get; set; }
    }
}
