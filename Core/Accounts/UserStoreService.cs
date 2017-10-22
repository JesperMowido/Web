using Core.Entities;
using Core.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Accounts
{
    public class UserStoreService : IUserRoleStore<User, int>, IUserPasswordStore<User, int>, IUserLockoutStore<User, int>, IUserTwoFactorStore<User, int>
    {
        BillionCompanyDbContext _context;
        public UserStoreService(BillionCompanyDbContext context)
        {
            _context = context;
        }

        public System.Threading.Tasks.Task CreateAsync(User user)
        {
            _context.UserRepository.Insert(user);

            return _context.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<User> FindByIdAsync(int userId)
        {
            Task<User> task =
          _context.UserRepository.GetAllAsync().Where(u => u.Id == userId)
          .FirstOrDefaultAsync();

            return task;
        }

        public System.Threading.Tasks.Task<User> FindByNameAsync(string userName)
        {
            Task<User> task =
          _context.UserRepository.GetAllAsync().Where(u => u.UserName == userName)
          .FirstOrDefaultAsync();

            return task;
        }

        public System.Threading.Tasks.Task UpdateAsync(User user)
        {
            _context.UserRepository.Update(user);

            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }
        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.Password != null);
        }
        public Task SetPasswordHashAsync(
          User user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            var roleResult = _context.RoleRepository.GetAllRolesAsync().Where(r => r.Name.Equals(roleName)).FirstOrDefaultAsync();
            if (roleResult != null && roleResult.Result != null)
            {
                var role = roleResult.Result;
                user.Roles.Add(role);
            }
            else
            {
                var newrole = new Role() { Name = roleName };
                _context.RoleRepository.Insert(newrole);
                user.Roles.Add(newrole);
            }

            _context.UserRepository.Update(user);
            return _context.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            List<string> roles = new List<string>();

            var dbRoles = _context.UserRepository.GetAllAsync()
                .Where(u => u.UserName == user.UserName)
                .SelectMany(u => u.Roles)
                .Select(r => r.Name)
                .ToListAsync();

            if (dbRoles != null && dbRoles.Result.Count > 0)
            {
                return Task.FromResult(dbRoles.Result as IList<string>);
            }
            else
            {
                roles.Add("NewUser");
                return Task.FromResult(roles as IList<string>);

            }
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
