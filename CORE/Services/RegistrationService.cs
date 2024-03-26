using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Interfaces.IRepositories;

namespace CORE.Services
{
    public class RegistrationService
    {
        private readonly ICarRepository _carRepository;

        public RegistrationService(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }
        public void CreateCar(int accountId, string licensePlate)
        {
            if (accountId <= 0)
                throw new ArgumentException("Account ID cannot be null.", nameof(accountId));

            if (string.IsNullOrEmpty(licensePlate))
                throw new ArgumentException("License plate cannot be empty.", nameof(licensePlate));

            Car car = new Car
            {
                AccountId = accountId,
                LicensePlate = licensePlate,
            };

            this._carRepository.Create(car);
        }

        //mogelijk de twee methods combineren als er geen specifieke validatie nodig is
        public void AddAdditionalLicensePlate(int accountId, string newLicensePlate)
        {
            if (accountId <= 0)
                throw new ArgumentException("Account ID must be positive and non-zero.", nameof(accountId));

            if (string.IsNullOrEmpty(newLicensePlate))
                throw new ArgumentException("New license plate cannot be empty.", nameof(newLicensePlate));

            var existingCar = this._carRepository.FindByAccountIdAndLicensePlate(accountId, newLicensePlate);
            if (existingCar != null)
            {
                throw new InvalidOperationException("This license plate is already registered for this account.");
            }

            Car newCar = new Car
            {
                AccountId = accountId,
                LicensePlate = newLicensePlate,
            };

            this._carRepository.Create(newCar);
        }

    }
}
