using CORE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IServices
{
    public interface IRegistrationService
    {
        void RegisterPlate(Car license);
    }
}
