using CORE.Entities;
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

        public AccountService(IAccountRepository accountRepo)
        { 
            this._accountRepository = accountRepo;
        }

        public Account GetAccountByAuth0Id(string auth0Id)
        {
            return this._accountRepository.GetAccountByAuth0Id(auth0Id);
        }

        public Account Create(string username, string auth0Id, string password)
        {
            Account newAccount = new Account
            {
                Username = username,
                Auth0UserId = auth0Id,
                Password = password,
                IsAdmin = false,
            };
            this._accountRepository.Create(newAccount);
            return newAccount;
        }
    }
}
