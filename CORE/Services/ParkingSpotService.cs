using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
	public class ParkingSpotService
	{
		readonly IParkingSpotRepository _parkingSpotRepository;
		public ParkingSpotService(IParkingSpotRepository parkingSpotRepository)
		{
			_parkingSpotRepository = parkingSpotRepository;
		}

		public List<ParkingSpot> GetParkingSpotByGarageId(int garageId)
		{
			return _parkingSpotRepository.GetParkingSpotsByGarageId(garageId);
		}
	}
}
