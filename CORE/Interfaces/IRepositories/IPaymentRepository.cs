using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IRepositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        public List<Payment> GetPaymentsByCarIds(List<int> carIds);
        public List<Payment> GetPaymentsByCarId(int carId);
    }
}
