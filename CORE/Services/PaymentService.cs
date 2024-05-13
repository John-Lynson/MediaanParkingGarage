using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace CORE.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISpotOccupationRepository _spotOccupationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICarRepository _carRepository;
        private readonly PaymentClient _molliePaymentClient;
        private const int RatePerHour = 300; // (€3/h)

        public PaymentService(IPaymentRepository paymentRepository, ISpotOccupationRepository spotOccupationRepository, IAccountRepository accountRepository, ICarRepository carRepository)
        {
            this._paymentRepository = paymentRepository;
            this._spotOccupationRepository = spotOccupationRepository;
            this._accountRepository = accountRepository;
            this._carRepository = carRepository;
            _molliePaymentClient = new PaymentClient("placeholder");
        }

        public async Task<Payment> ProcessPaymentAsync(int carId, int garageId, DateTime date, string redirectUrl)
        {
            // Get the latest SpotOccupation record for this car
            var spotOccupation = _spotOccupationRepository.GetLatestByCarId(carId);

            // Calculate total parking time in hours
            var parkingDuration = spotOccupation.ActualEndDate.Subtract(spotOccupation.ActualStartDate).TotalHours;

            // Calculate total fee in cents
            var totalFee = (int)(parkingDuration * RatePerHour);

            // Create internal payment record
            var payment = new Payment
            {
                CarId = carId,
                GarageId = garageId,
                Price = totalFee,
                Type = Enums.PaymentType.Standard,
                Date = date
            };

            _paymentRepository.Create(payment);

            // Create Mollie payment request
            var paymentRequest = new PaymentRequest
            {
                Amount = new Amount("EUR", totalFee / 100M), // Convert cents to euros
                Description = "Parking fee",
                RedirectUrl = redirectUrl,
                Method = PaymentMethod.CreditCard // or any other method you wish to support
            };

            var molliePaymentResponse = await _molliePaymentClient.CreatePaymentAsync(paymentRequest);

            // Here you might want to update your payment record with external payment details
            //payment.ExternalPaymentId = molliePaymentResponse.Id; // Link Mollie payment ID, Add new column for payment
            _paymentRepository.Update(payment);

            return payment;
        }

        public List<Payment> GetPaymentsByAuth0Id(string auth0Id)
        {
            Account account = this._accountRepository.GetAccountByAuth0Id(auth0Id);
            List<Car> cars = this._carRepository.GetAllByAccountId(account.Id);
            List<Payment> payments = this._paymentRepository.GetPaymentsByCarIds(cars.Select(car => car.Id).ToList<int>());
            return payments;
        }
    }
}
