using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Common
{
    public class ErrorResponseResult<T>: DefaultResponseResult<T>
    {
        public ErrorResponseResult()
        {
        }

        public ErrorResponseResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
