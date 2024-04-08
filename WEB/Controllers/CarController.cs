using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using CORE.Services;
using CORE.Entities;

namespace MediaanParkingGarage.Controllers
{
    public class CarController : Controller
    {
        private readonly RegistrationService _registrationService;

        public CarController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] CarRequestModel model)
        {
            try
            {
                _registrationService.CreateCar(model.AccountId, model.LicensePlate);
                return Ok("Car created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
