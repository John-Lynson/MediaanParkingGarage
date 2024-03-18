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
        public void CreatePlate(int accountId, string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
                throw new ArgumentException("License plate cannot be empty.", nameof(licensePlate));

            Car car = new Car
            {
                AccountId = accountId,
                LicensePlate = licensePlate,
            };

            this._carRepository.Add(car);
        }
    }
}
