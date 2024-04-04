using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
       public AccountRepository(GarageContext context) : base(context) { }

        public Account GetAccountByAuth0Id(string auth0Id)
        {
            return this._dbSet.Where(x => x.Auth0UserId == auth0Id).First();
        }
    }
}
