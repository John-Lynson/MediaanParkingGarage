using CORE.Entities;
using CORE.Services;
using DALL.Context;
using DALL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RegistrationService _registrationService;

        public HomeController(ILogger<HomeController> logger, GarageContext context)
        {
            this._logger = logger;
            this._registrationService = new RegistrationService(new CarRepository(context), new AccountRepository(context));
        }

        public IActionResult Index()
        {
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

            string? auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<Car> cars = this._registrationService.GetCarsByAuth0Id(auth0Id);
                return View(cars);
            }
        }

        public IActionResult RegisterPlate()
        {
            return View();
        }

        public IActionResult Reserve()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
