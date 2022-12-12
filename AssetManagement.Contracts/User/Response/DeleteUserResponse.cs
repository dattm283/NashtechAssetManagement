using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.User.Response
{
    public class DeleteUserResponse
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
    }
}
