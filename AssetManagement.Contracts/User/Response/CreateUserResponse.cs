using AssetManagement.Domain.Enums.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.User.Response
{
    public class CreateUserResponse
    {
        public Guid Id { get; set; }

        public string StaffCode { get; set; }
        
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
