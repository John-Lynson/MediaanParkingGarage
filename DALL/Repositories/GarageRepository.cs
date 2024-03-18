using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class GarageRepository : Repository<Garage>, IGarageRepository
    {
        public GarageRepository(GarageContext context) : base(context) { }
    }
}
