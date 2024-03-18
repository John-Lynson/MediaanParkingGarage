using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
       public AccountRepository(GarageContext context) : base(context) { }
    }
}
