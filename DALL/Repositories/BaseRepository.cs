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
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private GarageContext _context;
        private DbSet<T> _dbSet;

        public BaseRepository(GarageContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public void Add(T entity)
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
