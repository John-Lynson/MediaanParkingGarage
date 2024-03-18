using CORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces
{
    public interface IRegistrationService
    {
        void RegisterPlate(LicensePlate license);
    }
}
