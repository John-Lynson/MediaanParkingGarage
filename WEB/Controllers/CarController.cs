using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using CORE.Services;
using CORE.Entities;
using System.Security.Claims;
using DALL.Context;
using DALL.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MediaanParkingGarage.Controllers
{
    public class CarController : Controller
    {
        private readonly RegistrationService _registrationService;
        private readonly AccountService _accountService;

        public CarController(GarageContext context)
        {
            this._registrationService = new RegistrationService(new CarRepository(context));
            this._accountService = new AccountService(new AccountRepository(context));
        }

        [HttpPost]
        public IActionResult CreateCar(string licensePlate)
        {
            // Get account for it's id via the auth0Id
            string? auth0Id = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Account account = this._accountService.GetAccountByAuth0Id(auth0Id);

            // Register & Link car to the account
            try
            {
                this._registrationService.CreateCar(account.Id, licensePlate);
                this.TempData["StatusMessage"] = "Succesfully registered license plate.";
            }
            catch (DbUpdateException)
            {
                this.TempData["StatusMessage"] = "License plate registration failed. Possibly already exists and is linked to an account.";
            }
            
            return Redirect("/Home/RegisterPlate");
        }
    }
}
