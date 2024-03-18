using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class TariffRepository : Repository<Tariff>, ITariffRepository
    {
        public TariffRepository(GarageContext context) : base(context) { }
    }
}
