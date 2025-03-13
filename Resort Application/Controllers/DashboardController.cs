using Microsoft.AspNetCore.Mvc;

namespace Resort_Application.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
