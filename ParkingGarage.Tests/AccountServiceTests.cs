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
    public class AccountServiceTests
    {

        private AccountService _accountService;
        private Mock<IAccountRepository> _accountRepo;

        [TestInitialize]
        public void SetUp()
        {
            this._accountRepo = new Mock<IAccountRepository>();
            this._accountService = new AccountService(this._accountRepo.Object);
        }

        [TestMethod]
        public void GetAccountByAuth0Id_ReturnsAccount()
        {
            // Arrange
            Account account = new Account();
            this._accountRepo.Setup(repo => repo.GetAccountByAuth0Id("123")).Returns(account);

            // Act
            Account result = this._accountService.GetAccountByAuth0Id("123");

            // Assert
            Assert.AreEqual(account, result);
        }

        [TestMethod] 
        public void Create_ReturnsAccount()
        {
            // Arrange
            string username = "someName";
            string auth0id = "auth69id";
            string password = "password";

            // Act
            Account result = this._accountService.Create(username, auth0id, password);

            // Assert
            this._accountRepo.Verify(repo => repo.Create(result), Times.Once());
            Assert.AreEqual(username, result.Username);
            Assert.AreEqual(auth0id, result.Auth0UserId);
            Assert.AreEqual(password, result.Password);
        }
    }
}
