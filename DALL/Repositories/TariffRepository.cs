using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class TariffRepository : Repository<Tariff>, ITariffRepository
    {
        public TariffRepository(GarageContext context) : base(context) { }

        public List<Tariff> GetTariffsByInterval(DateTime startDate, DateTime endDate)
        {
            return this._dbSet.Where<Tariff>(t =>
                t.StartDate > startDate && t.EndDate < endDate      // All tariffs between start & end
                || t.StartDate < startDate && t.EndDate > startDate // The tariff that the startDate lands on.
                || t.StartDate < endDate && t.EndDate > endDate     // The tariff that the endDate lands on.
                ).ToList();
        }
    }
}
