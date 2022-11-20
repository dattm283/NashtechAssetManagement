using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Common
{
    public class SuccessResponseResult<T> : DefaultResponseResult<T>
    {
        public SuccessResponseResult(T result)
        {
            IsSuccessed = true;
            Result = result;
        }

        public SuccessResponseResult()
        {
            IsSuccessed = true;
        }
    }
}
