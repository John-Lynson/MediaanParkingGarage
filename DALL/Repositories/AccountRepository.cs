using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
       public AccountRepository(GarageContext context) : base(context) { }
    }
}
