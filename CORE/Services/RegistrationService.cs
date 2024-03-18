using CORE.Interfaces;
using CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILicensePlateRepository _licensePlateRepository;

        public RegistrationService(ILicensePlateRepository licensePlateRepository)
        {
            _licensePlateRepository = licensePlateRepository ?? throw new ArgumentNullException(nameof(licensePlateRepository));
        }
        public void RegisterPlate(LicensePlate license)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));

            if (string.IsNullOrEmpty(license.LicensePlateNumber))
                throw new ArgumentException("License plate cannot be empty.", nameof(license));

            // Check if the license plate is already registered
            var existingLicense = _licensePlateRepository.GetLicensePlate(license.LicensePlateNumber);
            if (existingLicense != null)
            {
                // Update existing record, or throw an exception, depending on your requirements
                throw new InvalidOperationException("License plate already registered.");
            }
            else
            {
                // Set entry time for new registration
                license.EntryTime = DateTime.Now;
                _licensePlateRepository.AddLicensePlate(license);
            }
        }
    }
}
