using CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces
{
    public interface ILicensePlateRepository
    {
        LicensePlate GetLicensePlate(string licensePlateNumber);
        void AddLicensePlate(LicensePlate license);
    }
}
