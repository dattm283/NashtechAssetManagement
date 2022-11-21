using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAssets()
        {
            return Ok();
        }
    }
}
