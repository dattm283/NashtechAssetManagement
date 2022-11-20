using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contracts.Common
{
    public class DefaultResponseResult<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }
}
