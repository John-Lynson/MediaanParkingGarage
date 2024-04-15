using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class ParkingSpotRepository : Repository<ParkingSpot>, IParkingSpotRepository
    {
        public ParkingSpotRepository(GarageContext context) : base(context) { }

        public List<int> GetAllParkingSpotIds()
        {
            return this._dbSet.Select(p => p.Id).ToList();
        }
    }
}
