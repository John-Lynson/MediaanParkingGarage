using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IServices
{
    public interface IReservationService
    {
        public bool ReserveSpots(int carId, List<int> parkingSpotIds, DateTime expectedStartDate, DateTime expectedEndDate);
    }
}
