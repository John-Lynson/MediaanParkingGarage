using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISpotOccupationRepository _spotOccupationRepository;
        private const int RatePerHour = 300; // (€3/h)
        public PaymentService(IPaymentRepository paymentRepository, ISpotOccupationRepository spotOccupationRepository)
        {
            _paymentRepository = paymentRepository;
            _spotOccupationRepository = spotOccupationRepository;
        }

        public Payment ProcessPayment(int carId, int garageId, DateTime date)
        {
            // Get latest SpotOccupation record for this car
            var spotOccupation = _spotOccupationRepository.GetLatestByCarId(carId);

            // Calculate total parking time in hours
            var parkingDuration = spotOccupation.ActualEndDate.Subtract(spotOccupation.ActualStartDate).TotalHours;

            // Calculate total fee in cents
            var totalFee = (int)(parkingDuration * RatePerHour);

            var payment = new Payment
            {
                CarId = carId,
                GarageId = garageId,
                Price = totalFee,
                Type = Enums.PaymentType.Standard,
                Date = date 
            };

            _paymentRepository.Create(payment);

            return payment;
        }
    }
}
