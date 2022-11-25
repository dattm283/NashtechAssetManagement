using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Tests.TestHelper
{
    public static class ConverterFromIActionResult
    {
        public static string ConvertOkObject<Dest>(IActionResult resultAction)
        {
            var resultOkObject = resultAction as OkObjectResult;
            var result = JsonConvert
                .SerializeObject((Dest)resultOkObject.Value);

            return result;
        }

        public static string ConvertStatusCode(IActionResult resultAction)
        {
            var resultStatusCode = resultAction as ObjectResult;
            var result = JsonConvert
                .SerializeObject(resultStatusCode.Value);

            return result;
        }
    }
}
