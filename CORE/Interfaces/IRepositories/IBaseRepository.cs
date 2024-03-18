using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        public void Add(T entity);
        public void Delete(T entity);
        public void Update(T entity);
        public T GetById(int id);
    }
}
