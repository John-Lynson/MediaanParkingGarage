using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Interfaces.IRepositories;
using CORE.Interfaces.IServices;

namespace CORE.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ICarRepository _carRepository;

        public RegistrationService(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }
        public void Create(int accountId, string licensePlate)
        {
            if (Car. == null)
                throw new ArgumentNullException(nameof(car.LicensePlate));

            if (string.IsNullOrEmpty(license.LicensePlateNumber))
                throw new ArgumentException("License plate cannot be empty.", nameof(car.LicensePlate));

            // Check if the license plate is already registered
            var existingLicense = _carRepository.GetLicensePlate(license.LicensePlateNumber);
            if (existingLicense != null)
            {
                // Update existing record, or throw an exception, depending on your requirements
                throw new InvalidOperationException("License plate already registered.");
            }
            else
            {
                // Set entry time for new registration
                license.EntryTime = DateTime.Now;
                _carRepository.AddLicensePlate(license);
            }

            Car car = new Car
            {
                AccountId
                LicensePlate
            };

            this._carRepository.Add(car);
        }
    }
}
