using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(GarageContext context) : base(context) { }

        public List<Payment> GetPaymentsByCarIds(List<int> carIds)
        {
            List<Payment> payments = new List<Payment>();

            foreach (int carId in carIds)
            {
                payments.Concat(this.GetPaymentsByCarId(carId));
            }

            return payments.OrderByDescending(p => p.Date).ToList();
        }

        public List<Payment> GetPaymentsByCarId(int carId)
        {
            return this._dbSet.Where(c => c.CarId == carId).ToList();
        }
    }
}
