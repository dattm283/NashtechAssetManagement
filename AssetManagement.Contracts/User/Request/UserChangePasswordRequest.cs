using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.User.Request
{
    public class UserChangePasswordRequest
    {
        [Required(ErrorMessage = "Please enter current password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
