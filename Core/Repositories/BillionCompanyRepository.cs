using Core.Entities;
using Core.Infrastructure;
using Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class BillionCompanyRepository<TEntity> : IBillionCompanyRepository<TEntity> where TEntity : class
    {
        internal BillionCompanyDbContext _context;
        internal DbSet<TEntity> _dbSet;

        public BillionCompanyRepository(BillionCompanyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public IEnumerable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _dbSet;
            return query;
        }

        public DbSet<User> GetAllAsync()
        {
            return _context.Set<User>();
        }

        public DbSet<Role> GetAllRolesAsync()
        {
            return _context.Set<Role>();
        }

        public TEntity GetByID(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
