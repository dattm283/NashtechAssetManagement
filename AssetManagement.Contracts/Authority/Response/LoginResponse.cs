using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Authority.Response
{
    public class LoginSuccessResponse
    {
        public string Token { get; set; }

        public string Role { get; set; }
    }
}
