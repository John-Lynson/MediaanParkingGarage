using CORE.Entities;
using CORE.Services;
using DALL.Context;
using DALL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using WEB.Models;
using static System.Net.WebRequestMethods;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RegistrationService _registrationService;
        private readonly PaymentService _paymentService;

        public HomeController(ILogger<HomeController> logger, GarageContext context, PaymentService paymentService)
        {
            this._logger = logger;
            this._registrationService = new RegistrationService(new CarRepository(context), new AccountRepository(context));
            this._paymentService = paymentService;
        }

        public IActionResult Index()
        {

            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Home");
            }

            return View();
        }

        public IActionResult Payment()
        {
            ViewData["Title"] = "Betalingen";

            string? auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
            {
                return this.RedirectToAction("Login", "Home");
            }
            else
            {
                List<Payment> payments = this._paymentService.GetPaymentsByAuth0Id(auth0Id);
                return View(payments);
            }
        }

        public async Task<IActionResult> PaymentRedirect()
        {

			string? auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (auth0Id == null)
			{
				return this.RedirectToAction("Login", "Home");
			}
			else
			{
				await this._paymentService.CheckPaymentsStatus(auth0Id);
                return RedirectToAction("Payment", "Home");
			}
        }

        public async Task<IActionResult> CreateDemoPayment()
        {
            string? auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
            {
                return this.RedirectToAction("Login", "Home");
            }
            else
            {
                List<Car> cars = this._registrationService.GetCarsByAuth0Id(auth0Id);
                string? redirectUrl = "https://localhost:7159/Home/PaymentRedirect";
                await this._paymentService.CreatePaymentAsync(cars.First().Id, 1, DateTime.Now, redirectUrl);

                return RedirectToAction("Payment", "Home");
            }
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Title"] = "Profiel";

            string? auth0Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
            {
                return this.RedirectToAction("Login", "Home");
            }
            else
            {
                List<Car> cars = this._registrationService.GetCarsByAuth0Id(auth0Id);
                return View(cars);
            }
        }

        public IActionResult Welcome()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
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
