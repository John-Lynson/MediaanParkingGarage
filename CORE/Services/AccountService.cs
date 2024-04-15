using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
	public class AccountService
	{
		private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public Account GetAccountByAuth0Id(string auth0Id)
        {
			return this._accountRepository.GetAccountByAuth0Id(auth0Id);
		}

		public Account GetAccountById(int id)
		{
			return this._accountRepository.GetById(id);
		}

		public Account CreateAccount(string username, string auth0UserId, bool isAdmin = false)
        {
			this.CheckIfCerdentialsAreValid(username, auth0UserId);

			Account account = new Account
			{
				Username = username,
				Auth0UserId = auth0UserId,
				IsAdmin = isAdmin,
			};
			this._accountRepository.Create(account);
			return account;
		}

        public void UpdateAccount(Account account)
        {
            this.CheckIfCerdentialsAreValid(account.Username, account.Auth0UserId);

            this._accountRepository.Update(account);
        }

		public void DeleteAccount(Account account)
		{
			this._accountRepository.Delete(account);
		}

        private void CheckIfCerdentialsAreValid (string username, string auth0UserId)
        {
			if (username == null) // Not using string.isnullorempty() here since then the error would be "incorect" and its being checked later in the code
				throw new ArgumentNullException("Username can't be null.", nameof(username));
			if (username.Length < 3 || username.Length > 50)
				throw new ArgumentException($"Username too long or short (min = 3, max = 50). \r\nCurrent length = {username.Length}.");

			if (auth0UserId == null) // Not using string.isnullorempty() here since then the error would be "incorect" and its being checked later in the code
				throw new ArgumentNullException("Auth0 user id can't be null.", nameof(auth0UserId));
			if (auth0UserId.Length < 3 || auth0UserId.Length > 64)
				throw new ArgumentException($"Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = {auth0UserId.Length}.");
		}
	}
}