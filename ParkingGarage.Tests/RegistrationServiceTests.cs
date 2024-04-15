using CORE.Entities;
using CORE.Interfaces;
using CORE.Interfaces.IRepositories;
using CORE.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingGarage.Tests
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private RegistrationService _registrationService;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ICarRepository> _carRepository;

        [TestInitialize]
        public void SetUp()
        {
            this._accountRepository = new Mock<IAccountRepository>();
            this._carRepository = new Mock<ICarRepository>();
            this._registrationService = new RegistrationService(this._carRepository.Object, this._accountRepository.Object);
        }

        [TestMethod]
        public void CreateCar_NewCar_ShouldCreateSuccessfully()
        {
            // Arrange
            var accountId = 1;
            var licensePlate = "ABC123";

			// Act
            this._registrationService.CreateCar(accountId, licensePlate);

            // Assert
            this._carRepository.Verify(repo => repo.Create(It.IsAny<Car>()), Times.Once);
        }

        [TestMethod]
        public void GetCarsByAuth0Id_ReturnsCars()
        {
            // Arrange
            string auth0Id = "testAuth0Id";
            int accountId = 1;
            Account account = new Account { Auth0UserId = auth0Id, Id = accountId };

            List<Car> cars = new List<Car>
            {
                new Car(), new Car(), new Car()
            };

            this._accountRepository.Setup(repo => repo.GetAccountByAuth0Id(auth0Id)).Returns(account);
            this._carRepository.Setup(repo => repo.GetAllByAccountId(accountId)).Returns(cars);

            // Act
            List<Car> result = this._registrationService.GetCarsByAuth0Id(auth0Id);

            // Assert
            Assert.AreEqual(result, cars);
        }

    }
}
