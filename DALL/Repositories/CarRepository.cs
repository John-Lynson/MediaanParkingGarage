using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(GarageContext context) : base(context) { }

		public List<Car> GetAllByAccountId(int accountId)
		{
			return this._dbSet.Where(x => x.AccountId == accountId).ToList();
		}
	}
}
