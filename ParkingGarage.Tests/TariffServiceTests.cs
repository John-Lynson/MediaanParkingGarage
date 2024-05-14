using CORE.Entities;
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
    public class TariffServiceTests
    {
        private TariffService _tariffService;
        private Mock<ITariffRepository> _tariffRepository;

        [TestInitialize]
        public void SetUp()
        {
            this._tariffRepository = new Mock<ITariffRepository>();
            this._tariffService = new TariffService(this._tariffRepository.Object);
        }

        [TestMethod]
        public void CreateTariff_Success()
        {
            // Arrange
            // n/a

            // Act 
            this._tariffService.CreateTariff(1, 1, DateTime.MinValue, DateTime.MaxValue);

            // Assert
            this._tariffRepository.Verify(r => r.Create(It.IsAny<Tariff>()), Times.Once);

        }

        [TestMethod]
        public void GetTariffsByInterval_ReturnsTariffs()
        {
            // Arrange
            List<Tariff> list = new List<Tariff> { new Tariff(), new Tariff(), new Tariff() };
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            this._tariffRepository.Setup(r => r.GetTariffsByInterval(startDate, endDate)).Returns(list);

            // Assert
            List<Tariff> result = this._tariffService.GetTariffsByInterval(startDate, endDate);

            // Assert
            Assert.AreEqual(list, result);
        }
    }
}
