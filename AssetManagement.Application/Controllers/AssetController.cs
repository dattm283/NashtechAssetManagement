using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Application.Controllers
{
    public class AssetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
