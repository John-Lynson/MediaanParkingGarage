using CORE.Entities;
using CORE.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class TariffService
    {
        private readonly ITariffRepository _tariffRepo;

        public TariffService(ITariffRepository tariffRepo)
        {
            this._tariffRepo = tariffRepo;
        }

        public void CreateTariff(int garageId, int rate, DateTime startDate, DateTime endDate)
        {
            Tariff newTariff = new Tariff
            {
                GarageId = garageId,
                Rate = rate,
                StartDate = startDate,
                EndDate = endDate
            };
            this._tariffRepo.Create(newTariff);
        }

        public List<Tariff> GetTariffsByInterval(DateTime startDate, DateTime endDate)
        {
            return this._tariffRepo.GetTariffsByInterval(startDate, endDate);
        }
    }
}
