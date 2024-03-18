using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class SpotOccupationRepository : Repository<SpotOccupation>, ISpotOccupationRepository
    {
        public SpotOccupationRepository(GarageContext context) : base(context) { }

        public SpotOccupation GetLatestByCarId(int carId)
        {
            return this._dbSet
                .Where(so => so.CarId == carId)
                .OrderByDescending(so => so.ActualStartDate)
                .FirstOrDefault();

        }
    }
}
