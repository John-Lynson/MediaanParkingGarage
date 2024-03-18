using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(GarageContext context) : base(context) { }
    }
}
