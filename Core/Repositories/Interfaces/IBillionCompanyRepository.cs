using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Interfaces
{
    public interface IBillionCompanyRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        DbSet<User> GetAllAsync();
        DbSet<Role> GetAllRolesAsync();
        TEntity GetByID(int id);
        void Insert(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
    }
}
