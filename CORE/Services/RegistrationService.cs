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
		private readonly IAccountRepository _accountRepository;

        private readonly AccountService _accountService;

		public RegistrationService(ICarRepository carRepository, IAccountRepository accountRepository)
        {
            this._carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            this._accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

            this._accountService = new AccountService(accountRepository);
        }

        public void CreateCar(int accountId, string licensePlate)
        {
            if (accountId <= 0)
                throw new ArgumentException("Account ID cannot be zero or less.", nameof(accountId));

            if (string.IsNullOrEmpty(licensePlate))
                throw new ArgumentException("License plate cannot be empty.", nameof(licensePlate));

            Car car = new Car
            {
                AccountId = accountId,
                LicensePlate = licensePlate,
            };

            this._carRepository.Create(car);
        }

        public List<Car> GetCarsByAuth0Id(string auth0Id)
        {
            Account account = this._accountService.GetAccountByAuth0Id(auth0Id) ?? throw new ArgumentNullException("Account does not exist.");
            return this._carRepository.GetAllByAccountId(account.Id);
        }
    }
}
