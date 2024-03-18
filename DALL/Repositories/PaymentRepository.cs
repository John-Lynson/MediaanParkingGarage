using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;

namespace DALL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(GarageContext context) : base(context) { }
    }
}
