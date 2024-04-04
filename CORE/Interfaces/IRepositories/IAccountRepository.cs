using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IRepositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Account GetAccountByAuth0Id(string auth0Id);
    }
}
