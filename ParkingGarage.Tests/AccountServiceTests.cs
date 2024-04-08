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
		[TestMethod]
		public void CreateAccount_Success()
		{
			// Arrange
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			string username = "Username";
			string auth0UserId = "hydar|123456789";

			// Act
			service.CreateAccount(username, auth0UserId);

			// Assert
			mockRepository.Verify(repo => repo.Create(It.IsAny<Account>()), Times.Once);
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
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			Exception exception = null;

			// Act
			try
			{
				service.CreateAccount(username, auth0UserId);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			// Assert
			Assert.AreEqual(expectedErrorMessage, exception.Message);
		}

		[TestMethod]
		public void UpdateAccount_Success()
		{
			// Arrange
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			Account account = new Account
			{
				Auth0UserId = "test|1",
				Username = "wooot"
			};

			// Act
			service.UpdateAccount(account);

			// Assert
			mockRepository.Verify(repo => repo.Update(It.IsAny<Account>()), Times.Once);
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
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			Exception exception = null;
			Account account = new Account
			{
				Id = 1,
				Auth0UserId = auth0UserId,
				Username = username
			};

			// Act
			try
			{
				service.UpdateAccount(account);
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			// Assert
			Assert.AreEqual(expectedErrorMessage, exception.Message);
		}

		[TestMethod]
		public void DeleteAccount_Success()
		{
			// Arrange
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			Account account = new Account
			{
				Id = 1,
				Username = "hydar",
				Auth0UserId = "test|01"
			};

			// Act
			service.DeleteAccount(account);

			// Assert
			mockRepository.Verify(repo => repo.Delete(It.IsAny<Account>()), Times.Once);
		}

		[TestMethod]
		public void GetAccountById_success()
		{
			// Arrange
			Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>();
			AccountService service = new AccountService(mockRepository.Object);
			int id = 1;

			// Act
			Account account = service.GetAccountById(1);

			// Assert
			mockRepository.Verify(repo => repo.GetById(id), Times.Once);
		}
	}
}
