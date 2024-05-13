using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
	public class ReservationService
	{
		private readonly ISpotOccupationRepository _spotOccupationRepository;
		private readonly IParkingSpotRepository _parkingSpotRepository;


		public ReservationService(ISpotOccupationRepository spotOccupationRepository, IParkingSpotRepository parkingSpotRepository)
		{
			_spotOccupationRepository = spotOccupationRepository;
			_parkingSpotRepository = parkingSpotRepository;
		}

		public int RetrieveOrReserveSpot(int carId, DateTime expectedStartDate, DateTime expectedEndDate)
		{
			// Check for existing reservation
			var existingReservation = _spotOccupationRepository.FindByCarIdAndDate(carId, expectedStartDate, expectedEndDate);
			if (existingReservation != null)
			{
				return existingReservation.ParkingSpotId; // Return the ID of the already reserved spot
			}

			// Attempt to find an available spot
			var allSpots = _parkingSpotRepository.GetAllParkingSpotIds();
			var availableSpots = _spotOccupationRepository.GetAvailableSpaces(allSpots, expectedStartDate, expectedEndDate);

			if (!availableSpots.Any())
			{
				throw new InvalidOperationException("No available spots.");
			}

			var spotId = availableSpots.First();
			AttemptToReserveSpot(carId, new List<int> { spotId }, expectedStartDate, expectedEndDate);

			return spotId;
		}

		public void AttemptToReserveSpotAtGarage(int carId, int garageId, DateTime expectedStartDate, DateTime expectedEndDate)
		{
			AttemptToReserveSpot(carId, new ParkingSpotService(_parkingSpotRepository).GetParkingSpotByGarageId(garageId).Select(model => model.Id).ToList(), expectedStartDate, expectedEndDate);
		}

		public void AttemptToReserveSpot(int carId, int parkingSpotId, DateTime expectedStartDate, DateTime expectedEndDate) => AttemptToReserveSpot(carId, new List<int> { parkingSpotId }, expectedStartDate, expectedEndDate);

		public void AttemptToReserveSpot(int carId, List<int> parkingSpotIds, DateTime expectedStartDate, DateTime expectedEndDate)
		{
			var availableSpots = _spotOccupationRepository.GetAvailableSpaces(parkingSpotIds, expectedStartDate, expectedEndDate);
			var unavailableSpots = parkingSpotIds.Except(availableSpots).ToList();

			if (unavailableSpots.Any())
			{
				// Throw an exception listing the unavailable spots
				throw new InvalidOperationException($"The following spots are unavailable: {string.Join(", ", unavailableSpots)}.");
			}

			if (!availableSpots.Any())
			{
				throw new InvalidOperationException($"None of the provided spots have the ability to be used. \r\n Passed spotIds: {string.Join(",", parkingSpotIds)}");
			}
			
			var spotOccupation = new SpotOccupation
			{
				ParkingSpotId = availableSpots.First(),
				CarId = carId,
				ExpectedStartDate = expectedStartDate,
				ExpectedEndDate = expectedEndDate,
				ActualStartDate = DateTime.MinValue, // Placeholder, to be updated when the spot is actually occupied.
				ActualEndDate = DateTime.MinValue, // Placeholder, to be updated when the spot is vacated.
			};

			_spotOccupationRepository.Create(spotOccupation);
		}
	}
}
