using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {

            }

            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Title"] = "Profiel";
            return View();
        }

        public IActionResult RegisterPlate()
        {
            return View();
        }

        public IActionResult Reserve()
        {
            ViewData["Title"] = "Reserve a Spot";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
