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
		public void CreateAccount_Success()
		{
			// Arrange
			string username = "someName";
			string auth0id = "auth69id";

			// Act
			Account result = this._accountService.CreateAccount(username, auth0id);

			// Assert
			this._accountRepo.Verify(repo => repo.Create(result), Times.Once());
			Assert.AreEqual(username, result.Username);
			Assert.AreEqual(auth0id, result.Auth0UserId);
		}

		[DataTestMethod]
		[DataRow(null, "test|1", "username (Parameter 'Username can't be null.')")]
		[DataRow("22", "test|1", "Username too long or short (min = 3, max = 50). \r\nCurrent length = 2.")]
		[DataRow("123456789012345678901234567890123456789012345678901234567890", "test|1", "Username too long or short (min = 3, max = 50). \r\nCurrent length = 60.")]
		[DataRow("username", null, "auth0UserId (Parameter 'Auth0 user id can't be null.')")]
		[DataRow("username", "1", "Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = 1.")]
		[DataRow("username", "1234567890123456789012345678901234567890123456789012345678901234567890", "Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = 70.")]
		public void CreateAccount_Failure(string username, string auth0UserId, string expectedErrorMessage)
		{
			// Arrange
			Account account = new Account()
			{
				Username = username,
				Auth0UserId = auth0UserId,
			};
			this._accountRepo.Setup(repo => repo.Create(account)).Throws(new Exception(expectedErrorMessage));

			// Act & Assert
			try
			{
				this._accountService.CreateAccount(username, auth0UserId);
				Assert.Fail();
			}
			catch (Exception e)
			{
				Assert.AreEqual(expectedErrorMessage, e.Message);
			}
		}

		[TestMethod]
		public void UpdateAccount_Success()
		{
			// Arrange
			Account account = new Account
			{
				Username = "username",
				Auth0UserId = "google-id",
			};

			// Act
			this._accountService.UpdateAccount(account);

			// Assert
			this._accountRepo.Verify(repo => repo.Update(account), Times.Once);
		}

		[DataTestMethod]
		[DataRow(null, "test|1", "username (Parameter 'Username can't be null.')")]
		[DataRow("22", "test|1", "Username too long or short (min = 3, max = 50). \r\nCurrent length = 2.")]
		[DataRow("123456789012345678901234567890123456789012345678901234567890", "test|1", "Username too long or short (min = 3, max = 50). \r\nCurrent length = 60.")]
		[DataRow("username", null, "auth0UserId (Parameter 'Auth0 user id can't be null.')")]
		[DataRow("username", "1", "Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = 1.")]
		[DataRow("username", "1234567890123456789012345678901234567890123456789012345678901234567890", "Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = 70.")]
		public void UpdateAccount_Failure(string username, string auth0UserId, string expectedErrorMessage)
		{
			// Arrange
			Account account = new Account
			{
				Id = 1,
				Auth0UserId = auth0UserId,
				Username = username
			};
			this._accountRepo.Setup(repo => repo.Update(account)).Throws(new Exception(expectedErrorMessage));

			// Act & Assert
			try
			{
				this._accountService.UpdateAccount(account);
				Assert.Fail();
			}
			catch (Exception e)
			{
				Assert.AreEqual(expectedErrorMessage, e.Message);
			}
		}

		[TestMethod]
		public void DeleteAccount_Success()
		{
			// Arrange
			Account account = new Account();

			// Act
			this._accountService.DeleteAccount(account);

			// Assert
			this._accountRepo.Verify(repo => repo.Delete(It.IsAny<Account>()), Times.Once);
		}

		[TestMethod]
		public void GetAccountById_success()
		{
			// Arrange
			int accountId = 1;
			string username = "ok";
			string auth0UserId = "test";
			Account account = new Account { 
				Id = accountId, Username = username, Auth0UserId = auth0UserId 
			};
			this._accountRepo.Setup(repo => repo.GetById(accountId)).Returns(account);

			// Act
			Account result = this._accountService.GetAccountById(1);

			// Assert
			Assert.AreEqual(accountId, result.Id);
			Assert.AreEqual(username, result.Username);
			Assert.AreEqual(auth0UserId, result.Auth0UserId);
			this._accountRepo.Verify(repo => repo.GetById(accountId), Times.Once);
		}

		[TestMethod]
		public void GetAccountByAuth0Id_ReturnsAccount()
		{
			// Arrange
			int accountId = 1;
			string username = "ok";
			string auth0UserId = "test";
			Account account = new Account
			{
				Id = accountId,
				Username = username,
				Auth0UserId = auth0UserId
			};
			this._accountRepo.Setup(repo => repo.GetAccountByAuth0Id("123")).Returns(account);

			// Act
			Account result = this._accountService.GetAccountByAuth0Id("123");

			// Assert
			Assert.AreEqual(accountId, result.Id);
			Assert.AreEqual(username, result.Username);
			Assert.AreEqual(auth0UserId, result.Auth0UserId);
		}
	}
}
