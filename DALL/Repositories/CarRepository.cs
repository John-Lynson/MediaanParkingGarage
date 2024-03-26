using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;
using Microsoft.EntityFrameworkCore;

namespace DALL.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(GarageContext context) : base(context) { }

        public Car FindByAccountIdAndLicensePlate(int accountId, string licensePlate)
        {
            return this._dbSet
                       .Where(car => car.AccountId == accountId && car.LicensePlate == licensePlate)
                       .FirstOrDefault(); 
        }
    }
}
