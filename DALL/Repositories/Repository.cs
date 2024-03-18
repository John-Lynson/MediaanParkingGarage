using CORE.Entities;
using CORE.Interfaces.IRepositories;
using DALL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private GarageContext _context;
        private DbSet<T> _dbSet;

        public Repository(GarageContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public void Create(T entity)
        {
            this._dbSet.Add(entity);
            this._context.SaveChanges();
        }

        public void Delete(T entity)
        {
            this._dbSet.Remove(entity);
            this._context.SaveChanges();
        }

        public void Update(T entity)
        {
            this._dbSet.Update(entity);
            this._context.SaveChanges();
        }

        public T GetById(int id)
        {
            return this._dbSet.Where(x => x.Id == id).First();
        }
    }
}
