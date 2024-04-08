using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;
using Microsoft.Data.SqlClient;

namespace DALL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
       public AccountRepository(GarageContext context) : base(context) { }

        public Account GetAccountByAuth0Id(string auth0Id)
        {
            try
            {
                return this._dbSet.Where(x => x.Auth0UserId == auth0Id).First();
            }
            catch (InvalidOperationException) // "Sequence contains no elements"
            {
                Console.Error.WriteLine("Table is empty.");
                return null;
            }
            catch (SqlException)
            {
                Console.Error.WriteLine("Account does not exist.");
                return null;
            }
        }
    }
}
