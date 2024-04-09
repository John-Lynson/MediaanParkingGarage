using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class SpotOccupationRepository : Repository<SpotOccupation>, ISpotOccupationRepository
    {
        public SpotOccupationRepository(GarageContext context) : base(context) { }

        public List<int> GetAvailableSpaces(List<int> parkingSpotIds, DateTime expectedStartDate, DateTime expectedEndDate)
        {
            var occupiedSpots = this._dbSet
                .Where(so => parkingSpotIds.Contains(so.ParkingSpotId)
                        && ((so.ExpectedStartDate < expectedEndDate && so.ExpectedEndDate > expectedStartDate)
                        || (so.ActualStartDate < expectedEndDate && so.ActualEndDate > expectedStartDate)))
                .Select(so => so.ParkingSpotId)
                .Distinct()
                .ToList();

            var availableSpots = parkingSpotIds.Except(occupiedSpots).ToList();
            return availableSpots;

        }

        public SpotOccupation FindByCarIdAndDate(int carId, DateTime startDate, DateTime endDate)
        {
            // Query the DbSet for SpotOccupation entities
            return this._dbSet
                       .Where(so => so.CarId == carId
                                    && ((so.ExpectedStartDate >= startDate && so.ExpectedStartDate <= endDate)
                                    || (so.ActualStartDate >= startDate && so.ActualStartDate <= endDate)))
                       .FirstOrDefault();
        }
    }
}
