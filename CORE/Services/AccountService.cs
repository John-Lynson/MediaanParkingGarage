﻿using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
	public class AccountService
	{
		private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
               _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public void CreateAccount(string username, string auth0UserId, bool administrator = false)
        {
            CheckIfCerdentialsAreValid(username, auth0UserId);

            Account account = new Account 
            { 
                Username = username,
                Auth0UserId = auth0UserId,
            };
            
            this._accountRepository.Create(account);
		}

        public void UpdateAccount(Account account)
        {
            CheckIfCerdentialsAreValid(account.Username, account.Auth0UserId);

            this._accountRepository.Update(account);
        }

        public Account GetAccountById(int id)
        {
            return this._accountRepository.GetById(id);
        }

        public void DeleteAccount(Account account) => this._accountRepository.Delete(account);

        private void CheckIfCerdentialsAreValid (string username, string auth0UserId)
        {
			if (username == null) // Not using string.isnullorempty() here since then the error would be incorect and its being checked later in the code
				throw new ArgumentNullException("Username can't be null.", nameof(username));
			if (username.Length < 3 || username.Length > 50)
				throw new ArgumentException($"Username too long or short (min = 3, max = 50). \r\nCurrent length = {username.Length}.");

			if (auth0UserId == null) // Not using string.isnullorempty() here since then the error would be incorect and its being checked later in the code
				throw new ArgumentNullException("Username can't be null.", nameof(username));
			if (auth0UserId.Length < 1 || auth0UserId.Length > 50)
				throw new ArgumentException($"Auth0 user Id too long or short (min = 1, max = 64). \r\nCurrent length = {username.Length}.");

		}
	}
}