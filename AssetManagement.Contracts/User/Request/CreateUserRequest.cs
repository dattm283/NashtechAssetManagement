using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.User.Request
{
    public class CreateUserRequest
    {
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters!")]
        [Required(ErrorMessage = "Please enter first name:")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters!")]
        [Required(ErrorMessage = "Please enter last name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter date of birth:")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Please enter joined date:")]
        public DateTime JoinedDate { get; set; }

        [Required(ErrorMessage = "Please select gender:")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select role:")]
        public string Role { get; set; }
    }
}
