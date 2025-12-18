using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace persian_code.web.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/About")]
        public IActionResult About()
        {
            return View();
        }

    }
}
